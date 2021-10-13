using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace InputEmulator
{

    static class ConfigManager
    {
        private static readonly string configFolderName = "Dwyriel";
        private static readonly string configFileName = "InputEmulator.ini";
        private static string configFilePath;
        private static string configFolderPath;

        public static readonly Configuration DefaultConfig = new Configuration();
        public static Configuration Config;
        public static bool ConfigFileExists { get { return File.Exists(configFilePath); } }

        public static void Initialize()
        {
            bool loaded = false;
            configFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), configFolderName);
            configFilePath = Path.Combine(configFolderPath, configFileName);
            if (File.Exists(configFilePath))
                loaded = Load();
            if (loaded)
                return;
            Config = DefaultConfig;
        }

        public static void Save()
        {
            try
            {
                if (!Directory.Exists(configFolderPath))
                    Directory.CreateDirectory(configFolderPath);
                FileStream outFile = File.Create(configFilePath);
                try
                {
                    XmlSerializer format = new XmlSerializer(typeof(Configuration));
                    format.Serialize(outFile, Config);
                }
                catch (Exception e)
                {
                    ErrorLogger.ShowErrorTextWithExceptionMessage($"An error occurred while saving cache to {configFilePath}", e, true);
                }
                finally
                {
                    outFile.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowErrorTextWithExceptionMessage($"An error occurred while saving cache to {configFilePath}", ex, true);
            }
        }

        public static bool Load()
        {
            XmlSerializer format = new XmlSerializer(typeof(Configuration));
            FileStream inFile = new FileStream(configFilePath, FileMode.Open);
            try
            {
                byte[] buffer = new byte[inFile.Length];
                inFile.Read(buffer, 0, (int)inFile.Length);
                MemoryStream stream = new MemoryStream(buffer);
                Config = (Configuration)format.Deserialize(stream);
                return true;
            }
            catch (Exception e)
            {
                ErrorLogger.ShowErrorTextWithExceptionMessage($"An error occurred while reading {configFilePath}", e, true);
                return false;
            }
            finally
            {
                inFile.Close();
            }
        }
    }

    public class Configuration
    {
        public Point StartPosition = new Point(300, 300);
        public bool AlwaysOnTop = false, EnableHotkeys = true, extraButtons = false;
        public InputHotkey InputHotkey1 = new InputHotkey(Keys.W, Keys.D1);
        public InputHotkey InputHotkey2 = new InputHotkey();
        public InputHotkey InputHotkey3 = new InputHotkey();
        public InputHotkey InputHotkey4 = new InputHotkey();
        public InputHotkey InputHotkey5 = new InputHotkey();
    }

    public class InputHotkey
    {
        public int input;
        public int hotkey;

        public InputHotkey()
        {
            input = (int)Keys.None;
            hotkey = (int)Keys.None;
        }

        public InputHotkey(Keys input, Keys hotkey)
        {
            this.input = (int)input;
            this.hotkey = (int)hotkey;
        }
    }
}
