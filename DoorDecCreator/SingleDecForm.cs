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
    public partial class SingleDecForm : Form
    {
        Image pic; // Original picture
        const int resolution = 96;

        public SingleDecForm()
        {
            InitializeComponent();
        }

        // Updates the picture
        private void UpdateImage()
        {
            // Sets dimensions
            int titleHeight = (int)(Convert.ToDouble(titleBox.Text) * resolution);
            int height = (int)(Convert.ToDouble(TotalHeightTextBox.Text) * resolution);
            int width = (int)(Convert.ToDouble(TotalWidthTextBox.Text) * resolution);

            // Create a new image to put this image into
            Image total = new Bitmap(width, height + titleHeight);

            // Draws
            using (Graphics g = Graphics.FromImage(total))
            {
                // Draws the background
                Brush brush = new SolidBrush(Color.White);
                g.FillRectangle(brush, new Rectangle(0, 0, total.Width, total.Height));

                // Draws the image on the bottom
                g.DrawImage(pic, new Rectangle(new Point(0, titleHeight), new Size(total.Width, total.Height - titleHeight)));

                // Draw a boarder
                Pen pen = new Pen(brush);
                pen.Color = Color.Black;
                pen.Width = Convert.ToInt16(borderThicknessBox.Text);
                g.DrawLine(pen, 0, 0, total.Width, 0);
                g.DrawLine(pen, 0, 0, 0, total.Height);
                g.DrawLine(pen, total.Width, 0, total.Width, total.Height);
                g.DrawLine(pen, 0, total.Height, total.Width, total.Height);
                g.DrawLine(pen, 0, titleHeight, total.Width, titleHeight);

                // Writes the name
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                Font font = new Font(FontComboBox.Text, Convert.ToInt16(fontSizeBox.Text));
                brush = new SolidBrush(Color.Black);
                g.DrawString(nameBox.Text, font, brush, new RectangleF(0, 0, total.Width, titleHeight), stringFormat);
            }

            // Updates the pictureBox
            pictureBox1.Image = total;            
        }

        // Open button
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Finds the picture
                pic = Image.FromFile(ofd.FileName);

                // Intalizes the size
                if (pic.Width > pic.Height)
                {
                    TotalWidthTextBox.Text = "4";
                    TotalHeightTextBox.Text = "3";
                }
                else
                {
                    TotalWidthTextBox.Text = "3";
                    TotalHeightTextBox.Text = "4";
                }

                // Updates to the screen
                UpdateImage();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.Save("FileSystem\\Finished\\" + nameBox.Text + ".jpg");
        }

        // Update button
        private void button3_Click(object sender, EventArgs e)
        {
            UpdateImage();
        }
    }
}
