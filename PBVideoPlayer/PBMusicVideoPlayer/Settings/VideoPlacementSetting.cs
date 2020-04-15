using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PBMusicVideoPlayer.Settings
{
    public enum VideoPlacement { Desert, Medieval, Space, Unknown };

    public class VideoPlacementSetting
    {
        public static Vector3 Position(VideoPlacement placement)
        {
            switch (placement)
            {
                case VideoPlacement.Desert:
                case VideoPlacement.Medieval:
                case VideoPlacement.Space:
                    return new Vector3(0, 10f, 20f);
                default:
                    return new Vector3(0, 10f, 20f);
            }
        }

        public static Vector3 Rotation(VideoPlacement placement)
        {
            switch (placement)
            {

                case VideoPlacement.Medieval:
                case VideoPlacement.Desert:
                case VideoPlacement.Space:
                    return new Vector3(0, 0, 180f);
                default:
                    return new Vector3(0, 0, 180f);
            }
        }

        public static float Scale(VideoPlacement placement)
        {
            switch (placement)
            {
                case VideoPlacement.Medieval:
                case VideoPlacement.Desert:
                case VideoPlacement.Space:
                    return 10;
                default:
                    return 10;
            }
        }

        public static float[] Modes()
        {
            return new float[]
            {
                (float)VideoPlacement.Desert,
                (float)VideoPlacement.Medieval,
                (float)VideoPlacement.Space,
                (float)VideoPlacement.Unknown
            };
        }
    }
}
