using System;
using UnityEngine;
using MiiskoWiiyaas.Audio;

namespace MiiskoWiiyaas.Core.Events
{
    public class SFXEventArgs : EventArgs
    {
        public MatchSFXType sfxType;
        public float startDelaySeconds;
        public int swapState;
        public int matchCount;
    }
}