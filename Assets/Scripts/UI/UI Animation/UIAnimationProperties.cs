using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UI Animation Properties", menuName = "UI Animations/UI Animation Properties")]
public class UIAnimationProperties : ScriptableObject
{
    public Color startColor;
    public Color endColor;
    public Vector3 startPosition;
    public Vector3 movePositionBy;
    public float durationInSeconds;
    public float startDelayInSeconds;
}
