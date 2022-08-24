using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Tweens
{
    public class Fade : IEffect
    {
        private ILerp _lerper;

        public event EventHandler OnEffectStarted;
        public event EventHandler OnEffectCompleted;

        public bool EffectPlaying { get => _lerper.IsComplete; }

        public Fade(CanvasGroup canvasGroup, EffectData<float> data)
        {
            _lerper = new FloatLerper()
                .Init(() => canvasGroup.alpha, (x) => canvasGroup.alpha = x, data.endValue, data.durationInSecs, data.startDelayInSecs);
        }

        public Fade(Graphic graphic, EffectData<float> data)
        {
            Color endColor = new Color(graphic.color.r, graphic.color.g, graphic.color.b, data.endValue);

            _lerper = new ColorLerper()
                .Init(() => graphic.color, (x) => graphic.color = x, endColor, data.durationInSecs, data.startDelayInSecs);
        }

        public Fade(SpriteRenderer spriteRenderer, EffectData<float> data)
        {
            Color endColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, data.endValue);
            _lerper = new ColorLerper()
                .Init(() => spriteRenderer.color, (x) => spriteRenderer.color = x, endColor, data.durationInSecs, data.startDelayInSecs);
        }

        public Fade(Button button, EffectData<float> data)
        {
            Graphic graphic = EffectUtils.getGraphicFromButton(button);

            Color endColor = new Color(graphic.color.r, graphic.color.g, graphic.color.b, data.endValue);

            _lerper = new ColorLerper()
                .Init(() => graphic.color, (x) => graphic.color = x, endColor, data.durationInSecs, data.startDelayInSecs);
        }

        public void Execute(MonoBehaviour owner)
        {
            if (_lerper == null) Debug.Log("lerper is not found");

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