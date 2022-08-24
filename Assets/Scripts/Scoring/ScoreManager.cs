using System;
using UnityEngine;
using MiiskoWiiyaas.UI;
using MiiskoWiiyaas.Grid;
using MiiskoWiiyaas.Core;
using MiiskoWiiyaas.Core.Events;
using TMPro;

namespace MiiskoWiiyaas.Scoring
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreUI;
        [SerializeField] private ScoreUIAnimator scoreAnimator;
        [SerializeField] private BonusUIAnimator bonusUIAnimator;

        [SerializeField] private float matchMultiplier;
        [SerializeField] private float multiMatchBonus;
        [SerializeField] private float chainBonus;
        [SerializeField] private float currentScore;

        private int chainCount;
        private int multiMatchCount;
        private int timesCalled;

        private GameGrid<GemCell> grid;

        public event EventHandler<BonusEventArgs> OnBonus;

        public float CurrentScore { get => currentScore; set => UpdateScore(value); }

        private float CalculateScore(MatchEventArgs eventArgs)
        {
            int multiplier = (int)MathF.Ceiling(eventArgs.matches.Count * matchMultiplier);
            int chainBonusScore = (int)(chainCount * chainBonus);
            int multiMatchBonusScore = (int)(multiMatchCount * multiMatchBonus);

            if (chainBonusScore > 0)
            {
                BonusEventArgs bonusEventArg = new BonusEventArgs() { bonusType = MiiskoWiiyaas.Scoring.BonusType.CHAIN, score = chainBonusScore };
                OnBonus?.Invoke(this, bonusEventArg);
            }

            if (multiMatchBonusScore > 0)
            {
                BonusEventArgs bonusEventArg = new BonusEventArgs() { bonusType = MiiskoWiiyaas.Scoring.BonusType.MULTI, score = multiMatchBonusScore };
                OnBonus?.Invoke(this, bonusEventArg);
            }

            float totalScore = (eventArgs.matches[0].Value * eventArgs.matches.Count * multiplier)
                + chainBonusScore + multiMatchBonusScore;

            return totalScore;
        }

        public void Initialize(GameGrid<GemCell> grid)
        {
            this.grid = grid;
            currentScore = 0;
            scoreUI.text = currentScore.ToString();

            scoreAnimator.Initialize(grid);
            bonusUIAnimator.Initialize(grid);

            OnBonus += bonusUIAnimator.ScoreManager_OnBonus;
        }

        public void InputHandler_OnMatchFindingDone(object sender, EventArgs eventArgs)
        {
            chainCount = 0;
            multiMatchCount = 0;
            timesCalled = 0;
        }

        public void LevelManager_OnChangedToFirstLevel(object sender, LevelManagerEventArgs eventArgs)
        {
            if (eventArgs.isALevelRestart)
            {
                currentScore = 0;
                UpdateScore(currentScore);
            }
        }

        public void MatchFinder_OnMatchMade(object sender, MatchEventArgs eventArgs)
        {
            if (timesCalled > 0) multiMatchCount++;

            float score = CalculateScore(eventArgs);
            currentScore += score;

            scoreUI.text = currentScore.ToString();
            UpdateScoreAnimatorText(eventArgs, score);

            timesCalled++;
        }

        public void MatchFinder_OnSequenceDone(object sender, EventArgs eventArgs)
        {
            chainCount++;
            timesCalled = 0;
        }

        private void UpdateScore(float newScore)
        {
            currentScore = newScore;
            scoreUI.text = currentScore.ToString();
        }

        private void UpdateScoreAnimatorText(MatchEventArgs eventArgs, float scoreValue)
        {
            float x = (eventArgs.scoreCell.ID % grid.Rows) + grid.StartingPosition.x;
            float y = (eventArgs.scoreCell.ID / grid.Rows) + grid.StartingPosition.y;

            scoreUI.text = currentScore.ToString();
            scoreAnimator.ShowScoreText(x, y, scoreValue);
        }
    }

}