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

        /// <summary>
        /// The Constructor for the GameGrid object
        /// </summary>
        /// <param name="rows">Number of rows for the game grid.</param>
        /// <param name="cols">Number of columns for the game grid.</param>
        /// <param name="cellSize">Size of each cell in the grid.</param>
        /// <param name="cellBuilder">Any object implementing IBuilder that is responsible for creating cells for the grid.</param>
        public GameGrid(int rows, int cols, float cellSize, IBuilder<T, T> cellBuilder)
        {
            this.rows = rows;
            this.cols = cols;
            this.cellSize = cellSize;
            this.cellBuilder = cellBuilder;
            cells = new T[rows * cols];
        }

        /// <summary>
        /// Builds the grid.
        /// </summary>
        /// <param name="startPosition">A Vector3 representing where the grid will be placed.</param>
        /// <param name="parent">The transform object that the game grid will become a child of.</param>
        /// <param name="input">The InputHandler object responsible for dealing with player input.</param>
        /// <param name="sfxPlayer">The GridSFXPlayer object responsibile for sfx associated with the grid.</param>
        /// <seealso cref="InputHandler"/>
        /// <seealso cref="GridSFXPlayer"/>
        public void Build(Vector3 startPosition, Transform parent, InputHandler input, GridSFXPlayer sfxPlayer)
        {
            startingPosition = startPosition;
            
            float currentPositionX = startPosition.x;
            float currentPositionY = startPosition.y;
            int columnPositionInGrid = 0;

            for(int i = 0; i < cells.Length; i++)
            {
                T cell = cellBuilder.Build(i, currentPositionX, currentPositionY, parent, cells);
                RegisterCellEvents(input, sfxPlayer, cell);

                cells[i] = cell;

                currentPositionX += cellSize;
                columnPositionInGrid++;

                bool shouldMoveToNextRow = columnPositionInGrid >= rows;

                if (shouldMoveToNextRow)
                {
                    currentPositionY += cellSize;
                    currentPositionX = startPosition.x;
                    columnPositionInGrid = 0;
                }
            }
        }

        private static void RegisterCellEvents(InputHandler input, GridSFXPlayer sfxPlayer, T cell)
        {
            cell.OnClicked += input.GemCell_OnClicked;
            cell.OnClicked += sfxPlayer.GemCell_OnClicked;
        }

        /// <summary>
        /// A input event that handles cell selection.
        /// </summary>
        /// <param name="sender">The object that invoked the event</param>
        /// <param name="eventArgs">A class that contains event data, and provides values to use for the event.
        /// Is unused in this case.</param>
        public void InputHandler_OnTilesSelected(object sender, EventArgs eventArgs)
        {
            SetCellsAsSelectable(false);
        }

        /// <summary>
        /// An input event that handles cell 'de-selection'
        /// </summary>
        /// <param name="sender">The object that invoked the event</param>
        /// <param name="eventArgs">A class that contains event data, and provides values to use for the event.
        /// Is unused in this case.</param>
        public void InputHandler_OnTilesDeselected(object sender, EventArgs eventArgs)
        {
            SetCellsAsSelectable(true);
        }

        /// <summary>
        /// Sets the selectability of all grid cells.
        /// </summary>
        /// <param name="selectable">A bool that determines whether the grid cells should be selectable or not.</param>
        public void SetCellsAsSelectable(bool selectable)
        {
            foreach(T cell in cells)
            {
                cell.CanClick = selectable;
            }
        }
    }
}
