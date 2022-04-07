using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiskoWiiyaas.Cells;
using MiskoWiiyaas.Events;
using MiskoWiiyaas.Tiles;
using MiskoWiiyaas.Enums;

namespace MiskoWiiyaas.Grid
{
    public class GameGrid : MonoBehaviour
    {
        [Header("Grid Info")]
        [SerializeField] private int rows;
        [SerializeField] private int cols;
        [SerializeField] [Range(0, 1)] private float cellAlpha;
        [SerializeField] private GridCell[] selectedCells;
        [SerializeField] private SpawnerCell levelDoneTarget;

        private Spawner tileSpawner;
        private MatchMaker matchMaker;

        private GridCell[,] cells;
        private HashSet<Tile> matches;

        private bool levelDone = false;
        private bool levelDoneAnimsComplete = false;
        private int tileDestroyedCount = 0;
        private int moveCoroutinesRunning = 0;
        private float dropTileProbability;

        public event EventHandler<GameGridEventArgs> OnNoMovesFound;
        public event EventHandler<ScoreEventArgs> OnChecksDone;
        public Action<Tile> registerTileEvents;

        public int Rows { get => rows; }
        public int Cols { get => cols; }
        public GridCell[,] Cells { get => cells; }
        public float CellAlpha { get => cellAlpha; }
        public bool LevelDone { get => levelDone; set => levelDone = value; }
        public bool LevelDoneAnimsComplete { get => levelDoneAnimsComplete; }
        public MatchMaker Matchmaker { get => matchMaker; }

        #region Init
        private void PreInit()
        {
            selectedCells = new GridCell[2];
            registerTileEvents = RegisterTileEvents;
            matchMaker = GetComponent<MatchMaker>();
        }

        public void Initialize(GameDifficulty difficulty, GridCell[,] gridCells, Spawner spawner, float dropTileProb)
        {
            PreInit();

            cells = gridCells;
            tileSpawner = spawner;
            dropTileProbability = dropTileProb;

            matchMaker.CheckForPotentialMatches();
        }
        #endregion

        #region Events
        /// <summary>
        /// A event listener for when a tile is destroyed
        /// </summary>
        /// <param name="sender">The tile being destroyed</param>
        /// <param name="e">An empty CellEventArgs</param>
        private void Tile_OnDestroyedComplete(object sender, CellEventArgs e)
        {
            tileDestroyedCount++;
        }

        /// <summary>
        /// An event listener that listens for when a tile completes its move animations.
        /// </summary>
        /// <param name="sender">The tile doing its move animation.</param>
        /// <param name="e">An empty CellEventArgs</param>
        private void Tile_OnMoveComplete(object sender, CellEventArgs e)
        {
            moveCoroutinesRunning--;
        }

        /// <summary>
        /// A subscriber to a event that is fired every time player clicks a cell.
        /// </summary>
        /// <param name="sender">The cell object</param>
        /// <param name="e">CellEventArgs object</param>
        private void Cell_OnClick(object sender, CellEventArgs e)
        {
            bool noTileSelected = (selectedCells[0] == null);
            bool currentTileAreadySelected = (selectedCells[0] == e.cell);

            if (noTileSelected)
            {
                selectedCells[0] = e.cell;
            }
            else
            {
                if (currentTileAreadySelected)
                {
                    ClearSelectedCells();
                    return;
                }

                if (selectedCells[0].IsNeighbor(e.cell))
                {
                    GridCell.selected = null;
                    selectedCells[1] = e.cell;
                    StartCoroutine(DoChecks());
                }
                else
                {
                    selectedCells[0] = e.cell;
                    selectedCells[1] = null;
                }
            }
        }

