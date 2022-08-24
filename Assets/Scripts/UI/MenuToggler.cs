using System;
using UnityEngine;
using UnityEngine.UI;
using MiiskoWiiyaas.Audio;

namespace MiiskoWiiyaas.UI
{
    public class MenuToggler : MonoBehaviour
    {
        [SerializeField] private Button menuOpenButton;
        [SerializeField] private Button menuCloseButton;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private SFXClip confirmSound;
        [SerializeField] private SFXClip cancelSound;
        [SerializeField] private SFXClip menuOpenSound;
        [SerializeField] private SFXClip menuCloseSound;

        private ButtonHandler menuOpenButtonHandler;
        private ButtonHandler menuCloseButtonHandler;
        private ButtonHandler confirmButtonHandler;
        private ButtonHandler cancelButtonHandler;

        public event EventHandler OnConfirmButtonClicked;
        public event EventHandler OnCancelButtonClicked;

        private void Awake()
        {
            SetMenuActive(false);
            GetButtonHandlerComponents();
            RegisterButtonHandlerEvents();
        }
        private void CancelButtonHandler_OnClicked(object sender, EventArgs e) => PerformCancel();

        public void CloseMenu()
        {
            menuCloseSound?.Play();
            SetMenuActive(false);
        }

        private void ConfirmButtonHandler_OnClicked(object sender, EventArgs e) => PerformConfirm();

        private void GetButtonHandlerComponents()
        {
            confirmButtonHandler = confirmButton?.GetComponent<ButtonHandler>();
            cancelButtonHandler = cancelButton?.GetComponent<ButtonHandler>();
            menuCloseButtonHandler = menuCloseButton?.GetComponent<ButtonHandler>();
            menuOpenButtonHandler = menuOpenButton?.GetComponent<ButtonHandler>();
        }

        private void MenuCloseButtonHandler_OnClicked(object sender, EventArgs e) => CloseMenu();

        private void MenuOpenButtonHandler_OnClicked(object sender, EventArgs e) => OpenMenu();

        public void OpenMenu()
        {
            SetMenuActive(true);
            menuOpenSound?.Play();
        }


        public void PerformCancel()
        {
            cancelSound?.Play();
            OnCancelButtonClicked?.Invoke(this, EventArgs.Empty);
            CloseMenu();
        }

        public void PerformConfirm()
        {
            confirmSound?.Play();
            OnConfirmButtonClicked?.Invoke(this, EventArgs.Empty);
            CloseMenu();
        }

        private void RegisterButtonHandlerEvents()
        {
            if (menuOpenButtonHandler != null)
            {
                menuOpenButtonHandler.OnClicked += MenuOpenButtonHandler_OnClicked;
            }

            if (menuCloseButtonHandler != null)
            {
                menuCloseButtonHandler.OnClicked += MenuCloseButtonHandler_OnClicked;
            }

            if (confirmButtonHandler != null)
            {
                confirmButtonHandler.OnClicked += ConfirmButtonHandler_OnClicked;
            }

            if (cancelButtonHandler != null)
            {
                cancelButtonHandler.OnClicked += CancelButtonHandler_OnClicked;
            }
        }

        private void SetMenuActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }

}