using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWRHelper
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void MPButton_Click(object sender, EventArgs e)
        {
            // Opens the multiplayer form
            MultiPlayerForm form = new MultiPlayerForm();
            this.Hide();
            form.Show();
        }

        private void SPButton_Click(object sender, EventArgs e)
        {
            // Opens the single player form

        }
    }
}
