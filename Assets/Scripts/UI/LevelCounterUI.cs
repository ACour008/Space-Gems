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
        

        public void SetLevelText(int level)
        {
            levelUGUI.text = level.ToString();
        }

        public void SetProgressBar(float value)
        {
            progressBar.fillAmount = value;
        }
    }
}
