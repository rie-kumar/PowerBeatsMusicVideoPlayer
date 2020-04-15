using PBMusicVideoPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBMusicVideoPlayer
{
    public class VideoPlayerViewController : Singleton<VideoPlayerViewController>
    {
        public VideoData CurrentVideo = null;
        public MappedCustomSong CurrentSong = null;

        public void OnLoad()
        {
            MenuUtil.Instance.SongSelectedEvent += OnSongSelected;
            MenuUtil.Instance.EnvironmentSceneLoadedEvent += OnEnvironmentSceneLoaded;
            MenuUtil.Instance.MenuSceneLoadedEvent += OnMenuSceneLoaded;
            MenuUtil.Instance.GamePausedEvent += OnGamePaused;
            MenuUtil.Instance.MenuReadyEvent += OnMenuReady;

            if(MenuUtil.Instance.TryGetSelectedSongNameCustom(out string songName))
            {
                SetVideo(songName);
            }
        }

        private void OnMenuReady(powerbeatsvr.Menu menu)
        {
            //PBUtil.Instance.PrintButtons();
            //PBUtil.Instance.PrintCameras();
        }

        private void OnGamePaused(bool isPaused)
        {
            if(CurrentVideo != null)
            {
                if (isPaused)
                {
                    VideoPlayerManager.Instance.Player.Pause();
                }
                else
                {
                    VideoPlayerManager.Instance.Player.Play();
                }
            }
        }

        private void OnMenuSceneLoaded(UnityEngine.SceneManagement.Scene scene)
        {
            if(CurrentVideo != null)
            {
                VideoPlayerManager.Instance.StopVideo();
            }

            VideoPlayerManager.Instance.SetPlayerActive(false);
        }

        private void OnEnvironmentSceneLoaded(UnityEngine.SceneManagement.Scene scene)
        {
            VideoPlayerManager.Instance.SetPlayerActive(true);

            if(CurrentVideo != null)
            {
                VideoPlayerManager.Instance.PlayVideo();
            }
        }

        private void OnSongSelected(int index, string songPath, string songName)
        {
            SetVideo(songName);
        }

        private void SetVideo(string songName)
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
