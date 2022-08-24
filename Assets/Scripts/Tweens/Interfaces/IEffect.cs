using System;
using System.Collections;
using UnityEngine;

public interface IEffect
{
    public bool EffectPlaying { get; }

    public event EventHandler OnEffectStarted;
    public event EventHandler OnEffectCompleted;
    public void Execute(MonoBehaviour owner);
}
