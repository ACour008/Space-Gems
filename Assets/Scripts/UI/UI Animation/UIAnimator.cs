using UnityEngine;
using UnityEngine.Pool;
using Tweens;

// Could remove MonoBehaviour, and inject prefab, poolSizes and properties from the BonusUIManager
// but lets leave as is for now.
public class UIAnimator : MonoBehaviour
{
    [SerializeField] private UIAnimatedComponent prefab;
    [SerializeField] private Transform parent;
    [SerializeField] private int defaultPoolSize;
    [SerializeField] private int maxPoolSize;
    [SerializeField] private UIAnimationProperties effectProperties;

    private int getCount = 0;

    private ObjectPool<UIAnimatedComponent> pool;

    // Own class?
    private EffectBuilder BuildEffect(UIAnimatedComponent uiObject)
    {
        MonoBehaviour owner = uiObject.GetComponent<MonoBehaviour>();
        RectTransform canvasRT = uiObject.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = uiObject.GetComponent<CanvasGroup>();

        Vector3 moveTarget = effectProperties.startPosition + effectProperties.movePositionBy;
        float floatTarget = effectProperties.endColor.a;

        EffectData<Vector3> moveData = new EffectData<Vector3>(moveTarget, effectProperties.durationInSeconds, effectProperties.startDelayInSeconds);
        EffectData<float> fadeData = new EffectData<float>(floatTarget, effectProperties.durationInSeconds, effectProperties.startDelayInSeconds);
      
        EffectBuilder eb = new EffectBuilder(owner);

        eb.AddEffect(new Move(canvasRT, moveData))
            .AddEffect(new Fade(canvasGroup, fadeData));

        return eb;
    }

    private void Awake()
    {
        pool = new ObjectPool<UIAnimatedComponent>(CreateUIObject, collectionCheck: true, defaultCapacity: defaultPoolSize, maxSize: maxPoolSize);
    }

    private UIAnimatedComponent CreateUIObject()
    {
        Debug.Log("Creating UI Animated Component");

        UIAnimatedComponent uiComponent = Instantiate<UIAnimatedComponent>(prefab, parent, false);
        EffectBuilder effectBuilder = BuildEffect(uiComponent);

        uiComponent.GetComponent<UIAnimatedComponent>()
            .Initialize(effectBuilder, this.pool, parent);

        return uiComponent;
    }

    public void StartUIAnimation(int scoreValue, Transform newParent = null)
    {
        UIAnimatedComponent uiComponent = pool.Get();

        if (newParent != null)
        {
            uiComponent.transform.SetParent(newParent, false);
        }
        
        uiComponent.Run(effectProperties.startPosition, effectProperties.startColor, scoreValue.ToString());
    }
}
