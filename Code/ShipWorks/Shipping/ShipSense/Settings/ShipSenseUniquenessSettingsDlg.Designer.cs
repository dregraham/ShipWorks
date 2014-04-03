namespace ShipWorks.Shipping.ShipSense.Settings
{
    partial class ShipSenseUniquenessSettingsDlg
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
            this.shipSenseHashConfigurationControl1 = new ShipWorks.Shipping.ShipSense.Settings.ShipSenseHashConfigurationControl();
            this.SuspendLayout();
            // 
            // shipSenseHashConfigurationControl1
            // 
            this.shipSenseHashConfigurationControl1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.shipSenseHashConfigurationControl1.Location = new System.Drawing.Point(13, 13);
            this.shipSenseHashConfigurationControl1.Name = "shipSenseHashConfigurationControl1";
            this.shipSenseHashConfigurationControl1.Size = new System.Drawing.Size(531, 391);
            this.shipSenseHashConfigurationControl1.TabIndex = 0;
            // 
            // ShipSenseUniquenessSettingsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 556);
            this.Controls.Add(this.shipSenseHashConfigurationControl1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShipSenseUniquenessSettingsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ShipSense Uniqueness Settings";
            this.ResumeLayout(false);

        }

        #endregion

        private ShipSenseHashConfigurationControl shipSenseHashConfigurationControl1;
    }
}