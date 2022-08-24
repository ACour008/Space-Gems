using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public class MoveComponent : MonoBehaviour
{
    [SerializeField] private float durationInSeconds;
    [SerializeField] private float startDelayInSeconds;
    [SerializeField] private Vector3 endValue;

    private EffectBuilder effectBuilder;

    void Start()
    {
        EffectData<Vector3> effectData = new EffectData<Vector3>() { durationInSecs = durationInSeconds, endValue = endValue, startDelayInSecs = startDelayInSeconds };
        effectBuilder = new EffectBuilder(this)
            .AddEffect(new Move(this.transform, effectData))
            .ExecuteAll();
    }
}
