using System;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using Tweens;

namespace MiiskoWiiyaas.UI
{
    public class ScoreTextUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private RectTransform canvasRectTransform;
        [SerializeField] private Color startColor;
        private EffectBuilder effectBuilder;
        private ObjectPool<ScoreTextUI> pool;

        public void RunAnimation(string score)
        {
            textMesh.text = score;
            Vector3 targetPosition = textMesh.transform.position + new Vector3(0, 2, 0);

            EffectData<Vector3> moveData = new EffectData<Vector3>() { endValue = targetPosition, durationInSecs = 0.75f, startDelayInSecs = 0f };
            EffectData<float> fadeData = new EffectData<float>() { endValue = 0, durationInSecs = 0.5f, startDelayInSecs = 0.25f };
            
            effectBuilder.AddEffect(new Move(textMesh.transform, moveData))
                .AddEffect(new Fade(textMesh, fadeData))
                .ExecuteAll();
        }

        private void Awake()
        {
            effectBuilder = new EffectBuilder(this);
            effectBuilder.OnExecutionCompleted += EffectBuilder_OnExecutionCompleted;
        }

        private void EffectBuilder_OnExecutionCompleted(object sender, EventArgs e)
        {
            this.pool.Release(this);
        }

        public void Reset()
        {
            textMesh.color = startColor;
            textMesh.transform.localPosition = new Vector3(0, 0, 0);
            canvasRectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
        }

        public void SetPool(ObjectPool<ScoreTextUI> pool) => this.pool = pool;

        public void SetPosition(float x, float y) => canvasRectTransform.anchoredPosition3D = new Vector3(x, y, 0);
    }

}