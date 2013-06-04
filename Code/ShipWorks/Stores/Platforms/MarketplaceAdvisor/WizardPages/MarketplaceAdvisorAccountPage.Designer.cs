namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.WizardPages
{
    partial class MarketplaceAdvisorAccountPage
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
            this.password = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.labelEnterCredentials = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radioOMS = new System.Windows.Forms.RadioButton();
            this.radioStandard = new System.Windows.Forms.RadioButton();
            this.radioCorporate = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(105, 62);
            this.password.MaxLength = 50;
            this.password.Name = "password";
            this.password.UseSystemPasswordChar = true;
            this.password.Size = new System.Drawing.Size(230, 21);
            this.password.TabIndex = 9;
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(105, 36);
            this.username.MaxLength = 50;
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(230, 21);
            this.username.TabIndex = 7;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(40, 65);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(57, 13);
            this.labelPassword.TabIndex = 8;
            this.labelPassword.Text = "Password:";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(38, 39);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(59, 13);
            this.labelUsername.TabIndex = 6;
            this.labelUsername.Text = "Username:";
            // 
            // labelEnterCredentials
            // 
            this.labelEnterCredentials.AutoSize = true;
            this.labelEnterCredentials.Location = new System.Drawing.Point(19, 12);
            this.labelEnterCredentials.Name = "labelEnterCredentials";
            this.labelEnterCredentials.Size = new System.Drawing.Size(246, 13);
            this.labelEnterCredentials.TabIndex = 5;
            this.labelEnterCredentials.Text = "Enter your MarketplaceAdvisor username and password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(248, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Select the MarketplaceAdvisor server your account uses:";
            // 
            // radioOMS
            // 
            this.radioOMS.AutoSize = true;
            this.radioOMS.Checked = true;
            this.radioOMS.Location = new System.Drawing.Point(41, 123);
            this.radioOMS.Name = "radioOMS";
            this.radioOMS.Size = new System.Drawing.Size(189, 17);
            this.radioOMS.TabIndex = 11;
            this.radioOMS.TabStop = true;
            this.radioOMS.Text = "Order Management System (OMS)";
            this.radioOMS.UseVisualStyleBackColor = true;
            // 
            // radioStandard
            // 
            this.radioStandard.AutoSize = true;
            this.radioStandard.Location = new System.Drawing.Point(41, 146);
            this.radioStandard.Name = "radioStandard";
            this.radioStandard.Size = new System.Drawing.Size(148, 17);
            this.radioStandard.TabIndex = 12;
            this.radioStandard.TabStop = true;
            this.radioStandard.Text = "Legacy - Standard Server";
            this.radioStandard.UseVisualStyleBackColor = true;
            // 
            // radioCorporate
            // 
            this.radioCorporate.AutoSize = true;
            this.radioCorporate.Location = new System.Drawing.Point(41, 169);
            this.radioCorporate.Name = "radioCorporate";
            this.radioCorporate.Size = new System.Drawing.Size(153, 17);
            this.radioCorporate.TabIndex = 13;
            this.radioCorporate.TabStop = true;
            this.radioCorporate.Text = "Legacy - Corporate Server";
            this.radioCorporate.UseVisualStyleBackColor = true;
            // 
            // MarketplaceAdvisorAccountPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radioCorporate);
            this.Controls.Add(this.radioStandard);
            this.Controls.Add(this.radioOMS);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.labelEnterCredentials);
            this.Description = "Enter the following information about your online store.";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "MarketplaceAdvisorAccountPage";
            this.Size = new System.Drawing.Size(536, 264);
            this.Title = "Store Setup";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelEnterCredentials;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioOMS;
        private System.Windows.Forms.RadioButton radioStandard;
        private System.Windows.Forms.RadioButton radioCorporate;
    }
}
