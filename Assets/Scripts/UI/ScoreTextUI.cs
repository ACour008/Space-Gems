using System;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using Tweens;

namespace MiiskoWiiyaas.UI
{
    /// <summary>
    /// A  that gets animated by a subclass of UIAnimator
    /// </summary>
    public class ScoreTextUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private RectTransform canvasRectTransform;
        [SerializeField] private Color startColor;
        private EffectBuilder effectBuilder;
        private ObjectPool<ScoreTextUI> pool;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        public void RunAnimation(int score)
        {
            textMesh.text = score.ToString();
            Vector3 targetPosition = textMesh.transform.position + new Vector3(0, 2, 0);

            EffectData<Vector3> moveData = new EffectData<Vector3>(targetPosition, durationInSeconds: 0.75f, startDelayInSeconds: 0);
            EffectData<float> fadeData = new EffectData<float>(endValue: 0, durationInSeconds: 0.5f, startDelayInSeconds: 0.25f);
            
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

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            textMesh.color = startColor;
            textMesh.transform.localPosition = new Vector3(0, 0, 0);
            canvasRectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pool"></param>
        public void SetPool(ObjectPool<ScoreTextUI> pool) => this.pool = pool;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPosition(float x, float y) => canvasRectTransform.anchoredPosition3D = new Vector3(x, y, 0);
    }

}