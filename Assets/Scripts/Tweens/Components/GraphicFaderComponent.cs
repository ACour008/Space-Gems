using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Tweens;

public class GraphicFaderComponent : MonoBehaviour
{
    [SerializeField] private Graphic graphic;
    [SerializeField] private float durationInSeconds;
    [SerializeField] private float endValue;
    [SerializeField] private float startDelayInSeconds;

    private EffectBuilder effectBuilder;


    private void Start()
    {
        EffectData<float> tweenData = new EffectData<float>() { durationInSecs = durationInSeconds, endValue = endValue, startDelayInSecs = startDelayInSeconds };
        effectBuilder = new EffectBuilder(this)
            .AddEffect(new Fade(graphic, tweenData))
            .ExecuteAll();
    }


}
