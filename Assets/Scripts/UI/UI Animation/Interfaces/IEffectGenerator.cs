using Tweens;
using UnityEngine;

public interface IEffectGenerator
{
    public Effect[] GenerateEffects(UIAnimationProperties properties);
}
