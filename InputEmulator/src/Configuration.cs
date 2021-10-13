using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InputEmulator
{

    static class ConfigManager
    {
        public static readonly Configuration DefaultConfig = new Configuration();
        public static Configuration Config;
    }

    class Configuration
    {
        public Point StartPosition = new Point(300, 300);
        public bool AlwaysOnTop = false, EnableHotkeys = true;
        public Keys Keybind1 = Keys.W;
        public Keys Keybind2 = Keys.None;
        public Keys Keybind3 = Keys.None;
        public Keys Keybind4 = Keys.None;
        public Keys Keybind5 = Keys.None;
    }
}
