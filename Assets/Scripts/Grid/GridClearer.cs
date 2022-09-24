using System.Collections;
using UnityEngine;
using MiiskoWiiyaas.Grid;

namespace MiiskoWiiyaas.Core
{
    public class GridClearer : MonoBehaviour
    {
        [SerializeField] Vector3 destroyPoint;
        private GameGrid<GemCell> grid;

        /// <summary>
        /// Clears the grid.
        /// </summary>
        public void Clear()
        {
            StartCoroutine(DoClear());
        }

        private IEnumerator DoClear()
        {
            yield return new WaitForSeconds(MoveAllGemsToDisappearPoint());
            yield return new WaitForSeconds(DisappearAllGems());
        }

        private float MoveAllGemsToDisappearPoint()
        {
            float moveRunTime = 0;

            for (int i = 0; i < grid.Cells.Length; i++)
            {
                moveRunTime = grid.Cells[i].CurrentGem.Move(destroyPoint);
            }
            return moveRunTime;
        }

        private float DisappearAllGems()
        {
            float disappearRunTime = 0;

            for (int i = 0; i < grid.Cells.Length; i++)
            {
                GemCell current = grid.Cells[i];
                disappearRunTime = current.CurrentGem.Disappear(current);
            }

            return disappearRunTime;
        }

        /// <summary>
        /// Sets up the GridClearer
        /// </summary>
        /// <param name="grid">The game grid to be cleared when instructed to do so.</param>
        public void Initialize(GameGrid<GemCell> grid)
        {
            this.grid = grid;
        }
    }

}