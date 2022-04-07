using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MiskoWiiyaas.MWAnimator.Animations
{
    [RequireComponent(typeof(ScriptAnimator))]
    public class SpriteFadeInAnimation : MWAnimation
    {
        private SpriteRenderer spriteRenderer;
        [SerializeField] private Color startColor;
        [SerializeField] private Color endColor;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = startColor;
        }

        public override IEnumerator DoAnimation()
        {
            _animationCompleted = false;

            yield return new WaitForSeconds(timeStartDelaySecs);

            float timeStart = Time.time;

            while (spriteRenderer.color.a < (endColor.a - lerpThreshold))
            {
                float timeSinceStart = Time.time - timeStart;
                float completed = timeSinceStart / (animationSpeedInMS * Time.deltaTime);

                spriteRenderer.color = Color.Lerp(spriteRenderer.color, endColor, Mathf.SmoothStep(0, 1, completed));
                yield return null;
            }

            spriteRenderer.color = endColor;
            _animationCompleted = true;
        }
    }
}