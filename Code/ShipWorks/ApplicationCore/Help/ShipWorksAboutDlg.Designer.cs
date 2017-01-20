namespace ShipWorks.ApplicationCore.Help
{
    partial class ShipWorksAboutDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShipWorksAboutDlg));
            this.close = new System.Windows.Forms.Button();
            this.phone = new System.Windows.Forms.Label();
            this.emailSupport = new System.Windows.Forms.LinkLabel();
            this.forumLink = new System.Windows.Forms.LinkLabel();
            this.labelPhone = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.labelForum = new System.Windows.Forms.Label();
            this.built = new System.Windows.Forms.Label();
            this.labelBuilt = new System.Windows.Forms.Label();
            this.version = new System.Windows.Forms.TextBox();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelShipWorks = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.separator = new System.Windows.Forms.Label();
            this.copyrightInfo = new System.Windows.Forms.Label();
            this.patentInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(195, 386);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 0;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // phone
            // 
            this.phone.AutoSize = true;
            this.phone.Location = new System.Drawing.Point(79, 176);
            this.phone.Name = "phone";
            this.phone.Size = new System.Drawing.Size(96, 13);
            this.phone.TabIndex = 45;
            this.phone.Text = "1-800-95-APPTIVE";
            // 
            // emailSupport
            // 
            this.emailSupport.AutoSize = true;
            this.emailSupport.Location = new System.Drawing.Point(77, 153);
            this.emailSupport.Name = "emailSupport";
            this.emailSupport.Size = new System.Drawing.Size(124, 13);
            this.emailSupport.TabIndex = 44;
            this.emailSupport.TabStop = true;
            this.emailSupport.Text = "support@shipworks.com";
            this.emailSupport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnEmailSupport);
            // 
            // forumLink
            // 
            this.forumLink.AutoSize = true;
            this.forumLink.Location = new System.Drawing.Point(77, 129);
            this.forumLink.Name = "forumLink";
            this.forumLink.Size = new System.Drawing.Size(146, 13);
            this.forumLink.TabIndex = 43;
            this.forumLink.TabStop = true;
            this.forumLink.Text = "www.shipworks.com/support";
            this.forumLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnGetSupport);
            // 
            // labelPhone
            // 
            this.labelPhone.AutoSize = true;
            this.labelPhone.Location = new System.Drawing.Point(33, 176);
            this.labelPhone.Name = "labelPhone";
            this.labelPhone.Size = new System.Drawing.Size(41, 13);
            this.labelPhone.TabIndex = 42;
            this.labelPhone.Text = "Phone:";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(39, 153);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(35, 13);
            this.labelEmail.TabIndex = 41;
            this.labelEmail.Text = "Email:";
            // 
            // labelForum
            // 
            this.labelForum.AutoSize = true;
            this.labelForum.Location = new System.Drawing.Point(33, 129);
            this.labelForum.Name = "labelForum";
            this.labelForum.Size = new System.Drawing.Size(41, 13);
            this.labelForum.TabIndex = 40;
            this.labelForum.Text = "Online:";
            // 
            // built
            // 
            this.built.AutoSize = true;
            this.built.Location = new System.Drawing.Point(78, 73);
            this.built.Name = "built";
            this.built.Size = new System.Drawing.Size(38, 13);
            this.built.TabIndex = 38;
            this.built.Text = "(Date)";
            // 
            // labelBuilt
            // 
            this.labelBuilt.AutoSize = true;
            this.labelBuilt.Location = new System.Drawing.Point(42, 73);
            this.labelBuilt.Name = "labelBuilt";
            this.labelBuilt.Size = new System.Drawing.Size(31, 13);
            this.labelBuilt.TabIndex = 37;
            this.labelBuilt.Text = "Built:";
            // 
            // version
            // 
            this.version.BackColor = System.Drawing.SystemColors.Window;
            this.version.Location = new System.Drawing.Point(78, 46);
            this.version.Name = "version";
            this.version.ReadOnly = true;
            this.version.Size = new System.Drawing.Size(100, 21);
            this.version.TabIndex = 36;
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(27, 49);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(46, 13);
            this.labelVersion.TabIndex = 35;
            this.labelVersion.Text = "Version:";
            // 
            // labelShipWorks
            // 
            this.labelShipWorks.AutoSize = true;
            this.labelShipWorks.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShipWorks.Location = new System.Drawing.Point(12, 24);
            this.labelShipWorks.Name = "labelShipWorks";
            this.labelShipWorks.Size = new System.Drawing.Size(77, 13);
            this.labelShipWorks.TabIndex = 34;
            this.labelShipWorks.Text = "ShipWorks®";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 66;
            this.label4.Text = "Get Support";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(171, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 14);
            this.label1.TabIndex = 67;
            this.label1.Text = "Interapptive®";
            // 
            // separator
            // 
            this.separator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separator.Location = new System.Drawing.Point(15, 199);
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(255, 2);
            this.separator.TabIndex = 68;
            // 
            // copyrightInfo
            // 
            this.copyrightInfo.AutoSize = true;
            this.copyrightInfo.Location = new System.Drawing.Point(12, 324);
            this.copyrightInfo.MaximumSize = new System.Drawing.Size(255, 0);
            this.copyrightInfo.Name = "copyrightInfo";
            this.copyrightInfo.Size = new System.Drawing.Size(243, 52);
            this.copyrightInfo.TabIndex = 70;
            this.copyrightInfo.Text = "Copyright © 2003 – 2017 Interapptive, Inc. All rights reserved. Interapptive, Shi" +
    "pWorks, and \r\nthe ShipWorks logo are trademarks or registered trademarks of Inte" +
    "rapptive, Inc.";
            // 
            // patentInfo
            // 
            this.patentInfo.AutoSize = true;
            this.patentInfo.Location = new System.Drawing.Point(12, 210);
            this.patentInfo.MaximumSize = new System.Drawing.Size(255, 0);
            this.patentInfo.Name = "patentInfo";
            this.patentInfo.Size = new System.Drawing.Size(253, 104);
            this.patentInfo.TabIndex = 69;
            this.patentInfo.Text = resources.GetString("patentInfo.Text");
            // 
            // ShipWorksAboutDlg
            // 
            this.AcceptButton = this.close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(278, 421);
            this.Controls.Add(this.copyrightInfo);
            this.Controls.Add(this.patentInfo);
            this.Controls.Add(this.separator);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.phone);
            this.Controls.Add(this.emailSupport);
            this.Controls.Add(this.forumLink);
            this.Controls.Add(this.labelPhone);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.labelForum);
            this.Controls.Add(this.built);
            this.Controls.Add(this.labelBuilt);
            this.Controls.Add(this.version);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelShipWorks);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShipWorksAboutDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About ShipWorks";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Label phone;
        private System.Windows.Forms.LinkLabel emailSupport;
        private System.Windows.Forms.LinkLabel forumLink;
        private System.Windows.Forms.Label labelPhone;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.Label labelForum;
        private System.Windows.Forms.Label built;
        private System.Windows.Forms.Label labelBuilt;
        private System.Windows.Forms.TextBox version;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelShipWorks;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label separator;
        private System.Windows.Forms.Label copyrightInfo;
        private System.Windows.Forms.Label patentInfo;
    }
}