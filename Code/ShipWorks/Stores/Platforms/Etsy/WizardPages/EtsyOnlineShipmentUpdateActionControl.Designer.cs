namespace ShipWorks.Stores.Platforms.Etsy.WizardPages
{
    partial class EtsyOnlineShipmentUpdateActionControl
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
            this.withComment = new System.Windows.Forms.CheckBox();
            this.paidCheckBox = new System.Windows.Forms.CheckBox();
            this.shippedCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tokenBox
            // 
            this.tokenBox.Location = new System.Drawing.Point(129, 27);
            this.tokenBox.MaxLength = 32767;
            this.tokenBox.Name = "tokenBox";
            this.tokenBox.Size = new System.Drawing.Size(329, 24);
            this.tokenBox.TabIndex = 5;
            this.tokenBox.Text = "{//ServiceUsed} - {//TrackingNumber}";
            // 
            // withComment
            // 
            this.withComment.AutoSize = true;
            this.withComment.Checked = true;
            this.withComment.CheckState = System.Windows.Forms.CheckState.Checked;
            this.withComment.Location = new System.Drawing.Point(30, 30);
            this.withComment.Margin = new System.Windows.Forms.Padding(0);
            this.withComment.Name = "withComment";
            this.withComment.Size = new System.Drawing.Size(98, 17);
            this.withComment.TabIndex = 8;
            this.withComment.Text = "With comment:";
            this.withComment.UseVisualStyleBackColor = true;
            this.withComment.CheckedChanged += new System.EventHandler(this.OnWithCommentCheckedChanged);
            // 
            // paidCheckBox
            // 
            this.paidCheckBox.AutoSize = true;
            this.paidCheckBox.Checked = true;
            this.paidCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.paidCheckBox.Location = new System.Drawing.Point(10, 50);
            this.paidCheckBox.Name = "paidCheckBox";
            this.paidCheckBox.Size = new System.Drawing.Size(165, 17);
            this.paidCheckBox.TabIndex = 7;
            this.paidCheckBox.Text = "Mark the online order as &paid";
            this.paidCheckBox.UseVisualStyleBackColor = true;
            // 
            // shippedCheckBox
            // 
            this.shippedCheckBox.AutoSize = true;
            this.shippedCheckBox.Checked = true;
            this.shippedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shippedCheckBox.Location = new System.Drawing.Point(10, 7);
            this.shippedCheckBox.Name = "shippedCheckBox";
            this.shippedCheckBox.Size = new System.Drawing.Size(182, 17);
            this.shippedCheckBox.TabIndex = 6;
            this.shippedCheckBox.Text = "Mark the online order as &shipped";
            this.shippedCheckBox.UseVisualStyleBackColor = true;
            this.shippedCheckBox.CheckedChanged += new System.EventHandler(this.OnShippedCheckBoxCheckedChanged);
            // 
            // EtsyOnlineShipmentUpdateActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tokenBox);
            this.Controls.Add(this.withComment);
            this.Controls.Add(this.paidCheckBox);
            this.Controls.Add(this.shippedCheckBox);
            this.Name = "EtsyOnlineShipmentUpdateActionControl";
            this.Size = new System.Drawing.Size(474, 73);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Templates.Tokens.TemplateTokenTextBox tokenBox;
        private System.Windows.Forms.CheckBox withComment;
        private System.Windows.Forms.CheckBox paidCheckBox;
        private System.Windows.Forms.CheckBox shippedCheckBox;


    }
}
