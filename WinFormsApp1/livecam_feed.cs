using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace WinFormsApp1
{
    public partial class livecam_feed : Form
    {
        private VideoCapture? capture;
        private Thread? cameraThread;
        private bool isCameraRunning = false;
        private bool cameraStopped = false;  // Track if the camera is stopped
        private Mat frame = new Mat();
        private Bitmap? image = null;
        private CancellationTokenSource? cancellationTokenSource;
        private bool cancellationTokenSourceDisposed = false;


        public livecam_feed()
        {
            InitializeComponent();
            StartCamera();
            button2.Enabled = false;  // Disable button2 initially
        }

        private void StartCamera()
        {
            capture = new VideoCapture(0);
            if (capture == null || !capture.IsOpened())
            {
                MessageBox.Show("Failed to open camera.");
                return;
            }

            // Create a CancellationTokenSource to allow stopping the camera thread
            cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            isCameraRunning = true;
            cameraThread = new Thread(() => CaptureCamera(cancellationToken));
            cameraThread.IsBackground = true;
            cameraThread.Start();
        }

        private void CaptureCamera(CancellationToken cancellationToken)
        {
            while (isCameraRunning && !cancellationToken.IsCancellationRequested)
            {
                capture?.Read(frame);
                if (!frame.Empty())
                {
                    image = BitmapConverter.ToBitmap(frame);
                    pictureBox1.Invoke(new MethodInvoker(delegate { pictureBox1.Image = image; }));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (capture != null && capture.IsOpened())
            {
                Mat capturedFrame = new Mat();
                capture.Read(capturedFrame);
                if (!capturedFrame.Empty())
                {
                    Bitmap capturedImage = BitmapConverter.ToBitmap(capturedFrame);
                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "JPEG Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp";
                        saveFileDialog.Title = "Save Captured Image";
                        saveFileDialog.FileName = $"captured_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            capturedImage.Save(saveFileDialog.FileName);
                            MessageBox.Show($"Image saved: {saveFileDialog.FileName}");
                        }
                    }
                    capturedFrame.Dispose();
                }
                else
                {
                    MessageBox.Show("Failed to capture image!");
                }
            }
            else
            {
                MessageBox.Show("No active camera to capture an image!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!cameraStopped)
            {
                MessageBox.Show("Please stop the camera using button3 before saving an image.");
                return;
            }

            // Stop the camera feed and release resources
            StopCamera();

            // Close the form
            this.Close();
        }


        private void StopCamera()
        {
            // Set the flag to stop the camera thread
            isCameraRunning = false;

            // Check if cancellationTokenSource is already disposed or not
            if (cancellationTokenSourceDisposed)
            {
                // If it's already disposed, there's no need to cancel again
                return;
            }

            // Cancel the camera capture thread gracefully
            cancellationTokenSource?.Cancel();

            // Do NOT use Join here. This could block the UI thread. Instead, use safe cleanup.
            Task.Run(() =>
            {
                // Wait for the camera thread to complete asynchronously
                if (cameraThread != null && cameraThread.IsAlive)
                {
                    cameraThread.Join(); // Allow the camera thread to finish, without blocking UI thread
                }

                // After thread completion, release the capture object and dispose of resources
                capture?.Release();
                capture = null;

                // Dispose the CancellationTokenSource
                cancellationTokenSource?.Dispose();
                cancellationTokenSourceDisposed = true;  // Mark as disposed to prevent further calls

                // Mark the camera as stopped
                cameraStopped = true;

                // Enable button2 now that the camera is stopped
                Invoke(new Action(() => button2.Enabled = true));  // Ensure UI updates on the main thread
            });
        }



        private void button3_Click(object sender, EventArgs e)
        {
            // Stop the camera feed
            StopCamera();

            // Now button2 can be pressed after button3 has been used
            MessageBox.Show("Camera stopped. You can now goes back to upload.");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Stop the camera feed before closing the form
            StopCamera();

            // Call the base form closing event
            base.OnFormClosing(e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // This method is empty, and if not needed, you can remove it.
        }
    }
}
