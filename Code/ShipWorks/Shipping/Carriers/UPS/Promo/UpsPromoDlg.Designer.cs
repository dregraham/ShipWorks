namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    partial class UpsPromoDlg
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
            this.remindMe = new System.Windows.Forms.Button();
            this.enroll = new System.Windows.Forms.Button();
            this.decline = new System.Windows.Forms.Button();
            this.acceptTerms = new System.Windows.Forms.RadioButton();
            this.declineTerms = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.promoDescription = new System.Windows.Forms.Label();
            this.termsLink = new ShipWorks.UI.Controls.LinkControl();
            this.SuspendLayout();
            // 
            // remindMe
            // 
            this.remindMe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.remindMe.Location = new System.Drawing.Point(272, 138);
            this.remindMe.Name = "remindMe";
            this.remindMe.Size = new System.Drawing.Size(75, 23);
            this.remindMe.TabIndex = 2;
            this.remindMe.Text = "Remind Me";
            this.remindMe.UseVisualStyleBackColor = true;
            this.remindMe.Click += new System.EventHandler(this.OnRemindMeClick);
            // 
            // enroll
            // 
            this.enroll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.enroll.Enabled = false;
            this.enroll.Location = new System.Drawing.Point(353, 138);
            this.enroll.Name = "enroll";
            this.enroll.Size = new System.Drawing.Size(75, 23);
            this.enroll.TabIndex = 3;
            this.enroll.Text = "Activate";
            this.enroll.UseVisualStyleBackColor = true;
            this.enroll.Click += new System.EventHandler(this.OnEnrollClick);
            // 
            // decline
            // 
            this.decline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.decline.Location = new System.Drawing.Point(191, 138);
            this.decline.Name = "decline";
            this.decline.Size = new System.Drawing.Size(75, 23);
            this.decline.TabIndex = 4;
            this.decline.Text = "No Thanks";
            this.decline.UseVisualStyleBackColor = true;
            this.decline.Click += new System.EventHandler(this.OnDeclineClick);
            // 
            // acceptTerms
            // 
            this.acceptTerms.AutoSize = true;
            this.acceptTerms.Location = new System.Drawing.Point(15, 83);
            this.acceptTerms.Name = "acceptTerms";
            this.acceptTerms.Size = new System.Drawing.Size(206, 17);
            this.acceptTerms.TabIndex = 6;
            this.acceptTerms.Text = "Yes, I accept the terms and conditions";
            this.acceptTerms.UseVisualStyleBackColor = true;
            this.acceptTerms.CheckedChanged += new System.EventHandler(this.OnAcceptTermsChanged);
            // 
            // declineTerms
            // 
            this.declineTerms.AutoSize = true;
            this.declineTerms.Checked = true;
            this.declineTerms.Location = new System.Drawing.Point(15, 106);
            this.declineTerms.Name = "declineTerms";
            this.declineTerms.Size = new System.Drawing.Size(235, 17);
            this.declineTerms.TabIndex = 7;
            this.declineTerms.TabStop = true;
            this.declineTerms.Text = "No, I do not accept the terms and conditions";
            this.declineTerms.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(352, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "You are eligible for a ShipWorks exclusive promotional price!";
            // 
            // promoDescription
            // 
            this.promoDescription.AutoSize = true;
            this.promoDescription.Location = new System.Drawing.Point(12, 31);
            this.promoDescription.MaximumSize = new System.Drawing.Size(400, 0);
            this.promoDescription.Name = "promoDescription";
            this.promoDescription.Size = new System.Drawing.Size(89, 13);
            this.promoDescription.TabIndex = 9;
            this.promoDescription.Text = "promoDescription";
            // 
            // termsLink
            // 
            this.termsLink.AutoSize = true;
            this.termsLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.termsLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.termsLink.ForeColor = System.Drawing.Color.Blue;
            this.termsLink.Location = new System.Drawing.Point(13, 67);
            this.termsLink.Name = "termsLink";
            this.termsLink.Size = new System.Drawing.Size(108, 13);
            this.termsLink.TabIndex = 5;
            this.termsLink.Text = "Terms and conditions";
            this.termsLink.Click += new System.EventHandler(this.OnTermsClick);
            // 
            // UpsPromoDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 169);
            this.ControlBox = false;
            this.Controls.Add(this.promoDescription);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.declineTerms);
            this.Controls.Add(this.acceptTerms);
            this.Controls.Add(this.termsLink);
            this.Controls.Add(this.decline);
            this.Controls.Add(this.enroll);
            this.Controls.Add(this.remindMe);
            this.Name = "UpsPromoDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Activate UPS Promo";
            this.Shown += new System.EventHandler(this.OnShow);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button remindMe;
        private System.Windows.Forms.Button enroll;
        private System.Windows.Forms.Button decline;
        private UI.Controls.LinkControl termsLink;
        private System.Windows.Forms.RadioButton acceptTerms;
        private System.Windows.Forms.RadioButton declineTerms;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label promoDescription;
    }
}