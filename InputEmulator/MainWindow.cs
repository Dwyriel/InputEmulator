using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;

namespace InputEmulator
{
    public partial class MainWindow : Form
    {
        #region DLL Imports
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(int hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int FindWindowEx(int parentHandle, int childAfter, string lclassName, string windowTitle);
        #endregion

        InputSimulator inputSimulator = new InputSimulator();

        #region Controctor
        public MainWindow()
        {
            ConfigManager.Initialize();
            SetStartAttributes();
            InitializeComponent();
        }
        #endregion

        #region Events
        #region Form Events
        private void MainWindow_Load(object sender, EventArgs e)
        {
            SetFormVariables();
            SetButtonsText();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConfigManager.Config.StartPosition = Location;
            ConfigManager.Save();
            if (ConfigManager.Config.EnableHotkeys)
                UnregisterHotKeys();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                int keyValue = m.WParam.ToInt32();
                if (keyValue == ConfigManager.Config.InputHotkey1.hotkey)
                    ToggleKey((WindowsInput.Native.VirtualKeyCode)ConfigManager.Config.InputHotkey1.input);
                if (keyValue == ConfigManager.Config.InputHotkey2.hotkey)
                    ToggleKey((WindowsInput.Native.VirtualKeyCode)ConfigManager.Config.InputHotkey2.input);
                if (keyValue == ConfigManager.Config.InputHotkey3.hotkey)
                    ToggleKey((WindowsInput.Native.VirtualKeyCode)ConfigManager.Config.InputHotkey3.input);
                if (keyValue == ConfigManager.Config.InputHotkey4.hotkey)
                    ToggleKey((WindowsInput.Native.VirtualKeyCode)ConfigManager.Config.InputHotkey4.input);
                if (keyValue == ConfigManager.Config.InputHotkey5.hotkey)
                    ToggleKey((WindowsInput.Native.VirtualKeyCode)ConfigManager.Config.InputHotkey5.input);
            }
            base.WndProc(ref m);
        }
        #endregion

        #region Input And Hotkeys Buttons
        private void Input1Btn_Click(object sender, EventArgs e)
        {
            SetInputOrHotkey(ref ConfigManager.Config.InputHotkey1.input);
        }

        private void Input2Btn_Click(object sender, EventArgs e)
        {
            SetInputOrHotkey(ref ConfigManager.Config.InputHotkey2.input);
        }

        private void Input3Btn_Click(object sender, EventArgs e)
        {
            SetInputOrHotkey(ref ConfigManager.Config.InputHotkey3.input);
        }

        private void Input4Btn_Click(object sender, EventArgs e)
        {
            SetInputOrHotkey(ref ConfigManager.Config.InputHotkey4.input);
        }

        private void Input5Btn_Click(object sender, EventArgs e)
        {
            SetInputOrHotkey(ref ConfigManager.Config.InputHotkey5.input);
        }

        private void Hotkey1Btn_Click(object sender, EventArgs e)
        {
            SetInputOrHotkey(ref ConfigManager.Config.InputHotkey1.hotkey);
        }

        private void Hotkey2Btn_Click(object sender, EventArgs e)
        {
            SetInputOrHotkey(ref ConfigManager.Config.InputHotkey2.hotkey);
        }

        private void Hotkey3Btn_Click(object sender, EventArgs e)
        {
            SetInputOrHotkey(ref ConfigManager.Config.InputHotkey3.hotkey);
        }

        private void Hotkey4Btn_Click(object sender, EventArgs e)
        {
            SetInputOrHotkey(ref ConfigManager.Config.InputHotkey4.hotkey);
        }

        private void Hotkey5Btn_Click(object sender, EventArgs e)
        {
            SetInputOrHotkey(ref ConfigManager.Config.InputHotkey5.hotkey);
        }

        private void toggleInput1Btn_Click(object sender, EventArgs e)
        {
            FindGameWindow();
            ToggleTimer(ConfigManager.Config.InputHotkey1.input);
        }

