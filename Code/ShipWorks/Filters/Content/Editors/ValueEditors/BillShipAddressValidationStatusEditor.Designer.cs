namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    partial class BillShipAddressValidationStatusEditor
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
            this.addressOperator = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            this.equalityOperator = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            this.addressValidationStatus = new ShipWorks.Filters.Content.Editors.ValueEditors.UI.AddressValidationStatusPopup();
            this.SuspendLayout();
            // 
            // addressOperator
            // 
            this.addressOperator.AutoSize = true;
            this.addressOperator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addressOperator.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addressOperator.ForeColor = System.Drawing.Color.Green;
            this.addressOperator.Location = new System.Drawing.Point(3, 6);
            this.addressOperator.Name = "addressOperator";
            this.addressOperator.Size = new System.Drawing.Size(93, 13);
            this.addressOperator.TabIndex = 0;
            this.addressOperator.Text = "Address Operator";
            // 
            // equalityOperator
            // 
            this.equalityOperator.AutoSize = true;
            this.equalityOperator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.equalityOperator.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.equalityOperator.ForeColor = System.Drawing.Color.Green;
            this.equalityOperator.Location = new System.Drawing.Point(102, 6);
            this.equalityOperator.Name = "equalityOperator";
            this.equalityOperator.Size = new System.Drawing.Size(92, 13);
            this.equalityOperator.TabIndex = 1;
            this.equalityOperator.Text = "Equality Operator";
            // 
            // addressValidationStatus
            // 
            this.addressValidationStatus.DropDownHeight = 293;
            this.addressValidationStatus.DropDownMinimumHeight = 293;
            this.addressValidationStatus.FormattingEnabled = true;
            this.addressValidationStatus.IntegralHeight = false;
            this.addressValidationStatus.Location = new System.Drawing.Point(200, 2);
            this.addressValidationStatus.Name = "addressValidationStatus";
            this.addressValidationStatus.Size = new System.Drawing.Size(179, 21);
            this.addressValidationStatus.TabIndex = 3;
            // 
            // BillShipAddressValidationStatusEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.addressValidationStatus);
            this.Controls.Add(this.equalityOperator);
            this.Controls.Add(this.addressOperator);
            this.Name = "BillShipAddressValidationStatusEditor";
            this.Size = new System.Drawing.Size(382, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChoiceLabel addressOperator;
        private ChoiceLabel equalityOperator;
        private UI.AddressValidationStatusPopup addressValidationStatus;
    }
}
