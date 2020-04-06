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

        private Vector3 customPosition;
        public Vector3 CustomPosition 
        {
            get => customPosition;
            set
            {
                customPosition = value;
                Config.SetFloat("CustomPosX", "X", value.x);
                Config.SetFloat("CustomPosY", "Y", value.y);
                Config.SetFloat("CustomPosZ", "Z", value.z);
            }
        }

        private float customScale;
        public float CustomScale
        {
            get => customScale;
            set
            {
                customScale = value;
                Config.SetFloat("CustomScale", "Amount", value);
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
            try
            {
                if (Enum.TryParse(Config.GetString("Placement", "Position", "Background"), out VideoPlacement placementParsed))
                    placement = placementParsed;
                else
                    placement = VideoPlacement.Custom;

                if (Enum.TryParse(Config.GetString("Format", "Quality", "Best"), out VideoQuality qualityParsed))
                    quality = qualityParsed;
                else
                    quality = VideoQuality.Best;

                customPosition = new Vector3(
                        Config.GetFloat("CustomPosX", "X"),
                        Config.GetFloat("CustomPosY", "Y"),
                        Config.GetFloat("CustomPosZ", "Z"));

                customScale = Config.GetFloat("CustomScale", "Amount");
            }
            catch (Exception e)
            {
                Logger.Instance.Log("Crashed due to " + e, Logger.LogSeverity.ERROR);
            }
        }

        private void ConfigUpdated(object sender, System.IO.FileSystemEventArgs e)
        {
            UpdateSettings();
            Logger.Instance.Log("Config Updated", Logger.LogSeverity.DEBUG);

            VideoPlayerManager.Instance.SetPlacement(placement);
        }
    }
}
