namespace ShipWorks.Filters.Content.Editors
{
    partial class ConditionEditor
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
            this.conditionTypes = new ShipWorks.Filters.Content.Editors.ConditionChooser();
            this.SuspendLayout();
            // 
            // conditionTypes
            // 
            this.conditionTypes.AutoSize = true;
            this.conditionTypes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.conditionTypes.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.conditionTypes.ForeColor = System.Drawing.Color.Blue;
            this.conditionTypes.Location = new System.Drawing.Point(5, 6);
            this.conditionTypes.Name = "conditionTypes";
            this.conditionTypes.SelectedConditionType = null;
            this.conditionTypes.Size = new System.Drawing.Size(77, 13);
            this.conditionTypes.TabIndex = 0;
            this.conditionTypes.Text = "Condition type";
            // 
            // ConditionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.conditionTypes);
            this.Name = "ConditionEditor";
            this.Size = new System.Drawing.Size(172, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ConditionChooser conditionTypes;
    }
}
