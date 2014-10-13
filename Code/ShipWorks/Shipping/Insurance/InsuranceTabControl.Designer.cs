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
            this.insuranceSubmitClaimControl = new ShipWorks.Shipping.Insurance.InsuranceSubmitClaimControl();
            this.insuranceViewClaimControl = new ShipWorks.Shipping.Insurance.InsuranceViewClaimControl();
            this.SuspendLayout();
            // 
            // messageLabel
            // 
            this.messageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageLabel.Location = new System.Drawing.Point(4, 0);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(425, 66);
            this.messageLabel.TabIndex = 1;
            this.messageLabel.Text = "Multiple shipments are selected. Select a single shipment to view insurance infor" +
    "mation.";
            this.messageLabel.Visible = false;
            // 
            // insuranceSubmitClaimControl
            // 
            this.insuranceSubmitClaimControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.insuranceSubmitClaimControl.AutoSize = true;
            this.insuranceSubmitClaimControl.ClaimSubmitted = null;
            this.insuranceSubmitClaimControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceSubmitClaimControl.Location = new System.Drawing.Point(7, 387);
            this.insuranceSubmitClaimControl.Name = "insuranceSubmitClaimControl";
            this.insuranceSubmitClaimControl.Size = new System.Drawing.Size(413, 369);
            this.insuranceSubmitClaimControl.TabIndex = 3;
            // 
            // insuranceViewClaimControl
            // 
            this.insuranceViewClaimControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.insuranceViewClaimControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceViewClaimControl.Location = new System.Drawing.Point(8, 69);
            this.insuranceViewClaimControl.Name = "insuranceViewClaimControl";
            this.insuranceViewClaimControl.Size = new System.Drawing.Size(412, 314);
            this.insuranceViewClaimControl.TabIndex = 1;
            // 
            // InsuranceTabControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.insuranceSubmitClaimControl);
            this.Controls.Add(this.insuranceViewClaimControl);
            this.Controls.Add(this.messageLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "InsuranceTabControl";
            this.Size = new System.Drawing.Size(440, 809);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label messageLabel;
        private InsuranceSubmitClaimControl insuranceSubmitClaimControl;
        private InsuranceViewClaimControl insuranceViewClaimControl;
    }
}
