using System.Diagnostics;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private string[] imagePaths;
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
                imagePaths = openFileDialog.FileNames;
                flowLayoutPanel1.Controls.Clear(); // Clear previous images

                foreach (string file in imagePaths)
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

        private void btnPythonRun(object sender, EventArgs e)
        {
            if (imagePaths == null || imagePaths.Length == 0)
            {
                MessageBox.Show("Please select images first.");
                return;
            }

            string pythonExe = "python"; // Ensure Python is installed and in system PATH
            string scriptPath = System.IO.Path.Combine(Application.StartupPath, @"..\..\..\process.py");
            string arguments = $"\"{scriptPath}\" \"" + string.Join("\" \"", imagePaths) + "\"";

            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            try
            {
                using (Process process = Process.Start(start))
                {
                    if (process == null)
                    {
                        MessageBox.Show("Failed to start Python process.");
                        return;
                    }

                    string output = process.StandardOutput.ReadToEnd().Trim();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(error))
                    {
                        MessageBox.Show($"Python Error: {error}");
                    }
                    else
                    {
                        // Split output for multiple images
                        string[] processedImagePaths = output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                        flowLayoutPanel2.Controls.Clear(); // Clear previous processed images

                        foreach (string processedPath in processedImagePaths)
                        {
                            if (System.IO.File.Exists(processedPath)) // Ensure the file exists
                            {
                                PictureBox pictureBox = new PictureBox
                                {
                                    Image = new Bitmap(processedPath),
                                    SizeMode = PictureBoxSizeMode.Zoom, // Fit image properly
                                    Width = 100,  // Set thumbnail width
                                    Height = 100, // Set thumbnail height
                                    Margin = new Padding(5) // Add spacing between images
                                };

                                flowLayoutPanel2.Controls.Add(pictureBox);
                            }
                            else
                            {
                                MessageBox.Show($"Processed image not found: {processedPath}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing Python script: {ex.Message}");
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            List<Image> selectedImages = new List<Image>(); // Create a list to store selected images

            // Ensure images are loaded (if you already have them stored globally, use that variable)
            if (imagePaths != null)
            {
                foreach (string file in imagePaths)
                {
                    selectedImages.Add(new Bitmap(file));
                }
            }

            // Pass `selectedImages` when creating Form2
            Form2 form2 = new Form2(selectedImages);
            form2.Show(); // Show Form2
        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
