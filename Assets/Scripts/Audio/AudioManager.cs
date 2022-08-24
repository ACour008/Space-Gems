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

        public void LevelManager_OnLevelChange(object sender, EventArgs e) => musicPlayer.Stop();

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

        private void SetUISlidersPostInit(out float musicValue, out float sfxValue)
        {
            musicValue = AudioManager.MusicUISlider.SliderValue;
            sfxValue = AudioManager.SfxUISlider.SliderValue;

            AudioManager.MusicUISlider = musicSliderHandler;
            AudioManager.SfxUISlider = sfxSliderHandler;

            AudioManager.MusicUISlider.VCAFader = AudioManager.MusicVCA;
            AudioManager.SfxUISlider.VCAFader = AudioManager.SfxVCA;
        }

        private void SetUISlidersPreInit(out float musicValue, out float sfxValue)
        {
            AudioManager.MusicUISlider = musicSliderHandler;
            AudioManager.SfxUISlider = sfxSliderHandler;

            AudioManager.MusicUISlider.VCAFader = AudioManager.MusicVCA;
            AudioManager.SfxUISlider.VCAFader = AudioManager.SfxVCA;

            musicValue = musicStartValue;
            sfxValue = sfxStartValue;
        }

        private void SetVCAs()
        {
            AudioManager.MusicVCA = RuntimeManager.GetVCA($"vca:/{musicVcaName}");
            AudioManager.SfxVCA = RuntimeManager.GetVCA($"vca:/{sfxVcaName}");
        }
    }

}