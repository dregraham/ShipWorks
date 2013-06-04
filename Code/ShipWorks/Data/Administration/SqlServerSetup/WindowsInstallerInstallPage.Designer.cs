namespace ShipWorks.Data.Administration.SqlServerSetup
{
    partial class WindowsInstallerInstallPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowsInstallerInstallPage));
            this.panelInstallWindowsInstaller = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.panelInstallingWindowsInstaller = new System.Windows.Forms.Panel();
            this.labelInstallingWindowsInstaller = new System.Windows.Forms.Label();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.panelInstallWindowsInstaller.SuspendLayout();
            this.panelInstallingWindowsInstaller.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox7)).BeginInit();
            this.SuspendLayout();
            // 
            // panelInstallWindowsInstaller
            // 
            this.panelInstallWindowsInstaller.Controls.Add(this.label20);
            this.panelInstallWindowsInstaller.Controls.Add(this.label22);
            this.panelInstallWindowsInstaller.Controls.Add(this.label23);
            this.panelInstallWindowsInstaller.Location = new System.Drawing.Point(15, 0);
            this.panelInstallWindowsInstaller.Name = "panelInstallWindowsInstaller";
            this.panelInstallWindowsInstaller.Size = new System.Drawing.Size(478, 102);
            this.panelInstallWindowsInstaller.TabIndex = 160;
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(9, 11);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(470, 52);
            this.label20.TabIndex = 2;
            this.label20.Text = "ShipWorks will now install Windows Installer 4.5. You will need to reboot your co" +
                "mputer after the installation is complete. \r\n\r\nClick Next to continue with the i" +
                "nstallation.";
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label22.Location = new System.Drawing.Point(9, 76);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(38, 22);
            this.label22.TabIndex = 152;
            this.label22.Text = "Note:";
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(47, 76);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(342, 18);
            this.label23.TabIndex = 153;
            this.label23.Text = "Please close all other running applications before proceeding.";
            // 
            // panelInstallingWindowsInstaller
            // 
            this.panelInstallingWindowsInstaller.Controls.Add(this.pictureBox7);
            this.panelInstallingWindowsInstaller.Controls.Add(this.labelInstallingWindowsInstaller);
            this.panelInstallingWindowsInstaller.Location = new System.Drawing.Point(15, 0);
            this.panelInstallingWindowsInstaller.Name = "panelInstallingWindowsInstaller";
            this.panelInstallingWindowsInstaller.Size = new System.Drawing.Size(478, 102);
            this.panelInstallingWindowsInstaller.TabIndex = 161;
            // 
            // labelInstallingWindowsInstaller
            // 
            this.labelInstallingWindowsInstaller.Location = new System.Drawing.Point(8, 12);
            this.labelInstallingWindowsInstaller.Name = "labelInstallingWindowsInstaller";
            this.labelInstallingWindowsInstaller.Size = new System.Drawing.Size(491, 33);
            this.labelInstallingWindowsInstaller.TabIndex = 158;
            this.labelInstallingWindowsInstaller.Text = "ShipWorks is now installing Windows Installer 4.5.  This may take a few minutes.";
            // 
            // pictureBox7
            // 
            this.pictureBox7.Image = ((System.Drawing.Image) (resources.GetObject("pictureBox7.Image")));
            this.pictureBox7.Location = new System.Drawing.Point(401, 11);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(16, 16);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox7.TabIndex = 159;
            this.pictureBox7.TabStop = false;
            // 
            // WindowsInstallerInstallPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelInstallWindowsInstaller);
            this.Controls.Add(this.panelInstallingWindowsInstaller);
            this.Description = "ShipWorks is ready to install Windows Installer 4.5.";
            this.Name = "WindowsInstallerInstallPage";
            this.NextRequiresElevation = true;
            this.Size = new System.Drawing.Size(529, 134);
            this.Title = "Install Windows Installer 4.5";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.Cancelling += new System.ComponentModel.CancelEventHandler(this.OnCancel);
            this.panelInstallWindowsInstaller.ResumeLayout(false);
            this.panelInstallingWindowsInstaller.ResumeLayout(false);
            this.panelInstallingWindowsInstaller.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox7)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelInstallWindowsInstaller;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Panel panelInstallingWindowsInstaller;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.Label labelInstallingWindowsInstaller;
    }
}
