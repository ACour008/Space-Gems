using System;
using MiiskoWiiyaas.Scoring;

namespace MiiskoWiiyaas.Core.Events
{
    public class BonusEventArgs : EventArgs
    {
        public GemCell chosenCell;
        public BonusType bonusType;
        public int score;
    }
}
