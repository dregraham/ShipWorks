namespace ShipWorks.ApplicationCore.Options
{
    partial class OptionPageInterapptive
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
            this.sectionShipping = new ShipWorks.UI.Controls.SectionTitle();
            this.postalWebTestServer = new System.Windows.Forms.CheckBox();
            this.stampsTestServer = new System.Windows.Forms.CheckBox();
            this.fedexTestServer = new System.Windows.Forms.CheckBox();
            this.upsOnLineTools = new System.Windows.Forms.CheckBox();
            this.sectionDatabase = new ShipWorks.UI.Controls.SectionTitle();
            this.deployAssembly = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelOptions = new System.Windows.Forms.Label();
            this.fedexListRates = new System.Windows.Forms.CheckBox();
            this.endiciaTestServer = new System.Windows.Forms.CheckBox();
            this.sectionPlatforms = new ShipWorks.UI.Controls.SectionTitle();
            this.ebay = new System.Windows.Forms.CheckBox();
            this.labelPlatformsTestServers = new System.Windows.Forms.Label();
            this.marketplaceAdvisor = new System.Windows.Forms.CheckBox();
            this.marketplaceAdvisorMarkProcessed = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.yahooDeleteMessages = new System.Windows.Forms.CheckBox();
            this.payPal = new System.Windows.Forms.CheckBox();
            this.searchFitDeleteAfterDownload = new System.Windows.Forms.CheckBox();
            this.sectionSettings = new ShipWorks.UI.Controls.SectionTitle();
            this.multipleInstances = new System.Windows.Forms.CheckBox();
            this.regenerateFilters = new System.Windows.Forms.Button();
            this.deployChosenAssembly = new System.Windows.Forms.Button();
            this.express1EndiciaTestServer = new System.Windows.Forms.CheckBox();
            this.purgeLabels = new System.Windows.Forms.Button();
            this.purgePrintJobsButton = new System.Windows.Forms.Button();
            this.buyDotComArchiveOrderFile = new System.Windows.Forms.CheckBox();
            this.buyDotComMapChooser = new ShipWorks.Data.Import.Spreadsheet.Types.Csv.OrderSchema.GenericCsvOrderMapChooserControl();
            this.sectionBuyDotCom = new ShipWorks.UI.Controls.SectionTitle();
            this.panelBuyDotCom = new System.Windows.Forms.Panel();
            this.onTracTestServer = new System.Windows.Forms.CheckBox();
            this.newegg = new System.Windows.Forms.CheckBox();
            this.endiciaTestServers = new System.Windows.Forms.ComboBox();
            this.express1StampsTestServer = new System.Windows.Forms.CheckBox();
            this.labelInsurance = new System.Windows.Forms.Label();
            this.useInsureShipTestServer = new System.Windows.Forms.CheckBox();
            this.panelBuyDotCom.SuspendLayout();
            this.SuspendLayout();
            // 
            // sectionShipping
            // 
            this.sectionShipping.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionShipping.Location = new System.Drawing.Point(10, 10);
            this.sectionShipping.Name = "sectionShipping";
            this.sectionShipping.Size = new System.Drawing.Size(350, 22);
            this.sectionShipping.TabIndex = 0;
            this.sectionShipping.Text = "Shipping";
            // 
            // postalWebTestServer
            // 
            this.postalWebTestServer.AutoSize = true;
            this.postalWebTestServer.Location = new System.Drawing.Point(46, 62);
            this.postalWebTestServer.Name = "postalWebTestServer";
            this.postalWebTestServer.Size = new System.Drawing.Size(122, 17);
            this.postalWebTestServer.TabIndex = 2;
            this.postalWebTestServer.Text = "USPS (w/o Postage)";
            this.postalWebTestServer.UseVisualStyleBackColor = true;
            // 
            // stampsTestServer
            // 
            this.stampsTestServer.AutoSize = true;
            this.stampsTestServer.Location = new System.Drawing.Point(46, 83);
            this.stampsTestServer.Name = "stampsTestServer";
            this.stampsTestServer.Size = new System.Drawing.Size(120, 17);
            this.stampsTestServer.TabIndex = 3;
            this.stampsTestServer.Text = "USPS";
            this.stampsTestServer.UseVisualStyleBackColor = true;
            // 
            // fedexTestServer
            // 
            this.fedexTestServer.AutoSize = true;
            this.fedexTestServer.Location = new System.Drawing.Point(46, 192);
            this.fedexTestServer.Name = "fedexTestServer";
            this.fedexTestServer.Size = new System.Drawing.Size(56, 17);
            this.fedexTestServer.TabIndex = 5;
            this.fedexTestServer.Text = "FedEx";
            this.fedexTestServer.UseVisualStyleBackColor = true;
            // 
            // upsOnLineTools
            // 
            this.upsOnLineTools.AutoSize = true;
            this.upsOnLineTools.Location = new System.Drawing.Point(46, 213);
            this.upsOnLineTools.Name = "upsOnLineTools";
            this.upsOnLineTools.Size = new System.Drawing.Size(108, 17);
            this.upsOnLineTools.TabIndex = 6;
            this.upsOnLineTools.Text = "UPS (Integrated)";
            this.upsOnLineTools.UseVisualStyleBackColor = true;
            // 
            // sectionDatabase
            // 
            this.sectionDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionDatabase.Location = new System.Drawing.Point(13, 631);
            this.sectionDatabase.Name = "sectionDatabase";
            this.sectionDatabase.Size = new System.Drawing.Size(350, 22);
            this.sectionDatabase.TabIndex = 9;
            this.sectionDatabase.Text = "Database";
            // 
            // deployAssembly
            // 
            this.deployAssembly.Location = new System.Drawing.Point(28, 659);
            this.deployAssembly.Name = "deployAssembly";
            this.deployAssembly.Size = new System.Drawing.Size(119, 23);
            this.deployAssembly.TabIndex = 10;
            this.deployAssembly.Text = "Deploy Assembly";
            this.deployAssembly.UseVisualStyleBackColor = true;
            this.deployAssembly.Click += new System.EventHandler(this.OnDeployAssemblies);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Test Servers";
            // 
            // labelOptions
            // 
            this.labelOptions.AutoSize = true;
            this.labelOptions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOptions.Location = new System.Drawing.Point(24, 256);
            this.labelOptions.Name = "labelOptions";
            this.labelOptions.Size = new System.Drawing.Size(99, 13);
            this.labelOptions.TabIndex = 7;
            this.labelOptions.Text = "Internal Options";
            // 
            // fedexListRates
            // 
            this.fedexListRates.AutoSize = true;
            this.fedexListRates.Location = new System.Drawing.Point(46, 275);
            this.fedexListRates.Name = "fedexListRates";
            this.fedexListRates.Size = new System.Drawing.Size(277, 17);
            this.fedexListRates.TabIndex = 8;
            this.fedexListRates.Text = "Use FedEx LIST rates (ACCOUNT is used by default)";
            this.fedexListRates.UseVisualStyleBackColor = true;
            // 
            // endiciaTestServer
            // 
            this.endiciaTestServer.AutoSize = true;
            this.endiciaTestServer.Location = new System.Drawing.Point(46, 104);
            this.endiciaTestServer.Name = "endiciaTestServer";
            this.endiciaTestServer.Size = new System.Drawing.Size(95, 17);
            this.endiciaTestServer.TabIndex = 4;
            this.endiciaTestServer.Text = "USPS (Endicia)";
            this.endiciaTestServer.UseVisualStyleBackColor = true;
            this.endiciaTestServer.CheckedChanged += new System.EventHandler(this.OnEndiciaTestServerCheckedChanged);
            // 
            // sectionPlatforms
            // 
            this.sectionPlatforms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionPlatforms.Location = new System.Drawing.Point(12, 335);
            this.sectionPlatforms.Name = "sectionPlatforms";
            this.sectionPlatforms.Size = new System.Drawing.Size(350, 22);
            this.sectionPlatforms.TabIndex = 11;
            this.sectionPlatforms.Text = "Platforms";
            // 
            // ebay
            // 
            this.ebay.AutoSize = true;
            this.ebay.Location = new System.Drawing.Point(46, 381);
            this.ebay.Name = "ebay";
            this.ebay.Size = new System.Drawing.Size(50, 17);
            this.ebay.TabIndex = 12;
            this.ebay.Text = "eBay";
            this.ebay.UseVisualStyleBackColor = true;
            // 
            // labelPlatformsTestServers
            // 
            this.labelPlatformsTestServers.AutoSize = true;
            this.labelPlatformsTestServers.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlatformsTestServers.Location = new System.Drawing.Point(24, 362);
            this.labelPlatformsTestServers.Name = "labelPlatformsTestServers";
            this.labelPlatformsTestServers.Size = new System.Drawing.Size(79, 13);
            this.labelPlatformsTestServers.TabIndex = 13;
            this.labelPlatformsTestServers.Text = "Test Servers";
            // 
            // marketplaceAdvisor
            // 
            this.marketplaceAdvisor.AutoSize = true;
            this.marketplaceAdvisor.Location = new System.Drawing.Point(46, 401);
            this.marketplaceAdvisor.Name = "marketplaceAdvisor";
            this.marketplaceAdvisor.Size = new System.Drawing.Size(120, 17);
            this.marketplaceAdvisor.TabIndex = 14;
            this.marketplaceAdvisor.Text = "MarketplaceAdvisor";
            this.marketplaceAdvisor.UseVisualStyleBackColor = true;
            // 
            // marketplaceAdvisorMarkProcessed
            // 
            this.marketplaceAdvisorMarkProcessed.AutoSize = true;
            this.marketplaceAdvisorMarkProcessed.Location = new System.Drawing.Point(47, 478);
            this.marketplaceAdvisorMarkProcessed.Name = "marketplaceAdvisorMarkProcessed";
            this.marketplaceAdvisorMarkProcessed.Size = new System.Drawing.Size(317, 17);
            this.marketplaceAdvisorMarkProcessed.TabIndex = 16;
            this.marketplaceAdvisorMarkProcessed.Text = "MarketplaceAdvisor OMS: Mark as processed after download";
            this.marketplaceAdvisorMarkProcessed.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 459);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Internal Options";
            // 
            // yahooDeleteMessages
            // 
            this.yahooDeleteMessages.AutoSize = true;
            this.yahooDeleteMessages.Location = new System.Drawing.Point(47, 520);
            this.yahooDeleteMessages.Name = "yahooDeleteMessages";
            this.yahooDeleteMessages.Size = new System.Drawing.Size(255, 17);
            this.yahooDeleteMessages.TabIndex = 17;
            this.yahooDeleteMessages.Text = "Yahoo!: Delete email messages after download.";
            this.yahooDeleteMessages.UseVisualStyleBackColor = true;
            // 
            // payPal
            // 
            this.payPal.AutoSize = true;
            this.payPal.Location = new System.Drawing.Point(46, 438);
            this.payPal.Name = "payPal";
            this.payPal.Size = new System.Drawing.Size(58, 17);
            this.payPal.TabIndex = 18;
            this.payPal.Text = "PayPal";
            this.payPal.UseVisualStyleBackColor = true;
            // 
            // searchFitDeleteAfterDownload
            // 
            this.searchFitDeleteAfterDownload.AutoSize = true;
            this.searchFitDeleteAfterDownload.Location = new System.Drawing.Point(47, 499);
            this.searchFitDeleteAfterDownload.Name = "searchFitDeleteAfterDownload";
            this.searchFitDeleteAfterDownload.Size = new System.Drawing.Size(254, 17);
            this.searchFitDeleteAfterDownload.TabIndex = 19;
            this.searchFitDeleteAfterDownload.Text = "SearchFit: Delete orders online after download.";
            this.searchFitDeleteAfterDownload.UseVisualStyleBackColor = true;
            // 
            // sectionSettings
            // 
            this.sectionSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionSettings.Location = new System.Drawing.Point(12, 572);
            this.sectionSettings.Name = "sectionSettings";
            this.sectionSettings.Size = new System.Drawing.Size(350, 22);
            this.sectionSettings.TabIndex = 20;
            this.sectionSettings.Text = "Settings";
            // 
            // multipleInstances
            // 
            this.multipleInstances.AutoSize = true;
            this.multipleInstances.Location = new System.Drawing.Point(47, 603);
            this.multipleInstances.Name = "multipleInstances";
            this.multipleInstances.Size = new System.Drawing.Size(192, 17);
            this.multipleInstances.TabIndex = 21;
            this.multipleInstances.Text = "Allow multiple application instances";
            this.multipleInstances.UseVisualStyleBackColor = true;
            // 
            // regenerateFilters
            // 
            this.regenerateFilters.Location = new System.Drawing.Point(28, 688);
            this.regenerateFilters.Name = "regenerateFilters";
            this.regenerateFilters.Size = new System.Drawing.Size(119, 23);
            this.regenerateFilters.TabIndex = 22;
            this.regenerateFilters.Text = "Regenerate Filters";
            this.regenerateFilters.UseVisualStyleBackColor = true;
            this.regenerateFilters.Click += new System.EventHandler(this.OnRegenerateFilters);
            // 
            // deployChosenAssembly
            // 
            this.deployChosenAssembly.Location = new System.Drawing.Point(153, 659);
            this.deployChosenAssembly.Name = "deployChosenAssembly";
            this.deployChosenAssembly.Size = new System.Drawing.Size(119, 23);
            this.deployChosenAssembly.TabIndex = 23;
            this.deployChosenAssembly.Text = "Deploy Assembly...";
            this.deployChosenAssembly.UseVisualStyleBackColor = true;
            this.deployChosenAssembly.Click += new System.EventHandler(this.OnDeployChosenAssembly);
            // 
            // express1EndiciaTestServer
            // 
            this.express1EndiciaTestServer.AutoSize = true;
            this.express1EndiciaTestServer.Location = new System.Drawing.Point(46, 146);
            this.express1EndiciaTestServer.Name = "express1EndiciaTestServer";
            this.express1EndiciaTestServer.Size = new System.Drawing.Size(149, 17);
            this.express1EndiciaTestServer.TabIndex = 24;
            this.express1EndiciaTestServer.Text = "USPS (Express1 - Endicia)";
            this.express1EndiciaTestServer.UseVisualStyleBackColor = true;
            // 
            // purgeLabels
            // 
            this.purgeLabels.Location = new System.Drawing.Point(154, 687);
            this.purgeLabels.Name = "purgeLabels";
            this.purgeLabels.Size = new System.Drawing.Size(118, 23);
            this.purgeLabels.TabIndex = 25;
            this.purgeLabels.Text = "&Purge Labels...";
            this.purgeLabels.UseVisualStyleBackColor = true;
            this.purgeLabels.Click += new System.EventHandler(this.OnPurgeLabels);
            // 
            // purgePrintJobsButton
            // 
            this.purgePrintJobsButton.Location = new System.Drawing.Point(28, 717);
            this.purgePrintJobsButton.Name = "purgePrintJobsButton";
            this.purgePrintJobsButton.Size = new System.Drawing.Size(119, 23);
            this.purgePrintJobsButton.TabIndex = 26;
            this.purgePrintJobsButton.Text = "&Purge Print Jobs...";
            this.purgePrintJobsButton.UseVisualStyleBackColor = true;
            this.purgePrintJobsButton.Click += new System.EventHandler(this.OnPurgePrintJobs);
            // 
            // buyDotComArchiveOrderFile
            // 
            this.buyDotComArchiveOrderFile.AutoSize = true;
            this.buyDotComArchiveOrderFile.Location = new System.Drawing.Point(47, 539);
            this.buyDotComArchiveOrderFile.Name = "buyDotComArchiveOrderFile";
            this.buyDotComArchiveOrderFile.Size = new System.Drawing.Size(236, 17);
            this.buyDotComArchiveOrderFile.TabIndex = 28;
            this.buyDotComArchiveOrderFile.Text = "Buy.com: Archive order file after download.";
            this.buyDotComArchiveOrderFile.UseVisualStyleBackColor = true;
            // 
            // buyDotComMapChooser
            // 
            this.buyDotComMapChooser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buyDotComMapChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buyDotComMapChooser.Location = new System.Drawing.Point(1, 5);
            this.buyDotComMapChooser.Map = null;
            this.buyDotComMapChooser.Name = "buyDotComMapChooser";
            this.buyDotComMapChooser.Size = new System.Drawing.Size(362, 127);
            this.buyDotComMapChooser.TabIndex = 29;
            // 
            // sectionBuyDotCom
            // 
            this.sectionBuyDotCom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionBuyDotCom.Location = new System.Drawing.Point(13, 747);
            this.sectionBuyDotCom.Name = "sectionBuyDotCom";
            this.sectionBuyDotCom.Size = new System.Drawing.Size(350, 22);
            this.sectionBuyDotCom.TabIndex = 30;
            this.sectionBuyDotCom.Text = "Buy.com Map";
            // 
            // panelBuyDotCom
            // 
            this.panelBuyDotCom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBuyDotCom.Controls.Add(this.buyDotComMapChooser);
            this.panelBuyDotCom.Location = new System.Drawing.Point(13, 760);
            this.panelBuyDotCom.Name = "panelBuyDotCom";
            this.panelBuyDotCom.Size = new System.Drawing.Size(347, 113);
            this.panelBuyDotCom.TabIndex = 31;
            // 
            // onTracTestServer
            // 
            this.onTracTestServer.AutoSize = true;
            this.onTracTestServer.Location = new System.Drawing.Point(46, 236);
            this.onTracTestServer.Name = "onTracTestServer";
            this.onTracTestServer.Size = new System.Drawing.Size(61, 17);
            this.onTracTestServer.TabIndex = 29;
            this.onTracTestServer.Text = "OnTrac";
            this.onTracTestServer.UseVisualStyleBackColor = true;
            // 
            // newegg
            // 
            this.newegg.AutoSize = true;
            this.newegg.Location = new System.Drawing.Point(46, 420);
            this.newegg.Name = "newegg";
            this.newegg.Size = new System.Drawing.Size(65, 17);
            this.newegg.TabIndex = 18;
            this.newegg.Text = "Newegg";
            this.newegg.UseVisualStyleBackColor = true;
            // 
            // endiciaTestServers
            // 
            this.endiciaTestServers.AutoCompleteCustomSource.AddRange(new string[] {
            "www.envmgr.com",
            "elstestserver.endicia.com (New Sandbox)"});
            this.endiciaTestServers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.endiciaTestServers.Enabled = false;
            this.endiciaTestServers.FormattingEnabled = true;
            this.endiciaTestServers.Location = new System.Drawing.Point(65, 121);
            this.endiciaTestServers.Name = "endiciaTestServers";
            this.endiciaTestServers.Size = new System.Drawing.Size(213, 21);
            this.endiciaTestServers.TabIndex = 32;
            // 
            // express1StampsTestServer
            // 
            this.express1StampsTestServer.AutoSize = true;
            this.express1StampsTestServer.Location = new System.Drawing.Point(46, 169);
            this.express1StampsTestServer.Name = "express1StampsTestServer";
            this.express1StampsTestServer.Size = new System.Drawing.Size(174, 17);
            this.express1StampsTestServer.TabIndex = 34;
            this.express1StampsTestServer.Text = "USPS (Express1 - USPS)";
            this.express1StampsTestServer.UseVisualStyleBackColor = true;
            // 
            // labelInsurance
            // 
            this.labelInsurance.AutoSize = true;
            this.labelInsurance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInsurance.Location = new System.Drawing.Point(24, 295);
            this.labelInsurance.Name = "labelInsurance";
            this.labelInsurance.Size = new System.Drawing.Size(64, 13);
            this.labelInsurance.TabIndex = 35;
            this.labelInsurance.Text = "Insurance";
            // 
            // useInsureShipTestServer
            // 
            this.useInsureShipTestServer.AutoSize = true;
            this.useInsureShipTestServer.Location = new System.Drawing.Point(46, 312);
            this.useInsureShipTestServer.Name = "useInsureShipTestServer";
            this.useInsureShipTestServer.Size = new System.Drawing.Size(157, 17);
            this.useInsureShipTestServer.TabIndex = 36;
            this.useInsureShipTestServer.Text = "Use InsureShip Test Server";
            this.useInsureShipTestServer.UseVisualStyleBackColor = true;
            // 
            // OptionPageInterapptive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.useInsureShipTestServer);
            this.Controls.Add(this.labelInsurance);
            this.Controls.Add(this.express1StampsTestServer);
            this.Controls.Add(this.sectionBuyDotCom);
            this.Controls.Add(this.endiciaTestServers);
            this.Controls.Add(this.onTracTestServer);
            this.Controls.Add(this.panelBuyDotCom);
            this.Controls.Add(this.buyDotComArchiveOrderFile);
            this.Controls.Add(this.purgePrintJobsButton);
            this.Controls.Add(this.purgeLabels);
            this.Controls.Add(this.express1EndiciaTestServer);
            this.Controls.Add(this.deployChosenAssembly);
            this.Controls.Add(this.regenerateFilters);
            this.Controls.Add(this.multipleInstances);
            this.Controls.Add(this.sectionSettings);
            this.Controls.Add(this.searchFitDeleteAfterDownload);
            this.Controls.Add(this.newegg);
            this.Controls.Add(this.payPal);
            this.Controls.Add(this.yahooDeleteMessages);
            this.Controls.Add(this.marketplaceAdvisorMarkProcessed);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.marketplaceAdvisor);
            this.Controls.Add(this.labelPlatformsTestServers);
            this.Controls.Add(this.ebay);
            this.Controls.Add(this.sectionPlatforms);
            this.Controls.Add(this.endiciaTestServer);
            this.Controls.Add(this.fedexListRates);
            this.Controls.Add(this.labelOptions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.deployAssembly);
            this.Controls.Add(this.sectionDatabase);
            this.Controls.Add(this.upsOnLineTools);
            this.Controls.Add(this.fedexTestServer);
            this.Controls.Add(this.stampsTestServer);
            this.Controls.Add(this.postalWebTestServer);
            this.Controls.Add(this.sectionShipping);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OptionPageInterapptive";
            this.Size = new System.Drawing.Size(374, 911);
            this.Load += new System.EventHandler(this.OnLoad);
            this.panelBuyDotCom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.UI.Controls.SectionTitle sectionShipping;
        private System.Windows.Forms.CheckBox postalWebTestServer;
        private System.Windows.Forms.CheckBox stampsTestServer;
        private System.Windows.Forms.CheckBox fedexTestServer;
        private System.Windows.Forms.CheckBox upsOnLineTools;
        private ShipWorks.UI.Controls.SectionTitle sectionDatabase;
        private System.Windows.Forms.Button deployAssembly;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelOptions;
        private System.Windows.Forms.CheckBox fedexListRates;
        private System.Windows.Forms.CheckBox endiciaTestServer;
        private ShipWorks.UI.Controls.SectionTitle sectionPlatforms;
        private System.Windows.Forms.CheckBox ebay;
        private System.Windows.Forms.Label labelPlatformsTestServers;
        private System.Windows.Forms.CheckBox marketplaceAdvisor;
        private System.Windows.Forms.CheckBox marketplaceAdvisorMarkProcessed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox yahooDeleteMessages;
        private System.Windows.Forms.CheckBox payPal;
        private System.Windows.Forms.CheckBox searchFitDeleteAfterDownload;
        private ShipWorks.UI.Controls.SectionTitle sectionSettings;
        private System.Windows.Forms.CheckBox multipleInstances;
        private System.Windows.Forms.Button regenerateFilters;
        private System.Windows.Forms.Button deployChosenAssembly;
        private System.Windows.Forms.CheckBox express1EndiciaTestServer;
        private System.Windows.Forms.Button purgeLabels;
        private System.Windows.Forms.Button purgePrintJobsButton;
        private System.Windows.Forms.CheckBox buyDotComArchiveOrderFile;
        private Data.Import.Spreadsheet.Types.Csv.OrderSchema.GenericCsvOrderMapChooserControl buyDotComMapChooser;
        private UI.Controls.SectionTitle sectionBuyDotCom;
        private System.Windows.Forms.Panel panelBuyDotCom;
        private System.Windows.Forms.CheckBox onTracTestServer;
        private System.Windows.Forms.CheckBox newegg;
        private System.Windows.Forms.ComboBox endiciaTestServers;
        private System.Windows.Forms.CheckBox express1StampsTestServer;
        private System.Windows.Forms.Label labelInsurance;
        private System.Windows.Forms.CheckBox useInsureShipTestServer;
    }
}
