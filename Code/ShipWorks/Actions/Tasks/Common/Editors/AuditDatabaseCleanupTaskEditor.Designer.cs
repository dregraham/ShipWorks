namespace ShipWorks.Actions.Tasks.Common.Editors
{
    partial class AuditDatabaseCleanupTaskEditor
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
            this.numericStopAfterMinutes = new System.Windows.Forms.NumericUpDown();
            this.labelCleanup = new System.Windows.Forms.Label();
            this.numericCleanupDays = new System.Windows.Forms.NumericUpDown();
            this.labelCleanupDays = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericStopAfterMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericCleanupDays)).BeginInit();
            this.SuspendLayout();
            // 
            // checkboxStopLongCleanups
            // 
            this.checkboxStopLongCleanups.AutoSize = true;
            this.checkboxStopLongCleanups.Location = new System.Drawing.Point(6, 31);
            this.checkboxStopLongCleanups.Name = "checkboxStopLongCleanups";
            this.checkboxStopLongCleanups.Size = new System.Drawing.Size(184, 17);
            this.checkboxStopLongCleanups.TabIndex = 18;
            this.checkboxStopLongCleanups.Text = "Stop if cleanup takes longer than";
            this.checkboxStopLongCleanups.UseVisualStyleBackColor = true;
            // 
            // lableBackups
            // 
            this.lableBackups.AutoSize = true;
            this.lableBackups.Location = new System.Drawing.Point(240, 32);
            this.lableBackups.Name = "lableBackups";
            this.lableBackups.Size = new System.Drawing.Size(48, 13);
            this.lableBackups.TabIndex = 17;
            this.lableBackups.Text = "minutes.";
            // 
            // numericStopAfterMinutes
            // 
            this.numericStopAfterMinutes.Location = new System.Drawing.Point(190, 30);
            this.numericStopAfterMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericStopAfterMinutes.Name = "numericStopAfterMinutes";
            this.numericStopAfterMinutes.Size = new System.Drawing.Size(47, 21);
            this.numericStopAfterMinutes.TabIndex = 16;
            this.numericStopAfterMinutes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelCleanup
            // 
            this.labelCleanup.AutoSize = true;
            this.labelCleanup.Location = new System.Drawing.Point(3, 4);
            this.labelCleanup.Name = "labelCleanup";
            this.labelCleanup.Size = new System.Drawing.Size(167, 13);
            this.labelCleanup.TabIndex = 19;
            this.labelCleanup.Text = "Clean up audit records older than";
            // 
            // numericCleanupDays
            // 
            this.numericCleanupDays.Location = new System.Drawing.Point(174, 2);
            this.numericCleanupDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericCleanupDays.Name = "numericCleanupDays";
            this.numericCleanupDays.Size = new System.Drawing.Size(47, 21);
            this.numericCleanupDays.TabIndex = 20;
            this.numericCleanupDays.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelCleanupDays
            // 
            this.labelCleanupDays.AutoSize = true;
            this.labelCleanupDays.Location = new System.Drawing.Point(225, 4);
            this.labelCleanupDays.Name = "labelCleanupDays";
            this.labelCleanupDays.Size = new System.Drawing.Size(34, 13);
            this.labelCleanupDays.TabIndex = 21;
            this.labelCleanupDays.Text = "days.";
            // 
            // AuditDatabaseCleanupTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelCleanupDays);
            this.Controls.Add(this.numericCleanupDays);
            this.Controls.Add(this.labelCleanup);
            this.Controls.Add(this.checkboxStopLongCleanups);
            this.Controls.Add(this.lableBackups);
            this.Controls.Add(this.numericStopAfterMinutes);
            this.Name = "AuditDatabaseCleanupTaskEditor";
            this.Size = new System.Drawing.Size(302, 57);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.numericStopAfterMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericCleanupDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkboxStopLongCleanups;
        private System.Windows.Forms.Label lableBackups;
        private System.Windows.Forms.NumericUpDown numericStopAfterMinutes;
        private System.Windows.Forms.Label labelCleanup;
        private System.Windows.Forms.NumericUpDown numericCleanupDays;
        private System.Windows.Forms.Label labelCleanupDays;
    }
}
