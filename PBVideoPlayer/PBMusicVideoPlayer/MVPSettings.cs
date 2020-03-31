using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PBMusicVideoPlayer
{
    public class MVPSettings : Singleton<MVPSettings>
    {
        private Config Config;

        public Vector3 Position 
        {
            get => 
                new Vector3(
                    Config.GetFloat("Position", "X"), 
                    Config.GetFloat("Position", "Y"), 
                    Config.GetFloat("Position", "Z"));
            set
            {
                Config.SetFloat("Position", "X", value.x);
                Config.SetFloat("Position", "Y", value.y);
                Config.SetFloat("Position", "Z", value.z);
            }
        }

        internal void OnLoad()
        {
            Logger.Instance.Log("Created Settings!", Logger.LogSeverity.DEBUG);
        }

        void Awake()
        {
            Logger.Instance.Log("Creating Config!", Logger.LogSeverity.DEBUG);
            Config = new Config("MVP", true);
            Config.OnFileChangedEvent += ConfigUpdated;
        }

        private void ConfigUpdated(object sender, System.IO.FileSystemEventArgs e)
        {
            Logger.Instance.Log("Config Updated", Logger.LogSeverity.DEBUG);
            VideoPlayerManager.Instance.UpdatePosition(Position);
        }
    }
}
