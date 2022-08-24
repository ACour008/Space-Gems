using UnityEngine;
using MiiskoWiiyaas.Grid;
using MiiskoWiiyaas.Core;
using UnityEngine.Pool;

namespace MiiskoWiiyaas.UI
{
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

        public void Initialize(GameGrid<GemCell> grid)
        {
            this.grid = grid;
            this.pool = new ObjectPool<ScoreTextUI>(CreateScoreText, OnGetScoreText, OnReleaseScoreText, defaultCapacity:poolSize, maxSize:maxPoolSize);

        }

        public void ShowScoreText(float x, float y, float scoreValue)
        {
            ScoreTextUI scoreText = this.pool.Get();

            scoreText.SetPosition(x, y);
            scoreText.RunAnimation(scoreValue.ToString());
        }

        public void OnGetScoreText(ScoreTextUI scoreText) => scoreText.gameObject.SetActive(true);

        public void OnReleaseScoreText(ScoreTextUI scoreText)
        {
            scoreText.gameObject.SetActive(false);
            scoreText.transform.SetParent(transform);
            scoreText.Reset();
        }
    }
}
