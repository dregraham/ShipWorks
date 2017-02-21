namespace ShipWorks.ApplicationCore.Options
{
    partial class OptionPagePersonal
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
            this.labelColorScheme = new System.Windows.Forms.Label();
            this.colorScheme = new System.Windows.Forms.ComboBox();
            this.sectionDisplay = new ShipWorks.UI.Controls.SectionTitle();
            this.systemTray = new System.Windows.Forms.CheckBox();
            this.sectionTitleRibbon = new ShipWorks.UI.Controls.SectionTitle();
            this.showQatBelowRibbon = new System.Windows.Forms.CheckBox();
            this.minimizeRibbon = new System.Windows.Forms.CheckBox();
            this.sectionTitleFilters = new ShipWorks.UI.Controls.SectionTitle();
            this.labelInitialFilter = new System.Windows.Forms.Label();
            this.radioInitialFilterRecent = new System.Windows.Forms.RadioButton();
            this.radioInitialFilterAlways = new System.Windows.Forms.RadioButton();
            this.filterComboBox = new ShipWorks.Filters.Controls.FilterComboBox();
            this.sectionTitleShipping = new ShipWorks.UI.Controls.SectionTitle();
            this.labelWeightFormat = new System.Windows.Forms.Label();
            this.comboWeightFormat = new System.Windows.Forms.ComboBox();
            this.labelFilterSortOrder = new System.Windows.Forms.Label();
            this.panelInitialFilter = new System.Windows.Forms.Panel();
            this.filterInitialSort = new System.Windows.Forms.ComboBox();
            this.infotipWeightFormat = new ShipWorks.UI.Controls.InfoTip();
            this.infotipMinimizeRibbon = new ShipWorks.UI.Controls.InfoTip();
            this.singleScan = new System.Windows.Forms.CheckBox();
            this.autoPrint = new System.Windows.Forms.CheckBox();
            this.registerScannerButton = new System.Windows.Forms.Button();
            this.registerScannerLabel = new System.Windows.Forms.Label();
            this.autoWeigh = new System.Windows.Forms.CheckBox();
            this.infoTipAutoWeigh = new ShipWorks.UI.Controls.InfoTip();
            this.infoTipAutoPrint = new ShipWorks.UI.Controls.InfoTip();
            this.infoTipSingleScan = new ShipWorks.UI.Controls.InfoTip();
            this.panelInitialFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelColorScheme
            // 
            this.labelColorScheme.AutoSize = true;
            this.labelColorScheme.Location = new System.Drawing.Point(25, 42);
            this.labelColorScheme.Name = "labelColorScheme";
            this.labelColorScheme.Size = new System.Drawing.Size(75, 13);
            this.labelColorScheme.TabIndex = 1;
            this.labelColorScheme.Text = "Color scheme:";
            // 
            // colorScheme
            // 
            this.colorScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorScheme.FormattingEnabled = true;
            this.colorScheme.Items.AddRange(new object[] {
            "Blue",
            "Silver",
            "Black"});
            this.colorScheme.Location = new System.Drawing.Point(106, 39);
            this.colorScheme.Name = "colorScheme";
            this.colorScheme.Size = new System.Drawing.Size(98, 21);
            this.colorScheme.TabIndex = 2;
            // 
            // sectionDisplay
            // 
            this.sectionDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionDisplay.Location = new System.Drawing.Point(10, 10);
            this.sectionDisplay.Name = "sectionDisplay";
            this.sectionDisplay.Size = new System.Drawing.Size(530, 22);
            this.sectionDisplay.TabIndex = 0;
            this.sectionDisplay.Text = "Display";
            // 
            // systemTray
            // 
            this.systemTray.AutoSize = true;
            this.systemTray.Location = new System.Drawing.Point(28, 68);
            this.systemTray.Name = "systemTray";
            this.systemTray.Size = new System.Drawing.Size(267, 17);
            this.systemTray.TabIndex = 3;
            this.systemTray.Text = "Hide ShipWorks in the system tray when minimized";
            this.systemTray.UseVisualStyleBackColor = true;
            // 
            // sectionTitleRibbon
            // 
            this.sectionTitleRibbon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitleRibbon.Location = new System.Drawing.Point(10, 99);
            this.sectionTitleRibbon.Name = "sectionTitleRibbon";
            this.sectionTitleRibbon.Size = new System.Drawing.Size(530, 22);
            this.sectionTitleRibbon.TabIndex = 4;
            this.sectionTitleRibbon.Text = "Ribbon";
            // 
            // showQatBelowRibbon
            // 
            this.showQatBelowRibbon.AutoSize = true;
            this.showQatBelowRibbon.Location = new System.Drawing.Point(28, 127);
            this.showQatBelowRibbon.Name = "showQatBelowRibbon";
            this.showQatBelowRibbon.Size = new System.Drawing.Size(242, 17);
            this.showQatBelowRibbon.TabIndex = 5;
            this.showQatBelowRibbon.Text = "Show Quick Access Toolbar below the Ribbon";
            this.showQatBelowRibbon.UseVisualStyleBackColor = true;
            // 
            // minimizeRibbon
            // 
            this.minimizeRibbon.AutoSize = true;
            this.minimizeRibbon.Location = new System.Drawing.Point(28, 150);
            this.minimizeRibbon.Name = "minimizeRibbon";
            this.minimizeRibbon.Size = new System.Drawing.Size(120, 17);
            this.minimizeRibbon.TabIndex = 6;
            this.minimizeRibbon.Text = "Minimize the Ribbon";
            this.minimizeRibbon.UseVisualStyleBackColor = true;
            // 
            // sectionTitleFilters
            // 
            this.sectionTitleFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitleFilters.Location = new System.Drawing.Point(10, 181);
            this.sectionTitleFilters.Name = "sectionTitleFilters";
            this.sectionTitleFilters.Size = new System.Drawing.Size(530, 22);
            this.sectionTitleFilters.TabIndex = 7;
            this.sectionTitleFilters.Text = "Filters";
            // 
            // labelInitialFilter
            // 
            this.labelInitialFilter.AutoSize = true;
            this.labelInitialFilter.Location = new System.Drawing.Point(25, 211);
            this.labelInitialFilter.Name = "labelInitialFilter";
            this.labelInitialFilter.Size = new System.Drawing.Size(234, 13);
            this.labelInitialFilter.TabIndex = 8;
            this.labelInitialFilter.Text = "When starting ShipWorks, select the filter that:";
            // 
            // radioInitialFilterRecent
            // 
            this.radioInitialFilterRecent.AutoSize = true;
            this.radioInitialFilterRecent.Location = new System.Drawing.Point(3, 3);
            this.radioInitialFilterRecent.Name = "radioInitialFilterRecent";
            this.radioInitialFilterRecent.Size = new System.Drawing.Size(230, 17);
            this.radioInitialFilterRecent.TabIndex = 0;
            this.radioInitialFilterRecent.TabStop = true;
            this.radioInitialFilterRecent.Text = "Was active the last time I used ShipWorks.";
            this.radioInitialFilterRecent.UseVisualStyleBackColor = true;
            this.radioInitialFilterRecent.CheckedChanged += new System.EventHandler(this.OnChangeInitialFilterSelection);
            // 
            // radioInitialFilterAlways
            // 
            this.radioInitialFilterAlways.AutoSize = true;
            this.radioInitialFilterAlways.Location = new System.Drawing.Point(3, 25);
            this.radioInitialFilterAlways.Name = "radioInitialFilterAlways";
            this.radioInitialFilterAlways.Size = new System.Drawing.Size(139, 17);
            this.radioInitialFilterAlways.TabIndex = 1;
            this.radioInitialFilterAlways.TabStop = true;
            this.radioInitialFilterAlways.Text = "Always select this filter:";
            this.radioInitialFilterAlways.UseVisualStyleBackColor = true;
            this.radioInitialFilterAlways.CheckedChanged += new System.EventHandler(this.OnChangeInitialFilterSelection);
            // 
            // filterComboBox
            // 
            this.filterComboBox.DropDownHeight = 350;
            this.filterComboBox.IntegralHeight = false;
            this.filterComboBox.Location = new System.Drawing.Point(141, 22);
            this.filterComboBox.Name = "filterComboBox";
            this.filterComboBox.Size = new System.Drawing.Size(274, 21);
            this.filterComboBox.TabIndex = 2;
            // 
            // sectionTitleShipping
            // 
            this.sectionTitleShipping.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitleShipping.Location = new System.Drawing.Point(10, 316);
            this.sectionTitleShipping.Name = "sectionTitleShipping";
            this.sectionTitleShipping.Size = new System.Drawing.Size(530, 22);
            this.sectionTitleShipping.TabIndex = 12;
            this.sectionTitleShipping.Text = "Shipping";
            // 
            // labelWeightFormat
            // 
            this.labelWeightFormat.AutoSize = true;
            this.labelWeightFormat.Location = new System.Drawing.Point(25, 349);
            this.labelWeightFormat.Name = "labelWeightFormat";
            this.labelWeightFormat.Size = new System.Drawing.Size(153, 13);
            this.labelWeightFormat.TabIndex = 13;
            this.labelWeightFormat.Text = "Format weight input boxes as:";
            // 
            // comboWeightFormat
            // 
            this.comboWeightFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboWeightFormat.FormattingEnabled = true;
            this.comboWeightFormat.Location = new System.Drawing.Point(184, 346);
            this.comboWeightFormat.Name = "comboWeightFormat";
            this.comboWeightFormat.Size = new System.Drawing.Size(173, 21);
            this.comboWeightFormat.TabIndex = 14;
            // 
            // labelFilterSortOrder
            // 
            this.labelFilterSortOrder.AutoSize = true;
            this.labelFilterSortOrder.Location = new System.Drawing.Point(25, 284);
            this.labelFilterSortOrder.Name = "labelFilterSortOrder";
            this.labelFilterSortOrder.Size = new System.Drawing.Size(212, 13);
            this.labelFilterSortOrder.TabIndex = 10;
            this.labelFilterSortOrder.Text = "When selecting a filter, sort the grid using:";
            // 
            // panelInitialFilter
            // 
            this.panelInitialFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInitialFilter.Controls.Add(this.radioInitialFilterRecent);
            this.panelInitialFilter.Controls.Add(this.filterComboBox);
            this.panelInitialFilter.Controls.Add(this.radioInitialFilterAlways);
            this.panelInitialFilter.Location = new System.Drawing.Point(47, 227);
            this.panelInitialFilter.Name = "panelInitialFilter";
            this.panelInitialFilter.Size = new System.Drawing.Size(488, 51);
            this.panelInitialFilter.TabIndex = 9;
            // 
            // filterInitialSort
            // 
            this.filterInitialSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterInitialSort.FormattingEnabled = true;
            this.filterInitialSort.Location = new System.Drawing.Point(236, 281);
            this.filterInitialSort.Name = "filterInitialSort";
            this.filterInitialSort.Size = new System.Drawing.Size(264, 21);
            this.filterInitialSort.TabIndex = 11;
            // 
            // infotipWeightFormat
            // 
            this.infotipWeightFormat.Caption = "Regardless of the format the weight is displayed as, you can enter the weight in " +
    "any format.";
            this.infotipWeightFormat.Location = new System.Drawing.Point(363, 350);
            this.infotipWeightFormat.Name = "infotipWeightFormat";
            this.infotipWeightFormat.Size = new System.Drawing.Size(12, 12);
            this.infotipWeightFormat.TabIndex = 20;
            this.infotipWeightFormat.Title = "Weight Input";
            // 
            // infotipMinimizeRibbon
            // 
            this.infotipMinimizeRibbon.Caption = "When the ribbon is minimized it has the appearance of a regular Windows menu.  \r\n" +
    "\r\nThe Ribbon can also be minimized by double-clicking a Ribbon tab. ";
            this.infotipMinimizeRibbon.Location = new System.Drawing.Point(147, 152);
            this.infotipMinimizeRibbon.Name = "infotipMinimizeRibbon";
            this.infotipMinimizeRibbon.Size = new System.Drawing.Size(12, 12);
            this.infotipMinimizeRibbon.TabIndex = 21;
            this.infotipMinimizeRibbon.Title = "Minimize the Ribbon";
            // 
            // singleScan
            // 
            this.singleScan.AutoSize = true;
            this.singleScan.Location = new System.Drawing.Point(28, 377);
            this.singleScan.Name = "singleScan";
            this.singleScan.Size = new System.Drawing.Size(175, 17);
            this.singleScan.TabIndex = 22;
            this.singleScan.Text = "Search orders by barcode scan";
            this.singleScan.UseVisualStyleBackColor = true;
            this.singleScan.CheckedChanged += new System.EventHandler(this.OnChangeSingleScanSettings);
            // 
            // autoPrint
            // 
            this.autoPrint.AutoSize = true;
            this.autoPrint.Location = new System.Drawing.Point(47, 402);
            this.autoPrint.Name = "autoPrint";
            this.autoPrint.Size = new System.Drawing.Size(227, 17);
            this.autoPrint.TabIndex = 23;
            this.autoPrint.Text = "Automatically print labels on barcode scan";
            this.autoPrint.UseVisualStyleBackColor = true;
            // 
            // registerScannerButton
            // 
            this.registerScannerButton.Location = new System.Drawing.Point(220, 373);
            this.registerScannerButton.Name = "registerScannerButton";
            this.registerScannerButton.Size = new System.Drawing.Size(75, 23);
            this.registerScannerButton.TabIndex = 26;
            this.registerScannerButton.Text = "Pair Scanner";
            this.registerScannerButton.UseVisualStyleBackColor = true;
            this.registerScannerButton.Click += new System.EventHandler(this.OnClickRegisterScanner);
            // 
            // registerScannerLabel
            // 
            this.registerScannerLabel.AutoSize = true;
            this.registerScannerLabel.ForeColor = System.Drawing.Color.Red;
            this.registerScannerLabel.Location = new System.Drawing.Point(301, 378);
            this.registerScannerLabel.Name = "registerScannerLabel";
            this.registerScannerLabel.Size = new System.Drawing.Size(98, 13);
            this.registerScannerLabel.TabIndex = 28;
            this.registerScannerLabel.Text = "Scanner not paired";
            // 
            // autoWeigh
            // 
            this.autoWeigh.AutoSize = true;
            this.autoWeigh.Location = new System.Drawing.Point(66, 425);
            this.autoWeigh.Name = "autoWeigh";
            this.autoWeigh.Size = new System.Drawing.Size(251, 17);
            this.autoWeigh.TabIndex = 29;
            this.autoWeigh.Text = "Automatically weigh packages on barcode scan";
            this.autoWeigh.UseVisualStyleBackColor = true;
            // 
            // infoTipAutoWeigh
            // 
            this.infoTipAutoWeigh.Caption = "Import the weight from your scale automatically on a barcode scan.\r\n\r\nThe weight " +
    "is imported if the scale registers a weight greater than 0.";
            this.infoTipAutoWeigh.Location = new System.Drawing.Point(316, 427);
            this.infoTipAutoWeigh.Name = "infoTipAutoWeigh";
            this.infoTipAutoWeigh.Size = new System.Drawing.Size(13, 12);
            this.infoTipAutoWeigh.TabIndex = 30;
            this.infoTipAutoWeigh.Title = "Automatically Weigh Packages on Barcode Scan";
            // 
            // infoTipAutoPrint
            // 
            this.infoTipAutoPrint.Caption = "Labels print automatically when a barcode is scanned.\r\n\r\nLabels are generated usi" +
    "ng the currently configured shipping rules.";
            this.infoTipAutoPrint.Location = new System.Drawing.Point(273, 404);
            this.infoTipAutoPrint.Name = "infoTipAutoPrint";
            this.infoTipAutoPrint.Size = new System.Drawing.Size(12, 12);
            this.infoTipAutoPrint.TabIndex = 31;
            this.infoTipAutoPrint.Title = "Automatically Print Labels on Barcode Scan";
            // 
            // infoTipSingleScan
            // 
            this.infoTipSingleScan.Caption = "Quickly locate orders by scanning a barcode.\r\n\r\nClick the \'Pair Scanner\' button t" +
    "o get started.";
            this.infoTipSingleScan.Location = new System.Drawing.Point(202, 379);
            this.infoTipSingleScan.Name = "infoTipSingleScan";
            this.infoTipSingleScan.Size = new System.Drawing.Size(12, 12);
            this.infoTipSingleScan.TabIndex = 32;
            this.infoTipSingleScan.Title = "Search Orders by Barcode Scan";
            // 
            // OptionPagePersonal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(0, 15);
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.infoTipSingleScan);
            this.Controls.Add(this.infoTipAutoPrint);
            this.Controls.Add(this.infoTipAutoWeigh);
            this.Controls.Add(this.autoWeigh);
            this.Controls.Add(this.registerScannerLabel);
            this.Controls.Add(this.registerScannerButton);
            this.Controls.Add(this.autoPrint);
            this.Controls.Add(this.singleScan);
            this.Controls.Add(this.infotipMinimizeRibbon);
            this.Controls.Add(this.infotipWeightFormat);
            this.Controls.Add(this.filterInitialSort);
            this.Controls.Add(this.panelInitialFilter);
            this.Controls.Add(this.labelFilterSortOrder);
            this.Controls.Add(this.comboWeightFormat);
            this.Controls.Add(this.labelWeightFormat);
            this.Controls.Add(this.sectionTitleShipping);
            this.Controls.Add(this.labelInitialFilter);
            this.Controls.Add(this.sectionTitleFilters);
            this.Controls.Add(this.minimizeRibbon);
            this.Controls.Add(this.showQatBelowRibbon);
            this.Controls.Add(this.sectionTitleRibbon);
            this.Controls.Add(this.systemTray);
            this.Controls.Add(this.colorScheme);
            this.Controls.Add(this.labelColorScheme);
            this.Controls.Add(this.sectionDisplay);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OptionPagePersonal";
            this.Size = new System.Drawing.Size(556, 652);
            this.Load += new System.EventHandler(this.OnLoad);
            this.panelInitialFilter.ResumeLayout(false);
            this.panelInitialFilter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.UI.Controls.SectionTitle sectionDisplay;
        private System.Windows.Forms.Label labelColorScheme;
        private System.Windows.Forms.ComboBox colorScheme;
        private System.Windows.Forms.CheckBox systemTray;
        private ShipWorks.UI.Controls.SectionTitle sectionTitleRibbon;
        private System.Windows.Forms.CheckBox showQatBelowRibbon;
        private System.Windows.Forms.CheckBox minimizeRibbon;
        private ShipWorks.UI.Controls.SectionTitle sectionTitleFilters;
        private System.Windows.Forms.Label labelInitialFilter;
        private System.Windows.Forms.RadioButton radioInitialFilterRecent;
        private System.Windows.Forms.RadioButton radioInitialFilterAlways;
        private ShipWorks.Filters.Controls.FilterComboBox filterComboBox;
        private ShipWorks.UI.Controls.SectionTitle sectionTitleShipping;
        private System.Windows.Forms.Label labelWeightFormat;
        private System.Windows.Forms.ComboBox comboWeightFormat;
        private System.Windows.Forms.Label labelFilterSortOrder;
        private System.Windows.Forms.Panel panelInitialFilter;
        private System.Windows.Forms.ComboBox filterInitialSort;
        private UI.Controls.InfoTip infotipWeightFormat;
        private UI.Controls.InfoTip infotipMinimizeRibbon;
        private System.Windows.Forms.CheckBox singleScan;
        private System.Windows.Forms.CheckBox autoPrint;
        private System.Windows.Forms.Button registerScannerButton;
        private System.Windows.Forms.Label registerScannerLabel;
        private System.Windows.Forms.CheckBox autoWeigh;
        private UI.Controls.InfoTip infoTipAutoWeigh;
        private UI.Controls.InfoTip infoTipAutoPrint;
        private UI.Controls.InfoTip infoTipSingleScan;
    }
}
