using UnityEngine;
using UnityEngine.Pool;

/* THIS SHOULD PROBABLY BE RENAMED.
 * HERE A FEW OPTIONS:
 *   - UIAnimatedObjectPooler
 *   - uh..
*/

public class UIAnimator : MonoBehaviour
{
    [SerializeField] private UIAnimatedComponent prefab;
    [SerializeField] private Transform parent;
    [SerializeField] private int defaultPoolSize;
    [SerializeField] private int maxPoolSize;
    [SerializeField] private UIAnimationProperties effectProperties; // this is prob redundant too? Maybe.

    private ObjectPool<IUIAnimatedComponent> pool;

    private void Awake()
    {
        pool = new ObjectPool<IUIAnimatedComponent>(CreateUIComponent, OnGetUIComponent, OnReleaseUIComponent, 
            collectionCheck: true, defaultCapacity: defaultPoolSize, maxSize: maxPoolSize);
    }

    private UIAnimatedComponent CreateUIComponent()
    {
        UIAnimatedComponent uiComponent = Instantiate<UIAnimatedComponent>(prefab, parent, false);

        uiComponent.GetComponent<UIAnimatedComponent>()
            .Initialize(this.pool, effectProperties);

        return uiComponent;
    }

    public IUIAnimatedComponent GetUIAnimatedComponent()
    {
        return pool.Get();
    }

    public void OnGetUIComponent(IUIAnimatedComponent uiComponent)
    {
        uiComponent.GameObject.SetActive(true);
    }
    public void OnReleaseUIComponent(IUIAnimatedComponent uiComponent)
    {
        uiComponent.ReturnTo(transform);
        uiComponent.GameObject.SetActive(false);
    }
}
