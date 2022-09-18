using UnityEngine;
using MiiskoWiiyaas.Grid;


namespace MiiskoWiiyaas.Core
{
    /// <summary>
    /// Checks for potential matches in the grid without triggering move animations.
    /// Not to be confused with MatchFinder which checks for matches after a swap has occurred.
    /// </summary>
    public class MatchChecker: MonoBehaviour
    {
        private GameGrid<GemCell> grid;
        private Switcher switcher;

        public bool PotentialMatchesExist { get => CheckPotentialMatches(); }

        /// <summary>
        /// Sets up the MatchFinder
        /// </summary>
        /// <param name="grid">The main game grid</param>
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

        /// <summary>
        /// Checks to see if swaping the given cells would cause a match without
        /// activating swap animations.
        /// </summary>
        /// <param name="cell">The cell that exchanges its data with the other cell to see if a match would occur.</param>
        /// <param name="otherCell">The other cell that gets exchanged with the first cell.</param>
        /// <returns>A boolean indicating whether a swapping of cells would cause a match.</returns>
        public bool SwitchWouldCauseMatch(GemCell cell, GemCell otherCell)
        {
            bool result = false;

            switcher.ExchangeGems(cell, otherCell);

            if (FindMatchAtCell(cell) || FindMatchAtCell(otherCell))
            {
                result = true;
            }

            switcher.ExchangeGems(cell, otherCell);

            return result;
        }


    }
}

