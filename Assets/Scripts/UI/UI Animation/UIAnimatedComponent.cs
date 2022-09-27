using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using Tweens;

// LETS CREATE 2 INTERFACES: IUIAnimatedComponent & IEffectGenerator
// IUIAnimatedComponent: Initialize, Run & SetParent.
// IEffectGenerator: GenerateEffects (maybe a part of Tween Daddy??)

public class UIAnimatedComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private EffectBuilder effectBuilder;
    private ObjectPool<UIAnimatedComponent> pool;
    private UIAnimationProperties properties;
    private RectTransform canvasRT;
    private RectTransform scoreTextRT;

    public TextMeshProUGUI ScoreText { get => scoreText; }

    private void Awake()
    {
        canvasRT = GetComponent<RectTransform>();
        scoreTextRT = scoreText.GetComponent<RectTransform>();
    }

    private void EffectBuilder_OnExecutionCompleted(object sender, System.EventArgs e)
    {
        this.pool.Release(this);
    }

    public void Initialize(ObjectPool<UIAnimatedComponent> pool, UIAnimationProperties properties)
    {
        this.effectBuilder = new EffectBuilder(this);
        this.pool = pool;
        this.properties = properties;

        effectBuilder.OnExecutionCompleted += EffectBuilder_OnExecutionCompleted;
    }

    private void OnDestroy()
    {
        effectBuilder.OnExecutionCompleted -= EffectBuilder_OnExecutionCompleted;
    }

    public void Run(Transform parent, string uiText)
    {
        SetupObject(parent, uiText); // could we do this at the onGet
        effectBuilder.AddEffects(GenerateEffects()).ExecuteAll();
    }

    private Effect[] GenerateEffects()
    {
        // REDUNDANT. REMOVE FROM TWEEN-DADDY
        EffectData<Vector3> moveData = new EffectData<Vector3>(properties.endPosition, properties.durationInSeconds, properties.startDelayInSeconds);
        EffectData<float> fadeData = new EffectData<float>(properties.endColor.a, properties.durationInSeconds, properties.startDelayInSeconds);

        return new Effect[2]
        {
            new Move(scoreTextRT, moveData),
            new Fade(scoreText, fadeData)
        };
    }

    private void SetupObject(Transform parent, string uiText)
    {
        canvasRT.SetParent(parent, false);

        scoreTextRT.anchoredPosition3D = properties.startPosition;
        scoreText.text = uiText;
        scoreText.color = properties.startColor;
    }

    public void SetParent(Transform parent)
    {
        canvasRT.SetParent(parent, false);
    }
}
