using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using Tweens;

public abstract class UIAnimatedComponent : MonoBehaviour, IUIAnimatedComponent, IEffectGenerator
{
    [SerializeField] protected TextMeshProUGUI scoreText;

    private EffectBuilder effectBuilder;
    private ObjectPool<IUIAnimatedComponent> pool;
    private UIAnimationProperties properties;
    protected RectTransform canvasRT;
    protected RectTransform scoreTextRT;
    protected CanvasGroup canvasGroup;

    public GameObject GameObject { get => gameObject; }

    #region Unity Messages
    void Awake()
    {
        canvasRT = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        scoreTextRT = scoreText.GetComponent<RectTransform>();
    }

    private void OnDestroy()
    {
        effectBuilder.OnExecutionCompleted -= EffectBuilder_OnExecutionCompleted;
    }

    #endregion

    #region Effect Builder Messages
    private void EffectBuilder_OnExecutionCompleted(object sender, System.EventArgs e)
    {
        this.pool.Release(this);
    }
    #endregion

    #region IUIAnimatedComponent implementation
    public void Initialize(ObjectPool<IUIAnimatedComponent> objectPool, UIAnimationProperties properties)
    {
        effectBuilder = new EffectBuilder(this);
        pool = objectPool;
        this.properties = properties;

        effectBuilder.OnExecutionCompleted += EffectBuilder_OnExecutionCompleted;
    }

    public void ReturnTo(Transform otherParent)
    {
        canvasRT.SetParent(otherParent, false);
    }

    public void Run(Transform parent, string uiText)
    {
        SetupObject(parent, uiText);
        Effect[] effects = GenerateEffects(properties);
        effectBuilder.AddEffects(effects).ExecuteAll();
    }
    #endregion

    #region Effect Generation
    public abstract Effect[] GenerateEffects(UIAnimationProperties properties);
    #endregion

    #region Helpers
    private void SetupObject(Transform parent, string uiText)
    {
        canvasRT.SetParent(parent, false);

        scoreTextRT.anchoredPosition3D = properties.startPosition;
        scoreText.text = uiText;
        scoreText.color = properties.startColor;
    }
    #endregion
}
