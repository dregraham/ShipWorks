using ShipWorks.Actions.Scheduling.ActionSchedules.Editors.UI;

namespace ShipWorks.Actions.Scheduling.ActionSchedules.Editors
{
    partial class MonthlyActionScheduleEditor
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
            this.dateSelected = new System.Windows.Forms.RadioButton();
            this.dateOfLabel = new System.Windows.Forms.Label();
            this.daySelected = new System.Windows.Forms.RadioButton();
            this.dayWeek = new System.Windows.Forms.ComboBox();
            this.dayDayOfWeek = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dateDayOfMonth = new ShipWorks.Actions.Scheduling.ActionSchedules.Editors.UI.DateOfMonthComboBox();
            this.dayMonth = new ShipWorks.Actions.Scheduling.ActionSchedules.Editors.UI.MonthComboBox();
            this.dateMonth = new ShipWorks.Actions.Scheduling.ActionSchedules.Editors.UI.MonthComboBox();
            this.SuspendLayout();
            // 
            // dateSelected
            // 
            this.dateSelected.AutoSize = true;
            this.dateSelected.Checked = true;
            this.dateSelected.Location = new System.Drawing.Point(3, 7);
            this.dateSelected.Name = "dateSelected";
            this.dateSelected.Size = new System.Drawing.Size(55, 17);
            this.dateSelected.TabIndex = 2;
            this.dateSelected.TabStop = true;
            this.dateSelected.Text = "Day(s)";
            this.dateSelected.UseVisualStyleBackColor = true;
            this.dateSelected.CheckedChanged += new System.EventHandler(this.OnDateSelectedCheckedChanged);
            // 
            // dateOfLabel
            // 
            this.dateOfLabel.AutoSize = true;
            this.dateOfLabel.Location = new System.Drawing.Point(193, 9);
            this.dateOfLabel.Name = "dateOfLabel";
            this.dateOfLabel.Size = new System.Drawing.Size(16, 13);
            this.dateOfLabel.TabIndex = 4;
            this.dateOfLabel.Text = "of";
            // 
            // daySelected
            // 
            this.daySelected.AutoSize = true;
            this.daySelected.Location = new System.Drawing.Point(3, 42);
            this.daySelected.Name = "daySelected";
            this.daySelected.Size = new System.Drawing.Size(57, 17);
            this.daySelected.TabIndex = 7;
            this.daySelected.Text = "On the";
            this.daySelected.UseVisualStyleBackColor = true;
            this.daySelected.CheckedChanged += new System.EventHandler(this.OnDaySelectedCheckedChanged);
            // 
            // dayWeek
            // 
            this.dayWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dayWeek.FormattingEnabled = true;
            this.dayWeek.Location = new System.Drawing.Point(66, 41);
            this.dayWeek.Name = "dayWeek";
            this.dayWeek.Size = new System.Drawing.Size(94, 21);
            this.dayWeek.TabIndex = 9;
            this.dayWeek.SelectedIndexChanged += new System.EventHandler(this.OnDayWeekSelectedIndexChanged);
            // 
            // dayDayOfWeek
            // 
            this.dayDayOfWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dayDayOfWeek.FormattingEnabled = true;
            this.dayDayOfWeek.Location = new System.Drawing.Point(166, 41);
            this.dayDayOfWeek.Name = "dayDayOfWeek";
            this.dayDayOfWeek.Size = new System.Drawing.Size(83, 21);
            this.dayDayOfWeek.TabIndex = 10;
            this.dayDayOfWeek.SelectedIndexChanged += new System.EventHandler(this.OnDayDayOfWeekSelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(255, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "of";
            // 
            // dateDayOfMonth
            // 
            this.dateDayOfMonth.DateChanged = null;
            this.dateDayOfMonth.DropDownHeight = 116;
            this.dateDayOfMonth.DropDownMinimumHeight = 116;
            this.dateDayOfMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dateDayOfMonth.DropDownWidth = 294;
            this.dateDayOfMonth.FormattingEnabled = true;
            this.dateDayOfMonth.IntegralHeight = false;
            this.dateDayOfMonth.Location = new System.Drawing.Point(66, 6);
            this.dateDayOfMonth.Name = "dateDayOfMonth";
            this.dateDayOfMonth.Size = new System.Drawing.Size(121, 21);
            this.dateDayOfMonth.TabIndex = 15;
            // 
            // dayMonth
            // 
            this.dayMonth.DropDownHeight = 294;
            this.dayMonth.DropDownMinimumHeight = 294;
            this.dayMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dayMonth.FormattingEnabled = true;
            this.dayMonth.IntegralHeight = false;
            this.dayMonth.Location = new System.Drawing.Point(277, 41);
            this.dayMonth.MonthChanged = null;
            this.dayMonth.Name = "dayMonth";
            this.dayMonth.Size = new System.Drawing.Size(162, 21);
            this.dayMonth.TabIndex = 14;
            // 
            // dateMonth
            // 
            this.dateMonth.DropDownHeight = 294;
            this.dateMonth.DropDownMinimumHeight = 294;
            this.dateMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dateMonth.FormattingEnabled = true;
            this.dateMonth.IntegralHeight = false;
            this.dateMonth.Location = new System.Drawing.Point(215, 6);
            this.dateMonth.MonthChanged = null;
            this.dateMonth.Name = "dateMonth";
            this.dateMonth.Size = new System.Drawing.Size(162, 21);
            this.dateMonth.TabIndex = 13;
            // 
            // MonthlyActionScheduleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dateDayOfMonth);
            this.Controls.Add(this.dayMonth);
            this.Controls.Add(this.dateMonth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dayDayOfWeek);
            this.Controls.Add(this.dayWeek);
            this.Controls.Add(this.daySelected);
            this.Controls.Add(this.dateOfLabel);
            this.Controls.Add(this.dateSelected);
            this.Name = "MonthlyActionScheduleEditor";
            this.Size = new System.Drawing.Size(455, 69);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton dateSelected;
        private System.Windows.Forms.Label dateOfLabel;
        private System.Windows.Forms.RadioButton daySelected;
        private System.Windows.Forms.ComboBox dayWeek;
        private System.Windows.Forms.ComboBox dayDayOfWeek;
        private System.Windows.Forms.Label label1;
        private MonthComboBox dateMonth;
        private MonthComboBox dayMonth;
        private DateOfMonthComboBox dateDayOfMonth;



    }
}
