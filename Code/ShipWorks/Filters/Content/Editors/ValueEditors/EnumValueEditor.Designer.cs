namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    partial class EnumValueEditor<T>
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
            this.equalityOperator = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            this.targetValue = new System.Windows.Forms.ComboBox();
            this.targetValueList = new UI.ValueChoicePopup<T>();
            this.SuspendLayout();
            // 
            // equalityOperator
            // 
            this.equalityOperator.AutoSize = true;
            this.equalityOperator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.equalityOperator.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.equalityOperator.ForeColor = System.Drawing.Color.Green;
            this.equalityOperator.Location = new System.Drawing.Point(3, 6);
            this.equalityOperator.Name = "equalityOperator";
            this.equalityOperator.Size = new System.Drawing.Size(92, 13);
            this.equalityOperator.TabIndex = 0;
            this.equalityOperator.Text = "Equality Operator";
            this.equalityOperator.SelectedValueChanged += new System.EventHandler(this.OnChangeOperator);
            // 
            // targetValue
            // 
            this.targetValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetValue.FormattingEnabled = true;
            this.targetValue.Location = new System.Drawing.Point(101, 3);
            this.targetValue.MaxDropDownItems = 25;
            this.targetValue.Name = "targetValue";
            this.targetValue.Size = new System.Drawing.Size(157, 21);
            this.targetValue.TabIndex = 1;
            this.targetValue.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
            // 
            // targetValueList
            // 
            this.targetValueList.DropDownHeight = 293;
            this.targetValueList.DropDownMinimumHeight = 293;
            this.targetValueList.FormattingEnabled = true;
            this.targetValueList.IntegralHeight = false;
            this.targetValueList.Location = new System.Drawing.Point(200, 3);
            this.targetValueList.Name = "targetValueList";
            this.targetValueList.Size = new System.Drawing.Size(179, 21);
            this.targetValueList.TabIndex = 2;
            this.targetValueList.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
            // 
            // ValueChoiceEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.targetValue);
            this.Controls.Add(this.targetValueList);
            this.Controls.Add(this.equalityOperator);
            this.Name = "ValueChoiceEditor";
            this.Size = new System.Drawing.Size(587, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChoiceLabel equalityOperator;
        private System.Windows.Forms.ComboBox targetValue;
        private UI.ValueChoicePopup<T> targetValueList;
    }
}
