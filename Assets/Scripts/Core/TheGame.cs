using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiskoWiiyaas.Grid;
using MiskoWiiyaas.Enums;
using MiskoWiiyaas.Cells;
using MiskoWiiyaas.Tiles;
using MiskoWiiyaas.UI;
using MiskoWiiyaas.Interfaces;

namespace MiskoWiiyaas.Core
{
    public class TheGame : MonoBehaviour
    {

        [Header("Game Settings")]
        [SerializeField] private GameDifficulty difficulty;
        [SerializeField] private int initialScoreTargetOnEasy;
        [SerializeField] private int initialScoreTargetOnHard;
        [SerializeField] private float targetScoreMultiplier;
        [SerializeField] private float scoreMultiplierIncrease;
        [SerializeField] private string levelStartText;
        [SerializeField] private string levelCompleteText;
        [SerializeField] private string gameOverText;


        [Header("Systems")]
        [SerializeField] private GameObject gridObject;
        [SerializeField] private Spawner tileSpawner;
        [SerializeField] private LevelKeeper levelKeeper;
        [SerializeField] private ScoreKeeper scoreKeeper;
        [SerializeField] private GameTextUI gameTextUI;
        // [SerializeField] private FadeTransition levelTransitioner;
        [SerializeField] private GameObject pauseMenuObject;

        [Header("Tile Probabilities")]
        [SerializeField, Range(0, 1)] private float retileProbabiltyOnEasy;
        [SerializeField, Range(0, 1)] private float retileProbabilityOnHard;
        [SerializeField, Range(0, 1)] private float easyDropTileProbability;
        [SerializeField, Range(0, 1)] private float hardDropTileProbability;
        private float retileProbability;
        private float dropTileProbability;
        private int initialScore;

        private GridMaker gridMaker;
        private GameGrid gameGrid;
        private Shuffler shuffler;
        private IMenu pauseMenu;

        private void Awake()
        {
            gridMaker = gridObject.GetComponent<GridMaker>();
            gameGrid = gridObject.GetComponent<GameGrid>();
            shuffler = GetComponent<Shuffler>();

            scoreKeeper.OnScoreTargetMet += ScoreKeeper_OnScoreTargetMet;

            switch (difficulty)
            {
                case GameDifficulty.NORMAL:
                    retileProbability = retileProbabiltyOnEasy;
                    dropTileProbability = easyDropTileProbability;
                    initialScore = initialScoreTargetOnEasy;
                    break;
                case GameDifficulty.HARDER:
                    retileProbability = retileProbabilityOnHard;
                    dropTileProbability = hardDropTileProbability;
                    initialScore = initialScoreTargetOnHard;
                    break;
                default:
                    retileProbability = retileProbabiltyOnEasy;
                    dropTileProbability = easyDropTileProbability;
                    initialScore = initialScoreTargetOnEasy;
                    break;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            GridCell[,] gridCells = gridMaker.Initialize(gameGrid.Rows, gameGrid.Cols, gameGrid.CellAlpha, retileProbability);
            tileSpawner.Init(gameGrid.Rows, gridMaker.TilePrefab);
            gameGrid.Initialize(difficulty, gridCells, tileSpawner, dropTileProbability);
            scoreKeeper.Initialize(initialScore, targetScoreMultiplier, scoreMultiplierIncrease);

            gameGrid.OnNoMovesFound += GameGrid_OnNoMovesFound;
            gameGrid.Matchmaker.OnMatchMade += scoreKeeper.GameGrid_OnUpdateScore;
            gameGrid.OnChecksDone += scoreKeeper.GameGrid_OnChecksDone;

            pauseMenu = pauseMenuObject.GetComponent<IMenu>();
            
            shuffler.Initialize(gameGrid.Cells);

            StartCoroutine(StartLevel());
        }

        private void GameGrid_OnNoMovesFound(object sender, System.EventArgs e)
        {
            GameOver();
        }

        private void ScoreKeeper_OnScoreTargetMet(object sender, Events.ScoreEventArgs e)
        {
            gameGrid.LevelDone = true;
            StartCoroutine(DoLevelWrapUp());
        }

        private IEnumerator DoLevelWrapUp()
        {
            yield return new WaitForSeconds(0.5f);

            StartCoroutine(gameTextUI.FadeInText(levelCompleteText));

            while (gameTextUI.IsFading || !gameGrid.LevelDoneAnimsComplete)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
            // levelTransitioner.DoOut();
            
            /*while (levelTransitioner.IsTransitioning)
            {
                yield return null;
            }*/

            gameTextUI.SetActive(false);
            scoreKeeper.SetProgressBar();

            GoToNextLevel();
        }

        private IEnumerator StartLevel()
        {
            // levelTransitioner.DoIn();

/*            yield return new WaitUntil(() =>
            {
                new WaitForSeconds(0.5f);
                return !levelTransitioner.IsTransitioning;
            });*/

            gameTextUI.ShowText(levelStartText);

            yield return new WaitForSeconds(1f);
            StartCoroutine(gameTextUI.FadeOutText());

            gameGrid.EnableCellEvents();
        }

        private void GoToNextLevel()
        {
            gameGrid.DisableCellEvents();

            levelKeeper.IncreaseLevel();
            gameGrid.DestroyAllTiles();
            gridMaker.FillCellsWithTiles(gameGrid.Cells, gameGrid.Rows, gameGrid.Cols, gameGrid.registerTileEvents);
            gameGrid.LevelDone = false;



            StartCoroutine(StartLevel());
        }

        private void GameOver()
        {
            StopAllCoroutines();
            StartCoroutine(gameTextUI.FadeInText(gameOverText));
        }

        public void ShuffleTiles()
        {
            shuffler.Shuffle();
            shuffler.MoveShuffledTiles(gameGrid.Cells);
            /*do
            {
                shuffler.Shuffle();
            }
            while (!gameGrid.Matchmaker.HasPotentialMatches(shuffler.GetShuffled()));

            shuffler.MoveShuffledTiles(gameGrid.Cells);*/
        }

        public void Pause()
        {
            gameGrid.DisableCellEvents();
            pauseMenu.Open();
        }

        public void Unpause()
        {
            gameGrid.EnableCellEvents();
            pauseMenu.Close();
        }
    }

}