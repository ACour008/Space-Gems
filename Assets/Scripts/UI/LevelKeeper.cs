using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MiskoWiiyaas.UI
{
    public class LevelKeeper : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI scoreText;
        private int currentLevel;


        private void Awake()
        {
            currentLevel = 1;
        }

        private void Start()
        {
            UpdateLevel(currentLevel);
        }

        public void UpdateLevel(string level)
        {
            scoreText.text = level;
        }

        public void UpdateLevel(int level)
        {
            scoreText.text = level.ToString();
        }

        public void IncreaseLevel()
        {
            currentLevel++;
            UpdateLevel(currentLevel);

        }
    }

}