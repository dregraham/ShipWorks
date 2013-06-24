namespace ShipWorks.Actions.Scheduling.ActionSchedules.Editors
{
    partial class DailyActionScheduleEditor
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
            this.recurrenceLabel = new System.Windows.Forms.Label();
            this.unitsLabel = new System.Windows.Forms.Label();
            this.days = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // recurrenceLabel
            // 
            this.recurrenceLabel.AutoSize = true;
            this.recurrenceLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.recurrenceLabel.Location = new System.Drawing.Point(4, 7);
            this.recurrenceLabel.Name = "recurrenceLabel";
            this.recurrenceLabel.Size = new System.Drawing.Size(61, 13);
            this.recurrenceLabel.TabIndex = 0;
            this.recurrenceLabel.Text = "Run every:";
            this.recurrenceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // unitsLabel
            // 
            this.unitsLabel.AutoSize = true;
            this.unitsLabel.Location = new System.Drawing.Point(109, 7);
            this.unitsLabel.Name = "unitsLabel";
            this.unitsLabel.Size = new System.Drawing.Size(30, 13);
            this.unitsLabel.TabIndex = 2;
            this.unitsLabel.Text = "day(s)";
            this.unitsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // days
            // 
            this.days.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.days.FormattingEnabled = true;
            this.days.Location = new System.Drawing.Point(67, 4);
            this.days.Name = "days";
            this.days.Size = new System.Drawing.Size(36, 21);
            this.days.TabIndex = 3;
            this.days.SelectedIndexChanged += new System.EventHandler(this.OnDaysChanged);
            // 
            // DailyActionScheduleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.days);
            this.Controls.Add(this.unitsLabel);
            this.Controls.Add(this.recurrenceLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "DailyActionScheduleEditor";
            this.Size = new System.Drawing.Size(162, 31);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label recurrenceLabel;
        private System.Windows.Forms.Label unitsLabel;
        private System.Windows.Forms.ComboBox days;
    }
}
