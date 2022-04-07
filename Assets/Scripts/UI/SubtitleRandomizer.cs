using UnityEngine;
using TMPro;

namespace MiskoWiiyaas.UI
{

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
