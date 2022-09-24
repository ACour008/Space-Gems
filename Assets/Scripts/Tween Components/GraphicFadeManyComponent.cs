using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tweens;

public class GraphicFadeManyComponent : MonoBehaviour
{
    [SerializeField] private List<Graphic> graphics;
    [SerializeField] private EffectData<float> tweenData;
    private List<EffectBuilder> builders = new List<EffectBuilder>();

    private void Awake()
    {
        foreach(Graphic graphic in graphics)
        {
            EffectBuilder builder = new EffectBuilder(this)
                .AddEffect(new Fade(graphic, tweenData));

            builders.Add(builder);
        }
    }

    private void Start()
    {
        foreach(EffectBuilder builder in builders)
        {
            builder.ExecuteAll();
        }
    }
}
