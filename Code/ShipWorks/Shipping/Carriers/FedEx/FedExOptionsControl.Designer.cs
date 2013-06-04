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
            this.thermalType = new System.Windows.Forms.ComboBox();
            this.labelThermalType = new System.Windows.Forms.Label();
            this.thermalDocTabType = new System.Windows.Forms.ComboBox();
            this.labelThermalDocTabType = new System.Windows.Forms.Label();
            this.thermalDocTab = new System.Windows.Forms.CheckBox();
            this.thermalPrinter = new System.Windows.Forms.CheckBox();
            this.labelLabels = new System.Windows.Forms.Label();
            this.maskAccountNumber = new System.Windows.Forms.CheckBox();
            this.infotipLabelType = new ShipWorks.UI.Controls.InfoTip();
            this.SuspendLayout();
            // 
            // thermalType
            // 
            this.thermalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.thermalType.Enabled = false;
            this.thermalType.FormattingEnabled = true;
            this.thermalType.Location = new System.Drawing.Point(114, 66);
            this.thermalType.Name = "thermalType";
            this.thermalType.Size = new System.Drawing.Size(115, 21);
            this.thermalType.TabIndex = 4;
            // 
            // labelThermalType
            // 
            this.labelThermalType.AutoSize = true;
            this.labelThermalType.Enabled = false;
            this.labelThermalType.Location = new System.Drawing.Point(38, 69);
            this.labelThermalType.Name = "labelThermalType";
            this.labelThermalType.Size = new System.Drawing.Size(74, 13);
            this.labelThermalType.TabIndex = 3;
            this.labelThermalType.Text = "Thermal type:";
            // 
            // thermalDocTabType
            // 
            this.thermalDocTabType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.thermalDocTabType.Enabled = false;
            this.thermalDocTabType.FormattingEnabled = true;
            this.thermalDocTabType.Location = new System.Drawing.Point(250, 112);
            this.thermalDocTabType.Name = "thermalDocTabType";
            this.thermalDocTabType.Size = new System.Drawing.Size(149, 21);
            this.thermalDocTabType.TabIndex = 7;
            // 
            // labelThermalDocTabType
            // 
            this.labelThermalDocTabType.AutoSize = true;
            this.labelThermalDocTabType.Enabled = false;
            this.labelThermalDocTabType.Location = new System.Drawing.Point(58, 115);
            this.labelThermalDocTabType.Name = "labelThermalDocTabType";
            this.labelThermalDocTabType.Size = new System.Drawing.Size(192, 13);
            this.labelThermalDocTabType.TabIndex = 6;
            this.labelThermalDocTabType.Text = "The doc-tab emerges from the printer:";
            // 
            // thermalDocTab
            // 
            this.thermalDocTab.AutoSize = true;
            this.thermalDocTab.Enabled = false;
            this.thermalDocTab.Location = new System.Drawing.Point(40, 92);
            this.thermalDocTab.Name = "thermalDocTab";
            this.thermalDocTab.Size = new System.Drawing.Size(166, 17);
            this.thermalDocTab.TabIndex = 5;
            this.thermalDocTab.Text = "My label stock has a doc-tab.";
            this.thermalDocTab.UseVisualStyleBackColor = true;
            this.thermalDocTab.CheckedChanged += new System.EventHandler(this.OnUpdateThermalUI);
            // 
            // thermalPrinter
            // 
            this.thermalPrinter.AutoSize = true;
            this.thermalPrinter.Location = new System.Drawing.Point(21, 47);
            this.thermalPrinter.Name = "thermalPrinter";
            this.thermalPrinter.Size = new System.Drawing.Size(253, 17);
            this.thermalPrinter.TabIndex = 2;
            this.thermalPrinter.Text = "The labels will be printed with a thermal printer.";
            this.thermalPrinter.UseVisualStyleBackColor = true;
            this.thermalPrinter.CheckedChanged += new System.EventHandler(this.OnUpdateThermalUI);
            // 
            // labelLabels
            // 
            this.labelLabels.AutoSize = true;
            this.labelLabels.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
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
            this.infotipLabelType.Location = new System.Drawing.Point(271, 49);
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
            this.Controls.Add(this.thermalType);
            this.Controls.Add(this.labelThermalType);
            this.Controls.Add(this.thermalDocTabType);
            this.Controls.Add(this.labelThermalDocTabType);
            this.Controls.Add(this.thermalDocTab);
            this.Controls.Add(this.thermalPrinter);
            this.Controls.Add(this.labelLabels);
            this.Controls.Add(this.maskAccountNumber);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "FedExOptionsControl";
            this.Size = new System.Drawing.Size(409, 144);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox thermalType;
        private System.Windows.Forms.Label labelThermalType;
        private System.Windows.Forms.ComboBox thermalDocTabType;
        private System.Windows.Forms.Label labelThermalDocTabType;
        private System.Windows.Forms.CheckBox thermalDocTab;
        private System.Windows.Forms.CheckBox thermalPrinter;
        private System.Windows.Forms.Label labelLabels;
        private System.Windows.Forms.CheckBox maskAccountNumber;
        private UI.Controls.InfoTip infotipLabelType;
    }
}
