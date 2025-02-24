using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        private List<Image> images;

        public Form2(List<Image> selectedImages)
        {
            InitializeComponent();
            images = selectedImages;
            flowLayoutPanel1.AutoScroll = true;
            DisplayImages();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            // Event if needed for future functionality
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Handle custom painting if necessary
        }

        private void DisplayImages()
        {
            flowLayoutPanel1.Controls.Clear();

            foreach (var img in images)
            {
                PictureBox pictureBox = new PictureBox
                {
                    Image = img,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 150,
                    Height = 150,
                    Margin = new Padding(5)
                };

                flowLayoutPanel1.Controls.Add(pictureBox);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF File|*.pdf",
                Title = "Save as PDF"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveImagesToPdf(saveFileDialog.FileName);
                MessageBox.Show("PDF saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SaveImagesToPdf(string filePath)
        {
            PdfDocument document = new PdfDocument();

            foreach (var img in images)
            {
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Resize page to match image dimensions
                page.Width = img.Width;
                page.Height = img.Height;

                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    XImage xImage = XImage.FromStream(ms);
                    gfx.DrawImage(xImage, 0, 0, img.Width, img.Height);
                }
            }

            document.Save(filePath);
            document.Close();
        }
    }
}
