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
            Config = new Config("MVP");

            UpdateSettings();
        }

        private void UpdateSettings()
        {
            try
            {
                if (Enum.TryParse(Config.GetString("Format", "Quality", "Best"), out VideoQuality qualityParsed))
                    quality = qualityParsed;
                else
                    quality = VideoQuality.Best;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("Crashed due to " + e, Logger.LogSeverity.ERROR);
            }
        }
    }
}
