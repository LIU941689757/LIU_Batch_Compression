namespace LIU_Batch_Compression
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button3 = new Button();
            label1 = new Label();
            label2 = new Label();
            progressBar1 = new ProgressBar();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(23, 43);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(438, 23);
            textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(23, 116);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(438, 23);
            textBox2.TabIndex = 1;
            // 
            // button3
            // 
            button3.Location = new Point(386, 184);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 2;
            button3.Text = "button3";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 23);
            label1.Name = "label1";
            label1.Size = new Size(43, 17);
            label1.TabIndex = 3;
            label1.Text = "label1";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 96);
            label2.Name = "label2";
            label2.Size = new Size(43, 17);
            label2.TabIndex = 4;
            label2.Text = "label2";
            // 
            // progressBar1
            // 
            progressBar1.BackColor = SystemColors.AppWorkspace;
            progressBar1.Location = new Point(23, 155);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(438, 23);
            progressBar1.TabIndex = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(502, 261);
            Controls.Add(progressBar1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private Button button3;
        private Label label1;
        private Label label2;
        private ProgressBar progressBar1;
    }
}