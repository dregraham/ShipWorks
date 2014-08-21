namespace ShipWorks.Shipping.Insurance
{
    partial class InsuranceTabControl
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
            this.messageLabel = new System.Windows.Forms.Label();
            this.submitClaimPanel = new System.Windows.Forms.Panel();
            this.insuranceSubmitClaimControl = new ShipWorks.Shipping.Insurance.InsuranceSubmitClaimControl();
            this.viewClaimPanel = new System.Windows.Forms.Panel();
            this.insuranceViewClaimControl = new ShipWorks.Shipping.Insurance.InsuranceViewClaimControl();
            this.notShippedPanel = new System.Windows.Forms.Panel();
            this.notShippedBrowser = new System.Windows.Forms.WebBrowser();
            this.submitClaimPanel.SuspendLayout();
            this.viewClaimPanel.SuspendLayout();
            this.notShippedPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(4, 0);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(425, 13);
            this.messageLabel.TabIndex = 1;
            this.messageLabel.Text = "Multiple shipments are selected. Select a single shipment to view insurance infor" +
    "mation.";
            this.messageLabel.Visible = false;
            // 
            // submitClaimPanel
            // 
            this.submitClaimPanel.Controls.Add(this.insuranceSubmitClaimControl);
            this.submitClaimPanel.Location = new System.Drawing.Point(8, 283);
            this.submitClaimPanel.Name = "submitClaimPanel";
            this.submitClaimPanel.Size = new System.Drawing.Size(450, 302);
            this.submitClaimPanel.TabIndex = 3;
            // 
            // insuranceSubmitClaimControl
            // 
            this.insuranceSubmitClaimControl.ClaimSubmitted = null;
            this.insuranceSubmitClaimControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceSubmitClaimControl.Location = new System.Drawing.Point(6, 3);
            this.insuranceSubmitClaimControl.Name = "insuranceSubmitClaimControl";
            this.insuranceSubmitClaimControl.Size = new System.Drawing.Size(441, 265);
            this.insuranceSubmitClaimControl.TabIndex = 3;
            // 
            // viewClaimPanel
            // 
            this.viewClaimPanel.Controls.Add(this.insuranceViewClaimControl);
            this.viewClaimPanel.Location = new System.Drawing.Point(11, 16);
            this.viewClaimPanel.Name = "viewClaimPanel";
            this.viewClaimPanel.Size = new System.Drawing.Size(447, 261);
            this.viewClaimPanel.TabIndex = 4;
            // 
            // insuranceViewClaimControl
            // 
            this.insuranceViewClaimControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.insuranceViewClaimControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceViewClaimControl.Location = new System.Drawing.Point(3, 3);
            this.insuranceViewClaimControl.Name = "insuranceViewClaimControl";
            this.insuranceViewClaimControl.Size = new System.Drawing.Size(441, 250);
            this.insuranceViewClaimControl.TabIndex = 1;
            // 
            // notShippedPanel
            // 
            this.notShippedPanel.Controls.Add(this.notShippedBrowser);
            this.notShippedPanel.Location = new System.Drawing.Point(8, 591);
            this.notShippedPanel.Name = "notShippedPanel";
            this.notShippedPanel.Size = new System.Drawing.Size(450, 292);
            this.notShippedPanel.TabIndex = 5;
            // 
            // notShippedBrowser
            // 
            this.notShippedBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.notShippedBrowser.Location = new System.Drawing.Point(0, 0);
            this.notShippedBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.notShippedBrowser.Name = "notShippedBrowser";
            this.notShippedBrowser.ScrollBarsEnabled = false;
            this.notShippedBrowser.Size = new System.Drawing.Size(450, 292);
            this.notShippedBrowser.TabIndex = 0;
            // 
            // InsuranceTabControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.notShippedPanel);
            this.Controls.Add(this.viewClaimPanel);
            this.Controls.Add(this.submitClaimPanel);
            this.Controls.Add(this.messageLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "InsuranceTabControl";
            this.Size = new System.Drawing.Size(471, 886);
            this.submitClaimPanel.ResumeLayout(false);
            this.viewClaimPanel.ResumeLayout(false);
            this.notShippedPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Panel submitClaimPanel;
        private InsuranceSubmitClaimControl insuranceSubmitClaimControl;
        private System.Windows.Forms.Panel viewClaimPanel;
        private InsuranceViewClaimControl insuranceViewClaimControl;
        private System.Windows.Forms.Panel notShippedPanel;
        private System.Windows.Forms.WebBrowser notShippedBrowser;
    }
}
