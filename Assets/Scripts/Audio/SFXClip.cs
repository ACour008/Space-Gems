using UnityEngine;
using FMODUnity;
using FMOD.Studio;

namespace MiiskoWiiyaas.Audio
{
    public class SFXClip : MonoBehaviour
    {
        [SerializeField] private EventReference soundClipReference;

        /// <summary>
        /// Plays the FMOD EventReference set in Unity's inspector, then releases
        /// the instance from memory.
        /// </summary>
        public void PlayOneShot()
        {
            EventInstance soundInstance = RuntimeManager.CreateInstance(soundClipReference);
            soundInstance.start();
            soundInstance.release();
        }
    }
}
