using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBMusicVideoPlayer.Models
{
    public class YTResult
    {
        public string Title;
        public string Author;
        public string Description;
        public string Duration;
        public string URL;
        public string ThumbnailURL;

        public new string ToString()
        {
            return String.Format("{0} by {1} [{2}] \n {3} \n {4} \n {5}", Title, Author, Duration, URL, Description, ThumbnailURL);
        }
    }
}
