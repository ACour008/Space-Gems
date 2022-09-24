using UnityEngine;
using Tweens;

public class CanvasGroupFadeInComponent : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] float fadeTimeSeconds;
    [SerializeField] float startDelaySeconds;
    [SerializeField] bool executeFadeAtStart;
    private EffectBuilder builder;

    public float FadeTime { get => fadeTimeSeconds; }
    public float StartDelay { get => startDelaySeconds; }

    private void Awake()
    {
        builder = CreateEffectBuilder();

        builder.OnExecutionCompleted += (_, _) => canvasGroup.blocksRaycasts = false;
    }

    private void Start()
    {
        if (executeFadeAtStart) builder.ExecuteAll();
    }

    public void Fade()
    {
        canvasGroup.alpha = 1;
        builder.ExecuteAll();
    }

    private EffectBuilder CreateEffectBuilder()
    {
        EffectData<float> data = new EffectData<float>(0f, fadeTimeSeconds, startDelaySeconds);
        EffectBuilder builder = new EffectBuilder(this)
            .AddEffect(new Fade(canvasGroup, data));

        return builder;
    }
}
