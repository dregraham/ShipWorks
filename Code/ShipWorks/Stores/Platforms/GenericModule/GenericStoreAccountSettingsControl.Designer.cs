namespace ShipWorks.Stores.Platforms.GenericModule
{
    partial class GenericStoreAccountSettingsControl
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
            this.helpLink = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.label4 = new System.Windows.Forms.Label();
            this.moduleUrl = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
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
            this.label2.Location = new System.Drawing.Point(30, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username:";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password:";
            //
            // username
            //
            this.username.Location = new System.Drawing.Point(97, 26);
            this.username.MaxLength = 50;
            this.fieldLengthProvider.SetMaxLengthSource(this.username, ShipWorks.Data.Utility.EntityFieldLengthSource.GenericUsername);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(230, 21);
            this.username.TabIndex = 3;
            //
            // password
            //
            this.password.Location = new System.Drawing.Point(97, 52);
            this.password.MaxLength = 50;
            this.fieldLengthProvider.SetMaxLengthSource(this.password, ShipWorks.Data.Utility.EntityFieldLengthSource.GenericPassword);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(230, 21);
            this.password.TabIndex = 4;
            this.password.UseSystemPasswordChar = true;
            //
            // helpLink
            //
            this.helpLink.AutoSize = true;
            this.helpLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.helpLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.helpLink.ForeColor = System.Drawing.Color.Blue;
            this.helpLink.Location = new System.Drawing.Point(39, 105);
            this.helpLink.Name = "helpLink";
            this.helpLink.Size = new System.Drawing.Size(55, 18);
            this.helpLink.TabIndex = 5;
            this.helpLink.TabStop = true;
            this.helpLink.Text = "click here.";
            this.helpLink.UseCompatibleTextRendering = true;
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Module URL:";
            //
            // moduleUrl
            //
            this.moduleUrl.Location = new System.Drawing.Point(97, 139);
            this.fieldLengthProvider.SetMaxLengthSource(this.moduleUrl, ShipWorks.Data.Utility.EntityFieldLengthSource.GenericModuleUrl);
            this.moduleUrl.Name = "moduleUrl";
            this.moduleUrl.Size = new System.Drawing.Size(367, 21);
            this.moduleUrl.TabIndex = 8;
            //
            // label5
            //
            this.label5.Location = new System.Drawing.Point(11, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(410, 29);
            this.label5.TabIndex = 9;
            this.label5.Text = "Enter the URL to the ShipWorks module.  For help installing or finding the module" +
    " URL,";
            //
            // GenericStoreAccountSettingsControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.moduleUrl);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.helpLink);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Name = "GenericStoreAccountSettingsControl";
            this.Size = new System.Drawing.Size(482, 188);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        protected ShipWorks.ApplicationCore.Interaction.HelpLink helpLink;
        protected System.Windows.Forms.Label label4;
        protected System.Windows.Forms.TextBox moduleUrl;
        protected System.Windows.Forms.Label label5;
        protected System.Windows.Forms.Label label1;
        protected System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Label label3;
        protected System.Windows.Forms.TextBox username;
        protected System.Windows.Forms.TextBox password;
    }
}
