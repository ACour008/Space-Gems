using UnityEngine;
using Tweens;
using MiiskoWiiyaas.Utils;


namespace MiiskoWiiyaas.Core
{
    public class NormalGemAnimator : GemAnimator
    {
        public NormalGemAnimator(Gem gem)
        {
            this.gem = gem;
            this.effectBuilder = new EffectBuilder(this.gem);

            effectBuilder.OnExecutionStarted += EffectBuilder_OnExecutionStarted;
            effectBuilder.OnExecutionCompleted += EffectBuilder_OnExecutionCompleted;
        }

        private void EffectBuilder_OnExecutionCompleted(object sender, System.EventArgs e)
        {
            this.animationCompleted = true;
        }

        private void EffectBuilder_OnExecutionStarted(object sender, System.EventArgs e)
        {
            this.animationCompleted = false;
        }

        /// <inheritdoc cref="GemAnimator.Disappear(GemCell, bool)"/>
        public override float Disappear(GemCell currentCell, bool selfOnly = true)
        {
            EffectData<Vector3> scaleUp = new EffectData<Vector3>(new Vector3(1.25f, 1.25f, 1), durationInSeconds: 0.25f, startDelayInSeconds: 0f);
            EffectData<Vector3> scaleDown = new EffectData<Vector3>(new Vector3(0.1f, 0.1f, 1), durationInSeconds: 0.25f, startDelayInSeconds: 0.25f);
            EffectData<Vector3> rotate = new EffectData<Vector3>(new Vector3(0, 0, -720), durationInSeconds: 0.5f, startDelayInSeconds: 0.25f);

            effectBuilder
                .AddEffect(new Scale(gem.transform, scaleUp))
                .AddEffect(new Scale(gem.transform, scaleDown))
                .AddEffect(new Rotate(gem.transform, rotate));

            effectBuilder.ExecuteAll();

            gem.StartCoroutine(GameObjectUtils.DestroyAfterSeconds(gem.gameObject, 0.5f));

            return rotate.Duration;
        }
    }

}