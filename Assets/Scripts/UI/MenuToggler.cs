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

        /// <summary>
        /// Closes the menu with sound effects if menuCloseSound set in the Inspector.
        /// </summary>
        public void CloseMenu()
        {
            menuCloseSound?.PlayOneShot();
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

        /// <summary>
        /// Opens the menu with sound effects if menuOpenSound set in inspector.
        /// </summary>
        public void OpenMenu()
        {
            SetMenuActive(true);
            menuOpenSound?.PlayOneShot();
        }

        /// <summary>
        /// Closes the menu & plays cancelSound if set in the inspector. An
        /// event is fired to notify subscribers that any data should be reverted.
        /// </summary>
        public void PerformCancel()
        {
            cancelSound?.PlayOneShot();
            OnCancelButtonClicked?.Invoke(this, EventArgs.Empty);
            CloseMenu();
        }

        /// <summary>
        /// Closes the menu and plays confirmSound if set in the inspector. An
        /// event is fired to notify subscribers that any data should be saved.
        /// </summary>
        public void PerformConfirm()
        {
            confirmSound?.PlayOneShot();
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