using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DoorDecCreator
{
    public partial class MultipleDecForm : Form
    {
        const int resolution = 96;
        Excel roster;
        List<Tuple<string, string>> fullNames = new List<Tuple<string, string>>();
        List<string> names = new List<string>();
        OpenFileDialog ofd = new OpenFileDialog();

        public MultipleDecForm()
        {
            InitializeComponent();
        }

        private void UpdateTemplates()
        {
            int pWidth = (int)(Convert.ToDouble(PorTotalWidthTextBox.Text));
            int pHeight = (int)(Convert.ToDouble(PorTotalHeightTextBox.Text));
            int lWidth = (int)(Convert.ToDouble(LanTotalWidthTextBox.Text));
            int lHeight = (int)(Convert.ToDouble(LanTotalHeightTextBox.Text));

            // Sets up the sample picture
            int sampleSize = 512;
            Image sampleImage = new Bitmap(sampleSize, sampleSize);

            // Draws a gray box
            using (Graphics g = Graphics.FromImage(sampleImage))
            {
                Brush brush = new SolidBrush(Color.Gray);
                g.FillRectangle(brush, new Rectangle(0, 0, sampleSize, sampleSize));
            }

            // Draws the image
            PorPictureBox.Image = DrawImage(pHeight, pWidth, "Name", sampleImage);
            LanPictureBox.Image = DrawImage(lHeight, lWidth, "Name", sampleImage);
        }
        private void UpdateNames()
        {
            try
            {
                // Creates the new excel sheet
                roster = new Excel(ofd.FileName, 1);

                // Update the lsit box with the residents name
                // Read the excel sheet
                // Finds the col and rows
                int firstCol = Convert.ToInt16(Convert.ToChar(FirstColTextBox.Text) - 'A');
                int lastCol = Convert.ToInt16(Convert.ToChar(LastColTextBox.Text) - 'A');
                firstCol++;
                lastCol++;
                int upper = Convert.ToInt16(UprTextBox.Text);
                int lower = Convert.ToInt16(LwrRngTextBox.Text);

                // reads them
                names.Clear();
                fullNames.Clear();
                for (int i = upper; i <= lower; ++i)
                {
                    Tuple<string, string> currName = new Tuple<string, string>(roster.ReadCell(i, firstCol), roster.ReadCell(i, lastCol));
                    fullNames.Add(currName);
                }

                // Checks if there are any duplicates
                // First we need to sort the names in order
                fullNames.Sort();

                // Check for duplicates and change the names accordingly
                for (int i = 0; i < fullNames.Count() - 1; ++i)
                {
                    if (String.Equals(fullNames[i].Item1, fullNames[i + 1].Item1))
                    {
                        // Changes the first one, add it to the list
                        names.Add(fullNames[i].Item1 + " " + fullNames[i].Item2[0] + ".");

                        // Changes and following one
                        do
                        {
                            i++;
                            names.Add(fullNames[i].Item1 + " " + fullNames[i].Item2[0] + ".");

                        } while (String.Equals(fullNames[i].Item1, fullNames[i + 1].Item1));
                    }
                    else
                    {
                        // It is an original name, at it to the list
                        names.Add(fullNames[i].Item1);
                    }
                }

                // Adds the last element in the list
                int lastIndex = fullNames.Count() - 1;
                if (String.Equals(fullNames[lastIndex - 1].Item1, fullNames[lastIndex].Item1))
                {
                    names.Add(fullNames[lastIndex].Item1 + " " + fullNames[lastIndex].Item2[0] + ".");
                }
                else
                    names.Add(fullNames[lastIndex].Item1);

                // Adds it all to the text box
                NameListBox.Items.Clear();
                foreach (string name in names)
                {
                    NameListBox.Items.Add(name);
                }

                // Close it
                roster.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString() + "\n\nHave you tried selecting an excel file?");
            }
        }

        // Drawing function
        private Image DrawImage(int picHeight, int picWidth, string name, Image picture)
        {
            // Pic height and pic width are measured in inches

            int titleHeight = (int)(Convert.ToDouble(titleBox.Text) * resolution);
            int height = (int)(picHeight * resolution);
            int width = (int)(picWidth * resolution);
            Image total = new Bitmap(width, height + titleHeight);

            // Draws
            using (Graphics g = Graphics.FromImage(total))
            {
                // Draws the background
                Brush brush = new SolidBrush(Color.White);
                g.FillRectangle(brush, new Rectangle(0, 0, total.Width, total.Height));

                // Draws the image on the bottom
                g.DrawImage(picture, new Rectangle(new Point(0, titleHeight), new Size(total.Width, total.Height - titleHeight)));

                // Draws a boarder
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
                g.DrawString(name, font, brush, new RectangleF(0, 0, total.Width, titleHeight), stringFormat);
            }

            return total;
        }


        private void MultipleDecForm_Load(object sender, EventArgs e)
        {
            // Open and read files
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            HelpForm f2 = new HelpForm();
            f2.ShowDialog();
        }

        private void SelectSpreadSheetButton_Click(object sender, EventArgs e)
        {          
            // Opens the file
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Updates
                UpdateNames();               
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            UpdateNames();
        }

        private void UpdateTemplatesButton_Click(object sender, EventArgs e)
        {
            UpdateTemplates();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Reads the images folder
            string[] paths = Directory.GetFiles(@"FileSystem\Images");

            // Checks if everything is ready to go
            if (names.Count() == 0)
            {
                MessageBox.Show("Error: You need to select roster for names!");
            }
            else if (paths.Length == 0 || paths.Length < names.Count())
            {
                MessageBox.Show("Error: You need (more) images in the FileSytem\\Images folder!" +
                                "\n\n Number of Images: " + paths.Length.ToString() + 
                                "\n\n Number of Names: " + names.Count().ToString());

            }
            else
            {
                // Creates all of the files
                try
                {
                    // Creats a door dec for each residnet
                    int width;
                    int height;
                    for (int i = 0; i < names.Count(); ++i)
                    {
                        // Getes a picture
                        Image pic = Image.FromFile(paths[i]);

                        // Intalizes the size
                        // Landscape
                        if (pic.Width > pic.Height)
                        {
                            width = Convert.ToInt16(LanTotalWidthTextBox.Text);
                            height = Convert.ToInt16(LanTotalHeightTextBox.Text);
                        }
                        // Portrait
                        else
                        {
                            width = Convert.ToInt16(PorTotalWidthTextBox.Text);
                            height = Convert.ToInt16(PorTotalHeightTextBox.Text);
                        }

                        // Draws an image
                        Image currDec = DrawImage(height, width, names[i], pic);

                        // Saves
                        currDec.Save("FileSystem\\Finished\\" + names[i] + ".jpg");
                    }

                    MessageBox.Show("Created " + names.Count().ToString() + " decs!");
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.ToString());
                }
            }
        }
    }
}
