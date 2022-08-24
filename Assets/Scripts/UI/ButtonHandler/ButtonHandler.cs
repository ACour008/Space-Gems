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

        public bool CanClick { get; set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this, EventArgs.Empty);

            clickSFX?.Play();
        }
    }

}