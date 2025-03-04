namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            Uploadbtn = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            ConfirmBtn = new Button();
            button3 = new Button();
            label1 = new Label();
            comboBox1 = new ComboBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(Uploadbtn);
            groupBox1.Controls.Add(flowLayoutPanel1);
            groupBox1.Location = new Point(176, 167);
            groupBox1.Margin = new Padding(3, 2, 3, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 2, 3, 2);
            groupBox1.Size = new Size(374, 283);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Upload image";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // Uploadbtn
            // 
            Uploadbtn.Location = new Point(291, 256);
            Uploadbtn.Margin = new Padding(3, 2, 3, 2);
            Uploadbtn.Name = "Uploadbtn";
            Uploadbtn.Size = new Size(82, 22);
            Uploadbtn.TabIndex = 4;
            Uploadbtn.Text = "Upload";
            Uploadbtn.UseVisualStyleBackColor = true;
            Uploadbtn.Click += Uploadbtn_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Location = new Point(5, 20);
            flowLayoutPanel1.Margin = new Padding(3, 2, 3, 2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(363, 232);
            flowLayoutPanel1.TabIndex = 4;
            flowLayoutPanel1.Paint += flowLayoutPanel1_Paint;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.Moccasin;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(176, 68);
            textBox1.Margin = new Padding(3, 2, 3, 2);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(514, 64);
            textBox1.TabIndex = 2;
            textBox1.Text = "Document Scanner";
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.Moccasin;
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Location = new Point(181, 147);
            textBox2.Margin = new Padding(3, 2, 3, 2);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(374, 16);
            textBox2.TabIndex = 3;
            textBox2.Text = "Upload your document images to be scan and conver to a PDF.";
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // ConfirmBtn
            // 
            ConfirmBtn.Location = new Point(468, 449);
            ConfirmBtn.Margin = new Padding(3, 2, 3, 2);
            ConfirmBtn.Name = "ConfirmBtn";
            ConfirmBtn.Size = new Size(82, 22);
            ConfirmBtn.TabIndex = 6;
            ConfirmBtn.Text = "Comfirm";
            ConfirmBtn.UseVisualStyleBackColor = true;
            ConfirmBtn.Click += Confirmbtn_Click;
            // 
            // button3
            // 
            button3.Location = new Point(594, 287);
            button3.Margin = new Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new Size(105, 22);
            button3.TabIndex = 7;
            button3.Text = "Camera";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(594, 270);
            label1.Name = "label1";
            label1.Size = new Size(170, 15);
            label1.TabIndex = 8;
            label1.Text = "Take picture of your document";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Neither", "Convert to black and white", "Color Enhancement" });
            comboBox1.Location = new Point(594, 235);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(170, 23);
            comboBox1.TabIndex = 0;
            comboBox1.SelectedIndexChanged += comboboxChange;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Moccasin;
            ClientSize = new Size(915, 551);
            Controls.Add(comboBox1);
            Controls.Add(label1);
            Controls.Add(button3);
            Controls.Add(ConfirmBtn);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(groupBox1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Doc Scanner";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private GroupBox groupBox1;
        private TextBox textBox1;
        private TextBox textBox2;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button ConfirmBtn;
        private Button button3;
        private Label label1;
        private Button Uploadbtn;
        private FlowLayoutPanel flowLayoutPanel2;
        private ComboBox comboBox1;
    }
}
