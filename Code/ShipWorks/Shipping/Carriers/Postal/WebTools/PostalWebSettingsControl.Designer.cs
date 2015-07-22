namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    partial class PostalWebSettingsControl
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
            this.servicePicker = new ShipWorks.Shipping.Carriers.Postal.PostalServicePickerControl();
            this.labelOriginInfo = new System.Windows.Forms.Label();
            this.originManagerControl = new ShipWorks.Shipping.Settings.Origin.ShippingOriginManagerControl();
            this.packagePicker = new ShipWorks.Shipping.Carriers.Postal.PostalPackagePickerControl();
            this.SuspendLayout();
            // 
            // servicePicker
            // 
            this.servicePicker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.servicePicker.Location = new System.Drawing.Point(9, 189);
            this.servicePicker.Name = "servicePicker";
            this.servicePicker.Size = new System.Drawing.Size(486, 200);
            this.servicePicker.TabIndex = 10;
            // 
            // labelOriginInfo
            // 
            this.labelOriginInfo.AutoSize = true;
            this.labelOriginInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOriginInfo.Location = new System.Drawing.Point(8, 5);
            this.labelOriginInfo.Name = "labelOriginInfo";
            this.labelOriginInfo.Size = new System.Drawing.Size(102, 13);
            this.labelOriginInfo.TabIndex = 0;
            this.labelOriginInfo.Text = "Origin Addresses";
            // 
            // originManagerControl
            // 
            this.originManagerControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originManagerControl.Location = new System.Drawing.Point(27, 26);
            this.originManagerControl.Name = "originManagerControl";
            this.originManagerControl.Size = new System.Drawing.Size(452, 150);
            this.originManagerControl.TabIndex = 1;
            // 
            // postalServicePickerControl1
            // 
            this.packagePicker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.packagePicker.Location = new System.Drawing.Point(9, 399);
            this.packagePicker.Name = "postalServicePickerControl1";
            this.packagePicker.Size = new System.Drawing.Size(486, 200);
            this.packagePicker.TabIndex = 11;
            // 
            // PostalWebSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.packagePicker);
            this.Controls.Add(this.servicePicker);
            this.Controls.Add(this.labelOriginInfo);
            this.Controls.Add(this.originManagerControl);
            this.Name = "PostalWebSettingsControl";
            this.Size = new System.Drawing.Size(495, 603);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelOriginInfo;
        private ShipWorks.Shipping.Settings.Origin.ShippingOriginManagerControl originManagerControl;
        private PostalServicePickerControl servicePicker;
        private PostalPackagePickerControl packagePicker;
    }
}
