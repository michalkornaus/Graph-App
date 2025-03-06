namespace GraphApp
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
            panel1 = new Panel();
            label1 = new Label();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton3 = new RadioButton();
            label2 = new Label();
            button1 = new Button();
            label3 = new Label();
            textBox1 = new TextBox();
            label4 = new Label();
            button2 = new Button();
            label5 = new Label();
            label6 = new Label();
            button3 = new Button();
            button4 = new Button();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Control;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Cursor = Cursors.Hand;
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(1072, 625);
            panel1.TabIndex = 1;
            panel1.Paint += panel1_Paint;
            panel1.MouseClick += panel1_MouseClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1122, 12);
            label1.Name = "label1";
            label1.Size = new Size(149, 15);
            label1.TabIndex = 2;
            label1.Text = "Opcje do tworzenia grafów";
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(1122, 117);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(159, 19);
            radioButton1.TabIndex = 3;
            radioButton1.Text = "Dodawanie wierzchołków";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Font = new Font("Segoe UI", 9F);
            radioButton2.Location = new Point(1122, 168);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(145, 19);
            radioButton2.TabIndex = 4;
            radioButton2.Text = "Łączenie wierzchołków";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Checked = true;
            radioButton3.Location = new Point(1122, 64);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(159, 19);
            radioButton3.TabIndex = 5;
            radioButton3.TabStop = true;
            radioButton3.Text = "Wybieranie wierzchołków";
            radioButton3.UseVisualStyleBackColor = true;
            radioButton3.CheckedChanged += radioButton3_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(1118, 247);
            label2.Name = "label2";
            label2.Size = new Size(163, 15);
            label2.TabIndex = 6;
            label2.Text = "Opcje do sprawdzania grafów";
            // 
            // button1
            // 
            button1.Location = new Point(1118, 265);
            button1.Name = "button1";
            button1.Size = new Size(145, 23);
            button1.TabIndex = 7;
            button1.Text = "Czy graf jest cykliczny?";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label3
            // 
            label3.Location = new Point(1112, 340);
            label3.Name = "label3";
            label3.Size = new Size(159, 67);
            label3.TabIndex = 8;
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(1118, 537);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(141, 23);
            textBox1.TabIndex = 9;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(1118, 416);
            label4.Name = "label4";
            label4.Size = new Size(128, 15);
            label4.TabIndex = 10;
            label4.Text = "Wygeneruj kod Prufera";
            // 
            // button2
            // 
            button2.Location = new Point(1118, 434);
            button2.Name = "button2";
            button2.Size = new Size(145, 23);
            button2.TabIndex = 11;
            button2.Text = "Generuj!";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(1122, 519);
            label5.Name = "label5";
            label5.Size = new Size(126, 15);
            label5.TabIndex = 12;
            label5.Text = "Wygeneruj graf z kodu";
            // 
            // label6
            // 
            label6.Location = new Point(1112, 471);
            label6.Name = "label6";
            label6.Size = new Size(190, 39);
            label6.TabIndex = 13;
            // 
            // button3
            // 
            button3.Location = new Point(1118, 566);
            button3.Name = "button3";
            button3.Size = new Size(145, 23);
            button3.TabIndex = 14;
            button3.Text = "Generuj graf";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(1118, 304);
            button4.Name = "button4";
            button4.Size = new Size(145, 23);
            button4.TabIndex = 15;
            button4.Text = "Czy graf jest regularny?";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1320, 649);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(radioButton3);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            Controls.Add(label1);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Grafy";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panel1;
        private Label label1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private Label label2;
        private Button button1;
        private Label label3;
        private TextBox textBox1;
        private Label label4;
        private Button button2;
        private Label label5;
        private Label label6;
        private Button button3;
        private Button button4;
    }
}
