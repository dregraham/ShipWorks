namespace ShipWorks.Shipping.Carriers.iParcel
{
    partial class iParcelOptionsControl
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
            this.labelLabels = new System.Windows.Forms.Label();
            this.thermalPrinter = new System.Windows.Forms.CheckBox();
            this.infotipLabelType = new ShipWorks.UI.Controls.InfoTip();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.labelThermalNote = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelLabels
            // 
            this.labelLabels.AutoSize = true;
            this.labelLabels.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLabels.Location = new System.Drawing.Point(1, 2);
            this.labelLabels.Name = "labelLabels";
            this.labelLabels.Size = new System.Drawing.Size(43, 13);
            this.labelLabels.TabIndex = 53;
            this.labelLabels.Text = "Labels";
            // 
            // thermalPrinter
            // 
            this.thermalPrinter.AutoSize = true;
            this.thermalPrinter.Location = new System.Drawing.Point(20, 24);
            this.thermalPrinter.Name = "thermalPrinter";
            this.thermalPrinter.Size = new System.Drawing.Size(279, 17);
            this.thermalPrinter.TabIndex = 54;
            this.thermalPrinter.Text = "The labels will be printed with an EPL thermal printer.";
            this.thermalPrinter.UseVisualStyleBackColor = true;
            // 
            // infotipLabelType
            // 
            this.infotipLabelType.Caption = "The printer type for a shipment cannot be changed after processing.";
            this.infotipLabelType.Location = new System.Drawing.Point(294, 26);
            this.infotipLabelType.Name = "infotipLabelType";
            this.infotipLabelType.Size = new System.Drawing.Size(12, 12);
            this.infotipLabelType.TabIndex = 57;
            this.infotipLabelType.Title = "Printer Type";
            // 
            // labelThermalNote
            // 
            this.labelThermalNote.AutoSize = true;
            this.labelThermalNote.Location = new System.Drawing.Point(38, 45);
            this.labelThermalNote.Name = "labelThermalNote";
            this.labelThermalNote.Size = new System.Drawing.Size(233, 13);
            this.labelThermalNote.TabIndex = 58;
            this.labelThermalNote.Text = "Note: i-parcel only supports EPL thermal labels.";
            this.labelThermalNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // iParcelOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelThermalNote);
            this.Controls.Add(this.infotipLabelType);
            this.Controls.Add(this.labelLabels);
            this.Controls.Add(this.thermalPrinter);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "iParcelOptionsControl";
            this.Size = new System.Drawing.Size(348, 72);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.InfoTip infotipLabelType;
        private Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.Label labelLabels;
        private System.Windows.Forms.CheckBox thermalPrinter;
        private System.Windows.Forms.Label labelThermalNote;
    }
}
