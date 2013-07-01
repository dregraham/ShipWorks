namespace ShipWorks.ApplicationCore.WindowsServices
{
    partial class ServiceStartHelpDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceStartHelpDlg));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.screenshot = new System.Windows.Forms.PictureBox();
            this.helpInstructions = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.screenshot)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.screenshot);
            this.groupBox1.Controls.Add(this.helpInstructions);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(575, 547);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Starting a ShipWorks service";
            // 
            // screenshot
            // 
            this.screenshot.Image = global::ShipWorks.Properties.Resources.start_shipworks_service;
            this.screenshot.Location = new System.Drawing.Point(9, 147);
            this.screenshot.Name = "screenshot";
            this.screenshot.Size = new System.Drawing.Size(560, 394);
            this.screenshot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.screenshot.TabIndex = 4;
            this.screenshot.TabStop = false;
            // 
            // helpInstructions
            // 
            this.helpInstructions.Location = new System.Drawing.Point(6, 27);
            this.helpInstructions.Name = "helpInstructions";
            this.helpInstructions.Size = new System.Drawing.Size(563, 102);
            this.helpInstructions.TabIndex = 2;
            this.helpInstructions.Text = resources.GetString("helpInstructions.Text");
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(512, 565);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.OnClose);
            // 
            // ServiceStartHelpDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 609);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServiceStartHelpDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Help - ShipWorks service not running";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.screenshot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox screenshot;
        private System.Windows.Forms.Label helpInstructions;
        private System.Windows.Forms.Button closeButton;
    }
}