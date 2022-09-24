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
        // [SerializeField] private ScoreUIAnimator scoreAnimator;
        // [SerializeField] private BonusUIAnimator bonusUIAnimator;

        [SerializeField] private float matchMultiplier;
        [SerializeField] private float multiMatchBonus;
        [SerializeField] private float chainBonus;
        [SerializeField] private float currentScore;

        private int chainCount;
        private int multiMatchCount;
        private int matchesMadePerTurn;

        private GameGrid<GemCell> grid;

        public event EventHandler<BonusEventArgs> OnBonus;

        public float CurrentScore { get => currentScore; set => UpdateScore(value); }

        private int CalculateScore(MatchEventArgs eventArgs)
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

            int totalScore = (int)(eventArgs.matches[0].Value * eventArgs.matches.Count * multiplier)
                + chainBonusScore + multiMatchBonusScore;

            return totalScore;
        }

        /// <summary>
        /// Sets up the Score Manager
        /// </summary>
        /// <param name="grid">The main game grid.</param>
        public void Initialize(GameGrid<GemCell> grid)
        {
            this.grid = grid;
            currentScore = 0;
            scoreUI.text = currentScore.ToString();

            // scoreAnimator.Initialize(grid);
            // bonusUIAnimator.Initialize(grid);

            // OnBonus += bonusUIAnimator.ScoreManager_OnBonus;
        }

        /// <summary>
        /// Resets the chain and multi-match bonus counts after the matchfinding
        /// process is complete.
        /// </summary>
        /// <param name="sender">The InputHandler object that invoked the event.</param>
        /// <param name="eventArgs">An empty System.EventArgs object.</param>
        public void InputHandler_OnMatchFindingDone(object sender, EventArgs eventArgs)
        {
            chainCount = 0;
            multiMatchCount = 0;
            matchesMadePerTurn = 0;
        }

        /// <summary>
        /// Updates the score to zero if the user decides to restart the game.
        /// </summary>
        /// <param name="sender">The LevelManager object that invoked the event.</param>
        /// <param name="eventArgs">An EventArgs subclass that holds the data as to whether
        /// the game has restarted or is starting for the first time.</param>
        public void LevelManager_OnChangedToFirstLevel(object sender, LevelManagerEventArgs eventArgs)
        {
            if (eventArgs.isALevelRestart)
            {
                currentScore = 0;
                UpdateScore(currentScore);
            }
        }

        /// <summary>
        /// Calculates the score with chain and multi-match bonuses when an match is found.
        /// </summary>
        /// <param name="sender">The MatchFinder object that invoked the event.</param>
        /// <param name="eventArgs"></param>
        public void MatchFinder_OnMatchProcessed(object sender, MatchEventArgs eventArgs)
        {
            if (matchesMadePerTurn > 0) multiMatchCount++;

            int score = CalculateScore(eventArgs);
            currentScore += score;

            scoreUI.text = currentScore.ToString();
            UpdateScoreAnimatorText(eventArgs, score);

            matchesMadePerTurn++;
        }

        /// <summary>
        /// Increments the chain bonus count after a match has been found.
        /// </summary>
        /// <param name="sender">The MatchFinder object that invoked the event.</param>
        /// <param name="eventArgs">An empty EventArgs object.</param>
        public void MatchFinder_OnMatchFound(object sender, EventArgs eventArgs)
        {
            chainCount++;
            matchesMadePerTurn = 0;
        }

        private void UpdateScore(float newScore)
        {
            currentScore = newScore;
            scoreUI.text = currentScore.ToString();
        }

        // TODO: Ensure that each generated text do not overlap.
        private void UpdateScoreAnimatorText(MatchEventArgs eventArgs, int scoreValue)
        {
            float x = (eventArgs.scoreCell.ID % grid.Rows) + grid.StartingPosition.x;
            float y = (eventArgs.scoreCell.ID / grid.Rows) + grid.StartingPosition.y;

            scoreUI.text = currentScore.ToString();
            // scoreAnimator.ShowScoreText(x, y, scoreValue);

            // OnCellScoreUpdated?.Invoke(this, new BonusEventArgs() { score = (int)currentScore });
        }
    }

}