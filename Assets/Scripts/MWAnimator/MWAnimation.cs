using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MiskoWiiyaas.MWAnimator
{
    public abstract class MWAnimation : MonoBehaviour
    {
        [SerializeField] protected float timeStartDelaySecs;
        [SerializeField] protected float animationSpeedInMS = 500f;
        [SerializeField] protected float lerpThreshold = 0.05f;

        protected bool _animationCompleted;

        public bool Completed { get => _animationCompleted; }

        public abstract IEnumerator DoAnimation();

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}