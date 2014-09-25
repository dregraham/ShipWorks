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
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.labelThermalNote = new System.Windows.Forms.Label();
            this.requestedLabelFormat = new ShipWorks.Shipping.Editing.RequestedLabelFormatOptionControl();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelLabels
            // 
            this.labelLabels.AutoSize = true;
            this.labelLabels.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLabels.Location = new System.Drawing.Point(2, 2);
            this.labelLabels.Name = "labelLabels";
            this.labelLabels.Size = new System.Drawing.Size(43, 13);
            this.labelLabels.TabIndex = 53;
            this.labelLabels.Text = "Labels";
            // 
            // labelThermalNote
            // 
            this.labelThermalNote.AutoSize = true;
            this.labelThermalNote.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelThermalNote.Location = new System.Drawing.Point(19, 49);
            this.labelThermalNote.Name = "labelThermalNote";
            this.labelThermalNote.Size = new System.Drawing.Size(233, 13);
            this.labelThermalNote.TabIndex = 58;
            this.labelThermalNote.Text = "Note: i-parcel only supports EPL thermal labels.";
            this.labelThermalNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // requestedLabelFormat
            // 
            this.requestedLabelFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.requestedLabelFormat.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.requestedLabelFormat.Location = new System.Drawing.Point(19, 23);
            this.requestedLabelFormat.Name = "requestedLabelFormat";
            this.requestedLabelFormat.Size = new System.Drawing.Size(369, 25);
            this.requestedLabelFormat.TabIndex = 60;
            // 
            // iParcelOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.requestedLabelFormat);
            this.Controls.Add(this.labelThermalNote);
            this.Controls.Add(this.labelLabels);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "iParcelOptionsControl";
            this.Size = new System.Drawing.Size(377, 70);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.Label labelLabels;
        private System.Windows.Forms.Label labelThermalNote;
        private Editing.RequestedLabelFormatOptionControl requestedLabelFormat;
    }
}
