namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    partial class TemplateValueEditor
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
            this.templateComboBox = new ShipWorks.Templates.Controls.TemplateComboBox();
            this.deletedTemplates = new System.Windows.Forms.Label();
            this.panelTemplateSelection = new System.Windows.Forms.Panel();
            this.panelTemplateSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // templateComboBox
            // 
            this.templateComboBox.DropDownHeight = 350;
            this.templateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.templateComboBox.FormattingEnabled = true;
            this.templateComboBox.IntegralHeight = false;
            this.templateComboBox.Location = new System.Drawing.Point(3, 2);
            this.templateComboBox.Name = "templateComboBox";
            this.templateComboBox.Size = new System.Drawing.Size(267, 21);
            this.templateComboBox.TabIndex = 1;
            // 
            // deletedTemplates
            // 
            this.deletedTemplates.AutoSize = true;
            this.deletedTemplates.Cursor = System.Windows.Forms.Cursors.Hand;
            this.deletedTemplates.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.deletedTemplates.ForeColor = System.Drawing.Color.RoyalBlue;
            this.deletedTemplates.Location = new System.Drawing.Point(274, 6);
            this.deletedTemplates.Name = "deletedTemplates";
            this.deletedTemplates.Size = new System.Drawing.Size(96, 13);
            this.deletedTemplates.TabIndex = 2;
            this.deletedTemplates.Text = "Deleted Templates";
            this.deletedTemplates.Click += new System.EventHandler(this.OnClickDeletedTemplates);
            // 
            // panelTemplateSelection
            // 
            this.panelTemplateSelection.Controls.Add(this.templateComboBox);
            this.panelTemplateSelection.Controls.Add(this.deletedTemplates);
            this.panelTemplateSelection.Location = new System.Drawing.Point(0, 0);
            this.panelTemplateSelection.Name = "panelTemplateSelection";
            this.panelTemplateSelection.Size = new System.Drawing.Size(376, 26);
            this.panelTemplateSelection.TabIndex = 0;
            // 
            // TemplateValueEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelTemplateSelection);
            this.Name = "TemplateValueEditor";
            this.Size = new System.Drawing.Size(381, 26);
            this.Load += new System.EventHandler(this.OnLoad);
            this.panelTemplateSelection.ResumeLayout(false);
            this.panelTemplateSelection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.Templates.Controls.TemplateComboBox templateComboBox;
        private System.Windows.Forms.Label deletedTemplates;
        protected System.Windows.Forms.Panel panelTemplateSelection;
    }
}
