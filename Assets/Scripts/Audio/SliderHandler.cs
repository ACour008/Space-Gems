using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;
using FMODUnity;

namespace MiiskoWiiyaas.Audio
{
    public class SliderHandler : MonoBehaviour
    {
        [SerializeField] private Slider uiSlider;
        [SerializeField] private string vcaName;
        private VCA vcaFader;

        public float SliderValue { get => uiSlider.value; }
        public VCA VCAFader { set => vcaFader = value; }

        public void SetValue(float value) => uiSlider.value = value;


        public void OnSliderValueChange(float volume)
        {
            vcaFader.setVolume(volume);
        }
    }

}