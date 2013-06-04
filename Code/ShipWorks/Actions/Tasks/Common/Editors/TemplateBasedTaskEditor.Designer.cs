namespace ShipWorks.Actions.Tasks.Common.Editors
{
    partial class TemplateBasedTaskEditor
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
            this.labelTemplate = new System.Windows.Forms.Label();
            this.templateCombo = new ShipWorks.Templates.Controls.TemplateComboBox();
            this.SuspendLayout();
            // 
            // labelTemplate
            // 
            this.labelTemplate.AutoSize = true;
            this.labelTemplate.Location = new System.Drawing.Point(3, 3);
            this.labelTemplate.Name = "labelTemplate";
            this.labelTemplate.Size = new System.Drawing.Size(81, 13);
            this.labelTemplate.TabIndex = 0;
            this.labelTemplate.Text = "With template: ";
            // 
            // templateCombo
            // 
            this.templateCombo.DropDownHeight = 300;
            this.templateCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.templateCombo.FormattingEnabled = true;
            this.templateCombo.IntegralHeight = false;
            this.templateCombo.Location = new System.Drawing.Point(81, 0);
            this.templateCombo.Name = "templateCombo";
            this.templateCombo.Size = new System.Drawing.Size(250, 21);
            this.templateCombo.TabIndex = 1;
            // 
            // TemplateBasedTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.templateCombo);
            this.Controls.Add(this.labelTemplate);
            this.Name = "TemplateBasedTaskEditor";
            this.Size = new System.Drawing.Size(343, 26);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label labelTemplate;
        protected ShipWorks.Templates.Controls.TemplateComboBox templateCombo;

    }
}
