namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    partial class StampsSettingsControl
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
            this.labelOriginInfo = new System.Windows.Forms.Label();
            this.originManagerControl = new ShipWorks.Shipping.Settings.Origin.ShippingOriginManagerControl();
            this.labelAccountType = new System.Windows.Forms.Label();
            this.accountControl = new ShipWorks.Shipping.Carriers.Postal.Stamps.StampsAccountManagerControl();
            this.optionsControl = new ShipWorks.Shipping.Carriers.Postal.Stamps.StampsOptionsControl();
            this.express1Options = new ShipWorks.Shipping.Carriers.Postal.Stamps.Express1.Express1StampsSingleSourceControl();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelOriginInfo
            // 
            this.labelOriginInfo.AutoSize = true;
            this.labelOriginInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOriginInfo.Location = new System.Drawing.Point(12, 138);
            this.labelOriginInfo.Name = "labelOriginInfo";
            this.labelOriginInfo.Size = new System.Drawing.Size(102, 13);
            this.labelOriginInfo.TabIndex = 2;
            this.labelOriginInfo.Text = "Origin Addresses";
            // 
            // originManagerControl
            // 
            this.originManagerControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originManagerControl.Location = new System.Drawing.Point(15, 157);
            this.originManagerControl.Name = "originManagerControl";
            this.originManagerControl.Size = new System.Drawing.Size(454, 150);
            this.originManagerControl.TabIndex = 3;
            // 
            // labelAccountType
            // 
            this.labelAccountType.AutoSize = true;
            this.labelAccountType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccountType.Location = new System.Drawing.Point(12, 5);
            this.labelAccountType.Name = "labelAccountType";
            this.labelAccountType.Size = new System.Drawing.Size(132, 13);
            this.labelAccountType.TabIndex = 0;
            this.labelAccountType.Text = "Stamps.com Accounts";
            // 
            // accountControl
            // 
            this.accountControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountControl.IsExpress1 = false;
            this.accountControl.Location = new System.Drawing.Point(12, 21);
            this.accountControl.Name = "accountControl";
            this.accountControl.Size = new System.Drawing.Size(459, 104);
            this.accountControl.TabIndex = 1;
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsControl.Location = new System.Drawing.Point(0, -1);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(435, 105);
            this.optionsControl.TabIndex = 4;
            // 
            // express1Options
            // 
            this.express1Options.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.express1Options.Location = new System.Drawing.Point(7, 115);
            this.express1Options.Name = "express1Options";
            this.express1Options.Size = new System.Drawing.Size(421, 49);
            this.express1Options.TabIndex = 5;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.labelAccountType);
            this.panelBottom.Controls.Add(this.originManagerControl);
            this.panelBottom.Controls.Add(this.labelOriginInfo);
            this.panelBottom.Controls.Add(this.accountControl);
            this.panelBottom.Location = new System.Drawing.Point(0, 170);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(495, 323);
            this.panelBottom.TabIndex = 6;
            // 
            // StampsSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.express1Options);
            this.Controls.Add(this.optionsControl);
            this.Name = "StampsSettingsControl";
            this.Size = new System.Drawing.Size(495, 507);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelOriginInfo;
        private ShipWorks.Shipping.Settings.Origin.ShippingOriginManagerControl originManagerControl;
        private System.Windows.Forms.Label labelAccountType;
        private StampsAccountManagerControl accountControl;
        private StampsOptionsControl optionsControl;
        private Stamps.Express1.Express1StampsSingleSourceControl express1Options;
        private System.Windows.Forms.Panel panelBottom;
    }
}
