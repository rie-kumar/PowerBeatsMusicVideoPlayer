using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PBMusicVideoPlayer
{
    public class Config
    {
        public event OnFileChanged OnFileChangedEvent;
        public delegate void OnFileChanged(object sender, FileSystemEventArgs e);

        private readonly string fileExtension = ".ini";
        private readonly string ModDataFolderName = "ModsData";

        private Configuration config;
        private FileSystemWatcher watcher;
        private string configFilePath;

        public Config(string configName, bool pollChanges = false)
        {
            string gameFolderPath = Directory.GetCurrentDirectory();
            string dataPath = Path.Combine(gameFolderPath, ModDataFolderName);

            Directory.CreateDirectory(dataPath);

            configFilePath = Path.Combine(dataPath, configName + fileExtension);

            if (!File.Exists(configFilePath))
            {
                var file = File.Create(configFilePath);
                file.Close();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                sb.AppendLine("<configuration>");
                sb.AppendLine("</configuration>");

                File.WriteAllText(configFilePath, sb.ToString());
            }

            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = configFilePath;
            config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            config.Save();

            if (pollChanges)
            {
                watcher = new FileSystemWatcher();
                watcher.Path = dataPath;
                watcher.EnableRaisingEvents = true;
                watcher.Changed += FileChanged;
            }

            Logger.Instance.Log($"Game Path: {gameFolderPath}", Logger.LogSeverity.DEBUG);
            Logger.Instance.Log($"Data Path: {dataPath}", Logger.LogSeverity.DEBUG);
            Logger.Instance.Log($"Config Path: {configFilePath}", Logger.LogSeverity.DEBUG);
        }

        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            if(OnFileChangedEvent != null)
            {
                OnFileChangedEvent(sender, e);
            }
        }

        public void SetString(string sectionName, string name, string value)
        {
            var section = GetSection(sectionName);
            var setting = section.Settings[name];

            if(setting == null)
            {
                section.Settings.Add(name, value);
            }
            else
            {
                setting.Value = value;
            }

            config.Save();
        }

        public string GetString(string sectionName, string name, string defaultValue)
        {
            var section = GetSection(sectionName);
            var setting = section.Settings[name];
            string value = defaultValue;

            if(setting != null)
            {
                value = setting.Value;
            }

            return value;
        }

        public void SetBool(string sectionName, string name, bool value)
        {
            var section = GetSection(sectionName);
            var setting = section.Settings[name];

            if (setting == null)
            {
                section.Settings.Add(name, value.ToString());
            }
            else
            {
                setting.Value = value.ToString();
            }

            config.Save();
        }

        public bool GetBool(string sectionName, string name)
        {
            var section = GetSection(sectionName);
            var setting = section.Settings[name];
            bool value = false;

            if (setting != null)
            {
                bool.TryParse(setting.Value, out value);
            }

            return value;
        }

        public void SetFloat(string sectionName, string name, float value)
        {
            var section = GetSection(sectionName);
            var setting = section.Settings[name];

            if (setting == null)
            {
                section.Settings.Add(name, value.ToString());
            }
            else
            {
                setting.Value = value.ToString();
            }

            config.Save();
        }

        public float GetFloat(string sectionName, string name)
        {
            var section = GetSection(sectionName);
            var setting = section.Settings[name];
            float value = 0.0f;

            if (setting != null)
            {
                float.TryParse(setting.Value, out value);
            }

            Logger.Instance.Log($"Is Null: {setting == null} - {value} of {name} else {setting.Value}", Logger.LogSeverity.DEBUG);

            return value;
        }

        private AppSettingsSection GetSection(string sectionName)
        {
            var section = config.GetSection(sectionName) as AppSettingsSection;

            if (section == null)
            {
                section = new AppSettingsSection();
                config.Sections.Add(sectionName, section);
            }
            else
            {
                ConfigurationManager.RefreshSection(sectionName);
                ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
                configMap.ExeConfigFilename = configFilePath;
                config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            }

            return section;
        }
    }
}
