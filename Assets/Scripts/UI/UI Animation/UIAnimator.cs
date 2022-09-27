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
    [SerializeField] private UIAnimationProperties effectProperties; // this is prob redundant too? Maybe.

    private ObjectPool<UIAnimatedComponent> pool;

    private void Awake()
    {
        pool = new ObjectPool<UIAnimatedComponent>(CreateUIComponent, OnGetUIComponent, OnReleaseUIComponent, 
            collectionCheck: true, defaultCapacity: defaultPoolSize, maxSize: maxPoolSize);
    }

    private UIAnimatedComponent CreateUIComponent()
    {
        UIAnimatedComponent uiComponent = Instantiate<UIAnimatedComponent>(prefab, parent, false);

        uiComponent.GetComponent<UIAnimatedComponent>()
            .Initialize(this.pool, effectProperties);

        return uiComponent;
    }

    public UIAnimatedComponent GetUIAnimatedComponent()
    {
        return pool.Get();
    }

    public void OnGetUIComponent(UIAnimatedComponent uiComponent)
    {
        uiComponent.gameObject.SetActive(true);
    }
    public void OnReleaseUIComponent(UIAnimatedComponent uiComponent)
    {
        uiComponent.SetParent(transform);
        uiComponent.gameObject.SetActive(false);
    }
}
