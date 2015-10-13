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
            this.termsLink = new ShipWorks.UI.Controls.LinkControl();
            this.SuspendLayout();
            // 
            // remindMe
            // 
            this.remindMe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.remindMe.Location = new System.Drawing.Point(272, 447);
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
            this.enroll.Location = new System.Drawing.Point(353, 447);
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
            this.decline.Location = new System.Drawing.Point(191, 447);
            this.decline.Name = "decline";
            this.decline.Size = new System.Drawing.Size(75, 23);
            this.decline.TabIndex = 4;
            this.decline.Text = "No Thanks";
            this.decline.UseVisualStyleBackColor = true;
            this.decline.Click += new System.EventHandler(this.OnDclineClick);
            // 
            // acceptTerms
            // 
            this.acceptTerms.AutoSize = true;
            this.acceptTerms.Location = new System.Drawing.Point(15, 371);
            this.acceptTerms.Name = "acceptTerms";
            this.acceptTerms.Size = new System.Drawing.Size(211, 17);
            this.acceptTerms.TabIndex = 6;
            this.acceptTerms.Text = "Yes, I accept the Terms and Conditions";
            this.acceptTerms.UseVisualStyleBackColor = true;
            this.acceptTerms.CheckedChanged += new System.EventHandler(this.OnAcceptTermsChanged);
            // 
            // declineTerms
            // 
            this.declineTerms.AutoSize = true;
            this.declineTerms.Checked = true;
            this.declineTerms.Location = new System.Drawing.Point(15, 394);
            this.declineTerms.Name = "declineTerms";
            this.declineTerms.Size = new System.Drawing.Size(239, 17);
            this.declineTerms.TabIndex = 7;
            this.declineTerms.TabStop = true;
            this.declineTerms.Text = "No, I do not accept the Terms and conditions";
            this.declineTerms.UseVisualStyleBackColor = true;
            // 
            // termsLink
            // 
            this.termsLink.AutoSize = true;
            this.termsLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.termsLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.termsLink.ForeColor = System.Drawing.Color.Blue;
            this.termsLink.Location = new System.Drawing.Point(12, 355);
            this.termsLink.Name = "termsLink";
            this.termsLink.Size = new System.Drawing.Size(110, 13);
            this.termsLink.TabIndex = 5;
            this.termsLink.Text = "Terms and Conditions";
            this.termsLink.Click += new System.EventHandler(this.OnTermsClick);
            // 
            // UpsPromoDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 478);
            this.ControlBox = false;
            this.Controls.Add(this.declineTerms);
            this.Controls.Add(this.acceptTerms);
            this.Controls.Add(this.termsLink);
            this.Controls.Add(this.decline);
            this.Controls.Add(this.enroll);
            this.Controls.Add(this.remindMe);
            this.Name = "UpsPromoDlg";
            this.ShowInTaskbar = false;
            this.Text = "Activate Ups Promo";
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
    }
}