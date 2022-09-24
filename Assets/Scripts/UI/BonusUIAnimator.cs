using UnityEngine;
using UnityEngine.Pool;
using MiiskoWiiyaas.Grid;
using MiiskoWiiyaas.Core;
using MiiskoWiiyaas.Core.Events;

namespace MiiskoWiiyaas.UI
{
    public class BonusUIAnimator : MonoBehaviour
    {
        [SerializeField] private BonusUI bonusUIPrefab;
        [SerializeField] private int poolSize;
        [SerializeField] private int maxPoolSize;

        [SerializeField] private ObjectPool<BonusUI> pool;
        private GameGrid<GemCell> grid;

        private BonusUI CreateScoreText()
        {
            BonusUI scoreText = Instantiate<BonusUI>(bonusUIPrefab, transform);
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
            this.pool = new ObjectPool<BonusUI>(CreateScoreText, OnGetScoreText, OnReleaseScoreText, defaultCapacity: poolSize, maxSize: maxPoolSize);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public void ScoreManager_OnBonus(object sender, BonusEventArgs eventArgs)
        {
            BonusUI bonusUI = pool.Get();

            bonusUI.SetRandomStartPoint();
            bonusUI.RunAnimation(eventArgs.bonusType, eventArgs.score);
        }

        private void OnGetScoreText(BonusUI bonusUI) => bonusUI.gameObject.SetActive(true);

        private void OnReleaseScoreText(BonusUI bonusUI)
        {
            bonusUI.gameObject.SetActive(false);
            bonusUI.transform.SetParent(transform);
            bonusUI.Reset();
        }
    }

}
