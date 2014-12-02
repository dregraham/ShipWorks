namespace ShipWorks.Data.Controls
{
    partial class AutofillPersonControl
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
            this.copyAddressLabel = new System.Windows.Forms.Label();
            this.storeSelectorPanel = new System.Windows.Forms.Panel();
            this.storeAddressLink = new System.Windows.Forms.LinkLabel();
            this.personControl = new ShipWorks.Data.Controls.PersonControl();
            this.storeSelectorPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // copyAddressLabel
            // 
            this.copyAddressLabel.AutoSize = true;
            this.copyAddressLabel.Location = new System.Drawing.Point(3, 1);
            this.copyAddressLabel.Name = "copyAddressLabel";
            this.copyAddressLabel.Size = new System.Drawing.Size(94, 13);
            this.copyAddressLabel.TabIndex = 1;
            this.copyAddressLabel.Text = "Copy address from";
            // 
            // storeSelectorPanel
            // 
            this.storeSelectorPanel.Controls.Add(this.storeAddressLink);
            this.storeSelectorPanel.Controls.Add(this.copyAddressLabel);
            this.storeSelectorPanel.Location = new System.Drawing.Point(4, 4);
            this.storeSelectorPanel.Name = "storeSelectorPanel";
            this.storeSelectorPanel.Size = new System.Drawing.Size(339, 17);
            this.storeSelectorPanel.TabIndex = 2;
            // 
            // storeAddressLink
            // 
            this.storeAddressLink.AutoSize = true;
            this.storeAddressLink.Location = new System.Drawing.Point(94, 1);
            this.storeAddressLink.Name = "storeAddressLink";
            this.storeAddressLink.Size = new System.Drawing.Size(30, 13);
            this.storeAddressLink.TabIndex = 2;
            this.storeAddressLink.TabStop = true;
            this.storeAddressLink.Text = "store";
            this.storeAddressLink.Click += new System.EventHandler(this.OnStoreAddressLinkClick);
            // 
            // personControl
            // 
            this.personControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.personControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.personControl.Location = new System.Drawing.Point(0, 22);
            this.personControl.Name = "personControl";
            this.personControl.Size = new System.Drawing.Size(346, 385);
            this.personControl.TabIndex = 0;
            // 
            // AutofillPersonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.storeSelectorPanel);
            this.Controls.Add(this.personControl);
            this.Name = "AutofillPersonControl";
            this.Size = new System.Drawing.Size(346, 409);
            this.storeSelectorPanel.ResumeLayout(false);
            this.storeSelectorPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PersonControl personControl;
        private System.Windows.Forms.Label copyAddressLabel;
        private System.Windows.Forms.Panel storeSelectorPanel;
        private System.Windows.Forms.LinkLabel storeAddressLink;
    }
}
