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
            this.domesticThermal = new System.Windows.Forms.CheckBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.infotipLabelType = new ShipWorks.UI.Controls.InfoTip();
            this.infoTip1 = new ShipWorks.UI.Controls.InfoTip();
            this.internationalThermal = new System.Windows.Forms.CheckBox();
            this.labelThermalOption = new System.Windows.Forms.Label();
            this.labelThermalTypeHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // thermalType
            // 
            this.thermalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.thermalType.Enabled = false;
            this.thermalType.FormattingEnabled = true;
            this.thermalType.Location = new System.Drawing.Point(92, 98);
            this.thermalType.Name = "thermalType";
            this.thermalType.Size = new System.Drawing.Size(115, 21);
            this.thermalType.TabIndex = 3;
            // 
            // labelThermalType
            // 
            this.labelThermalType.AutoSize = true;
            this.labelThermalType.Enabled = false;
            this.labelThermalType.Location = new System.Drawing.Point(16, 101);
            this.labelThermalType.Name = "labelThermalType";
            this.labelThermalType.Size = new System.Drawing.Size(74, 13);
            this.labelThermalType.TabIndex = 2;
            this.labelThermalType.Text = "Thermal type:";
            // 
            // labelLabels
            // 
            this.labelLabels.AutoSize = true;
            this.labelLabels.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLabels.Location = new System.Drawing.Point(8, 6);
            this.labelLabels.Name = "labelLabels";
            this.labelLabels.Size = new System.Drawing.Size(43, 13);
            this.labelLabels.TabIndex = 0;
            this.labelLabels.Text = "Labels";
            // 
            // domesticThermal
            // 
            this.domesticThermal.AutoSize = true;
            this.domesticThermal.Location = new System.Drawing.Point(19, 38);
            this.domesticThermal.Name = "domesticThermal";
            this.domesticThermal.Size = new System.Drawing.Size(278, 17);
            this.domesticThermal.TabIndex = 1;
            this.domesticThermal.Text = "Domestic labels will be printed with a thermal printer.";
            this.domesticThermal.UseVisualStyleBackColor = true;
            this.domesticThermal.CheckedChanged += new System.EventHandler(this.OnUpdateThermalUI);
            // 
            // infotipLabelType
            // 
            this.infotipLabelType.Caption = "The printer type for a shipment cannot be changed after processing.";
            this.infotipLabelType.Location = new System.Drawing.Point(292, 40);
            this.infotipLabelType.Name = "infotipLabelType";
            this.infotipLabelType.Size = new System.Drawing.Size(12, 12);
            this.infotipLabelType.TabIndex = 46;
            this.infotipLabelType.Title = "Printer Type";
            // 
            // infoTip1
            // 
            this.infoTip1.Caption = "The printer type for a shipment cannot be changed after processing.";
            this.infoTip1.Location = new System.Drawing.Point(312, 60);
            this.infoTip1.Name = "infoTip1";
            this.infoTip1.Size = new System.Drawing.Size(12, 12);
            this.infoTip1.TabIndex = 48;
            this.infoTip1.Title = "Printer Type";
            // 
            // internationalThermal
            // 
            this.internationalThermal.AutoSize = true;
            this.internationalThermal.Location = new System.Drawing.Point(19, 58);
            this.internationalThermal.Name = "internationalThermal";
            this.internationalThermal.Size = new System.Drawing.Size(297, 17);
            this.internationalThermal.TabIndex = 47;
            this.internationalThermal.Text = "International labels will be printed with a thermal printer.";
            this.internationalThermal.UseVisualStyleBackColor = true;
            this.internationalThermal.CheckedChanged += new System.EventHandler(this.OnUpdateThermalUI);
            // 
            // labelThermalOption
            // 
            this.labelThermalOption.AutoSize = true;
            this.labelThermalOption.Location = new System.Drawing.Point(16, 22);
            this.labelThermalOption.Name = "labelThermalOption";
            this.labelThermalOption.Size = new System.Drawing.Size(156, 13);
            this.labelThermalOption.TabIndex = 49;
            this.labelThermalOption.Text = "Do you have a thermal printer?";
            // 
            // labelThermalTypeHeader
            // 
            this.labelThermalTypeHeader.AutoSize = true;
            this.labelThermalTypeHeader.Location = new System.Drawing.Point(16, 82);
            this.labelThermalTypeHeader.Name = "labelThermalTypeHeader";
            this.labelThermalTypeHeader.Size = new System.Drawing.Size(182, 13);
            this.labelThermalTypeHeader.TabIndex = 50;
            this.labelThermalTypeHeader.Text = "What format is your thermal printer?";
            // 
            // StampsOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelThermalTypeHeader);
            this.Controls.Add(this.labelThermalOption);
            this.Controls.Add(this.infoTip1);
            this.Controls.Add(this.internationalThermal);
            this.Controls.Add(this.infotipLabelType);
            this.Controls.Add(this.thermalType);
            this.Controls.Add(this.labelThermalType);
            this.Controls.Add(this.labelLabels);
            this.Controls.Add(this.domesticThermal);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "StampsOptionsControl";
            this.Size = new System.Drawing.Size(435, 132);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox thermalType;
        private System.Windows.Forms.Label labelThermalType;
        private System.Windows.Forms.Label labelLabels;
        private System.Windows.Forms.CheckBox domesticThermal;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private UI.Controls.InfoTip infotipLabelType;
        private UI.Controls.InfoTip infoTip1;
        private System.Windows.Forms.CheckBox internationalThermal;
        private System.Windows.Forms.Label labelThermalOption;
        private System.Windows.Forms.Label labelThermalTypeHeader;

    }
}
