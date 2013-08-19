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
            this.labelCleanup = new System.Windows.Forms.Label();
            this.retentionPeriodInDays = new System.Windows.Forms.NumericUpDown();
            this.retentionDaysLabel = new System.Windows.Forms.Label();
            this.email = new System.Windows.Forms.CheckBox();
            this.audit = new System.Windows.Forms.CheckBox();
            this.printJobs = new System.Windows.Forms.CheckBox();
            this.labels = new System.Windows.Forms.CheckBox();
            this.downloads = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.reclaimDiskSpaceCheckbox = new System.Windows.Forms.CheckBox();
            this.purgeToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.retentionHeading = new System.Windows.Forms.Label();
            this.timeoutHelp = new ShipWorks.UI.Controls.InfoTip();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutInHours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.retentionPeriodInDays)).BeginInit();
            this.SuspendLayout();
            // 
            // timeoutPurgeCheckbox
            // 
            this.timeoutPurgeCheckbox.AutoSize = true;
            this.timeoutPurgeCheckbox.Location = new System.Drawing.Point(21, 199);
            this.timeoutPurgeCheckbox.Name = "timeoutPurgeCheckbox";
            this.timeoutPurgeCheckbox.Size = new System.Drawing.Size(185, 17);
            this.timeoutPurgeCheckbox.TabIndex = 10;
            this.timeoutPurgeCheckbox.Text = "Stop if deletion takes longer than";
            this.timeoutPurgeCheckbox.UseVisualStyleBackColor = true;
            // 
            // timeoutHoursLabel
            // 
            this.timeoutHoursLabel.AutoSize = true;
            this.timeoutHoursLabel.Location = new System.Drawing.Point(258, 200);
            this.timeoutHoursLabel.Name = "timeoutHoursLabel";
            this.timeoutHoursLabel.Size = new System.Drawing.Size(38, 13);
            this.timeoutHoursLabel.TabIndex = 12;
            this.timeoutHoursLabel.Text = "hours.";
            // 
            // timeoutInHours
            // 
            this.timeoutInHours.Location = new System.Drawing.Point(206, 198);
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
            // labelCleanup
            // 
            this.labelCleanup.AutoSize = true;
            this.labelCleanup.Location = new System.Drawing.Point(18, 20);
            this.labelCleanup.Name = "labelCleanup";
            this.labelCleanup.Size = new System.Drawing.Size(129, 13);
            this.labelCleanup.TabIndex = 7;
            this.labelCleanup.Text = "Delete records older than";
            // 
            // retentionPeriodInDays
            // 
            this.retentionPeriodInDays.Location = new System.Drawing.Point(149, 20);
            this.retentionPeriodInDays.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.retentionPeriodInDays.Minimum = new decimal(new int[] {
            7,
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
            this.retentionDaysLabel.Location = new System.Drawing.Point(202, 20);
            this.retentionDaysLabel.Name = "retentionDaysLabel";
            this.retentionDaysLabel.Size = new System.Drawing.Size(34, 13);
            this.retentionDaysLabel.TabIndex = 9;
            this.retentionDaysLabel.Text = "days.";
            // 
            // email
            // 
            this.email.AutoSize = true;
            this.email.Location = new System.Drawing.Point(21, 109);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(92, 17);
            this.email.TabIndex = 3;
            this.email.Text = "Email Content";
            this.purgeToolTip.SetToolTip(this.email, "The message body is the only thing that will be deleted. You will still be able t" +
        "o see if an email was sent, when, to whom, and the subject line.");
            this.email.UseVisualStyleBackColor = true;
            // 
            // audit
            // 
            this.audit.AutoSize = true;
            this.audit.Location = new System.Drawing.Point(21, 63);
            this.audit.Name = "audit";
            this.audit.Size = new System.Drawing.Size(88, 17);
            this.audit.TabIndex = 1;
            this.audit.Text = "Audit History";
            this.audit.UseVisualStyleBackColor = true;
            // 
            // printJobs
            // 
            this.printJobs.AutoSize = true;
            this.printJobs.Location = new System.Drawing.Point(21, 155);
            this.printJobs.Name = "printJobs";
            this.printJobs.Size = new System.Drawing.Size(110, 17);
            this.printJobs.TabIndex = 5;
            this.printJobs.Text = "Print Job Content";
            this.purgeToolTip.SetToolTip(this.printJobs, "The content of the print job is the only thing that will deleted. You will still " +
        "be able to see your print history and filter against it.\r\n");
            this.printJobs.UseVisualStyleBackColor = true;
            // 
            // labels
            // 
            this.labels.AutoSize = true;
            this.labels.Location = new System.Drawing.Point(21, 132);
            this.labels.Name = "labels";
            this.labels.Size = new System.Drawing.Size(132, 17);
            this.labels.TabIndex = 4;
            this.labels.Text = "Shipping Label Images";
            this.purgeToolTip.SetToolTip(this.labels, "This only deletes the actual label image. The shipment settings, tracking number," +
        " and all other information is preserved.");
            this.labels.UseVisualStyleBackColor = true;
            // 
            // downloads
            // 
            this.downloads.AutoSize = true;
            this.downloads.Location = new System.Drawing.Point(21, 86);
            this.downloads.Name = "downloads";
            this.downloads.Size = new System.Drawing.Size(110, 17);
            this.downloads.TabIndex = 2;
            this.downloads.Text = "Download History";
            this.purgeToolTip.SetToolTip(this.downloads, "This does not delete any of your orders, just the log of when they were downloade" +
        "d.");
            this.downloads.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Items to delete:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 179);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Options";
            // 
            // reclaimDiskSpaceCheckbox
            // 
            this.reclaimDiskSpaceCheckbox.AutoSize = true;
            this.reclaimDiskSpaceCheckbox.Location = new System.Drawing.Point(21, 223);
            this.reclaimDiskSpaceCheckbox.Name = "reclaimDiskSpaceCheckbox";
            this.reclaimDiskSpaceCheckbox.Size = new System.Drawing.Size(114, 17);
            this.reclaimDiskSpaceCheckbox.TabIndex = 19;
            this.reclaimDiskSpaceCheckbox.Text = "Reclaim disk space";
            this.reclaimDiskSpaceCheckbox.UseVisualStyleBackColor = true;
            this.reclaimDiskSpaceCheckbox.CheckedChanged += new System.EventHandler(this.OnReclaimDiskSpaceCheckedChanged);
            // 
            // retentionHeading
            // 
            this.retentionHeading.AutoSize = true;
            this.retentionHeading.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.retentionHeading.Location = new System.Drawing.Point(3, 0);
            this.retentionHeading.Name = "retentionHeading";
            this.retentionHeading.Size = new System.Drawing.Size(126, 13);
            this.retentionHeading.TabIndex = 20;
            this.retentionHeading.Text = "How far back to start";
            // 
            // timeoutHelp
            // 
            this.timeoutHelp.Caption = "Deletion will pick up where it left off the next time it runs.";
            this.timeoutHelp.Location = new System.Drawing.Point(294, 201);
            this.timeoutHelp.Name = "timeoutHelp";
            this.timeoutHelp.Size = new System.Drawing.Size(12, 12);
            this.timeoutHelp.TabIndex = 21;
            this.timeoutHelp.Title = "";
            // 
            // PurgeDatabaseTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.timeoutHelp);
            this.Controls.Add(this.retentionHeading);
            this.Controls.Add(this.reclaimDiskSpaceCheckbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.downloads);
            this.Controls.Add(this.labels);
            this.Controls.Add(this.printJobs);
            this.Controls.Add(this.audit);
            this.Controls.Add(this.email);
            this.Controls.Add(this.retentionDaysLabel);
            this.Controls.Add(this.retentionPeriodInDays);
            this.Controls.Add(this.labelCleanup);
            this.Controls.Add(this.timeoutPurgeCheckbox);
            this.Controls.Add(this.timeoutHoursLabel);
            this.Controls.Add(this.timeoutInHours);
            this.Name = "PurgeDatabaseTaskEditor";
            this.Size = new System.Drawing.Size(318, 249);
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
        private System.Windows.Forms.Label labelCleanup;
        private System.Windows.Forms.NumericUpDown retentionPeriodInDays;
        private System.Windows.Forms.Label retentionDaysLabel;
        private System.Windows.Forms.CheckBox email;
        private System.Windows.Forms.CheckBox audit;
        private System.Windows.Forms.CheckBox printJobs;
        private System.Windows.Forms.CheckBox labels;
        private System.Windows.Forms.CheckBox downloads;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox reclaimDiskSpaceCheckbox;
        private System.Windows.Forms.ToolTip purgeToolTip;
        private System.Windows.Forms.Label retentionHeading;
        private ShipWorks.UI.Controls.InfoTip timeoutHelp;
    }
}
