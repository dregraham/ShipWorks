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
            this.label1 = new System.Windows.Forms.Label();
            this.stampsAccountControl = new ShipWorks.Shipping.Carriers.Postal.Stamps.StampsAccountManagerControl();
            this.optionsControl = new ShipWorks.Shipping.Carriers.Postal.Stamps.StampsOptionsControl();
            this.SuspendLayout();
            // 
            // labelOriginInfo
            // 
            this.labelOriginInfo.AutoSize = true;
            this.labelOriginInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelOriginInfo.Location = new System.Drawing.Point(9, 251);
            this.labelOriginInfo.Name = "labelOriginInfo";
            this.labelOriginInfo.Size = new System.Drawing.Size(102, 13);
            this.labelOriginInfo.TabIndex = 2;
            this.labelOriginInfo.Text = "Origin Addresses";
            // 
            // originManagerControl
            // 
            this.originManagerControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.originManagerControl.Location = new System.Drawing.Point(12, 270);
            this.originManagerControl.Name = "originManagerControl";
            this.originManagerControl.Size = new System.Drawing.Size(454, 150);
            this.originManagerControl.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.Location = new System.Drawing.Point(9, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Stamps.com Accounts";
            // 
            // stampsAccountControl
            // 
            this.stampsAccountControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.stampsAccountControl.Location = new System.Drawing.Point(9, 134);
            this.stampsAccountControl.Name = "stampsAccountControl";
            this.stampsAccountControl.Size = new System.Drawing.Size(459, 104);
            this.stampsAccountControl.TabIndex = 1;
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.optionsControl.Location = new System.Drawing.Point(0, -1);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(435, 105);
            this.optionsControl.TabIndex = 4;
            // 
            // StampsSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.optionsControl);
            this.Controls.Add(this.stampsAccountControl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelOriginInfo);
            this.Controls.Add(this.originManagerControl);
            this.Name = "StampsSettingsControl";
            this.Size = new System.Drawing.Size(495, 444);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelOriginInfo;
        private ShipWorks.Shipping.Settings.Origin.ShippingOriginManagerControl originManagerControl;
        private System.Windows.Forms.Label label1;
        private StampsAccountManagerControl stampsAccountControl;
        private StampsOptionsControl optionsControl;
    }
}