        /// <summary>
        /// Subscribes <cref>GameGrid</cref> to <cref>Cell</cref> and its associated <cref>Tile</cref> events
        /// </summary>
        /// <param name="cell">The cell that the <cref>GameGrid</cref> will subscribe to.</param>
        private void RegisterEventsFor(GridCell cell)
        {
            cell.OnClick += Cell_OnClick;
            RegisterTileEvents(cell.currentTile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        private void UnregisterEventsFor(GridCell cell)
        {
            cell.OnClick -= Cell_OnClick;
            UnregisterTileEvents(cell.currentTile);
        }

        /// <summary>
        /// Subscrbes <cref>GameGrid</cref> to the events of a <cref>Tile</cref>.
        /// </summary>
        /// <param name="tile">The <cref>Tile</cref> that the <cref>GameGrid</cref> will subscribe to.</param>
        private void RegisterTileEvents(Tile tile)
        {
            tile.OnTileDestroyed += Tile_OnDestroyedComplete;
            tile.OnTileMoveDone += Tile_OnMoveComplete;
        }

        private void UnregisterTileEvents(Tile tile)
        {
            if (tile != null)
            {
                tile.OnTileDestroyed -= Tile_OnDestroyedComplete;
                tile.OnTileMoveDone -= Tile_OnMoveComplete;
            }
        }

        #endregion

        #region Main
        /// <summary>
        /// The main loop of the game where the player selects one or more cells. If two cells
        /// are selected and are neighbors, it swaps the tiles and checks for matches while
        /// matches are found. To be used as a coroutine.
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator DoChecks()
        {
            if (selectedCells[0].currentTile == null || selectedCells[1].currentTile == null)
            {
                ClearSelectedCells();
                yield break;
            }

            yield return StartCoroutine(SwapTiles());

            bool matchesFound = matchMaker.FindMatches();
            if (matchesFound)
            {
                do
                {
                    matchMaker.ClearAllMatches();
                    StartCoroutine(DropTilesAndRefill());
                    yield return new WaitUntil(() => !matchMaker.IsRunningMatchCheck);
                }
                while (matchMaker.FindMatches());

                if (!matchMaker.CheckForPotentialMatches())
                {
                    OnNoMovesFound?.Invoke(this, new GameGridEventArgs());
                }

                if (levelDone)
                {
                    StartCoroutine(DoLevelCompleteAnims());
                }


                OnChecksDone?.Invoke(this, new ScoreEventArgs());
            }
            else
            {
                SwapBack();
            }

            ClearSelectedCells();
        }
        #endregion

        #region Helpers
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator DropTilesAndRefill()
        {

            while (tileDestroyedCount < matchMaker.MatchCount)
            {
                yield return null;
            }

            for (int idx = 0; idx < rows * cols; idx++)
            {
                int x = idx % rows;
                int y = idx / rows;
                GridCell cell = cells[x, y];

                if (cell.currentTile == null)
                {
                    for (int i = y + 1; i < rows; i++)
                    {
                        Tile next = Helpers.GetTileAt(cells, x, i);

                        if (next == null)
                        {
                            continue;
                        }
                        else
                        {
                            Helpers.GetCellById(cells, next.currentCellId).currentTile = null;
                            StartCoroutine(next.MoveTo(cell, true));
                            moveCoroutinesRunning++;
                            break;
                        }
                    }
                }
            }
            matchMaker.ResetMatchCount();
            tileDestroyedCount = 0;
            StartCoroutine(RefillGrid());
        }

        /// <summary>
        /// Adds a new tile to any empty cells by calling on the Spawner object to create a new tile,
        /// then moves it to the empty cell. To be used in a coroutine.
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator RefillGrid()
        {
            while (moveCoroutinesRunning > 0)
            {
                yield return null;
            }

            moveCoroutinesRunning = 0;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    GridCell cell = cells[x, y];

                    if (cell.currentTile == null)
                    {
                        List<TileType> availableTypes = GetAvailableTypes(x, y);
                        Tile newTile = tileSpawner.SpawnTileAtCol(x, availableTypes);
                        RegisterTileEvents(newTile);

                        StartCoroutine(newTile.MoveTo(cell, true));
                        moveCoroutinesRunning++;
                    }
                }
            }

            while (moveCoroutinesRunning > 0)
            {
                yield return null;
            }

            moveCoroutinesRunning = 0;
            matchMaker.SetRunningMatchCheck(false);
        }

