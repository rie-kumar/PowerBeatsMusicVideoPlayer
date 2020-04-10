using PBMusicVideoPlayer.Settings;
using powerbeatsvr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PBMusicVideoPlayer
{
    public class Loader
    {
        public static void Init()
        {
            try
            {
                SongLoader.Instance.OnLoad();
                MenuUtil.Instance.OnLoad();
                MVPSettings.Instance.OnLoad();
                VideoLoader.Instance.OnLoad();
                VideoPlayerManager.Instance.OnLoad();
                VideoPlayerViewController.Instance.OnLoad();
            }
            catch (Exception e)
            {
                Logger.Instance.Log(e.ToString(), Logger.LogSeverity.DEBUG);
            }
            
        }
    }
}
