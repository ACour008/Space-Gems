using UnityEngine;
using UnityEngine.UI;
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
        EffectData<float> tweenData = new EffectData<float>(endValue, durationInSeconds, startDelayInSeconds);
        effectBuilder = new EffectBuilder(this)
            .AddEffect(new Fade(graphic, tweenData))
            .ExecuteAll();
    }


}
