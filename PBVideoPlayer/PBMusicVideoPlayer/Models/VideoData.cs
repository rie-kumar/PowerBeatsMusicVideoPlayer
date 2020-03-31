using PBMusicVideoPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBMusicVideoPlayer.Models
{
    public enum DownloadState { NotDownloaded, Queued, Downloading, Downloaded, Cancelled };

    public class VideoData
    {
        public string Title;
        public string Author;
        public string Description;
        public string Duration;
        public string URL;
        public string ThumbnailURL;
        public bool Loop = false;
        public int Offset = 0; // ms
        public string VideoPath;

        [System.NonSerialized]
        public SongInfoData Song;
        [System.NonSerialized]
        public float DownloadProgress = 0f;
        [System.NonSerialized]
        public DownloadState DownloadState = DownloadState.NotDownloaded;

        public new string ToString()
        {
            return String.Format("{0} by {1} [{2}] \n {3} \n {4} \n {5}", Title, Author, Duration, URL, Description, ThumbnailURL);
        }

        public VideoData(YTResult ytResult, SongInfoData song)
        {
            Title = ytResult.Title;
            Author = ytResult.Author;
            Description = ytResult.Description;
            Duration = ytResult.Duration;
            URL = ytResult.URL;
            ThumbnailURL = ytResult.ThumbnailURL;
            Song = song;
        }
    }
}
