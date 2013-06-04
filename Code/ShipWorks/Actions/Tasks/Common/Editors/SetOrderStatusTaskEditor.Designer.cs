namespace ShipWorks.Actions.Tasks.Common.Editors
{
    partial class SetOrderStatusTaskEditor
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
            this.label = new System.Windows.Forms.Label();
            this.tokenBox = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(3, 3);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(42, 13);
            this.label.TabIndex = 0;
            this.label.Text = "Status:";
            // 
            // tokenBox
            // 
            this.tokenBox.Location = new System.Drawing.Point(47, 0);
            this.tokenBox.Name = "tokenBox";
            this.tokenBox.Size = new System.Drawing.Size(241, 21);
            this.tokenBox.TabIndex = 1;
            this.tokenBox.TokenUsage = ShipWorks.Templates.Tokens.TokenUsage.OrderStatus;
            // 
            // SetOrderStatusTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tokenBox);
            this.Controls.Add(this.label);
            this.Name = "SetOrderStatusTaskEditor";
            this.Size = new System.Drawing.Size(299, 26);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox tokenBox;
    }
}
