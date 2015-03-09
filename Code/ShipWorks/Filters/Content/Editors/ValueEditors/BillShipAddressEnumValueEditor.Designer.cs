namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    partial class BillShipAddressEnumValueEditor<T>
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
            this.targetValue = new System.Windows.Forms.ComboBox();
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
            // targetValue
            // 
            this.targetValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetValue.FormattingEnabled = true;
            this.targetValue.Location = new System.Drawing.Point(200, 3);
            this.targetValue.MaxDropDownItems = 25;
            this.targetValue.Name = "targetValue";
            this.targetValue.Size = new System.Drawing.Size(233, 21);
            this.targetValue.TabIndex = 2;
            this.targetValue.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
            // 
            // BillShipAddressEnumValueEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.targetValue);
            this.Controls.Add(this.equalityOperator);
            this.Controls.Add(this.addressOperator);
            this.Name = "BillShipAddressEnumValueEditor";
            this.Size = new System.Drawing.Size(436, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChoiceLabel addressOperator;
        private ChoiceLabel equalityOperator;
        private System.Windows.Forms.ComboBox targetValue;
    }
}
