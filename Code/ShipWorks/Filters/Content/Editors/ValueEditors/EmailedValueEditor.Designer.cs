namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    partial class EmailedValueEditor
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
            this.labelQueryType = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            this.SuspendLayout();
            // 
            // panelTemplateSelection
            // 
            this.panelTemplateSelection.Location = new System.Drawing.Point(108, 0);
            this.panelTemplateSelection.TabIndex = 1;
            // 
            // labelQueryType
            // 
            this.labelQueryType.AutoSize = true;
            this.labelQueryType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelQueryType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelQueryType.ForeColor = System.Drawing.Color.Green;
            this.labelQueryType.Location = new System.Drawing.Point(3, 6);
            this.labelQueryType.Name = "labelQueryType";
            this.labelQueryType.Size = new System.Drawing.Size(102, 13);
            this.labelQueryType.TabIndex = 0;
            this.labelQueryType.Text = "Has Been Sent With";
            // 
            // EmailedValueEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelQueryType);
            this.Name = "EmailedValueEditor";
            this.Size = new System.Drawing.Size(490, 26);
            this.Load += new System.EventHandler(this.OnLoad);
            this.Controls.SetChildIndex(this.labelQueryType, 0);
            this.Controls.SetChildIndex(this.panelTemplateSelection, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChoiceLabel labelQueryType;
    }
}
