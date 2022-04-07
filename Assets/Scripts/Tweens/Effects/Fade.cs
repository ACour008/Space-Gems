using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tweens
{
    public class Fade : IEffect
    {
        private ILerp _lerper;

        public bool EffectPlaying { get => _lerper.IsComplete; }

        public Fade(CanvasGroup canvasGroup, EffectData data)
        {
            _lerper = new FloatLerper()
                .Init(() => canvasGroup.alpha, (x) => canvasGroup.alpha = x, data.endValue, data.durationInSecs, data.startDelayInSecs);
        }

        public Fade(Graphic graphic, EffectData data)
        {
            Color endColor = new Color(graphic.color.r, graphic.color.g, graphic.color.b, data.endValue);

            _lerper = new ColorLerper()
                .Init(() => graphic.color, (x) => graphic.color = x, endColor, data.durationInSecs, data.startDelayInSecs);
        }

        public Fade(Renderer renderer, EffectData data)
        {

        }

        public Fade(Button button, EffectData data)
        {
            Graphic graphic = EffectUtils.getGraphicFromButton(button);

            Color endColor = new Color(graphic.color.r, graphic.color.g, graphic.color.b, data.endValue);

            _lerper = new ColorLerper()
                .Init(() => graphic.color, (x) => graphic.color = x, endColor, data.durationInSecs, data.startDelayInSecs);
        }

        private void _lerper_onLerpComplete(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Execute(MonoBehaviour owner)
        {
            if (_lerper == null) Debug.Log("lerper is not found");
            owner.StartCoroutine(_lerper.StartLerping());
        }
    }
}