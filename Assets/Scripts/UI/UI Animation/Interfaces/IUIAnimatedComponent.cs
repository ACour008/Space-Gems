using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IUIAnimatedComponent
{
    public GameObject GameObject { get; }

    public void Initialize(ObjectPool<IUIAnimatedComponent> objectPool, UIAnimationProperties properties);

    public void ReturnTo(Transform otherParent);

    public void Run(Transform parent, string uiText);
}
