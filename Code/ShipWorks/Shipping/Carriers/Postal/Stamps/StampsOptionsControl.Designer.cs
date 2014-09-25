namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    partial class StampsOptionsControl
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
            this.components = new System.ComponentModel.Container();
            this.thermalType = new System.Windows.Forms.ComboBox();
            this.labelThermalType = new System.Windows.Forms.Label();
            this.labelLabels = new System.Windows.Forms.Label();
            this.thermalPrinter = new System.Windows.Forms.CheckBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.infotipLabelType = new ShipWorks.UI.Controls.InfoTip();
            this.labelLabelsInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // thermalType
            // 
            this.thermalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.thermalType.Enabled = false;
            this.thermalType.FormattingEnabled = true;
            this.thermalType.Location = new System.Drawing.Point(121, 48);
            this.thermalType.Name = "thermalType";
            this.thermalType.Size = new System.Drawing.Size(115, 21);
            this.thermalType.TabIndex = 3;
            // 
            // labelThermalType
            // 
            this.labelThermalType.AutoSize = true;
            this.labelThermalType.Enabled = false;
            this.labelThermalType.Location = new System.Drawing.Point(45, 51);
            this.labelThermalType.Name = "labelThermalType";
            this.labelThermalType.Size = new System.Drawing.Size(74, 13);
            this.labelThermalType.TabIndex = 2;
            this.labelThermalType.Text = "Thermal type:";
            // 
            // labelLabels
            // 
            this.labelLabels.AutoSize = true;
            this.labelLabels.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelLabels.Location = new System.Drawing.Point(8, 6);
            this.labelLabels.Name = "labelLabels";
            this.labelLabels.Size = new System.Drawing.Size(43, 13);
            this.labelLabels.TabIndex = 0;
            this.labelLabels.Text = "Labels";
            // 
            // thermalPrinter
            // 
            this.thermalPrinter.AutoSize = true;
            this.thermalPrinter.Location = new System.Drawing.Point(27, 28);
            this.thermalPrinter.Name = "thermalPrinter";
            this.thermalPrinter.Size = new System.Drawing.Size(253, 17);
            this.thermalPrinter.TabIndex = 1;
            this.thermalPrinter.Text = "The labels will be printed with a thermal printer.";
            this.thermalPrinter.UseVisualStyleBackColor = true;
            this.thermalPrinter.CheckedChanged += new System.EventHandler(this.OnUpdateThermalUI);
            // 
            // infotipLabelType
            // 
            this.infotipLabelType.Caption = "The printer type for a shipment cannot be changed after processing.";
            this.infotipLabelType.Location = new System.Drawing.Point(279, 30);
            this.infotipLabelType.Name = "infotipLabelType";
            this.infotipLabelType.Size = new System.Drawing.Size(12, 12);
            this.infotipLabelType.TabIndex = 46;
            this.infotipLabelType.Title = "Printer Type";
            // 
            // labelLabelsInfo
            // 
            this.labelLabelsInfo.ForeColor = System.Drawing.Color.DimGray;
            this.labelLabelsInfo.Location = new System.Drawing.Point(23, 77);
            this.labelLabelsInfo.Name = "labelLabelsInfo";
            this.labelLabelsInfo.Size = new System.Drawing.Size(409, 34);
            this.labelLabelsInfo.TabIndex = 47;
            this.labelLabelsInfo.Text = "Stamps.com does not support thermal labels for international shipments.  Internat" +
                "ional shipments will print using a standard printer regardless of this setting.";
            // 
            // StampsOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelLabelsInfo);
            this.Controls.Add(this.infotipLabelType);
            this.Controls.Add(this.thermalType);
            this.Controls.Add(this.labelThermalType);
            this.Controls.Add(this.labelLabels);
            this.Controls.Add(this.thermalPrinter);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "StampsOptionsControl";
            this.Size = new System.Drawing.Size(435, 123);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox thermalType;
        private System.Windows.Forms.Label labelThermalType;
        private System.Windows.Forms.Label labelLabels;
        private System.Windows.Forms.CheckBox thermalPrinter;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private UI.Controls.InfoTip infotipLabelType;
        private System.Windows.Forms.Label labelLabelsInfo;

    }
}
