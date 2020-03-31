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
        public Dictionary<string, string> CustomSongs;

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
                CustomSongs.Add(Path.GetFileName(song), song);
                Logger.Instance.Log($"Song: {Path.GetFileName(song)}, Path: {song}", Logger.LogSeverity.DEBUG);
                Directory.CreateDirectory(Path.Combine(customSongsFullPath, song));
            }
        }
    }
}
