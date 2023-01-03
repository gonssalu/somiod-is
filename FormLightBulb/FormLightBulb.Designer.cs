namespace FormLightBulb
{
    partial class FormLightBulb
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
            this.pbLightbulb.Location = new System.Drawing.Point(16, 15);
            this.pbLightbulb.Margin = new System.Windows.Forms.Padding(4);
            this.pbLightbulb.Name = "pbLightbulb";
            this.pbLightbulb.Size = new System.Drawing.Size(296, 283);
            this.pbLightbulb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLightbulb.TabIndex = 1;
            this.pbLightbulb.TabStop = false;
            // 
            // FormLightBulb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 313);
            this.Controls.Add(this.pbLightbulb);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormLightBulb";
            this.Text = "Light Bulb";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLightBulb_FormClosing);
            this.Shown += new System.EventHandler(this.FormLightBulb_Shown);
            ((System.ComponentModel.ISupportInitialize) (this.pbLightbulb)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox pbLightbulb;
    }
}
