using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private List<Image> selectedImages = new List<Image>();
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
                flowLayoutPanel1.Controls.Clear(); // Clear previous images
                selectedImages.Clear(); // Clear previously selected images

                foreach (string file in openFileDialog.FileNames)
                {
                    Image img = new Bitmap(file);
                    selectedImages.Add(img); // Store the image

                    PictureBox pictureBox = new PictureBox
                    {
                        Image = img,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Width = 100,
                        Height = 100,
                        Margin = new Padding(5)
                    };

                    flowLayoutPanel1.Controls.Add(pictureBox);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (selectedImages.Count > 0)
            {
                Form2 form2 = new Form2(selectedImages);
                form2.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please upload images first before confirming.", "No Images Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            livecam_feed livecamForm = new livecam_feed();
            livecamForm.Show(); 

        }

    }
}
