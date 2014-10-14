namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    partial class ClaimFiledValueEditor
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
            this.beenFileLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelOperator
            // 
            this.labelOperator.AutoSize = true;
            this.labelOperator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelOperator.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOperator.ForeColor = System.Drawing.Color.Green;
            this.labelOperator.Location = new System.Drawing.Point(-3, 6);
            this.labelOperator.Name = "labelOperator";
            this.labelOperator.Size = new System.Drawing.Size(24, 13);
            this.labelOperator.TabIndex = 0;
            this.labelOperator.Text = "has";
            this.labelOperator.SelectedValueChanged += new System.EventHandler(this.OnChangeOperator);
            // 
            // beenFileLabel
            // 
            this.beenFileLabel.AutoSize = true;
            this.beenFileLabel.Location = new System.Drawing.Point(18, 6);
            this.beenFileLabel.Name = "beenFileLabel";
            this.beenFileLabel.Size = new System.Drawing.Size(54, 13);
            this.beenFileLabel.TabIndex = 3;
            this.beenFileLabel.Text = "been filed";
            // 
            // ClaimFiledValueEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.beenFileLabel);
            this.Controls.Add(this.labelOperator);
            this.Name = "ClaimFiledValueEditor";
            this.Size = new System.Drawing.Size(128, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChoiceLabel labelOperator;
        private System.Windows.Forms.Label beenFileLabel;
    }
}
