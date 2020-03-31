using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PBMusicVideoPlayer.Settings
{
    public class MVPSettings : Singleton<MVPSettings>
    {
        private Config Config;

        private VideoQuality quality;
        public VideoQuality Quality
        {
            get => quality;
            set
            {
                quality = value;
                Config.SetString("Format", "Quality", quality.ToString());
            }
        }

        private VideoPlacement placement;
        public VideoPlacement Placement
        {
            get => placement;
            set
            {
                placement = value;
                Config.SetString("Placement", "Position", placement.ToString());
            }
        }

        private Vector3 position;
        public Vector3 Position 
        {
            get => position;
            set
            {
                position = value;
                Config.SetFloat("Placement", "X", value.x);
                Config.SetFloat("Placement", "Y", value.y);
                Config.SetFloat("Placement", "Z", value.z);
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

            UpdateSettings();
        }

        private void UpdateSettings()
        {
            if (Enum.TryParse(Config.GetString("Placement", "Position", "Bottom"), out VideoPlacement placementParsed))
                placement = placementParsed;
            else
                placement = VideoPlacement.Bottom;

            if (Enum.TryParse(Config.GetString("Format", "Quality", "Best"), out VideoQuality qualityParsed))
                quality = qualityParsed;
            else
                quality = VideoQuality.Best;

            position = new Vector3(
                    Config.GetFloat("Placement", "X"),
                    Config.GetFloat("Placement", "Y"),
                    Config.GetFloat("Placement", "Z"));
        }

        private void ConfigUpdated(object sender, System.IO.FileSystemEventArgs e)
        {
            Logger.Instance.Log("Config Updating", Logger.LogSeverity.DEBUG);
            UpdateSettings();
            Logger.Instance.Log("Config Updated", Logger.LogSeverity.DEBUG);

            VideoPlayerManager.Instance.SetPosition(placement);
        }
    }
}
