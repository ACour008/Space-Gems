using System.Collections;
using UnityEngine;

namespace MiskoWiiyaas.MWAnimator.Animations
{
    [RequireComponent(typeof(ScriptAnimator))]
    public class RotateAnimation : MWAnimation
    {
        [SerializeField] Vector3 degreesPerSecond;

        public override IEnumerator DoAnimation()
        {
            yield return new WaitForSeconds(timeStartDelaySecs);
            _animationCompleted = false;

            while (!_animationCompleted)
            {
                transform.Rotate(degreesPerSecond * Time.deltaTime);
                yield return null;
            }
        }
    }
}