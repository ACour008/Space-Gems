using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public class SpriteFaderComponent : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float durationInSeconds;
    [SerializeField] private float endValue;
    [SerializeField] private float startDelayInSeconds;

    private EffectBuilder effectBuilder;

    private void Start()
    {
        EffectData<float> effectData = new EffectData<float>() { durationInSecs = durationInSeconds, endValue = endValue, startDelayInSecs = startDelayInSeconds };

        effectBuilder = new EffectBuilder(this)
            .AddEffect(new Fade(spriteRenderer, effectData))
            .ExecuteAll();
    }
}
