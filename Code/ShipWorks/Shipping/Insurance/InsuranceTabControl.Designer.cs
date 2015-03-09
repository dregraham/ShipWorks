using ShipWorks.UI.Controls;

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
            this.messagePanel = new System.Windows.Forms.Panel();
            this.insureShipQuestionsControl = new ShipWorks.Shipping.Insurance.InsureShipQuestionsControl();
            this.messageLabel = new ShipWorks.UI.Controls.ResizingRichTextBox();
            this.insuranceSubmitClaimControl = new ShipWorks.Shipping.Insurance.InsuranceSubmitClaimControl();
            this.insuranceViewClaimControl = new ShipWorks.Shipping.Insurance.InsuranceViewClaimControl();
            this.messagePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // messagePanel
            // 
            this.messagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messagePanel.Controls.Add(this.insureShipQuestionsControl);
            this.messagePanel.Controls.Add(this.messageLabel);
            this.messagePanel.Location = new System.Drawing.Point(11, 9);
            this.messagePanel.Name = "messagePanel";
            this.messagePanel.Size = new System.Drawing.Size(430, 83);
            this.messagePanel.TabIndex = 5;
            // 
            // insureShipQuestionsControl
            // 
            this.insureShipQuestionsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.insureShipQuestionsControl.Location = new System.Drawing.Point(0, 17);
            this.insureShipQuestionsControl.Name = "insureShipQuestionsControl";
            this.insureShipQuestionsControl.Size = new System.Drawing.Size(430, 66);
            this.insureShipQuestionsControl.TabIndex = 4;
            // 
            // messageLabel
            // 
            this.messageLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.messageLabel.Location = new System.Drawing.Point(0, 0);
            this.messageLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(430, 17);
            this.messageLabel.TabIndex = 1;
            // 
            // insuranceSubmitClaimControl
            // 
            this.insuranceSubmitClaimControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.insuranceSubmitClaimControl.AutoSize = true;
            this.insuranceSubmitClaimControl.ClaimSubmitted = null;
            this.insuranceSubmitClaimControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceSubmitClaimControl.Location = new System.Drawing.Point(6, 410);
            this.insuranceSubmitClaimControl.Name = "insuranceSubmitClaimControl";
            this.insuranceSubmitClaimControl.Size = new System.Drawing.Size(413, 369);
            this.insuranceSubmitClaimControl.TabIndex = 3;
            // 
            // insuranceViewClaimControl
            // 
            this.insuranceViewClaimControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.insuranceViewClaimControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceViewClaimControl.Location = new System.Drawing.Point(7, 92);
            this.insuranceViewClaimControl.Name = "insuranceViewClaimControl";
            this.insuranceViewClaimControl.Size = new System.Drawing.Size(412, 314);
            this.insuranceViewClaimControl.TabIndex = 1;
            // 
            // InsuranceTabControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.messagePanel);
            this.Controls.Add(this.insuranceSubmitClaimControl);
            this.Controls.Add(this.insuranceViewClaimControl);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "InsuranceTabControl";
            this.Size = new System.Drawing.Size(440, 809);
            this.messagePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ResizingRichTextBox messageLabel;
        private InsuranceSubmitClaimControl insuranceSubmitClaimControl;
        private InsuranceViewClaimControl insuranceViewClaimControl;
        private InsureShipQuestionsControl insureShipQuestionsControl;
        private System.Windows.Forms.Panel messagePanel;
    }
}
