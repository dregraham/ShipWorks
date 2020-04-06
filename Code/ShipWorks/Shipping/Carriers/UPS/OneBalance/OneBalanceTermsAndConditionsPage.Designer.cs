namespace ShipWorks.Shipping.Carriers.UPS.OneBalance
{
    partial class OneBalanceTermsAndConditionsPage
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
            this.labelHeader = new System.Windows.Forms.Label();
            this.labelAgreementInfo = new System.Windows.Forms.Label();
            this.labelList1 = new System.Windows.Forms.Label();
            this.labelList2 = new System.Windows.Forms.Label();
            this.linkPromoRatesAgreement = new System.Windows.Forms.LinkLabel();
            this.linkTechnologyAgreement = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkProhibitedGoods = new System.Windows.Forms.LinkLabel();
            this.labelList3ProhibitedGoods = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeader.Location = new System.Drawing.Point(70, 29);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(328, 18);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "UPS Promotional Rates && Technology Agreement";
            // 
            // labelAgreementInfo
            // 
            this.labelAgreementInfo.AutoSize = true;
            this.labelAgreementInfo.Location = new System.Drawing.Point(19, 75);
            this.labelAgreementInfo.Name = "labelAgreementInfo";
            this.labelAgreementInfo.Size = new System.Drawing.Size(317, 13);
            this.labelAgreementInfo.TabIndex = 1;
            this.labelAgreementInfo.Text = "To start shipping with UPS, you must first agree to the following:";
            // 
            // labelList1
            // 
            this.labelList1.AutoSize = true;
            this.labelList1.Location = new System.Drawing.Point(34, 98);
            this.labelList1.Name = "labelList1";
            this.labelList1.Size = new System.Drawing.Size(17, 13);
            this.labelList1.TabIndex = 2;
            this.labelList1.Text = "1.";
            // 
            // labelList2
            // 
            this.labelList2.AutoSize = true;
            this.labelList2.Location = new System.Drawing.Point(34, 121);
            this.labelList2.Name = "labelList2";
            this.labelList2.Size = new System.Drawing.Size(17, 13);
            this.labelList2.TabIndex = 3;
            this.labelList2.Text = "2.";
            // 
            // linkPromoRatesAgreement
            // 
            this.linkPromoRatesAgreement.AutoSize = true;
            this.linkPromoRatesAgreement.Location = new System.Drawing.Point(52, 98);
            this.linkPromoRatesAgreement.Name = "linkPromoRatesAgreement";
            this.linkPromoRatesAgreement.Size = new System.Drawing.Size(197, 13);
            this.linkPromoRatesAgreement.TabIndex = 4;
            this.linkPromoRatesAgreement.TabStop = true;
            this.linkPromoRatesAgreement.Text = "The UPS Promotional Rates Agreement.";
            this.linkPromoRatesAgreement.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnClickRateAgreement);
            // 
            // linkTechnologyAgreement
            // 
            this.linkTechnologyAgreement.AutoSize = true;
            this.linkTechnologyAgreement.Location = new System.Drawing.Point(52, 121);
            this.linkTechnologyAgreement.Name = "linkTechnologyAgreement";
            this.linkTechnologyAgreement.Size = new System.Drawing.Size(165, 13);
            this.linkTechnologyAgreement.TabIndex = 5;
            this.linkTechnologyAgreement.TabStop = true;
            this.linkTechnologyAgreement.Text = "The UPS Technology Agreement.";
            this.linkTechnologyAgreement.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnClickTechnologyAgreement);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.glo_ups_brandmark_pfv;
            this.pictureBox1.Location = new System.Drawing.Point(21, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(42, 55);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // linkProhibitedGoods
            // 
            this.linkProhibitedGoods.AutoSize = true;
            this.linkProhibitedGoods.Location = new System.Drawing.Point(52, 145);
            this.linkProhibitedGoods.Name = "linkProhibitedGoods";
            this.linkProhibitedGoods.Size = new System.Drawing.Size(129, 13);
            this.linkProhibitedGoods.TabIndex = 8;
            this.linkProhibitedGoods.TabStop = true;
            this.linkProhibitedGoods.Text = "List of Prohibited Articles.";
            this.linkProhibitedGoods.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnClickLinkProhibitedGoods);
            // 
            // labelList3ProhibitedGoods
            // 
            this.labelList3ProhibitedGoods.AutoSize = true;
            this.labelList3ProhibitedGoods.Location = new System.Drawing.Point(34, 145);
            this.labelList3ProhibitedGoods.Name = "labelList3ProhibitedGoods";
            this.labelList3ProhibitedGoods.Size = new System.Drawing.Size(20, 13);
            this.labelList3ProhibitedGoods.TabIndex = 7;
            this.labelList3ProhibitedGoods.Text = "3. ";
            // 
            // OneBalanceTermsAndConditionsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkProhibitedGoods);
            this.Controls.Add(this.labelList3ProhibitedGoods);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.linkTechnologyAgreement);
            this.Controls.Add(this.linkPromoRatesAgreement);
            this.Controls.Add(this.labelList2);
            this.Controls.Add(this.labelList1);
            this.Controls.Add(this.labelAgreementInfo);
            this.Controls.Add(this.labelHeader);
            this.Description = "Setup ShipWorks to work with your UPS account.";
            this.Name = "OneBalanceTermsAndConditionsPage";
            this.Size = new System.Drawing.Size(579, 474);
            this.Title = "Account Registration";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelAgreementInfo;
        private System.Windows.Forms.Label labelList1;
        private System.Windows.Forms.Label labelList2;
        private System.Windows.Forms.LinkLabel linkPromoRatesAgreement;
        private System.Windows.Forms.LinkLabel linkTechnologyAgreement;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel linkProhibitedGoods;
        private System.Windows.Forms.Label labelList3ProhibitedGoods;
    }
}
