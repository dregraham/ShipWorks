namespace ShipWorks.Data.Connection
{
    partial class DatabaseLogonDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseLogonDlg));
            this.label1 = new System.Windows.Forms.Label();
            this.labelWindowsAuthDescription = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.sqlServerAuth = new System.Windows.Forms.RadioButton();
            this.windowsAuth = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelConnectUsing = new System.Windows.Forms.Label();
            this.database = new System.Windows.Forms.TextBox();
            this.labelDatabase = new System.Windows.Forms.Label();
            this.sqlServer = new System.Windows.Forms.TextBox();
            this.labelServer = new System.Windows.Forms.Label();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.sep = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.headerImage = new System.Windows.Forms.PictureBox();
            this.subtitle = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this.headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerImage)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Connecting to:";
            // 
            // labelWindowsAuthDescription
            // 
            this.labelWindowsAuthDescription.ForeColor = System.Drawing.Color.DimGray;
            this.labelWindowsAuthDescription.Location = new System.Drawing.Point(65, 280);
            this.labelWindowsAuthDescription.Name = "labelWindowsAuthDescription";
            this.labelWindowsAuthDescription.Size = new System.Drawing.Size(336, 28);
            this.labelWindowsAuthDescription.TabIndex = 15;
            this.labelWindowsAuthDescription.Text = "You must be currently logged in to Windows as a user that has access to SQL Serve" +
    "r.";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(123, 199);
            this.username.MaxLength = 128;
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(217, 21);
            this.username.TabIndex = 10;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(123, 225);
            this.password.MaxLength = 128;
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(217, 21);
            this.password.TabIndex = 12;
            this.password.UseSystemPasswordChar = true;
            // 
            // sqlServerAuth
            // 
            this.sqlServerAuth.Location = new System.Drawing.Point(47, 171);
            this.sqlServerAuth.Name = "sqlServerAuth";
            this.sqlServerAuth.Size = new System.Drawing.Size(236, 24);
            this.sqlServerAuth.TabIndex = 8;
            this.sqlServerAuth.Text = "SQL Server authentication";
            this.sqlServerAuth.CheckedChanged += new System.EventHandler(this.OnChangeSqlAuthType);
            // 
            // windowsAuth
            // 
            this.windowsAuth.Location = new System.Drawing.Point(47, 258);
            this.windowsAuth.Name = "windowsAuth";
            this.windowsAuth.Size = new System.Drawing.Size(228, 24);
            this.windowsAuth.TabIndex = 14;
            this.windowsAuth.Text = "Windows authentication";
            this.windowsAuth.CheckedChanged += new System.EventHandler(this.OnChangeSqlAuthType);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(63, 202);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Username:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(65, 228);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Password:";
            // 
            // labelConnectUsing
            // 
            this.labelConnectUsing.AutoSize = true;
            this.labelConnectUsing.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConnectUsing.Location = new System.Drawing.Point(13, 153);
            this.labelConnectUsing.Name = "labelConnectUsing";
            this.labelConnectUsing.Size = new System.Drawing.Size(160, 13);
            this.labelConnectUsing.TabIndex = 7;
            this.labelConnectUsing.Text = "Log on to SQL Server using:";
            // 
            // database
            // 
            this.database.Location = new System.Drawing.Point(130, 120);
            this.database.Name = "database";
            this.database.ReadOnly = true;
            this.database.Size = new System.Drawing.Size(210, 21);
            this.database.TabIndex = 6;
            this.database.Text = "ShipWorks";
            // 
            // labelDatabase
            // 
            this.labelDatabase.AutoSize = true;
            this.labelDatabase.Location = new System.Drawing.Point(67, 123);
            this.labelDatabase.Name = "labelDatabase";
            this.labelDatabase.Size = new System.Drawing.Size(57, 13);
            this.labelDatabase.TabIndex = 5;
            this.labelDatabase.Text = "Database:";
            // 
            // sqlServer
            // 
            this.sqlServer.Location = new System.Drawing.Point(130, 93);
            this.sqlServer.Name = "sqlServer";
            this.sqlServer.ReadOnly = true;
            this.sqlServer.Size = new System.Drawing.Size(210, 21);
            this.sqlServer.TabIndex = 4;
            this.sqlServer.Text = "INTERAPPTIVE\\SHIPWORKS";
            // 
            // labelServer
            // 
            this.labelServer.AutoSize = true;
            this.labelServer.Location = new System.Drawing.Point(14, 96);
            this.labelServer.Name = "labelServer";
            this.labelServer.Size = new System.Drawing.Size(110, 13);
            this.labelServer.TabIndex = 3;
            this.labelServer.Text = "SQL Server Instance:";
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(216, 324);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 16;
            this.ok.Text = "Connect";
            this.ok.Click += new System.EventHandler(this.OnConnect);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(298, 324);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 17;
            this.cancel.Text = "Cancel";
            // 
            // sep
            // 
            this.sep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sep.Dock = System.Windows.Forms.DockStyle.Top;
            this.sep.Location = new System.Drawing.Point(0, 58);
            this.sep.Name = "sep";
            this.sep.Size = new System.Drawing.Size(389, 3);
            this.sep.TabIndex = 1;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.Controls.Add(this.headerImage);
            this.headerPanel.Controls.Add(this.subtitle);
            this.headerPanel.Controls.Add(this.title);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(389, 58);
            this.headerPanel.TabIndex = 0;
            // 
            // headerImage
            // 
            this.headerImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.headerImage.Cursor = System.Windows.Forms.Cursors.Default;
            this.headerImage.Image = ((System.Drawing.Image)(resources.GetObject("headerImage.Image")));
            this.headerImage.Location = new System.Drawing.Point(325, 4);
            this.headerImage.Name = "headerImage";
            this.headerImage.Size = new System.Drawing.Size(48, 48);
            this.headerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.headerImage.TabIndex = 5;
            this.headerImage.TabStop = false;
            // 
            // subtitle
            // 
            this.subtitle.Location = new System.Drawing.Point(40, 30);
            this.subtitle.Name = "subtitle";
            this.subtitle.Size = new System.Drawing.Size(386, 14);
            this.subtitle.TabIndex = 1;
            this.subtitle.Text = "ShipWorks needs to connect to SQL Server.";
            // 
            // title
            // 
            this.title.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(20, 10);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(296, 14);
            this.title.TabIndex = 0;
            this.title.Text = "Connect to SQL Server";
            // 
            // DatabaseLogonDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(389, 359);
            this.Controls.Add(this.sep);
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.labelWindowsAuthDescription);
            this.Controls.Add(this.username);
            this.Controls.Add(this.password);
            this.Controls.Add(this.sqlServerAuth);
            this.Controls.Add(this.windowsAuth);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.labelConnectUsing);
            this.Controls.Add(this.database);
            this.Controls.Add(this.labelDatabase);
            this.Controls.Add(this.sqlServer);
            this.Controls.Add(this.labelServer);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DatabaseLogonDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect to SQL Server";
            this.Load += new System.EventHandler(this.OnLoad);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelWindowsAuthDescription;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.RadioButton sqlServerAuth;
        private System.Windows.Forms.RadioButton windowsAuth;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelConnectUsing;
        private System.Windows.Forms.TextBox database;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.TextBox sqlServer;
        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label sep;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.PictureBox headerImage;
        private System.Windows.Forms.Label subtitle;
        private System.Windows.Forms.Label title;
    }
}