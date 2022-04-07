using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiskoWiiyaas.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private int minimum;
        [SerializeField] private int maximum;
        [SerializeField] private int current;
        [SerializeField] private Image barMask;
        [SerializeField] private Image barFill;
        [SerializeField] private Color fillColor;


        [SerializeField] private float fillDuration;
        [SerializeField] private AnimationCurve fillCurve;
        public int Minimum { get => minimum; set => minimum = value; }
        public int Maximum { get => maximum; set => maximum = value; }
        public int Current { get => current; set => SetCurrent(value); }

        public void Start()
        {
            barFill.color = fillColor;
        }

        private void SetCurrent(int value)
        {
            current = value;
            StartCoroutine(UpdateBarValue());
        }

        public void SetCurrentWithoutAnimation(int value)
        {
            current = value;
        }

        public IEnumerator UpdateBarValue()
        {
            float currentOffset = current - minimum;
            float maxOffset = maximum - minimum;
            float fillAmount = currentOffset / maxOffset;
            float timeStart = Time.time;

            while (barMask.fillAmount < fillAmount)
            {
                float timeSinceStart = Time.time - timeStart;
                float complete = timeSinceStart / fillDuration;

                barMask.fillAmount = Mathf.Lerp(barMask.fillAmount, fillAmount, fillCurve.Evaluate(complete));

                yield return null;
            }
        }
    }
}