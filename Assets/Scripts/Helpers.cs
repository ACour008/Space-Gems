using System.Collections.Generic;
using UnityEngine;
using MiskoWiiyaas.Tiles;
using MiskoWiiyaas.Cells;
using MiskoWiiyaas.Enums;

namespace MiskoWiiyaas
{
    public static class Helpers
    {

        public static GridCell GetCell(GridCell[,] cells, int x, int y)
        {
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x >= cells.GetLength(0)) x = cells.GetLength(0) - 1;
            if (y >= cells.GetLength(1)) y = cells.GetLength(1) - 1;
            
            return cells[x, y];
        }
        public static TileType? GetTileType(GridCell[,] cells, int x, int y)
        {
            Tile tile = GetTileAt(cells, x, y);
            if (tile != null)
            {
                return tile.tileType;
            }
            return null;
        }

        public static Tile GetTileAt(GridCell[,] cells, int x, int y)
        {
            if (x < 0 || y < 0 || x >= cells.GetLength(0) || y >= cells.GetLength(1)) return null;
            return cells[x, y].currentTile;
        }

        public static GridCell GetCellById(GridCell[,]cells, int id)
        {
            foreach(GridCell cell in cells)
            {
                if (cell.id == id)
                {
                    return cell;
                }
            }
            return null;
        }

        public static List<GridCell> MakeListFrom(GridCell[,] cells, int rows, int cols)
        {
            List<GridCell> cellsList = new List<GridCell>();
            int x, y;

            for (int i = 0; i < rows * cols; i++)
            {
                x = i % rows;
                y = i / rows;

                cellsList.Add(cells[x, y]);
            }

            return cellsList;
        }

        public static GridCell GetRandomCellFrom(GridCell[,] cells)
        {
            int rows = cells.GetUpperBound(0) + 1;
            int cols = cells.GetUpperBound(1) + 1;
            int x = Random.Range(0, rows);
            int y = Random.Range(0, cols);

            return cells[x, y];
        }

        public static GridCell GetRandomCellFrom(List<GridCell> cells)
        {
            int randomIndex = Random.Range(0, cells.Count);
            return cells[randomIndex];
        }

    }
}