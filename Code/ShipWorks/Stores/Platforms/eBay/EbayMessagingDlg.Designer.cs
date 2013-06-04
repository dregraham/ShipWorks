namespace ShipWorks.Stores.Platforms.Ebay
{
    partial class EbayMessagingDlg
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.messageTypeComboBox = new System.Windows.Forms.ComboBox();
            this.copyMeCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.subjectLabel = new System.Windows.Forms.Label();
            this.introLabel = new System.Windows.Forms.Label();
            this.iconPictureBox = new System.Windows.Forms.PictureBox();
            this.sendButon = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.itemSelectionControl = new ShipWorks.Stores.Platforms.Ebay.EbayItemSelectionControl();
            this.subjectTextBox = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.messageTextBox = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            ((System.ComponentModel.ISupportInitialize) (this.iconPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "&Type:";
            // 
            // messageTypeComboBox
            // 
            this.messageTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.messageTypeComboBox.FormattingEnabled = true;
            this.messageTypeComboBox.Location = new System.Drawing.Point(96, 88);
            this.messageTypeComboBox.Name = "messageTypeComboBox";
            this.messageTypeComboBox.Size = new System.Drawing.Size(302, 21);
            this.messageTypeComboBox.TabIndex = 0;
            // 
            // copyMeCheckBox
            // 
            this.copyMeCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copyMeCheckBox.AutoSize = true;
            this.copyMeCheckBox.Location = new System.Drawing.Point(96, 285);
            this.copyMeCheckBox.Name = "copyMeCheckBox";
            this.copyMeCheckBox.Size = new System.Drawing.Size(117, 17);
            this.copyMeCheckBox.TabIndex = 3;
            this.copyMeCheckBox.Text = "&Send a Copy to Me";
            this.copyMeCheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 150);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "&Message:";
            // 
            // subjectLabel
            // 
            this.subjectLabel.AutoSize = true;
            this.subjectLabel.Location = new System.Drawing.Point(33, 123);
            this.subjectLabel.Name = "subjectLabel";
            this.subjectLabel.Size = new System.Drawing.Size(47, 13);
            this.subjectLabel.TabIndex = 8;
            this.subjectLabel.Text = "&Subject:";
            // 
            // introLabel
            // 
            this.introLabel.Location = new System.Drawing.Point(59, 14);
            this.introLabel.Name = "introLabel";
            this.introLabel.Size = new System.Drawing.Size(321, 34);
            this.introLabel.TabIndex = 15;
            this.introLabel.Text = "eBay Messages are sent to the buyer\'s eBay account and appear in their My eBay In" +
                "box.";
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.Image = global::ShipWorks.Properties.Resources.note2_32;
            this.iconPictureBox.Location = new System.Drawing.Point(12, 12);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(32, 32);
            this.iconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.iconPictureBox.TabIndex = 16;
            this.iconPictureBox.TabStop = false;
            // 
            // sendButon
            // 
            this.sendButon.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sendButon.Location = new System.Drawing.Point(242, 310);
            this.sendButon.Name = "sendButon";
            this.sendButon.Size = new System.Drawing.Size(75, 23);
            this.sendButon.TabIndex = 4;
            this.sendButon.Text = "&Send";
            this.sendButon.UseVisualStyleBackColor = true;
            this.sendButon.Click += new System.EventHandler(this.OnSendClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(323, 310);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Related to:";
            // 
            // itemSelectionControl
            // 
            this.itemSelectionControl.Location = new System.Drawing.Point(96, 61);
            this.itemSelectionControl.Name = "itemSelectionControl";
            this.itemSelectionControl.Size = new System.Drawing.Size(302, 23);
            this.itemSelectionControl.TabIndex = 17;
            // 
            // subjectTextBox
            // 
            this.subjectTextBox.Location = new System.Drawing.Point(96, 117);
            this.subjectTextBox.MaxLength = 32767;
            this.subjectTextBox.Name = "subjectTextBox";
            this.subjectTextBox.Size = new System.Drawing.Size(302, 21);
            this.subjectTextBox.TabIndex = 1;
            // 
            // messageTextBox
            // 
            this.messageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.messageTextBox.Location = new System.Drawing.Point(96, 144);
            this.messageTextBox.MaxLength = 32767;
            this.messageTextBox.Multiline = true;
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(302, 135);
            this.messageTextBox.TabIndex = 2;
            // 
            // EbayMessagingDlg
            // 
            this.AcceptButton = this.sendButon;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(410, 345);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.itemSelectionControl);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.sendButon);
            this.Controls.Add(this.iconPictureBox);
            this.Controls.Add(this.introLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.messageTypeComboBox);
            this.Controls.Add(this.subjectTextBox);
            this.Controls.Add(this.copyMeCheckBox);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.subjectLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EbayMessagingDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Send eBay Message";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize) (this.iconPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox messageTypeComboBox;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox subjectTextBox;
        private System.Windows.Forms.CheckBox copyMeCheckBox;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox messageTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label subjectLabel;
        private System.Windows.Forms.Label introLabel;
        private System.Windows.Forms.PictureBox iconPictureBox;
        private System.Windows.Forms.Button sendButon;
        private System.Windows.Forms.Button cancelButton;
        private EbayItemSelectionControl itemSelectionControl;
        private System.Windows.Forms.Label label3;
    }
}