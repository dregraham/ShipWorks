namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    partial class UpsPharmaceuticalControl
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
            this.labelWillShipPharmacuticals = new System.Windows.Forms.Label();
            this.willShipYes = new System.Windows.Forms.RadioButton();
            this.willShipAnswerPanel = new System.Windows.Forms.Panel();
            this.willShipNo = new System.Windows.Forms.RadioButton();
            this.followUpQuestionPanel = new System.Windows.Forms.Panel();
            this.onlinePharmacyAnswerPanel = new System.Windows.Forms.Panel();
            this.onlinePharmacyNo = new System.Windows.Forms.RadioButton();
            this.onlinePharmacyYes = new System.Windows.Forms.RadioButton();
            this.labelOnlinePharmacy = new System.Windows.Forms.Label();
            this.licensedAnswerPanel = new System.Windows.Forms.Panel();
            this.licensedNo = new System.Windows.Forms.RadioButton();
            this.licensedYes = new System.Windows.Forms.RadioButton();
            this.labelAreYouLicensed = new System.Windows.Forms.Label();
            this.willShipAnswerPanel.SuspendLayout();
            this.followUpQuestionPanel.SuspendLayout();
            this.onlinePharmacyAnswerPanel.SuspendLayout();
            this.licensedAnswerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelWillShipPharmacuticals
            // 
            this.labelWillShipPharmacuticals.AutoSize = true;
            this.labelWillShipPharmacuticals.Location = new System.Drawing.Point(0, 4);
            this.labelWillShipPharmacuticals.Name = "labelWillShipPharmacuticals";
            this.labelWillShipPharmacuticals.Size = new System.Drawing.Size(303, 13);
            this.labelWillShipPharmacuticals.TabIndex = 0;
            this.labelWillShipPharmacuticals.Text = "Will you ship prescription pharmaceuticals directly to patients?";
            // 
            // willShipYes
            // 
            this.willShipYes.AutoSize = true;
            this.willShipYes.Location = new System.Drawing.Point(44, 0);
            this.willShipYes.Name = "willShipYes";
            this.willShipYes.Size = new System.Drawing.Size(42, 17);
            this.willShipYes.TabIndex = 1;
            this.willShipYes.Text = "Yes";
            this.willShipYes.UseVisualStyleBackColor = true;
            this.willShipYes.CheckedChanged += new System.EventHandler(this.OnWillShipYesCheckedChanged);
            // 
            // willShipAnswerPanel
            // 
            this.willShipAnswerPanel.Controls.Add(this.willShipNo);
            this.willShipAnswerPanel.Controls.Add(this.willShipYes);
            this.willShipAnswerPanel.Location = new System.Drawing.Point(22, 20);
            this.willShipAnswerPanel.Name = "willShipAnswerPanel";
            this.willShipAnswerPanel.Size = new System.Drawing.Size(115, 20);
            this.willShipAnswerPanel.TabIndex = 2;
            // 
            // willShipNo
            // 
            this.willShipNo.AutoSize = true;
            this.willShipNo.Checked = true;
            this.willShipNo.Location = new System.Drawing.Point(0, 0);
            this.willShipNo.Name = "willShipNo";
            this.willShipNo.Size = new System.Drawing.Size(38, 17);
            this.willShipNo.TabIndex = 2;
            this.willShipNo.TabStop = true;
            this.willShipNo.Text = "No";
            this.willShipNo.UseVisualStyleBackColor = true;
            // 
            // followUpQuestionPanel
            // 
            this.followUpQuestionPanel.Controls.Add(this.onlinePharmacyAnswerPanel);
            this.followUpQuestionPanel.Controls.Add(this.labelOnlinePharmacy);
            this.followUpQuestionPanel.Controls.Add(this.licensedAnswerPanel);
            this.followUpQuestionPanel.Controls.Add(this.labelAreYouLicensed);
            this.followUpQuestionPanel.Location = new System.Drawing.Point(17, 43);
            this.followUpQuestionPanel.Name = "followUpQuestionPanel";
            this.followUpQuestionPanel.Size = new System.Drawing.Size(467, 100);
            this.followUpQuestionPanel.TabIndex = 3;
            this.followUpQuestionPanel.Visible = false;
            // 
            // onlinePharmacyAnswerPanel
            // 
            this.onlinePharmacyAnswerPanel.Controls.Add(this.onlinePharmacyNo);
            this.onlinePharmacyAnswerPanel.Controls.Add(this.onlinePharmacyYes);
            this.onlinePharmacyAnswerPanel.Location = new System.Drawing.Point(25, 58);
            this.onlinePharmacyAnswerPanel.Name = "onlinePharmacyAnswerPanel";
            this.onlinePharmacyAnswerPanel.Size = new System.Drawing.Size(115, 20);
            this.onlinePharmacyAnswerPanel.TabIndex = 6;
            // 
            // onlinePharmacyNo
            // 
            this.onlinePharmacyNo.AutoSize = true;
            this.onlinePharmacyNo.Checked = true;
            this.onlinePharmacyNo.Location = new System.Drawing.Point(0, 0);
            this.onlinePharmacyNo.Name = "onlinePharmacyNo";
            this.onlinePharmacyNo.Size = new System.Drawing.Size(38, 17);
            this.onlinePharmacyNo.TabIndex = 2;
            this.onlinePharmacyNo.TabStop = true;
            this.onlinePharmacyNo.Text = "No";
            this.onlinePharmacyNo.UseVisualStyleBackColor = true;
            // 
            // onlinePharmacyYes
            // 
            this.onlinePharmacyYes.AutoSize = true;
            this.onlinePharmacyYes.Location = new System.Drawing.Point(44, 0);
            this.onlinePharmacyYes.Name = "onlinePharmacyYes";
            this.onlinePharmacyYes.Size = new System.Drawing.Size(42, 17);
            this.onlinePharmacyYes.TabIndex = 1;
            this.onlinePharmacyYes.Text = "Yes";
            this.onlinePharmacyYes.UseVisualStyleBackColor = true;
            // 
            // labelOnlinePharmacy
            // 
            this.labelOnlinePharmacy.AutoSize = true;
            this.labelOnlinePharmacy.Location = new System.Drawing.Point(3, 42);
            this.labelOnlinePharmacy.Name = "labelOnlinePharmacy";
            this.labelOnlinePharmacy.Size = new System.Drawing.Size(261, 13);
            this.labelOnlinePharmacy.TabIndex = 5;
            this.labelOnlinePharmacy.Text = "Do you operate as an online or mail-order pharmacy?";
            // 
            // licensedAnswerPanel
            // 
            this.licensedAnswerPanel.Controls.Add(this.licensedNo);
            this.licensedAnswerPanel.Controls.Add(this.licensedYes);
            this.licensedAnswerPanel.Location = new System.Drawing.Point(25, 16);
            this.licensedAnswerPanel.Name = "licensedAnswerPanel";
            this.licensedAnswerPanel.Size = new System.Drawing.Size(115, 20);
            this.licensedAnswerPanel.TabIndex = 4;
            // 
            // licensedNo
            // 
            this.licensedNo.AutoSize = true;
            this.licensedNo.Checked = true;
            this.licensedNo.Location = new System.Drawing.Point(0, 0);
            this.licensedNo.Name = "licensedNo";
            this.licensedNo.Size = new System.Drawing.Size(38, 17);
            this.licensedNo.TabIndex = 2;
            this.licensedNo.TabStop = true;
            this.licensedNo.Text = "No";
            this.licensedNo.UseVisualStyleBackColor = true;
            // 
            // licensedYes
            // 
            this.licensedYes.AutoSize = true;
            this.licensedYes.Location = new System.Drawing.Point(44, 0);
            this.licensedYes.Name = "licensedYes";
            this.licensedYes.Size = new System.Drawing.Size(42, 17);
            this.licensedYes.TabIndex = 1;
            this.licensedYes.Text = "Yes";
            this.licensedYes.UseVisualStyleBackColor = true;
            // 
            // labelAreYouLicensed
            // 
            this.labelAreYouLicensed.AutoSize = true;
            this.labelAreYouLicensed.Location = new System.Drawing.Point(3, 0);
            this.labelAreYouLicensed.Name = "labelAreYouLicensed";
            this.labelAreYouLicensed.Size = new System.Drawing.Size(459, 13);
            this.labelAreYouLicensed.TabIndex = 3;
            this.labelAreYouLicensed.Text = "Are you licensed in every state or territory to which you will ship prescription " +
    "pharmaceuticals?";
            // 
            // UpsPharmaceuticalControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.followUpQuestionPanel);
            this.Controls.Add(this.willShipAnswerPanel);
            this.Controls.Add(this.labelWillShipPharmacuticals);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UpsPharmaceuticalControl";
            this.Size = new System.Drawing.Size(546, 150);
            this.willShipAnswerPanel.ResumeLayout(false);
            this.willShipAnswerPanel.PerformLayout();
            this.followUpQuestionPanel.ResumeLayout(false);
            this.followUpQuestionPanel.PerformLayout();
            this.onlinePharmacyAnswerPanel.ResumeLayout(false);
            this.onlinePharmacyAnswerPanel.PerformLayout();
            this.licensedAnswerPanel.ResumeLayout(false);
            this.licensedAnswerPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelWillShipPharmacuticals;
        private System.Windows.Forms.RadioButton willShipYes;
        private System.Windows.Forms.Panel willShipAnswerPanel;
        private System.Windows.Forms.RadioButton willShipNo;
        private System.Windows.Forms.Panel followUpQuestionPanel;
        private System.Windows.Forms.Panel onlinePharmacyAnswerPanel;
        private System.Windows.Forms.RadioButton onlinePharmacyNo;
        private System.Windows.Forms.RadioButton onlinePharmacyYes;
        private System.Windows.Forms.Label labelOnlinePharmacy;
        private System.Windows.Forms.Panel licensedAnswerPanel;
        private System.Windows.Forms.RadioButton licensedNo;
        private System.Windows.Forms.RadioButton licensedYes;
        private System.Windows.Forms.Label labelAreYouLicensed;
    }
}
