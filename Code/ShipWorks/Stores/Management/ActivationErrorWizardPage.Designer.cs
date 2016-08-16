namespace ShipWorks.Stores.Management
{
    partial class ActivationErrorWizardPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.errorMessage = new System.Windows.Forms.Label();
            this.elementHost = new System.Windows.Forms.Integration.ElementHost();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.error32;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(41, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "A problem occurred with your activation.";
            // 
            // errorMessage
            // 
            this.errorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorMessage.Location = new System.Drawing.Point(41, 38);
            this.errorMessage.Name = "errorMessage";
            this.errorMessage.Size = new System.Drawing.Size(395, 18);
            this.errorMessage.TabIndex = 2;
            this.errorMessage.Text = "An error has occurred";
            // 
            // elementHost
            // 
            this.elementHost.BackColor = System.Drawing.Color.Transparent;
            this.elementHost.Location = new System.Drawing.Point(20, 129);
            this.elementHost.Name = "elementHost";
            this.elementHost.Size = new System.Drawing.Size(400, 250);
            this.elementHost.TabIndex = 3;
            this.elementHost.Text = "elementHost";
            this.elementHost.Child = null;
            this.elementHost.Visible = false;
            // 
            // ActivationErrorWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.elementHost);
            this.Controls.Add(this.errorMessage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ActivationErrorWizardPage";
            this.Size = new System.Drawing.Size(451, 410);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label errorMessage;
        private System.Windows.Forms.Integration.ElementHost elementHost;
    }
}
