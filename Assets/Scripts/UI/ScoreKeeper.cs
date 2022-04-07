using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MiskoWiiyaas.Cells;
using MiskoWiiyaas.Events;
using MiskoWiiyaas.Tiles;

namespace MiskoWiiyaas.UI
{
    public class ScoreKeeper : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private ProgressBar progressBar;

        [Header("Score Settings")]
        [SerializeField] private float scoreMultiplierIncrease;
        [SerializeField] private float chainMultiplierIncrease;
        [SerializeField] private int currentScore;
        [SerializeField] private int targetScore;
        private float targetScoreMultiplier;
        private float targetScoreMultiplierIncrease;

        [SerializeField] private float scoreMultiplier;
        [SerializeField] private float chainMultiplier;

        public event EventHandler<ScoreEventArgs> OnScoreTargetMet;

        public void Initialize(int initialScoreTarget, float scoreMultiplier, float multiplierIncrease)
        {
            currentScore = 0;
            targetScore = initialScoreTarget;
            targetScoreMultiplier = scoreMultiplier;
            targetScoreMultiplierIncrease = multiplierIncrease;

            progressBar.Minimum = 0;
            progressBar.Maximum = targetScore;
            progressBar.Current = currentScore;

            ResetMultipliers();
        }
        public void GameGrid_OnChecksDone(object sender, ScoreEventArgs e)
        {
            ResetMultipliers();
        }

        public void GameGrid_OnUpdateScore(object sender, ScoreEventArgs e)
        {
            List<Tile> matches = e.matches;
            GridCell cell = e.cell;
            int matchScore = 0;

            for (int i = 0; i < matches.Count; i++)
            {
                if (i > 2)
                {
                    scoreMultiplier += scoreMultiplierIncrease;
                }
                matchScore += (int)((matches[i].Value * scoreMultiplier) * chainMultiplier);
            }

            chainMultiplier += chainMultiplierIncrease;
            cell.PlayScoreAnimation(matchScore);
            UpdateScore(matchScore);
        }

        private void UpdateScore(int score)
        {
            currentScore += score;

            progressBar.Current = currentScore;
            scoreText.text = currentScore.ToString();

            if (currentScore >= targetScore)
            {
                targetScore = (int)(currentScore * targetScoreMultiplier);
                OnScoreTargetMet?.Invoke(this, new ScoreEventArgs { score = currentScore });
                targetScoreMultiplier += targetScoreMultiplierIncrease;
            }
        }

        public void SetProgressBar()
        {
            progressBar.Minimum = currentScore;
            progressBar.Maximum = targetScore;
            progressBar.SetCurrentWithoutAnimation(currentScore);
        }

        private void ResetMultipliers()
        {
            scoreMultiplier = 1;
            chainMultiplier = 1;
        }
    }

}
