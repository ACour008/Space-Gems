using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class EffectUtils
{
    public static Graphic getGraphicFromButton(Button button)
    {
        return button.transform.GetChild(0).GetComponent<Graphic>();
    }
}
