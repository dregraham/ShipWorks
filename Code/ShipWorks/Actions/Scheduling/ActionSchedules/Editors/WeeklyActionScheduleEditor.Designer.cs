namespace ShipWorks.Actions.Scheduling.ActionSchedules.Editors
{
    partial class WeeklyActionScheduleEditor
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
            this.recurrsEveryNumberOfWeeks = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.sunday = new System.Windows.Forms.CheckBox();
            this.saturday = new System.Windows.Forms.CheckBox();
            this.friday = new System.Windows.Forms.CheckBox();
            this.thursday = new System.Windows.Forms.CheckBox();
            this.wednesday = new System.Windows.Forms.CheckBox();
            this.tuesday = new System.Windows.Forms.CheckBox();
            this.monday = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // recurrsEveryNumberOfWeeks
            // 
            this.recurrsEveryNumberOfWeeks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.recurrsEveryNumberOfWeeks.FormattingEnabled = true;
            this.recurrsEveryNumberOfWeeks.Location = new System.Drawing.Point(69, 2);
            this.recurrsEveryNumberOfWeeks.Name = "recurrsEveryNumberOfWeeks";
            this.recurrsEveryNumberOfWeeks.Size = new System.Drawing.Size(36, 21);
            this.recurrsEveryNumberOfWeeks.TabIndex = 6;
            this.recurrsEveryNumberOfWeeks.SelectedIndexChanged += new System.EventHandler(this.OnRecurrenceWeeksChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "weeks on:";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.AutoSize = true;
            this.frequencyLabel.Location = new System.Drawing.Point(4, 5);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(59, 13);
            this.frequencyLabel.TabIndex = 4;
            this.frequencyLabel.Text = "Run every:";
            // 
            // sunday
            // 
            this.sunday.AutoSize = true;
            this.sunday.Location = new System.Drawing.Point(7, 29);
            this.sunday.Name = "sunday";
            this.sunday.Size = new System.Drawing.Size(62, 17);
            this.sunday.TabIndex = 7;
            this.sunday.Text = "Sunday";
            this.sunday.UseVisualStyleBackColor = true;
            this.sunday.CheckedChanged += new System.EventHandler(this.OnDayCheckedChanged);
            // 
            // saturday
            // 
            this.saturday.AutoSize = true;
            this.saturday.Location = new System.Drawing.Point(151, 52);
            this.saturday.Name = "saturday";
            this.saturday.Size = new System.Drawing.Size(68, 17);
            this.saturday.TabIndex = 8;
            this.saturday.Text = "Saturday";
            this.saturday.UseVisualStyleBackColor = true;
            this.saturday.CheckedChanged += new System.EventHandler(this.OnDayCheckedChanged);
            // 
            // friday
            // 
            this.friday.AutoSize = true;
            this.friday.Location = new System.Drawing.Point(83, 52);
            this.friday.Name = "friday";
            this.friday.Size = new System.Drawing.Size(54, 17);
            this.friday.TabIndex = 9;
            this.friday.Text = "Friday";
            this.friday.UseVisualStyleBackColor = true;
            this.friday.CheckedChanged += new System.EventHandler(this.OnDayCheckedChanged);
            // 
            // thursday
            // 
            this.thursday.AutoSize = true;
            this.thursday.Location = new System.Drawing.Point(7, 52);
            this.thursday.Name = "thursday";
            this.thursday.Size = new System.Drawing.Size(70, 17);
            this.thursday.TabIndex = 10;
            this.thursday.Text = "Thursday";
            this.thursday.UseVisualStyleBackColor = true;
            this.thursday.CheckedChanged += new System.EventHandler(this.OnDayCheckedChanged);
            // 
            // wednesday
            // 
            this.wednesday.AutoSize = true;
            this.wednesday.Location = new System.Drawing.Point(226, 29);
            this.wednesday.Name = "wednesday";
            this.wednesday.Size = new System.Drawing.Size(83, 17);
            this.wednesday.TabIndex = 11;
            this.wednesday.Text = "Wednesday";
            this.wednesday.UseVisualStyleBackColor = true;
            this.wednesday.CheckedChanged += new System.EventHandler(this.OnDayCheckedChanged);
            // 
            // tuesday
            // 
            this.tuesday.AutoSize = true;
            this.tuesday.Location = new System.Drawing.Point(151, 29);
            this.tuesday.Name = "tuesday";
            this.tuesday.Size = new System.Drawing.Size(67, 17);
            this.tuesday.TabIndex = 12;
            this.tuesday.Text = "Tuesday";
            this.tuesday.UseVisualStyleBackColor = true;
            this.tuesday.CheckedChanged += new System.EventHandler(this.OnDayCheckedChanged);
            // 
            // monday
            // 
            this.monday.AutoSize = true;
            this.monday.Location = new System.Drawing.Point(83, 29);
            this.monday.Name = "monday";
            this.monday.Size = new System.Drawing.Size(64, 17);
            this.monday.TabIndex = 13;
            this.monday.Text = "Monday";
            this.monday.UseVisualStyleBackColor = true;
            this.monday.CheckedChanged += new System.EventHandler(this.OnDayCheckedChanged);
            // 
            // WeeklyActionScheduleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.saturday);
            this.Controls.Add(this.friday);
            this.Controls.Add(this.thursday);
            this.Controls.Add(this.wednesday);
            this.Controls.Add(this.tuesday);
            this.Controls.Add(this.monday);
            this.Controls.Add(this.sunday);
            this.Controls.Add(this.recurrsEveryNumberOfWeeks);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.frequencyLabel);
            this.Name = "WeeklyActionScheduleEditor";
            this.Size = new System.Drawing.Size(312, 72);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox recurrsEveryNumberOfWeeks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.CheckBox sunday;
        private System.Windows.Forms.CheckBox saturday;
        private System.Windows.Forms.CheckBox friday;
        private System.Windows.Forms.CheckBox thursday;
        private System.Windows.Forms.CheckBox wednesday;
        private System.Windows.Forms.CheckBox tuesday;
        private System.Windows.Forms.CheckBox monday;
    }
}
