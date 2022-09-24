using System.Collections.Generic;
using UnityEngine;
using MiiskoWiiyaas.Core;
using MiiskoWiiyaas.Utils;
using Tweens;

/// <summary>
/// A subclass of GemAnimator that controls the movement of a Bomb Power Gem.
/// </summary>
/// <seealso cref="GemAnimator"/>
public class BombGemAnimator : GemAnimator
{
    /// <summary>
    /// The constructor of BombGemAnimator
    /// </summary>
    /// <param name="gem">The gem associated with the Animator</param>
    public BombGemAnimator(Gem gem)
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
            foreach (KeyValuePair<string, GemCell> kv in currentCell.GetAllNeighbors())
            {
                GemCell cell = kv.Value;

                if (cell.CurrentGem != null)
                {
                    cell.CurrentGem.Disappear(cell, false);
                    cell.CurrentGem = null;
                }
            }
        }

        return runTime;
    }

    private float DisappearSelf()
    {
        effectBuilder.ClearAllEffects();

        EffectData<Vector3> scaleUp = new EffectData<Vector3>(new Vector3(1.25f, 1.25f, 1), durationInSeconds:0.25f, startDelayInSeconds:0f );
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
