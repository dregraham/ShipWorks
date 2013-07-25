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
            this.checkboxStopLongCleanups = new System.Windows.Forms.CheckBox();
            this.lableBackups = new System.Windows.Forms.Label();
            this.timeoutInHours = new System.Windows.Forms.NumericUpDown();
            this.labelCleanup = new System.Windows.Forms.Label();
            this.retentionPeriodInDays = new System.Windows.Forms.NumericUpDown();
            this.labelCleanupDays = new System.Windows.Forms.Label();
            this.email = new System.Windows.Forms.CheckBox();
            this.audit = new System.Windows.Forms.CheckBox();
            this.printJobs = new System.Windows.Forms.CheckBox();
            this.labels = new System.Windows.Forms.CheckBox();
            this.downloads = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutInHours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.retentionPeriodInDays)).BeginInit();
            this.SuspendLayout();
            // 
            // checkboxStopLongCleanups
            // 
            this.checkboxStopLongCleanups.AutoSize = true;
            this.checkboxStopLongCleanups.Location = new System.Drawing.Point(18, 179);
            this.checkboxStopLongCleanups.Name = "checkboxStopLongCleanups";
            this.checkboxStopLongCleanups.Size = new System.Drawing.Size(185, 17);
            this.checkboxStopLongCleanups.TabIndex = 18;
            this.checkboxStopLongCleanups.Text = "Stop if deletion takes longer than";
            this.checkboxStopLongCleanups.UseVisualStyleBackColor = true;
            // 
            // lableBackups
            // 
            this.lableBackups.AutoSize = true;
            this.lableBackups.Location = new System.Drawing.Point(256, 180);
            this.lableBackups.Name = "lableBackups";
            this.lableBackups.Size = new System.Drawing.Size(38, 13);
            this.lableBackups.TabIndex = 17;
            this.lableBackups.Text = "hours.";
            // 
            // timeoutInHours
            // 
            this.timeoutInHours.Location = new System.Drawing.Point(203, 177);
            this.timeoutInHours.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timeoutInHours.Name = "timeoutInHours";
            this.timeoutInHours.Size = new System.Drawing.Size(47, 21);
            this.timeoutInHours.TabIndex = 16;
            this.timeoutInHours.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelCleanup
            // 
            this.labelCleanup.AutoSize = true;
            this.labelCleanup.Location = new System.Drawing.Point(15, 158);
            this.labelCleanup.Name = "labelCleanup";
            this.labelCleanup.Size = new System.Drawing.Size(129, 13);
            this.labelCleanup.TabIndex = 19;
            this.labelCleanup.Text = "Delete records older than";
            // 
            // retentionPeriodInDays
            // 
            this.retentionPeriodInDays.Location = new System.Drawing.Point(146, 156);
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
            this.retentionPeriodInDays.TabIndex = 20;
            this.retentionPeriodInDays.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // labelCleanupDays
            // 
            this.labelCleanupDays.AutoSize = true;
            this.labelCleanupDays.Location = new System.Drawing.Point(199, 158);
            this.labelCleanupDays.Name = "labelCleanupDays";
            this.labelCleanupDays.Size = new System.Drawing.Size(34, 13);
            this.labelCleanupDays.TabIndex = 21;
            this.labelCleanupDays.Text = "days.";
            // 
            // email
            // 
            this.email.AutoSize = true;
            this.email.Location = new System.Drawing.Point(18, 66);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(50, 17);
            this.email.TabIndex = 22;
            this.email.Text = "Email";
            this.email.UseVisualStyleBackColor = true;
            // 
            // audit
            // 
            this.audit.AutoSize = true;
            this.audit.Location = new System.Drawing.Point(18, 20);
            this.audit.Name = "audit";
            this.audit.Size = new System.Drawing.Size(74, 17);
            this.audit.TabIndex = 23;
            this.audit.Text = "Audit Trail";
            this.audit.UseVisualStyleBackColor = true;
            // 
            // printJobs
            // 
            this.printJobs.AutoSize = true;
            this.printJobs.Location = new System.Drawing.Point(18, 112);
            this.printJobs.Name = "printJobs";
            this.printJobs.Size = new System.Drawing.Size(73, 17);
            this.printJobs.TabIndex = 24;
            this.printJobs.Text = "Print Jobs";
            this.printJobs.UseVisualStyleBackColor = true;
            // 
            // labels
            // 
            this.labels.AutoSize = true;
            this.labels.Location = new System.Drawing.Point(18, 89);
            this.labels.Name = "labels";
            this.labels.Size = new System.Drawing.Size(56, 17);
            this.labels.TabIndex = 25;
            this.labels.Text = "Labels";
            this.labels.UseVisualStyleBackColor = true;
            // 
            // downloads
            // 
            this.downloads.AutoSize = true;
            this.downloads.Location = new System.Drawing.Point(18, 43);
            this.downloads.Name = "downloads";
            this.downloads.Size = new System.Drawing.Size(115, 17);
            this.downloads.TabIndex = 26;
            this.downloads.Text = "Download Records";
            this.downloads.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Items to delete:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Options";
            // 
            // PurgeDatabaseTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.downloads);
            this.Controls.Add(this.labels);
            this.Controls.Add(this.printJobs);
            this.Controls.Add(this.audit);
            this.Controls.Add(this.email);
            this.Controls.Add(this.labelCleanupDays);
            this.Controls.Add(this.retentionPeriodInDays);
            this.Controls.Add(this.labelCleanup);
            this.Controls.Add(this.checkboxStopLongCleanups);
            this.Controls.Add(this.lableBackups);
            this.Controls.Add(this.timeoutInHours);
            this.Name = "PurgeDatabaseTaskEditor";
            this.Size = new System.Drawing.Size(318, 218);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.timeoutInHours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.retentionPeriodInDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkboxStopLongCleanups;
        private System.Windows.Forms.Label lableBackups;
        private System.Windows.Forms.NumericUpDown timeoutInHours;
        private System.Windows.Forms.Label labelCleanup;
        private System.Windows.Forms.NumericUpDown retentionPeriodInDays;
        private System.Windows.Forms.Label labelCleanupDays;
        private System.Windows.Forms.CheckBox email;
        private System.Windows.Forms.CheckBox audit;
        private System.Windows.Forms.CheckBox printJobs;
        private System.Windows.Forms.CheckBox labels;
        private System.Windows.Forms.CheckBox downloads;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
