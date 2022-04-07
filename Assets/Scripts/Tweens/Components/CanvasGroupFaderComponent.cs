using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public class CanvasGroupFaderComponent : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private EffectData tweenData;
    private EffectBuilder builder;

    void Start()
    {
        builder = new EffectBuilder(this)
            .AddEffect(new Fade(canvasGroup, tweenData))
            .ExecuteAll();
    }

    // Update is called once per frame
    void Update()
    {
        canvasGroup.blocksRaycasts = !(canvasGroup.alpha == 0);
    }
}
