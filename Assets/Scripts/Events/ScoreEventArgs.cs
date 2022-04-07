using System.Collections.Generic;
using System;
using MiskoWiiyaas.Tiles;
using MiskoWiiyaas.Cells;

namespace MiskoWiiyaas.Events
{
    public class ScoreEventArgs : EventArgs
    {
        public List<Tile> matches;
        public GridCell cell;
        public int score;
    }
}
