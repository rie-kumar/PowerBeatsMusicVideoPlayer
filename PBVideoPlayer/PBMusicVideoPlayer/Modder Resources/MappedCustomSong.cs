using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBMusicVideoPlayer
{
    public class MappedCustomSong
    {
        public string SongName { get; set; }
        public string LayoutPath { get; set; }
        public string ModPath { get; set; }

        public MappedCustomSong(string songName, string layoutPath, string modPath)
        {
            SongName = songName;
            LayoutPath = layoutPath;
            ModPath = modPath;
        }
    }
}
