using PBMusicVideoPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBMusicVideoPlayer
{
    public class VideoPlayerController : Singleton<VideoPlayerController>
    {
        public VideoData CurrentVideo = null;
        public MappedCustomSong CurrentSong = null;

        public void OnLoad()
        {
            MenuUtil.Instance.SongSelectedEvent += OnSongSelected;
            MenuUtil.Instance.EnvironmentSceneLoadedEvent += OnEnvironmentSceneLoaded;
            MenuUtil.Instance.MenuSceneLoadedEvent += OnMenuSceneLoaded;
            MenuUtil.Instance.GamePausedEvent += OnGamePaused;
        }

        private void OnGamePaused(bool isPaused)
        {
            if(isPaused)
            {
                VideoPlayerManager.Instance.Player.Pause();
            }
            else
            {
                VideoPlayerManager.Instance.Player.Play();
            }
        }

        private void OnMenuSceneLoaded(UnityEngine.SceneManagement.Scene scene)
        {
            VideoPlayerManager.Instance.StopVideo();
            VideoPlayerManager.Instance.SetPlayerActive(false);
        }

        private void OnEnvironmentSceneLoaded(UnityEngine.SceneManagement.Scene scene)
        {
            VideoPlayerManager.Instance.SetPlayerActive(true);
            VideoPlayerManager.Instance.PlayVideo();
        }

        private void OnSongSelected(int index, string songPath, string songName)
        {
            try
            {
                CurrentSong = SongLoader.Instance.GetMappedSong(songName);

                if (VideoLoader.Instance.TryGetVideo(CurrentSong, out CurrentVideo))
                {
                    Logger.Instance.Log($"Found video for {songName}: \n{CurrentVideo}", Logger.LogSeverity.DEBUG);
                    VideoPlayerManager.Instance.PrepareVideo(CurrentVideo);
                }
                else
                {
                    Logger.Instance.Log($"Video could not be found for {songName}", Logger.LogSeverity.DEBUG);
                }
            }
            catch (Exception e)
            {
                Logger.Instance.Log($"Got {e} on {songName}", Logger.LogSeverity.DEBUG);
            }
            
        }
    }
}
