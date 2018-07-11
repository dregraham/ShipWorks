namespace SmokeTest
{
    partial class DisappearingWindow
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
            this.Install = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Content = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // Content
            //
            this.Content.Text = "Do you want to proceed with BullZip Printer installation?";
            this.Content.Location = new System.Drawing.Point(12, 12);
            this.Content.Size = new System.Drawing.Size(150, 50);
           	//this.Content.AutoSize = true;
            // 
            // Install
            // 
            this.Install.Location = new System.Drawing.Point(12, 44);
            this.Install.Name = "Install";
            this.Install.Size = new System.Drawing.Size(75, 23);
            this.Install.TabIndex = 0;
            this.Install.Text = "Install";
            this.Install.UseVisualStyleBackColor = true;
            this.Install.Click += new System.EventHandler(this.Install_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(93, 44);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // DisappearingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CenterToScreen();
            this.ClientSize = new System.Drawing.Size(180, 75);
            //this.AutoSize = true;
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Install);
            this.Controls.Add(this.Content);
            this.Name = "InstallBullZip";
            this.Text = "InstallBullZip";
            this.Load += new System.EventHandler(this.DisappearingWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Install;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label Content;
    }
}