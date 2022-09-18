using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiiskoWiiyaas.Grid;
using MiiskoWiiyaas.Core.Events;

namespace MiiskoWiiyaas.Core
{

    public class GemMover : MonoBehaviour
    {
        private bool animationCompleted = false;

        private GameGrid<GemCell> grid;
        private GridRetiler retiler;
        private Switcher switcher;
        private int swapState = 0;

        public bool IsFinishedMoving { get => animationCompleted; }

        public event EventHandler<Events.SFXEventArgs> OnSwap;
        public event EventHandler<Events.SFXEventArgs> OnGemMoved;

        /// <summary>
        /// Sets up GemMover.
        /// </summary>
        /// <param name="grid">The main game grid object.</param>
        /// <param name="retiler">The object responsible for retiling the grid after it has been cleared.</param>
        public void Initialize(GameGrid<GemCell> grid, GridRetiler retiler)
        {
            switcher = new Switcher();
            this.retiler = retiler;
            this.grid = grid;
        }

        private IEnumerator DoSwap(GemCell cell1, GemCell cell2)
        {
            animationCompleted = false;

            OnSwap?.Invoke(this, new Events.SFXEventArgs() { startDelaySeconds = 0, swapState = this.swapState });

            cell1.CurrentGem.Move(cell2.transform.position);
            cell2.CurrentGem.Move(cell1.transform.position);

            switcher.ExchangeGems(cell1, cell2);

            yield return new WaitUntil(() => cell1.CurrentGem.AnimationCompleted && cell2.CurrentGem.AnimationCompleted);
            animationCompleted = true;

            swapState = (swapState + 1) % 2;
        }

        /// <summary>
        /// Starts the swap animation between two Gems.
        /// </summary>
        /// <param name="cell">The cell containing the Gem to be swapped with another Gem.</param>
        /// <param name="otherCell">The other cell containing the Gem to be swapped.</param>
        public void Swap(GemCell cell, GemCell otherCell)
        {
            StopAllCoroutines();
            StartCoroutine(DoSwap(cell, otherCell));
        }

        /// <summary>
        /// An event method that removes all matches from the grid after they have been found.
        /// </summary>
        /// <param name="sender">The MatchFinder object that invoked the event.</param>
        /// <param name="eventArgs">The EventArgs containing the data needed to determine if a power Gem is needed.</param>
        /// <seealso cref="MatchFinder"/>
        public void MatchFinder_OnMatchMade(object sender, MatchEventArgs eventArgs)
        {
            swapState = 0;

            int powerGemCellId = (!eventArgs.firstSearch) ? eventArgs.scoreCell.ID : eventArgs.powerUpCellId;
            StopAllCoroutines();
            StartCoroutine(RemoveMatches(eventArgs.matches, powerGemCellId));
        }

        private void MoveTilesDown()
        {
            int numberOfCells = grid.Cells.Length;

            for (int i = 0; i < numberOfCells; i++)
            {
                GemCell currentCell = grid.Cells[i];
                bool cellIsEmpty = currentCell.CurrentGem == null;

                if (cellIsEmpty)
                {
                    MoveDownCellsInSameColumn(numberOfCells, grid.Rows, currentCell);
                }
            }

            retiler.RefillGrid(0.25f);
        }

        private void MoveDownCellsInSameColumn(int max, int rows, GemCell currentCell)
        {
            for (int i = currentCell.ID + rows; i < max; i += rows)
            {
                GemCell nextCell = grid.Cells[i];
                bool nextCellIsEmpty = nextCell.CurrentGem == null;

                if (nextCellIsEmpty) continue;

                nextCell.CurrentGem.Move(currentCell.transform.position);
                OnGemMoved?.Invoke(this, new Events.SFXEventArgs() { startDelaySeconds = 0.25f });

                currentCell.CurrentGem = nextCell.CurrentGem;
                currentCell.CurrentGem.CurrentCellId = currentCell.ID;
                nextCell.CurrentGem = null;

                break;
            }
        }

        private IEnumerator RemoveMatches(List<Gem> matches, int gemCellID)
        {
            float animationRunTime = 0;
            bool powerGemNeeded = matches.Count >= 4;

            foreach (Gem gem in matches)
            {
                GemCell current = grid.Cells[gem.CurrentCellId];
                current.CurrentGem = null;
                animationRunTime = gem.Disappear(current, false);
            }

            if (powerGemNeeded) retiler.AddPowerGemAt(gemCellID, matches[0].Color);

            yield return new WaitForSeconds(animationRunTime);
            MoveTilesDown();
        }
    }
}