        /// <summary>
        /// Swaps the tiles of the selected cells chosen by the Player. Used as a coroutine.
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator SwapTiles()
        {
            StartCoroutine(selectedCells[0].currentTile.MoveTo(selectedCells[1], false));
            StartCoroutine(selectedCells[1].currentTile.MoveTo(selectedCells[0], false));

            while (selectedCells[0].currentTile.IsMoving && selectedCells[0].currentTile.IsMoving)
            {
                yield return null;
            }
        }

        /// <summary>
        /// Moves the tiles that were previously swapped by the player back to their original position.
        /// </summary>
        private void SwapBack()
        {
            StartCoroutine(selectedCells[0].currentTile.MoveTo(selectedCells[1], false));
            StartCoroutine(selectedCells[1].currentTile.MoveTo(selectedCells[0], false));
        }

        /// <summary>
        /// Clears the Selected Cells Array.
        /// </summary>
        private void ClearSelectedCells()
        {
            Array.Clear(selectedCells, 0, selectedCells.Length);
        }

        /// <summary>
        /// Checks the neighbors at Cell (x,y) and tests the probability of tile type removal based on the game's difficulty level.
        /// </summary>
        /// <param name="x">X coordinate of the cell</param>
        /// <param name="y">Y coordinate of the cell</param>
        /// <returns>A List of TileTypes</returns>
        private List<TileType> GetAvailableTypes(int x, int y)
        {
            List<TileType> types = new List<TileType>((TileType[])Enum.GetValues(typeof(TileType)));

            GridCell east = Helpers.GetCell(cells, x + 1, y);
            GridCell west = Helpers.GetCell(cells, x - 1, y);
            GridCell north = Helpers.GetCell(cells, x, y + 1);
            GridCell south = Helpers.GetCell(cells, x, y - 1);

            if (east.currentTile)
            {
                TestProbabilityOfRemoval(types, east.currentTile.tileType);
            }

            if (west.currentTile)
            {
                TestProbabilityOfRemoval(types, west.currentTile.tileType);
            }

            if (north.currentTile)
            {
                TestProbabilityOfRemoval(types, north.currentTile.tileType);
            }

            if (south.currentTile)
            {
                TestProbabilityOfRemoval(types, south.currentTile.tileType);
            }

            return types;
        }

        private void TestProbabilityOfRemoval(List<TileType> tileTypes, TileType currentType)
        {
            float prob = UnityEngine.Random.Range(0f, 1f);
            if (dropTileProbability > prob)
            {
                tileTypes.Remove(currentType);
            }
        }

        private IEnumerator DoLevelCompleteAnims()
        {
            yield return new WaitUntil(() => !matchMaker.IsRunningMatchCheck);

            levelDoneAnimsComplete = false;
            int x;
            int y;

            for (int i = 0; i < rows * cols; i++)
            {
                x = i % rows;
                y = i / rows;

                Tile tile = Helpers.GetTileAt(cells, x, y);
                StartCoroutine(tile.MoveTo(levelDoneTarget, false));
                cells[x, y].currentTile = null;
                yield return null;
            }

            levelDoneAnimsComplete = true;
        }

        public void DestroyAllTiles()
        {
            foreach(GridCell cell in cells)
            {
                if (cell.currentTile != null)
                {
                    cell.DestroyTile();
                }
            }
        }

        public void EnableCellEvents()
        {
            foreach (GridCell cell in cells)
            {
                RegisterEventsFor(cell);
                cell.IsActive = true;
            }
        }

        public void DisableCellEvents()
        {
            foreach (GridCell cell in cells)
            {
                UnregisterEventsFor(cell);
                cell.IsActive = false;
            }
        }

        #endregion
    }
}
