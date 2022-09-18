using System;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

namespace MiiskoWiiyaas.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] float musicStartValue;
        [SerializeField] float sfxStartValue;
        [SerializeField] SliderHandler musicSliderHandler;
        [SerializeField] SliderHandler sfxSliderHandler;
        [SerializeField] string musicVcaName;
        [SerializeField] string sfxVcaName;
        [SerializeField] EventReference musicPlaylist;

        MusicPlayer musicPlayer;

        private static VCA MusicVCA;
        private static VCA SfxVCA;
        private static SliderHandler MusicUISlider;
        private static SliderHandler SfxUISlider;
        private static bool initialized = false;

        #region Initialization

        private void CheckInitialization()
        {
            float musicSliderValue = 0f;
            float sfxSliderValue = 0f;

            if (!AudioManager.initialized)
            {
                SetVCAs();
                SetUISlidersPreInit(out musicSliderValue, out sfxSliderValue);
                AudioManager.initialized = true;
            }
            else
            {
                SetUISlidersPostInit(out musicSliderValue, out sfxSliderValue);
            }

            AudioManager.MusicVCA.setVolume(musicSliderValue);
            AudioManager.MusicUISlider.SetValue(musicSliderValue);

            AudioManager.SfxVCA.setVolume(sfxSliderValue);
            AudioManager.SfxUISlider.SetValue(sfxSliderValue);
        }

        // If the static variables of AudioManager has already initialized:
        // Assigns the values of the previous scene's Music and SFX UI Sliders to
        // musicSliderValue and sfxSliderValue BEFORE assigning the current scene's
        // UI Sliders & VCAs.
        // This helps to sharing sound settings across scenes.
        private void SetUISlidersPostInit(out float musicSliderValue, out float sfxSliderValue)
        {
            musicSliderValue = AudioManager.MusicUISlider.SliderValue;
            sfxSliderValue = AudioManager.SfxUISlider.SliderValue;

            AudioManager.MusicUISlider = musicSliderHandler;
            AudioManager.SfxUISlider = sfxSliderHandler;

            AudioManager.MusicUISlider.VCAFader = AudioManager.MusicVCA;
            AudioManager.SfxUISlider.VCAFader = AudioManager.SfxVCA;
        }

        // If the static variables of AudioManager has NOT been initialized:
        // AudioManager gets the current scene's Music & SFX UI Sliders to
        // assign their values to musicSliderValue & sfxSliderValue.
        // This helps to share sound settings across scenes.
        private void SetUISlidersPreInit(out float musicSliderValue, out float sfxSliderValue)
        {
            AudioManager.MusicUISlider = musicSliderHandler;
            AudioManager.SfxUISlider = sfxSliderHandler;

            AudioManager.MusicUISlider.VCAFader = AudioManager.MusicVCA;
            AudioManager.SfxUISlider.VCAFader = AudioManager.SfxVCA;

            musicSliderValue = musicStartValue;
            sfxSliderValue = sfxStartValue;
        }

        private void SetVCAs()
        {
            AudioManager.MusicVCA = RuntimeManager.GetVCA($"vca:/{musicVcaName}");
            AudioManager.SfxVCA = RuntimeManager.GetVCA($"vca:/{sfxVcaName}");
        }
        #endregion

        #region Events

        /// <summary>
        /// Stops the music player when the game changes a level.
        /// </summary>
        /// <param name="sender">The object that invoked the event.</param>
        /// <param name="e">An empty EventArgs object.</param>
        public void LevelManager_OnLevelChange(object sender, EventArgs e) => musicPlayer.Stop();

        /// <summary>
        /// Gets the music player's next track and plays it.
        /// </summary>
        /// <param name="sender">The object that invoked the event.</param>
        /// <param name="e">An empty EventArgs object</param>
        public void LevelManager_OnLevelChangeCompleted(object sender, EventArgs e)
        {
            musicPlayer.NextTrack();
            musicPlayer.Play();
        }

        private void OnDestroy()
        {
            musicPlayer.StopAndRelease();
        }

        private void Start()
        {
            CheckInitialization();
            musicPlayer = new MusicPlayer(musicPlaylist);

            musicPlayer.Play();
        }
        #endregion

    }

}