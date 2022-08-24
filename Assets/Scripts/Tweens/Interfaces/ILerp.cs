using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILerp
{
    public Type LerpType { get; }
    public bool IsComplete { get; }

    public IEnumerator StartLerping();
}
