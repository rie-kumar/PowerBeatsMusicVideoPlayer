using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PBMusicVideoPlayer
{
    public class Logger : Singleton<Logger>
    {
        private StringBuilder log = new StringBuilder();
        private static Queue<LogMessage> messages = new Queue<LogMessage>();

        public void Log(string message, LogSeverity severity)
        {
            messages.Enqueue(new LogMessage(message, severity));
        }

        void OnGUI()
        {
            if(messages.Count > 0)
            {
                var message = messages.Dequeue();

                switch (message.Severity)
                {
                    case LogSeverity.INFO:
                        GUI.color = Color.white;
                        break;
                    case LogSeverity.DEBUG:
                        GUI.color = Color.cyan;
                        break;
                    case LogSeverity.WARN:
                        GUI.color = Color.yellow;
                        break;
                    case LogSeverity.ERROR:
                        GUI.color = Color.red;
                        break;
                    default:
                        break;
                }

                log.PrependLine(message.Message);
            }

            GUI.Label(new Rect(0, 0, Screen.width/2, Screen.height), log.ToString());
        }

        public enum LogSeverity
        {
            INFO,
            DEBUG,
            WARN,
            ERROR
        }

        private class LogMessage
        {
            public string Message { get; set; }
            public LogSeverity Severity { get; set; }

            public LogMessage(string msg, LogSeverity severity)
            {
                Message = msg;
                Severity = severity;
            }
        }
    }
}
