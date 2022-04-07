using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiskoWiiyaas.MWAnimator.Animations
{
    [RequireComponent(typeof(ScriptAnimator))]
    public class MovePositionAnimation : MWAnimation
    {
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Vector3 endPosition;

        private void Start()
        {
            transform.position = startPosition;
        }

        public override IEnumerator DoAnimation()
        {
            _animationCompleted = false;

            yield return new WaitForSeconds(timeStartDelaySecs);

            float timeStart = Time.time;

            while (Vector3.Distance(transform.position, endPosition) > lerpThreshold)
            {
                float timeSinceStart = Time.time - timeStart;
                float completed = timeSinceStart / (animationSpeedInMS * Time.deltaTime);

                transform.position = Vector3.Lerp(transform.position, endPosition, Mathf.SmoothStep(0, 1, completed));
                yield return null;
            }

            transform.position = endPosition;
            _animationCompleted = true;
        }
    }
}
