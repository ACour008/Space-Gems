using System;
using System.Collections.Generic;

namespace MiiskoWiiyaas.Core.Events
{
    public class MatchEventArgs : EventArgs
    {
        public List<Gem> matches;
        public GemCell scoreCell;
        public int powerUpCellId;
        public bool firstSearch;
    }
}
