using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBMusicVideoPlayer
{
    public static class HelperExtensions
    {
        public static StringBuilder Prepend(this StringBuilder sb, string content)
        {
            return sb.Insert(0, content);
        }

        public static StringBuilder PrependLine(this StringBuilder sb, string content)
        {
            return sb.Insert(0, content + Environment.NewLine);
        }
    }
}
