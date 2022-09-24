using UnityEngine;
using Tweens;

public class CanvasGroupFadeOutComponent : MonoBehaviour
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

        builder.OnExecutionStarted += (_, _) => canvasGroup.blocksRaycasts = true;
    }

    private void Start()
    {
        if (executeFadeAtStart) builder.ExecuteAll();
    }

    public void Fade()
    {
        builder.ExecuteAll();
    }

    private EffectBuilder CreateEffectBuilder()
    {
        EffectData<float> data = new EffectData<float>(fadeTimeSeconds, 1f, startDelaySeconds);
        EffectBuilder builder = new EffectBuilder(this)
            .AddEffect(new Fade(canvasGroup, data));

        return builder;
    }
}
