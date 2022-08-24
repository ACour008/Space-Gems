using System;
using UnityEngine;
using MiiskoWiiyaas.Audio;


namespace MiiskoWiiyaas.Grid
{
    [System.Serializable]
    public class GameGrid<T> where T: IClickable
    {
        private int rows;
        private int cols;
        private float cellSize;
        private T[] cells;
        private IBuilder<T, T> cellBuilder;
        private Vector3 startingPosition;

        public T[] Cells { get => cells; }
        public int Rows { get => rows; }
        public int Cols { get => cols; }
        public Vector3 StartingPosition { get => startingPosition; }

        public GameGrid(int rows, int cols, float cellSize, IBuilder<T, T> cellBuilder)
        {
            this.rows = rows;
            this.cols = cols;
            this.cellSize = cellSize;
            this.cellBuilder = cellBuilder;
            cells = new T[rows * cols];
        }

        public void Build(Vector3 startPosition, Transform parent, InputHandler input, GridSFXPlayer sfxPlayer)
        {
            startingPosition = startPosition;
            
            float xPos = startPosition.x;
            float yPos = startPosition.y;
            int xCounter = 0;

            for(int i = 0; i < cells.Length; i++)
            {
                T cell = cellBuilder.Build(i, xPos, yPos, parent, cells);

                cell.OnClicked += input.GemCell_OnClicked;
                cell.OnClicked += sfxPlayer.GemCell_OnClicked;

                cells[i] = cell;

                xPos += cellSize;
                xCounter++;

                if (xCounter >= rows)
                {
                    yPos += cellSize;
                    xPos = startPosition.x;
                    xCounter = 0;
                }
            }
        }

        public void InputHandler_OnTilesSelected(object sender, EventArgs eventArgs)
        {
            SetCellsAsSelectable(false);
        }

        public void InputHandler_OnTilesDeselected(object sender, EventArgs eventArgs)
        {
            SetCellsAsSelectable(true);
        }

        // This should be elsewhere
        public void SetCellsAsSelectable(bool selectable)
        {
            foreach(T cell in cells)
            {
                cell.CanClick = selectable;
            }
        }
    }
}
