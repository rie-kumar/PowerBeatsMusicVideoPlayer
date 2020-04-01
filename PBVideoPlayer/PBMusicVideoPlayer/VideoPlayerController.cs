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
            MenuUtil.Instance.SongSelectedEvent += SongSelected;
        }

        private void SongSelected(int index, string songPath, string songName)
        {
            CurrentSong = SongLoader.Instance.GetMappedSong(songName);
            
            if(VideoLoader.Instance.TryGetVideo(CurrentSong, out CurrentVideo))
            {
                //TODO: Prepare Video
            }
            else
            {
                Logger.Instance.Log($"Video could not be found for {songName}", Logger.LogSeverity.DEBUG);
            }
        }
    }
}
