namespace Try_To_ESCAPE__GAME
{
    partial class Form3
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel98 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panel96 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.button15 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Green = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Green2 = new System.Windows.Forms.Panel();
            this.Yellow = new System.Windows.Forms.Panel();
            this.Orange = new System.Windows.Forms.Panel();
            this.Red = new System.Windows.Forms.Panel();
            this.jauge = new System.Windows.Forms.Panel();
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel98.SuspendLayout();
            this.panel96.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 10;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::Try_To_ESCAPE__GAME.Properties.Resources.piste_de_course;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.panel98);
            this.panel1.Controls.Add(this.panel96);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.Green);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.Green2);
            this.panel1.Controls.Add(this.Yellow);
            this.panel1.Controls.Add(this.Orange);
            this.panel1.Controls.Add(this.Red);
            this.panel1.Controls.Add(this.jauge);
            this.panel1.Location = new System.Drawing.Point(0, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(802, 453);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            // 
            // panel98
            // 
            this.panel98.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel98.Controls.Add(this.label3);
            this.panel98.Controls.Add(this.label12);
            this.panel98.Controls.Add(this.label11);
            this.panel98.Controls.Add(this.label10);
            this.panel98.Location = new System.Drawing.Point(0, 0);
            this.panel98.Name = "panel98";
            this.panel98.Size = new System.Drawing.Size(807, 502);
            this.panel98.TabIndex = 140;
            this.panel98.Visible = false;
            this.panel98.Paint += new System.Windows.Forms.PaintEventHandler(this.panel98_Paint);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(379, 429);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Retour";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(373, 460);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(39, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Retour";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(21, 116);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(750, 176);
            this.label11.TabIndex = 1;
            this.label11.Text = "Il faut s\'échapper de ce monde en résoudant toutes les énigmes qui viendrons à se" +
    " présenter\r\n\r\nSe déplacer : \r\nUtilisez WASD\r\n\r\nLes intéractions se feront toujou" +
    "rs avec E\r\n\r\n\r\n";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(341, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(124, 39);
            this.label10.TabIndex = 0;
            this.label10.Text = "Règles";
            // 
            // panel96
            // 
            this.panel96.BackColor = System.Drawing.Color.Black;
            this.panel96.Controls.Add(this.label13);
            this.panel96.Controls.Add(this.button15);
            this.panel96.Controls.Add(this.button14);
            this.panel96.Controls.Add(this.button13);
            this.panel96.Controls.Add(this.button12);
            this.panel96.Controls.Add(this.label6);
            this.panel96.Location = new System.Drawing.Point(0, 0);
            this.panel96.Name = "panel96";
            this.panel96.Size = new System.Drawing.Size(807, 453);
            this.panel96.TabIndex = 138;
            this.panel96.Visible = false;
            this.panel96.Click += new System.EventHandler(this.panel96_Click);
            this.panel96.Paint += new System.Windows.Forms.PaintEventHandler(this.panel96_Paint);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(757, 7);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 15);
            this.label13.TabIndex = 7;
            this.label13.Text = "Crédits";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(363, 399);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(75, 40);
            this.button15.TabIndex = 6;
            this.button15.Text = "Règles";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button14
            // 
            this.button14.ForeColor = System.Drawing.Color.Red;
            this.button14.Location = new System.Drawing.Point(709, 403);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(75, 36);
            this.button14.TabIndex = 5;
            this.button14.Text = "Réinitialiser";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(364, 170);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(75, 40);
            this.button13.TabIndex = 2;
            this.button13.Text = "Langue : Français";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(364, 109);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(75, 36);
            this.button12.TabIndex = 1;
            this.button12.Text = "Timer : \r\nDisebled";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(339, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(119, 46);
            this.label6.TabIndex = 0;
            this.label6.Text = "Menu";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(362, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 17;
            // 
            // Green
            // 
            this.Green.BackColor = System.Drawing.Color.LimeGreen;
            this.Green.Location = new System.Drawing.Point(660, 377);
            this.Green.Name = "Green";
            this.Green.Size = new System.Drawing.Size(62, 30);
            this.Green.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(365, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 31);
            this.label1.TabIndex = 10;
            this.label1.Text = "YOU";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::Try_To_ESCAPE__GAME.Properties.Resources.Yousport;
            this.pictureBox1.Location = new System.Drawing.Point(267, 182);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(276, 225);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Green2
            // 
            this.Green2.BackColor = System.Drawing.Color.YellowGreen;
            this.Green2.Location = new System.Drawing.Point(660, 332);
            this.Green2.Name = "Green2";
            this.Green2.Size = new System.Drawing.Size(62, 74);
            this.Green2.TabIndex = 13;
            this.Green2.Visible = false;
            // 
            // Yellow
            // 
            this.Yellow.BackColor = System.Drawing.Color.Yellow;
            this.Yellow.Location = new System.Drawing.Point(661, 274);
            this.Yellow.Name = "Yellow";
            this.Yellow.Size = new System.Drawing.Size(61, 64);
            this.Yellow.TabIndex = 14;
            this.Yellow.Visible = false;
            // 
            // Orange
            // 
            this.Orange.BackColor = System.Drawing.Color.Orange;
            this.Orange.Location = new System.Drawing.Point(661, 224);
            this.Orange.Name = "Orange";
            this.Orange.Size = new System.Drawing.Size(61, 52);
            this.Orange.TabIndex = 15;
            this.Orange.Visible = false;
            // 
            // Red
            // 
            this.Red.BackColor = System.Drawing.Color.Red;
            this.Red.Location = new System.Drawing.Point(661, 182);
            this.Red.Name = "Red";
            this.Red.Size = new System.Drawing.Size(61, 47);
            this.Red.TabIndex = 16;
            this.Red.Visible = false;
            // 
            // jauge
            // 
            this.jauge.BackColor = System.Drawing.Color.Black;
            this.jauge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.jauge.Location = new System.Drawing.Point(661, 182);
            this.jauge.Name = "jauge";
            this.jauge.Size = new System.Drawing.Size(61, 225);
            this.jauge.TabIndex = 11;
            // 
            // timer3
            // 
            this.timer3.Enabled = true;
            this.timer3.Interval = 10;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 450);
            this.Controls.Add(this.panel1);
            this.MaximumSize = new System.Drawing.Size(821, 489);
            this.MinimumSize = new System.Drawing.Size(821, 489);
            this.Name = "Form3";
            this.Text = "Form3";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form3_FormClosing);
            this.Load += new System.EventHandler(this.Form3_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form3_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel98.ResumeLayout(false);
            this.panel98.PerformLayout();
            this.panel96.ResumeLayout(false);
            this.panel96.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel Green;
        private System.Windows.Forms.Panel Green2;
        private System.Windows.Forms.Panel jauge;
        private System.Windows.Forms.Panel Yellow;
        private System.Windows.Forms.Panel Orange;
        private System.Windows.Forms.Panel Red;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Panel panel96;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Panel panel98;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label13;
    }
}