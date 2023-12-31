﻿namespace ShipWorks.Shipping.Carriers.Postal.WebTools
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
            this.labelLabels = new System.Windows.Forms.Label();
            this.shippingCutoff = new ShipWorks.Shipping.Editing.ShippingDateCutoffControl();
            this.SuspendLayout();
            // 
            // servicePicker
            // 
            this.servicePicker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.servicePicker.Location = new System.Drawing.Point(9, 238);
            this.servicePicker.Name = "servicePicker";
            this.servicePicker.Size = new System.Drawing.Size(486, 200);
            this.servicePicker.TabIndex = 10;
            // 
            // labelOriginInfo
            // 
            this.labelOriginInfo.AutoSize = true;
            this.labelOriginInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOriginInfo.Location = new System.Drawing.Point(8, 54);
            this.labelOriginInfo.Name = "labelOriginInfo";
            this.labelOriginInfo.Size = new System.Drawing.Size(102, 13);
            this.labelOriginInfo.TabIndex = 0;
            this.labelOriginInfo.Text = "Origin Addresses";
            // 
            // originManagerControl
            // 
            this.originManagerControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originManagerControl.Location = new System.Drawing.Point(27, 75);
            this.originManagerControl.Name = "originManagerControl";
            this.originManagerControl.Size = new System.Drawing.Size(452, 150);
            this.originManagerControl.TabIndex = 1;
            // 
            // packagePicker
            // 
            this.packagePicker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.packagePicker.Location = new System.Drawing.Point(9, 448);
            this.packagePicker.Name = "packagePicker";
            this.packagePicker.Size = new System.Drawing.Size(486, 200);
            this.packagePicker.TabIndex = 11;
            // 
            // labelLabels
            // 
            this.labelLabels.AutoSize = true;
            this.labelLabels.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLabels.Location = new System.Drawing.Point(8, 5);
            this.labelLabels.Name = "labelLabels";
            this.labelLabels.Size = new System.Drawing.Size(43, 13);
            this.labelLabels.TabIndex = 12;
            this.labelLabels.Text = "Labels";
            // 
            // shippingCutoff
            // 
            this.shippingCutoff.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.shippingCutoff.Location = new System.Drawing.Point(24, 25);
            this.shippingCutoff.Name = "shippingCutoff";
            this.shippingCutoff.Size = new System.Drawing.Size(467, 22);
            this.shippingCutoff.TabIndex = 13;
            // 
            // PostalWebSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.shippingCutoff);
            this.Controls.Add(this.labelLabels);
            this.Controls.Add(this.packagePicker);
            this.Controls.Add(this.servicePicker);
            this.Controls.Add(this.labelOriginInfo);
            this.Controls.Add(this.originManagerControl);
            this.Name = "PostalWebSettingsControl";
            this.Size = new System.Drawing.Size(501, 664);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelOriginInfo;
        private ShipWorks.Shipping.Settings.Origin.ShippingOriginManagerControl originManagerControl;
        private PostalServicePickerControl servicePicker;
        private PostalPackagePickerControl packagePicker;
        private System.Windows.Forms.Label labelLabels;
        private Editing.ShippingDateCutoffControl shippingCutoff;
    }
}
