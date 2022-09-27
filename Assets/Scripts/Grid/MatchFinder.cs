using System;
using System.Collections.Generic;
using UnityEngine;
using MiiskoWiiyaas.Grid;
using MiiskoWiiyaas.Core.Events;
using MiiskoWiiyaas.Audio;
using MiiskoWiiyaas.Utils;

namespace MiiskoWiiyaas.Core
{
    /// <summary>
    /// Checks for matches of 3 or more after a swap has occurred. Not to be
    /// confused with MatchChecker which checks for potential matches without
    /// triggering move animations.
    /// </summary>
    public class MatchFinder : MonoBehaviour
    {
        private GameGrid<GemCell> grid;
        
        private HashSet<Gem> matches;
        private int numMatchesInCurrentTurn = 1;
        private MatchSFXType sfxType;

        public bool HasPotentialMatches { get => true; }

        public event EventHandler<MatchEventArgs> OnMatchProcessed;
        public event EventHandler OnMatchFound;
        public event EventHandler<SFXEventArgs> OnPlayMatchSFX;

        private void AddNextGemsToResultsIfTheyMatch(int index, bool searchCols, GemCell currentCell, List<Gem> results)
        {
            int x = index % grid.Rows;
            int y = index / grid.Rows;
            int startAxis = searchCols ? y : x;

            for (int i = startAxis + 1; i < grid.Rows; i++)
            {
                int nextIndex = (searchCols) ? x + grid.Rows * i : i + grid.Rows * y;

                GemCell next = grid.Cells[nextIndex];
                if (next.CurrentGem == null) break;

                bool nextMatchesWithCurrentGem = next.CurrentGem.Color == currentCell.CurrentGem.Color;
                if (!nextMatchesWithCurrentGem) break;

                results.Add(next.CurrentGem);
            }
        }

        /// <summary>
        /// Finds all matches of 3 or more in a grid.
        /// </summary>
        /// <param name="selected">An array containing the GemCells that will swap Gems</param>
        /// <param name="firstSearch">A bool indicating whether the match-finding process
        /// is on its first iteration or is looking for subsequent matches that may have occured
        /// when the first set of matches disappeared and the grid was refilled.</param>
        /// <returns>A bool that indicates whether any matches were found.</returns>
        public bool FindAllMatches(GemCell[] selected, bool firstSearch)
        {
            matches.Clear();

            for (int i = 0; i < grid.Cells.Length; i++)
            {
                GemCell current = grid.Cells[i];

                List<Gem> rowMatches = FindRowMatches(current, i);
                List<Gem> colMatches = FindColMatches(current, i);

                ProcessMatchList(rowMatches, selected, firstSearch);
                ProcessMatchList(colMatches, selected, firstSearch);
            }

            return IsMatchFound();
        }

        private List<Gem> FindColMatches(GemCell currentCell, int index)
        {
            List<Gem> results = new List<Gem>();

            if (currentCell.CurrentGem != null)
            {
                results.Add(currentCell.CurrentGem);
                AddNextGemsToResultsIfTheyMatch(index, true, currentCell, results);
            }

            return results;
        }

        private List<Gem> FindRowMatches(GemCell currentCell, int index)
        {
            List<Gem> results = new List<Gem>();

            if (currentCell.CurrentGem != null)
            {
                results.Add(currentCell.CurrentGem);
                AddNextGemsToResultsIfTheyMatch(index, false, currentCell, results);
            }

            return results;
        }

        /// <summary>
        /// Sets up the MatchFinder object
        /// </summary>
        /// <param name="grid">The main Game Grid.</param>
        public void Initialize(GameGrid<GemCell> grid)
        {
            this.grid = grid;
            matches = new HashSet<Gem>();
        }

        private bool IsMatchFound()
        {
            bool matchesFound = matches.Count > 0;
            if (matchesFound)
            {
                OnPlayMatchSFX?.Invoke(this, new SFXEventArgs { matchCount = this.numMatchesInCurrentTurn, sfxType = this.sfxType });
                OnMatchFound?.Invoke(this, EventArgs.Empty);
                numMatchesInCurrentTurn = (numMatchesInCurrentTurn + 1) % 6;
            }
            else
            {
                numMatchesInCurrentTurn = 1;
            }

            return matchesFound;
        }

        private void ProcessMatchList(List<Gem> matchList, GemCell[] selected, bool firstSearch)
        {
            bool matchExists = matchList.Count >= 3;
            
            if (matchExists)
            {
                DetermineSFX(matchList);

                matches.UnionWith(matchList);
                int powerUpLocation = (GameGridUtils.MatchContains(matchList, selected[1])) ? selected[1].ID : selected[0].ID;

                OnMatchProcessed?.Invoke(this, new MatchEventArgs()
                {
                    matches = matchList,
                    scoreCell = grid.Cells[matchList[matchList.Count / 2].CurrentCellId],
                    powerUpCellId = powerUpLocation,
                    firstSearch = firstSearch
                });
            }
        }

        // TODO: rethink logic. Sometimes it does not play correct SFX.
        private void DetermineSFX(List<Gem> matchList)
        {
            if (GameGridUtils.MatchContainsType(matchList, MatchSFXType.BOMB))
            {
                sfxType = MatchSFXType.BOMB;
                return;
            }

            if (GameGridUtils.MatchContainsType(matchList, MatchSFXType.ELECTRIC))
            {
                sfxType = MatchSFXType.ELECTRIC;
                return;
            }

            if (matchList.Count >= 4)
            {
                sfxType = MatchSFXType.POWERUP;
                return;
            }

            sfxType = MatchSFXType.NORMAL;
        }
    }
} 