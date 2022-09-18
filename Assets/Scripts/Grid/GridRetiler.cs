using System;
using System.Collections;
using UnityEngine;
using MiiskoWiiyaas.Grid;
using MiiskoWiiyaas.Builders;

namespace MiiskoWiiyaas.Core
{
    public class GridRetiler : MonoBehaviour
    {
        [SerializeField] float spawnPositionY;
        private GameGrid<GemCell> grid;
        private GemBuilder gemBuilder;

        public event EventHandler OnGridRetiled;
        public event EventHandler<Events.SFXEventArgs> OnGemMovedToPosition;

        /// <summary>
        /// Sets up the GridRetiler.
        /// </summary>
        /// <param name="grid">The game grid to be retiled when instructed to do so.</param>
        /// <param name="gridCellPrefab">The prefab GameObject for the grid cell. Is passed along to the GemBuilder.</param>
        /// <seealso cref="GemBuilder"/>
        public void Initialize(GameGrid<GemCell> grid, GameObject gridCellPrefab)
        {
            this.grid = grid;
            this.gemBuilder = new GemBuilder(gridCellPrefab, grid.Rows);
        }

        /// <summary>
        /// Instantiates a new Power Gem for the cell of the given index.
        /// </summary>
        /// <param name="index">The index position of the array holding all grid cells.</param>
        /// <param name="gemColor">A specified color within the GemColor enum.</param>
        /// <seealso cref="GemColor"/>
        public void AddPowerGemAt(int index, GemColor gemColor)
        {
            GemCell cell = grid.Cells[index];
            Gem powerGem = gemBuilder.BuildPowerGem(cell.ID, 0, 0, cell.transform, gemColor);
            cell.CurrentGem = powerGem;
        }

        /// <summary>
        /// Refills the Grid with a procedurally generated (ie., semi-randomized) layout.
        /// </summary>
        /// <param name="startDelaySeconds">How long in seconds until the grid is refilled.</param>
        public void RefillGrid(float startDelaySeconds)
        {
            StopAllCoroutines();
            StartCoroutine(Refill(startDelaySeconds));
        }

        private IEnumerator Refill(float startDelaySeconds)
        {
            yield return new WaitForSeconds(startDelaySeconds);
            float animationRunTime = 0;

            for (int i = 0; i < grid.Cells.Length; i++)
            {
                GemCell cell = grid.Cells[i];

                if (cell.CurrentGem == null)
                {
                    Gem newGem = gemBuilder.Build(cell.ID, 0, spawnPositionY, cell.transform, grid.Cells);
                    
                    cell.CurrentGem = newGem;
                    animationRunTime = newGem.Move(cell.transform.position);
                    OnGemMovedToPosition?.Invoke(this, new Events.SFXEventArgs { startDelaySeconds = animationRunTime });
                }
            }

            yield return new WaitForSeconds(animationRunTime);
            OnGridRetiled?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Refills the Grid with a given layout as determined by the given GemColor array.
        /// </summary>
        /// <param name="layout">An array of GemColors that determines the color of the gem in each cell.</param>
        /// <param name="startDelaySeconds">How long in seconds until the Grid is refilled.</param>
        public void RefillGridWithLayout(GemColor[] layout, float startDelaySeconds)
        {
            StopAllCoroutines();
            StartCoroutine(RefillWithLayout(layout, startDelaySeconds));
        }

        private IEnumerator RefillWithLayout(GemColor[] layout, float startDelaySeconds)
        {
            yield return new WaitForSeconds(startDelaySeconds);
            float animationRunTime = 0;

            for (int i = 0; i < grid.Cells.Length; i++)
            {
                GemCell cell = grid.Cells[i];

                Gem newGem = gemBuilder.BuildFromLayout(cell.ID, 0, spawnPositionY, cell.transform, grid.Cells, layout);

                cell.CurrentGem = newGem;
                animationRunTime = newGem.Move(cell.transform.position);
                OnGemMovedToPosition?.Invoke(this, new Events.SFXEventArgs { startDelaySeconds = animationRunTime });
            }

            yield return new WaitForSeconds(animationRunTime);
        }

    }

}