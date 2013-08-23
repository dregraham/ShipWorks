﻿namespace ShipWorks.Data.Administration
{
    partial class DatabaseDetailsDlg
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
            this.labelSqlInstance = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
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
            this.labelDatabase = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelLoggedInAs = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelRemoteConnections = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.linkEnableRemoteConnections = new System.Windows.Forms.LinkLabel();
            this.infotipSizeRemaining = new ShipWorks.UI.Controls.InfoTip();
            this.sectionActiveConnection = new ShipWorks.UI.Controls.SectionTitle();
            this.sectionDiskUsage = new ShipWorks.UI.Controls.SectionTitle();
            this.configureDatabase = new System.Windows.Forms.Button();
            this.sectionConfiguration = new ShipWorks.UI.Controls.SectionTitle();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(272, 368);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 0;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // labelSqlInstance
            // 
            this.labelSqlInstance.AutoSize = true;
            this.labelSqlInstance.Location = new System.Drawing.Point(124, 44);
            this.labelSqlInstance.Name = "labelSqlInstance";
            this.labelSqlInstance.Size = new System.Drawing.Size(196, 13);
            this.labelSqlInstance.TabIndex = 85;
            this.labelSqlInstance.Text = "SHIPWORKS-PC4\\SHIPWORKSTESTING";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(43, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 84;
            this.label2.Text = "SQL Instance:";
            // 
            // usageRemaining
            // 
            this.usageRemaining.AutoSize = true;
            this.usageRemaining.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usageRemaining.Location = new System.Drawing.Point(124, 334);
            this.usageRemaining.Name = "usageRemaining";
            this.usageRemaining.Size = new System.Drawing.Size(41, 13);
            this.usageRemaining.TabIndex = 82;
            this.usageRemaining.Text = "14 MB";
            // 
            // labelUsageRemaining
            // 
            this.labelUsageRemaining.AutoSize = true;
            this.labelUsageRemaining.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUsageRemaining.Location = new System.Drawing.Point(48, 334);
            this.labelUsageRemaining.Name = "labelUsageRemaining";
            this.labelUsageRemaining.Size = new System.Drawing.Size(70, 13);
            this.labelUsageRemaining.TabIndex = 81;
            this.labelUsageRemaining.Text = "Remaining:";
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(35, 309);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(140, 1);
            this.kryptonBorderEdge.TabIndex = 80;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // usageTotal
            // 
            this.usageTotal.AutoSize = true;
            this.usageTotal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usageTotal.Location = new System.Drawing.Point(124, 314);
            this.usageTotal.Name = "usageTotal";
            this.usageTotal.Size = new System.Drawing.Size(41, 13);
            this.usageTotal.TabIndex = 79;
            this.usageTotal.Text = "14 MB";
            // 
            // labelUsageTotal
            // 
            this.labelUsageTotal.AutoSize = true;
            this.labelUsageTotal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUsageTotal.Location = new System.Drawing.Point(79, 314);
            this.labelUsageTotal.Name = "labelUsageTotal";
            this.labelUsageTotal.Size = new System.Drawing.Size(39, 13);
            this.labelUsageTotal.TabIndex = 78;
            this.labelUsageTotal.Text = "Total:";
            // 
            // usageAudit
            // 
            this.usageAudit.AutoSize = true;
            this.usageAudit.Location = new System.Drawing.Point(124, 267);
            this.usageAudit.Name = "usageAudit";
            this.usageAudit.Size = new System.Drawing.Size(36, 13);
            this.usageAudit.TabIndex = 77;
            this.usageAudit.Text = "14 MB";
            // 
            // labelUsageAudit
            // 
            this.labelUsageAudit.AutoSize = true;
            this.labelUsageAudit.Location = new System.Drawing.Point(57, 267);
            this.labelUsageAudit.Name = "labelUsageAudit";
            this.labelUsageAudit.Size = new System.Drawing.Size(61, 13);
            this.labelUsageAudit.TabIndex = 76;
            this.labelUsageAudit.Text = "Audit Logs:";
            this.labelUsageAudit.UseMnemonic = false;
            // 
            // usageResources
            // 
            this.usageResources.AutoSize = true;
            this.usageResources.Location = new System.Drawing.Point(124, 289);
            this.usageResources.Name = "usageResources";
            this.usageResources.Size = new System.Drawing.Size(36, 13);
            this.usageResources.TabIndex = 75;
            this.usageResources.Text = "14 MB";
            // 
            // labelUsageResources
            // 
            this.labelUsageResources.AutoSize = true;
            this.labelUsageResources.Location = new System.Drawing.Point(57, 289);
            this.labelUsageResources.Name = "labelUsageResources";
            this.labelUsageResources.Size = new System.Drawing.Size(61, 13);
            this.labelUsageResources.TabIndex = 74;
            this.labelUsageResources.Text = "Resources:";
            this.labelUsageResources.UseMnemonic = false;
            // 
            // usageOrders
            // 
            this.usageOrders.AutoSize = true;
            this.usageOrders.Location = new System.Drawing.Point(124, 246);
            this.usageOrders.Name = "usageOrders";
            this.usageOrders.Size = new System.Drawing.Size(36, 13);
            this.usageOrders.TabIndex = 73;
            this.usageOrders.Text = "14 MB";
            // 
            // labelUsageOrders
            // 
            this.labelUsageOrders.AutoSize = true;
            this.labelUsageOrders.Location = new System.Drawing.Point(53, 246);
            this.labelUsageOrders.Name = "labelUsageOrders";
            this.labelUsageOrders.Size = new System.Drawing.Size(65, 13);
            this.labelUsageOrders.TabIndex = 72;
            this.labelUsageOrders.Text = "Order Data:";
            // 
            // labelDatabase
            // 
            this.labelDatabase.AutoSize = true;
            this.labelDatabase.Location = new System.Drawing.Point(124, 66);
            this.labelDatabase.Name = "labelDatabase";
            this.labelDatabase.Size = new System.Drawing.Size(57, 13);
            this.labelDatabase.TabIndex = 71;
            this.labelDatabase.Text = "ShipWorks";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(61, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 70;
            this.label3.Text = "Database:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 86;
            this.label1.Text = "Connection";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(47, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 87;
            this.label4.Text = "Logged in as:";
            // 
            // labelLoggedInAs
            // 
            this.labelLoggedInAs.AutoSize = true;
            this.labelLoggedInAs.Location = new System.Drawing.Point(124, 91);
            this.labelLoggedInAs.Name = "labelLoggedInAs";
            this.labelLoggedInAs.Size = new System.Drawing.Size(18, 13);
            this.labelLoggedInAs.TabIndex = 88;
            this.labelLoggedInAs.Text = "sa";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(24, 223);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 89;
            this.label6.Text = "Disk Usage";
            // 
            // labelRemoteConnections
            // 
            this.labelRemoteConnections.AutoSize = true;
            this.labelRemoteConnections.Location = new System.Drawing.Point(124, 114);
            this.labelRemoteConnections.Name = "labelRemoteConnections";
            this.labelRemoteConnections.Size = new System.Drawing.Size(77, 13);
            this.labelRemoteConnections.TabIndex = 91;
            this.labelRemoteConnections.Text = "Not Supported";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 114);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 13);
            this.label8.TabIndex = 90;
            this.label8.Text = "Remote connections:";
            // 
            // linkEnableRemoteConnections
            // 
            this.linkEnableRemoteConnections.AutoSize = true;
            this.linkEnableRemoteConnections.LinkColor = System.Drawing.Color.CornflowerBlue;
            this.linkEnableRemoteConnections.Location = new System.Drawing.Point(201, 114);
            this.linkEnableRemoteConnections.Name = "linkEnableRemoteConnections";
            this.linkEnableRemoteConnections.Size = new System.Drawing.Size(87, 13);
            this.linkEnableRemoteConnections.TabIndex = 92;
            this.linkEnableRemoteConnections.TabStop = true;
            this.linkEnableRemoteConnections.Text = "(Enable support)";
            this.linkEnableRemoteConnections.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnEnableRemoteConnections);
            // 
            // infotipSizeRemaining
            // 
            this.infotipSizeRemaining.Caption = "SQL Server Express has a {0} GB size limit.  \r\n\r\nThere is no limit for paid editi" +
    "ons of SQL Server.";
            this.infotipSizeRemaining.Location = new System.Drawing.Point(169, 335);
            this.infotipSizeRemaining.Name = "infotipSizeRemaining";
            this.infotipSizeRemaining.Size = new System.Drawing.Size(12, 12);
            this.infotipSizeRemaining.TabIndex = 83;
            this.infotipSizeRemaining.Title = "Remaining Size";
            // 
            // sectionActiveConnection
            // 
            this.sectionActiveConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionActiveConnection.Location = new System.Drawing.Point(13, 11);
            this.sectionActiveConnection.Name = "sectionActiveConnection";
            this.sectionActiveConnection.Size = new System.Drawing.Size(333, 22);
            this.sectionActiveConnection.TabIndex = 93;
            this.sectionActiveConnection.Text = "Active Connection";
            // 
            // sectionDiskUsage
            // 
            this.sectionDiskUsage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionDiskUsage.Location = new System.Drawing.Point(13, 219);
            this.sectionDiskUsage.Name = "sectionDiskUsage";
            this.sectionDiskUsage.Size = new System.Drawing.Size(333, 22);
            this.sectionDiskUsage.TabIndex = 94;
            this.sectionDiskUsage.Text = "Disk Usage";
            // 
            // configureDatabase
            // 
            this.configureDatabase.Location = new System.Drawing.Point(46, 178);
            this.configureDatabase.Name = "configureDatabase";
            this.configureDatabase.Size = new System.Drawing.Size(138, 23);
            this.configureDatabase.TabIndex = 95;
            this.configureDatabase.Text = "Configure Database...";
            this.configureDatabase.UseVisualStyleBackColor = true;
            this.configureDatabase.Click += new System.EventHandler(this.OnConfigureDatabase);
            // 
            // sectionConfiguration
            // 
            this.sectionConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionConfiguration.Location = new System.Drawing.Point(13, 143);
            this.sectionConfiguration.Name = "sectionConfiguration";
            this.sectionConfiguration.Size = new System.Drawing.Size(333, 22);
            this.sectionConfiguration.TabIndex = 96;
            this.sectionConfiguration.Text = "Configuration";
            // 
            // DatabaseDetailsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(359, 403);
            this.Controls.Add(this.sectionConfiguration);
            this.Controls.Add(this.configureDatabase);
            this.Controls.Add(this.sectionDiskUsage);
            this.Controls.Add(this.sectionActiveConnection);
            this.Controls.Add(this.linkEnableRemoteConnections);
            this.Controls.Add(this.labelRemoteConnections);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.labelLoggedInAs);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelSqlInstance);
            this.Controls.Add(this.label2);
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
            this.Controls.Add(this.labelDatabase);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DatabaseDetailsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database Configuration";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Label labelSqlInstance;
        private System.Windows.Forms.Label label2;
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
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelLoggedInAs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelRemoteConnections;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.LinkLabel linkEnableRemoteConnections;
        private UI.Controls.SectionTitle sectionActiveConnection;
        private UI.Controls.SectionTitle sectionDiskUsage;
        private System.Windows.Forms.Button configureDatabase;
        private UI.Controls.SectionTitle sectionConfiguration;
    }
}