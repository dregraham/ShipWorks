namespace ShipWorks.Stores.Platforms.Miva.WizardPages
{
    partial class MivaModuleUrlPage
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
            this.labelAutoFind = new System.Windows.Forms.Label();
            this.radioFindUrl = new System.Windows.Forms.RadioButton();
            this.labelWebsite = new System.Windows.Forms.Label();
            this.website = new System.Windows.Forms.TextBox();
            this.labelVersion = new System.Windows.Forms.Label();
            this.mivaVersion = new System.Windows.Forms.TextBox();
            this.labelMivaVersionExample = new System.Windows.Forms.Label();
            this.radioSpecifyUrl = new System.Windows.Forms.RadioButton();
            this.moduleUrl = new System.Windows.Forms.TextBox();
            this.labelModuleUrl = new System.Windows.Forms.Label();
            this.specifiedUseSecure = new System.Windows.Forms.CheckBox();
            this.panelSpecifyUrl = new System.Windows.Forms.Panel();
            this.panelFindUrl = new System.Windows.Forms.Panel();
            this.findUseSecure = new System.Windows.Forms.CheckBox();
            this.infoTipSecure1 = new ShipWorks.UI.Controls.InfoTip();
            this.infoTipSecure2 = new ShipWorks.UI.Controls.InfoTip();
            this.panelSpecifyUrl.SuspendLayout();
            this.panelFindUrl.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelAutoFind
            // 
            this.labelAutoFind.Location = new System.Drawing.Point(19, 12);
            this.labelAutoFind.Name = "labelAutoFind";
            this.labelAutoFind.Size = new System.Drawing.Size(451, 34);
            this.labelAutoFind.TabIndex = 0;
            this.labelAutoFind.Text = "ShipWorks can try to find the ShipWorks Miva Module, or if you already know the U" +
                "RL you can enter it manually.";
            // 
            // radioFindUrl
            // 
            this.radioFindUrl.AutoSize = true;
            this.radioFindUrl.Checked = true;
            this.radioFindUrl.Location = new System.Drawing.Point(38, 49);
            this.radioFindUrl.Name = "radioFindUrl";
            this.radioFindUrl.Size = new System.Drawing.Size(162, 17);
            this.radioFindUrl.TabIndex = 1;
            this.radioFindUrl.TabStop = true;
            this.radioFindUrl.Text = "Help me find the module URL";
            this.radioFindUrl.UseVisualStyleBackColor = true;
            this.radioFindUrl.CheckedChanged += new System.EventHandler(this.OnChoiceChanged);
            // 
            // labelWebsite
            // 
            this.labelWebsite.AutoSize = true;
            this.labelWebsite.Location = new System.Drawing.Point(3, 6);
            this.labelWebsite.Name = "labelWebsite";
            this.labelWebsite.Size = new System.Drawing.Size(73, 13);
            this.labelWebsite.TabIndex = 0;
            this.labelWebsite.Text = "Your website:";
            // 
            // website
            // 
            this.website.Location = new System.Drawing.Point(82, 3);
            this.website.Name = "website";
            this.website.Size = new System.Drawing.Size(333, 21);
            this.website.TabIndex = 1;
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(5, 55);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(71, 13);
            this.labelVersion.TabIndex = 3;
            this.labelVersion.Text = "Miva version:";
            // 
            // mivaVersion
            // 
            this.mivaVersion.Location = new System.Drawing.Point(82, 52);
            this.mivaVersion.Name = "mivaVersion";
            this.mivaVersion.Size = new System.Drawing.Size(82, 21);
            this.mivaVersion.TabIndex = 4;
            this.mivaVersion.Text = "5.00";
            // 
            // labelMivaVersionExample
            // 
            this.labelMivaVersionExample.AutoSize = true;
            this.labelMivaVersionExample.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelMivaVersionExample.Location = new System.Drawing.Point(79, 76);
            this.labelMivaVersionExample.Name = "labelMivaVersionExample";
            this.labelMivaVersionExample.Size = new System.Drawing.Size(147, 13);
            this.labelMivaVersionExample.TabIndex = 5;
            this.labelMivaVersionExample.Text = "(Examples: 4.14, 4.22, 5.00)";
            // 
            // radioSpecifyUrl
            // 
            this.radioSpecifyUrl.AutoSize = true;
            this.radioSpecifyUrl.Location = new System.Drawing.Point(38, 177);
            this.radioSpecifyUrl.Name = "radioSpecifyUrl";
            this.radioSpecifyUrl.Size = new System.Drawing.Size(243, 17);
            this.radioSpecifyUrl.TabIndex = 3;
            this.radioSpecifyUrl.Text = "I know the URL of my ShipWorks Miva Module";
            this.radioSpecifyUrl.UseVisualStyleBackColor = true;
            this.radioSpecifyUrl.CheckedChanged += new System.EventHandler(this.OnChoiceChanged);
            // 
            // moduleUrl
            // 
            this.moduleUrl.Location = new System.Drawing.Point(76, 2);
            this.moduleUrl.Name = "moduleUrl";
            this.moduleUrl.Size = new System.Drawing.Size(333, 21);
            this.moduleUrl.TabIndex = 1;
            // 
            // labelModuleUrl
            // 
            this.labelModuleUrl.AutoSize = true;
            this.labelModuleUrl.Location = new System.Drawing.Point(3, 5);
            this.labelModuleUrl.Name = "labelModuleUrl";
            this.labelModuleUrl.Size = new System.Drawing.Size(67, 13);
            this.labelModuleUrl.TabIndex = 0;
            this.labelModuleUrl.Text = "Module URL:";
            // 
            // specifiedUseSecure
            // 
            this.specifiedUseSecure.AutoSize = true;
            this.specifiedUseSecure.Checked = true;
            this.specifiedUseSecure.CheckState = System.Windows.Forms.CheckState.Checked;
            this.specifiedUseSecure.Location = new System.Drawing.Point(77, 27);
            this.specifiedUseSecure.Name = "specifiedUseSecure";
            this.specifiedUseSecure.Size = new System.Drawing.Size(178, 17);
            this.specifiedUseSecure.TabIndex = 2;
            this.specifiedUseSecure.Text = "Use secure https:// connection.";
            this.specifiedUseSecure.UseVisualStyleBackColor = true;
            // 
            // panelSpecifyUrl
            // 
            this.panelSpecifyUrl.Controls.Add(this.infoTipSecure2);
            this.panelSpecifyUrl.Controls.Add(this.labelModuleUrl);
            this.panelSpecifyUrl.Controls.Add(this.moduleUrl);
            this.panelSpecifyUrl.Controls.Add(this.specifiedUseSecure);
            this.panelSpecifyUrl.Enabled = false;
            this.panelSpecifyUrl.Location = new System.Drawing.Point(53, 197);
            this.panelSpecifyUrl.Name = "panelSpecifyUrl";
            this.panelSpecifyUrl.Size = new System.Drawing.Size(425, 56);
            this.panelSpecifyUrl.TabIndex = 4;
            // 
            // panelFindUrl
            // 
            this.panelFindUrl.Controls.Add(this.infoTipSecure1);
            this.panelFindUrl.Controls.Add(this.findUseSecure);
            this.panelFindUrl.Controls.Add(this.labelWebsite);
            this.panelFindUrl.Controls.Add(this.website);
            this.panelFindUrl.Controls.Add(this.labelMivaVersionExample);
            this.panelFindUrl.Controls.Add(this.labelVersion);
            this.panelFindUrl.Controls.Add(this.mivaVersion);
            this.panelFindUrl.Location = new System.Drawing.Point(52, 66);
            this.panelFindUrl.Name = "panelFindUrl";
            this.panelFindUrl.Size = new System.Drawing.Size(424, 103);
            this.panelFindUrl.TabIndex = 2;
            // 
            // findUseSecure
            // 
            this.findUseSecure.AutoSize = true;
            this.findUseSecure.Checked = true;
            this.findUseSecure.CheckState = System.Windows.Forms.CheckState.Checked;
            this.findUseSecure.Location = new System.Drawing.Point(83, 28);
            this.findUseSecure.Name = "findUseSecure";
            this.findUseSecure.Size = new System.Drawing.Size(178, 17);
            this.findUseSecure.TabIndex = 2;
            this.findUseSecure.Text = "Use secure https:// connection.";
            this.findUseSecure.UseVisualStyleBackColor = true;
            // 
            // infoTipSecure1
            // 
            this.infoTipSecure1.Caption = "ShipWorks will not download payment details over an unsecure connection.";
            this.infoTipSecure1.Location = new System.Drawing.Point(257, 30);
            this.infoTipSecure1.Name = "infoTipSecure1";
            this.infoTipSecure1.Size = new System.Drawing.Size(12, 12);
            this.infoTipSecure1.TabIndex = 23;
            this.infoTipSecure1.Title = "Secure Connection";
            // 
            // infoTipSecure2
            // 
            this.infoTipSecure2.Caption = "ShipWorks will not download payment details over an unsecure connection.";
            this.infoTipSecure2.Location = new System.Drawing.Point(253, 29);
            this.infoTipSecure2.Name = "infoTipSecure2";
            this.infoTipSecure2.Size = new System.Drawing.Size(12, 12);
            this.infoTipSecure2.TabIndex = 24;
            this.infoTipSecure2.Title = "Secure Connection";
            // 
            // MivaModuleUrlPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelFindUrl);
            this.Controls.Add(this.panelSpecifyUrl);
            this.Controls.Add(this.radioSpecifyUrl);
            this.Controls.Add(this.radioFindUrl);
            this.Controls.Add(this.labelAutoFind);
            this.Name = "MivaModuleUrlPage";
            this.Size = new System.Drawing.Size(489, 280);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.panelSpecifyUrl.ResumeLayout(false);
            this.panelSpecifyUrl.PerformLayout();
            this.panelFindUrl.ResumeLayout(false);
            this.panelFindUrl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAutoFind;
        private System.Windows.Forms.RadioButton radioFindUrl;
        private System.Windows.Forms.Label labelWebsite;
        private System.Windows.Forms.TextBox website;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.TextBox mivaVersion;
        private System.Windows.Forms.Label labelMivaVersionExample;
        private System.Windows.Forms.RadioButton radioSpecifyUrl;
        private System.Windows.Forms.TextBox moduleUrl;
        private System.Windows.Forms.Label labelModuleUrl;
        private System.Windows.Forms.CheckBox specifiedUseSecure;
        private System.Windows.Forms.Panel panelSpecifyUrl;
        private System.Windows.Forms.Panel panelFindUrl;
        private System.Windows.Forms.CheckBox findUseSecure;
        private UI.Controls.InfoTip infoTipSecure2;
        private UI.Controls.InfoTip infoTipSecure1;
    }
}
