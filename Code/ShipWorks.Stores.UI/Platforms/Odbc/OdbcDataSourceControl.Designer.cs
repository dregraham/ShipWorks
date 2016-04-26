namespace ShipWorks.Stores.UI.Platforms.Odbc
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
            this.dataSourceComboBox = new System.Windows.Forms.ComboBox();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.credentialsOptionalLabel = new System.Windows.Forms.Label();
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
            this.dataSourceLabel.TabIndex = 1;
            this.dataSourceLabel.Text = "Data Source:";
            // 
            // dataSourceComboBox
            // 
            this.dataSourceComboBox.FormattingEnabled = true;
            this.dataSourceComboBox.Location = new System.Drawing.Point(106, 38);
            this.dataSourceComboBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dataSourceComboBox.Name = "dataSourceComboBox";
            this.dataSourceComboBox.Size = new System.Drawing.Size(249, 21);
            this.dataSourceComboBox.TabIndex = 2;
            this.dataSourceComboBox.SelectionChangeCommitted += new System.EventHandler(this.SelectedDataSourceChanged);
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Location = new System.Drawing.Point(126, 103);
            this.usernameTextBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(228, 21);
            this.usernameTextBox.TabIndex = 3;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(126, 129);
            this.passwordTextBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(228, 21);
            this.passwordTextBox.TabIndex = 4;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(50, 132);
            this.passwordLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(57, 13);
            this.passwordLabel.TabIndex = 5;
            this.passwordLabel.Text = "Password:";
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(50, 106);
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
            this.credentialsOptionalLabel.Location = new System.Drawing.Point(30, 75);
            this.credentialsOptionalLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.credentialsOptionalLabel.Name = "credentialsOptionalLabel";
            this.credentialsOptionalLabel.Size = new System.Drawing.Size(255, 13);
            this.credentialsOptionalLabel.TabIndex = 7;
            this.credentialsOptionalLabel.Text = "Typically optional, but required by some databases.";
            // 
            // OdbcDataSourceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.credentialsOptionalLabel);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.dataSourceComboBox);
            this.Controls.Add(this.dataSourceLabel);
            this.Controls.Add(this.selectDataSourceLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "OdbcDataSourceControl";
            this.Size = new System.Drawing.Size(433, 209);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label selectDataSourceLabel;
        private System.Windows.Forms.Label dataSourceLabel;
        private System.Windows.Forms.ComboBox dataSourceComboBox;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Label credentialsOptionalLabel;
    }
}
