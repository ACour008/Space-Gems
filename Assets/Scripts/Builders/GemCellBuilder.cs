using UnityEngine;
using MiiskoWiiyaas.Core;

namespace MiiskoWiiyaas.Builders
{
    [System.Serializable]
    public class GemCellBuilder : IBuilder<GemCell, GemCell>
    {
        private GameObject cellPrefab;
        private int rows, cols, colorOffset;
        private IBuilder<Gem, GemCell> gemBuilder;

        public GemCellBuilder(GameObject cellPrefab, GameObject cellItemPrefab, int rows, int cols)
        {
            this.cellPrefab = cellPrefab;
            this.rows = rows;
            this.cols = cols;
            this.gemBuilder = new GemBuilder(cellItemPrefab, rows);
            this.colorOffset = 0;
        }

        public GemCell Build(int id, float xPosition, float yPosition, Transform parent, GemCell[] cells)
        {
            if (id % rows == 0) colorOffset++;

            Vector3 position = new Vector3(xPosition, yPosition, 0);
            GameObject instance = GameObject.Instantiate<GameObject>(cellPrefab, position, Quaternion.identity, parent);

            instance.name = "Cell " + id;

            SpriteRenderer spriteRenderer = instance.GetComponent<SpriteRenderer>();
            GemCell gemCell = instance.GetComponent<GemCell>();

            gemCell.Init(id);
            SetNeighbors(gemCell, cells);

            float blackOrWhite = (id + colorOffset) % 2;
            spriteRenderer.color = new Color(blackOrWhite, blackOrWhite, blackOrWhite, 0.15f);

            gemCell.CurrentGem = gemBuilder.Build(id, 0, 0, gemCell.transform, cells);

            return gemCell;
        }

        public GemCell BuildFromLayout(int id, float xPosition, float yPosition, Transform parent, GemCell[] cells, GemColor[] layout)
        {

            if (id % rows == 0) colorOffset++;

            Vector3 position = new Vector3(xPosition, yPosition, 0);
            GameObject instance = GameObject.Instantiate<GameObject>(cellPrefab, position, Quaternion.identity, parent);

            instance.name = $"Cell {id}";

            SpriteRenderer spriteRenderer = instance.GetComponent<SpriteRenderer>();
            GemCell gemCell = instance.GetComponent<GemCell>();

            gemCell.Init(id);
            SetNeighbors(gemCell, cells);

            float blackOrWhite = (id + colorOffset) % 2;
            spriteRenderer.color = new Color(blackOrWhite, blackOrWhite, blackOrWhite, 0.15f);

            gemCell.CurrentGem = gemBuilder.BuildFromLayout(id, xPosition, yPosition, parent, cells, layout);

            return gemCell;


        }

        private void SetNeighbors(GemCell cell, GemCell[] cells)
        {
            int id = cell.ID;
            int x = id % rows;
            int y = id / rows;
            GemCell neighbor;

            bool notBottomRow = (y % rows) != 0;
            bool notLeftColumn = (x % cols) != 0;
            bool notRightCol = (x % rows - 1) != 0;


            if (notBottomRow)
            {
                neighbor = cells[id - rows];
                cell.AddNeighbor("south", neighbor);
                neighbor.AddNeighbor("north", cell);

                if (notRightCol)
                {
                    neighbor = cells[id - rows + 1];
                    cell.AddNeighbor("southeast", neighbor);
                    neighbor.AddNeighbor("northwest", cell);
                }
            }

            if (notLeftColumn)
            {
                neighbor = cells[id - 1];
                cell.AddNeighbor("west", neighbor);
                neighbor.AddNeighbor("east", cell);

                if (notBottomRow)
                {
                    neighbor = cells[id - rows - 1];
                    cell.AddNeighbor("southwest", neighbor);
                    neighbor.AddNeighbor("northeast", cell);
                }
            }
        }
    }
}
