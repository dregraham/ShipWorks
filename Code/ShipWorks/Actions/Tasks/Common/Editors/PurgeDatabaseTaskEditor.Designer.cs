namespace ShipWorks.Actions.Tasks.Common.Editors
{
    partial class PurgeDatabaseTaskEditor
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
            this.components = new System.ComponentModel.Container();
            this.timeoutPurgeCheckbox = new System.Windows.Forms.CheckBox();
            this.timeoutHoursLabel = new System.Windows.Forms.Label();
            this.timeoutInHours = new System.Windows.Forms.NumericUpDown();
            this.retentionPeriodInDays = new System.Windows.Forms.NumericUpDown();
            this.retentionDaysLabel = new System.Windows.Forms.Label();
            this.email = new System.Windows.Forms.CheckBox();
            this.audit = new System.Windows.Forms.CheckBox();
            this.printJobs = new System.Windows.Forms.CheckBox();
            this.labels = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.reclaimDiskSpaceCheckbox = new System.Windows.Forms.CheckBox();
            this.purgeToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.orders = new System.Windows.Forms.CheckBox();
            this.emailHistory = new System.Windows.Forms.CheckBox();
            this.printJobHistory = new System.Windows.Forms.CheckBox();
            this.retentionHeading = new System.Windows.Forms.Label();
            this.timeoutHelp = new ShipWorks.UI.Controls.InfoTip();
            this.emailContentHelp = new ShipWorks.UI.Controls.InfoTip();
            this.shippingLabelsHelp = new ShipWorks.UI.Controls.InfoTip();
            this.printJobHelp = new ShipWorks.UI.Controls.InfoTip();
            this.reclaimDiskSpaceHelp = new ShipWorks.UI.Controls.InfoTip();
            this.infoTip1 = new ShipWorks.UI.Controls.InfoTip();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutInHours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.retentionPeriodInDays)).BeginInit();
            this.SuspendLayout();
            // 
            // timeoutPurgeCheckbox
            // 
            this.timeoutPurgeCheckbox.AutoSize = true;
            this.timeoutPurgeCheckbox.Location = new System.Drawing.Point(21, 248);
            this.timeoutPurgeCheckbox.Name = "timeoutPurgeCheckbox";
            this.timeoutPurgeCheckbox.Size = new System.Drawing.Size(185, 17);
            this.timeoutPurgeCheckbox.TabIndex = 10;
            this.timeoutPurgeCheckbox.Text = "Stop if deletion takes longer than";
            this.timeoutPurgeCheckbox.UseVisualStyleBackColor = true;
            // 
            // timeoutHoursLabel
            // 
            this.timeoutHoursLabel.AutoSize = true;
            this.timeoutHoursLabel.Location = new System.Drawing.Point(258, 249);
            this.timeoutHoursLabel.Name = "timeoutHoursLabel";
            this.timeoutHoursLabel.Size = new System.Drawing.Size(38, 13);
            this.timeoutHoursLabel.TabIndex = 12;
            this.timeoutHoursLabel.Text = "hours.";
            // 
            // timeoutInHours
            // 
            this.timeoutInHours.Location = new System.Drawing.Point(206, 247);
            this.timeoutInHours.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timeoutInHours.Name = "timeoutInHours";
            this.timeoutInHours.Size = new System.Drawing.Size(47, 21);
            this.timeoutInHours.TabIndex = 11;
            this.timeoutInHours.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // retentionPeriodInDays
            // 
            this.retentionPeriodInDays.Location = new System.Drawing.Point(21, 18);
            this.retentionPeriodInDays.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.retentionPeriodInDays.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.retentionPeriodInDays.Name = "retentionPeriodInDays";
            this.retentionPeriodInDays.Size = new System.Drawing.Size(47, 21);
            this.retentionPeriodInDays.TabIndex = 8;
            this.retentionPeriodInDays.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // retentionDaysLabel
            // 
            this.retentionDaysLabel.AutoSize = true;
            this.retentionDaysLabel.Location = new System.Drawing.Point(74, 20);
            this.retentionDaysLabel.Name = "retentionDaysLabel";
            this.retentionDaysLabel.Size = new System.Drawing.Size(30, 13);
            this.retentionDaysLabel.TabIndex = 9;
            this.retentionDaysLabel.Text = "days";
            // 
            // email
            // 
            this.email.AutoSize = true;
            this.email.Location = new System.Drawing.Point(21, 92);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(122, 17);
            this.email.TabIndex = 3;
            this.email.Text = "Email Message Body";
            this.purgeToolTip.SetToolTip(this.email, "The message body is the only thing that will be deleted. You will still be able t" +
        "o see if an email was sent, when, to whom, and the subject line.");
            this.email.UseVisualStyleBackColor = true;
            // 
            // audit
            // 
            this.audit.AutoSize = true;
            this.audit.Location = new System.Drawing.Point(21, 69);
            this.audit.Name = "audit";
            this.audit.Size = new System.Drawing.Size(88, 17);
            this.audit.TabIndex = 1;
            this.audit.Text = "Audit History";
            this.audit.UseVisualStyleBackColor = true;
            // 
            // printJobs
            // 
            this.printJobs.AutoSize = true;
            this.printJobs.Location = new System.Drawing.Point(21, 161);
            this.printJobs.Name = "printJobs";
            this.printJobs.Size = new System.Drawing.Size(110, 17);
            this.printJobs.TabIndex = 5;
            this.printJobs.Text = "Print Job Content";
            this.purgeToolTip.SetToolTip(this.printJobs, "The content of the print job is the only thing that will deleted. You will still " +
        "be able to see your print history and filter against it.");
            this.printJobs.UseVisualStyleBackColor = true;
            // 
            // labels
            // 
            this.labels.AutoSize = true;
            this.labels.Location = new System.Drawing.Point(21, 138);
            this.labels.Name = "labels";
            this.labels.Size = new System.Drawing.Size(132, 17);
            this.labels.TabIndex = 4;
            this.labels.Text = "Shipping Label Images";
            this.purgeToolTip.SetToolTip(this.labels, "This only deletes the actual label image. The shipment settings, tracking number," +
        " and all other information is preserved.");
            this.labels.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data to delete:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Options";
            // 
            // reclaimDiskSpaceCheckbox
            // 
            this.reclaimDiskSpaceCheckbox.AutoSize = true;
            this.reclaimDiskSpaceCheckbox.Checked = true;
            this.reclaimDiskSpaceCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.reclaimDiskSpaceCheckbox.Location = new System.Drawing.Point(21, 272);
            this.reclaimDiskSpaceCheckbox.Name = "reclaimDiskSpaceCheckbox";
            this.reclaimDiskSpaceCheckbox.Size = new System.Drawing.Size(192, 17);
            this.reclaimDiskSpaceCheckbox.TabIndex = 19;
            this.reclaimDiskSpaceCheckbox.Text = "Reclaim disk space (recommended)";
            this.reclaimDiskSpaceCheckbox.UseVisualStyleBackColor = true;
            this.reclaimDiskSpaceCheckbox.CheckedChanged += new System.EventHandler(this.OnReclaimDiskSpaceCheckedChanged);
            // 
            // orders
            // 
            this.orders.AutoSize = true;
            this.orders.Location = new System.Drawing.Point(21, 207);
            this.orders.Name = "orders";
            this.orders.Size = new System.Drawing.Size(59, 17);
            this.orders.TabIndex = 27;
            this.orders.Text = "Orders";
            this.purgeToolTip.SetToolTip(this.orders, "The content of the print job is the only thing that will deleted. You will still " +
        "be able to see your print history and filter against it.");
            this.orders.UseVisualStyleBackColor = true;
            // 
            // emailHistory
            // 
            this.emailHistory.AutoSize = true;
            this.emailHistory.Location = new System.Drawing.Point(39, 115);
            this.emailHistory.Name = "emailHistory";
            this.emailHistory.Size = new System.Drawing.Size(87, 17);
            this.emailHistory.TabIndex = 29;
            this.emailHistory.Text = "Email History";
            this.purgeToolTip.SetToolTip(this.emailHistory, "The message body is the only thing that will be deleted. You will still be able t" +
        "o see if an email was sent, when, to whom, and the subject line.");
            this.emailHistory.UseVisualStyleBackColor = true;
            // 
            // printJobHistory
            // 
            this.printJobHistory.AutoSize = true;
            this.printJobHistory.Location = new System.Drawing.Point(39, 184);
            this.printJobHistory.Name = "printJobHistory";
            this.printJobHistory.Size = new System.Drawing.Size(85, 17);
            this.printJobHistory.TabIndex = 30;
            this.printJobHistory.Text = "Print History";
            this.purgeToolTip.SetToolTip(this.printJobHistory, "The message body is the only thing that will be deleted. You will still be able t" +
        "o see if an email was sent, when, to whom, and the subject line.");
            this.printJobHistory.UseVisualStyleBackColor = true;
            // 
            // retentionHeading
            // 
            this.retentionHeading.AutoSize = true;
            this.retentionHeading.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.retentionHeading.Location = new System.Drawing.Point(3, 0);
            this.retentionHeading.Name = "retentionHeading";
            this.retentionHeading.Size = new System.Drawing.Size(134, 13);
            this.retentionHeading.TabIndex = 20;
            this.retentionHeading.Text = "Delete data older than";
            // 
            // timeoutHelp
            // 
            this.timeoutHelp.Caption = "Deletion will pick up where it left off the next time it runs.";
            this.timeoutHelp.Location = new System.Drawing.Point(294, 250);
            this.timeoutHelp.Name = "timeoutHelp";
            this.timeoutHelp.Size = new System.Drawing.Size(12, 12);
            this.timeoutHelp.TabIndex = 21;
            this.timeoutHelp.Title = "Deletion timeout";
            // 
            // emailContentHelp
            // 
            this.emailContentHelp.Caption = "The message body is the only thing that will be deleted. You will still be able t" +
    "o see if an email was sent, when, to whom, and the subject line.";
            this.emailContentHelp.Location = new System.Drawing.Point(139, 94);
            this.emailContentHelp.Name = "emailContentHelp";
            this.emailContentHelp.Size = new System.Drawing.Size(12, 12);
            this.emailContentHelp.TabIndex = 23;
            this.emailContentHelp.Title = "Email Content";
            // 
            // shippingLabelsHelp
            // 
            this.shippingLabelsHelp.Caption = "This only deletes the actual label image. The shipment settings, tracking number," +
    " and all other information is preserved.";
            this.shippingLabelsHelp.Location = new System.Drawing.Point(149, 140);
            this.shippingLabelsHelp.Name = "shippingLabelsHelp";
            this.shippingLabelsHelp.Size = new System.Drawing.Size(12, 12);
            this.shippingLabelsHelp.TabIndex = 24;
            this.shippingLabelsHelp.Title = "Shipping Label Images";
            // 
            // printJobHelp
            // 
            this.printJobHelp.Caption = "The content of the print job is the only thing that will be deleted. You will sti" +
    "ll be able to see your print history and filter against it.";
            this.printJobHelp.Location = new System.Drawing.Point(128, 163);
            this.printJobHelp.Name = "printJobHelp";
            this.printJobHelp.Size = new System.Drawing.Size(12, 12);
            this.printJobHelp.TabIndex = 25;
            this.printJobHelp.Title = "Print Job Content";
            // 
            // reclaimDiskSpaceHelp
            // 
            this.reclaimDiskSpaceHelp.Caption = "This will free up storage by truncating the SQL log file.";
            this.reclaimDiskSpaceHelp.Location = new System.Drawing.Point(211, 274);
            this.reclaimDiskSpaceHelp.Name = "reclaimDiskSpaceHelp";
            this.reclaimDiskSpaceHelp.Size = new System.Drawing.Size(12, 12);
            this.reclaimDiskSpaceHelp.TabIndex = 26;
            this.reclaimDiskSpaceHelp.Title = "Reclaim Disk Space";
            // 
            // infoTip1
            // 
            this.infoTip1.Caption = "When the last order of a customer is deleted, the customer will also be deleted.";
            this.infoTip1.Location = new System.Drawing.Point(80, 210);
            this.infoTip1.Name = "infoTip1";
            this.infoTip1.Size = new System.Drawing.Size(12, 12);
            this.infoTip1.TabIndex = 28;
            this.infoTip1.Title = "Orders";
            // 
            // PurgeDatabaseTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.printJobHistory);
            this.Controls.Add(this.emailHistory);
            this.Controls.Add(this.infoTip1);
            this.Controls.Add(this.orders);
            this.Controls.Add(this.reclaimDiskSpaceHelp);
            this.Controls.Add(this.printJobHelp);
            this.Controls.Add(this.shippingLabelsHelp);
            this.Controls.Add(this.emailContentHelp);
            this.Controls.Add(this.timeoutHelp);
            this.Controls.Add(this.retentionHeading);
            this.Controls.Add(this.reclaimDiskSpaceCheckbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labels);
            this.Controls.Add(this.printJobs);
            this.Controls.Add(this.audit);
            this.Controls.Add(this.email);
            this.Controls.Add(this.retentionDaysLabel);
            this.Controls.Add(this.retentionPeriodInDays);
            this.Controls.Add(this.timeoutPurgeCheckbox);
            this.Controls.Add(this.timeoutHoursLabel);
            this.Controls.Add(this.timeoutInHours);
            this.Name = "PurgeDatabaseTaskEditor";
            this.Size = new System.Drawing.Size(318, 328);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.timeoutInHours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.retentionPeriodInDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox timeoutPurgeCheckbox;
        private System.Windows.Forms.Label timeoutHoursLabel;
        private System.Windows.Forms.NumericUpDown timeoutInHours;
        private System.Windows.Forms.NumericUpDown retentionPeriodInDays;
        private System.Windows.Forms.Label retentionDaysLabel;
        private System.Windows.Forms.CheckBox email;
        private System.Windows.Forms.CheckBox audit;
        private System.Windows.Forms.CheckBox printJobs;
        private System.Windows.Forms.CheckBox labels;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox reclaimDiskSpaceCheckbox;
        private System.Windows.Forms.ToolTip purgeToolTip;
        private System.Windows.Forms.Label retentionHeading;
        private ShipWorks.UI.Controls.InfoTip timeoutHelp;
        private ShipWorks.UI.Controls.InfoTip emailContentHelp;
        private ShipWorks.UI.Controls.InfoTip shippingLabelsHelp;
        private ShipWorks.UI.Controls.InfoTip printJobHelp;
        private ShipWorks.UI.Controls.InfoTip reclaimDiskSpaceHelp;
        private System.Windows.Forms.CheckBox orders;
        private ShipWorks.UI.Controls.InfoTip infoTip1;
        private System.Windows.Forms.CheckBox emailHistory;
        private System.Windows.Forms.CheckBox printJobHistory;
    }
}
