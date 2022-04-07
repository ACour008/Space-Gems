using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Tweens;

public class GraphicFaderComponent : MonoBehaviour
{
    [SerializeField] private Graphic graphic;
    [SerializeField] private EffectData tweenData;

    private EffectBuilder effectBuilder;


    private void Start()
    {
        effectBuilder = new EffectBuilder(this)
            .AddEffect(new Fade(graphic, tweenData))
            .ExecuteAll();
    }


}
