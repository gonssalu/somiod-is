namespace FormLightBulb
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
            if (disposing && (components != null)) {
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
            this.pbLightbulb = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize) (this.pbLightbulb)).BeginInit();
            this.SuspendLayout();
            // 
            // pbLightbulb
            // 
            this.pbLightbulb.Image = global::FormLightBulb.Properties.Resources.light_bulb_off;
            this.pbLightbulb.Location = new System.Drawing.Point(12, 12);
            this.pbLightbulb.Name = "pbLightbulb";
            this.pbLightbulb.Size = new System.Drawing.Size(222, 230);
            this.pbLightbulb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLightbulb.TabIndex = 1;
            this.pbLightbulb.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 254);
            this.Controls.Add(this.pbLightbulb);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing_1);
            this.Shown += new System.EventHandler(this.FormLightBulb_Shown);
            ((System.ComponentModel.ISupportInitialize) (this.pbLightbulb)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox pbLightbulb;
    }
}
