
namespace MiiskoWiiyaas.Core
{
    public class Switcher
    {
        /// <summary>
        /// Exchanges the GemCell data of each cell without triggering a Move
        /// animation.
        /// </summary>
        /// <param name="current">The first cell that exchanges data with the other cell.</param>
        /// <param name="other">The other cell that exchanges data with the first cell.</param>
        public void ExchangeGems(GemCell current, GemCell other)
        {
            Gem temp = current.CurrentGem;
            current.CurrentGem = other.CurrentGem;
            other.CurrentGem = temp;

            int tempId = current.CurrentGem.CurrentCellId;
            current.CurrentGem.CurrentCellId = other.CurrentGem.CurrentCellId;
            other.CurrentGem.CurrentCellId = tempId;
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
        /// Tests if two gems switching places would cause a match.
        /// </summary>
        /// <param name="current">The first cell that exchanges data with the other cell.</param>
        /// <param name="other">The other cell that exchanges data with the first cell.</param>
        /// <returns>True if a switch would cause a match, false if not.</returns>
        public bool SwitchWouldCauseMatch(GemCell current, GemCell other)
        {
            bool result = false;

            ExchangeGems(current, other);

            if (FindMatchAtCell(current) || FindMatchAtCell(other))
            {
                result = true;
            }

            ExchangeGems(current, other);

            return result;
        }
    }

}