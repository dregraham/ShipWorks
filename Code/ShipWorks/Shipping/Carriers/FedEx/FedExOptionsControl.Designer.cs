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
            this.labelFormat = new System.Windows.Forms.ComboBox();
            this.labelThermalType = new System.Windows.Forms.Label();
            this.thermalDocTabType = new System.Windows.Forms.ComboBox();
            this.labelThermalDocTabType = new System.Windows.Forms.Label();
            this.thermalDocTab = new System.Windows.Forms.CheckBox();
            this.labelLabels = new System.Windows.Forms.Label();
            this.maskAccountNumber = new System.Windows.Forms.CheckBox();
            this.infotipLabelType = new ShipWorks.UI.Controls.InfoTip();
            this.SuspendLayout();
            // 
            // labelFormat
            // 
            this.labelFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.labelFormat.FormattingEnabled = true;
            this.labelFormat.Location = new System.Drawing.Point(137, 45);
            this.labelFormat.Name = "labelFormat";
            this.labelFormat.Size = new System.Drawing.Size(115, 21);
            this.labelFormat.TabIndex = 4;
            // 
            // labelThermalType
            // 
            this.labelThermalType.AutoSize = true;
            this.labelThermalType.Location = new System.Drawing.Point(18, 48);
            this.labelThermalType.Name = "labelThermalType";
            this.labelThermalType.Size = new System.Drawing.Size(113, 13);
            this.labelThermalType.TabIndex = 3;
            this.labelThermalType.Text = "Requested label type:";
            // 
            // thermalDocTabType
            // 
            this.thermalDocTabType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.thermalDocTabType.Enabled = false;
            this.thermalDocTabType.FormattingEnabled = true;
            this.thermalDocTabType.Location = new System.Drawing.Point(250, 92);
            this.thermalDocTabType.Name = "thermalDocTabType";
            this.thermalDocTabType.Size = new System.Drawing.Size(149, 21);
            this.thermalDocTabType.TabIndex = 7;
            // 
            // labelThermalDocTabType
            // 
            this.labelThermalDocTabType.AutoSize = true;
            this.labelThermalDocTabType.Enabled = false;
            this.labelThermalDocTabType.Location = new System.Drawing.Point(58, 95);
            this.labelThermalDocTabType.Name = "labelThermalDocTabType";
            this.labelThermalDocTabType.Size = new System.Drawing.Size(192, 13);
            this.labelThermalDocTabType.TabIndex = 6;
            this.labelThermalDocTabType.Text = "The doc-tab emerges from the printer:";
            // 
            // thermalDocTab
            // 
            this.thermalDocTab.AutoSize = true;
            this.thermalDocTab.Enabled = false;
            this.thermalDocTab.Location = new System.Drawing.Point(40, 72);
            this.thermalDocTab.Name = "thermalDocTab";
            this.thermalDocTab.Size = new System.Drawing.Size(166, 17);
            this.thermalDocTab.TabIndex = 5;
            this.thermalDocTab.Text = "My label stock has a doc-tab.";
            this.thermalDocTab.UseVisualStyleBackColor = true;
            this.thermalDocTab.CheckedChanged += new System.EventHandler(this.OnUpdateThermalUI);
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
            // maskAccountNumber
            // 
            this.maskAccountNumber.AutoSize = true;
            this.maskAccountNumber.Location = new System.Drawing.Point(21, 24);
            this.maskAccountNumber.Name = "maskAccountNumber";
            this.maskAccountNumber.Size = new System.Drawing.Size(207, 17);
            this.maskAccountNumber.TabIndex = 1;
            this.maskAccountNumber.Text = "Mask FedEx account number on label.";
            this.maskAccountNumber.UseVisualStyleBackColor = true;
            // 
            // infotipLabelType
            // 
            this.infotipLabelType.Caption = "The printer type for a shipment cannot be changed after processing.";
            this.infotipLabelType.Location = new System.Drawing.Point(258, 49);
            this.infotipLabelType.Name = "infotipLabelType";
            this.infotipLabelType.Size = new System.Drawing.Size(12, 12);
            this.infotipLabelType.TabIndex = 32;
            this.infotipLabelType.Title = "Printer Type";
            // 
            // FedExOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.infotipLabelType);
            this.Controls.Add(this.labelFormat);
            this.Controls.Add(this.labelThermalType);
            this.Controls.Add(this.thermalDocTabType);
            this.Controls.Add(this.labelThermalDocTabType);
            this.Controls.Add(this.thermalDocTab);
            this.Controls.Add(this.labelLabels);
            this.Controls.Add(this.maskAccountNumber);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FedExOptionsControl";
            this.Size = new System.Drawing.Size(409, 128);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox labelFormat;
        private System.Windows.Forms.Label labelThermalType;
        private System.Windows.Forms.ComboBox thermalDocTabType;
        private System.Windows.Forms.Label labelThermalDocTabType;
        private System.Windows.Forms.CheckBox thermalDocTab;
        private System.Windows.Forms.Label labelLabels;
        private System.Windows.Forms.CheckBox maskAccountNumber;
        private UI.Controls.InfoTip infotipLabelType;
    }
}
