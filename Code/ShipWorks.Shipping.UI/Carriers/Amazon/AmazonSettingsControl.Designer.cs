using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Amazon;

namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    partial class AmazonSettingsControl
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
            this.amazonOptionsControl1 = new ShipWorks.Shipping.Carriers.Amazon.AmazonOptionsControl();
            this.servicePicker = new System.Windows.Forms.CheckedListBox();
            this.servicePickerTitle = new System.Windows.Forms.Label();
            this.servicePickerDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // amazonOptionsControl1
            // 
            this.amazonOptionsControl1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.amazonOptionsControl1.Location = new System.Drawing.Point(2, 0);
            this.amazonOptionsControl1.Name = "amazonOptionsControl1";
            this.amazonOptionsControl1.Size = new System.Drawing.Size(377, 46);
            this.amazonOptionsControl1.TabIndex = 25;
            // 
            // servicePicker
            // 
            this.servicePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.servicePicker.CheckOnClick = true;
            this.servicePicker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.servicePicker.FormattingEnabled = true;
            this.servicePicker.Location = new System.Drawing.Point(29, 114);
            this.servicePicker.Name = "servicePicker";
            this.servicePicker.Size = new System.Drawing.Size(401, 164);
            this.servicePicker.TabIndex = 12;
            // 
            // servicePickerTitle
            // 
            this.servicePickerTitle.AutoSize = true;
            this.servicePickerTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.servicePickerTitle.Location = new System.Drawing.Point(11, 61);
            this.servicePickerTitle.Name = "servicePickerTitle";
            this.servicePickerTitle.Size = new System.Drawing.Size(110, 13);
            this.servicePickerTitle.TabIndex = 26;
            this.servicePickerTitle.Text = "Available Services";
            // 
            // servicePickerDescription
            // 
            this.servicePickerDescription.Location = new System.Drawing.Point(26, 80);
            this.servicePickerDescription.Name = "servicePickerDescription";
            this.servicePickerDescription.Size = new System.Drawing.Size(404, 27);
            this.servicePickerDescription.TabIndex = 27;
            this.servicePickerDescription.Text = "ShipWorks can be configured to only show the service types that are important to " +
    "you. Simply select the services below.";
            // 
            // AmazonSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.servicePickerDescription);
            this.Controls.Add(this.servicePickerTitle);
            this.Controls.Add(this.amazonOptionsControl1);
            this.Controls.Add(this.servicePicker);
            this.Name = "AmazonSettingsControl";
            this.Size = new System.Drawing.Size(445, 288);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private AmazonOptionsControl amazonOptionsControl1;
        private CheckedListBox servicePicker;
        private Label servicePickerTitle;
        private Label servicePickerDescription;
    }
}
