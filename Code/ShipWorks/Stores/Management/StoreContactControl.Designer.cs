namespace ShipWorks.Stores.Management
{
    partial class StoreContactControl
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
            this.components = new System.ComponentModel.Container();
            this.labelContactInfo = new System.Windows.Forms.Label();
            this.labelWebsite = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.labelPhone = new System.Windows.Forms.Label();
            this.labelSocialMedia = new System.Windows.Forms.Label();
            this.labelTwitter = new System.Windows.Forms.Label();
            this.labelFacebook = new System.Windows.Forms.Label();
            this.labelYourLogo = new System.Windows.Forms.Label();
            this.linkSelectLogo = new ShipWorks.UI.Controls.LinkControl();
            this.twitter = new ShipWorks.UI.Controls.PromptTextBox();
            this.facebook = new ShipWorks.UI.Controls.PromptTextBox();
            this.website = new System.Windows.Forms.TextBox();
            this.phone = new System.Windows.Forms.TextBox();
            this.email = new System.Windows.Forms.TextBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.pictureBoxFacebook = new System.Windows.Forms.PictureBox();
            this.pictureBoxTwitter = new System.Windows.Forms.PictureBox();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFacebook)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTwitter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // labelContactInfo
            // 
            this.labelContactInfo.AutoSize = true;
            this.labelContactInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelContactInfo.Location = new System.Drawing.Point(0, 3);
            this.labelContactInfo.Name = "labelContactInfo";
            this.labelContactInfo.Size = new System.Drawing.Size(122, 13);
            this.labelContactInfo.TabIndex = 73;
            this.labelContactInfo.Text = "Contact Information";
            // 
            // labelWebsite
            // 
            this.labelWebsite.AutoSize = true;
            this.labelWebsite.Location = new System.Drawing.Point(23, 27);
            this.labelWebsite.Name = "labelWebsite";
            this.labelWebsite.Size = new System.Drawing.Size(50, 13);
            this.labelWebsite.TabIndex = 71;
            this.labelWebsite.Text = "Website:";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(37, 54);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(35, 13);
            this.labelEmail.TabIndex = 70;
            this.labelEmail.Text = "Email:";
            // 
            // labelPhone
            // 
            this.labelPhone.AutoSize = true;
            this.labelPhone.Location = new System.Drawing.Point(31, 81);
            this.labelPhone.Name = "labelPhone";
            this.labelPhone.Size = new System.Drawing.Size(41, 13);
            this.labelPhone.TabIndex = 68;
            this.labelPhone.Text = "Phone:";
            // 
            // labelSocialMedia
            // 
            this.labelSocialMedia.AutoSize = true;
            this.labelSocialMedia.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSocialMedia.Location = new System.Drawing.Point(0, 111);
            this.labelSocialMedia.Name = "labelSocialMedia";
            this.labelSocialMedia.Size = new System.Drawing.Size(77, 13);
            this.labelSocialMedia.TabIndex = 74;
            this.labelSocialMedia.Text = "Social Media";
            this.labelSocialMedia.Visible = false;
            // 
            // labelTwitter
            // 
            this.labelTwitter.AutoSize = true;
            this.labelTwitter.Location = new System.Drawing.Point(27, 164);
            this.labelTwitter.Name = "labelTwitter";
            this.labelTwitter.Size = new System.Drawing.Size(45, 13);
            this.labelTwitter.TabIndex = 80;
            this.labelTwitter.Text = "Twitter:";
            this.labelTwitter.Visible = false;
            // 
            // labelFacebook
            // 
            this.labelFacebook.AutoSize = true;
            this.labelFacebook.Location = new System.Drawing.Point(15, 137);
            this.labelFacebook.Name = "labelFacebook";
            this.labelFacebook.Size = new System.Drawing.Size(57, 13);
            this.labelFacebook.TabIndex = 78;
            this.labelFacebook.Text = "Facebook:";
            this.labelFacebook.Visible = false;
            // 
            // labelYourLogo
            // 
            this.labelYourLogo.AutoSize = true;
            this.labelYourLogo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelYourLogo.Location = new System.Drawing.Point(10, 193);
            this.labelYourLogo.Name = "labelYourLogo";
            this.labelYourLogo.Size = new System.Drawing.Size(63, 13);
            this.labelYourLogo.TabIndex = 81;
            this.labelYourLogo.Text = "Your Logo";
            this.labelYourLogo.Visible = false;
            // 
            // linkSelectLogo
            // 
            this.linkSelectLogo.AutoSize = true;
            this.linkSelectLogo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkSelectLogo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkSelectLogo.ForeColor = System.Drawing.Color.Blue;
            this.linkSelectLogo.Location = new System.Drawing.Point(31, 214);
            this.linkSelectLogo.Name = "linkSelectLogo";
            this.linkSelectLogo.Size = new System.Drawing.Size(48, 13);
            this.linkSelectLogo.TabIndex = 85;
            this.linkSelectLogo.Text = "Select...";
            this.linkSelectLogo.Visible = false;
            this.linkSelectLogo.Click += new System.EventHandler(this.OnSelectLogo);
            // 
            // twitter
            // 
            this.twitter.Location = new System.Drawing.Point(100, 161);
            this.twitter.Name = "twitter";
            this.twitter.PromptColor = System.Drawing.Color.DarkGray;
            this.twitter.PromptText = "@YourAccount";
            this.twitter.Size = new System.Drawing.Size(247, 21);
            this.twitter.TabIndex = 5;
            this.twitter.Visible = false;
            // 
            // facebook
            // 
            this.facebook.Location = new System.Drawing.Point(100, 134);
            this.facebook.Name = "facebook";
            this.facebook.PromptColor = System.Drawing.Color.DarkGray;
            this.facebook.PromptText = "www.facebook.com/YourPage";
            this.facebook.Size = new System.Drawing.Size(247, 21);
            this.facebook.TabIndex = 4;
            this.facebook.Visible = false;
            // 
            // website
            // 
            this.website.Location = new System.Drawing.Point(78, 24);
            this.fieldLengthProvider.SetMaxLengthSource(this.website, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonWebsite);
            this.website.Name = "website";
            this.website.Size = new System.Drawing.Size(271, 21);
            this.website.TabIndex = 1;
            // 
            // phone
            // 
            this.phone.Location = new System.Drawing.Point(78, 78);
            this.fieldLengthProvider.SetMaxLengthSource(this.phone, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonPhone);
            this.phone.Name = "phone";
            this.phone.Size = new System.Drawing.Size(271, 21);
            this.phone.TabIndex = 3;
            // 
            // email
            // 
            this.email.Location = new System.Drawing.Point(78, 51);
            this.fieldLengthProvider.SetMaxLengthSource(this.email, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonEmail);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(271, 21);
            this.email.TabIndex = 2;
            // 
            // pictureBoxFacebook
            // 
            this.pictureBoxFacebook.Image = global::ShipWorks.Properties.Resources.social_facebook;
            this.pictureBoxFacebook.Location = new System.Drawing.Point(78, 137);
            this.pictureBoxFacebook.Name = "pictureBoxFacebook";
            this.pictureBoxFacebook.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxFacebook.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxFacebook.TabIndex = 87;
            this.pictureBoxFacebook.TabStop = false;
            this.pictureBoxFacebook.Visible = false;
            // 
            // pictureBoxTwitter
            // 
            this.pictureBoxTwitter.Image = global::ShipWorks.Properties.Resources.social_twitter;
            this.pictureBoxTwitter.Location = new System.Drawing.Point(78, 164);
            this.pictureBoxTwitter.Name = "pictureBoxTwitter";
            this.pictureBoxTwitter.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxTwitter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxTwitter.TabIndex = 86;
            this.pictureBoxTwitter.TabStop = false;
            this.pictureBoxTwitter.Visible = false;
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Image = global::ShipWorks.Properties.Resources.dashed_outline;
            this.pictureBoxLogo.Location = new System.Drawing.Point(34, 232);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(84, 48);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 82;
            this.pictureBoxLogo.TabStop = false;
            this.pictureBoxLogo.Visible = false;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Filter = "All Pictures Files|*.*";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OnLogoSelectOK);
            // 
            // StoreContactControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBoxFacebook);
            this.Controls.Add(this.pictureBoxTwitter);
            this.Controls.Add(this.linkSelectLogo);
            this.Controls.Add(this.twitter);
            this.Controls.Add(this.facebook);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.labelYourLogo);
            this.Controls.Add(this.labelTwitter);
            this.Controls.Add(this.labelFacebook);
            this.Controls.Add(this.labelSocialMedia);
            this.Controls.Add(this.labelContactInfo);
            this.Controls.Add(this.website);
            this.Controls.Add(this.labelWebsite);
            this.Controls.Add(this.phone);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.email);
            this.Controls.Add(this.labelPhone);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "StoreContactControl";
            this.Size = new System.Drawing.Size(360, 300);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFacebook)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTwitter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label labelContactInfo;
        protected System.Windows.Forms.TextBox website;
        protected System.Windows.Forms.Label labelWebsite;
        protected System.Windows.Forms.TextBox phone;
        protected System.Windows.Forms.Label labelEmail;
        protected System.Windows.Forms.TextBox email;
        protected System.Windows.Forms.Label labelPhone;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        protected System.Windows.Forms.Label labelSocialMedia;
        protected System.Windows.Forms.Label labelTwitter;
        protected System.Windows.Forms.Label labelFacebook;
        protected System.Windows.Forms.Label labelYourLogo;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private UI.Controls.PromptTextBox facebook;
        private UI.Controls.PromptTextBox twitter;
        private UI.Controls.LinkControl linkSelectLogo;
        private System.Windows.Forms.PictureBox pictureBoxTwitter;
        private System.Windows.Forms.PictureBox pictureBoxFacebook;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}
