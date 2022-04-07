using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBuilder
{
    private List<IEffect> effects = new List<IEffect>();

    private bool AllEffectsCompleted { get => CheckAllEffectsForCompletion(); }

    public event EventHandler OnExecutionCompleted;
    public event EventHandler OnExecutionStarted;

    public MonoBehaviour Owner { get; private set; }

    public EffectBuilder(MonoBehaviour owner)
    {
        Owner = owner;
    }

    public EffectBuilder AddEffect(IEffect effect)
    {
        effects.Add(effect);
        return this;
    }

    public EffectBuilder ExecuteAll()
    {
        Owner.StopAllCoroutines();
        OnExecutionStarted?.Invoke(this, EventArgs.Empty);

        foreach(IEffect effect in effects)
        {
            effect.Execute(Owner);
        }

        Owner.StartCoroutine(WaitTilCompletion());

        return this;
    }

    private IEnumerator WaitTilCompletion()
    {
        yield return new WaitUntil(() => AllEffectsCompleted);
        OnExecutionCompleted?.Invoke(this, EventArgs.Empty);

    }

    private bool CheckAllEffectsForCompletion()
    {
        foreach(IEffect effect in effects)
        {
            if (effect.EffectPlaying) return false;
        }

        return true;
    }
}
