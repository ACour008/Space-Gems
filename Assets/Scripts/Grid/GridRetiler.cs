using System;
using System.Collections;
using UnityEngine;
using MiiskoWiiyaas.Grid;
using MiiskoWiiyaas.Builders;
using MiiskoWiiyaas.Core.Events;

namespace MiiskoWiiyaas.Core
{
    public class GridRetiler : MonoBehaviour
    {
        [SerializeField] float spawnPositionY;
        private GameGrid<GemCell> grid;
        private GemBuilder gemBuilder;

        public event EventHandler OnGridRetiled;
        public event EventHandler<Events.SFXEventArgs> OnGemMovedToPosition;

        public void Initialize(GameGrid<GemCell> grid, GameObject builderPrefab)
        {
            this.grid = grid;
            this.gemBuilder = new GemBuilder(builderPrefab, grid.Rows);
        }

        public void AddPowerGemAt(int index, GemColor gemColor)
        {
            GemCell cell = grid.Cells[index];
            Gem powerGem = gemBuilder.BuildPowerGem(cell.ID, 0, 0, cell.transform, gemColor);
            cell.CurrentGem = powerGem;
        }

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