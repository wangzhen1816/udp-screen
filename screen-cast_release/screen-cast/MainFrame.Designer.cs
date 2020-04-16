namespace screen_cast
{
    partial class MainFrame
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.picture_screen = new System.Windows.Forms.PictureBox();
            this.btn_start = new System.Windows.Forms.Button();
            this.text_fps = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.text_quality = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture_screen)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.picture_screen);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(622, 461);
            this.panel1.TabIndex = 0;
            // 
            // picture_screen
            // 
            this.picture_screen.BackColor = System.Drawing.SystemColors.ControlText;
            this.picture_screen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picture_screen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picture_screen.Location = new System.Drawing.Point(0, 0);
            this.picture_screen.Name = "picture_screen";
            this.picture_screen.Size = new System.Drawing.Size(622, 461);
            this.picture_screen.TabIndex = 0;
            this.picture_screen.TabStop = false;
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(630, 119);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(75, 23);
            this.btn_start.TabIndex = 1;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // text_fps
            // 
            this.text_fps.Location = new System.Drawing.Point(630, 36);
            this.text_fps.Name = "text_fps";
            this.text_fps.Size = new System.Drawing.Size(72, 21);
            this.text_fps.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(633, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "fps:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(633, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "quality:";
            // 
            // text_quality
            // 
            this.text_quality.Location = new System.Drawing.Point(630, 78);
            this.text_quality.Name = "text_quality";
            this.text_quality.Size = new System.Drawing.Size(72, 21);
            this.text_quality.TabIndex = 4;
            // 
            // MainFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 465);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.text_quality);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.text_fps);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(480, 320);
            this.Name = "MainFrame";
            this.Text = "screen-cast";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picture_screen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox picture_screen;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.TextBox text_fps;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox text_quality;
    }
}

