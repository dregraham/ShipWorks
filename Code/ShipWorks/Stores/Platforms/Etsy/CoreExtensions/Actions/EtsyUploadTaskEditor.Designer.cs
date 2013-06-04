namespace ShipWorks.Stores.Platforms.Etsy.CoreExtensions.Actions
{
    partial class EtsyUploadTaskEditor
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
            this.tokenBox = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.paidCheckBox = new System.Windows.Forms.CheckBox();
            this.shippedCheckBox = new System.Windows.Forms.CheckBox();
            this.setComment = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tokenBox
            // 
            this.tokenBox.Location = new System.Drawing.Point(121, 24);
            this.tokenBox.MaxLength = 32767;
            this.tokenBox.Name = "tokenBox";
            this.tokenBox.Size = new System.Drawing.Size(298, 25);
            this.tokenBox.TabIndex = 0;
            // 
            // paidCheckBox
            // 
            this.paidCheckBox.AutoSize = true;
            this.paidCheckBox.Location = new System.Drawing.Point(3, 46);
            this.paidCheckBox.Name = "paidCheckBox";
            this.paidCheckBox.Size = new System.Drawing.Size(86, 17);
            this.paidCheckBox.TabIndex = 3;
            this.paidCheckBox.Text = "Mark as &Paid";
            this.paidCheckBox.UseVisualStyleBackColor = true;
            this.paidCheckBox.CheckedChanged += new System.EventHandler(this.OnPaidCheckBoxCheckedChanged);
            // 
            // shippedCheckBox
            // 
            this.shippedCheckBox.AutoSize = true;
            this.shippedCheckBox.Location = new System.Drawing.Point(3, 3);
            this.shippedCheckBox.Name = "shippedCheckBox";
            this.shippedCheckBox.Size = new System.Drawing.Size(104, 17);
            this.shippedCheckBox.TabIndex = 2;
            this.shippedCheckBox.Text = "Mark as &Shipped";
            this.shippedCheckBox.UseVisualStyleBackColor = true;
            this.shippedCheckBox.CheckedChanged += new System.EventHandler(this.OnShippedCheckBoxCheckedChanged);
            // 
            // setComment
            // 
            this.setComment.AutoSize = true;
            this.setComment.Location = new System.Drawing.Point(23, 26);
            this.setComment.Margin = new System.Windows.Forms.Padding(0);
            this.setComment.Name = "setComment";
            this.setComment.Size = new System.Drawing.Size(98, 17);
            this.setComment.TabIndex = 4;
            this.setComment.Text = "With comment:";
            this.setComment.UseVisualStyleBackColor = true;
            this.setComment.CheckedChanged += new System.EventHandler(this.OnWithCommentCheckedChanged);
            // 
            // EtsyUploadTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tokenBox);
            this.Controls.Add(this.setComment);
            this.Controls.Add(this.paidCheckBox);
            this.Controls.Add(this.shippedCheckBox);
            this.Name = "EtsyUploadTaskEditor";
            this.Size = new System.Drawing.Size(435, 69);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Templates.Tokens.TemplateTokenTextBox tokenBox;
        private System.Windows.Forms.CheckBox paidCheckBox;
        private System.Windows.Forms.CheckBox shippedCheckBox;
        private System.Windows.Forms.CheckBox setComment;



    }
}
