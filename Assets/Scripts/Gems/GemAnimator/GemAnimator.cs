using UnityEngine;
using Tweens;

namespace MiiskoWiiyaas.Core
{
    public abstract class GemAnimator
    {
        protected Gem gem;
        protected EffectBuilder effectBuilder;
        public bool animationCompleted = true;

        public bool Completed { get => animationCompleted; }

        public Gem Gem { get => gem; }

        /// <summary>
        /// Makes the current gem disappear from the game grid.
        /// </summary>
        /// <param name="currentCell">The cell that the gem is associated with.</param>
        /// <param name="selfOnly">For power gems. If selfOnly is true, only the Gem will disappear,
        /// otherwise its neighbors will disappear as well, according to the type of GemAnimator.</param>
        /// <returns>The animation runtime in seconds.</returns>
        public abstract float Disappear(GemCell currentCell, bool selfOnly);

        /// <summary>
        /// Moves the gem to a specified position
        /// </summary>
        /// <param name="targetPosition">The position that the gem should move to.</param>
        /// <returns>The animation runtime in seconds.</returns>
        public float Move(Vector3 targetPosition)
        {
            // effectBuilder.ClearAllEffects();

            EffectData<Vector3> effectData = new EffectData<Vector3>(targetPosition, 0.25f, 0f);

            effectBuilder
                .AddEffect(new Move(gem.transform, effectData))
                .ExecuteAll();

            return effectData.Duration + effectData.StartDelay;
        }
    }

}