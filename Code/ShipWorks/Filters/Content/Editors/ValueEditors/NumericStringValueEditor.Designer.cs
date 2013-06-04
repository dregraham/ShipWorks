namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    partial class NumericStringValueEditor<T>
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
            this.labelOperator = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            this.stringValueBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelOperator
            // 
            this.labelOperator.AutoSize = true;
            this.labelOperator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelOperator.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelOperator.ForeColor = System.Drawing.Color.Green;
            this.labelOperator.Location = new System.Drawing.Point(3, 6);
            this.labelOperator.Name = "labelOperator";
            this.labelOperator.Size = new System.Drawing.Size(140, 13);
            this.labelOperator.TabIndex = 0;
            this.labelOperator.Text = "Is Greater Than or Equal To";
            this.labelOperator.SelectedValueChanged += new System.EventHandler(this.OnChangeOperator);
            // 
            // stringValueBox
            // 
            this.stringValueBox.Location = new System.Drawing.Point(145, 3);
            this.stringValueBox.Name = "stringValueBox";
            this.stringValueBox.Size = new System.Drawing.Size(93, 21);
            this.stringValueBox.TabIndex = 1;
            // 
            // OrderNumberValueEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stringValueBox);
            this.Controls.Add(this.labelOperator);
            this.Name = "OrderNumberValueEditor";
            this.Size = new System.Drawing.Size(245, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChoiceLabel labelOperator;
        private System.Windows.Forms.TextBox stringValueBox;
    }
}
