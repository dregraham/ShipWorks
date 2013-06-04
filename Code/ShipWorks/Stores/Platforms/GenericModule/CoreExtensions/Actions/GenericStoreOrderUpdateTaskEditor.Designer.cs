namespace ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Actions
{
    partial class GenericStoreOrderUpdateTaskEditor
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
            this.comboBoxStatus = new System.Windows.Forms.ComboBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.panelComments = new System.Windows.Forms.Panel();
            this.tokenBox = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelComment = new System.Windows.Forms.Label();
            this.panelComments.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxStatus
            // 
            this.comboBoxStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatus.FormattingEnabled = true;
            this.comboBoxStatus.Location = new System.Drawing.Point(61, 0);
            this.comboBoxStatus.Name = "comboBoxStatus";
            this.comboBoxStatus.Size = new System.Drawing.Size(196, 21);
            this.comboBoxStatus.TabIndex = 1;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(16, 3);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(42, 13);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "Status:";
            // 
            // panelComments
            // 
            this.panelComments.Controls.Add(this.tokenBox);
            this.panelComments.Controls.Add(this.labelComment);
            this.panelComments.Location = new System.Drawing.Point(0, 21);
            this.panelComments.Name = "panelComments";
            this.panelComments.Size = new System.Drawing.Size(327, 30);
            this.panelComments.TabIndex = 2;
            // 
            // tokenBox
            // 
            this.tokenBox.Location = new System.Drawing.Point(61, 4);
            this.tokenBox.Name = "tokenBox";
            this.tokenBox.Size = new System.Drawing.Size(249, 21);
            this.tokenBox.TabIndex = 1;
            // 
            // labelComment
            // 
            this.labelComment.AutoSize = true;
            this.labelComment.Location = new System.Drawing.Point(2, 7);
            this.labelComment.Name = "labelComment";
            this.labelComment.Size = new System.Drawing.Size(56, 13);
            this.labelComment.TabIndex = 0;
            this.labelComment.Text = "Comment:";
            // 
            // GenericStoreOrderUpdateTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxStatus);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.panelComments);
            this.Name = "GenericStoreOrderUpdateTaskEditor";
            this.Size = new System.Drawing.Size(327, 58);
            this.Load += new System.EventHandler(this.OnLoad);
            this.panelComments.ResumeLayout(false);
            this.panelComments.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxStatus;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Panel panelComments;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox tokenBox;
        private System.Windows.Forms.Label labelComment;
    }
}
