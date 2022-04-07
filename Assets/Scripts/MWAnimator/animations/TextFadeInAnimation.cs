using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MiskoWiiyaas.MWAnimator.Animations
{
    [RequireComponent(typeof(ScriptAnimator))]
    public class TextFadeInAnimation : MWAnimation
    {
        private Graphic graphic;
        [SerializeField] private Color startColor;
        [SerializeField] private Color endColor;

        private void Start()
        {
            graphic = GetComponent<Graphic>();
            graphic.color = startColor;
        }

        public override IEnumerator DoAnimation()
        {
            _animationCompleted = false;

            yield return new WaitForSeconds(timeStartDelaySecs);
            
            float timeStart = Time.time;

            while (graphic.color.a < (endColor.a - lerpThreshold))
            {
                float timeSinceStart = Time.time - timeStart;
                float completed = timeSinceStart / (animationSpeedInMS * Time.deltaTime);

                graphic.color = Color.Lerp(graphic.color, endColor, Mathf.SmoothStep(0, 1, completed));
                yield return null;
            }

            graphic.color = endColor;
            _animationCompleted = true;
        }
    }
}