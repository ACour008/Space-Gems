using System;
using UnityEngine;
using UnityEngine.EventSystems;
using MiiskoWiiyaas.Audio;

namespace MiiskoWiiyaas.UI
{
    public class ButtonHandler : MonoBehaviour, IClickable, IPointerClickHandler
    {
        public event EventHandler OnClicked;
        [SerializeField] private SFXClip clickSFX;

        private bool canClick;

        /// <summary>
        /// Determines whether the button being handled can be clicked.
        /// </summary>
        public bool CanClick { get => canClick; set => canClick = value; }

        /// <summary>
        /// Invokes the OnClicked EventHandler when the button being handled
        /// is clicked.
        /// </summary>
        /// <param name="eventData">The PointerEventData passed to the method.
        /// Is unused.</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this, EventArgs.Empty);

            clickSFX?.PlayOneShot();
        }
    }

}