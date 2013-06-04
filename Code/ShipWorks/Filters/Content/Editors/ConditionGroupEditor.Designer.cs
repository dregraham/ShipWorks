namespace ShipWorks.Filters.Content.Editors
{
    partial class ConditionGroupEditor
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
            this.labelAfter = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.joinType = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            this.SuspendLayout();
            // 
            // labelAfter
            // 
            this.labelAfter.ForeColor = System.Drawing.Color.DimGray;
            this.labelAfter.Location = new System.Drawing.Point(40, 5);
            this.labelAfter.Name = "labelAfter";
            this.labelAfter.Size = new System.Drawing.Size(174, 18);
            this.labelAfter.TabIndex = 2;
            this.labelAfter.Text = "of the following conditions are met";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "If";
            // 
            // joinType
            // 
            this.joinType.AutoSize = true;
            this.joinType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.joinType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.joinType.ForeColor = System.Drawing.Color.Red;
            this.joinType.LabelUsage = ShipWorks.Filters.Content.Editors.ChoiceLabelUsage.Join;
            this.joinType.Location = new System.Drawing.Point(16, 5);
            this.joinType.Name = "joinType";
            this.joinType.Size = new System.Drawing.Size(17, 13);
            this.joinType.TabIndex = 1;
            this.joinType.Text = "all";
            this.joinType.SelectedValueChanged += new System.EventHandler(this.OnChangeJoinType);
            // 
            // ConditionGroupEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.joinType);
            this.Controls.Add(this.labelAfter);
            this.Controls.Add(this.label1);
            this.Name = "ConditionGroupEditor";
            this.Size = new System.Drawing.Size(279, 26);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAfter;
        private System.Windows.Forms.Label label1;
        private ChoiceLabel joinType;

    }
}
