using UnityEngine;
using FMODUnity;
using FMOD.Studio;

namespace MiiskoWiiyaas.Audio
{
    public class SFXClip : MonoBehaviour
    {
        [SerializeField] private EventReference soundClipReference;

        public void Play()
        {
            EventInstance soundInstance = RuntimeManager.CreateInstance(soundClipReference);
            soundInstance.start();
            soundInstance.release();
        }
    }
}
