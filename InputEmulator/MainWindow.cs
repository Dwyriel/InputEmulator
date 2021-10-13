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
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        InputSimulator inputSimulator = new InputSimulator();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            RegisterHotKeys();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKeys();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                int keyValue = m.WParam.ToInt32();
                if (keyValue == (int)Keys.Add)
                    ToggleKey(Keys.W);
            }
            base.WndProc(ref m);
        }

        void ToggleKey(Keys key)
        {
            if (!inputSimulator.InputDeviceState.IsKeyDown((WindowsInput.Native.VirtualKeyCode)key))
                inputSimulator.Keyboard.KeyDown((WindowsInput.Native.VirtualKeyCode)key);
            else
                inputSimulator.Keyboard.KeyUp((WindowsInput.Native.VirtualKeyCode)key);
        }

        private GetKeyStroke GenNewKeystrokeForm()
        {
            return new GetKeyStroke()
            {//todo Icon thingy
                //Icon = Properties.Resources.icon,
                Owner = this,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedSingle,
                TopMost = this.TopMost,
                MaximizeBox = false
            };
        }

        private void RegisterHotKeys()
        {
            RegisterHotKey(this.Handle, (int)Keys.Add, 0, (int)Keys.Add);
        }

        private void UnregisterHotKeys()
        {
            RegisterHotKey(this.Handle, (int)Keys.Add, 0, (int)Keys.Add);
        }
    }
}
