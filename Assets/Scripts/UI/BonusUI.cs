using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using MiiskoWiiyaas.Scoring;
using Tweens;
using TMPro;

namespace MiiskoWiiyaas.UI
{
    public class BonusUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI bonusText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform rectTransform;

        [SerializeField] private string multiBonusName = "Multi";
        [SerializeField] private string chainBonusName = "Chain";

        private EffectBuilder effectBuilder;
        private Dictionary<BonusType, string> bonusNames;
        private ObjectPool<BonusUI> pool;

        public void RunAnimation(BonusType bonusType, int score)
        {
            SetText(bonusType, score);

            Vector3 targetPosition = transform.position + new Vector3(0, 50, 0);
            float delay = (bonusType == BonusType.MULTI) ? 0.25f : 0;

            EffectData<Vector3> moveData = new EffectData<Vector3>() { durationInSecs = 1f, endValue = targetPosition, startDelayInSecs = delay };
            EffectData<float> fadeData = new EffectData<float>() { durationInSecs = 0.5f, endValue = 0, startDelayInSecs = delay + 0.5f };

            effectBuilder.ClearAllEffects();
            effectBuilder
                .AddEffect(new Move(transform, moveData))
                .AddEffect(new Fade(canvasGroup, fadeData))
                .ExecuteAll();
        }

        private void Awake()
        {
            effectBuilder = new EffectBuilder(this);
            bonusNames = new Dictionary<BonusType, string>()
            {
                {BonusType.MULTI, multiBonusName },
                {BonusType.CHAIN, chainBonusName }
            };
        }

        public void Reset()
        {
            rectTransform.anchoredPosition = new Vector3(0, 0, 0);   
        }

        public void SetPool(ObjectPool<BonusUI> pool)
        {
            this.pool = pool;
        }

        public void SetRandomStartPoint()
        {
            Vector3 anchoredPos = rectTransform.anchoredPosition3D;
            float halfWidth = (rectTransform.rect.width / 2);
            float randomX = Random.Range(-100f, 200);
            rectTransform.anchoredPosition3D = new Vector3(randomX, anchoredPos.y, anchoredPos.z);
        }

        public void SetText(BonusType bonusType, int score)
        {
            bonusText.text = bonusNames[bonusType] + " Bonus";
            scoreText.text = score.ToString();
        }


    }

}