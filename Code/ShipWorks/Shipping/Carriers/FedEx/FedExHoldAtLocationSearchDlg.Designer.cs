namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExHoldAtLocationSearchDlg
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
            this.OkButton = new System.Windows.Forms.Button();
            this.topLabel = new System.Windows.Forms.Label();
            this.addressPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.Location = new System.Drawing.Point(248, 44);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 0;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.ClickOkButton);
            // 
            // topLabel
            // 
            this.topLabel.AutoSize = true;
            this.topLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.topLabel.Location = new System.Drawing.Point(13, 13);
            this.topLabel.Name = "topLabel";
            this.topLabel.Size = new System.Drawing.Size(255, 13);
            this.topLabel.TabIndex = 3;
            this.topLabel.Text = "Requesting Hold at Locations From FedEx®...";
            // 
            // addressPanel
            // 
            this.addressPanel.AutoScroll = true;
            this.addressPanel.Location = new System.Drawing.Point(16, 35);
            this.addressPanel.Name = "addressPanel";
            this.addressPanel.Size = new System.Drawing.Size(305, 17);
            this.addressPanel.TabIndex = 4;
            // 
            // FedExHoldAtLocationSearchDlg
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 79);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.addressPanel);
            this.Controls.Add(this.topLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FedExHoldAtLocationSearchDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FedEx Location Search";
            this.Load += new System.EventHandler(this.LoadDialog);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Label topLabel;
        private System.Windows.Forms.Panel addressPanel;
    }
}