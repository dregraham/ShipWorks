namespace ShipWorks.ApplicationCore.Licensing.MessageBoxes
{
    partial class LicenseActiveElsewhereDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseActiveElsewhereDlg));
            this.linkInterapptiveAccount = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.close = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // linkInterapptiveAccount
            // 
            this.linkInterapptiveAccount.AutoSize = true;
            this.linkInterapptiveAccount.Location = new System.Drawing.Point(61, 67);
            this.linkInterapptiveAccount.Name = "linkInterapptiveAccount";
            this.linkInterapptiveAccount.Size = new System.Drawing.Size(198, 18);
            this.linkInterapptiveAccount.TabIndex = 9;
            this.linkInterapptiveAccount.TabStop = true;
            this.linkInterapptiveAccount.Text = "https://www.interapptive.com/account";
            this.linkInterapptiveAccount.UseCompatibleTextRendering = true;
            this.linkInterapptiveAccount.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnClickAccountLink);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(61, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(368, 36);
            this.label2.TabIndex = 8;
            this.label2.Text = "You can reset the license and make it available for use by logging in to your Int" +
                "erapptive account using the following link.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(61, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(368, 33);
            this.label1.TabIndex = 7;
            this.label1.Text = "The ShipWorks license you entered is already being used by another store.";
            // 
            // close
            // 
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(365, 88);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 5;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image) (resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(7, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // LicenseActiveElsewhereDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 123);
            this.Controls.Add(this.linkInterapptiveAccount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LicenseActiveElsewhereDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "License In Use";
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkInterapptiveAccount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button close;
    }
}