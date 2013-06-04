namespace ShipWorks.Stores.Platforms.GenericFile.Sources.Email
{
    partial class GenericFileSourceEmailControl
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
            this.incomingFolderBrowse = new System.Windows.Forms.Button();
            this.incomingFolder = new System.Windows.Forms.TextBox();
            this.labelIncomingFolder = new System.Windows.Forms.Label();
            this.onlyImportUnread = new System.Windows.Forms.CheckBox();
            this.accountNew = new System.Windows.Forms.Button();
            this.accountEdit = new System.Windows.Forms.Button();
            this.emailAccountDescription = new System.Windows.Forms.TextBox();
            this.labelAccount = new System.Windows.Forms.Label();
            this.pictureEmail = new System.Windows.Forms.PictureBox();
            this.subjectCantMatchPattern = new System.Windows.Forms.TextBox();
            this.subjectMustMatch = new System.Windows.Forms.CheckBox();
            this.subjectCantMatch = new System.Windows.Forms.CheckBox();
            this.subjectMustMatchPattern = new System.Windows.Forms.TextBox();
            this.actionsControl = new ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceActionsSetupControl();
            this.infotipWeightFormat = new ShipWorks.UI.Controls.InfoTip();
            this.infoTip1 = new ShipWorks.UI.Controls.InfoTip();
            ((System.ComponentModel.ISupportInitialize) (this.pictureEmail)).BeginInit();
            this.SuspendLayout();
            // 
            // incomingFolderBrowse
            // 
            this.incomingFolderBrowse.Location = new System.Drawing.Point(351, 31);
            this.incomingFolderBrowse.Name = "incomingFolderBrowse";
            this.incomingFolderBrowse.Size = new System.Drawing.Size(75, 23);
            this.incomingFolderBrowse.TabIndex = 106;
            this.incomingFolderBrowse.Text = "Browse...";
            this.incomingFolderBrowse.UseVisualStyleBackColor = true;
            this.incomingFolderBrowse.Click += new System.EventHandler(this.OnBrowseIncomingFolder);
            // 
            // incomingFolder
            // 
            this.incomingFolder.Location = new System.Drawing.Point(145, 33);
            this.incomingFolder.Name = "incomingFolder";
            this.incomingFolder.ReadOnly = true;
            this.incomingFolder.Size = new System.Drawing.Size(202, 21);
            this.incomingFolder.TabIndex = 105;
            // 
            // labelIncomingFolder
            // 
            this.labelIncomingFolder.AutoSize = true;
            this.labelIncomingFolder.Location = new System.Drawing.Point(84, 36);
            this.labelIncomingFolder.Name = "labelIncomingFolder";
            this.labelIncomingFolder.Size = new System.Drawing.Size(60, 13);
            this.labelIncomingFolder.TabIndex = 104;
            this.labelIncomingFolder.Text = "Mail folder:";
            // 
            // onlyImportUnread
            // 
            this.onlyImportUnread.AutoSize = true;
            this.onlyImportUnread.Location = new System.Drawing.Point(71, 66);
            this.onlyImportUnread.Name = "onlyImportUnread";
            this.onlyImportUnread.Size = new System.Drawing.Size(168, 17);
            this.onlyImportUnread.TabIndex = 103;
            this.onlyImportUnread.Text = "Only import unread messages";
            this.onlyImportUnread.UseVisualStyleBackColor = true;
            // 
            // accountNew
            // 
            this.accountNew.Location = new System.Drawing.Point(432, 4);
            this.accountNew.Name = "accountNew";
            this.accountNew.Size = new System.Drawing.Size(75, 23);
            this.accountNew.TabIndex = 101;
            this.accountNew.Text = "New...";
            this.accountNew.UseVisualStyleBackColor = true;
            this.accountNew.Click += new System.EventHandler(this.OnNewAccount);
            // 
            // accountEdit
            // 
            this.accountEdit.Location = new System.Drawing.Point(351, 4);
            this.accountEdit.Name = "accountEdit";
            this.accountEdit.Size = new System.Drawing.Size(75, 23);
            this.accountEdit.TabIndex = 100;
            this.accountEdit.Text = "Edit...";
            this.accountEdit.UseVisualStyleBackColor = true;
            this.accountEdit.Click += new System.EventHandler(this.OnEditAccount);
            // 
            // emailAccountDescription
            // 
            this.emailAccountDescription.Location = new System.Drawing.Point(145, 6);
            this.emailAccountDescription.Name = "emailAccountDescription";
            this.emailAccountDescription.ReadOnly = true;
            this.emailAccountDescription.Size = new System.Drawing.Size(202, 21);
            this.emailAccountDescription.TabIndex = 99;
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Location = new System.Drawing.Point(68, 9);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(76, 13);
            this.labelAccount.TabIndex = 98;
            this.labelAccount.Text = "Email account:";
            // 
            // pictureEmail
            // 
            this.pictureEmail.Image = global::ShipWorks.Properties.Resources.mailbox_empty;
            this.pictureEmail.Location = new System.Drawing.Point(23, 4);
            this.pictureEmail.Name = "pictureEmail";
            this.pictureEmail.Size = new System.Drawing.Size(32, 32);
            this.pictureEmail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureEmail.TabIndex = 89;
            this.pictureEmail.TabStop = false;
            // 
            // subjectCantMatchPattern
            // 
            this.subjectCantMatchPattern.Enabled = false;
            this.subjectCantMatchPattern.Location = new System.Drawing.Point(304, 117);
            this.subjectCantMatchPattern.Name = "subjectCantMatchPattern";
            this.subjectCantMatchPattern.Size = new System.Drawing.Size(154, 21);
            this.subjectCantMatchPattern.TabIndex = 93;
            // 
            // subjectMustMatch
            // 
            this.subjectMustMatch.AutoSize = true;
            this.subjectMustMatch.Location = new System.Drawing.Point(71, 93);
            this.subjectMustMatch.Name = "subjectMustMatch";
            this.subjectMustMatch.Size = new System.Drawing.Size(227, 17);
            this.subjectMustMatch.TabIndex = 90;
            this.subjectMustMatch.Text = "Only import mail with subjects that match:";
            this.subjectMustMatch.UseVisualStyleBackColor = true;
            this.subjectMustMatch.CheckedChanged += new System.EventHandler(this.OnCheckedChangedMustMatch);
            // 
            // subjectCantMatch
            // 
            this.subjectCantMatch.AutoSize = true;
            this.subjectCantMatch.Location = new System.Drawing.Point(71, 119);
            this.subjectCantMatch.Name = "subjectCantMatch";
            this.subjectCantMatch.Size = new System.Drawing.Size(204, 17);
            this.subjectCantMatch.TabIndex = 92;
            this.subjectCantMatch.Text = "Ignore mail with subjects that match:";
            this.subjectCantMatch.UseVisualStyleBackColor = true;
            this.subjectCantMatch.CheckedChanged += new System.EventHandler(this.OnCheckedChangedCantMatch);
            // 
            // subjectMustMatchPattern
            // 
            this.subjectMustMatchPattern.Enabled = false;
            this.subjectMustMatchPattern.Location = new System.Drawing.Point(304, 91);
            this.subjectMustMatchPattern.Name = "subjectMustMatchPattern";
            this.subjectMustMatchPattern.Size = new System.Drawing.Size(156, 21);
            this.subjectMustMatchPattern.TabIndex = 91;
            // 
            // actionsControl
            // 
            this.actionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.actionsControl.Location = new System.Drawing.Point(23, 146);
            this.actionsControl.Name = "actionsControl";
            this.actionsControl.Size = new System.Drawing.Size(490, 160);
            this.actionsControl.TabIndex = 107;
            this.actionsControl.BrowseForSuccessFolder += new ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceFolderBrowseEventHandler(this.OnBrowseForActionFolder);
            this.actionsControl.BrowseForErrorFolder += new ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceFolderBrowseEventHandler(this.OnBrowseForActionFolder);
            this.actionsControl.SizeChanged += new System.EventHandler(this.OnActionsSizeChanged);
            // 
            // infotipWeightFormat
            // 
            this.infotipWeightFormat.Caption = "This matching is done using Regular Expressions.";
            this.infotipWeightFormat.Location = new System.Drawing.Point(466, 96);
            this.infotipWeightFormat.Name = "infotipWeightFormat";
            this.infotipWeightFormat.Size = new System.Drawing.Size(12, 12);
            this.infotipWeightFormat.TabIndex = 114;
            this.infotipWeightFormat.Title = "Subject Matching";
            // 
            // infoTip1
            // 
            this.infoTip1.Caption = "This matching is done using Regular Expressions.";
            this.infoTip1.Location = new System.Drawing.Point(467, 121);
            this.infoTip1.Name = "infoTip1";
            this.infoTip1.Size = new System.Drawing.Size(12, 12);
            this.infoTip1.TabIndex = 115;
            this.infoTip1.Title = "Subject Matching";
            // 
            // GenericFileSourceEmailControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.infoTip1);
            this.Controls.Add(this.infotipWeightFormat);
            this.Controls.Add(this.actionsControl);
            this.Controls.Add(this.incomingFolderBrowse);
            this.Controls.Add(this.incomingFolder);
            this.Controls.Add(this.labelIncomingFolder);
            this.Controls.Add(this.onlyImportUnread);
            this.Controls.Add(this.accountNew);
            this.Controls.Add(this.accountEdit);
            this.Controls.Add(this.emailAccountDescription);
            this.Controls.Add(this.labelAccount);
            this.Controls.Add(this.pictureEmail);
            this.Controls.Add(this.subjectCantMatchPattern);
            this.Controls.Add(this.subjectMustMatch);
            this.Controls.Add(this.subjectCantMatch);
            this.Controls.Add(this.subjectMustMatchPattern);
            this.Name = "GenericFileSourceEmailControl";
            this.Size = new System.Drawing.Size(535, 319);
            ((System.ComponentModel.ISupportInitialize) (this.pictureEmail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button accountNew;
        private System.Windows.Forms.Button accountEdit;
        private System.Windows.Forms.TextBox emailAccountDescription;
        private System.Windows.Forms.Label labelAccount;
        private System.Windows.Forms.PictureBox pictureEmail;
        private System.Windows.Forms.TextBox subjectCantMatchPattern;
        private System.Windows.Forms.CheckBox subjectMustMatch;
        private System.Windows.Forms.CheckBox subjectCantMatch;
        private System.Windows.Forms.TextBox subjectMustMatchPattern;
        private System.Windows.Forms.CheckBox onlyImportUnread;
        private System.Windows.Forms.Label labelIncomingFolder;
        private System.Windows.Forms.TextBox incomingFolder;
        private System.Windows.Forms.Button incomingFolderBrowse;
        private GenericFileSourceActionsSetupControl actionsControl;
        private UI.Controls.InfoTip infotipWeightFormat;
        private UI.Controls.InfoTip infoTip1;
    }
}
