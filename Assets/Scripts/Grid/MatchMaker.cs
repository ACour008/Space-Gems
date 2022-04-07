using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiskoWiiyaas.Tiles;
using MiskoWiiyaas.Cells;
using MiskoWiiyaas.Events;

namespace MiskoWiiyaas.Grid
{
    [RequireComponent(typeof(GameGrid))]
    public class MatchMaker : MonoBehaviour
    {
        private GameGrid gameGrid;
        private HashSet<Tile> matches;
        private int numMatches;
        private int matchCount;
        private bool isRunningMatchCheck;

        public event EventHandler<ScoreEventArgs> OnMatchMade;

        public HashSet<Tile> Matches { get => matches; }
        public int NumMatches { get => numMatches; }
        public int MatchCount { get => matchCount; }
        public bool IsRunningMatchCheck { get => isRunningMatchCheck; }


        private void Awake()
        {
            gameGrid = GetComponent<GameGrid>();
            matches = new HashSet<Tile>();
            isRunningMatchCheck = false;
        }

        /// <summary>
        /// Finds all horizontal and vertical matches of 3 or more tiles.
        /// </summary>
        /// <returns>A boolean based on if the matches HashSet has more than one tile in it.</returns>
        public bool FindMatches()
        {
            int x = 0;
            int y = 0;
            int rows = gameGrid.Rows;
            int cols = gameGrid.Cols;
            GridCell[,] cells = gameGrid.Cells;

            for (int i = 0; i < rows * cols; i++)
            {
                x = i % rows;
                y = i / rows;

                Tile current = Helpers.GetTileAt(cells, x, y);

                if (matches.Contains(current)) { continue; }

                List<Tile> rowMatches = FindRowMatches(x, y, current);
                List<Tile> colMatches = FindColMatches(x, y, current);

                if (rowMatches.Count >= 3)
                {
                    matches.Add(current);
                    matches.UnionWith(rowMatches);

                    GridCell scoreCell = Helpers.GetCellById(cells, rowMatches[rowMatches.Count / 2].currentCellId);
                    OnMatchMade?.Invoke(this, new ScoreEventArgs { matches = rowMatches, cell = scoreCell });
                }

                if (colMatches.Count >= 3)
                {
                    matches.Add(current);
                    matches.UnionWith(colMatches);

                    GridCell scoreCell = Helpers.GetCellById(cells, colMatches[colMatches.Count / 2].currentCellId);
                    OnMatchMade?.Invoke(this, new ScoreEventArgs { matches = colMatches, cell = scoreCell });
                }
            }

            return matches.Count > 0;
        }

        /// <summary>
        /// Finds a horizontal match of 3 or more tiles starting (x,y).
        /// </summary>
        /// <param name="x">Int: Current x coordinate</param>
        /// <param name="y">Int: Current y coordinate</param>
        /// <param name="current">The current <cref>Tile</cref>to be compared to for any matches.</param>
        /// <returns></returns>
        private List<Tile> FindRowMatches(int x, int y, Tile current, bool debug = false)
        {
            List<Tile> results = new List<Tile>();
            results.Add(current);

            if (current != null)
            {
                for (int i = x + 1; i < gameGrid.Rows; i++)
                {
                    Tile next = Helpers.GetTileAt(gameGrid.Cells, i, y);
                    if (next == null) { continue; }

                    if (debug)
                    {
                        Debug.Log($"{next}, {current}");
                    }

                    if (next.tileType != current.tileType)
                    {
                        break;
                    }

                    results.Add(next);
                }
            }
            return results;
        }

        /// <summary>
        /// Finds a vertical match of 3 or more tiles starting at (x, y).
        /// </summary>
        /// <param name="x">Int: Current x coordinate</param>
        /// <param name="y">Int: Current y coordinate</param>
        /// <param name="current">The current <cref>Tile</cref>to be compared to for any matches.</param>
        /// <returns></returns>
        private List<Tile> FindColMatches(int x, int y, Tile current)
        {
            List<Tile> results = new List<Tile>();
            results.Add(current);

            if (current != null)
            {
                for (int i = y + 1; i < gameGrid.Cols; i++)
                {
                    Tile next = Helpers.GetTileAt(gameGrid.Cells, x, i);
                    if (next == null) { continue; }

                    if (next.tileType != current.tileType)
                    {
                        break;
                    }
                    results.Add(next);
                }
            }

            return results;
        }

        /// <summary>
        /// Finds all potential matches in the grid without carrying out any animations.
        /// </summary>
        /// <returns>Boolean: true if there are matches, if not, false.</returns>
        public bool HasPotentialMatches(GridCell[,] cells)
        {
            int x;
            int y;
            int rows = gameGrid.Rows;
            int cols = gameGrid.Cols;
            

            for (int i = 0; i < rows * cols; i++)
            {
                x = i % rows;
                y = i / rows;

                Tile current = cells[x, y].currentTile;

                List<Tile> rowMatches = FindRowMatches(x, y, current);
                List<Tile> colMatches = FindColMatches(x, y, current);

                if (rowMatches.Count >= 3 || colMatches.Count >= 3)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// A function that checks if potential matches exist.
        /// </summary>
        /// <returns>Boolean: true if potential matches exist, false if not.</returns>
        public bool CheckForPotentialMatches()
        {
            // for testing gameover sequences
            // return false;

            GridCell[,] cells = gameGrid.Cells;
            int rows = gameGrid.Rows;
            int cols = gameGrid.Cols;
            int x = 0;
            int y = 0;
            bool hasMatch = false;
            numMatches = 0;

            for (int i = 0; i < rows * cols; i++)
            {
                x = i % rows;
                y = i / rows;

                GridCell current = Helpers.GetCell(cells, x, y);
                GridCell up = Helpers.GetCell(cells, x, y + 1);
                GridCell left = Helpers.GetCell(cells, x - 1, y);

                if (MatchesFound(current, up) || MatchesFound(current, left))
                {
                    hasMatch = true;
                }
            }

            return hasMatch;
        }

        /// <summary>
        /// A helper function that swaps two tiles and looks for matches within the grid.
        /// </summary>
        /// <param name="current">The first GridCell to be swapped for match checking.</param>
        /// <param name="next">The second GridCell to be swapped for match checking.</param>
        /// <returns>Boolean: true if there is a match, if not, false.</returns>
        private bool MatchesFound(GridCell current, GridCell next)
        {
            bool result = false;

            if (next)
            {
                Tile temp = current.currentTile;
                current.currentTile = next.currentTile;
                next.currentTile = temp;

                if (HasPotentialMatches(gameGrid.Cells))
                {
                    numMatches++;
                    result = true;
                }

                temp = current.currentTile;
                current.currentTile = next.currentTile;
                next.currentTile = temp;
            }

            return result;
        }

        /// <summary>
        /// Destroys each tile in the matches Hashset and then clears the hashset.
        /// </summary>
        public void ClearAllMatches()
        {
            isRunningMatchCheck = true;
            matchCount = matches.Count;

            foreach (Tile tile in matches)
            {
                GridCell cell = Helpers.GetCellById(gameGrid.Cells, tile.currentCellId);
                cell?.DestroyTile();
            }
            matches.Clear();
        }

        public void ResetMatchCount()
        {
            matchCount = 0;
        }

        public void SetRunningMatchCheck(bool value)
        {
            isRunningMatchCheck = value;
        }
    }
}