namespace ShipWorks.Stores.Platforms.Volusion.WizardPages
{
    partial class VolusionAccountPage
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.labelConfiguration = new System.Windows.Forms.Label();
            this.configurationType = new System.Windows.Forms.ComboBox();
            this.autoConfigPanel = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.manualConfigPanel = new System.Windows.Forms.Panel();
            this.apiPasswordTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.linkHelpPassword = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.label8 = new System.Windows.Forms.Label();
            this.autoConfigPanel.SuspendLayout();
            this.manualConfigPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(354, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter your Volusion account information (requires a Gold plan or higher):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Store Url:";
            // 
            // urlTextBox
            // 
            this.urlTextBox.Location = new System.Drawing.Point(111, 40);
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(315, 21);
            this.urlTextBox.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Login Email:";
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(111, 65);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(253, 21);
            this.emailTextBox.TabIndex = 4;
            // 
            // labelConfiguration
            // 
            this.labelConfiguration.AutoSize = true;
            this.labelConfiguration.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelConfiguration.Location = new System.Drawing.Point(16, 99);
            this.labelConfiguration.Name = "labelConfiguration";
            this.labelConfiguration.Size = new System.Drawing.Size(83, 13);
            this.labelConfiguration.TabIndex = 5;
            this.labelConfiguration.Text = "Configuration";
            // 
            // configurationType
            // 
            this.configurationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.configurationType.FormattingEnabled = true;
            this.configurationType.Items.AddRange(new object[] {
            "Automatic (Recommended)",
            "Manual"});
            this.configurationType.Location = new System.Drawing.Point(41, 119);
            this.configurationType.Name = "configurationType";
            this.configurationType.Size = new System.Drawing.Size(179, 21);
            this.configurationType.TabIndex = 6;
            this.configurationType.SelectedIndexChanged += new System.EventHandler(this.OnConfigurationChanged);
            // 
            // autoConfigPanel
            // 
            this.autoConfigPanel.Controls.Add(this.label7);
            this.autoConfigPanel.Controls.Add(this.passwordTextBox);
            this.autoConfigPanel.Controls.Add(this.label6);
            this.autoConfigPanel.Controls.Add(this.label5);
            this.autoConfigPanel.Location = new System.Drawing.Point(42, 149);
            this.autoConfigPanel.Name = "autoConfigPanel";
            this.autoConfigPanel.Size = new System.Drawing.Size(448, 95);
            this.autoConfigPanel.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(356, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Click Next to start the autoconfiguration process.  This may take a while.";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(92, 37);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(192, 21);
            this.passwordTextBox.TabIndex = 2;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Password:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(-3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(460, 28);
            this.label5.TabIndex = 0;
            this.label5.Text = "To allow ShipWorks to automatically configure communication with Volusion, the ad" +
    "ministrator password for the Login Email entered above is required.";
            // 
            // manualConfigPanel
            // 
            this.manualConfigPanel.Controls.Add(this.apiPasswordTextBox);
            this.manualConfigPanel.Controls.Add(this.label9);
            this.manualConfigPanel.Controls.Add(this.linkHelpPassword);
            this.manualConfigPanel.Controls.Add(this.label8);
            this.manualConfigPanel.Location = new System.Drawing.Point(41, 249);
            this.manualConfigPanel.Name = "manualConfigPanel";
            this.manualConfigPanel.Size = new System.Drawing.Size(477, 84);
            this.manualConfigPanel.TabIndex = 8;
            this.manualConfigPanel.Visible = false;
            // 
            // apiPasswordTextBox
            // 
            this.apiPasswordTextBox.Location = new System.Drawing.Point(2, 53);
            this.apiPasswordTextBox.Name = "apiPasswordTextBox";
            this.apiPasswordTextBox.Size = new System.Drawing.Size(467, 21);
            this.apiPasswordTextBox.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(-3, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(151, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Volusion Encrypted Password:";
            // 
            // linkHelpPassword
            // 
            this.linkHelpPassword.AutoSize = true;
            this.linkHelpPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkHelpPassword.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkHelpPassword.ForeColor = System.Drawing.Color.Blue;
            this.linkHelpPassword.Location = new System.Drawing.Point(256, 13);
            this.linkHelpPassword.Name = "linkHelpPassword";
            this.linkHelpPassword.Size = new System.Drawing.Size(55, 13);
            this.linkHelpPassword.TabIndex = 1;
            this.linkHelpPassword.Text = "click here.";
            this.linkHelpPassword.Url = "http://support.shipworks.com/support/solutions/articles/129345-adding-a-volusion-" +
    "store-";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(-3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(470, 29);
            this.label8.TabIndex = 0;
            this.label8.Text = "Manual configuration requires your encrypted password, which is not the same as y" +
    "our login password.  For help finding your encrypted password";
            // 
            // VolusionAccountPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.manualConfigPanel);
            this.Controls.Add(this.autoConfigPanel);
            this.Controls.Add(this.configurationType);
            this.Controls.Add(this.labelConfiguration);
            this.Controls.Add(this.emailTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.urlTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "VolusionAccountPage";
            this.Size = new System.Drawing.Size(544, 377);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.autoConfigPanel.ResumeLayout(false);
            this.autoConfigPanel.PerformLayout();
            this.manualConfigPanel.ResumeLayout(false);
            this.manualConfigPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.Label labelConfiguration;
        private System.Windows.Forms.ComboBox configurationType;
        private System.Windows.Forms.Panel autoConfigPanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel manualConfigPanel;
        private System.Windows.Forms.Label label8;
        private ShipWorks.ApplicationCore.Interaction.HelpLink linkHelpPassword;
        private System.Windows.Forms.TextBox apiPasswordTextBox;
        private System.Windows.Forms.Label label9;
    }
}
