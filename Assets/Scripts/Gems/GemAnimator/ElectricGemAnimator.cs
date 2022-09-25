using UnityEngine;
using MiiskoWiiyaas.Core;
using MiiskoWiiyaas.Utils;
using Tweens;

namespace MiiskoWiiyaas.Core
{
    /// <summary>
    /// A subclass of GemAnimator that controls the movement of an Electric Power Gem.
    /// </summary>
    /// <seealso cref="GemAnimator"/>
    public class ElectricGemAnimator : GemAnimator
    {

        /// <summary>
        /// The constructor of ElectricGemAnimator
        /// </summary>
        /// <param name="gem">The gem associated with the Animator</param>
        public ElectricGemAnimator(Gem gem)
        {
            this.gem = gem;
            this.effectBuilder = new EffectBuilder(this.gem);

            effectBuilder.OnExecutionStarted += (_, _) => animationCompleted = false;
            effectBuilder.OnExecutionCompleted += (_, _) => animationCompleted = true;
        }

        /// <inheritdoc cref="GemAnimator.Disappear(GemCell, bool)"/>
        public override float Disappear(GemCell currentCell, bool selfOnly = true)
        {
            float runTime = DisappearSelf();
            currentCell.CurrentGem = null;

            if (!selfOnly)
            {
                DisappearDirection("north", currentCell);
                DisappearDirection("south", currentCell);
                DisappearDirection("east", currentCell);
                DisappearDirection("west", currentCell);
            }

            return runTime;
        }

        private void DisappearDirection(string direction, GemCell startCell)
        {
            GemCell current = startCell;
            while (current.TryGetNeighbor(direction) != null)
            {
                current = current.TryGetNeighbor(direction);
                if (current.CurrentGem != null)
                {
                    current.CurrentGem.Disappear(current, false);
                    current.CurrentGem = null;
                }
            }

        }

        private float DisappearSelf()
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
