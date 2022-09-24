using UnityEngine;
using MiiskoWiiyaas.Grid;
using MiiskoWiiyaas.Scoring;

namespace MiiskoWiiyaas.Core
{
    /// <summary>
    /// Keeps track of the game grid layout.
    /// </summary>
    public class GameTracker: MonoBehaviour
    {
        private float cachedScore;
        private GemColor[] cachedGridLayout;

        private GameGrid<GemCell> grid;
        private GridRetiler retiler;
        private GridClearer clearer;
        private ScoreManager scoreManager;

        public GemColor[] GridLayout { get => cachedGridLayout; }
        public float Score { get => cachedScore; }

        /// <summary>
        /// Initializes the object.
        /// </summary>
        /// <param name="grid">The main game grid object.</param>
        /// <param name="retiler">The object responsible for retiling the grid after it has been cleared.</param>
        /// <param name="clearer">The object responsibile for clearing the grid.</param>
        /// <param name="scoreManager">The score manager object.</param>
        public void Initialize(GameGrid<GemCell> grid, GridRetiler retiler, GridClearer clearer, ScoreManager scoreManager)
        {
            this.grid = grid;
            this.retiler = retiler;
            this.scoreManager = scoreManager;
            this.clearer = clearer;

            this.cachedScore = scoreManager.CurrentScore;
            cachedGridLayout = new GemColor[grid.Cells.Length];

            CacheData();
        }

        /// <summary>
        /// Reverts the grid and the score of the game to what it was at the begining of the level.
        /// </summary>
        public void RevertToLevelStart()
        {
            scoreManager.CurrentScore = cachedScore;
            RevertGrid();
        }

        private void RevertGrid()
        {
            clearer.Clear();
            retiler.RefillGridWithLayout(cachedGridLayout, 1f);

        }

        /// <summary>
        /// Caches the current grid layout and score of the game.
        /// </summary>
        public void CacheData()
        {
            cachedScore = scoreManager.CurrentScore;
            UpdateGridLayout();
        }

        private void UpdateGridLayout()
        {
            cachedGridLayout = new GemColor[grid.Cells.Length];

            for (int i = 0; i < grid.Cells.Length; i++)
            {
                cachedGridLayout[i] = grid.Cells[i].CurrentGem.Color;
            }
        }

    }
}
