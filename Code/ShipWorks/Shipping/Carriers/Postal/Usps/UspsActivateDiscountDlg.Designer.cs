using System;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    partial class UspsActivateDiscountDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UspsActivateDiscountDlg));
            this.close = new System.Windows.Forms.Button();
            this.labelDiscountedPostage = new System.Windows.Forms.Label();
            this.labelDiscountInfo = new System.Windows.Forms.Label();
            this.expeditedSignup = new System.Windows.Forms.Button();
            this.expeditedLearnMore = new ShipWorks.UI.Controls.LinkControl();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(353, 174);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 1;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // labelDiscountedPostage
            // 
            this.labelDiscountedPostage.AutoSize = true;
            this.labelDiscountedPostage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDiscountedPostage.Location = new System.Drawing.Point(10, 12);
            this.labelDiscountedPostage.Name = "labelDiscountedPostage";
            this.labelDiscountedPostage.Size = new System.Drawing.Size(105, 13);
            this.labelDiscountedPostage.TabIndex = 15;
            this.labelDiscountedPostage.Text = "Postage Discount";
            // 
            // labelDiscountInfo
            // 
            this.labelDiscountInfo.Location = new System.Drawing.Point(29, 37);
            this.labelDiscountInfo.Name = "labelDiscountInfo";
            this.labelDiscountInfo.Size = new System.Drawing.Size(379, 101);
            this.labelDiscountInfo.TabIndex = 16;
            this.labelDiscountInfo.Text = resources.GetString("labelDiscountInfo.Text");
            // 
            // expeditedSignup
            // 
            this.expeditedSignup.Location = new System.Drawing.Point(31, 141);
            this.expeditedSignup.Name = "expeditedSignup";
            this.expeditedSignup.Size = new System.Drawing.Size(94, 23);
            this.expeditedSignup.TabIndex = 13;
            this.expeditedSignup.Text = "Signup for Free";
            this.expeditedSignup.UseVisualStyleBackColor = true;
            this.expeditedSignup.Click += new System.EventHandler(this.OnSignup);
            // 
            // expeditedLearnMore
            // 
            this.expeditedLearnMore.AutoSize = true;
            this.expeditedLearnMore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.expeditedLearnMore.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.expeditedLearnMore.ForeColor = System.Drawing.Color.Blue;
            this.expeditedLearnMore.Location = new System.Drawing.Point(130, 146);
            this.expeditedLearnMore.Name = "expeditedLearnMore";
            this.expeditedLearnMore.Size = new System.Drawing.Size(69, 13);
            this.expeditedLearnMore.TabIndex = 14;
            this.expeditedLearnMore.Text = "(Learn more)";
            this.expeditedLearnMore.Click += new System.EventHandler(this.OnExpedited1LearnMore);
            // 
            // UspsActivateDiscountDlg
            // 
            this.AcceptButton = this.close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(443, 205);
            this.Controls.Add(this.labelDiscountedPostage);
            this.Controls.Add(this.expeditedLearnMore);
            this.Controls.Add(this.labelDiscountInfo);
            this.Controls.Add(this.expeditedSignup);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UspsActivateDiscountDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Activate Postage Discount";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Label labelDiscountedPostage;
        private UI.Controls.LinkControl expeditedLearnMore;
        private System.Windows.Forms.Label labelDiscountInfo;
        private System.Windows.Forms.Button expeditedSignup;
    }
}