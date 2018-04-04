namespace ShipWorks.ApplicationCore.Options
{
    partial class OptionPageShortcuts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionPageShortcuts));
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
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // infoTipSingleScan
            // 
            this.infoTipSingleScan.Caption = "Quickly locate orders, apply Shipping Profiles, and more by scanning barcodes.\r\n\r\nClick the \'Pair Scanner\' button t" +
    "o get started.";
            this.infoTipSingleScan.Location = new System.Drawing.Point(165, 107);
            this.infoTipSingleScan.Name = "infoTipSingleScan";
            this.infoTipSingleScan.Size = new System.Drawing.Size(12, 12);
            this.infoTipSingleScan.TabIndex = 40;
            this.infoTipSingleScan.Title = "Enable Barcode Scanner";
            // 
            // infoTipAutoPrint
            // 
            this.infoTipAutoPrint.Caption = "Labels print automatically when a barcode is scanned.\r\n\r\nLabels are generated usi" +
    "ng the currently configured shipping rules.";
            this.infoTipAutoPrint.Location = new System.Drawing.Point(265, 155);
            this.infoTipAutoPrint.Name = "infoTipAutoPrint";
            this.infoTipAutoPrint.Size = new System.Drawing.Size(12, 12);
            this.infoTipAutoPrint.TabIndex = 39;
            this.infoTipAutoPrint.Title = "Automatically Print Labels on Barcode Scan";
            // 
            // infoTipAutoWeigh
            // 
            this.infoTipAutoWeigh.Caption = "Import the weight from your scale automatically on a barcode scan.\r\n\r\nThe weight " +
    "is imported if the scale registers a weight greater than 0.";
            this.infoTipAutoWeigh.Location = new System.Drawing.Point(293, 132);
            this.infoTipAutoWeigh.Name = "infoTipAutoWeigh";
            this.infoTipAutoWeigh.Size = new System.Drawing.Size(13, 12);
            this.infoTipAutoWeigh.TabIndex = 38;
            this.infoTipAutoWeigh.Title = "Automatically Weigh Packages on Barcode Scan";
            // 
            // autoWeigh
            // 
            this.autoWeigh.AutoSize = true;
            this.autoWeigh.Location = new System.Drawing.Point(39, 130);
            this.autoWeigh.Name = "autoWeigh";
            this.autoWeigh.Size = new System.Drawing.Size(252, 17);
            this.autoWeigh.TabIndex = 37;
            this.autoWeigh.Text = "Automatically weigh packages on barcode scan";
            this.autoWeigh.UseVisualStyleBackColor = true;
            this.autoWeigh.CheckedChanged += new System.EventHandler(this.OnChangeSingleScanSettings);
            // 
            // registerScannerLabel
            // 
            this.registerScannerLabel.AutoSize = true;
            this.registerScannerLabel.ForeColor = System.Drawing.Color.Red;
            this.registerScannerLabel.Location = new System.Drawing.Point(308, 106);
            this.registerScannerLabel.Name = "registerScannerLabel";
            this.registerScannerLabel.Size = new System.Drawing.Size(97, 13);
            this.registerScannerLabel.TabIndex = 36;
            this.registerScannerLabel.Text = "Scanner not paired";
            // 
            // registerScannerButton
            // 
            this.registerScannerButton.Location = new System.Drawing.Point(206, 101);
            this.registerScannerButton.Name = "registerScannerButton";
            this.registerScannerButton.Size = new System.Drawing.Size(100, 23);
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
            this.autoPrint.Size = new System.Drawing.Size(224, 17);
            this.autoPrint.TabIndex = 34;
            this.autoPrint.Text = "Automatically print labels on barcode scan";
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
            this.sectionTitleShipping.Text = "Shipping";
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
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(17, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(523, 50);
            this.label1.TabIndex = 44;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 233);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 45;
            this.button1.Text = "Manage Profiles";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnClickManageProfiles);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(130, 233);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(212, 23);
            this.button2.TabIndex = 46;
            this.button2.Text = "Print All Keyboard && Barcode Shortcuts";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnClickPrintShortcuts);
            // 
            // OptionPageShortcuts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
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
            this.Name = "OptionPageShortcuts";
            this.Size = new System.Drawing.Size(550, 300);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}
