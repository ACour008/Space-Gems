using UnityEngine;
using Tweens;
using MiiskoWiiyaas.Utils;
using MiiskoWiiyaas.Core;


public class NormalGemAnimator : GemAnimator
{

    /// <summary>
    /// A constructor for NormalGemAnimator that is meant for non-power Gems.
    /// </summary>
    /// <param name="gem">The Gem associated with the GemAnimator</param>
    public NormalGemAnimator(Gem gem)
    {
        this.gem = gem;
        this.effectBuilder = new EffectBuilder(this.gem);

        effectBuilder.OnExecutionStarted += (_, _) => animationCompleted = false;
        effectBuilder.OnExecutionCompleted += (_, _) => animationCompleted = true;
    }

    /// <summary>
    /// Removes the gem from the game grid.
    /// </summary>
    /// <param name="currentCell">The cell that the gem is associated with.</param>
    /// <param name="selfOnly">For power gems. If selfOnly is true, only the Gem will disappear,
    /// otherwise its neighbors will disappear as well, according to the type of GemAnimator.</param>
    /// <returns></returns>
    public override float Disappear(GemCell currentCell, bool selfOnly = true)
    {
        effectBuilder.ClearAllEffects();

        EffectData<Vector3> scaleUp = new EffectData<Vector3>() { endValue = new Vector3(1.25f, 1.25f, 1), durationInSecs = 0.25f, startDelayInSecs = 0f };
        EffectData<Vector3> scaleDown = new EffectData<Vector3>() { endValue = new Vector3(0.1f, 0.1f, 1), durationInSecs = 0.25f, startDelayInSecs = 0.25f };
        EffectData<Vector3> rotate = new EffectData<Vector3>() { endValue = new Vector3(0, 0, -720), durationInSecs = 0.5f, startDelayInSecs = 0.25f };

        effectBuilder
            .AddEffect(new Scale(gem.transform, scaleUp))
            .AddEffect(new Scale(gem.transform, scaleDown))
            .AddEffect(new Rotate(gem.transform, rotate));

        effectBuilder.ExecuteAll();

        gem.StartCoroutine(GameObjectUtils.DestroyAfterSeconds(gem.gameObject, 0.5f));

        return rotate.durationInSecs;
    }
}
