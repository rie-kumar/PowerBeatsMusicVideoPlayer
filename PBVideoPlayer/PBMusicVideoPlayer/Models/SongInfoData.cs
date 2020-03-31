using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBMusicVideoPlayer.Models
{
    public class SongInfoData
    {
        public int Index;
        public string Path;

        public SongInfoData(int index, string path)
        {
            Index = index;
            Path = path;
        }

        public override string ToString()
        {
            return $"Index: {Index}\nPath: {Path}";
        }
    }
}
