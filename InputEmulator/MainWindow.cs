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
                    Task.Factory.StartNew(someMethod);
            }
            base.WndProc(ref m);
        }

        bool toggled = false;
        void someMethod()
        {
            toggled = !toggled;
            if (toggled)
                inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
            else
                inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
            /*while (toggled)
            {
                if (inputSimulator.InputDeviceState.IsKeyDown(WindowsInput.Native.VirtualKeyCode.VK_W))
                {
                    toggled = !toggled;
                    break;
                }
                inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
                inputSimulator.Keyboard.Sleep(100);
                inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
            }*/
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private void RegisterHotKeys()
        {
            RegisterHotKey(this.Handle, (int)Keys.Add, 0, (int)Keys.Add);
            RegisterHotKey(this.Handle, (int)Keys.W, 0, (int)Keys.W);
        }

        private void UnregisterHotKeys()
        {
            RegisterHotKey(this.Handle, (int)Keys.Add, 0, (int)Keys.Add);
            RegisterHotKey(this.Handle, (int)Keys.W, 0, (int)Keys.W);
        }
    }
}
