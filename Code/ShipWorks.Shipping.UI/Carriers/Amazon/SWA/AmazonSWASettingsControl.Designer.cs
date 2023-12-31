﻿using ShipWorks.Shipping.UI.Carriers.Amazon.SWA;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    partial class AmazonSWASettingsControl
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
            this.carrierAccountManagerControl = new ShipWorks.Shipping.UI.Carriers.Amazon.SWA.AmazonSWAAccountManagerControl();
            this.label1 = new System.Windows.Forms.Label();
            this.shippingCutoff = new ShipWorks.Shipping.Editing.ShippingDateCutoffControl();
            this.labelsLabel = new System.Windows.Forms.Label();
            this.requestedLabelFormatOptionControl = new ShipWorks.Shipping.Editing.RequestedLabelFormatOptionControl();
            this.excludedServiceControl = new ShipWorks.Shipping.UI.Carriers.Amazon.SWA.AmazonSWAServicePickerControl();
            this.labelThermalNote = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // carrierAccountManagerControl
            // 
            this.carrierAccountManagerControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.carrierAccountManagerControl.Location = new System.Drawing.Point(25, 100);
            this.carrierAccountManagerControl.Name = "carrierAccountManagerControl";
            this.carrierAccountManagerControl.Size = new System.Drawing.Size(462, 80);
            this.carrierAccountManagerControl.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Amazon Shipping Accounts";
            // 
            // shippingCutoff
            // 
            this.shippingCutoff.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.shippingCutoff.Location = new System.Drawing.Point(22, 50);
            this.shippingCutoff.Name = "shippingCutoff";
            this.shippingCutoff.Size = new System.Drawing.Size(467, 22);
            this.shippingCutoff.TabIndex = 16;
            // 
            // labelsLabel
            // 
            this.labelsLabel.AutoSize = true;
            this.labelsLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelsLabel.Location = new System.Drawing.Point(6, 8);
            this.labelsLabel.Name = "labelsLabel";
            this.labelsLabel.Size = new System.Drawing.Size(43, 13);
            this.labelsLabel.TabIndex = 17;
            this.labelsLabel.Text = "Labels";
            // 
            // requestedLabelFormatOptionControl
            // 
            this.requestedLabelFormatOptionControl.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.requestedLabelFormatOptionControl.Location = new System.Drawing.Point(22, 24);
            this.requestedLabelFormatOptionControl.Name = "requestedLabelFormatOptionControl";
            this.requestedLabelFormatOptionControl.Size = new System.Drawing.Size(465, 23);
            this.requestedLabelFormatOptionControl.TabIndex = 18;
            // 
            // excludedServiceControl
            // 
            this.excludedServiceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excludedServiceControl.Location = new System.Drawing.Point(9, 208);
            this.excludedServiceControl.Name = "excludedServiceControl";
            this.excludedServiceControl.Size = new System.Drawing.Size(384, 198);
            this.excludedServiceControl.TabIndex = 19;
            // 
            // labelThermalNote
            // 
            this.labelThermalNote.AutoSize = true;
            this.labelThermalNote.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelThermalNote.Location = new System.Drawing.Point(22, 184);
            this.labelThermalNote.Name = "labelThermalNote";
            this.labelThermalNote.Size = new System.Drawing.Size(354, 13);
            this.labelThermalNote.TabIndex = 104;
            this.labelThermalNote.Text = "Note: ShipWorks supports one Amazon shipping account per installation.";
            this.labelThermalNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AmazonSWASettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelThermalNote);
            this.Controls.Add(this.excludedServiceControl);
            this.Controls.Add(this.requestedLabelFormatOptionControl);
            this.Controls.Add(this.labelsLabel);
            this.Controls.Add(this.shippingCutoff);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.carrierAccountManagerControl);
            this.Name = "AmazonSWASettingsControl";
            this.Size = new System.Drawing.Size(500, 402);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AmazonSWAAccountManagerControl carrierAccountManagerControl;
        private System.Windows.Forms.Label label1;
        private Editing.ShippingDateCutoffControl shippingCutoff;
        private System.Windows.Forms.Label labelsLabel;
        private Editing.RequestedLabelFormatOptionControl requestedLabelFormatOptionControl;
        private UI.Carriers.Amazon.SWA.AmazonSWAServicePickerControl excludedServiceControl;
        private System.Windows.Forms.Label labelThermalNote;
    }
}
