namespace ShipWorks.ApplicationCore.Options
{
    partial class OptionPageLogging
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
            this.sectionShipWorks = new ShipWorks.UI.Controls.SectionTitle();
            this.sectionTitle1 = new ShipWorks.UI.Controls.SectionTitle();
            this.label2 = new System.Windows.Forms.Label();
            this.logSize = new System.Windows.Forms.Label();
            this.logShipWorks = new System.Windows.Forms.CheckBox();
            this.logApiCalls = new System.Windows.Forms.CheckBox();
            this.linkLabelExtendedLogging = new System.Windows.Forms.LinkLabel();
            this.linkViewCurrent = new System.Windows.Forms.LinkLabel();
            this.linkViewAll = new System.Windows.Forms.LinkLabel();
            this.linkLabelSaveZip = new System.Windows.Forms.LinkLabel();
            this.panelLogContent = new System.Windows.Forms.Panel();
            this.panelOtherOptions = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.logAge = new System.Windows.Forms.ComboBox();
            this.panelContentOptions = new System.Windows.Forms.Panel();
            this.logRateCalls = new System.Windows.Forms.CheckBox();
            this.panelLogFiles = new System.Windows.Forms.Panel();
            this.panelLogContent.SuspendLayout();
            this.panelOtherOptions.SuspendLayout();
            this.panelContentOptions.SuspendLayout();
            this.panelLogFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // sectionShipWorks
            // 
            this.sectionShipWorks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionShipWorks.Location = new System.Drawing.Point(10, 9);
            this.sectionShipWorks.Name = "sectionShipWorks";
            this.sectionShipWorks.Size = new System.Drawing.Size(366, 22);
            this.sectionShipWorks.TabIndex = 0;
            this.sectionShipWorks.Text = "Log Options";
            // 
            // sectionTitle1
            // 
            this.sectionTitle1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitle1.Location = new System.Drawing.Point(10, 10);
            this.sectionTitle1.Name = "sectionTitle1";
            this.sectionTitle1.Size = new System.Drawing.Size(366, 22);
            this.sectionTitle1.TabIndex = 4;
            this.sectionTitle1.Text = "Log Files";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Total current size of all log files:";
            // 
            // logSize
            // 
            this.logSize.AutoSize = true;
            this.logSize.Location = new System.Drawing.Point(188, 44);
            this.logSize.Name = "logSize";
            this.logSize.Size = new System.Drawing.Size(30, 13);
            this.logSize.TabIndex = 8;
            this.logSize.Text = "0 MB";
            // 
            // logShipWorks
            // 
            this.logShipWorks.AutoSize = true;
            this.logShipWorks.Location = new System.Drawing.Point(3, 6);
            this.logShipWorks.Name = "logShipWorks";
            this.logShipWorks.Size = new System.Drawing.Size(150, 17);
            this.logShipWorks.TabIndex = 1;
            this.logShipWorks.Text = "Log ShipWorks operations";
            this.logShipWorks.UseVisualStyleBackColor = true;
            // 
            // logApiCalls
            // 
            this.logApiCalls.AutoSize = true;
            this.logApiCalls.Location = new System.Drawing.Point(3, 27);
            this.logApiCalls.Name = "logApiCalls";
            this.logApiCalls.Size = new System.Drawing.Size(177, 17);
            this.logApiCalls.TabIndex = 2;
            this.logApiCalls.Text = "Log shipping and store API calls";
            this.logApiCalls.UseVisualStyleBackColor = true;
            // 
            // linkLabelExtendedLogging
            // 
            this.linkLabelExtendedLogging.AutoSize = true;
            this.linkLabelExtendedLogging.Location = new System.Drawing.Point(180, 28);
            this.linkLabelExtendedLogging.Name = "linkLabelExtendedLogging";
            this.linkLabelExtendedLogging.Size = new System.Drawing.Size(125, 13);
            this.linkLabelExtendedLogging.TabIndex = 11;
            this.linkLabelExtendedLogging.TabStop = true;
            this.linkLabelExtendedLogging.Text = "Enable extended logging";
            this.linkLabelExtendedLogging.Visible = false;
            this.linkLabelExtendedLogging.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnEnableExtendedLogging);
            // 
            // linkViewCurrent
            // 
            this.linkViewCurrent.AutoSize = true;
            this.linkViewCurrent.Location = new System.Drawing.Point(30, 67);
            this.linkViewCurrent.Name = "linkViewCurrent";
            this.linkViewCurrent.Size = new System.Drawing.Size(69, 13);
            this.linkViewCurrent.TabIndex = 6;
            this.linkViewCurrent.TabStop = true;
            this.linkViewCurrent.Text = "View Current";
            this.linkViewCurrent.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnOpenLogFolder);
            // 
            // linkViewAll
            // 
            this.linkViewAll.AutoSize = true;
            this.linkViewAll.Location = new System.Drawing.Point(100, 67);
            this.linkViewAll.Name = "linkViewAll";
            this.linkViewAll.Size = new System.Drawing.Size(43, 13);
            this.linkViewAll.TabIndex = 12;
            this.linkViewAll.TabStop = true;
            this.linkViewAll.Text = "View All";
            this.linkViewAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnOpenAllLogs);
            // 
            // linkLabelSaveZip
            // 
            this.linkLabelSaveZip.AutoSize = true;
            this.linkLabelSaveZip.Location = new System.Drawing.Point(147, 67);
            this.linkLabelSaveZip.Name = "linkLabelSaveZip";
            this.linkLabelSaveZip.Size = new System.Drawing.Size(80, 13);
            this.linkLabelSaveZip.TabIndex = 13;
            this.linkLabelSaveZip.TabStop = true;
            this.linkLabelSaveZip.Text = "Save to Zip File";
            this.linkLabelSaveZip.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnSaveLogsToZip);
            // 
            // panelLogContent
            // 
            this.panelLogContent.Controls.Add(this.panelOtherOptions);
            this.panelLogContent.Controls.Add(this.panelContentOptions);
            this.panelLogContent.Controls.Add(this.sectionShipWorks);
            this.panelLogContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogContent.Location = new System.Drawing.Point(0, 0);
            this.panelLogContent.Name = "panelLogContent";
            this.panelLogContent.Size = new System.Drawing.Size(390, 139);
            this.panelLogContent.TabIndex = 14;
            // 
            // panelOtherOptions
            // 
            this.panelOtherOptions.Controls.Add(this.label3);
            this.panelOtherOptions.Controls.Add(this.logAge);
            this.panelOtherOptions.Location = new System.Drawing.Point(25, 100);
            this.panelOtherOptions.Name = "panelOtherOptions";
            this.panelOtherOptions.Size = new System.Drawing.Size(348, 27);
            this.panelOtherOptions.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(199, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Automatically delete log files older than:";
            // 
            // logAge
            // 
            this.logAge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.logAge.FormattingEnabled = true;
            this.logAge.Items.AddRange(new object[] {
            "1 Day",
            "1 Week",
            "1 Month",
            "Never"});
            this.logAge.Location = new System.Drawing.Point(206, 4);
            this.logAge.Name = "logAge";
            this.logAge.Size = new System.Drawing.Size(133, 21);
            this.logAge.TabIndex = 17;
            // 
            // panelContentOptions
            // 
            this.panelContentOptions.Controls.Add(this.logRateCalls);
            this.panelContentOptions.Controls.Add(this.logShipWorks);
            this.panelContentOptions.Controls.Add(this.logApiCalls);
            this.panelContentOptions.Controls.Add(this.linkLabelExtendedLogging);
            this.panelContentOptions.Location = new System.Drawing.Point(25, 32);
            this.panelContentOptions.Name = "panelContentOptions";
            this.panelContentOptions.Size = new System.Drawing.Size(350, 71);
            this.panelContentOptions.TabIndex = 12;
            // 
            // logRateCalls
            // 
            this.logRateCalls.AutoSize = true;
            this.logRateCalls.Location = new System.Drawing.Point(34, 48);
            this.logRateCalls.Name = "logRateCalls";
            this.logRateCalls.Size = new System.Drawing.Size(109, 17);
            this.logRateCalls.TabIndex = 12;
            this.logRateCalls.Text = "Log rate API calls";
            this.logRateCalls.UseVisualStyleBackColor = true;
            // 
            // panelLogFiles
            // 
            this.panelLogFiles.Controls.Add(this.sectionTitle1);
            this.panelLogFiles.Controls.Add(this.linkLabelSaveZip);
            this.panelLogFiles.Controls.Add(this.linkViewAll);
            this.panelLogFiles.Controls.Add(this.logSize);
            this.panelLogFiles.Controls.Add(this.linkViewCurrent);
            this.panelLogFiles.Controls.Add(this.label2);
            this.panelLogFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogFiles.Location = new System.Drawing.Point(0, 139);
            this.panelLogFiles.Name = "panelLogFiles";
            this.panelLogFiles.Size = new System.Drawing.Size(390, 99);
            this.panelLogFiles.TabIndex = 15;
            // 
            // OptionPageLogging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelLogFiles);
            this.Controls.Add(this.panelLogContent);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OptionPageLogging";
            this.Size = new System.Drawing.Size(390, 243);
            this.Load += new System.EventHandler(this.OnLoad);
            this.panelLogContent.ResumeLayout(false);
            this.panelOtherOptions.ResumeLayout(false);
            this.panelOtherOptions.PerformLayout();
            this.panelContentOptions.ResumeLayout(false);
            this.panelContentOptions.PerformLayout();
            this.panelLogFiles.ResumeLayout(false);
            this.panelLogFiles.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.SectionTitle sectionShipWorks;
        private ShipWorks.UI.Controls.SectionTitle sectionTitle1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label logSize;
        private System.Windows.Forms.CheckBox logShipWorks;
        private System.Windows.Forms.CheckBox logApiCalls;
        private System.Windows.Forms.LinkLabel linkLabelExtendedLogging;
        private System.Windows.Forms.LinkLabel linkViewCurrent;
        private System.Windows.Forms.LinkLabel linkViewAll;
        private System.Windows.Forms.LinkLabel linkLabelSaveZip;
        private System.Windows.Forms.Panel panelLogContent;
        private System.Windows.Forms.Panel panelLogFiles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelContentOptions;
        private System.Windows.Forms.ComboBox logAge;
        private System.Windows.Forms.Panel panelOtherOptions;
        private System.Windows.Forms.CheckBox logRateCalls;
    }
}
