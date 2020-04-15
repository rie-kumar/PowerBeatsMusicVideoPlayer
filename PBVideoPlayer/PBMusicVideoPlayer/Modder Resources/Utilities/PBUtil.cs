using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace PBMusicVideoPlayer
{
    public class PBUtil : Singleton<PBUtil>
    {
        private GameObject parent;

        public void OnLoad()
        {
            //Logger.Instance.Log("Creating canvas", Logger.LogSeverity.DEBUG);

            //parent = new GameObject();
            //Canvas canvas = parent.AddComponent<Canvas>();
            //canvas.renderMode = RenderMode.WorldSpace;
            //parent.AddComponent<CanvasScaler>();
            //parent.AddComponent<GraphicRaycaster>();
            //parent.transform.position = new Vector3(0f, 0f, 5);

            //MenuUtil.Instance.MenuReadyEvent += MenuReadyEvent;

            //Logger.Instance.Log("Created canvas", Logger.LogSeverity.DEBUG);
        }

        public void AddMenu()
        {

        }

        public void PrintCameras()
        {
            foreach (var camera in FindObjectsOfType<Camera>())
            {
                Logger.Instance.Log(camera.name, Logger.LogSeverity.DEBUG);
            }
        }

        public void PrintButtons()
        {
            foreach (var button in FindObjectsOfType<Button>())
            {
                Logger.Instance.Log(button.name, Logger.LogSeverity.DEBUG);
            }
        }

        private void MenuReadyEvent(powerbeatsvr.Menu menu)
        {
            Button test = null;

            foreach (var button in FindObjectsOfType<Button>())
            {
                if(button.name == "Official")
                {
                    Logger.Instance.Log("Found the button.", Logger.LogSeverity.DEBUG);
                    test = button;
                    break;
                }
            }

            if(test != null)
            {
                GameObject btnCont = Instantiate(test.gameObject);
                btnCont.transform.parent = parent.transform;
                var t = btnCont.GetComponent<RectTransform>();
                t.localPosition = new Vector3(0, 2, 0);
                t.localScale = new Vector3(10, 10, 10);
            }
        }
    }
}