        private void toggleInput2Btn_Click(object sender, EventArgs e)
        {
            FindGameWindow();
            ToggleTimer(ConfigManager.Config.InputHotkey2.input);
        }

        private void toggleInput3Btn_Click(object sender, EventArgs e)
        {
            FindGameWindow();
            ToggleTimer(ConfigManager.Config.InputHotkey3.input);
        }

        private void toggleInput4Btn_Click(object sender, EventArgs e)
        {
            FindGameWindow();
            ToggleTimer(ConfigManager.Config.InputHotkey4.input);
        }

        private void toggleInput5Btn_Click(object sender, EventArgs e)
        {
            FindGameWindow();
            ToggleTimer(ConfigManager.Config.InputHotkey5.input);
        }
        #endregion

        #region Check Boxes
        private void HotkeysCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ConfigManager.Config.EnableHotkeys)
                UnregisterHotKeys();
            ConfigManager.Config.EnableHotkeys = HotkeysCheckbox.Checked;
            if (ConfigManager.Config.EnableHotkeys)
                RegisterHotKeys();
        }

        private void AlwaysOnTopCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ConfigManager.Config.AlwaysOnTop = AlwaysOnTopCheckbox.Checked;
            TopMost = ConfigManager.Config.AlwaysOnTop;
        }
        #endregion
        #endregion

        #region Methods
        void SetStartAttributes()
        {
            if (!ConfigManager.ConfigFileExists)
                StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Icon = Properties.Resources.icon; TopMost = true;
        }

        void SetFormVariables()
        {
            if (ConfigManager.ConfigFileExists)
                Location = ConfigManager.Config.StartPosition;
            if (ConfigManager.Config.EnableHotkeys)
                RegisterHotKeys();
            TopMost = ConfigManager.Config.AlwaysOnTop;
            HotkeysCheckbox.Checked = ConfigManager.Config.EnableHotkeys;
            AlwaysOnTopCheckbox.Checked = ConfigManager.Config.AlwaysOnTop;
        }

        void SetButtonsText()
        {
            KeysConverter keysConverter = new KeysConverter();
            Input1Btn.Text = keysConverter.ConvertToInvariantString(ConfigManager.Config.InputHotkey1.input);
            Input2Btn.Text = keysConverter.ConvertToString(ConfigManager.Config.InputHotkey2.input);
            Input3Btn.Text = keysConverter.ConvertToString(ConfigManager.Config.InputHotkey3.input);
            Input4Btn.Text = keysConverter.ConvertToString(ConfigManager.Config.InputHotkey4.input);
            Input5Btn.Text = keysConverter.ConvertToString(ConfigManager.Config.InputHotkey5.input);
            Hotkey1Btn.Text = keysConverter.ConvertToString(ConfigManager.Config.InputHotkey1.hotkey);
            Hotkey2Btn.Text = keysConverter.ConvertToString(ConfigManager.Config.InputHotkey2.hotkey);
            Hotkey3Btn.Text = keysConverter.ConvertToString(ConfigManager.Config.InputHotkey3.hotkey);
            Hotkey4Btn.Text = keysConverter.ConvertToString(ConfigManager.Config.InputHotkey4.hotkey);
            Hotkey5Btn.Text = keysConverter.ConvertToString(ConfigManager.Config.InputHotkey5.hotkey);
        }

        void SetInputOrHotkey(ref int value)
        {
            if (ConfigManager.Config.EnableHotkeys)
                UnregisterHotKeys();
            GetKeyStroke getKeyStroke = GenNewKeystrokeForm();
            getKeyStroke.ShowDialog();
            if (getKeyStroke.KeyPressed != Keys.None)
            {
                value = (int)getKeyStroke.KeyPressed;
                SetButtonsText();
            }
            getKeyStroke.Dispose();
            if (ConfigManager.Config.EnableHotkeys)
                RegisterHotKeys();
            ConfigManager.Save();
        }

        private GetKeyStroke GenNewKeystrokeForm()
        {
            return new GetKeyStroke()
            {
                Icon = Properties.Resources.icon,
                Owner = this,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedSingle,
                TopMost = this.TopMost,
                MaximizeBox = false
            };
        }

        void ToggleKey(WindowsInput.Native.VirtualKeyCode key)
        {
            if (!inputSimulator.InputDeviceState.IsKeyDown(key))
                inputSimulator.Keyboard.KeyDown(key);
            else
                inputSimulator.Keyboard.KeyUp(key);
        }

        void FindGameWindow()
        {
            int hWnd = FindWindowEx(0, 0, null, "MONSTER HUNTER STORIES 2: WINGS OF RUIN");
            if (hWnd != 0)
                SetForegroundWindow(hWnd);
            else
                AltTab();
        }

        void AltTab()
        {
            inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.MENU);
            inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.TAB);
            inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.TAB);
            inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.MENU);
        }

        void ToggleTimer(int value, int delay = 200)//!wasn't working without the delay, even when finding the right windows and setting foreground before continuing
        {
            Timer timer = new Timer();
            timer.Tick += (object sender2, EventArgs e2) =>
            {
                ToggleKey((WindowsInput.Native.VirtualKeyCode)value);
                timer.Stop();
                timer.Dispose();
            };
            timer.Interval = delay;
            timer.Start();
        }

        private void RegisterHotKeys()
        {
            if (ConfigManager.Config.InputHotkey1.input != 0 && ConfigManager.Config.InputHotkey1.hotkey != 0)
                RegisterHotKey(this.Handle, ConfigManager.Config.InputHotkey1.hotkey, 0, ConfigManager.Config.InputHotkey1.hotkey);
            if (ConfigManager.Config.InputHotkey2.input != 0 && ConfigManager.Config.InputHotkey2.hotkey != 0)
                RegisterHotKey(this.Handle, ConfigManager.Config.InputHotkey2.hotkey, 0, ConfigManager.Config.InputHotkey2.hotkey);
            if (ConfigManager.Config.InputHotkey3.input != 0 && ConfigManager.Config.InputHotkey3.hotkey != 0)
                RegisterHotKey(this.Handle, ConfigManager.Config.InputHotkey3.hotkey, 0, ConfigManager.Config.InputHotkey3.hotkey);
            if (ConfigManager.Config.InputHotkey4.input != 0 && ConfigManager.Config.InputHotkey4.hotkey != 0)
                RegisterHotKey(this.Handle, ConfigManager.Config.InputHotkey4.hotkey, 0, ConfigManager.Config.InputHotkey4.hotkey);
            if (ConfigManager.Config.InputHotkey5.input != 0 && ConfigManager.Config.InputHotkey5.hotkey != 0)
                RegisterHotKey(this.Handle, ConfigManager.Config.InputHotkey5.hotkey, 0, ConfigManager.Config.InputHotkey5.hotkey);
        }

        private void UnregisterHotKeys()
        {
            if (ConfigManager.Config.InputHotkey1.input != 0 && ConfigManager.Config.InputHotkey1.hotkey != 0)
                UnregisterHotKey(this.Handle, ConfigManager.Config.InputHotkey1.hotkey);
            if (ConfigManager.Config.InputHotkey2.input != 0 && ConfigManager.Config.InputHotkey2.hotkey != 0)
                UnregisterHotKey(this.Handle, ConfigManager.Config.InputHotkey2.hotkey);
            if (ConfigManager.Config.InputHotkey3.input != 0 && ConfigManager.Config.InputHotkey3.hotkey != 0)
                UnregisterHotKey(this.Handle, ConfigManager.Config.InputHotkey3.hotkey);
            if (ConfigManager.Config.InputHotkey4.input != 0 && ConfigManager.Config.InputHotkey4.hotkey != 0)
                UnregisterHotKey(this.Handle, ConfigManager.Config.InputHotkey4.hotkey);
            if (ConfigManager.Config.InputHotkey5.input != 0 && ConfigManager.Config.InputHotkey5.hotkey != 0)
                UnregisterHotKey(this.Handle, ConfigManager.Config.InputHotkey5.hotkey);
        }
        #endregion
    }
}
