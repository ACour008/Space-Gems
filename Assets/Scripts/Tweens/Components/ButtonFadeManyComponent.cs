using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tweens;

public class ButtonFadeManyComponent : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;
    [SerializeField] private float durationInSeconds;
    [SerializeField] private float endValue;
    [SerializeField] private float startDelayInSeconds;

    private List<EffectBuilder> effectBuilders = new List<EffectBuilder>();

    // Start is called before the first frame update
    void Awake()
    {
        foreach(Button button in buttons)
        {
            MonoBehaviour owner = button.GetComponent<MonoBehaviour>();
            EffectData<float> tweenData = new EffectData<float>() { durationInSecs = durationInSeconds, endValue = endValue, startDelayInSecs = startDelayInSeconds };
            
            EffectBuilder builder = new EffectBuilder(owner)
                .AddEffect(new Fade(button, tweenData));

            builder.OnExecutionStarted += Builder_OnExecutionStarted;
            builder.OnExecutionCompleted += Builder_OnExecutionCompleted;
            effectBuilders.Add(builder);
        }    
    }

    private void Builder_OnExecutionStarted(object sender, EventArgs e)
    {
        EffectBuilder builder = sender as EffectBuilder;

        Debug.Log(builder.Owner as Button);

        Button button = builder.Owner as Button;

        button.interactable = false;
    }

    private void Builder_OnExecutionCompleted(object sender, EventArgs e)
    {
        EffectBuilder builder = sender as EffectBuilder;
        Button button = builder.Owner as Button;

        button.interactable = true;
    }

    private void Start()
    {
        foreach(EffectBuilder builder in effectBuilders)
        {
            builder.ExecuteAll();
        }
    }
}
