namespace ShipWorks.Stores.Platforms.ProStores.WizardPages
{
    partial class ProStoresManualSettingsPage
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
            this.labelFailed = new System.Windows.Forms.Label();
            this.labelServerSettingsInfo = new System.Windows.Forms.Label();
            this.radioExists = new System.Windows.Forms.RadioButton();
            this.apiEntryPoint = new System.Windows.Forms.TextBox();
            this.labelApiEntryPoint = new System.Windows.Forms.Label();
            this.radioNotExists = new System.Windows.Forms.RadioButton();
            this.labelInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelFailed
            // 
            this.labelFailed.AutoSize = true;
            this.labelFailed.Location = new System.Drawing.Point(19, 12);
            this.labelFailed.Name = "labelFailed";
            this.labelFailed.Size = new System.Drawing.Size(473, 13);
            this.labelFailed.TabIndex = 0;
            this.labelFailed.Text = "ShipWorks could not automatically determine your ProStores settings from the URL " +
                "you provided.";
            // 
            // labelServerSettingsInfo
            // 
            this.labelServerSettingsInfo.Location = new System.Drawing.Point(19, 37);
            this.labelServerSettingsInfo.Name = "labelServerSettingsInfo";
            this.labelServerSettingsInfo.Size = new System.Drawing.Size(461, 35);
            this.labelServerSettingsInfo.TabIndex = 1;
            this.labelServerSettingsInfo.Text = "Log on to your ProStores Store Administration and navigate to Store Settings -> S" +
                "erver.  Under \"URL Information\", look for a section called \"API Entry Point\".";
            // 
            // radioExists
            // 
            this.radioExists.AutoSize = true;
            this.radioExists.Checked = true;
            this.radioExists.Location = new System.Drawing.Point(40, 75);
            this.radioExists.Name = "radioExists";
            this.radioExists.Size = new System.Drawing.Size(269, 17);
            this.radioExists.TabIndex = 2;
            this.radioExists.TabStop = true;
            this.radioExists.Text = "The section exists, and contains the following URL:";
            this.radioExists.UseVisualStyleBackColor = true;
            this.radioExists.CheckedChanged += new System.EventHandler(this.OnRadioChanged);
            // 
            // apiEntryPoint
            // 
            this.apiEntryPoint.Location = new System.Drawing.Point(151, 96);
            this.apiEntryPoint.Name = "apiEntryPoint";
            this.apiEntryPoint.Size = new System.Drawing.Size(365, 21);
            this.apiEntryPoint.TabIndex = 5;
            // 
            // labelApiEntryPoint
            // 
            this.labelApiEntryPoint.AutoSize = true;
            this.labelApiEntryPoint.Location = new System.Drawing.Point(61, 99);
            this.labelApiEntryPoint.Name = "labelApiEntryPoint";
            this.labelApiEntryPoint.Size = new System.Drawing.Size(84, 13);
            this.labelApiEntryPoint.TabIndex = 4;
            this.labelApiEntryPoint.Text = "API Entry Point:";
            // 
            // radioNotExists
            // 
            this.radioNotExists.AutoSize = true;
            this.radioNotExists.Location = new System.Drawing.Point(40, 124);
            this.radioNotExists.Name = "radioNotExists";
            this.radioNotExists.Size = new System.Drawing.Size(203, 17);
            this.radioNotExists.TabIndex = 6;
            this.radioNotExists.Text = "There is no \"API Entry Point\" section.";
            this.radioNotExists.UseVisualStyleBackColor = true;
            this.radioNotExists.CheckedChanged += new System.EventHandler(this.OnRadioChanged);
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelInfo.Location = new System.Drawing.Point(55, 144);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(453, 13);
            this.labelInfo.TabIndex = 7;
            this.labelInfo.Text = "(This means you are on an older version of ProStores that does not support status" +
                " updates.)";
            // 
            // ProStoresManualSettingsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.radioNotExists);
            this.Controls.Add(this.apiEntryPoint);
            this.Controls.Add(this.labelApiEntryPoint);
            this.Controls.Add(this.radioExists);
            this.Controls.Add(this.labelServerSettingsInfo);
            this.Controls.Add(this.labelFailed);
            this.Name = "ProStoresManualSettingsPage";
            this.Size = new System.Drawing.Size(554, 267);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFailed;
        private System.Windows.Forms.Label labelServerSettingsInfo;
        private System.Windows.Forms.RadioButton radioExists;
        private System.Windows.Forms.TextBox apiEntryPoint;
        private System.Windows.Forms.Label labelApiEntryPoint;
        private System.Windows.Forms.RadioButton radioNotExists;
        private System.Windows.Forms.Label labelInfo;
    }
}
