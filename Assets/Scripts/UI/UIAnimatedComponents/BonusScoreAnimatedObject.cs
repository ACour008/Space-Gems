using Tweens;
using UnityEngine;

public class BonusScoreAnimatedObject : UIAnimatedComponent
{
    public override Effect[] GenerateEffects(UIAnimationProperties properties)
    {
        float randomOffsetX = Random.Range(-150f, 150f);
        float randomOffsetY = Random.Range(100, 250);
        Vector3 randomEndPosition = new Vector3(randomOffsetX, randomOffsetY, properties.endPosition.z);

        EffectData<Vector3> moveData = new EffectData<Vector3>(randomEndPosition, properties.durationInSeconds, properties.startDelayInSeconds);
        EffectData<float> fadeData = new EffectData<float>(properties.endColor.a, properties.durationInSeconds, properties.startDelayInSeconds);

        return new Effect[2]
        {
            new Move(canvasRT, moveData),
            new Fade(canvasGroup, fadeData)
        };
    }
}
