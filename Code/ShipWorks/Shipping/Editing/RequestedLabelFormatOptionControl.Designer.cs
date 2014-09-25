namespace ShipWorks.Shipping.Editing
{
    partial class RequestedLabelFormatOptionControl
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
            this.labelFormat = new System.Windows.Forms.ComboBox();
            this.labelFormatMessage = new System.Windows.Forms.Label();
            this.settingsMovedMessage = new System.Windows.Forms.Label();
            this.primaryProfileLink = new System.Windows.Forms.LinkLabel();
            this.infotipLabelType = new ShipWorks.UI.Controls.InfoTip();
            this.SuspendLayout();
            // 
            // labelFormat
            // 
            this.labelFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.labelFormat.FormattingEnabled = true;
            this.labelFormat.Location = new System.Drawing.Point(125, 0);
            this.labelFormat.Name = "labelFormat";
            this.labelFormat.Size = new System.Drawing.Size(115, 21);
            this.labelFormat.TabIndex = 60;
            // 
            // labelFormatMessage
            // 
            this.labelFormatMessage.AutoSize = true;
            this.labelFormatMessage.Location = new System.Drawing.Point(0, 3);
            this.labelFormatMessage.Name = "labelFormatMessage";
            this.labelFormatMessage.Size = new System.Drawing.Size(123, 13);
            this.labelFormatMessage.TabIndex = 59;
            this.labelFormatMessage.Text = "Requested label format:";
            // 
            // settingsMovedMessage
            // 
            this.settingsMovedMessage.AutoSize = true;
            this.settingsMovedMessage.Location = new System.Drawing.Point(2, 38);
            this.settingsMovedMessage.Name = "settingsMovedMessage";
            this.settingsMovedMessage.Size = new System.Drawing.Size(267, 13);
            this.settingsMovedMessage.TabIndex = 61;
            this.settingsMovedMessage.Text = "Label format settings are now configured through the ";
            // 
            // primaryProfileLink
            // 
            this.primaryProfileLink.AutoSize = true;
            this.primaryProfileLink.Location = new System.Drawing.Point(263, 38);
            this.primaryProfileLink.Name = "primaryProfileLink";
            this.primaryProfileLink.Size = new System.Drawing.Size(76, 13);
            this.primaryProfileLink.TabIndex = 62;
            this.primaryProfileLink.TabStop = true;
            this.primaryProfileLink.Text = "primary profile";
            this.primaryProfileLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnProfileLinkClicked);
            // 
            // infotipLabelType
            // 
            this.infotipLabelType.Caption = "The printer type for a shipment cannot be changed after processing.";
            this.infotipLabelType.Location = new System.Drawing.Point(246, 4);
            this.infotipLabelType.Name = "infotipLabelType";
            this.infotipLabelType.Size = new System.Drawing.Size(12, 12);
            this.infotipLabelType.TabIndex = 58;
            this.infotipLabelType.Title = "Printer Type";
            // 
            // RequestedLabelFormatOptionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.primaryProfileLink);
            this.Controls.Add(this.settingsMovedMessage);
            this.Controls.Add(this.labelFormat);
            this.Controls.Add(this.labelFormatMessage);
            this.Controls.Add(this.infotipLabelType);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "RequestedLabelFormatOptionControl";
            this.Size = new System.Drawing.Size(344, 68);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox labelFormat;
        private System.Windows.Forms.Label labelFormatMessage;
        private UI.Controls.InfoTip infotipLabelType;
        private System.Windows.Forms.Label settingsMovedMessage;
        private System.Windows.Forms.LinkLabel primaryProfileLink;
    }
}
