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
            this.infoTipSingleScan = new ShipWorks.UI.Controls.InfoTip();
            this.infoTipAutoPrint = new ShipWorks.UI.Controls.InfoTip();
            this.infoTipAutoWeigh = new ShipWorks.UI.Controls.InfoTip();
            this.autoWeigh = new System.Windows.Forms.CheckBox();
            this.registerScannerLabel = new System.Windows.Forms.Label();
            this.registerScannerButton = new System.Windows.Forms.Button();
            this.autoPrint = new System.Windows.Forms.CheckBox();
            this.enableScanner = new System.Windows.Forms.CheckBox();
            this.sectionTitleShipping = new ShipWorks.UI.Controls.SectionTitle();
            this.profileShortcutsLabel = new System.Windows.Forms.Label();
            this.manageProfilesButton = new System.Windows.Forms.Button();
            this.printBarcodeButton = new System.Windows.Forms.Button();
            this.requireVerificationToShip = new System.Windows.Forms.CheckBox();
            this.infoTipVerificationWarehouseOnly = new ShipWorks.UI.Controls.InfoTip();
            this.printShortcutsLabel = new System.Windows.Forms.Label();
            this.sectionTitle1 = new ShipWorks.UI.Controls.SectionTitle();
            this.SuspendLayout();
            // 
            // infoTipSingleScan
            // 
            this.infoTipSingleScan.Caption = "Quickly locate orders, apply Shipping Profiles, and more by scanning barcodes.\r\n\r" +
    "\nClick the \'Pair Scanner\' button to get started.";
            this.infoTipSingleScan.Location = new System.Drawing.Point(165, 48);
            this.infoTipSingleScan.Name = "infoTipSingleScan";
            this.infoTipSingleScan.Size = new System.Drawing.Size(12, 12);
            this.infoTipSingleScan.TabIndex = 40;
            this.infoTipSingleScan.Title = "Enable Barcode Scanner";
            // 
            // infoTipAutoPrint
            // 
            this.infoTipAutoPrint.Caption = "Automatically print a label when you search for an order using a barcode scan.\r\n\r" +
    "\nLabels are generated using the currently configured shipping rules.";
            this.infoTipAutoPrint.Location = new System.Drawing.Point(225, 96);
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
            this.infoTipAutoWeigh.Location = new System.Drawing.Point(257, 73);
            this.infoTipAutoWeigh.Name = "infoTipAutoWeigh";
            this.infoTipAutoWeigh.Size = new System.Drawing.Size(13, 12);
            this.infoTipAutoWeigh.TabIndex = 38;
            this.infoTipAutoWeigh.Title = "Automatically Weigh Packages on Barcode Scan Search";
            // 
            // autoWeigh
            // 
            this.autoWeigh.AutoSize = true;
            this.autoWeigh.Location = new System.Drawing.Point(39, 71);
            this.autoWeigh.Name = "autoWeigh";
            this.autoWeigh.Size = new System.Drawing.Size(216, 17);
            this.autoWeigh.TabIndex = 37;
            this.autoWeigh.Text = "Auto-weigh order items on barcode scan";
            this.autoWeigh.UseVisualStyleBackColor = true;
            this.autoWeigh.CheckedChanged += new System.EventHandler(this.OnChangeSingleScanSettings);
            // 
            // registerScannerLabel
            // 
            this.registerScannerLabel.AutoSize = true;
            this.registerScannerLabel.ForeColor = System.Drawing.Color.Red;
            this.registerScannerLabel.Location = new System.Drawing.Point(183, 47);
            this.registerScannerLabel.Name = "registerScannerLabel";
            this.registerScannerLabel.Size = new System.Drawing.Size(97, 13);
            this.registerScannerLabel.TabIndex = 36;
            this.registerScannerLabel.Text = "Scanner not paired";
            // 
            // registerScannerButton
            // 
            this.registerScannerButton.Location = new System.Drawing.Point(427, 42);
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
            this.autoPrint.Location = new System.Drawing.Point(39, 94);
            this.autoPrint.Name = "autoPrint";
            this.autoPrint.Size = new System.Drawing.Size(184, 17);
            this.autoPrint.TabIndex = 34;
            this.autoPrint.Text = "Auto-print labels on barcode scan";
            this.autoPrint.UseVisualStyleBackColor = true;
            this.autoPrint.CheckedChanged += new System.EventHandler(this.OnChangeSingleScanSettings);
            // 
            // enableScanner
            // 
            this.enableScanner.AutoSize = true;
            this.enableScanner.Location = new System.Drawing.Point(20, 46);
            this.enableScanner.Name = "enableScanner";
            this.enableScanner.Size = new System.Drawing.Size(142, 17);
            this.enableScanner.TabIndex = 33;
            this.enableScanner.Text = "Enable barcode scanner";
            this.enableScanner.UseVisualStyleBackColor = true;
            this.enableScanner.CheckedChanged += new System.EventHandler(this.OnChangeSingleScanSettings);
            // 
            // sectionTitleShipping
            // 
            this.sectionTitleShipping.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitleShipping.Location = new System.Drawing.Point(10, 10);
            this.sectionTitleShipping.Name = "sectionTitleShipping";
            this.sectionTitleShipping.Size = new System.Drawing.Size(527, 22);
            this.sectionTitleShipping.TabIndex = 42;
            this.sectionTitleShipping.Text = "Shipping Automation";
            // 
            // profileShortcutsLabel
            // 
            this.profileShortcutsLabel.Location = new System.Drawing.Point(17, 123);
            this.profileShortcutsLabel.Name = "profileShortcutsLabel";
            this.profileShortcutsLabel.Size = new System.Drawing.Size(404, 29);
            this.profileShortcutsLabel.TabIndex = 44;
            this.profileShortcutsLabel.Text = "You can add custom keyboard and barcode shortcuts to your Shipping Profiles to ap" +
    "ply shipping settings easily.";
            // 
            // manageProfilesButton
            // 
            this.manageProfilesButton.Location = new System.Drawing.Point(427, 123);
            this.manageProfilesButton.Name = "manageProfilesButton";
            this.manageProfilesButton.Size = new System.Drawing.Size(110, 24);
            this.manageProfilesButton.TabIndex = 45;
            this.manageProfilesButton.Text = "Manage Profiles...";
            this.manageProfilesButton.UseVisualStyleBackColor = true;
            this.manageProfilesButton.Click += new System.EventHandler(this.OnClickManageProfiles);
            // 
            // printBarcodeButton
            // 
            this.printBarcodeButton.Location = new System.Drawing.Point(427, 162);
            this.printBarcodeButton.Name = "printBarcodeButton";
            this.printBarcodeButton.Size = new System.Drawing.Size(110, 23);
            this.printBarcodeButton.TabIndex = 46;
            this.printBarcodeButton.Text = "Print Shortcuts...";
            this.printBarcodeButton.UseVisualStyleBackColor = true;
            this.printBarcodeButton.Click += new System.EventHandler(this.OnClickPrintShortcuts);
            // 
            // requireVerificationToShip
            // 
            this.requireVerificationToShip.AutoSize = true;
            this.requireVerificationToShip.Enabled = false;
            this.requireVerificationToShip.Location = new System.Drawing.Point(21, 242);
            this.requireVerificationToShip.Name = "requireVerificationToShip";
            this.requireVerificationToShip.Size = new System.Drawing.Size(281, 17);
            this.requireVerificationToShip.TabIndex = 47;
            this.requireVerificationToShip.Text = "Require orders/shipments to be verified before printing";
            this.requireVerificationToShip.UseVisualStyleBackColor = true;
            this.requireVerificationToShip.CheckedChanged += new System.EventHandler(this.OnChangeSingleScanSettings);
            // 
            // infoTipVerificationWarehouseOnly
            // 
            this.infoTipVerificationWarehouseOnly.Caption = "This feature is only available on warehouse plans.";
            this.infoTipVerificationWarehouseOnly.Location = new System.Drawing.Point(304, 243);
            this.infoTipVerificationWarehouseOnly.Name = "infoTipVerificationWarehouseOnly";
            this.infoTipVerificationWarehouseOnly.Size = new System.Drawing.Size(12, 12);
            this.infoTipVerificationWarehouseOnly.TabIndex = 48;
            this.infoTipVerificationWarehouseOnly.Title = "Require Verification";
            // 
            // printShortcutsLabel
            // 
            this.printShortcutsLabel.Location = new System.Drawing.Point(17, 162);
            this.printShortcutsLabel.Name = "printShortcutsLabel";
            this.printShortcutsLabel.Size = new System.Drawing.Size(404, 31);
            this.printShortcutsLabel.TabIndex = 49;
            this.printShortcutsLabel.Text = "You can print preconfigured keyboard and barcode shortcuts that automate shipping" +
    ".";
            // 
            // sectionTitle1
            // 
            this.sectionTitle1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitle1.Location = new System.Drawing.Point(10, 206);
            this.sectionTitle1.Name = "sectionTitle1";
            this.sectionTitle1.Size = new System.Drawing.Size(527, 22);
            this.sectionTitle1.TabIndex = 43;
            this.sectionTitle1.Text = "Shipping Workflow";
            // 
            // SettingsPageScanToShip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sectionTitle1);
            this.Controls.Add(this.printBarcodeButton);
            this.Controls.Add(this.printShortcutsLabel);
            this.Controls.Add(this.infoTipVerificationWarehouseOnly);
            this.Controls.Add(this.requireVerificationToShip);
            this.Controls.Add(this.manageProfilesButton);
            this.Controls.Add(this.profileShortcutsLabel);
            this.Controls.Add(this.sectionTitleShipping);
            this.Controls.Add(this.infoTipSingleScan);
            this.Controls.Add(this.infoTipAutoPrint);
            this.Controls.Add(this.infoTipAutoWeigh);
            this.Controls.Add(this.autoWeigh);
            this.Controls.Add(this.registerScannerLabel);
            this.Controls.Add(this.registerScannerButton);
            this.Controls.Add(this.autoPrint);
            this.Controls.Add(this.enableScanner);
            this.Name = "SettingsPageScanToShip";
            this.Size = new System.Drawing.Size(547, 333);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox autoWeigh;
        private System.Windows.Forms.Label registerScannerLabel;
        private System.Windows.Forms.Button registerScannerButton;
        private System.Windows.Forms.CheckBox autoPrint;
        private System.Windows.Forms.CheckBox enableScanner;
        private System.Windows.Forms.Label profileShortcutsLabel;
        private System.Windows.Forms.Button manageProfilesButton;
        private System.Windows.Forms.Button printBarcodeButton;
        private System.Windows.Forms.CheckBox requireVerificationToShip;
        private System.Windows.Forms.Label printShortcutsLabel;
        private ShipWorks.UI.Controls.SectionTitle sectionTitle1;
        private ShipWorks.UI.Controls.InfoTip infoTipVerificationWarehouseOnly;
        private ShipWorks.UI.Controls.SectionTitle sectionTitleShipping;
        private ShipWorks.UI.Controls.InfoTip infoTipAutoWeigh;
        private ShipWorks.UI.Controls.InfoTip infoTipAutoPrint;
        private ShipWorks.UI.Controls.InfoTip infoTipSingleScan;
    }
}
