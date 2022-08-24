using System;
using System.Collections;
using UnityEngine;
using MiiskoWiiyaas.Core.Events;
using MiiskoWiiyaas.Audio;
using MiiskoWiiyaas.Grid;
using MiiskoWiiyaas.UI;

namespace MiiskoWiiyaas.Core
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private int targetLevelTurns;
        [SerializeField] private int targetLevelIncreaseBy;
        [SerializeField] private LevelCompleteUI levelCompleteUI;
        [SerializeField] private LevelCounterUI levelCounterUI;
        [SerializeField] private int turnCount = 0;
        [SerializeField] private int currentLevel = 1;
        [SerializeField] private ButtonHandler restartButton;
        [SerializeField] private MenuToggler gameOverUIMenuToggler;
        [SerializeField] private SFXClip restartSFXClip;

        private GameGrid<GemCell> gameGrid;
        private GameTracker gameTracker;
        private GridClearer gridClearer;
        private GridRetiler gridRetiler;
        private int initialTargetLevelTurns;

        public event EventHandler OnLevelChange;
        public event EventHandler<LevelManagerEventArgs> OnLevelChangeCompleted;

        public void GotoNextLevel()
        {
            StartCoroutine(ChangeToNextLevel());
        }

        private IEnumerator ChangeToNextLevel()
        {
            gameGrid.SetCellsAsSelectable(false);
            OnLevelChange?.Invoke(this, EventArgs.Empty);

            yield return new WaitForSeconds(0.5f);

            gridClearer.Clear();

            yield return new WaitForSeconds(levelCompleteUI.ShowUI());

            levelCounterUI.SetLevelText(++currentLevel);
            levelCounterUI.SetProgressBar(0);

            targetLevelTurns += targetLevelIncreaseBy;
            gridRetiler.RefillGrid(0.5f);
            turnCount = 0;

            yield return new WaitForSeconds(0.5f);
            gameTracker.UpdateData();
            gameGrid.SetCellsAsSelectable(true);

            OnLevelChangeCompleted?.Invoke(this, new LevelManagerEventArgs() { isALevelRestart = false });
        }

        private IEnumerator ChangeToFirstLevel()
        {
            gameGrid.SetCellsAsSelectable(false);
            OnLevelChange?.Invoke(this, EventArgs.Empty);

            yield return new WaitForSeconds(0.5f);

            gridClearer.Clear();
            yield return new WaitForSeconds(0.5f);

            levelCounterUI.SetLevelText(1);
            levelCounterUI.SetProgressBar(0);

            targetLevelTurns = initialTargetLevelTurns;
            gridRetiler.RefillGrid(0.5f);
            turnCount = 0;

            yield return new WaitForSeconds(0.5f);
            gameTracker.UpdateData();
            gameGrid.SetCellsAsSelectable(true);

            OnLevelChangeCompleted?.Invoke(this, new LevelManagerEventArgs() { isALevelRestart = true });
        }

        private void GameOverUIMenuToggler_OnCancelButtonClicked(object sender, EventArgs e) => StartCoroutine(ChangeToFirstLevel());

        public void Initialize(GameGrid<GemCell> grid, GridClearer clearer, GridRetiler retiler, GameTracker tracker)
        {
            gameGrid = grid;
            gameTracker = tracker;
            gridClearer = clearer;
            gridRetiler = retiler;

            turnCount = 0;
            levelCounterUI.SetProgressBar(turnCount);
            levelCounterUI.SetLevelText(currentLevel);

            restartButton.OnClicked += RestartButton_OnRestartButtonClicked;
            gameOverUIMenuToggler.OnCancelButtonClicked += GameOverUIMenuToggler_OnCancelButtonClicked;
            initialTargetLevelTurns = targetLevelTurns;
        }

        public void InputHandler_OnMatchFindingDone(object sender, EventArgs eventArgs)
        {
            turnCount++;

            float value = (float)turnCount / (float)targetLevelTurns;

            levelCounterUI.SetProgressBar(value);

            if (turnCount == targetLevelTurns)
            {
                StartCoroutine(ChangeToNextLevel());
            }
        }

        private void RestartButton_OnRestartButtonClicked(object sender, EventArgs e) => RestartLevel();

        public void RestartLevel()
        {
            restartSFXClip.Play();
            levelCounterUI.SetProgressBar(0);
            turnCount = 0;
            gameTracker.RevertToLevelStart();

        }
    }
}
