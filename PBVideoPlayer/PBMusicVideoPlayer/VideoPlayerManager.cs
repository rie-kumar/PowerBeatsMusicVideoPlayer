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

        private class VideoData
        {
            public int customSongIndex;
            public string customSongPath;

            public VideoData(int index, string path)
            {
                customSongIndex = index;
                customSongPath = path;
            }

            public override string ToString()
            {
                return $"Index: {customSongIndex}\nPath: {customSongPath}"; 
            }
        }

        public void OnLoad()
        {
            Logger.Instance.Log("Hooked!", Logger.LogSeverity.DEBUG);
        }

        void Start()
        {
            CreatePlayer();
            Logger.Instance.Log("Created Player!", Logger.LogSeverity.DEBUG);
        }

        public void CreatePlayer()
        {
            Logger.Instance.Log("Creating Container!", Logger.LogSeverity.DEBUG);
            //Creates the container for the video player.
            container = GameObject.CreatePrimitive(PrimitiveType.Cube);
            container.transform.parent = gameObject.transform;
            container.transform.localScale = new Vector3(16f / 9f + 0.1f, 1.1f, 0.01f);
            Renderer renderer = container.GetComponent<Renderer>();
            Logger.Instance.Log("Creating Renderer!", Logger.LogSeverity.DEBUG);
            renderer.material = new Material(Shader.Find("Standard"));
            renderer.material.color = Color.clear;

            Logger.Instance.Log("Creating Player!", Logger.LogSeverity.DEBUG);
            //Attach the player to that container.
            Player = container.AddComponent<VideoPlayer>();
            Player.isLooping = true;
            Player.audioOutputMode = VideoAudioOutputMode.None;
            Player.SetDirectAudioMute(0, true);

            Player.errorReceived += ErrorRecieved;
        }

        public void UpdatePosition(Vector3 position)
        {
            container.transform.position = position;
            Logger.Instance.Log(position.ToString(), Logger.LogSeverity.DEBUG);
        }

        private void ErrorRecieved(VideoPlayer source, string message)
        {
            Logger.Instance.Log(message, Logger.LogSeverity.ERROR);
        }
    }
}
