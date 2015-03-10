namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    partial class BillShipAddressStringValueEditor
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
            this.targetValue = new System.Windows.Forms.TextBox();
            this.addressOperator = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            this.stringOperator = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            this.SuspendLayout();
            // 
            // targetValue
            // 
            this.targetValue.Location = new System.Drawing.Point(190, 3);
            this.targetValue.Name = "targetValue";
            this.targetValue.Size = new System.Drawing.Size(233, 21);
            this.targetValue.TabIndex = 2;
            this.targetValue.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
            // 
            // addressOperator
            // 
            this.addressOperator.AutoSize = true;
            this.addressOperator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addressOperator.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.addressOperator.ForeColor = System.Drawing.Color.Green;
            this.addressOperator.Location = new System.Drawing.Point(3, 6);
            this.addressOperator.Name = "addressOperator";
            this.addressOperator.Size = new System.Drawing.Size(93, 13);
            this.addressOperator.TabIndex = 0;
            this.addressOperator.Text = "Address Operator";
            // 
            // stringOperator
            // 
            this.stringOperator.AutoSize = true;
            this.stringOperator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.stringOperator.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.stringOperator.ForeColor = System.Drawing.Color.Green;
            this.stringOperator.Location = new System.Drawing.Point(102, 6);
            this.stringOperator.Name = "stringOperator";
            this.stringOperator.Size = new System.Drawing.Size(82, 13);
            this.stringOperator.TabIndex = 1;
            this.stringOperator.Text = "String Operator";
            // 
            // AddressValueEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stringOperator);
            this.Controls.Add(this.addressOperator);
            this.Controls.Add(this.targetValue);
            this.Name = "AddressValueEditor";
            this.Size = new System.Drawing.Size(436, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox targetValue;
        private ChoiceLabel addressOperator;
        private ChoiceLabel stringOperator;
    }
}
