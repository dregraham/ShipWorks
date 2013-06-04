﻿namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExAccountSettingsControl
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
            this.additionalHubs = new System.Windows.Forms.TextBox();
            this.labelHubInstructions = new System.Windows.Forms.Label();
            this.labelAdditionalHubs = new System.Windows.Forms.Label();
            this.hubID = new System.Windows.Forms.TextBox();
            this.labelHubID = new System.Windows.Forms.Label();
            this.labelSmartPost = new System.Windows.Forms.Label();
            this.labelSignature = new System.Windows.Forms.Label();
            this.labelAuth = new System.Windows.Forms.Label();
            this.signatureAuth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.infoTipHubID = new ShipWorks.UI.Controls.InfoTip();
            this.infoTipExtraHubs = new ShipWorks.UI.Controls.InfoTip();
            this.SuspendLayout();
            // 
            // additionalHubs
            // 
            this.additionalHubs.AcceptsReturn = true;
            this.additionalHubs.Location = new System.Drawing.Point(68, 129);
            this.additionalHubs.Multiline = true;
            this.additionalHubs.Name = "additionalHubs";
            this.additionalHubs.Size = new System.Drawing.Size(165, 62);
            this.additionalHubs.TabIndex = 8;
            this.additionalHubs.WordWrap = false;
            // 
            // labelHubInstructions
            // 
            this.labelHubInstructions.AutoSize = true;
            this.labelHubInstructions.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelHubInstructions.Location = new System.Drawing.Point(118, 111);
            this.labelHubInstructions.Name = "labelHubInstructions";
            this.labelHubInstructions.Size = new System.Drawing.Size(100, 13);
            this.labelHubInstructions.TabIndex = 0;
            this.labelHubInstructions.Text = "(enter one per line)";
            // 
            // labelAdditionalHubs
            // 
            this.labelAdditionalHubs.AutoSize = true;
            this.labelAdditionalHubs.Location = new System.Drawing.Point(20, 111);
            this.labelAdditionalHubs.Name = "labelAdditionalHubs";
            this.labelAdditionalHubs.Size = new System.Drawing.Size(99, 13);
            this.labelAdditionalHubs.TabIndex = 6;
            this.labelAdditionalHubs.Text = "Additional Hub IDs:";
            // 
            // hubID
            // 
            this.hubID.Location = new System.Drawing.Point(70, 79);
            this.hubID.Name = "hubID";
            this.hubID.Size = new System.Drawing.Size(165, 21);
            this.hubID.TabIndex = 5;
            // 
            // labelHubID
            // 
            this.labelHubID.AutoSize = true;
            this.labelHubID.Location = new System.Drawing.Point(21, 82);
            this.labelHubID.Name = "labelHubID";
            this.labelHubID.Size = new System.Drawing.Size(44, 13);
            this.labelHubID.TabIndex = 4;
            this.labelHubID.Text = "Hub ID:";
            // 
            // labelSmartPost
            // 
            this.labelSmartPost.AutoSize = true;
            this.labelSmartPost.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSmartPost.Location = new System.Drawing.Point(0, 60);
            this.labelSmartPost.Name = "labelSmartPost";
            this.labelSmartPost.Size = new System.Drawing.Size(103, 13);
            this.labelSmartPost.TabIndex = 3;
            this.labelSmartPost.Text = "FedEx SmartPost";
            // 
            // labelSignature
            // 
            this.labelSignature.AutoSize = true;
            this.labelSignature.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSignature.Location = new System.Drawing.Point(0, 3);
            this.labelSignature.Name = "labelSignature";
            this.labelSignature.Size = new System.Drawing.Size(110, 13);
            this.labelSignature.TabIndex = 0;
            this.labelSignature.Text = "Signature Release";
            // 
            // labelAuth
            // 
            this.labelAuth.AutoSize = true;
            this.labelAuth.Location = new System.Drawing.Point(19, 26);
            this.labelAuth.Name = "labelAuth";
            this.labelAuth.Size = new System.Drawing.Size(75, 13);
            this.labelAuth.TabIndex = 1;
            this.labelAuth.Text = "Authorization:";
            // 
            // signatureAuth
            // 
            this.signatureAuth.Location = new System.Drawing.Point(99, 23);
            this.signatureAuth.Name = "signatureAuth";
            this.signatureAuth.Size = new System.Drawing.Size(165, 21);
            this.signatureAuth.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label1.Location = new System.Drawing.Point(265, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "(optional)";
            // 
            // infoTipHubID
            // 
            this.infoTipHubID.Caption = "This is assigned by FedEx when your account is approved for SmartPost.";
            this.infoTipHubID.Location = new System.Drawing.Point(239, 83);
            this.infoTipHubID.Name = "infoTipHubID";
            this.infoTipHubID.Size = new System.Drawing.Size(12, 12);
            this.infoTipHubID.TabIndex = 23;
            this.infoTipHubID.Title = "Hub ID";
            // 
            // infoTipExtraHubs
            // 
            this.infoTipExtraHubs.Caption = "If FedEx has assigned more than one Hub ID for your account they should be entere" +
    "d here.  The Hub ID above will be the default for the account and should not be " +
    "repeated in this list.";
            this.infoTipExtraHubs.Location = new System.Drawing.Point(239, 129);
            this.infoTipExtraHubs.Name = "infoTipExtraHubs";
            this.infoTipExtraHubs.Size = new System.Drawing.Size(12, 12);
            this.infoTipExtraHubs.TabIndex = 24;
            this.infoTipExtraHubs.Title = "Additional Hub IDs";
            // 
            // FedExAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.infoTipExtraHubs);
            this.Controls.Add(this.infoTipHubID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelSignature);
            this.Controls.Add(this.labelAuth);
            this.Controls.Add(this.signatureAuth);
            this.Controls.Add(this.additionalHubs);
            this.Controls.Add(this.labelHubInstructions);
            this.Controls.Add(this.labelAdditionalHubs);
            this.Controls.Add(this.hubID);
            this.Controls.Add(this.labelHubID);
            this.Controls.Add(this.labelSmartPost);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FedExAccountSettingsControl";
            this.Size = new System.Drawing.Size(345, 201);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox additionalHubs;
        private System.Windows.Forms.Label labelHubInstructions;
        private System.Windows.Forms.Label labelAdditionalHubs;
        private System.Windows.Forms.TextBox hubID;
        private System.Windows.Forms.Label labelHubID;
        private System.Windows.Forms.Label labelSmartPost;
        private System.Windows.Forms.Label labelSignature;
        private System.Windows.Forms.Label labelAuth;
        private System.Windows.Forms.TextBox signatureAuth;
        private System.Windows.Forms.Label label1;
        private UI.Controls.InfoTip infoTipHubID;
        private UI.Controls.InfoTip infoTipExtraHubs;
    }
}
