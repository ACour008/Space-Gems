using System;
using System.Collections;
using UnityEngine;

public interface IEffect
{
    public bool EffectPlaying { get; }
    public void Execute(MonoBehaviour owner);
}
