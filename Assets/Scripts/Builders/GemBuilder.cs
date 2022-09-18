using UnityEngine;
using MiiskoWiiyaas.Core;

namespace MiiskoWiiyaas.Builders
{
    public class GemBuilder : IBuilder<Gem, GemCell>
    {
        private GameObject prefab;
        private GemSelector gemSelector;
        private int rows;

        /// <summary>
        /// The Constructor for the GemBuilder, which procedurally builds a gem for the game grid.
        /// </summary>
        /// <param name="gemPrefab">The prefab for the Gem to be built by GemBuilder</param>
        /// <param name="rows">The number of rows the game grid has</param>
        public GemBuilder(GameObject gemPrefab, int rows)
        {
            this.rows = rows;
            prefab = gemPrefab;
            gemSelector = new GemSelector(new System.Random());
        }

        /// <summary>
        /// Builds all the components for a Gem object and assigns it to a cell within the game grid.
        /// </summary>
        /// <param name="id">The id of the cell assigned to the Gem.</param>
        /// <param name="xPosition">The x coordinate of the Gem's cell.</param>
        /// <param name="yPosition">The y coorindate of the Gem's cell.</param>
        /// <param name="parent">The transform object of the cell.</param>
        /// <param name="cells">An array of all GemCells that belongs to the game grid.</param>
        /// <returns>A Gem object with a generated color</returns>
        public Gem Build(int id, float xPosition, float yPosition, Transform parent, GemCell[] cells)
        {
            GameObject instance = GameObject.Instantiate<GameObject>(prefab, parent);
            Gem newGem = instance.GetComponent<Gem>();
            newGem.transform.localPosition = new Vector3(xPosition, yPosition, 0);
            newGem.CurrentCellId = id;


            GemColor randomColor = gemSelector.GetRandomColor(id, rows, cells, 0, 6);
            newGem.SetGemColor(randomColor);
            newGem.SetGemType(GemType.NORMAL);
            newGem.SetAnimator(new NormalGemAnimator(newGem));

            instance.name = $"{newGem.Color}";
            
            return newGem;
        }

        /// <summary>
        /// Builds all the components for a Gem object and assigns it to a cell within the game
        /// grid based on a color layout.
        /// </summary>
        /// <param name="id">The id of the cell assigned to the Gem.</param>
        /// <param name="xPosition">The x coordinate of the Gem's cell.</param>
        /// <param name="yPosition">The y coordinate of the Gem's cell.</param>
        /// <param name="parent">The transform object of the cell.</param>
        /// <param name="cells">An array that represents the game grid.</param>
        /// <param name="gemColorGrid">An array that holds the grid layout data.</param>
        /// <returns>A Gem object with a specified color from the <c>gameColorGrid</c> array.</returns>
        public Gem BuildFromLayout(int id, float xPosition, float yPosition, Transform parent, GemCell[] cells, GemColor[] gemColorGrid)
        {
            GameObject instance = GameObject.Instantiate<GameObject>(prefab, parent);
            Gem newGem = instance.GetComponent<Gem>();

            newGem.transform.localPosition = new Vector3(xPosition, yPosition, 0);
            newGem.CurrentCellId = id;
            newGem.SetGemColor(gemColorGrid[id]);
            newGem.SetGemType(GemType.NORMAL);

            newGem.SetAnimator(new NormalGemAnimator(newGem));

            instance.name = $"{newGem.Color}";

            return newGem;
        }

        /// <summary>
        /// Builds all the components for a Gem object and assigns a color to it independently of any game grid layout.
        /// </summary>
        /// <param name="id">The id of the cell assigned to the Gem</param>
        /// <param name="xPosition">The x coordinate of the Gem's cell.</param>
        /// <param name="yPosition">The y coordinate of the Gem's cell.</param>
        /// <param name="parent">The transform object of the cell.</param>
        /// <param name="cells">An array of all GemCells that belongs to the game grid.</param>
        /// <param name="color">The color that gets assigned to the Gem</param>
        /// <returns>A Gem object with a specified color from the argument <c>color</c>.</returns>
        public Gem BuildWithColor(int id, float xPosition, float yPosition, Transform parent, GemCell[] cells, GemColor color)
        {
            GameObject instance = GameObject.Instantiate<GameObject>(prefab, parent);
            Gem newGem = instance.GetComponent<Gem>();

            newGem.transform.localPosition = new Vector3(xPosition, yPosition, 0);
            newGem.CurrentCellId = id;
            newGem.SetGemColor(color);
            newGem.SetGemType(GemType.NORMAL);

            newGem.SetAnimator(new NormalGemAnimator(newGem));

            instance.name = $"{newGem.Color}";
            return newGem;
        }

        /// <summary>
        /// Builds all the necessary components for a power Gem object.
        /// </summary>
        /// <param name="id">The id of the cell assigned to the Gem</param>
        /// <param name="xPosition">The x coordinate of the Gem's cell.</param>
        /// <param name="yPosition">The y coordinate of the Gem's cell.</param>
        /// <param name="parent">The transform object of the cell.</param>
        /// <param name="gemColor">The GemColor of the Gem that got turned into a Power Gem.</param>
        /// <returns>A Gem object with a Power Gem Animator.</returns>
        /// <seealso cref="GemAnimator"/>
        public Gem BuildPowerGem(int id, float xPosition, float yPosition, Transform parent, GemColor gemColor)
        {
            GameObject instance = GameObject.Instantiate<GameObject>(prefab, parent);
            Gem powerGem = instance.GetComponent<Gem>();

            powerGem.transform.localPosition = new Vector3(xPosition, yPosition, 0);
            powerGem.CurrentCellId = id;

            powerGem.SetGemColor(gemColor);
            powerGem.SetGemType((GemType)UnityEngine.Random.Range(1, 3));

            GemAnimator animator = (powerGem.Type == GemType.BOMB) ? new BombGemAnimator(powerGem) : new ElectricGemAnimator(powerGem);

            powerGem.SetAnimator(animator);

            instance.name = $"{powerGem.Color}";

            return powerGem;
        }
    }
}
