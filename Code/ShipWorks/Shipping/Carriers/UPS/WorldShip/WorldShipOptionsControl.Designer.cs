namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    partial class WorldShipOptionsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorldShipOptionsControl));
            this.launchWorldShip = new System.Windows.Forms.CheckBox();
            this.labelWorldShipIntegration = new System.Windows.Forms.Label();
            this.labelWorldShipLaunch = new System.Windows.Forms.Label();
            this.labelWorldShipConnectionSetup = new System.Windows.Forms.Label();
            this.labelConnectionSetupHeader = new System.Windows.Forms.Label();
            this.integrateWorldShip = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // launchWorldShip
            // 
            this.launchWorldShip.AutoSize = true;
            this.launchWorldShip.Location = new System.Drawing.Point(26, 24);
            this.launchWorldShip.Name = "launchWorldShip";
            this.launchWorldShip.Size = new System.Drawing.Size(330, 17);
            this.launchWorldShip.TabIndex = 0;
            this.launchWorldShip.Text = "Launch WorldShip after processing if installed on this computer.";
            this.launchWorldShip.UseVisualStyleBackColor = true;
            // 
            // labelWorldShipIntegration
            // 
            this.labelWorldShipIntegration.AutoSize = true;
            this.labelWorldShipIntegration.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWorldShipIntegration.Location = new System.Drawing.Point(1, 2);
            this.labelWorldShipIntegration.Name = "labelWorldShipIntegration";
            this.labelWorldShipIntegration.Size = new System.Drawing.Size(132, 13);
            this.labelWorldShipIntegration.TabIndex = 4;
            this.labelWorldShipIntegration.Text = "WorldShip Integration";
            // 
            // labelWorldShipLaunch
            // 
            this.labelWorldShipLaunch.AutoSize = true;
            this.labelWorldShipLaunch.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelWorldShipLaunch.Location = new System.Drawing.Point(43, 39);
            this.labelWorldShipLaunch.MaximumSize = new System.Drawing.Size(380, 0);
            this.labelWorldShipLaunch.Name = "labelWorldShipLaunch";
            this.labelWorldShipLaunch.Size = new System.Drawing.Size(372, 52);
            this.labelWorldShipLaunch.TabIndex = 5;
            this.labelWorldShipLaunch.Text = resources.GetString("labelWorldShipLaunch.Text");
            // 
            // labelWorldShipConnectionSetup
            // 
            this.labelWorldShipConnectionSetup.AutoSize = true;
            this.labelWorldShipConnectionSetup.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelWorldShipConnectionSetup.Location = new System.Drawing.Point(43, 113);
            this.labelWorldShipConnectionSetup.MaximumSize = new System.Drawing.Size(380, 0);
            this.labelWorldShipConnectionSetup.Name = "labelWorldShipConnectionSetup";
            this.labelWorldShipConnectionSetup.Size = new System.Drawing.Size(374, 52);
            this.labelWorldShipConnectionSetup.TabIndex = 8;
            this.labelWorldShipConnectionSetup.Text = resources.GetString("labelWorldShipConnectionSetup.Text");
            // 
            // labelConnectionSetupHeader
            // 
            this.labelConnectionSetupHeader.AutoSize = true;
            this.labelConnectionSetupHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConnectionSetupHeader.Location = new System.Drawing.Point(23, 98);
            this.labelConnectionSetupHeader.Name = "labelConnectionSetupHeader";
            this.labelConnectionSetupHeader.Size = new System.Drawing.Size(106, 13);
            this.labelConnectionSetupHeader.TabIndex = 7;
            this.labelConnectionSetupHeader.Text = "Connection Setup";
            // 
            // integrateWorldShip
            // 
            this.integrateWorldShip.Location = new System.Drawing.Point(46, 170);
            this.integrateWorldShip.Name = "integrateWorldShip";
            this.integrateWorldShip.Size = new System.Drawing.Size(161, 23);
            this.integrateWorldShip.TabIndex = 6;
            this.integrateWorldShip.Text = "Create WorldShip Connection";
            this.integrateWorldShip.UseVisualStyleBackColor = true;
            this.integrateWorldShip.Click += new System.EventHandler(this.OnIntegrateWorldShip);
            // 
            // WorldShipOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelWorldShipConnectionSetup);
            this.Controls.Add(this.labelConnectionSetupHeader);
            this.Controls.Add(this.integrateWorldShip);
            this.Controls.Add(this.labelWorldShipLaunch);
            this.Controls.Add(this.labelWorldShipIntegration);
            this.Controls.Add(this.launchWorldShip);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "WorldShipOptionsControl";
            this.Size = new System.Drawing.Size(456, 193);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox launchWorldShip;
        private System.Windows.Forms.Label labelWorldShipIntegration;
        private System.Windows.Forms.Label labelWorldShipLaunch;
        private System.Windows.Forms.Label labelWorldShipConnectionSetup;
        private System.Windows.Forms.Label labelConnectionSetupHeader;
        private System.Windows.Forms.Button integrateWorldShip;

    }
}
