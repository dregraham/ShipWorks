namespace ShipWorks.Stores.Platforms.SellerVantage
{
    partial class SellerVantageAccountSettingsControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.clientTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.url = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(371, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the username and password of the administrator of your online store:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password:";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(140, 25);
            this.username.MaxLength = 50;
            this.fieldLengthProvider.SetMaxLengthSource(this.username, ShipWorks.Data.Utility.EntityFieldLengthSource.GenericUsername);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(230, 21);
            this.username.TabIndex = 3;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(140, 51);
            this.password.MaxLength = 50;
            this.fieldLengthProvider.SetMaxLengthSource(this.password, ShipWorks.Data.Utility.EntityFieldLengthSource.GenericPassword);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(230, 21);
            this.password.TabIndex = 4;
            this.password.UseSystemPasswordChar = true;
            // 
            // clientTextBox
            // 
            this.clientTextBox.Location = new System.Drawing.Point(140, 77);
            this.clientTextBox.MaxLength = 50;
            this.fieldLengthProvider.SetMaxLengthSource(this.clientTextBox, ShipWorks.Data.Utility.EntityFieldLengthSource.GenericPassword);
            this.clientTextBox.Name = "clientTextBox";
            this.clientTextBox.Size = new System.Drawing.Size(230, 21);
            this.clientTextBox.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "&SellerVantage Client:";
            // 
            // url
            // 
            this.url.Location = new System.Drawing.Point(140, 104);
            this.url.MaxLength = 50;
            this.fieldLengthProvider.SetMaxLengthSource(this.url, ShipWorks.Data.Utility.EntityFieldLengthSource.GenericPassword);
            this.url.Name = "url";
            this.url.Size = new System.Drawing.Size(230, 21);
            this.url.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(102, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "URL:";
            // 
            // SellerVantageAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.url);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.clientTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SellerVantageAccountSettingsControl";
            this.Size = new System.Drawing.Size(482, 283);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox clientTextBox;
        private System.Windows.Forms.TextBox url;
        private System.Windows.Forms.Label label5;
    }
}
