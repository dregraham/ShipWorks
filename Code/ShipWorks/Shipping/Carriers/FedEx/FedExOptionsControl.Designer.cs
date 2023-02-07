namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExOptionsControl
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
            this.thermalDocTabType = new System.Windows.Forms.ComboBox();
            this.labelThermalDocTabType = new System.Windows.Forms.Label();
            this.thermalDocTab = new System.Windows.Forms.CheckBox();
            this.labelLabels = new System.Windows.Forms.Label();
            this.maskAccountNumber = new System.Windows.Forms.CheckBox();
            this.requestedLabelFormat = new ShipWorks.Shipping.Editing.RequestedLabelFormatOptionControl();
            this.SuspendLayout();
            // 
            // labelLabels
            // 
            this.labelLabels.AutoSize = true;
            this.labelLabels.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLabels.Location = new System.Drawing.Point(2, 2);
            this.labelLabels.Name = "labelLabels";
            this.labelLabels.Size = new System.Drawing.Size(43, 13);
            this.labelLabels.TabIndex = 0;
            this.labelLabels.Text = "Labels";
            // 
            // requestedLabelFormat
            // 
            this.requestedLabelFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.requestedLabelFormat.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.requestedLabelFormat.Location = new System.Drawing.Point(19, 22);
            this.requestedLabelFormat.Name = "requestedLabelFormat";
            this.requestedLabelFormat.Size = new System.Drawing.Size(340, 25);
            this.requestedLabelFormat.TabIndex = 59;
            #region Removed For FedEx Platform
            // 
            // maskAccountNumber
            // 
            this.maskAccountNumber.AutoSize = true;
            this.maskAccountNumber.Location = new System.Drawing.Point(21, 22);
            this.maskAccountNumber.Name = "maskAccountNumber";
            this.maskAccountNumber.Size = new System.Drawing.Size(207, 17);
            this.maskAccountNumber.TabIndex = 1;
            this.maskAccountNumber.Text = "Mask FedEx account number on label.";
            this.maskAccountNumber.UseVisualStyleBackColor = true;
            // 
            // thermalDocTabType
            // 
            this.thermalDocTabType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.thermalDocTabType.FormattingEnabled = true;
            this.thermalDocTabType.Location = new System.Drawing.Point(250, 90);
            this.thermalDocTabType.Name = "thermalDocTabType";
            this.thermalDocTabType.Size = new System.Drawing.Size(149, 21);
            this.thermalDocTabType.TabIndex = 7;
            // 
            // labelThermalDocTabType
            // 
            this.labelThermalDocTabType.AutoSize = true;
            this.labelThermalDocTabType.Location = new System.Drawing.Point(58, 93);
            this.labelThermalDocTabType.Name = "labelThermalDocTabType";
            this.labelThermalDocTabType.Size = new System.Drawing.Size(192, 13);
            this.labelThermalDocTabType.TabIndex = 6;
            this.labelThermalDocTabType.Text = "The doc-tab emerges from the printer:";
            // 
            // thermalDocTab
            // 
            this.thermalDocTab.AutoSize = true;
            this.thermalDocTab.Location = new System.Drawing.Point(40, 70);
            this.thermalDocTab.Name = "thermalDocTab";
            this.thermalDocTab.Size = new System.Drawing.Size(166, 17);
            this.thermalDocTab.TabIndex = 5;
            this.thermalDocTab.Text = "My label stock has a doc-tab.";
            this.thermalDocTab.UseVisualStyleBackColor = true;
            #endregion
            // 
            // FedExOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.requestedLabelFormat);
            this.Controls.Add(this.labelLabels);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FedExOptionsControl";
            this.Size = new System.Drawing.Size(409, 45);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox thermalDocTabType;
        private System.Windows.Forms.Label labelThermalDocTabType;
        private System.Windows.Forms.CheckBox thermalDocTab;
        private System.Windows.Forms.Label labelLabels;
        private System.Windows.Forms.CheckBox maskAccountNumber;
        private Editing.RequestedLabelFormatOptionControl requestedLabelFormat;
    }
}
