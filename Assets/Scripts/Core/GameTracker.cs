using UnityEngine;
using MiiskoWiiyaas.Grid;
using MiiskoWiiyaas.Scoring;

namespace MiiskoWiiyaas.Core
{
    public class GameTracker: MonoBehaviour
    {
        private float score;
        private GemColor[] gridLayout;

        private GameGrid<GemCell> grid;
        private GridRetiler retiler;
        private GridClearer clearer;
        private ScoreManager scoreManager;

        public GemColor[] GridLayout { get => gridLayout; }
        public float Score { get => score; }

        public void Initialize(GameGrid<GemCell> grid, GridRetiler retiler, GridClearer clearer, ScoreManager scoreManager)
        {
            this.grid = grid;
            this.retiler = retiler;
            this.scoreManager = scoreManager;
            this.clearer = clearer;

            this.score = scoreManager.CurrentScore;
            gridLayout = new GemColor[grid.Cells.Length];

            UpdateData();
        }

        public void RevertToLevelStart()
        {
            scoreManager.CurrentScore = score;
            RevertGrid();
        }

        private void RevertGrid()
        {
            clearer.Clear();
            retiler.RefillGridWithLayout(gridLayout, 1f);

        }

        public void UpdateData()
        {
            score = scoreManager.CurrentScore;
            UpdateGridLayout();
        }

        private void UpdateGridLayout()
        {
            gridLayout = new GemColor[grid.Cells.Length];

            for (int i = 0; i < grid.Cells.Length; i++)
            {
                gridLayout[i] = grid.Cells[i].CurrentGem.Color;
            }
        }

    }
}
