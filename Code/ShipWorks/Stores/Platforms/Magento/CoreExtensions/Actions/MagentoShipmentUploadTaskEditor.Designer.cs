namespace ShipWorks.Stores.Platforms.Magento.CoreExtensions.Actions
{
    partial class MagentoShipmentUploadTaskEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tokenBox = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelComment = new System.Windows.Forms.Label();
            this.sendEmailCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(29, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(395, 30);
            this.label1.TabIndex = 3;
            this.label1.Text = "The online order will be completed and include tracking information of the most r" +
                "ecently processed shipment.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox1.Location = new System.Drawing.Point(9, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // tokenBox
            // 
            this.tokenBox.Location = new System.Drawing.Point(64, 37);
            this.tokenBox.MaxLength = 32767;
            this.tokenBox.Name = "tokenBox";
            this.tokenBox.Size = new System.Drawing.Size(347, 21);
            this.tokenBox.TabIndex = 9;
            // 
            // labelComment
            // 
            this.labelComment.AutoSize = true;
            this.labelComment.Location = new System.Drawing.Point(5, 40);
            this.labelComment.Name = "labelComment";
            this.labelComment.Size = new System.Drawing.Size(56, 13);
            this.labelComment.TabIndex = 8;
            this.labelComment.Text = "Comment:";
            // 
            // sendEmailCheckBox
            // 
            this.sendEmailCheckBox.AutoSize = true;
            this.sendEmailCheckBox.Location = new System.Drawing.Point(64, 64);
            this.sendEmailCheckBox.Name = "sendEmailCheckBox";
            this.sendEmailCheckBox.Size = new System.Drawing.Size(198, 17);
            this.sendEmailCheckBox.TabIndex = 10;
            this.sendEmailCheckBox.Text = "Send customer email from Magento.";
            this.sendEmailCheckBox.UseVisualStyleBackColor = true;
            this.sendEmailCheckBox.CheckedChanged += new System.EventHandler(this.OnSendEmailChecked);
            // 
            // MagentoShipmentUploadTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sendEmailCheckBox);
            this.Controls.Add(this.tokenBox);
            this.Controls.Add(this.labelComment);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MagentoShipmentUploadTaskEditor";
            this.Size = new System.Drawing.Size(427, 87);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox tokenBox;
        private System.Windows.Forms.Label labelComment;
        private System.Windows.Forms.CheckBox sendEmailCheckBox;

    }
}
