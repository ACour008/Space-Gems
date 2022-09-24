using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MiiskoWiiyaas.UI
{
    public class LevelCompleteUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI subtitleUI;
        [SerializeField] float hideUIAfterSeconds;
        [SerializeField] string[] subtitles;

        private System.Random random = new System.Random();

        /// <summary>
        /// Activates the Level Complete UI text.
        /// </summary>
        /// <returns>The number of seconds that took to hide the UI text after it was activated.</returns>
        public float ShowUI()
        {
            subtitleUI.text = subtitles[random.Next(0, subtitles.Length)];
            gameObject.SetActive(true);
            StartCoroutine(HideUIAfterSeconds(hideUIAfterSeconds));
            return hideUIAfterSeconds;
        }

        /// <summary>
        /// Hides the Level Complete UI text.
        /// </summary>
        public void HideUI()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator HideUIAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            HideUI();
        }
    }
}
