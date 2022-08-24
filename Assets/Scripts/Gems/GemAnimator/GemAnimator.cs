using UnityEngine;
using MiiskoWiiyaas.Core;
using Tweens;

public abstract class GemAnimator
{
    protected Gem gem;
    protected EffectBuilder effectBuilder;
    public bool animationCompleted = true;

    public bool Completed { get => animationCompleted; }

    public Gem Gem { get => gem; }

    public abstract float Disappear(GemCell currentCell, bool selfOnly);

    public float Move(Vector3 targetPosition)
    {
        effectBuilder.ClearAllEffects();

        EffectData<Vector3> effectData = new EffectData<Vector3>() { endValue = targetPosition, durationInSecs = 0.25f, startDelayInSecs = 0f };
        effectBuilder
            .AddEffect(new Move(gem.transform, effectData))
            .ExecuteAll();

        return effectData.durationInSecs + effectData.startDelayInSecs;
    }
}
