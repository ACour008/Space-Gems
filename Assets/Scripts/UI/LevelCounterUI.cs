using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MiiskoWiiyaas.UI
{
    public class LevelCounterUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI levelUGUI;
        [SerializeField] Image progressBar;
        
        /// <summary>
        /// Sets the text of the Level UI that is passed in through Unity's
        /// inspector.
        /// </summary>
        /// <param name="level">An int that represents the level of the game.
        /// Is converted to a string inside the method.</param>
        public void SetLevelText(int level)
        {
            levelUGUI.text = level.ToString();
        }

        /// <summary>
        /// Sets the value of the progress bar that is passed in through
        /// Unity's inspector.
        /// </summary>
        /// <param name="value">The value that the progress bar should be
        /// set at.</param>
        public void SetProgressBar(float value)
        {
            progressBar.fillAmount = value;
        }
    }
}
