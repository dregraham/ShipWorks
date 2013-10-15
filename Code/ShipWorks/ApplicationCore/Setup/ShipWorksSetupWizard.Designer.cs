namespace ShipWorks.ApplicationCore.Setup
{
    partial class ShipWorksSetupWizard
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
            this.wizardPage1 = new ShipWorks.UI.Wizard.WizardPage();
            this.wizardPage2 = new ShipWorks.UI.Wizard.WizardPage();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(380, 343);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(461, 343);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(299, 343);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPage1);
            this.mainPanel.Size = new System.Drawing.Size(548, 271);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 333);
            this.etchBottom.Size = new System.Drawing.Size(552, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.sw_cubes_big;
            this.pictureBox.Location = new System.Drawing.Point(486, 3);
            this.pictureBox.Size = new System.Drawing.Size(54, 50);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(548, 56);
            // 
            // wizardPage1
            // 
            this.wizardPage1.Description = "The description of the page.";
            this.wizardPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPage1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPage1.Location = new System.Drawing.Point(0, 0);
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.Size = new System.Drawing.Size(548, 271);
            this.wizardPage1.TabIndex = 0;
            this.wizardPage1.Title = "Wizard page 1.";
            // 
            // wizardPage2
            // 
            this.wizardPage2.Description = "The description of the page.";
            this.wizardPage2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPage2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPage2.Location = new System.Drawing.Point(0, 0);
            this.wizardPage2.Name = "wizardPage2";
            this.wizardPage2.Size = new System.Drawing.Size(548, 271);
            this.wizardPage2.TabIndex = 0;
            this.wizardPage2.Title = "Wizard page 2.";
            // 
            // ShipWorksSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 378);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimumSize = new System.Drawing.Size(564, 416);
            this.Name = "ShipWorksSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            ((ShipWorks.UI.Wizard.WizardPage)(this.wizardPage1)),
            ((ShipWorks.UI.Wizard.WizardPage)(this.wizardPage2))});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ShipWorks Setup";
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPage1;
        private UI.Wizard.WizardPage wizardPage2;
    }
}