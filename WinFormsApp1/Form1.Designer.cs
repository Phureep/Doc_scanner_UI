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
            checkBox1 = new CheckBox();
            checkBox2 = new CheckBox();
            ConfirmBtn = new Button();
            button3 = new Button();
            label1 = new Label();
            flowLayoutPanel2 = new FlowLayoutPanel();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(Uploadbtn);
            groupBox1.Controls.Add(flowLayoutPanel1);
            groupBox1.Location = new Point(301, 147);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(427, 377);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Upload image";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // Uploadbtn
            // 
            Uploadbtn.Location = new Point(333, 341);
            Uploadbtn.Name = "Uploadbtn";
            Uploadbtn.Size = new Size(94, 29);
            Uploadbtn.TabIndex = 4;
            Uploadbtn.Text = "Upload";
            Uploadbtn.UseVisualStyleBackColor = true;
            Uploadbtn.Click += Uploadbtn_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Location = new Point(6, 27);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(415, 309);
            flowLayoutPanel1.TabIndex = 4;
            flowLayoutPanel1.Paint += flowLayoutPanel1_Paint;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.Moccasin;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(301, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(587, 80);
            textBox1.TabIndex = 2;
            textBox1.Text = "Document Scanner";
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.Moccasin;
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Location = new Point(301, 113);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(427, 20);
            textBox2.TabIndex = 3;
            textBox2.Text = "Upload your document images to be scan and conver to a PDF.";
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(768, 189);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(148, 24);
            checkBox1.TabIndex = 4;
            checkBox1.Text = "Remove Shadows";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(768, 220);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(159, 24);
            checkBox2.TabIndex = 5;
            checkBox2.Text = "Color Enchancment";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // ConfirmBtn
            // 
            ConfirmBtn.Location = new Point(634, 529);
            ConfirmBtn.Name = "ConfirmBtn";
            ConfirmBtn.Size = new Size(94, 29);
            ConfirmBtn.TabIndex = 6;
            ConfirmBtn.Text = "Comfirm";
            ConfirmBtn.UseVisualStyleBackColor = true;
            ConfirmBtn.Click += Confirmbtn_Click;
            // 
            // button3
            // 
            button3.Location = new Point(768, 339);
            button3.Name = "button3";
            button3.Size = new Size(120, 29);
            button3.TabIndex = 7;
            button3.Text = "Camera";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(768, 316);
            label1.Name = "label1";
            label1.Size = new Size(210, 20);
            label1.TabIndex = 8;
            label1.Text = "Take picture of your document";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Location = new Point(1035, 173);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(415, 309);
            flowLayoutPanel2.TabIndex = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Moccasin;
            ClientSize = new Size(1570, 824);
            Controls.Add(flowLayoutPanel2);
            Controls.Add(label1);
            Controls.Add(button3);
            Controls.Add(ConfirmBtn);
            Controls.Add(checkBox2);
            Controls.Add(checkBox1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(groupBox1);
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
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private Button ConfirmBtn;
        private Button button3;
        private Label label1;
        private Button Uploadbtn;
        private FlowLayoutPanel flowLayoutPanel2;
    }
}
