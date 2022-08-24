using UnityEngine;
using UnityEngine.UI;
using Tweens;

public class ButtonFaderComponent : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private float durationInSeconds;
    [SerializeField] private float endValue;
    [SerializeField] private float startDelayInSeconds;

    private EffectBuilder builder;
    private Graphic buttonText;

    void Start()
    {
        buttonText = EffectUtils.getGraphicFromButton(button);

        EffectData<float> effectData = new EffectData<float>() { durationInSecs = durationInSeconds, endValue = endValue, startDelayInSecs = startDelayInSeconds };

        builder = new EffectBuilder(this)
            .AddEffect(new Fade(button, effectData))
            .ExecuteAll();

        builder.OnExecutionStarted += (_, _) => button.interactable = false;
        builder.OnExecutionCompleted += (_, _) => button.interactable = true;
    }
}
