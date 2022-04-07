using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiskoWiiyaas.Tiles;
using MiskoWiiyaas.Cells;
using MiskoWiiyaas.Enums;

namespace MiskoWiiyaas.Grid
{
    public class GridMaker : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject tilePrefab;
        private float retileProb;
        private float cellTransparency;

        public GameObject TilePrefab { get => tilePrefab; }

        public GridCell[,] Initialize(int rows, int cols, float cellAlpha, float retileProbability)
        {
            GridCell[,] cells = new GridCell[rows, cols];
            retileProb = retileProbability;
            cellTransparency = cellAlpha;

            // Refactor. Create Empty cells then neighbors. Then add tiles;
            PopulateGridWithCells(cells, rows, cols);
            CreateCellNeighbors(cells, rows, cols);
            DoRetiling(cells, rows, cols);

            return cells;

        }

        public void FillCellsWithTiles(GridCell[,] cells, int rows, int cols, Action<Tile> callback)
        {
            int x;
            int y;

            for (int i =0; i < rows * cols; i++)
            {
                x = i % rows;
                y = i / rows;
                GridCell cell = cells[x, y];

                Tile tile = cell.CreateTile(tilePrefab, ListPossibleTypes(cells, x, y), cell.transform);
                callback(tile);
            }

            DoRetiling(cells, rows, cols);
        }

        public Tile CreateRandomTile(GridCell cell)
        {
            List<TileType> allTypes = new List<TileType>((TileType[])Enum.GetValues(typeof(TileType)));
            return cell.CreateTile(tilePrefab, allTypes, cell.transform);
        }

        private void PopulateGridWithCells(GridCell[,] cells, int rows, int cols)
        {
            float xPos = -3.5f + transform.position.x; // remove hardcode.
            float yPos = -3.5f + transform.position.y;
            float colorOffset = 0;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    int id = y * rows + x;
                    if (id % rows == 0) { colorOffset++; }

                    List<TileType> possibilities = ListPossibleTypes(cells, x, y);
                    cells[x, y] = CreateCell(id, x, y, xPos, yPos, colorOffset, possibilities, cellTransparency);

                    xPos += 1f;
                }
                xPos = -3.5f + transform.position.x;
                yPos += 1f;
            }
        }

        private GridCell CreateCell(int index, int x, int y, float xPos, float yPos, float offset, List<TileType> possibleTypes, float cellAlpha)
        {
            GameObject instance = GameObject.Instantiate<GameObject>(cellPrefab, transform);
            instance.name = $"Cell_{index}";
            instance.transform.position = new Vector3(xPos, yPos, 0);

            SpriteRenderer spriteRenderer = instance.GetComponent<SpriteRenderer>();
            float blackOrWhite = (index + offset) % 2;
            spriteRenderer.color = new Color(blackOrWhite, blackOrWhite, blackOrWhite, cellAlpha);

            GridCell cell = instance.GetComponent<GridCell>();
            cell.SetID(index);
            cell.CreateTile(tilePrefab, possibleTypes, cell.transform);

            return cell;
        }

        private void CreateCellNeighbors(GridCell[,] cells, int rows, int cols)
        {
            for (int i = 0; i < rows * cols; i++)
            {
                int x = i % rows;
                int y = i / rows;

                bool notBottomRow = (y % rows) != 0;
                bool notLeftCol = (x % cols) != 0;
                bool notRightCol = x + 1 != rows;
                bool notTopRow = y + 1 != cols;

                if (notBottomRow) { cells[x, y].AddNeighbor("south", cells[x, y - 1]); }

                if (notLeftCol) { cells[x, y].AddNeighbor("west", cells[x - 1, y]); }

                if (notRightCol) { cells[x, y].AddNeighbor("east", cells[x + 1, y]); }

                if (notTopRow) { cells[x, y].AddNeighbor("north", cells[x, y + 1]); }
            }
        }

        private List<TileType> ListPossibleTypes(GridCell[,] cells, int x, int y)
        {
            List<TileType> possibleTypes = new List<TileType>((TileType[])Enum.GetValues(typeof(TileType)));

            TileType? west = Helpers.GetTileType(cells, x - 1, y) ;
            TileType? nextWest = Helpers.GetTileType(cells, x - 2, y);
            bool matchFoundInRow = nextWest != null && west == nextWest;

            if (matchFoundInRow)
            {
                possibleTypes.Remove((TileType)west);
            }

            TileType? south = Helpers.GetTileType(cells, x, y - 1);
            TileType? nextSouth = Helpers.GetTileType(cells, x, y - 2);
            bool matchFoundInCol = nextSouth != null && south == nextSouth;

            if (matchFoundInCol)
            {
                possibleTypes.Remove((TileType)south);
            }

            return possibleTypes;
        }

        private void DoRetiling(GridCell[,] cells, int rows, int cols)
        {
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    TileType? current = Helpers.GetTileType(cells, x, y);
                    TileType? left = Helpers.GetTileType(cells, x - 1, y);

                    List<TileType> types = new List<TileType>((TileType[])Enum.GetValues(typeof(TileType)));
                    float prob = UnityEngine.Random.Range(0f, 1f);

                    if (left != null & left == current)
                    {
                        if (retileProb > prob)
                        {
                            types.Remove((TileType)current);

                            GameObject.Destroy(cells[x, y].currentTile.gameObject);
                            cells[x, y].currentTile = null;
                            cells[x, y].CreateTile(tilePrefab, types, cells[x, y].gameObject.transform);       
                        }
                    }


                    TileType? down = Helpers.GetTileType(cells, x, y - 1);

                    if (down != null & down == current)
                    {
                        if (retileProb > prob)
                        {
                            types.Remove((TileType)current);

                            GameObject.Destroy(cells[x, y].currentTile.gameObject);
                            cells[x, y].currentTile = null;
                            cells[x, y].CreateTile(tilePrefab, types, cells[x, y].gameObject.transform);                    
                        }
                    }
                }
            }
        }

        public void Shuffle()
        {

        }


    }
}
