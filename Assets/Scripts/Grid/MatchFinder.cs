using System;
using System.Collections.Generic;
using UnityEngine;
using MiiskoWiiyaas.Grid;
using MiiskoWiiyaas.Core.Events;
using MiiskoWiiyaas.Audio;

namespace MiiskoWiiyaas.Core
{
    public class MatchFinder : MonoBehaviour
    {
        private GameGrid<GemCell> grid;
        
        private HashSet<Gem> matches;
        private int matchCount = 1;
        private MatchSFXType sfxType;

        public bool HasPotentialMatches { get => true; }

        public event EventHandler<MatchEventArgs> OnMatchMade;
        public event EventHandler OnSequenceDone;
        public event EventHandler<SFXEventArgs> OnPlayMatchSFX;

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

            bool matchesFound = matches.Count > 0;
            if (matchesFound)
            {
                OnPlayMatchSFX?.Invoke(this, new SFXEventArgs { matchCount = this.matchCount, sfxType = this.sfxType });
                OnSequenceDone?.Invoke(this, EventArgs.Empty);
                matchCount = (matchCount + 1) % 6;
            } 
            else
            {
                matchCount = 1;
            }

            return matchesFound;
        }

        private List<Gem> FindColMatches(GemCell gemCell, int index)
        {
            List<Gem> results = new List<Gem>();

            if (gemCell.CurrentGem != null)
            {
                results.Add(gemCell.CurrentGem);

                for (int i = index + 8; i < grid.Cells.Length; i += 8)
                {
                    GemCell next = grid.Cells[i];
                    if (next.CurrentGem == null) break;

                    bool nextMatchesWithCurrent = next.CurrentGem.Color == gemCell.CurrentGem.Color;

                    if (!nextMatchesWithCurrent) break;
                    results.Add(next.CurrentGem);
                }
            }

            return results;
        }

        private List<Gem> FindRowMatches(GemCell gemCell, int index)
        {
            List<Gem> results = new List<Gem>();

            if (gemCell.CurrentGem != null)
            {
                results.Add(gemCell.CurrentGem);

                int x = index % grid.Rows;
                int y = index / grid.Rows;

                for (int i = x + 1; i < grid.Cols; i++)
                {
                    GemCell next = grid.Cells[i + grid.Rows * y];
                    if (next.CurrentGem == null) break;

                    bool nextMatchesWithCurrent = next.CurrentGem.Color == gemCell.CurrentGem.Color;

                    if (!nextMatchesWithCurrent) break;
                    results.Add(next.CurrentGem);
                }
            }

            return results;
        }

        public void Initialize(GameGrid<GemCell> grid)
        {
            this.grid = grid;
            matches = new HashSet<Gem>();
        }

        private void ProcessMatchList(List<Gem> matchList, GemCell[] selected, bool firstSearch)
        {
            bool matchExists = matchList.Count >= 3;
            
            if (matchExists)
            {
                DetermineSFX(matchList);

                matches.UnionWith(matchList);
                int powerUpLocation = (MatchContains(matchList, selected[1])) ? selected[1].ID : selected[0].ID;

                OnMatchMade?.Invoke(this, new MatchEventArgs()
                {
                    matches = matchList,
                    scoreCell = grid.Cells[matchList[matchList.Count / 2].CurrentCellId],
                    powerUpCellId = powerUpLocation,
                    firstSearch = firstSearch
                });
            }
        }

        private void DetermineSFX(List<Gem> matchList)
        {
            if (MatchContainsType(matchList, MatchSFXType.BOMB))
            {
                sfxType = MatchSFXType.BOMB;
                return;
            }

            if (MatchContainsType(matchList, MatchSFXType.ELECTRIC))
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

        // To own Utils class
        private bool MatchContains(List<Gem> match, GemCell gemCell)
        {
            for (int i = 0; i < match.Count; i++)
            {
                if (match[i].CurrentCellId == gemCell.ID)
                {
                    return true;
                }
            }
            return false;
        }
        private bool MatchContainsType(List<Gem> matchList, MatchSFXType matchSFXType)
        {
            foreach(Gem gem in matchList)
            {
                if ((int)gem.Type == (int)matchSFXType) return true;
            }

            return false;
        }
    }
} 