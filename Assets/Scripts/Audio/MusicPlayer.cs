using FMODUnity;
using FMOD.Studio;

namespace MiiskoWiiyaas.Audio
{
    [System.Serializable]
    public class MusicPlayer
    {
        private EventInstance playlist;
        private int currentTrack = 1;


        public MusicPlayer(EventReference newPlaylist)
        {
            playlist = RuntimeManager.CreateInstance(newPlaylist);
        }

        public void Play()
        {
            playlist.getPlaybackState(out PLAYBACK_STATE state);
            if (state == PLAYBACK_STATE.PLAYING) return;

            playlist.start();
        }

        public void Pause()
        {
            playlist.getPaused(out bool isPaused);
            playlist.setPaused(!isPaused);
        }

        public void Stop()
        {
            playlist.getPlaybackState(out PLAYBACK_STATE state);
            if (state == PLAYBACK_STATE.STOPPED) return;

            playlist.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        public void StopAndRelease()
        {
            Stop();
            playlist.release();
        }

        public void NextTrack()
        {
            if (++currentTrack > 10) currentTrack = 1;
            playlist.setParameterByName("TrackNumber", currentTrack);
        }

        public void PreviousTrack()
        {
            if (--currentTrack < 1) currentTrack = 10;
            playlist.setParameterByName("TrackNumber", currentTrack);
            playlist.start();
        }
    }
}

