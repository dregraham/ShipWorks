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
            this.labelOriginInfo = new System.Windows.Forms.Label();
            this.originManagerControl = new ShipWorks.Shipping.Settings.Origin.ShippingOriginManagerControl();
            this.SuspendLayout();
            // 
            // labelOriginInfo
            // 
            this.labelOriginInfo.AutoSize = true;
            this.labelOriginInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelOriginInfo.Location = new System.Drawing.Point(9, 4);
            this.labelOriginInfo.Name = "labelOriginInfo";
            this.labelOriginInfo.Size = new System.Drawing.Size(102, 13);
            this.labelOriginInfo.TabIndex = 0;
            this.labelOriginInfo.Text = "Origin Addresses";
            // 
            // originManagerControl
            // 
            this.originManagerControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.originManagerControl.Location = new System.Drawing.Point(12, 23);
            this.originManagerControl.Name = "originManagerControl";
            this.originManagerControl.Size = new System.Drawing.Size(452, 150);
            this.originManagerControl.TabIndex = 1;
            // 
            // PostalWebSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelOriginInfo);
            this.Controls.Add(this.originManagerControl);
            this.Name = "PostalWebSettingsControl";
            this.Size = new System.Drawing.Size(495, 193);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelOriginInfo;
        private ShipWorks.Shipping.Settings.Origin.ShippingOriginManagerControl originManagerControl;
    }
}
