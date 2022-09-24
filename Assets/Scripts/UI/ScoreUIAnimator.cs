using UnityEngine;
using MiiskoWiiyaas.Grid;
using MiiskoWiiyaas.Core;
using UnityEngine.Pool;

namespace MiiskoWiiyaas.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class ScoreUIAnimator : MonoBehaviour
    {
        [SerializeField] private ScoreTextUI scoreTextPrefab;
        [SerializeField] private int poolSize;
        [SerializeField] private int maxPoolSize;

        [SerializeField] private ObjectPool<ScoreTextUI> pool;
        private GameGrid<GemCell> grid;

        private ScoreTextUI CreateScoreText()
        {
            ScoreTextUI scoreText = Instantiate<ScoreTextUI>(scoreTextPrefab, transform);
            scoreText.SetPool(this.pool);
            return scoreText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        public void Initialize(GameGrid<GemCell> grid)
        {
            this.grid = grid;
            this.pool = new ObjectPool<ScoreTextUI>(CreateScoreText, OnGetScoreText, OnReleaseScoreText, defaultCapacity:poolSize, maxSize:maxPoolSize);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="scoreValue"></param>
        public void ShowScoreText(float x, float y, int scoreValue)
        {
            ScoreTextUI scoreText = this.pool.Get();

            scoreText.SetPosition(x, y);
            scoreText.RunAnimation(scoreValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scoreText"></param>
        public void OnGetScoreText(ScoreTextUI scoreText) => scoreText.gameObject.SetActive(true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scoreText"></param>
        public void OnReleaseScoreText(ScoreTextUI scoreText)
        {
            scoreText.gameObject.SetActive(false);
            scoreText.transform.SetParent(transform);
            scoreText.Reset();
        }
    }
}
