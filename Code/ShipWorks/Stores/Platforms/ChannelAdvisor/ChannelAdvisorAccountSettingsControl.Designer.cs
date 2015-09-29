namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    partial class ChannelAdvisorAccountSettingsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelAdvisorAccountSettingsControl));
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.labelProfileID = new System.Windows.Forms.Label();
            this.labelSectionProfileID = new System.Windows.Forms.Label();
            this.profileId = new System.Windows.Forms.TextBox();
            this.linkHelpProfileID = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.statusLabel = new System.Windows.Forms.Label();
            this.statusText = new System.Windows.Forms.Label();
            this.statusPicture = new System.Windows.Forms.PictureBox();
            this.accessPanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.caLinkControl = new ShipWorks.UI.Controls.LinkControl();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.requestAccessButton = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusPicture)).BeginInit();
            this.accessPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelProfileID
            // 
            this.labelProfileID.AutoSize = true;
            this.labelProfileID.Location = new System.Drawing.Point(45, 35);
            this.labelProfileID.Name = "labelProfileID";
            this.labelProfileID.Size = new System.Drawing.Size(55, 13);
            this.labelProfileID.TabIndex = 1;
            this.labelProfileID.Text = "Profile ID:";
            // 
            // labelSectionProfileID
            // 
            this.labelSectionProfileID.AutoSize = true;
            this.labelSectionProfileID.Location = new System.Drawing.Point(7, 7);
            this.labelSectionProfileID.Name = "labelSectionProfileID";
            this.labelSectionProfileID.Size = new System.Drawing.Size(394, 13);
            this.labelSectionProfileID.TabIndex = 0;
            this.labelSectionProfileID.Text = "Enter your ChannelAdvisor Profile ID and your account username and password.";
            // 
            // profileId
            // 
            this.profileId.Location = new System.Drawing.Point(109, 32);
            this.profileId.Name = "profileId";
            this.profileId.Size = new System.Drawing.Size(102, 21);
            this.profileId.TabIndex = 2;
            // 
            // linkHelpProfileID
            // 
            this.linkHelpProfileID.AutoSize = true;
            this.linkHelpProfileID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkHelpProfileID.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkHelpProfileID.ForeColor = System.Drawing.Color.RoyalBlue;
            this.linkHelpProfileID.Location = new System.Drawing.Point(215, 35);
            this.linkHelpProfileID.Name = "linkHelpProfileID";
            this.linkHelpProfileID.Size = new System.Drawing.Size(107, 13);
            this.linkHelpProfileID.TabIndex = 3;
            this.linkHelpProfileID.Text = "Where do I find this?";
            this.linkHelpProfileID.Url = "http://support.shipworks.com/support/solutions/articles/4000026252";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(58, 63);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(42, 13);
            this.statusLabel.TabIndex = 8;
            this.statusLabel.Text = "Status:";
            this.statusLabel.Visible = false;
            // 
            // statusText
            // 
            this.statusText.AutoSize = true;
            this.statusText.ForeColor = System.Drawing.Color.DimGray;
            this.statusText.Location = new System.Drawing.Point(132, 63);
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(243, 13);
            this.statusText.TabIndex = 18;
            this.statusText.Text = "Waiting for you to finish authorizing ShipWorks...";
            this.statusText.Visible = false;
            // 
            // statusPicture
            // 
            this.statusPicture.Image = ((System.Drawing.Image)(resources.GetObject("statusPicture.Image")));
            this.statusPicture.Location = new System.Drawing.Point(110, 62);
            this.statusPicture.Name = "statusPicture";
            this.statusPicture.Size = new System.Drawing.Size(16, 16);
            this.statusPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.statusPicture.TabIndex = 17;
            this.statusPicture.TabStop = false;
            this.statusPicture.Visible = false;
            // 
            // accessPanel
            // 
            this.accessPanel.Controls.Add(this.panel2);
            this.accessPanel.Controls.Add(this.caLinkControl);
            this.accessPanel.Controls.Add(this.label3);
            this.accessPanel.Controls.Add(this.label2);
            this.accessPanel.Controls.Add(this.requestAccessButton);
            this.accessPanel.Location = new System.Drawing.Point(10, 93);
            this.accessPanel.Name = "accessPanel";
            this.accessPanel.Size = new System.Drawing.Size(471, 120);
            this.accessPanel.TabIndex = 21;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(190, 81);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(278, 33);
            this.panel2.TabIndex = 4;
            // 
            // caLinkControl
            // 
            this.caLinkControl.AutoSize = true;
            this.caLinkControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.caLinkControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.caLinkControl.ForeColor = System.Drawing.Color.Blue;
            this.caLinkControl.Location = new System.Drawing.Point(4, 61);
            this.caLinkControl.Name = "caLinkControl";
            this.caLinkControl.Size = new System.Drawing.Size(152, 13);
            this.caLinkControl.TabIndex = 3;
            this.caLinkControl.Text = "Visit www.channeladvisor.com";
            this.caLinkControl.Click += new System.EventHandler(this.OnCALinkClick);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(464, 47);
            this.label3.TabIndex = 2;
            this.label3.Text = "Click Request Access and then login to your ChannelAdvisor account, go to My Acco" +
                "unt -> Developer Network -> Account Authorization and Enable the ShipWorks reque" +
                "st.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(367, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "ShipWorks needs to be authorized to access your ChannelAdvisor account.";
            // 
            // requestAccessButton
            // 
            this.requestAccessButton.Location = new System.Drawing.Point(45, 86);
            this.requestAccessButton.Name = "requestAccessButton";
            this.requestAccessButton.Size = new System.Drawing.Size(126, 23);
            this.requestAccessButton.TabIndex = 0;
            this.requestAccessButton.Text = "Request Access...";
            this.requestAccessButton.UseVisualStyleBackColor = true;
            this.requestAccessButton.Click += new System.EventHandler(this.OnRequestAccess);
            // 
            // timer
            // 
            this.timer.Interval = 5000;
            this.timer.Tick += new System.EventHandler(this.OnTimerTick);
            // 
            // ChannelAdvisorAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusText);
            this.Controls.Add(this.statusPicture);
            this.Controls.Add(this.accessPanel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.linkHelpProfileID);
            this.Controls.Add(this.profileId);
            this.Controls.Add(this.labelSectionProfileID);
            this.Controls.Add(this.labelProfileID);
            this.Name = "ChannelAdvisorAccountSettingsControl";
            this.Size = new System.Drawing.Size(501, 224);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusPicture)).EndInit();
            this.accessPanel.ResumeLayout(false);
            this.accessPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.Label labelProfileID;
        private System.Windows.Forms.Label labelSectionProfileID;
        private System.Windows.Forms.TextBox profileId;
        private ShipWorks.ApplicationCore.Interaction.HelpLink linkHelpProfileID;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label statusText;
        private System.Windows.Forms.PictureBox statusPicture;
        private System.Windows.Forms.Panel accessPanel;
        private UI.Controls.LinkControl caLinkControl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button requestAccessButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Timer timer;
    }
}
