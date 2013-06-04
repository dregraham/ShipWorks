namespace ShipWorks.Email.Accounts
{
    partial class EmailAccountEditorDlg
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.accountSettings = new ShipWorks.Email.Accounts.EmailAccountSettingsControl();
            this.optionControl = new ShipWorks.UI.Controls.OptionControl();
            this.optionPageAccount = new ShipWorks.UI.Controls.OptionPage();
            this.optionPageDelivery = new ShipWorks.UI.Controls.OptionPage();
            this.infoTipLimits = new ShipWorks.UI.Controls.InfoTip();
            this.infoTipSending = new ShipWorks.UI.Controls.InfoTip();
            this.labelMessageInterval = new System.Windows.Forms.Label();
            this.messageInterval = new System.Windows.Forms.NumericUpDown();
            this.intervalLimit = new System.Windows.Forms.CheckBox();
            this.labelMessagesPerHour = new System.Windows.Forms.Label();
            this.messageHourLimit = new System.Windows.Forms.NumericUpDown();
            this.perHourLimit = new System.Windows.Forms.CheckBox();
            this.labelMessagesPerConnection = new System.Windows.Forms.Label();
            this.messageConnectionLimit = new System.Windows.Forms.NumericUpDown();
            this.perConnectionLimit = new System.Windows.Forms.CheckBox();
            this.sectionEmailLimits = new ShipWorks.UI.Controls.SectionTitle();
            this.sectionSending = new ShipWorks.UI.Controls.SectionTitle();
            this.labelRetryMinutes = new System.Windows.Forms.Label();
            this.autoSendMinutes = new System.Windows.Forms.NumericUpDown();
            this.autoSend = new System.Windows.Forms.CheckBox();
            this.optionControl.SuspendLayout();
            this.optionPageAccount.SuspendLayout();
            this.optionPageDelivery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.messageInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.messageHourLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.messageConnectionLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.autoSendMinutes)).BeginInit();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(346, 384);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(427, 384);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // accountSettings
            // 
            this.accountSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.accountSettings.Location = new System.Drawing.Point(1, 4);
            this.accountSettings.Name = "accountSettings";
            this.accountSettings.Size = new System.Drawing.Size(378, 355);
            this.accountSettings.TabIndex = 2;
            // 
            // optionControl
            // 
            this.optionControl.Controls.Add(this.optionPageAccount);
            this.optionControl.Controls.Add(this.optionPageDelivery);
            this.optionControl.Location = new System.Drawing.Point(12, 12);
            this.optionControl.MenuListWidth = 100;
            this.optionControl.Name = "optionControl";
            this.optionControl.SelectedIndex = 0;
            this.optionControl.Size = new System.Drawing.Size(488, 366);
            this.optionControl.TabIndex = 3;
            // 
            // optionPageAccount
            // 
            this.optionPageAccount.BackColor = System.Drawing.Color.White;
            this.optionPageAccount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.optionPageAccount.Controls.Add(this.accountSettings);
            this.optionPageAccount.Location = new System.Drawing.Point(103, 0);
            this.optionPageAccount.Name = "optionPageAccount";
            this.optionPageAccount.Padding = new System.Windows.Forms.Padding(3);
            this.optionPageAccount.Size = new System.Drawing.Size(385, 366);
            this.optionPageAccount.TabIndex = 1;
            this.optionPageAccount.Text = "Account";
            // 
            // optionPageDelivery
            // 
            this.optionPageDelivery.BackColor = System.Drawing.Color.White;
            this.optionPageDelivery.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.optionPageDelivery.Controls.Add(this.infoTipLimits);
            this.optionPageDelivery.Controls.Add(this.infoTipSending);
            this.optionPageDelivery.Controls.Add(this.labelMessageInterval);
            this.optionPageDelivery.Controls.Add(this.messageInterval);
            this.optionPageDelivery.Controls.Add(this.intervalLimit);
            this.optionPageDelivery.Controls.Add(this.labelMessagesPerHour);
            this.optionPageDelivery.Controls.Add(this.messageHourLimit);
            this.optionPageDelivery.Controls.Add(this.perHourLimit);
            this.optionPageDelivery.Controls.Add(this.labelMessagesPerConnection);
            this.optionPageDelivery.Controls.Add(this.messageConnectionLimit);
            this.optionPageDelivery.Controls.Add(this.perConnectionLimit);
            this.optionPageDelivery.Controls.Add(this.sectionEmailLimits);
            this.optionPageDelivery.Controls.Add(this.sectionSending);
            this.optionPageDelivery.Controls.Add(this.labelRetryMinutes);
            this.optionPageDelivery.Controls.Add(this.autoSendMinutes);
            this.optionPageDelivery.Controls.Add(this.autoSend);
            this.optionPageDelivery.Location = new System.Drawing.Point(103, 0);
            this.optionPageDelivery.Name = "optionPageDelivery";
            this.optionPageDelivery.Padding = new System.Windows.Forms.Padding(3);
            this.optionPageDelivery.Size = new System.Drawing.Size(385, 366);
            this.optionPageDelivery.TabIndex = 2;
            this.optionPageDelivery.Text = "Delivery";
            // 
            // infoTipLimits
            // 
            this.infoTipLimits.Caption = "Some email providers limit how many messages can be sent during each session.  Sh" +
                "ipWorks will log out and log back in when the limit is reached.";
            this.infoTipLimits.Location = new System.Drawing.Point(335, 96);
            this.infoTipLimits.Name = "infoTipLimits";
            this.infoTipLimits.Size = new System.Drawing.Size(12, 12);
            this.infoTipLimits.TabIndex = 20;
            this.infoTipLimits.Title = "Message Limit";
            // 
            // infoTipSending
            // 
            this.infoTipSending.Caption = "ShipWorks sends all emails immediately.  However, if emailing is canceled or erro" +
                "rs occur, some emails may be left unsent.";
            this.infoTipSending.Location = new System.Drawing.Point(356, 38);
            this.infoTipSending.Name = "infoTipSending";
            this.infoTipSending.Size = new System.Drawing.Size(12, 12);
            this.infoTipSending.TabIndex = 19;
            this.infoTipSending.Title = "Automatic Sending";
            // 
            // labelMessageInterval
            // 
            this.labelMessageInterval.AutoSize = true;
            this.labelMessageInterval.Location = new System.Drawing.Point(129, 147);
            this.labelMessageInterval.Name = "labelMessageInterval";
            this.labelMessageInterval.Size = new System.Drawing.Size(206, 13);
            this.labelMessageInterval.TabIndex = 18;
            this.labelMessageInterval.Text = "seconds between sending each message.";
            // 
            // messageInterval
            // 
            this.messageInterval.Location = new System.Drawing.Point(75, 145);
            this.messageInterval.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.messageInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.messageInterval.Name = "messageInterval";
            this.messageInterval.Size = new System.Drawing.Size(51, 21);
            this.messageInterval.TabIndex = 17;
            this.messageInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // intervalLimit
            // 
            this.intervalLimit.AutoSize = true;
            this.intervalLimit.Location = new System.Drawing.Point(22, 146);
            this.intervalLimit.Name = "intervalLimit";
            this.intervalLimit.Size = new System.Drawing.Size(55, 17);
            this.intervalLimit.TabIndex = 16;
            this.intervalLimit.Text = "Pause";
            this.intervalLimit.UseVisualStyleBackColor = true;
            this.intervalLimit.CheckedChanged += new System.EventHandler(this.OnChangeUsePause);
            // 
            // labelMessagesPerHour
            // 
            this.labelMessagesPerHour.AutoSize = true;
            this.labelMessagesPerHour.Location = new System.Drawing.Point(203, 122);
            this.labelMessagesPerHour.Name = "labelMessagesPerHour";
            this.labelMessagesPerHour.Size = new System.Drawing.Size(102, 13);
            this.labelMessagesPerHour.TabIndex = 15;
            this.labelMessagesPerHour.Text = "messages per hour.";
            // 
            // messageHourLimit
            // 
            this.messageHourLimit.Location = new System.Drawing.Point(149, 120);
            this.messageHourLimit.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.messageHourLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.messageHourLimit.Name = "messageHourLimit";
            this.messageHourLimit.Size = new System.Drawing.Size(51, 21);
            this.messageHourLimit.TabIndex = 14;
            this.messageHourLimit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // perHourLimit
            // 
            this.perHourLimit.AutoSize = true;
            this.perHourLimit.Location = new System.Drawing.Point(22, 121);
            this.perHourLimit.Name = "perHourLimit";
            this.perHourLimit.Size = new System.Drawing.Size(129, 17);
            this.perHourLimit.TabIndex = 13;
            this.perHourLimit.Text = "Don\'t send more than";
            this.perHourLimit.UseVisualStyleBackColor = true;
            this.perHourLimit.CheckedChanged += new System.EventHandler(this.OnChangeUseHourLimit);
            // 
            // labelMessagesPerConnection
            // 
            this.labelMessagesPerConnection.AutoSize = true;
            this.labelMessagesPerConnection.Location = new System.Drawing.Point(203, 95);
            this.labelMessagesPerConnection.Name = "labelMessagesPerConnection";
            this.labelMessagesPerConnection.Size = new System.Drawing.Size(132, 13);
            this.labelMessagesPerConnection.TabIndex = 10;
            this.labelMessagesPerConnection.Text = "messages per connection.";
            // 
            // messageConnectionLimit
            // 
            this.messageConnectionLimit.Location = new System.Drawing.Point(149, 93);
            this.messageConnectionLimit.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.messageConnectionLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.messageConnectionLimit.Name = "messageConnectionLimit";
            this.messageConnectionLimit.Size = new System.Drawing.Size(51, 21);
            this.messageConnectionLimit.TabIndex = 9;
            this.messageConnectionLimit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // perConnectionLimit
            // 
            this.perConnectionLimit.AutoSize = true;
            this.perConnectionLimit.Location = new System.Drawing.Point(22, 94);
            this.perConnectionLimit.Name = "perConnectionLimit";
            this.perConnectionLimit.Size = new System.Drawing.Size(129, 17);
            this.perConnectionLimit.TabIndex = 8;
            this.perConnectionLimit.Text = "Don\'t send more than";
            this.perConnectionLimit.UseVisualStyleBackColor = true;
            this.perConnectionLimit.CheckedChanged += new System.EventHandler(this.OnChangePerConnectionLimit);
            // 
            // sectionEmailLimits
            // 
            this.sectionEmailLimits.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionEmailLimits.Location = new System.Drawing.Point(6, 64);
            this.sectionEmailLimits.Name = "sectionEmailLimits";
            this.sectionEmailLimits.Size = new System.Drawing.Size(371, 22);
            this.sectionEmailLimits.TabIndex = 7;
            this.sectionEmailLimits.Text = "Email Provider Limits";
            // 
            // sectionSending
            // 
            this.sectionSending.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionSending.Location = new System.Drawing.Point(6, 6);
            this.sectionSending.Name = "sectionSending";
            this.sectionSending.Size = new System.Drawing.Size(371, 22);
            this.sectionSending.TabIndex = 6;
            this.sectionSending.Text = "Sending";
            // 
            // labelRetryMinutes
            // 
            this.labelRetryMinutes.AutoSize = true;
            this.labelRetryMinutes.Location = new System.Drawing.Point(308, 37);
            this.labelRetryMinutes.Name = "labelRetryMinutes";
            this.labelRetryMinutes.Size = new System.Drawing.Size(48, 13);
            this.labelRetryMinutes.TabIndex = 2;
            this.labelRetryMinutes.Text = "minutes.";
            // 
            // autoSendMinutes
            // 
            this.autoSendMinutes.Location = new System.Drawing.Point(252, 35);
            this.autoSendMinutes.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.autoSendMinutes.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.autoSendMinutes.Name = "autoSendMinutes";
            this.autoSendMinutes.Size = new System.Drawing.Size(54, 21);
            this.autoSendMinutes.TabIndex = 1;
            this.autoSendMinutes.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // autoSend
            // 
            this.autoSend.AutoSize = true;
            this.autoSend.Location = new System.Drawing.Point(22, 36);
            this.autoSend.Name = "autoSend";
            this.autoSend.Size = new System.Drawing.Size(236, 17);
            this.autoSend.TabIndex = 0;
            this.autoSend.Text = "Automatically send unsent messages every ";
            this.autoSend.UseVisualStyleBackColor = true;
            this.autoSend.CheckedChanged += new System.EventHandler(this.OnChangeAutoSend);
            // 
            // EmailAccountEditorDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(514, 419);
            this.Controls.Add(this.optionControl);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmailAccountEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Email Account Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.optionControl.ResumeLayout(false);
            this.optionPageAccount.ResumeLayout(false);
            this.optionPageDelivery.ResumeLayout(false);
            this.optionPageDelivery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.messageInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.messageHourLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.messageConnectionLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.autoSendMinutes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private EmailAccountSettingsControl accountSettings;
        private ShipWorks.UI.Controls.OptionControl optionControl;
        private ShipWorks.UI.Controls.OptionPage optionPageAccount;
        private ShipWorks.UI.Controls.OptionPage optionPageDelivery;
        private System.Windows.Forms.CheckBox autoSend;
        private System.Windows.Forms.Label labelRetryMinutes;
        private System.Windows.Forms.NumericUpDown autoSendMinutes;
        private ShipWorks.UI.Controls.SectionTitle sectionSending;
        private ShipWorks.UI.Controls.SectionTitle sectionEmailLimits;
        private System.Windows.Forms.CheckBox perConnectionLimit;
        private System.Windows.Forms.Label labelMessagesPerConnection;
        private System.Windows.Forms.NumericUpDown messageConnectionLimit;
        private System.Windows.Forms.Label labelMessagesPerHour;
        private System.Windows.Forms.NumericUpDown messageHourLimit;
        private System.Windows.Forms.CheckBox perHourLimit;
        private System.Windows.Forms.Label labelMessageInterval;
        private System.Windows.Forms.NumericUpDown messageInterval;
        private System.Windows.Forms.CheckBox intervalLimit;
        private UI.Controls.InfoTip infoTipLimits;
        private UI.Controls.InfoTip infoTipSending;
    }
}