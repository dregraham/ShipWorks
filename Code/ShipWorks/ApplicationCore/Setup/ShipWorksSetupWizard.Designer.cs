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
            this.wizardPageOnlineStore = new ShipWorks.UI.Wizard.WizardPage();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.wizardPagePrinters = new ShipWorks.UI.Wizard.WizardPage();
            this.printerSelectionControl1 = new ShipWorks.Templates.Printing.Configuration.PrinterFormatControl();
            this.wizardPage1 = new ShipWorks.UI.Wizard.WizardPage();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageOnlineStore.SuspendLayout();
            this.wizardPagePrinters.SuspendLayout();
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
            this.mainPanel.Controls.Add(this.wizardPagePrinters);
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
            // wizardPageOnlineStore
            // 
            this.wizardPageOnlineStore.Controls.Add(this.radioButton2);
            this.wizardPageOnlineStore.Controls.Add(this.radioButton1);
            this.wizardPageOnlineStore.Description = "What platform do you sell on?";
            this.wizardPageOnlineStore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageOnlineStore.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageOnlineStore.Location = new System.Drawing.Point(0, 0);
            this.wizardPageOnlineStore.Name = "wizardPageOnlineStore";
            this.wizardPageOnlineStore.Size = new System.Drawing.Size(548, 271);
            this.wizardPageOnlineStore.TabIndex = 0;
            this.wizardPageOnlineStore.Title = "Online Store";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(23, 82);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(299, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Just give me some sample orders and let me start playing";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(23, 12);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(184, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Get connected to my online store";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // wizardPagePrinters
            // 
            this.wizardPagePrinters.Controls.Add(this.printerSelectionControl1);
            this.wizardPagePrinters.Description = "Select the printer you will use for shipping labels.";
            this.wizardPagePrinters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPagePrinters.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPagePrinters.Location = new System.Drawing.Point(0, 0);
            this.wizardPagePrinters.Name = "wizardPagePrinters";
            this.wizardPagePrinters.Size = new System.Drawing.Size(548, 271);
            this.wizardPagePrinters.TabIndex = 0;
            this.wizardPagePrinters.Title = "Printer Selection";
            // 
            // printerSelectionControl1
            // 
            this.printerSelectionControl1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printerSelectionControl1.Location = new System.Drawing.Point(23, 8);
            this.printerSelectionControl1.Name = "printerSelectionControl1";
            this.printerSelectionControl1.Size = new System.Drawing.Size(427, 246);
            this.printerSelectionControl1.TabIndex = 0;
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
            this.wizardPage1.Title = "Wizard page 3.";
            // 
            // ShipWorksSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 378);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(564, 416);
            this.MinimumSize = new System.Drawing.Size(564, 416);
            this.Name = "ShipWorksSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPagePrinters,
            this.wizardPageOnlineStore,
            this.wizardPage1});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ShipWorks Setup";
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageOnlineStore.ResumeLayout(false);
            this.wizardPageOnlineStore.PerformLayout();
            this.wizardPagePrinters.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageOnlineStore;
        private UI.Wizard.WizardPage wizardPagePrinters;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private UI.Wizard.WizardPage wizardPage1;
        private Templates.Printing.Configuration.PrinterFormatControl printerSelectionControl1;
    }
}