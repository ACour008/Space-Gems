using UnityEngine;
using MiiskoWiiyaas.Grid;


namespace MiiskoWiiyaas.Core
{
    public class MatchChecker: MonoBehaviour
    {
        private GameGrid<GemCell> grid;
        private Switcher switcher;

        public bool PotentialMatchesExist { get => CheckPotentialMatches(); }

        public void Initialize(GameGrid<GemCell> grid)
        {
            this.grid = grid;
            switcher = new Switcher();
        }

        private bool CheckPotentialMatches()
        {
            for (int i = 1; i < grid.Cells.Length; i++)
            {
                GemCell current = grid.Cells[i];
                GemCell east = current.TryGetNeighbor("east");
                GemCell south = current.TryGetNeighbor("north");

                if (east != null && SwitchWouldCauseMatch(current, east))
                {
                    return true;
                }

                if (south != null && SwitchWouldCauseMatch(current, south))
                {
                    return true;
                }
            }

            return false;
        }

        private bool FindDirectionalMatch(GemCell cell, string direction)
        {
            GemCell neighbor = cell.TryGetNeighbor(direction);
            GemCell nextNeighbor = neighbor?.TryGetNeighbor(direction);

            if (neighbor != null && nextNeighbor != null)
            {
                bool gemsMatch = neighbor.CurrentGem.Color == cell.CurrentGem.Color &&
                    nextNeighbor.CurrentGem.Color == cell.CurrentGem.Color;

                if (gemsMatch) return true;
            }

            return false;
        }

        private bool FindMatchAtCell(GemCell cell)
        {
            bool middleMatchFound = FindMiddleMatch(cell);
            bool northMatchFound = FindDirectionalMatch(cell, "north");
            bool southMatchFound = FindDirectionalMatch(cell, "south");
            bool eastMatchFound = FindDirectionalMatch(cell, "east");
            bool westMatchFound = FindDirectionalMatch(cell, "west");

            return northMatchFound || southMatchFound || eastMatchFound ||
                   westMatchFound || middleMatchFound;
        }

        private bool FindMiddleMatch(GemCell cell)
        {
            GemCell left = cell.TryGetNeighbor("east");
            GemCell right = cell.TryGetNeighbor("west");

            if (left?.CurrentGem.Color == cell.CurrentGem.Color &&
                right?.CurrentGem.Color == cell.CurrentGem.Color)
            {
                return true;
            }

            GemCell up = cell.TryGetNeighbor("north");
            GemCell down = cell.TryGetNeighbor("south");

            if (up?.CurrentGem.Color == cell.CurrentGem.Color &&
                down?.CurrentGem.Color == cell.CurrentGem.Color)
            {
                return true;
            }

            return false;
        }

        public bool SwitchWouldCauseMatch(GemCell cell1, GemCell cell2)
        {
            bool result = false;

            switcher.ExchangeGems(cell1, cell2);

            if (FindMatchAtCell(cell1) || FindMatchAtCell(cell2))
            {
                result = true;
            }

            switcher.ExchangeGems(cell1, cell2);

            return result;
        }


    }
}

