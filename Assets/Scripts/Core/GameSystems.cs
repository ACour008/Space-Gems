using UnityEngine;
using MiiskoWiiyaas.Grid;
using MiiskoWiiyaas.UI;
using MiiskoWiiyaas.Audio;
using MiiskoWiiyaas.Scoring;

namespace MiiskoWiiyaas.Core
{
    [System.Serializable]
    public class GameSystems
    {
        [SerializeField] private GemCellUISelector uiSelector;
        [SerializeField] private GemMover gemMover;
        [SerializeField] private MatchFinder matchFinder;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private GridRetiler gridRetiler;
        [SerializeField] private GridClearer gridClearer;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private GameTracker gameTracker;
        [SerializeField] private GridShuffler shuffler;
        [SerializeField] private MatchChecker matchChecker;
        [SerializeField] private GridSFXPlayer sfxPlayer;
        [SerializeField] private AudioManager audioManager;

        private GameGrid<GemCell> grid;

        public InputHandler InputHandler { get => inputHandler; }
        public GridSFXPlayer SFXPlayer { get => sfxPlayer; }

        /// <summary>
        /// Sets up the game grid, initializes all systems associated with the grid
        /// and registers all grid events.
        /// </summary>
        /// <param name="grid">The main game grid that holds all the gems.</param>
        /// <param name="gridCellPrefab">The prefab for game grid cells.</param>
        public void Initialize(GameGrid<GemCell> grid, GameObject gridCellPrefab)
        {
            this.grid = grid;
            InitializeAllSystems(this.grid, gridCellPrefab);
            RegisterEvents();
        }

        private void InitializeAllSystems(GameGrid<GemCell> grid, GameObject gridItemPrefab)
        {
            matchFinder.Initialize(grid);
            matchChecker.Initialize(grid);
            gridRetiler.Initialize(grid, gridItemPrefab);
            gemMover.Initialize(grid, gridRetiler);
            gridClearer.Initialize(grid);
            scoreManager.Initialize(grid);
            inputHandler.Initialize(matchFinder, matchChecker, gemMover, uiSelector);
            shuffler.Initialize(grid, matchChecker);
            gameTracker.Initialize(grid, gridRetiler, gridClearer, scoreManager);
            levelManager.Initialize(grid, gridClearer, gridRetiler, gameTracker);
        }

        private void RegisterEvents()
        {
            matchFinder.OnMatchProcessed += gemMover.MatchFinder_OnMatchMade;
            matchFinder.OnMatchProcessed += scoreManager.MatchFinder_OnMatchMade;
            matchFinder.OnPlayMatchSFX += sfxPlayer.MatchFinder_OnPlayMatchSFX;
            matchFinder.OnMatchFound += scoreManager.MatchFinder_OnSequenceDone;

            gemMover.OnSwap += sfxPlayer.GemMover_OnSwap;
            gemMover.OnGemMoved += sfxPlayer.GemMover_OnGemMoved;

            gridRetiler.OnGridRetiled += inputHandler.GridRetiler_OnGridRetiled;
            gridRetiler.OnGemMovedToPosition += sfxPlayer.GridRetiler_OnGemMovedToPosition;

            inputHandler.OnMatchFindingDone += scoreManager.InputHandler_OnMatchFindingDone;
            inputHandler.OnMatchFindingDone += levelManager.InputHandler_OnMatchFindingDone;
            inputHandler.OnTilesDeselected += grid.InputHandler_OnTilesDeselected;
            inputHandler.OnTilesSelected += grid.InputHandler_OnTilesSelected;

            levelManager.OnLevelChangeCompleted += scoreManager.LevelManager_OnChangedToFirstLevel;
            levelManager.OnLevelChangeCompleted += audioManager.LevelManager_OnLevelChangeCompleted;
            levelManager.OnLevelChange += audioManager.LevelManager_OnLevelChange;
        }
    }
}
