
namespace MiiskoWiiyaas.Core
{
    public class Switcher
    {

        public Switcher() { }

        public void ExchangeGems(GemCell cell1, GemCell cell2)
        {
            Gem temp = cell1.CurrentGem;
            cell1.CurrentGem = cell2.CurrentGem;
            cell2.CurrentGem = temp;

            int tempId = cell1.CurrentGem.CurrentCellId;
            cell1.CurrentGem.CurrentCellId = cell2.CurrentGem.CurrentCellId;
            cell2.CurrentGem.CurrentCellId = tempId;
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

            ExchangeGems(cell1, cell2);

            if (FindMatchAtCell(cell1) || FindMatchAtCell(cell2))
            {
                result = true;
            }

            ExchangeGems(cell1, cell2);

            return result;
        }
    }

}