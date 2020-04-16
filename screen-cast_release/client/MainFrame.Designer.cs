namespace client
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
            this.pciture_screen = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pciture_screen)).BeginInit();
            this.SuspendLayout();
            // 
            // pciture_screen
            // 
            this.pciture_screen.BackColor = System.Drawing.SystemColors.ControlText;
            this.pciture_screen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pciture_screen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pciture_screen.Location = new System.Drawing.Point(0, 0);
            this.pciture_screen.Name = "pciture_screen";
            this.pciture_screen.Size = new System.Drawing.Size(537, 294);
            this.pciture_screen.TabIndex = 0;
            this.pciture_screen.TabStop = false;
            // 
            // MainFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 294);
            this.Controls.Add(this.pciture_screen);
            this.Name = "MainFrame";
            this.Text = "screen_cast client";
            ((System.ComponentModel.ISupportInitialize)(this.pciture_screen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pciture_screen;
    }
}

