﻿namespace ShipWorks.Stores.Platforms.Choxi
{
    partial class ChoxiStoreAccountSettingsControl
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
            this.vendorId = new System.Windows.Forms.TextBox();
            this.useTestServer = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.helpLink = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.helpLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(418, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the administrator username and password you use to login to your online sto" +
    "re:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Username:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(66, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Password:";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(131, 26);
            this.username.MaxLength = 50;
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(230, 21);
            this.username.TabIndex = 1;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(131, 52);
            this.password.MaxLength = 50;
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(230, 21);
            this.password.TabIndex = 2;
            this.password.UseSystemPasswordChar = true;
            // 
            // vendorId
            // 
            this.vendorId.Location = new System.Drawing.Point(131, 79);
            this.vendorId.MaxLength = 50;
            this.vendorId.Name = "vendorId";
            this.vendorId.Size = new System.Drawing.Size(230, 21);
            this.vendorId.TabIndex = 3;
            // 
            // useTestServer
            // 
            this.useTestServer.AutoSize = true;
            this.useTestServer.Location = new System.Drawing.Point(19, 104);
            this.useTestServer.Name = "useTestServer";
            this.useTestServer.Size = new System.Drawing.Size(103, 17);
            this.useTestServer.TabIndex = 6;
            this.useTestServer.Text = "Use Test Server";
            this.useTestServer.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(64, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Vendor ID:";
            // 
            // helpLink
            // 
            this.helpLink.AutoSize = true;
            this.helpLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.helpLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.helpLink.ForeColor = System.Drawing.Color.Blue;
            this.helpLink.Location = new System.Drawing.Point(331, 103);
            this.helpLink.Name = "helpLink";
            this.helpLink.Size = new System.Drawing.Size(55, 18);
            this.helpLink.TabIndex = 5;
            this.helpLink.TabStop = true;
            this.helpLink.Text = "click here.";
            this.helpLink.Url = "http://www.interapptive.com/shipworks/help";
            this.helpLink.UseCompatibleTextRendering = true;
            // 
            // helpLabel
            // 
            this.helpLabel.Location = new System.Drawing.Point(128, 103);
            this.helpLabel.Name = "helpLabel";
            this.helpLabel.Size = new System.Drawing.Size(223, 29);
            this.helpLabel.TabIndex = 9;
            this.helpLabel.Text = "For help with adding Choxi (NoMoreRack) ";
            // 
            // ChoxiStoreAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.helpLink);
            this.Controls.Add(this.helpLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.vendorId);
            this.Controls.Add(this.useTestServer);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ChoxiStoreAccountSettingsControl";
            this.Size = new System.Drawing.Size(482, 135);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.CheckBox useTestServer;
        private System.Windows.Forms.TextBox vendorId;
        private System.Windows.Forms.Label label4;
        private ApplicationCore.Interaction.HelpLink helpLink;
        private System.Windows.Forms.Label helpLabel;
    }
}
