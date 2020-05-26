using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoorDecCreator
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();

            // Removes the ability to change window size
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Open the singl form and closes the previous one
            this.Hide(); // Hides the current form
            SingleDecForm f2 = new SingleDecForm();
            f2.Closed += (s, args) => this.Close();
            f2.Show();
        }

        private void MultiDoorDecButton_Click(object sender, EventArgs e)
        {
            // Open the singl form and closes the previous one
            this.Hide(); // Hides the current form
            MultipleDecForm f2 = new MultipleDecForm();
            f2.Closed += (s, args) => this.Close();
            f2.Show();
        }
    }
}
