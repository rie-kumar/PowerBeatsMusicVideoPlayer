using PBMusicVideoPlayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PBMusicVideoPlayer
{
    public class VideoLoader : Singleton<VideoLoader>
    {
        public bool IsRetrieving = false;
        public event Action VideosLoadedEvent; 

        private Dictionary<MappedCustomSong, VideoData> videos = new Dictionary<MappedCustomSong, VideoData>();

        public static void SaveVideoToDisk(VideoData video)
        {
            if (video == null)
            {
                return;
            }

            if (video.Song != null)
            {
                Directory.CreateDirectory(SongLoader.Instance.CustomSongsFullPath);
            }

            File.WriteAllText(Path.Combine(video.Song.ModPath, "video.json"), JsonUtility.ToJson(video));
        }

        public void OnLoad()
        {
            RetrieveVideoData();
        }

        public bool TryGetVideo(MappedCustomSong song, out VideoData video)
        {
            if (song == null || !videos.TryGetValue(song, out video))
            {
                video = null;
            }

            return video != null;
        }

        public bool TryGetVideo(string songName, out VideoData video)
        {
            if (string.IsNullOrEmpty(songName) || !videos.TryGetValue(SongLoader.Instance.GetMappedSong(songName), out video))
            {
                video = null;
            }

            return video != null;
        }

        public void AddVideo(VideoData video)
        {
            videos.Add(video.Song, video);
        }

        public void RemoveVideo(VideoData video)
        {
            videos.Remove(video.Song);
        }

        public void DeleteVideo(VideoData video)
        {
            RemoveVideo(video);
            File.Delete(GetJSONPath(video.Song.ModPath));
            File.Delete(GetVideoPath(video));
        }

        public void RetrieveVideoData()
        {
            Task.Run(() =>
            {
                IsRetrieving = true;

                foreach (var customSongKVP in SongLoader.Instance.CustomSongs)
                {
                    string videoJsonPath = GetJSONPath(customSongKVP.Value.ModPath);

                    if(File.Exists(videoJsonPath))
                    {
                        var video = LoadVideo(videoJsonPath, customSongKVP.Value);
                        
                        if(video != null)
                        {
                            AddVideo(video);
                        }
                    }
                }

                IsRetrieving = false;
                VideosLoadedEvent?.Invoke();
            });
        }

        public string GetVideoPath(VideoData video)
        {
            return Path.Combine(video.Song.ModPath, video.VideoPath);
        }

        private VideoData LoadVideo(string videoJsonPath, MappedCustomSong customSong)
        {
            var json = File.ReadAllText(videoJsonPath);
            VideoData video = null;

            try
            {
                video = JsonUtility.FromJson<VideoData>(json);
                video.Song = customSong;

                if(File.Exists(GetVideoPath(video)))
                {
                    video.DownloadState = DownloadState.Downloaded;
                }
            }
            catch
            {
                Logger.Instance.Log($"Failed to parse video json at {videoJsonPath}", Logger.LogSeverity.ERROR);
            }

            return video;
        }

        private string GetJSONPath(string modPath)
        {
            return Path.Combine(modPath, "video.json");
        }
    }
}
