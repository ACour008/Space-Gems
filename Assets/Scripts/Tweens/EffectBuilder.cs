using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBuilder
{
    private List<IEffect> effects = new List<IEffect>();
    private bool effectsPlaying = false;
    private int coroutinesRunning = 0;

    public bool EffectsPlaying { get => effectsPlaying; }
    public int CurrentAnimations { get => effects.Count; }

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
        effect.OnEffectCompleted += Effect_OnEffectCompleted;
        return this;
    }

    public void ClearAllEffects()
    {
        foreach (IEffect effect in effects)
        {
            effect.OnEffectCompleted -= Effect_OnEffectCompleted;
        }

        effects.Clear();
    }

    private void Effect_OnEffectCompleted(object sender, EventArgs e)
    {
        coroutinesRunning--;
    }

    public EffectBuilder ExecuteAll()
    {
        OnExecutionStarted?.Invoke(this, EventArgs.Empty);

        coroutinesRunning = 0;
        
        Owner.StopAllCoroutines();
        Owner.StartCoroutine(ExecuteEffects());

        return this;
    }

    private IEnumerator ExecuteEffects()
    {
        foreach (IEffect effect in effects)
        {
            effect.Execute(Owner);
            coroutinesRunning++;
        }

        yield return new WaitUntil(() => coroutinesRunning <= 0);
        OnExecutionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public void StopEffects()
    {
        Owner.StopAllCoroutines();
    }
}
