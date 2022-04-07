using System.Collections;
using UnityEngine;

namespace Tweens
{
    public class Move : IEffect
    {
        private ILerp _lerper;

        public bool EffectPlaying => _lerper.IsComplete;

        public void Execute(MonoBehaviour owner)
        {

        }
    }
}