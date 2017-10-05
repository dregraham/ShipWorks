﻿namespace ShipWorks.Shipping.Carriers.Dhl
{
    partial class DhlExpressSettingsControl
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
            this.carrierAccountManagerControl = new ShipWorks.Shipping.Carriers.CarrierAccountManagerControl();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // carrierAccountManagerControl
            // 
            this.carrierAccountManagerControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.carrierAccountManagerControl.Location = new System.Drawing.Point(25, 33);
            this.carrierAccountManagerControl.Name = "carrierAccountManagerControl";
            this.carrierAccountManagerControl.Size = new System.Drawing.Size(400, 168);
            this.carrierAccountManagerControl.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "DHL Express Accounts";
            // 
            // DhlExpressSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.carrierAccountManagerControl);
            this.Name = "DhlExpressSettingsControl";
            this.Size = new System.Drawing.Size(440, 220);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CarrierAccountManagerControl carrierAccountManagerControl;
        private System.Windows.Forms.Label label1;
    }
}
