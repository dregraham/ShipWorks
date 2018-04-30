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
            this.settingsMovedMessage = new System.Windows.Forms.Label();
            this.primaryProfileLink = new System.Windows.Forms.LinkLabel();
            this.requestedLabelFormat = new ShipWorks.Shipping.Editing.RequestedLabelFormatProfileControl();
            this.SuspendLayout();
            // 
            // settingsMovedMessage
            // 
            this.settingsMovedMessage.AutoSize = true;
            this.settingsMovedMessage.Location = new System.Drawing.Point(2, 38);
            this.settingsMovedMessage.Name = "settingsMovedMessage";
            this.settingsMovedMessage.Size = new System.Drawing.Size(222, 13);
            this.settingsMovedMessage.TabIndex = 61;
            this.settingsMovedMessage.Text = "Label format settings are configured through";
            // 
            // primaryProfileLink
            // 
            this.primaryProfileLink.AutoSize = true;
            this.primaryProfileLink.Location = new System.Drawing.Point(221, 38);
            this.primaryProfileLink.Name = "primaryProfileLink";
            this.primaryProfileLink.Size = new System.Drawing.Size(76, 13);
            this.primaryProfileLink.TabIndex = 1;
            this.primaryProfileLink.TabStop = true;
            this.primaryProfileLink.Text = "primary profile";
            this.primaryProfileLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnProfileLinkClicked);
            // 
            // requestedLabelFormat
            // 
            this.requestedLabelFormat.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.requestedLabelFormat.Location = new System.Drawing.Point(0, 0);
            this.requestedLabelFormat.Name = "requestedLabelFormat";
            this.requestedLabelFormat.Size = new System.Drawing.Size(267, 21);
            this.requestedLabelFormat.State = false;
            this.requestedLabelFormat.TabIndex = 0;
            // 
            // RequestedLabelFormatOptionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.primaryProfileLink);
            this.Controls.Add(this.settingsMovedMessage);
            this.Controls.Add(this.requestedLabelFormat);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "RequestedLabelFormatOptionControl";
            this.Size = new System.Drawing.Size(344, 68);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label settingsMovedMessage;
        private System.Windows.Forms.LinkLabel primaryProfileLink;
        private RequestedLabelFormatProfileControl requestedLabelFormat;
    }
}
