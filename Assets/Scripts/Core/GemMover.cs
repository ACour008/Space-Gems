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

        public void Swap(GemCell cell1, GemCell cell2)
        {
            StopAllCoroutines();
            StartCoroutine(DoSwap(cell1, cell2));
        }

        public void MatchFinder_OnMatchMade(object sender, MatchEventArgs eventArgs)
        {
            swapState = 0;

            int powerGemCellId = (!eventArgs.firstSearch) ? eventArgs.scoreCell.ID : eventArgs.powerUpCellId;
            StopAllCoroutines();
            StartCoroutine(RemoveMatches(eventArgs.matches, powerGemCellId));
        }

        private void MoveTilesDown()
        {
            int max = grid.Cells.Length;
            int rows = grid.Rows;

            for (int idx = 0; idx < max; idx++)
            {
                GemCell cell = grid.Cells[idx];
                bool cellIsEmpty = cell.CurrentGem == null;

                if (cellIsEmpty)
                {
                    for (int i = idx + rows; i < max; i += rows)
                    {
                        GemCell nextCell = grid.Cells[i];
                        bool nextCellIsEmpty = nextCell.CurrentGem == null;

                        if (nextCellIsEmpty) continue;

                        nextCell.CurrentGem.Move(cell.transform.position);
                        OnGemMoved?.Invoke(this, new Events.SFXEventArgs() { startDelaySeconds = 0.25f });

                        cell.CurrentGem = nextCell.CurrentGem;
                        cell.CurrentGem.CurrentCellId = cell.ID;
                        nextCell.CurrentGem = null;

                        break;
                    }
                }
            }

            retiler.RefillGrid(0.25f);
        }

        private IEnumerator RemoveMatches(List<Gem> matches, int gemCellID)
        {
            float animationRunTime = 0;

            foreach (Gem gem in matches)
            {
                GemCell current = grid.Cells[gem.CurrentCellId];
                current.CurrentGem = null;
                animationRunTime = gem.Disappear(current, false);
            }

            if (matches.Count >= 4)
            {
                retiler.AddPowerGemAt(gemCellID, matches[0].Color);
            }

            yield return new WaitForSeconds(animationRunTime);
            MoveTilesDown();
        }
    }
}
