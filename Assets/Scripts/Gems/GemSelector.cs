using System;
using System.Collections.Generic;
using MiiskoWiiyaas.Utils;

namespace MiiskoWiiyaas.Core
{
    public class GemSelector
    {
        System.Random rdm;
        public GemSelector(System.Random random)
        {
            rdm = random;
        }

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
