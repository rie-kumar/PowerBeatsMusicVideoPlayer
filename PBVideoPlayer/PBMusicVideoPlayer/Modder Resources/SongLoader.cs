using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBMusicVideoPlayer
{
    public class SongLoader : Singleton<SongLoader>
    {
        public string customSongsFullPath = string.Empty;
        public Dictionary<string, MappedCustomSong> CustomSongs = new Dictionary<string, MappedCustomSong>();

        private readonly string layoutsPath = "PowerBeatsVR_Data\\Layouts";
        private readonly string customSongsPath = "CustomSongs";
        private string layoutsFullPath = string.Empty;

        public void OnLoad()
        {
            string gameFolderPath = Directory.GetCurrentDirectory();
            layoutsFullPath = Path.Combine(gameFolderPath, layoutsPath);
            customSongsFullPath = Path.Combine(layoutsFullPath, customSongsPath);

            Directory.CreateDirectory(customSongsFullPath);

            foreach (var song in Directory.GetFiles(layoutsFullPath))
            {
                string songName = Path.GetFileNameWithoutExtension(song);
                string songModPath = Path.Combine(customSongsFullPath, songName);
                CustomSongs.Add(songName, new MappedCustomSong(songName, song, songModPath));
                Directory.CreateDirectory(songModPath);
                Logger.Instance.Log($"Song: {songName}, Path: {song}", Logger.LogSeverity.DEBUG);
            }
        }
    }
}
