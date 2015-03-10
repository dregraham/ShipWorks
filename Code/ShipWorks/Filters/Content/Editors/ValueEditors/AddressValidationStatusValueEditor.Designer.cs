namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    partial class AddressValidationStatusValueEditor
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
            this.addressValidationStatus = new ShipWorks.Filters.Content.Editors.ValueEditors.UI.AddressValidationStatusPopup();
            this.equalityOperator = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            this.SuspendLayout();
            // 
            // addressValidationStatus
            // 
            this.addressValidationStatus.FormattingEnabled = true;
            this.addressValidationStatus.IntegralHeight = false;
            this.addressValidationStatus.Location = new System.Drawing.Point(101, 3);
            this.addressValidationStatus.Name = "addressValidationStatus";
            this.addressValidationStatus.Size = new System.Drawing.Size(110, 21);
            this.addressValidationStatus.TabIndex = 2;
            // 
            // equalityOperator
            // 
            this.equalityOperator.AutoSize = true;
            this.equalityOperator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.equalityOperator.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.equalityOperator.ForeColor = System.Drawing.Color.Green;
            this.equalityOperator.Location = new System.Drawing.Point(3, 6);
            this.equalityOperator.Name = "equalityOperator";
            this.equalityOperator.Size = new System.Drawing.Size(92, 13);
            this.equalityOperator.TabIndex = 1;
            this.equalityOperator.Text = "Equality Operator";
            this.equalityOperator.SelectedValueChanged += new System.EventHandler(this.OnEqualityOperatorChanged);
            // 
            // AddressValidationStatusValueEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.addressValidationStatus);
            this.Controls.Add(this.equalityOperator);
            this.Name = "AddressValidationStatusValueEditor";
            this.Size = new System.Drawing.Size(250, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChoiceLabel equalityOperator;
        private UI.AddressValidationStatusPopup addressValidationStatus;
    }
}
