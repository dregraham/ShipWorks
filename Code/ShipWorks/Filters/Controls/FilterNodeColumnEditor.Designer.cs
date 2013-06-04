namespace ShipWorks.Filters.Controls
{
    partial class FilterNodeColumnEditor
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
            this.labelColumnsToUse = new System.Windows.Forms.Label();
            this.radioUseCustom = new System.Windows.Forms.RadioButton();
            this.radioUseParent = new System.Windows.Forms.RadioButton();
            this.panelInheritance = new System.Windows.Forms.Panel();
            this.layoutEditor = new ShipWorks.Data.Grid.Columns.GridColumnLayoutEditor();
            this.panelInheritance.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelColumnsToUse
            // 
            this.labelColumnsToUse.AutoSize = true;
            this.labelColumnsToUse.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelColumnsToUse.Location = new System.Drawing.Point(5, 3);
            this.labelColumnsToUse.Name = "labelColumnsToUse";
            this.labelColumnsToUse.Size = new System.Drawing.Size(94, 13);
            this.labelColumnsToUse.TabIndex = 0;
            this.labelColumnsToUse.Text = "Columns to Use";
            // 
            // radioUseCustom
            // 
            this.radioUseCustom.AutoSize = true;
            this.radioUseCustom.Location = new System.Drawing.Point(16, 41);
            this.radioUseCustom.Name = "radioUseCustom";
            this.radioUseCustom.Size = new System.Drawing.Size(199, 17);
            this.radioUseCustom.TabIndex = 2;
            this.radioUseCustom.Text = "Use this filter\'s own column settings.";
            this.radioUseCustom.UseVisualStyleBackColor = true;
            // 
            // radioUseParent
            // 
            this.radioUseParent.AutoSize = true;
            this.radioUseParent.Location = new System.Drawing.Point(16, 20);
            this.radioUseParent.Name = "radioUseParent";
            this.radioUseParent.Size = new System.Drawing.Size(205, 17);
            this.radioUseParent.TabIndex = 1;
            this.radioUseParent.Text = "Use the columns of the parent folder.";
            this.radioUseParent.UseVisualStyleBackColor = true;
            // 
            // panelInheritance
            // 
            this.panelInheritance.Controls.Add(this.labelColumnsToUse);
            this.panelInheritance.Controls.Add(this.radioUseParent);
            this.panelInheritance.Controls.Add(this.radioUseCustom);
            this.panelInheritance.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInheritance.Location = new System.Drawing.Point(0, 0);
            this.panelInheritance.Name = "panelInheritance";
            this.panelInheritance.Size = new System.Drawing.Size(344, 63);
            this.panelInheritance.TabIndex = 0;
            // 
            // layoutEditor
            // 
            this.layoutEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutEditor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.layoutEditor.Location = new System.Drawing.Point(0, 63);
            this.layoutEditor.MinimumSize = new System.Drawing.Size(344, 200);
            this.layoutEditor.Name = "layoutEditor";
            this.layoutEditor.Size = new System.Drawing.Size(344, 284);
            this.layoutEditor.TabIndex = 1;
            // 
            // FilterNodeColumnEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutEditor);
            this.Controls.Add(this.panelInheritance);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MinimumSize = new System.Drawing.Size(344, 270);
            this.Name = "FilterNodeColumnEditor";
            this.Size = new System.Drawing.Size(344, 347);
            this.panelInheritance.ResumeLayout(false);
            this.panelInheritance.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelColumnsToUse;
        private System.Windows.Forms.RadioButton radioUseCustom;
        private System.Windows.Forms.RadioButton radioUseParent;
        private System.Windows.Forms.Panel panelInheritance;
        private ShipWorks.Data.Grid.Columns.GridColumnLayoutEditor layoutEditor;
    }
}
