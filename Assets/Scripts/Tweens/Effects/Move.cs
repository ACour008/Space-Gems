using System;
using System.Collections;
using UnityEngine;

namespace Tweens
{
    public class Move : IEffect
    {
        private ILerp _lerper;

        public event EventHandler OnEffectStarted;
        public event EventHandler OnEffectCompleted;

        public bool EffectPlaying => !_lerper.IsComplete;

        public Move(Transform transform, EffectData<Vector3> effectData)
        {
            _lerper = new Vector3Lerper()
                .Init(() => transform.position, (pos) => transform.position = pos, effectData.endValue, effectData.durationInSecs, effectData.startDelayInSecs);
        }

        public void Execute(MonoBehaviour owner)
        {
            OnEffectStarted?.Invoke(this, EventArgs.Empty);

            owner.StartCoroutine(SendCompleteMessage());
            owner.StartCoroutine(_lerper.StartLerping());
        }

        private IEnumerator SendCompleteMessage()
        {
            yield return new WaitUntil(() => _lerper.IsComplete);
            OnEffectCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}