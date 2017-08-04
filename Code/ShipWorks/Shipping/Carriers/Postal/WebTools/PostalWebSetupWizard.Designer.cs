namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    partial class PostalWebSetupWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostalWebSetupWizard));
            this.wizardPageWelcome = new ShipWorks.UI.Wizard.WizardPage();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageWelcome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            //
            // next
            //
            this.next.Location = new System.Drawing.Point(434, 460);
            this.next.Text = "Finish";
            //
            // cancel
            //
            this.cancel.Location = new System.Drawing.Point(515, 460);
            //
            // back
            //
            this.back.Location = new System.Drawing.Point(353, 460);
            //
            // mainPanel
            //
            this.mainPanel.Controls.Add(this.wizardPageWelcome);
            this.mainPanel.Size = new System.Drawing.Size(602, 388);
            //
            // etchBottom
            //
            this.etchBottom.Location = new System.Drawing.Point(0, 450);
            this.etchBottom.Size = new System.Drawing.Size(606, 2);
            //
            // pictureBox
            //
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.usps;
            this.pictureBox.Location = new System.Drawing.Point(378, 0);
            this.pictureBox.Size = new System.Drawing.Size(236, 54);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            //
            // topPanel
            //
            this.topPanel.Size = new System.Drawing.Size(602, 56);
            //
            // wizardPageWelcome
            //
            this.wizardPageWelcome.AutoSize = true;
            this.wizardPageWelcome.Controls.Add(this.label2);
            this.wizardPageWelcome.Controls.Add(this.pictureBox1);
            this.wizardPageWelcome.Controls.Add(this.label1);
            this.wizardPageWelcome.Controls.Add(this.labelInfo1);
            this.wizardPageWelcome.Description = "Setup ShipWorks to print USPS labels.";
            this.wizardPageWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWelcome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageWelcome.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWelcome.Name = "wizardPageWelcome";
            this.wizardPageWelcome.Size = new System.Drawing.Size(602, 388);
            this.wizardPageWelcome.TabIndex = 0;
            this.wizardPageWelcome.Title = "Setup USPS Shipping";
            //
            // label2
            //
            this.label2.Location = new System.Drawing.Point(115, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(419, 110);
            this.label2.TabIndex = 7;
            this.label2.Text = resources.GetString("label2.Text");
            //
            // pictureBox1
            //
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.exclamation16;
            this.pictureBox1.Location = new System.Drawing.Point(23, 49);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.Location = new System.Drawing.Point(43, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Important";
            //
            // labelInfo1
            //
            this.labelInfo1.Location = new System.Drawing.Point(23, 10);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(486, 34);
            this.labelInfo1.TabIndex = 4;
            this.labelInfo1.Text = "This wizard will assist you in configuring ShipWorks to download USPS shipping la" +
    "bels. This enables you to begin shipping, tracking, and printing USPS labels dir" +
    "ectly from ShipWorks.";
            //
            // PostalWebSetupWizard
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 495);
            this.Name = "PostalWebSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageWelcome});
            this.Text = "USPS w/o Postage Setup Wizard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageWelcome.ResumeLayout(false);
            this.wizardPageWelcome.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageWelcome;
        private System.Windows.Forms.Label labelInfo1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
    }
}