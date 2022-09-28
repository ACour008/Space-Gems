using Tweens;
using UnityEngine;

public class CellScoreAnimatedObject : UIAnimatedComponent
{
    public override Effect[] GenerateEffects(UIAnimationProperties properties)
    {
        // Either remove EffectData or add Constructors in each effect to just accept the three properties
        EffectData<Vector3> moveData = new EffectData<Vector3>(properties.endPosition, properties.durationInSeconds, properties.startDelayInSeconds);
        EffectData<float> fadeData = new EffectData<float>(properties.endColor.a, properties.durationInSeconds, properties.startDelayInSeconds);

        return new Effect[2]
        {
            new Move(scoreTextRT, moveData),
            new Fade(scoreText, fadeData)
        };
    }
}
