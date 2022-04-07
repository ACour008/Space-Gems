using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tweens;

public class ButtonFaderComponent : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private EffectData effectData;
    private EffectBuilder builder;
    private Graphic buttonText;

    // Start is called before the first frame update
    void Start()
    {
        buttonText = EffectUtils.getGraphicFromButton(button);

        builder = new EffectBuilder(this)
            .AddEffect(new Fade(button, effectData))
            .ExecuteAll();
    }

    // Update is called once per frame
    void Update()
    {
        button.interactable = (buttonText.color.a < 1);
    }
}
