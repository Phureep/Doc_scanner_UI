using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog.Multiselect = true; // Allow multiple file selection

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                flowLayoutPanel1.Controls.Clear(); // Clear previous images

                foreach (string file in openFileDialog.FileNames)
                {
                    PictureBox pictureBox = new PictureBox
                    {
                        Image = new Bitmap(file),
                        SizeMode = PictureBoxSizeMode.Zoom, // Fit image properly
                        Width = 100,  // Set thumbnail width
                        Height = 100, // Set thumbnail height
                        Margin = new Padding(5) // Add spacing between images
                    };

                    flowLayoutPanel1.Controls.Add(pictureBox);
                }
            }

        }


        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                List<Image> selectedImages = new List<Image>();

                foreach (string file in openFileDialog.FileNames)
                {
                    selectedImages.Add(new Bitmap(file));
                }

                // Open Form2 and pass the selected images to it
                Form2 form2 = new Form2(selectedImages);
                form2.ShowDialog();
            }
        }
    }
}
