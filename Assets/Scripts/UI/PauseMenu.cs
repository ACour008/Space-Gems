using System.Collections;
using UnityEngine;
using MiskoWiiyaas.Interfaces;

namespace MiskoWiiyaas.UI
{
    public class PauseMenu : MonoBehaviour, IMenu
    {
        [Header("Main Settings")]
        [SerializeField] private Vector3 startPos;
        [SerializeField] private Vector3 endPos;
        [SerializeField] private float moveDurationInSeconds;

        [Header("Animation Easing")]
        [SerializeField] private AnimationCurve openEasing;
        [SerializeField] private AnimationCurve closeEasing;

        private bool isMoving = false;
        private float _moveDuration;
        public bool IsMoving { get => isMoving; }

        public void Start()
        {
            transform.position = startPos;
        }

        public void Open()
        {
            StartCoroutine(RunOpen());
        }

        public void Close()
        {
            StartCoroutine(RunClose());
        }

        public IEnumerator RunClose()
        {
            float startTime = Time.time;

            isMoving = true;
            while (Vector3.Distance(transform.position, startPos) > 0.01f)
            {
                float timeSinceStart = Time.time - startTime;
                float complete = timeSinceStart / (moveDurationInSeconds);

                transform.position = Vector3.Lerp(endPos, startPos, closeEasing.Evaluate(complete));
                yield return null;
            }
            transform.position = startPos;
            isMoving = false;
        }

        public IEnumerator RunOpen()
        {
            float startTime = Time.time;

            isMoving = true;
            while (Vector3.Distance(transform.position, endPos) > 0.01f)
            {
                float timeSinceStart = Time.time - startTime;
                float complete = timeSinceStart / (moveDurationInSeconds );

                transform.position = Vector3.Lerp(startPos, endPos, openEasing.Evaluate(complete));
                yield return null;
            }
            transform.position = endPos;
            isMoving = false;
        }
    }
}