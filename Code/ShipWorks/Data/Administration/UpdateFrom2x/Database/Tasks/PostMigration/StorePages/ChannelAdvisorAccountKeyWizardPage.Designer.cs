namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.StorePages
{
    partial class ChannelAdvisorAccountKeyWizardPage
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
            this.skipCheckBox = new System.Windows.Forms.CheckBox();
            this.storeNameLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.accountKey = new System.Windows.Forms.TextBox();
            this.labelAccountKey = new System.Windows.Forms.Label();
            this.accessFakeLinkLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // skipCheckBox
            // 
            this.skipCheckBox.AutoSize = true;
            this.skipCheckBox.Location = new System.Drawing.Point(24, 97);
            this.skipCheckBox.Name = "skipCheckBox";
            this.skipCheckBox.Size = new System.Drawing.Size(208, 17);
            this.skipCheckBox.TabIndex = 20;
            this.skipCheckBox.Text = "I will configure this after the upgrade.";
            this.skipCheckBox.UseVisualStyleBackColor = true;
            this.skipCheckBox.Click += new System.EventHandler(this.OnSkipChanged);
            // 
            // storeNameLabel
            // 
            this.storeNameLabel.AutoSize = true;
            this.storeNameLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeNameLabel.Location = new System.Drawing.Point(20, 77);
            this.storeNameLabel.Name = "storeNameLabel";
            this.storeNameLabel.Size = new System.Drawing.Size(129, 13);
            this.storeNameLabel.TabIndex = 19;
            this.storeNameLabel.Text = "ChannelAdvisor Store";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(78, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(365, 32);
            this.label1.TabIndex = 18;
            this.label1.Text = "Your ChannelAdvisor store is missing the Account Key necessary for download orde" +
                "rs.";
            // 
            // accountKey
            // 
            this.accountKey.Location = new System.Drawing.Point(118, 120);
            this.accountKey.Name = "accountKey";
            this.accountKey.Size = new System.Drawing.Size(325, 21);
            this.accountKey.TabIndex = 22;
            // 
            // labelAccountKey
            // 
            this.labelAccountKey.AutoSize = true;
            this.labelAccountKey.Location = new System.Drawing.Point(38, 124);
            this.labelAccountKey.Name = "labelAccountKey";
            this.labelAccountKey.Size = new System.Drawing.Size(71, 13);
            this.labelAccountKey.TabIndex = 21;
            this.labelAccountKey.Text = "Account Key:";
            // 
            // accessFakeLinkLabel
            // 
            this.accessFakeLinkLabel.AutoSize = true;
            this.accessFakeLinkLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.accessFakeLinkLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accessFakeLinkLabel.ForeColor = System.Drawing.Color.RoyalBlue;
            this.accessFakeLinkLabel.Location = new System.Drawing.Point(115, 145);
            this.accessFakeLinkLabel.Name = "accessFakeLinkLabel";
            this.accessFakeLinkLabel.Size = new System.Drawing.Size(53, 13);
            this.accessFakeLinkLabel.TabIndex = 23;
            this.accessFakeLinkLabel.Text = "Click here";
            this.accessFakeLinkLabel.Click += new System.EventHandler(this.OnClickGrantAccess);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label2.Location = new System.Drawing.Point(166, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "to request an Account Key.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.school;
            this.pictureBox1.Location = new System.Drawing.Point(24, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // ChannelAdvisorAccountKeyWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.accountKey);
            this.Controls.Add(this.labelAccountKey);
            this.Controls.Add(this.accessFakeLinkLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.skipCheckBox);
            this.Controls.Add(this.storeNameLabel);
            this.Controls.Add(this.label1);
            this.Description = "ShipWorks needs more information about your ChannelAdvisor store.";
            this.Name = "ChannelAdvisorAccountKeyWizardPage";
            this.Size = new System.Drawing.Size(500, 300);
            this.Title = "ChannelAdvisor Account Key";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox skipCheckBox;
        private System.Windows.Forms.Label storeNameLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox accountKey;
        private System.Windows.Forms.Label labelAccountKey;
        private System.Windows.Forms.Label accessFakeLinkLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
