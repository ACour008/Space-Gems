using System.Collections.Generic;
using UnityEngine;
using MiiskoWiiyaas.Core;
using MiiskoWiiyaas.Audio;


namespace MiiskoWiiyaas.Utils
{
    public static class GameGridUtils
    {
        public static bool MatchContains(List<Gem> match, GemCell gemCell)
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

        public static bool MatchContainsType(List<Gem> matchList, MatchSFXType matchSFXType)
        {
            foreach (Gem gem in matchList)
            {
                if ((int)gem.Type == (int)matchSFXType) return true;
            }

            return false;
        }
    }
}
