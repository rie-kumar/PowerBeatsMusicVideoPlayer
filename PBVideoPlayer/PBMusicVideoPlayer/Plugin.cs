using powerbeatsvr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PBMusicVideoPlayer
{
    public class Plugin
    {
        public static void Init()
        {
            MenuUtil.Instance.OnLoad();
            VideoPlayerManager.Instance.OnLoad();
            MVPSettings.Instance.OnLoad();
        }
    }
}
