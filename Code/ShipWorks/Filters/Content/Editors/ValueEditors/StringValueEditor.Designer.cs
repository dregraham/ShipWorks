namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    partial class StringValueEditor
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
            this.targetValueBox = new System.Windows.Forms.TextBox();
            this.labelOperator = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            this.targetValueList = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // targetValueBox
            // 
            this.targetValueBox.Location = new System.Drawing.Point(67, 3);
            this.targetValueBox.MaxLength = 3998;
            this.targetValueBox.Name = "targetValueBox";
            this.targetValueBox.Size = new System.Drawing.Size(233, 21);
            this.targetValueBox.TabIndex = 1;
            this.targetValueBox.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
            // 
            // labelOperator
            // 
            this.labelOperator.AutoSize = true;
            this.labelOperator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelOperator.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelOperator.ForeColor = System.Drawing.Color.Green;
            this.labelOperator.Location = new System.Drawing.Point(3, 6);
            this.labelOperator.Name = "labelOperator";
            this.labelOperator.Size = new System.Drawing.Size(59, 13);
            this.labelOperator.TabIndex = 0;
            this.labelOperator.Text = "Starts with";
            this.labelOperator.SelectedValueChanged += new System.EventHandler(this.OnChangeOperator);
            // 
            // targetValueList
            // 
            this.targetValueList.FormattingEnabled = true;
            this.targetValueList.Location = new System.Drawing.Point(306, 3);
            this.targetValueList.Name = "targetValueList";
            this.targetValueList.Size = new System.Drawing.Size(233, 21);
            this.targetValueList.TabIndex = 2;
            this.targetValueList.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
            // 
            // StringValueEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.targetValueList);
            this.Controls.Add(this.labelOperator);
            this.Controls.Add(this.targetValueBox);
            this.Name = "StringValueEditor";
            this.Size = new System.Drawing.Size(638, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox targetValueBox;
        private ChoiceLabel labelOperator;
        private System.Windows.Forms.ComboBox targetValueList;
    }
}
