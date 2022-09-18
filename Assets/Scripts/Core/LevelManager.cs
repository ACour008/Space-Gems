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
        [SerializeField] private int turnsPerLevel = 12;
        [SerializeField] private int turnsPerLevelIncrease = 3;
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

        private IEnumerator ChangeToNextLevel()
        {
            gameGrid.SetCellsAsSelectable(false);
            OnLevelChange?.Invoke(this, EventArgs.Empty);

            yield return new WaitForSeconds(0.5f);

            gridClearer.Clear();

            yield return new WaitForSeconds(levelCompleteUI.ShowUI());

            levelCounterUI.SetLevelText(++currentLevel);
            levelCounterUI.SetProgressBar(0);

            turnsPerLevel += turnsPerLevelIncrease;
            gridRetiler.RefillGrid(0.5f);
            turnCount = 0;

            yield return new WaitForSeconds(0.5f);
            gameTracker.CacheData();
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

            turnsPerLevel = initialTargetLevelTurns;
            gridRetiler.RefillGrid(0.5f);
            turnCount = 0;

            yield return new WaitForSeconds(0.5f);
            gameTracker.CacheData();
            gameGrid.SetCellsAsSelectable(true);

            OnLevelChangeCompleted?.Invoke(this, new LevelManagerEventArgs() { isALevelRestart = true });
        }

        private void GameOverUIMenuToggler_OnCancelButtonClicked(object sender, EventArgs e) => StartCoroutine(ChangeToFirstLevel());

        /// <summary>
        /// Changes the level by clearing and creating a new grid layout and increases the target for the number of turns/level.
        /// </summary>
        public void GotoNextLevel()
        {
            StartCoroutine(ChangeToNextLevel());
        }

        /// <summary>
        /// Sets up the LevelManager
        /// </summary>
        /// <param name="grid">The main game grid object.</param>
        /// <param name="clearer">The object responsibile for clearing the grid.</param>
        /// <param name="retiler">The object responsible for retiling the grid after it has been cleared.</param>
        /// <param name="tracker">The object responsibile for tracking the game's current score and grid layout.</param>
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
            initialTargetLevelTurns = turnsPerLevel;
        }

        /// <summary>
        /// An event method that checks to see if the level needs changing after
        /// the match-finding process is complete.
        /// </summary>
        /// <param name="sender">The InputHandler object that invoked the event.</param>
        /// <param name="eventArgs">An empty EventArgs object</param>
        /// <seealso cref="InputHandler"/>
        /// <seealso cref="MatchFinder"/>
        public void InputHandler_OnMatchFindingDone(object sender, EventArgs eventArgs)
        {
            turnCount++;

            float value = (float)turnCount / (float)turnsPerLevel;

            levelCounterUI.SetProgressBar(value);

            if (turnCount == turnsPerLevel)
            {
                StartCoroutine(ChangeToNextLevel());
            }
        }

        /// <summary>
        /// An event method that restarts the level.
        /// </summary>
        /// <param name="sender">The restart button object that invoked the event</param>
        /// <param name="e">An empty EventArgs object.</param>
        private void RestartButton_OnRestartButtonClicked(object sender, EventArgs e) => RestartLevel();

        /// <summary>
        /// Restarts the level to initial settings, including score and grid layout.
        /// </summary>
        public void RestartLevel()
        {
            restartSFXClip.PlayOneShot();
            levelCounterUI.SetProgressBar(0);
            turnCount = 0;
            gameTracker.RevertToLevelStart();

        }
    }
}
