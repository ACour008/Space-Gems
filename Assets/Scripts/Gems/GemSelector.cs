using System.Collections.Generic;
using MiiskoWiiyaas.Utils;

namespace MiiskoWiiyaas.Core
{
    public class GemSelector
    {
        System.Random rdm;

        /// <summary>
        /// The Constructor for GemSelector.
        /// </summary>
        /// <param name="random">A already initalized System.Random object</param>
        public GemSelector(System.Random random)
        {
            rdm = random;
        }

        /// <summary>
        /// Generates a <c>GemColor</c> to be associated to a Gem in a way that ensures
        /// no matches occur in the game grid.
        /// </summary>
        /// <param name="cellId">The id of the cell the gem is assigned to.</param>
        /// <param name="rows">The number of rows in the game grid.</param>
        /// <param name="cells">The array of GemCells that represents the game grid.</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>A semi-randomized GemColor</returns>
        /// 
        /// <seealso cref="Gem"/>
        /// <seealso cref="GemColor"/>
        public GemColor GetRandomColor(int cellId, int rows, GemCell[] cells, int start, int end)
        {

            List<GemColor> gemTypes = EnumUtils.GetValuesFrom<GemColor>(start, end);

            bool hasTwoWestNeighbors = cellId >= 2;
            bool hasTwoSouthNeighbors = cellId > rows * 2;

            if (hasTwoWestNeighbors)
            {
                GemCell cell1 = cells[cellId - 1];
                GemCell cell2 = cells[cellId - 2];

                if (cell1.CurrentGem != null && cell2.CurrentGem != null)
                {
                    GemColor firstWestNeighborType = cells[cellId - 1].CurrentGem.Color;
                    GemColor secondWestNeighborType = cells[cellId - 2].CurrentGem.Color;

                    if (firstWestNeighborType == secondWestNeighborType)
                    {
                        gemTypes.Remove(firstWestNeighborType);
                    }
                }
            }
            
            if (hasTwoSouthNeighbors)
            {
                GemCell cell1 = cells[cellId - rows];
                GemCell cell2 = cells[cellId - (rows * 2)];

                if (cell1.CurrentGem != null & cell2.CurrentGem != null)
                {
                    GemColor firstSouthNeighborType = cells[cellId - rows].CurrentGem.Color;
                    GemColor secondSouthNeighorType = cells[cellId - (rows * 2)].CurrentGem.Color;

                    if (firstSouthNeighborType == secondSouthNeighorType)
                    {
                        gemTypes.Remove(firstSouthNeighborType);
                    }
                }
            }

            return gemTypes[rdm.Next(gemTypes.Count)];
        }
    }
}
