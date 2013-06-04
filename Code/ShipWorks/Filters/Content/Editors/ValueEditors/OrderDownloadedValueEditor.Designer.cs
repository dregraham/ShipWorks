namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    partial class OrderDownloadedValueEditor
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
            this.presenceOperator = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            this.dateSpecifiedOperator = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            this.SuspendLayout();
            // 
            // panelDateControls
            // 
            this.panelDateControls.Location = new System.Drawing.Point(183, 0);
            this.panelDateControls.Size = new System.Drawing.Size(741, 27);
            // 
            // presenceOperator
            // 
            this.presenceOperator.AutoSize = true;
            this.presenceOperator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.presenceOperator.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.presenceOperator.ForeColor = System.Drawing.Color.Green;
            this.presenceOperator.Location = new System.Drawing.Point(3, 6);
            this.presenceOperator.Name = "presenceOperator";
            this.presenceOperator.Size = new System.Drawing.Size(98, 13);
            this.presenceOperator.TabIndex = 244;
            this.presenceOperator.Text = "Presence Operator";
            // 
            // dateSpecifiedOperator
            // 
            this.dateSpecifiedOperator.AutoSize = true;
            this.dateSpecifiedOperator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dateSpecifiedOperator.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.dateSpecifiedOperator.ForeColor = System.Drawing.Color.Green;
            this.dateSpecifiedOperator.Location = new System.Drawing.Point(104, 6);
            this.dateSpecifiedOperator.Name = "dateSpecifiedOperator";
            this.dateSpecifiedOperator.Size = new System.Drawing.Size(77, 13);
            this.dateSpecifiedOperator.TabIndex = 245;
            this.dateSpecifiedOperator.Text = "Date Operator";
            // 
            // OrderDownloadedValueEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dateSpecifiedOperator);
            this.Controls.Add(this.presenceOperator);
            this.Name = "OrderDownloadedValueEditor";
            this.Size = new System.Drawing.Size(926, 27);
            this.Controls.SetChildIndex(this.panelDateControls, 0);
            this.Controls.SetChildIndex(this.presenceOperator, 0);
            this.Controls.SetChildIndex(this.dateSpecifiedOperator, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChoiceLabel presenceOperator;
        private ChoiceLabel dateSpecifiedOperator;

    }
}
