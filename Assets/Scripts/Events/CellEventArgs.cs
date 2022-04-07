using System;
using MiskoWiiyaas.Tiles;
using MiskoWiiyaas.Cells;

namespace MiskoWiiyaas.Events
{
    public class CellEventArgs : EventArgs
    {
        public Tile tile;
        public GridCell cell;
        public bool shouldRunMatchCheck;
    }
}