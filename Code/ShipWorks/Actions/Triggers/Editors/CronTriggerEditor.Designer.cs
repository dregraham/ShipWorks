namespace ShipWorks.Actions.Triggers.Editors
{
    partial class CronTriggerEditor
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
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.startDateTimeLabel = new System.Windows.Forms.Label();
            this.startTime = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // startDate
            // 
            this.startDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startDate.Location = new System.Drawing.Point(44, 3);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(98, 21);
            this.startDate.TabIndex = 0;
            this.startDate.ValueChanged += new System.EventHandler(this.OnStartDateTimeChanged);
            // 
            // startDateTimeLabel
            // 
            this.startDateTimeLabel.AutoSize = true;
            this.startDateTimeLabel.Location = new System.Drawing.Point(3, 9);
            this.startDateTimeLabel.Name = "startDateTimeLabel";
            this.startDateTimeLabel.Size = new System.Drawing.Size(35, 13);
            this.startDateTimeLabel.TabIndex = 1;
            this.startDateTimeLabel.Text = "Start:";
            // 
            // startTime
            // 
            this.startTime.CustomFormat = "h:mm tt";
            this.startTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startTime.Location = new System.Drawing.Point(148, 3);
            this.startTime.Name = "startTime";
            this.startTime.ShowUpDown = true;
            this.startTime.Size = new System.Drawing.Size(101, 21);
            this.startTime.TabIndex = 2;
            this.startTime.ValueChanged += new System.EventHandler(this.OnStartDateTimeChanged);
            // 
            // CronTriggerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.startTime);
            this.Controls.Add(this.startDateTimeLabel);
            this.Controls.Add(this.startDate);
            this.Name = "CronTriggerEditor";
            this.Size = new System.Drawing.Size(312, 32);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker startDate;
        private System.Windows.Forms.Label startDateTimeLabel;
        private System.Windows.Forms.DateTimePicker startTime;
    }
}
