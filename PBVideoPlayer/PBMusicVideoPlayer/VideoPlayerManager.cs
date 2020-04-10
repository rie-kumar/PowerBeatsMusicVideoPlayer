using PBMusicVideoPlayer.Models;
using PBMusicVideoPlayer.Settings;
using powerbeatsvr;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace PBMusicVideoPlayer
{
    public class VideoPlayerManager : Singleton<VideoPlayerManager>
    {
        public VideoPlayer Player;
        private GameObject container;
        private Renderer videoRenderer;
        private VideoData currentVideo;
        private readonly Vector3 defaultScale = new Vector3(16f / 9f + 0.1f, 1.1f, 0.01f);
        private readonly Vector3 multScale = new Vector3(16f / 9f + 0.1f, 1.1f, 0.01f);

        public void OnLoad()
        {
        }

        void Start()
        {
            CreatePlayer();
        }

        public void CreatePlayer()
        {
            //Creates the container for the video player.
            container = GameObject.CreatePrimitive(PrimitiveType.Cube);
            container.transform.parent = gameObject.transform;
            SetPosition(MVPSettings.Instance.Placement);
            SetRotation(MVPSettings.Instance.Placement);
            SetScale(MVPSettings.Instance.Placement);
            videoRenderer = container.GetComponent<Renderer>();
            videoRenderer.material = new Material(Shader.Find("Unlit/Texture"));
            //videoRenderer.material.color = Color.clear;

            //Attach the player to that container.
            Player = container.AddComponent<VideoPlayer>();
            Player.isLooping = false;
            Player.audioOutputMode = VideoAudioOutputMode.None;
            Player.errorReceived += ErrorRecieved;

            SetPlayerActive(false);
            //TestVideo();
        }

        public void PrepareVideo(VideoData video)
        {
            Player.Stop();

            Player.url = video != null ? VideoLoader.Instance.GetVideoPath(video) : null;
            Player.Prepare();
            Player.Pause();

            if (video != null && video.DownloadState == DownloadState.Downloaded)
            {
                Player.isLooping = video.Loop;

                float offsetInSeconds = video.Offset / 1000f; // ms -> s
                if (video.Offset >= 0)
                {
                    Player.time = video.Offset;
                }
                else
                {
                    Player.time = 0;
                }
            }

            currentVideo = video;
        }

        public void StopVideo()
        {
            Player.Stop();
        }

        public void PlayVideo()
        {
            Player.Stop();

            if (IsVideoPlayable())
            {
                if (!Player.isPrepared)
                {
                    PrepareVideo(currentVideo);
                }

                if(currentVideo.Offset < 0)
                {
                    StopAllCoroutines();

                    Player.time = 0;
                    StartCoroutine(PlayDelayedVideo());
                }
                else
                {
                    Player.time = currentVideo.Offset;
                    Player.Play();
                }

                Logger.Instance.Log($"Playing: \n{currentVideo.ToString()}!", Logger.LogSeverity.DEBUG);
            }
            else
            {
                Logger.Instance.Log("Failed to play video because there is no video set!", Logger.LogSeverity.ERROR);
            }
        }

        public void SetPlacement(VideoPlacement placement)
        {
            SetPosition(placement);
            SetScale(placement);
            SetRotation(placement);
        }

        public void SetPosition(VideoPlacement placement)
        {
            var position = VideoPlacementSetting.Position(placement);
            container.transform.position = position;
            Logger.Instance.Log($"Player moved to {position.ToString()}", Logger.LogSeverity.DEBUG);
        }

        public void SetScale(VideoPlacement placement)
        {
            var scale = VideoPlacementSetting.Scale(placement) * multScale;
            Logger.Instance.Log($"Player scaled from {container.transform.localScale.ToString()}", Logger.LogSeverity.DEBUG);
            container.transform.localScale = defaultScale + scale;
            Logger.Instance.Log($"Player scaled to {container.transform.localScale.ToString()}", Logger.LogSeverity.DEBUG);
        }

        public void SetRotation(VideoPlacement placement)
        {
            var rotation = VideoPlacementSetting.Rotation(placement);
            container.transform.eulerAngles = rotation;
            Logger.Instance.Log($"Player rotated to {rotation.ToString()}", Logger.LogSeverity.DEBUG);
        }

        public void SetPlayerActive(bool isShowing)
        {
            container.SetActive(isShowing);
        }

        public bool IsVideoPlayable()
        {
            return currentVideo != null && currentVideo.DownloadState == DownloadState.Downloaded;
        }

        private IEnumerator PlayDelayedVideo()
        {
            yield return new WaitForSeconds((currentVideo.Offset * -1) / 1000);

            Player.Play();
        }

        private void ErrorRecieved(VideoPlayer source, string message)
        {
            Logger.Instance.Log(message, Logger.LogSeverity.ERROR);
        }

        private void TestVideo()
        {
            Player.url = @"E:\Games\Steam\steamapps\common\PowerBeatsVR\video.mp4";
            Player.Play();
        }
    }
}
