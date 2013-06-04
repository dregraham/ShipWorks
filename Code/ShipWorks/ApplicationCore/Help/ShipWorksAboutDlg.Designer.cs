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
            this.close = new System.Windows.Forms.Button();
            this.infotipSizeRemaining = new ShipWorks.UI.Controls.InfoTip();
            this.usageRemaining = new System.Windows.Forms.Label();
            this.labelUsageRemaining = new System.Windows.Forms.Label();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.usageTotal = new System.Windows.Forms.Label();
            this.labelUsageTotal = new System.Windows.Forms.Label();
            this.usageAudit = new System.Windows.Forms.Label();
            this.labelUsageAudit = new System.Windows.Forms.Label();
            this.usageResources = new System.Windows.Forms.Label();
            this.labelUsageResources = new System.Windows.Forms.Label();
            this.usageOrders = new System.Windows.Forms.Label();
            this.labelUsageOrders = new System.Windows.Forms.Label();
            this.sectionDiskUsage = new ShipWorks.UI.Controls.SectionTitle();
            this.labelCredentials = new System.Windows.Forms.Label();
            this.labelDatabase = new System.Windows.Forms.Label();
            this.labelSqlServer = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.sectionTitle1 = new ShipWorks.UI.Controls.SectionTitle();
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
            this.sectionSupport = new ShipWorks.UI.Controls.SectionTitle();
            this.sectionShipWorks = new ShipWorks.UI.Controls.SectionTitle();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(287, 459);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 0;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // infotipSizeRemaining
            // 
            this.infotipSizeRemaining.Caption = "SQL Server Express has a {0} GB size limit.  \r\n\r\nThere is no limit for paid editi" +
                "ons of SQL Server.";
            this.infotipSizeRemaining.Location = new System.Drawing.Point(144, 429);
            this.infotipSizeRemaining.Name = "infotipSizeRemaining";
            this.infotipSizeRemaining.Size = new System.Drawing.Size(12, 12);
            this.infotipSizeRemaining.TabIndex = 65;
            this.infotipSizeRemaining.Title = "Remaining Size";
            // 
            // usageRemaining
            // 
            this.usageRemaining.AutoSize = true;
            this.usageRemaining.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.usageRemaining.Location = new System.Drawing.Point(99, 428);
            this.usageRemaining.Name = "usageRemaining";
            this.usageRemaining.Size = new System.Drawing.Size(41, 13);
            this.usageRemaining.TabIndex = 64;
            this.usageRemaining.Text = "14 MB";
            // 
            // labelUsageRemaining
            // 
            this.labelUsageRemaining.AutoSize = true;
            this.labelUsageRemaining.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelUsageRemaining.Location = new System.Drawing.Point(31, 428);
            this.labelUsageRemaining.Name = "labelUsageRemaining";
            this.labelUsageRemaining.Size = new System.Drawing.Size(70, 13);
            this.labelUsageRemaining.TabIndex = 63;
            this.labelUsageRemaining.Text = "Remaining:";
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(28, 406);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(140, 1);
            this.kryptonBorderEdge.TabIndex = 62;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // usageTotal
            // 
            this.usageTotal.AutoSize = true;
            this.usageTotal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.usageTotal.Location = new System.Drawing.Point(99, 411);
            this.usageTotal.Name = "usageTotal";
            this.usageTotal.Size = new System.Drawing.Size(41, 13);
            this.usageTotal.TabIndex = 61;
            this.usageTotal.Text = "14 MB";
            // 
            // labelUsageTotal
            // 
            this.labelUsageTotal.AutoSize = true;
            this.labelUsageTotal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelUsageTotal.Location = new System.Drawing.Point(62, 411);
            this.labelUsageTotal.Name = "labelUsageTotal";
            this.labelUsageTotal.Size = new System.Drawing.Size(39, 13);
            this.labelUsageTotal.TabIndex = 60;
            this.labelUsageTotal.Text = "Total:";
            // 
            // usageAudit
            // 
            this.usageAudit.AutoSize = true;
            this.usageAudit.Location = new System.Drawing.Point(101, 371);
            this.usageAudit.Name = "usageAudit";
            this.usageAudit.Size = new System.Drawing.Size(36, 13);
            this.usageAudit.TabIndex = 59;
            this.usageAudit.Text = "14 MB";
            // 
            // labelUsageAudit
            // 
            this.labelUsageAudit.AutoSize = true;
            this.labelUsageAudit.Location = new System.Drawing.Point(41, 371);
            this.labelUsageAudit.Name = "labelUsageAudit";
            this.labelUsageAudit.Size = new System.Drawing.Size(61, 13);
            this.labelUsageAudit.TabIndex = 58;
            this.labelUsageAudit.Text = "Audit Logs:";
            this.labelUsageAudit.UseMnemonic = false;
            // 
            // usageResources
            // 
            this.usageResources.AutoSize = true;
            this.usageResources.Location = new System.Drawing.Point(101, 388);
            this.usageResources.Name = "usageResources";
            this.usageResources.Size = new System.Drawing.Size(36, 13);
            this.usageResources.TabIndex = 57;
            this.usageResources.Text = "14 MB";
            // 
            // labelUsageResources
            // 
            this.labelUsageResources.AutoSize = true;
            this.labelUsageResources.Location = new System.Drawing.Point(41, 388);
            this.labelUsageResources.Name = "labelUsageResources";
            this.labelUsageResources.Size = new System.Drawing.Size(61, 13);
            this.labelUsageResources.TabIndex = 56;
            this.labelUsageResources.Text = "Resources:";
            this.labelUsageResources.UseMnemonic = false;
            // 
            // usageOrders
            // 
            this.usageOrders.AutoSize = true;
            this.usageOrders.Location = new System.Drawing.Point(101, 354);
            this.usageOrders.Name = "usageOrders";
            this.usageOrders.Size = new System.Drawing.Size(36, 13);
            this.usageOrders.TabIndex = 55;
            this.usageOrders.Text = "14 MB";
            // 
            // labelUsageOrders
            // 
            this.labelUsageOrders.AutoSize = true;
            this.labelUsageOrders.Location = new System.Drawing.Point(38, 354);
            this.labelUsageOrders.Name = "labelUsageOrders";
            this.labelUsageOrders.Size = new System.Drawing.Size(65, 13);
            this.labelUsageOrders.TabIndex = 54;
            this.labelUsageOrders.Text = "Order Data:";
            // 
            // sectionDiskUsage
            // 
            this.sectionDiskUsage.Location = new System.Drawing.Point(11, 325);
            this.sectionDiskUsage.Name = "sectionDiskUsage";
            this.sectionDiskUsage.Size = new System.Drawing.Size(350, 22);
            this.sectionDiskUsage.TabIndex = 53;
            this.sectionDiskUsage.Text = "Database Disk Usage";
            // 
            // labelCredentials
            // 
            this.labelCredentials.AutoSize = true;
            this.labelCredentials.Location = new System.Drawing.Point(103, 302);
            this.labelCredentials.Name = "labelCredentials";
            this.labelCredentials.Size = new System.Drawing.Size(61, 13);
            this.labelCredentials.TabIndex = 52;
            this.labelCredentials.Text = "Credentials";
            // 
            // labelDatabase
            // 
            this.labelDatabase.AutoSize = true;
            this.labelDatabase.Location = new System.Drawing.Point(103, 279);
            this.labelDatabase.Name = "labelDatabase";
            this.labelDatabase.Size = new System.Drawing.Size(53, 13);
            this.labelDatabase.TabIndex = 51;
            this.labelDatabase.Text = "Database";
            // 
            // labelSqlServer
            // 
            this.labelSqlServer.AutoSize = true;
            this.labelSqlServer.Location = new System.Drawing.Point(103, 257);
            this.labelSqlServer.Name = "labelSqlServer";
            this.labelSqlServer.Size = new System.Drawing.Size(108, 13);
            this.labelSqlServer.TabIndex = 50;
            this.labelSqlServer.Text = "COMPUTER\\Instance";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.Location = new System.Drawing.Point(25, 302);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 49;
            this.label1.Text = "Credentials:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label2.Location = new System.Drawing.Point(33, 279);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 48;
            this.label2.Text = "Database:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label3.Location = new System.Drawing.Point(25, 257);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 47;
            this.label3.Text = "SQL Server:";
            // 
            // sectionTitle1
            // 
            this.sectionTitle1.Location = new System.Drawing.Point(11, 227);
            this.sectionTitle1.Name = "sectionTitle1";
            this.sectionTitle1.Size = new System.Drawing.Size(350, 22);
            this.sectionTitle1.TabIndex = 46;
            this.sectionTitle1.Text = "Database Connection";
            // 
            // phone
            // 
            this.phone.AutoSize = true;
            this.phone.Location = new System.Drawing.Point(82, 199);
            this.phone.Name = "phone";
            this.phone.Size = new System.Drawing.Size(96, 13);
            this.phone.TabIndex = 45;
            this.phone.Text = "1-800-95-APPTIVE";
            // 
            // emailSupport
            // 
            this.emailSupport.AutoSize = true;
            this.emailSupport.Location = new System.Drawing.Point(79, 176);
            this.emailSupport.Name = "emailSupport";
            this.emailSupport.Size = new System.Drawing.Size(135, 13);
            this.emailSupport.TabIndex = 44;
            this.emailSupport.TabStop = true;
            this.emailSupport.Text = "support@interapptive.com";
            this.emailSupport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnEmailSupport);
            // 
            // forumLink
            // 
            this.forumLink.AutoSize = true;
            this.forumLink.Location = new System.Drawing.Point(79, 152);
            this.forumLink.Name = "forumLink";
            this.forumLink.Size = new System.Drawing.Size(157, 13);
            this.forumLink.TabIndex = 43;
            this.forumLink.TabStop = true;
            this.forumLink.Text = "www.interapptive.com/support";
            this.forumLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnSupportForum);
            // 
            // labelPhone
            // 
            this.labelPhone.AutoSize = true;
            this.labelPhone.Location = new System.Drawing.Point(29, 199);
            this.labelPhone.Name = "labelPhone";
            this.labelPhone.Size = new System.Drawing.Size(41, 13);
            this.labelPhone.TabIndex = 42;
            this.labelPhone.Text = "Phone:";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(35, 176);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(35, 13);
            this.labelEmail.TabIndex = 41;
            this.labelEmail.Text = "Email:";
            // 
            // labelForum
            // 
            this.labelForum.AutoSize = true;
            this.labelForum.Location = new System.Drawing.Point(31, 152);
            this.labelForum.Name = "labelForum";
            this.labelForum.Size = new System.Drawing.Size(41, 13);
            this.labelForum.TabIndex = 40;
            this.labelForum.Text = "Forum:";
            // 
            // built
            // 
            this.built.AutoSize = true;
            this.built.Location = new System.Drawing.Point(76, 87);
            this.built.Name = "built";
            this.built.Size = new System.Drawing.Size(38, 13);
            this.built.TabIndex = 38;
            this.built.Text = "(Date)";
            // 
            // labelBuilt
            // 
            this.labelBuilt.AutoSize = true;
            this.labelBuilt.Location = new System.Drawing.Point(40, 87);
            this.labelBuilt.Name = "labelBuilt";
            this.labelBuilt.Size = new System.Drawing.Size(31, 13);
            this.labelBuilt.TabIndex = 37;
            this.labelBuilt.Text = "Built:";
            // 
            // version
            // 
            this.version.BackColor = System.Drawing.SystemColors.Window;
            this.version.Location = new System.Drawing.Point(76, 60);
            this.version.Name = "version";
            this.version.ReadOnly = true;
            this.version.Size = new System.Drawing.Size(100, 21);
            this.version.TabIndex = 36;
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(25, 63);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(46, 13);
            this.labelVersion.TabIndex = 35;
            this.labelVersion.Text = "Version:";
            // 
            // labelShipWorks
            // 
            this.labelShipWorks.AutoSize = true;
            this.labelShipWorks.Location = new System.Drawing.Point(25, 41);
            this.labelShipWorks.Name = "labelShipWorks";
            this.labelShipWorks.Size = new System.Drawing.Size(130, 13);
            this.labelShipWorks.TabIndex = 34;
            this.labelShipWorks.Text = "Interapptive® ShipWorks";
            // 
            // sectionSupport
            // 
            this.sectionSupport.Location = new System.Drawing.Point(11, 116);
            this.sectionSupport.Name = "sectionSupport";
            this.sectionSupport.Size = new System.Drawing.Size(350, 22);
            this.sectionSupport.TabIndex = 39;
            this.sectionSupport.Text = "Support";
            // 
            // sectionShipWorks
            // 
            this.sectionShipWorks.Location = new System.Drawing.Point(11, 11);
            this.sectionShipWorks.Name = "sectionShipWorks";
            this.sectionShipWorks.Size = new System.Drawing.Size(350, 22);
            this.sectionShipWorks.TabIndex = 33;
            this.sectionShipWorks.Text = "Version";
            // 
            // ShipWorksAboutDlg
            // 
            this.AcceptButton = this.close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(374, 494);
            this.Controls.Add(this.infotipSizeRemaining);
            this.Controls.Add(this.usageRemaining);
            this.Controls.Add(this.labelUsageRemaining);
            this.Controls.Add(this.kryptonBorderEdge);
            this.Controls.Add(this.usageTotal);
            this.Controls.Add(this.labelUsageTotal);
            this.Controls.Add(this.usageAudit);
            this.Controls.Add(this.labelUsageAudit);
            this.Controls.Add(this.usageResources);
            this.Controls.Add(this.labelUsageResources);
            this.Controls.Add(this.usageOrders);
            this.Controls.Add(this.labelUsageOrders);
            this.Controls.Add(this.sectionDiskUsage);
            this.Controls.Add(this.labelCredentials);
            this.Controls.Add(this.labelDatabase);
            this.Controls.Add(this.labelSqlServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.sectionTitle1);
            this.Controls.Add(this.phone);
            this.Controls.Add(this.emailSupport);
            this.Controls.Add(this.forumLink);
            this.Controls.Add(this.labelPhone);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.labelForum);
            this.Controls.Add(this.sectionSupport);
            this.Controls.Add(this.built);
            this.Controls.Add(this.labelBuilt);
            this.Controls.Add(this.version);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelShipWorks);
            this.Controls.Add(this.sectionShipWorks);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
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
        private UI.Controls.InfoTip infotipSizeRemaining;
        private System.Windows.Forms.Label usageRemaining;
        private System.Windows.Forms.Label labelUsageRemaining;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private System.Windows.Forms.Label usageTotal;
        private System.Windows.Forms.Label labelUsageTotal;
        private System.Windows.Forms.Label usageAudit;
        private System.Windows.Forms.Label labelUsageAudit;
        private System.Windows.Forms.Label usageResources;
        private System.Windows.Forms.Label labelUsageResources;
        private System.Windows.Forms.Label usageOrders;
        private System.Windows.Forms.Label labelUsageOrders;
        private UI.Controls.SectionTitle sectionDiskUsage;
        private System.Windows.Forms.Label labelCredentials;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.Label labelSqlServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private UI.Controls.SectionTitle sectionTitle1;
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
        private UI.Controls.SectionTitle sectionSupport;
        private UI.Controls.SectionTitle sectionShipWorks;
    }
}