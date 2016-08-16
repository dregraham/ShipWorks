namespace ShipWorks.Shipping.Carriers.Other
{
    partial class OtherSetupWizard
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
            this.wizardPageWelcome = new ShipWorks.UI.Wizard.WizardPage();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageWelcome.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.next.Location = new System.Drawing.Point(419, 377);
            this.next.Text = "Finish";
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(500, 377);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(338, 377);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageWelcome);
            this.mainPanel.Size = new System.Drawing.Size(587, 305);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 367);
            this.etchBottom.Size = new System.Drawing.Size(591, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.box_closed;
            this.pictureBox.Location = new System.Drawing.Point(534, 3);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(587, 56);
            // 
            // wizardPageWelcome
            // 
            this.wizardPageWelcome.Controls.Add(this.labelInfo1);
            this.wizardPageWelcome.Description = "Setup shipping for other carriers.";
            this.wizardPageWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWelcome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageWelcome.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWelcome.Name = "wizardPageWelcome";
            this.wizardPageWelcome.Size = new System.Drawing.Size(587, 305);
            this.wizardPageWelcome.TabIndex = 0;
            this.wizardPageWelcome.Title = "Setup Wizard";
            // 
            // labelInfo1
            // 
            this.labelInfo1.Location = new System.Drawing.Point(22, 8);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(454, 47);
            this.labelInfo1.TabIndex = 4;
            this.labelInfo1.Text = "This wizard will assist you in configuring ShipWorks to work with other carriers " +
                "not supported by ShipWorks.  This enables you to manually enter shipment carrier" +
                ", service, and tracking information.";
            // 
            // OtherSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 412);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimumSize = new System.Drawing.Size(542, 435);
            this.Name = "OtherSetupWizard";
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageWelcome});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Other Setup Wizard";
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageWelcome.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageWelcome;
        private System.Windows.Forms.Label labelInfo1;
    }
}