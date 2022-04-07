using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MiskoWiiyaas.UI
{
    public class GameTextUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textBox;
        [SerializeField] private float fadeInDuration;

        private bool _isFading;
        private Color initialColor;
        private Color targetColor;

        public bool IsFading { get => _isFading; }

        // Start is called before the first frame update
        void Awake()
        {
            initialColor = textBox.color;
            targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1);
        }

        public void ShowText(string newText)
        {
            textBox.text = newText;
            textBox.color = targetColor;
            SetActive(true);
        }

        public IEnumerator FadeInText(string newText)
        {
            textBox.text = newText;

            float timeStart = Time.time;
            SetActive(true);
            _isFading = true;

            while (textBox.color.a <= 0.95f)
            {
                float timeSinceStart = Time.time - timeStart;
                float completed = timeSinceStart / fadeInDuration;

                textBox.color = Color.Lerp(initialColor, targetColor, completed);
                yield return null;
            }
            _isFading = false;
            textBox.color = targetColor;
        }

        public IEnumerator FadeOutText()
        {
            float timeStart = Time.time;
            _isFading = true;

            while (textBox.color.a >= 0.05f)
            {
                float timeSinceStart = Time.time - timeStart;
                float completed = timeSinceStart / fadeInDuration;

                textBox.color = Color.Lerp(targetColor, initialColor, completed);
                yield return null;
            }
            _isFading = false;

            textBox.color = initialColor;
            textBox.text = "";

            SetActive(false);
        }

        public void SetActive(bool active)
        {
            textBox.gameObject.SetActive(active);
        }

    }
}
