using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

namespace MiiskoWiiyaas.Audio
{
    public class SliderHandler : MonoBehaviour
    {
        [SerializeField] private Slider uiSlider;
        [SerializeField] private string vcaName;

        private VCA vcaFader;

        public float SliderValue { get => uiSlider.value; }
        public VCA VCAFader { set => vcaFader = value; }


        /// <summary>
        /// Sets the value of the UI Slider set in Unity's inspector.
        /// </summary>
        /// <param name="value">The value assigned to the slider.</param>
        public void SetValue(float value) => uiSlider.value = value;

        /// <summary>
        /// A event method that changes the volume of the FMOD VCA Fader.
        /// </summary>
        /// <param name="volume">The value of the volume to be set.</param>
        public void OnSliderValueChange(float volume)
        {
            vcaFader.setVolume(volume);
        }
    }

}