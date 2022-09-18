using FMODUnity;
using FMOD.Studio;

namespace MiiskoWiiyaas.Audio
{
    [System.Serializable]
    public class MusicPlayer
    {
        private EventInstance playlist;
        private int currentTrack = 1;

        /// <summary>
        /// The Constructor for MusicPlayer
        /// </summary>
        /// <param name="newPlaylist">An FMOD Event Reference that contains the music to be played in-scene.</param>
        public MusicPlayer(EventReference newPlaylist)
        {
            playlist = RuntimeManager.CreateInstance(newPlaylist);
        }

        /// <summary>
        /// Plays the current track.
        /// </summary>
        public void Play()
        {
            playlist.getPlaybackState(out PLAYBACK_STATE state);
            if (state == PLAYBACK_STATE.PLAYING) return;

            playlist.start();
        }

        /// <summary>
        /// Pauses the current track.
        /// </summary>
        public void Pause()
        {
            playlist.getPaused(out bool isPaused);
            playlist.setPaused(!isPaused);
        }

        /// <summary>
        /// Stops the current track with a small fade-out.
        /// </summary>
        /// <remarks>Use this if you plan to play music again.</remarks>
        public void Stop()
        {
            playlist.getPlaybackState(out PLAYBACK_STATE state);
            if (state == PLAYBACK_STATE.STOPPED) return;

            playlist.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        /// <summary>
        /// Stops the current track with a small fade-out before releasing the
        /// FMOD Event Instance from memory. Usually used in OnDestroy Methods.
        /// </summary>
        public void StopAndRelease()
        {
            Stop();
            playlist.release();
        }

        /// <summary>
        /// Switches to the next track in the playlist (but does not play).
        /// </summary>
        public void NextTrack()
        {
            if (++currentTrack > 10) currentTrack = 1;
            playlist.setParameterByName("TrackNumber", currentTrack);
        }

        /// <summary>
        /// Switches to the previous track in the playlist (but does not play).
        /// </summary>
        public void PreviousTrack()
        {
            if (--currentTrack < 1) currentTrack = 10;
            playlist.setParameterByName("TrackNumber", currentTrack);
            playlist.start();
        }
    }
}

