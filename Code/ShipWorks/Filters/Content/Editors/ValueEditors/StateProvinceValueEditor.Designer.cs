namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    partial class StateProvinceValueEditor
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
            this.targetValue = new System.Windows.Forms.ComboBox();
            this.equalityOperator = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            this.SuspendLayout();
            // 
            // targetValue
            // 
            this.targetValue.FormattingEnabled = true;
            this.targetValue.Location = new System.Drawing.Point(89, 3);
            this.targetValue.MaxDropDownItems = 25;
            this.targetValue.Name = "targetValue";
            this.targetValue.Size = new System.Drawing.Size(157, 21);
            this.targetValue.TabIndex = 4;
            this.targetValue.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
            // 
            // equalityOperator
            // 
            this.equalityOperator.AutoSize = true;
            this.equalityOperator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.equalityOperator.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.equalityOperator.ForeColor = System.Drawing.Color.Green;
            this.equalityOperator.Location = new System.Drawing.Point(3, 6);
            this.equalityOperator.Name = "equalityOperator";
            this.equalityOperator.Size = new System.Drawing.Size(82, 13);
            this.equalityOperator.TabIndex = 3;
            this.equalityOperator.Text = "String Operator";
            // 
            // StateProvinceValueEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.targetValue);
            this.Controls.Add(this.equalityOperator);
            this.Name = "StateProvinceValueEditor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox targetValue;
        private ChoiceLabel equalityOperator;
    }
}
