using UnityEngine;
using TMPro;

namespace MiiskoWiiyaas.UI
{
    /// <summary>
    /// Randomizes the subtitle on the Title Screen
    /// </summary>
    public class SubtitleRandomizer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textBox;
        [SerializeField] private string[] subtitles;

        private void Start()
        {
            Random.InitState((int)System.DateTime.Now.Ticks);

            int randomIndex = Random.Range(0, subtitles.Length);
            textBox.text = subtitles[randomIndex];
        }
    }

}
