namespace ShipWorks.ApplicationCore.Settings
{
    partial class SettingsPageScanToShip
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsPageScanToShip));
            this.infoTipSingleScan = new ShipWorks.UI.Controls.InfoTip();
            this.infoTipAutoPrint = new ShipWorks.UI.Controls.InfoTip();
            this.infoTipAutoWeigh = new ShipWorks.UI.Controls.InfoTip();
            this.autoWeigh = new System.Windows.Forms.CheckBox();
            this.registerScannerLabel = new System.Windows.Forms.Label();
            this.registerScannerButton = new System.Windows.Forms.Button();
            this.autoPrint = new System.Windows.Forms.CheckBox();
            this.singleScan = new System.Windows.Forms.CheckBox();
            this.sectionTitleDisplay = new ShipWorks.UI.Controls.SectionTitle();
            this.sectionTitleShipping = new ShipWorks.UI.Controls.SectionTitle();
            this.displayShortcutIndicator = new System.Windows.Forms.CheckBox();
            this.instructionsLabel = new System.Windows.Forms.Label();
            this.manageProfilesButton = new System.Windows.Forms.Button();
            this.printBarcodeButton = new System.Windows.Forms.Button();
            this.requireVerificationForAutoPrint = new System.Windows.Forms.CheckBox();
            this.infoTipRequireVerification = new ShipWorks.UI.Controls.InfoTip();
            this.autoAdvance = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // infoTipSingleScan
            // 
            this.infoTipSingleScan.Caption = "Quickly locate orders, apply Shipping Profiles, and more by scanning barcodes.\r\n\r" +
    "\nClick the \'Pair Scanner\' button to get started.";
            this.infoTipSingleScan.Location = new System.Drawing.Point(165, 107);
            this.infoTipSingleScan.Name = "infoTipSingleScan";
            this.infoTipSingleScan.Size = new System.Drawing.Size(12, 12);
            this.infoTipSingleScan.TabIndex = 40;
            this.infoTipSingleScan.Title = "Enable Barcode Scanner";
            // 
            // infoTipAutoPrint
            // 
            this.infoTipAutoPrint.Caption = "Automatically print a label when you search for an order using a barcode scan.\r\n\r" +
    "\nLabels are generated using the currently configured shipping rules.";
            this.infoTipAutoPrint.Location = new System.Drawing.Point(300, 155);
            this.infoTipAutoPrint.Name = "infoTipAutoPrint";
            this.infoTipAutoPrint.Size = new System.Drawing.Size(12, 12);
            this.infoTipAutoPrint.TabIndex = 39;
            this.infoTipAutoPrint.Title = "Automatically Print Labels on Barcode Scan Search";
            // 
            // infoTipAutoWeigh
            // 
            this.infoTipAutoWeigh.Caption = "Import the weight from your scale automatically when you search for an order usin" +
    "g a barcode scan.\r\n\r\nThe weight is imported if the scale registers a weight grea" +
    "ter than 0.";
            this.infoTipAutoWeigh.Location = new System.Drawing.Point(328, 132);
            this.infoTipAutoWeigh.Name = "infoTipAutoWeigh";
            this.infoTipAutoWeigh.Size = new System.Drawing.Size(13, 12);
            this.infoTipAutoWeigh.TabIndex = 38;
            this.infoTipAutoWeigh.Title = "Automatically Weigh Packages on Barcode Scan Search";
            // 
            // autoWeigh
            // 
            this.autoWeigh.AutoSize = true;
            this.autoWeigh.Location = new System.Drawing.Point(39, 130);
            this.autoWeigh.Name = "autoWeigh";
            this.autoWeigh.Size = new System.Drawing.Size(287, 17);
            this.autoWeigh.TabIndex = 37;
            this.autoWeigh.Text = "Automatically weigh packages on barcode scan search";
            this.autoWeigh.UseVisualStyleBackColor = true;
            this.autoWeigh.CheckedChanged += new System.EventHandler(this.OnChangeSingleScanSettings);
            // 
            // registerScannerLabel
            // 
            this.registerScannerLabel.AutoSize = true;
            this.registerScannerLabel.ForeColor = System.Drawing.Color.Red;
            this.registerScannerLabel.Location = new System.Drawing.Point(316, 106);
            this.registerScannerLabel.Name = "registerScannerLabel";
            this.registerScannerLabel.Size = new System.Drawing.Size(97, 13);
            this.registerScannerLabel.TabIndex = 36;
            this.registerScannerLabel.Text = "Scanner not paired";
            // 
            // registerScannerButton
            // 
            this.registerScannerButton.Location = new System.Drawing.Point(196, 101);
            this.registerScannerButton.Name = "registerScannerButton";
            this.registerScannerButton.Size = new System.Drawing.Size(110, 23);
            this.registerScannerButton.TabIndex = 35;
            this.registerScannerButton.Text = "Pair Scanner...";
            this.registerScannerButton.UseVisualStyleBackColor = true;
            this.registerScannerButton.Click += new System.EventHandler(this.OnClickRegisterScanner);
            // 
            // autoPrint
            // 
            this.autoPrint.AutoSize = true;
            this.autoPrint.Location = new System.Drawing.Point(39, 153);
            this.autoPrint.Name = "autoPrint";
            this.autoPrint.Size = new System.Drawing.Size(259, 17);
            this.autoPrint.TabIndex = 34;
            this.autoPrint.Text = "Automatically print labels on barcode scan search";
            this.autoPrint.UseVisualStyleBackColor = true;
            this.autoPrint.CheckedChanged += new System.EventHandler(this.OnChangeSingleScanSettings);
            // 
            // singleScan
            // 
            this.singleScan.AutoSize = true;
            this.singleScan.Location = new System.Drawing.Point(20, 105);
            this.singleScan.Name = "singleScan";
            this.singleScan.Size = new System.Drawing.Size(142, 17);
            this.singleScan.TabIndex = 33;
            this.singleScan.Text = "Enable barcode scanner";
            this.singleScan.UseVisualStyleBackColor = true;
            this.singleScan.CheckedChanged += new System.EventHandler(this.OnChangeSingleScanSettings);
            // 
            // sectionTitleDisplay
            // 
            this.sectionTitleDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitleDisplay.Location = new System.Drawing.Point(10, 10);
            this.sectionTitleDisplay.Name = "sectionTitleDisplay";
            this.sectionTitleDisplay.Size = new System.Drawing.Size(530, 22);
            this.sectionTitleDisplay.TabIndex = 41;
            this.sectionTitleDisplay.Text = "Display";
            // 
            // sectionTitleShipping
            // 
            this.sectionTitleShipping.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitleShipping.Location = new System.Drawing.Point(10, 69);
            this.sectionTitleShipping.Name = "sectionTitleShipping";
            this.sectionTitleShipping.Size = new System.Drawing.Size(530, 22);
            this.sectionTitleShipping.TabIndex = 42;
            this.sectionTitleShipping.Text = "Shipping Automation";
            // 
            // displayShortcutIndicator
            // 
            this.displayShortcutIndicator.AutoSize = true;
            this.displayShortcutIndicator.Location = new System.Drawing.Point(20, 42);
            this.displayShortcutIndicator.Name = "displayShortcutIndicator";
            this.displayShortcutIndicator.Size = new System.Drawing.Size(240, 17);
            this.displayShortcutIndicator.TabIndex = 43;
            this.displayShortcutIndicator.Text = "Show Keyboard && Barcode Shortcut Indicator";
            this.displayShortcutIndicator.UseVisualStyleBackColor = true;
            // 
            // instructionsLabel
            // 
            this.instructionsLabel.Location = new System.Drawing.Point(17, 228);
            this.instructionsLabel.Name = "instructionsLabel";
            this.instructionsLabel.Size = new System.Drawing.Size(523, 50);
            this.instructionsLabel.TabIndex = 44;
            this.instructionsLabel.Text = resources.GetString("instructionsLabel.Text");
            // 
            // manageProfilesButton
            // 
            this.manageProfilesButton.Location = new System.Drawing.Point(20, 281);
            this.manageProfilesButton.Name = "manageProfilesButton";
            this.manageProfilesButton.Size = new System.Drawing.Size(110, 23);
            this.manageProfilesButton.TabIndex = 45;
            this.manageProfilesButton.Text = "Manage Profiles...";
            this.manageProfilesButton.UseVisualStyleBackColor = true;
            this.manageProfilesButton.Click += new System.EventHandler(this.OnClickManageProfiles);
            // 
            // printBarcodeButton
            // 
            this.printBarcodeButton.Location = new System.Drawing.Point(140, 281);
            this.printBarcodeButton.Name = "printBarcodeButton";
            this.printBarcodeButton.Size = new System.Drawing.Size(220, 23);
            this.printBarcodeButton.TabIndex = 46;
            this.printBarcodeButton.Text = "Print All Keyboard && Barcode Shortcuts...";
            this.printBarcodeButton.UseVisualStyleBackColor = true;
            this.printBarcodeButton.Click += new System.EventHandler(this.OnClickPrintShortcuts);
            // 
            // requireVerificationForAutoPrint
            // 
            this.requireVerificationForAutoPrint.AutoSize = true;
            this.requireVerificationForAutoPrint.Enabled = false;
            this.requireVerificationForAutoPrint.Location = new System.Drawing.Point(58, 176);
            this.requireVerificationForAutoPrint.Name = "requireVerificationForAutoPrint";
            this.requireVerificationForAutoPrint.Size = new System.Drawing.Size(253, 17);
            this.requireVerificationForAutoPrint.TabIndex = 47;
            this.requireVerificationForAutoPrint.Text = "Require orders to be verified before printing";
            this.requireVerificationForAutoPrint.UseVisualStyleBackColor = true;
            this.requireVerificationForAutoPrint.CheckedChanged += new System.EventHandler(this.OnChangeSingleScanSettings);
            // 
            // infoTipRequireVerification
            // 
            this.infoTipRequireVerification.Caption = "This feature is only available on warehouse plans.";
            this.infoTipRequireVerification.Location = new System.Drawing.Point(320, 178);
            this.infoTipRequireVerification.Name = "infoTipRequireVerification";
            this.infoTipRequireVerification.Size = new System.Drawing.Size(12, 12);
            this.infoTipRequireVerification.TabIndex = 48;
            this.infoTipRequireVerification.Title = "Require Verification";
            // 
            // autoAdvance
            // 
            this.autoAdvance.AutoSize = true;
            this.autoAdvance.Location = new System.Drawing.Point(20, 199);
            this.autoAdvance.Name = "autoAdvance";
            this.autoAdvance.Size = new System.Drawing.Size(316, 17);
            this.autoAdvance.TabIndex = 49;
            this.autoAdvance.Text = "Automatically advance to Ship details once orders are verified";
            this.autoAdvance.UseVisualStyleBackColor = true;
            // 
            // SettingsPageScanToShip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.autoAdvance);
            this.Controls.Add(this.infoTipRequireVerification);
            this.Controls.Add(this.requireVerificationForAutoPrint);
            this.Controls.Add(this.printBarcodeButton);
            this.Controls.Add(this.manageProfilesButton);
            this.Controls.Add(this.instructionsLabel);
            this.Controls.Add(this.displayShortcutIndicator);
            this.Controls.Add(this.sectionTitleShipping);
            this.Controls.Add(this.sectionTitleDisplay);
            this.Controls.Add(this.infoTipSingleScan);
            this.Controls.Add(this.infoTipAutoPrint);
            this.Controls.Add(this.infoTipAutoWeigh);
            this.Controls.Add(this.autoWeigh);
            this.Controls.Add(this.registerScannerLabel);
            this.Controls.Add(this.registerScannerButton);
            this.Controls.Add(this.autoPrint);
            this.Controls.Add(this.singleScan);
            this.Name = "SettingsPageScanToShip";
            this.Size = new System.Drawing.Size(550, 311);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.InfoTip infoTipSingleScan;
        private UI.Controls.InfoTip infoTipAutoPrint;
        private UI.Controls.InfoTip infoTipAutoWeigh;
        private System.Windows.Forms.CheckBox autoWeigh;
        private System.Windows.Forms.Label registerScannerLabel;
        private System.Windows.Forms.Button registerScannerButton;
        private System.Windows.Forms.CheckBox autoPrint;
        private System.Windows.Forms.CheckBox singleScan;
        private UI.Controls.SectionTitle sectionTitleDisplay;
        private UI.Controls.SectionTitle sectionTitleShipping;
        private System.Windows.Forms.CheckBox displayShortcutIndicator;
        private System.Windows.Forms.Label instructionsLabel;
        private System.Windows.Forms.Button manageProfilesButton;
        private System.Windows.Forms.Button printBarcodeButton;
        private System.Windows.Forms.CheckBox requireVerificationForAutoPrint;
        private UI.Controls.InfoTip infoTipRequireVerification;
        private System.Windows.Forms.CheckBox autoAdvance;
    }
}
