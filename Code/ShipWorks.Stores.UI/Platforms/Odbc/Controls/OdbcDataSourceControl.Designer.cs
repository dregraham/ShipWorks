﻿namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls
{
    partial class OdbcDataSourceControl
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
            this.selectDataSourceLabel = new System.Windows.Forms.Label();
            this.dataSourceLabel = new System.Windows.Forms.Label();
            this.dataSource = new System.Windows.Forms.ComboBox();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.credentialsOptionalLabel = new System.Windows.Forms.Label();
            this.addDataSource = new System.Windows.Forms.Button();
            this.credentialsPanel = new System.Windows.Forms.Panel();
            this.btnTestConnection2 = new System.Windows.Forms.Button();
            this.customPanel = new System.Windows.Forms.Panel();
            this.customConnectionStringPanel = new System.Windows.Forms.Panel();
            this.customConnectionString = new System.Windows.Forms.TextBox();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.customLabel = new System.Windows.Forms.Label();
            this.credentialsPanel.SuspendLayout();
            this.customPanel.SuspendLayout();
            this.customConnectionStringPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectDataSourceLabel
            // 
            this.selectDataSourceLabel.AutoSize = true;
            this.selectDataSourceLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectDataSourceLabel.Location = new System.Drawing.Point(10, 10);
            this.selectDataSourceLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.selectDataSourceLabel.Name = "selectDataSourceLabel";
            this.selectDataSourceLabel.Size = new System.Drawing.Size(325, 13);
            this.selectDataSourceLabel.TabIndex = 0;
            this.selectDataSourceLabel.Text = "Select the ODBC data source and provide credentials (if required):";
            // 
            // dataSourceLabel
            // 
            this.dataSourceLabel.AutoSize = true;
            this.dataSourceLabel.Location = new System.Drawing.Point(30, 40);
            this.dataSourceLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.dataSourceLabel.Name = "dataSourceLabel";
            this.dataSourceLabel.Size = new System.Drawing.Size(70, 13);
            this.dataSourceLabel.TabIndex = 0;
            this.dataSourceLabel.Text = "Data Source:";
            // 
            // dataSource
            // 
            this.dataSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataSource.FormattingEnabled = true;
            this.dataSource.Location = new System.Drawing.Point(106, 38);
            this.dataSource.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dataSource.Name = "dataSource";
            this.dataSource.Size = new System.Drawing.Size(249, 21);
            this.dataSource.TabIndex = 1;
            this.dataSource.SelectionChangeCommitted += new System.EventHandler(this.SelectedDataSourceChanged);
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(92, 5);
            this.username.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.username.MaxLength = 255;
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(374, 21);
            this.username.TabIndex = 3;
            this.username.TextChanged += new System.EventHandler(this.OnChangedUsername);
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(92, 32);
            this.password.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.password.MaxLength = 255;
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(374, 21);
            this.password.TabIndex = 4;
            this.password.UseSystemPasswordChar = true;
            this.password.TextChanged += new System.EventHandler(this.OnChangedPassword);
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(29, 35);
            this.passwordLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(57, 13);
            this.passwordLabel.TabIndex = 5;
            this.passwordLabel.Text = "Password:";
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(27, 8);
            this.usernameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(59, 13);
            this.usernameLabel.TabIndex = 6;
            this.usernameLabel.Text = "Username:";
            // 
            // credentialsOptionalLabel
            // 
            this.credentialsOptionalLabel.AutoSize = true;
            this.credentialsOptionalLabel.Enabled = false;
            this.credentialsOptionalLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.credentialsOptionalLabel.ForeColor = System.Drawing.Color.DimGray;
            this.credentialsOptionalLabel.Location = new System.Drawing.Point(89, 60);
            this.credentialsOptionalLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.credentialsOptionalLabel.Name = "credentialsOptionalLabel";
            this.credentialsOptionalLabel.Size = new System.Drawing.Size(254, 13);
            this.credentialsOptionalLabel.TabIndex = 7;
            this.credentialsOptionalLabel.Text = "Not all connections require username and password";
            // 
            // addDataSource
            // 
            this.addDataSource.Location = new System.Drawing.Point(360, 37);
            this.addDataSource.Name = "addDataSource";
            this.addDataSource.Size = new System.Drawing.Size(121, 23);
            this.addDataSource.TabIndex = 2;
            this.addDataSource.Text = "Manage Data Sources";
            this.addDataSource.UseVisualStyleBackColor = true;
            this.addDataSource.Click += new System.EventHandler(this.OnClickAddDataSource);
            // 
            // credentialsPanel
            // 
            this.credentialsPanel.Controls.Add(this.btnTestConnection2);
            this.credentialsPanel.Controls.Add(this.passwordLabel);
            this.credentialsPanel.Controls.Add(this.username);
            this.credentialsPanel.Controls.Add(this.credentialsOptionalLabel);
            this.credentialsPanel.Controls.Add(this.password);
            this.credentialsPanel.Controls.Add(this.usernameLabel);
            this.credentialsPanel.Location = new System.Drawing.Point(14, 60);
            this.credentialsPanel.Name = "credentialsPanel";
            this.credentialsPanel.Size = new System.Drawing.Size(475, 105);
            this.credentialsPanel.TabIndex = 9;
            // 
            // btnTestConnection2
            // 
            this.btnTestConnection2.Location = new System.Drawing.Point(368, 58);
            this.btnTestConnection2.Name = "btnTestConnection2";
            this.btnTestConnection2.Size = new System.Drawing.Size(99, 23);
            this.btnTestConnection2.TabIndex = 11;
            this.btnTestConnection2.Text = "Test Connection";
            this.btnTestConnection2.UseVisualStyleBackColor = true;
            this.btnTestConnection2.Click += new System.EventHandler(this.OnTestConnection);
            // 
            // customPanel
            // 
            this.customPanel.Controls.Add(this.customConnectionStringPanel);
            this.customPanel.Controls.Add(this.btnTestConnection);
            this.customPanel.Controls.Add(this.customLabel);
            this.customPanel.Location = new System.Drawing.Point(0, 60);
            this.customPanel.Name = "customPanel";
            this.customPanel.Size = new System.Drawing.Size(492, 114);
            this.customPanel.TabIndex = 8;
            // 
            // customConnectionStringPanel
            // 
            this.customConnectionStringPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.customConnectionStringPanel.Controls.Add(this.customConnectionString);
            this.customConnectionStringPanel.Location = new System.Drawing.Point(106, 5);
            this.customConnectionStringPanel.Name = "customConnectionStringPanel";
            this.customConnectionStringPanel.Size = new System.Drawing.Size(374, 52);
            this.customConnectionStringPanel.TabIndex = 0;
            // 
            // customConnectionString
            // 
            this.customConnectionString.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.customConnectionString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customConnectionString.Location = new System.Drawing.Point(0, 0);
            this.customConnectionString.MaxLength = 2048;
            this.customConnectionString.Name = "customConnectionString";
            this.customConnectionString.Size = new System.Drawing.Size(372, 50);
            this.customConnectionString.TabIndex = 0;
            this.customConnectionString.Text = "";
            this.customConnectionString.Multiline = true;
            this.customConnectionString.TextChanged += new System.EventHandler(this.OnChangedCustomConnectionString);
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(382, 62);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(99, 23);
            this.btnTestConnection.TabIndex = 1;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.OnTestConnection);
            // 
            // customLabel
            // 
            this.customLabel.AutoSize = true;
            this.customLabel.Location = new System.Drawing.Point(5, 8);
            this.customLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.customLabel.Name = "customLabel";
            this.customLabel.Size = new System.Drawing.Size(96, 13);
            this.customLabel.TabIndex = 6;
            this.customLabel.Text = "Connection String:";
            this.customLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // OdbcDataSourceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customPanel);
            this.Controls.Add(this.addDataSource);
            this.Controls.Add(this.dataSource);
            this.Controls.Add(this.dataSourceLabel);
            this.Controls.Add(this.selectDataSourceLabel);
            this.Controls.Add(this.credentialsPanel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "OdbcDataSourceControl";
            this.Size = new System.Drawing.Size(492, 178);
            this.credentialsPanel.ResumeLayout(false);
            this.credentialsPanel.PerformLayout();
            this.customPanel.ResumeLayout(false);
            this.customPanel.PerformLayout();
            this.customConnectionStringPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label selectDataSourceLabel;
        private System.Windows.Forms.Label dataSourceLabel;
        private System.Windows.Forms.ComboBox dataSource;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Label credentialsOptionalLabel;
        private System.Windows.Forms.Button addDataSource;
        private System.Windows.Forms.Panel credentialsPanel;
        private System.Windows.Forms.Panel customPanel;
        private System.Windows.Forms.Label customLabel;
        private System.Windows.Forms.Button btnTestConnection2;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.TextBox customConnectionString;
        private System.Windows.Forms.Panel customConnectionStringPanel;
    }
}
