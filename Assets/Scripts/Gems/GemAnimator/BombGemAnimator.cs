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

    /// <summary>
    /// Removes the Gem associated with the Animator from the game grid.
    /// </summary>
    /// <param name="currentCell">The GemCell object associated with the cell to disappear.</param>
    /// <param name="selfOnly">For power gems. If selfOnly is true, only the Gem will disappear,
    /// otherwise its neighbors will disappear as well, according to the type of GemAnimator.</param>
    /// <returns>The animation runtime in seconds.</returns>
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
