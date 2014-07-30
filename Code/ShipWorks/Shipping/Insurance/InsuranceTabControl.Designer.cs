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
            this.insuranceViewClaimControl = new ShipWorks.Shipping.Insurance.InsuranceViewClaimControl();
            this.insuranceSubmitClaimControl = new ShipWorks.Shipping.Insurance.InsuranceSubmitClaimControl();
            this.SuspendLayout();
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(4, 0);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(417, 13);
            this.messageLabel.TabIndex = 1;
            this.messageLabel.Text = "Multiple shipments are selected. Select a single shipment to view insurance infor" +
    "mation.";
            this.messageLabel.Visible = false;
            // 
            // insuranceViewClaimControl
            // 
            this.insuranceViewClaimControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.insuranceViewClaimControl.Location = new System.Drawing.Point(-3, 25);
            this.insuranceViewClaimControl.Name = "insuranceViewClaimControl";
            this.insuranceViewClaimControl.Size = new System.Drawing.Size(461, 93);
            this.insuranceViewClaimControl.TabIndex = 0;
            // 
            // insuranceSubmitClaimControl
            // 
            this.insuranceSubmitClaimControl.ClaimSubmitted = null;
            this.insuranceSubmitClaimControl.Location = new System.Drawing.Point(7, 124);
            this.insuranceSubmitClaimControl.Name = "insuranceSubmitClaimControl";
            this.insuranceSubmitClaimControl.Size = new System.Drawing.Size(287, 255);
            this.insuranceSubmitClaimControl.TabIndex = 2;
            // 
            // InsuranceTabControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.insuranceSubmitClaimControl);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.insuranceViewClaimControl);
            this.Name = "InsuranceTabControl";
            this.Size = new System.Drawing.Size(471, 398);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private InsuranceViewClaimControl insuranceViewClaimControl;
        private System.Windows.Forms.Label messageLabel;
        private InsuranceSubmitClaimControl insuranceSubmitClaimControl;
    }
}
