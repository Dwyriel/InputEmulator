using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InputEmulator
{
    public partial class GetKeyStroke : Form
    {
        public Keys KeyPressed = Keys.None;

        public GetKeyStroke()
        {
            InitializeComponent();
        }

        private void GetKeyStroke_Load(object sender, EventArgs e)
        {
            KeyDown += GetKeyStroke_KeyDown;
        }

        private void GetKeyStroke_KeyDown(object sender, KeyEventArgs e)
        {
            KeyPressed = e.KeyCode;
            Close();
        }
    }
}
