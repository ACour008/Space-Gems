using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiskoWiiyaas.Cells;
using MiskoWiiyaas.Tiles;

namespace MiskoWiiyaas.Grid
{
    public class Shuffler : MonoBehaviour
    {
        private GridCell[,] selectionPool;
        private List<GridCell> newSelectionPool;
        private int rows;
        private int cols;

        public void Initialize(GridCell[,] cells)
        {
            selectionPool = cells;
            rows = cells.GetUpperBound(0) + 1;
            cols = cells.GetUpperBound(1) + 1;
        }

        public GridCell[,] GetShuffled()
        {
            GridCell[,] shuffled = new GridCell[rows, cols];
            int x, y;

            for (int i = 0; i < newSelectionPool.Count; i++)
            {
                x = i % rows;
                y = i / rows;

                shuffled[x, y] = newSelectionPool[i]; 
            }

            return shuffled;
        }

        public void Shuffle()
        {
            newSelectionPool = Helpers.MakeListFrom(selectionPool, rows, cols);
            int i = 0;

            while (i < newSelectionPool.Count)
            {
                GridCell cell = newSelectionPool[i];
                GridCell random = Helpers.GetRandomCellFrom(newSelectionPool);

                if (!MoveCausesMatchAt(cell, random) && !MoveCausesMatchAt(random, cell))
                {
                    Tile temp = cell.currentTile;
                    cell.currentTile = random.currentTile;
                    random.currentTile = temp;

                    newSelectionPool.Remove(cell);
                    newSelectionPool.Remove(random);
                }
            }
        }

        public void MoveShuffledTiles(GridCell[,] cells)
        {
            int rows = cells.GetUpperBound(0) + 1;
            int cols = cells.GetUpperBound(1) + 1;
            int x, y;

            for (int i = 0; i < rows * cols; i++)
            {
                x = i % rows;
                y = i / rows;

                Tile current = cells[x, y].currentTile;
                GridCell newCell = Helpers.GetCellById(cells, current.currentCellId);

                StartCoroutine(MoveTile(current, newCell));
            }
        }

        private bool MoveCausesMatchAt(GridCell current, GridCell random)
        {
            Tile randomized = random.currentTile;
            return (RowMatchExists(current, randomized) || ColMatchExists(current, randomized));
        }

        private IEnumerator MoveTile(Tile tile, GridCell cell)
        {
            yield return new WaitForSeconds(0.1f);
            yield return StartCoroutine(tile.MoveTo(cell, false));
        }

        private bool RowMatchExists(GridCell current, Tile randomized)
        {
            Tile west = current.GetNeighbor("west")?.currentTile;
            Tile nextWest = current.GetNextNeighbor("west")?.currentTile;
            Tile east = current.GetNeighbor("east")?.currentTile;
            Tile nextEast = current.GetNextNeighbor("east")?.currentTile;

            bool centerMatch = (randomized.tileType == east?.tileType && randomized.tileType == west?.tileType);
            bool matchGoingRight = (randomized.tileType == west?.tileType && randomized.tileType == nextWest?.tileType);
            bool matchGoingLeft = (randomized.tileType == east?.tileType && randomized.tileType == nextEast?.tileType);

            return centerMatch || matchGoingLeft || matchGoingRight;
        }

        private bool ColMatchExists(GridCell current, Tile randomized)
        {
            Tile south = current.GetNeighbor("south")?.currentTile;
            Tile nextSouth = current.GetNextNeighbor("south")?.currentTile;
            Tile north = current.GetNeighbor("north")?.currentTile;
            Tile nextNorth = current.GetNextNeighbor("north")?.currentTile;

            bool centerMatch = (randomized.tileType == north?.tileType && randomized.tileType == south?.tileType);
            bool matchGoingUp = (randomized.tileType == north?.tileType && randomized.tileType == nextNorth?.tileType);
            bool matchGoingDown = (randomized.tileType == south?.tileType && randomized.tileType == nextSouth?.tileType);

            return centerMatch || matchGoingUp || matchGoingDown;
        }
    }
}

