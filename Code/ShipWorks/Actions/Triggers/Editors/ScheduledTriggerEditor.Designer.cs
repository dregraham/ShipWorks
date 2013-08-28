namespace ShipWorks.Actions.Triggers.Editors
{
    partial class ScheduledTriggerEditor
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
            this.actionScheduleTypeLink = new System.Windows.Forms.Label();
            this.labelTriggerHeader = new System.Windows.Forms.Label();
            this.recurringSettingsGroup = new System.Windows.Forms.GroupBox();
            this.startTime = new System.Windows.Forms.DateTimePicker();
            this.startDateTimeLabel = new System.Windows.Forms.Label();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.endsLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.endsOnTime = new System.Windows.Forms.DateTimePicker();
            this.endsOnDate = new System.Windows.Forms.DateTimePicker();
            this.endsOnRadioButton = new System.Windows.Forms.RadioButton();
            this.endsNeverRadioButton = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // actionScheduleTypeLink
            // 
            this.actionScheduleTypeLink.AutoSize = true;
            this.actionScheduleTypeLink.BackColor = System.Drawing.Color.Transparent;
            this.actionScheduleTypeLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.actionScheduleTypeLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actionScheduleTypeLink.ForeColor = System.Drawing.Color.Blue;
            this.actionScheduleTypeLink.Location = new System.Drawing.Point(83, 4);
            this.actionScheduleTypeLink.Name = "actionScheduleTypeLink";
            this.actionScheduleTypeLink.Size = new System.Drawing.Size(50, 13);
            this.actionScheduleTypeLink.TabIndex = 3;
            this.actionScheduleTypeLink.Text = "One time";
            this.actionScheduleTypeLink.Click += new System.EventHandler(this.OnClickInputSourceLink);
            // 
            // labelTriggerHeader
            // 
            this.labelTriggerHeader.AutoSize = true;
            this.labelTriggerHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTriggerHeader.Location = new System.Drawing.Point(3, 4);
            this.labelTriggerHeader.Name = "labelTriggerHeader";
            this.labelTriggerHeader.Size = new System.Drawing.Size(82, 13);
            this.labelTriggerHeader.TabIndex = 13;
            this.labelTriggerHeader.Text = "Run this action:";
            // 
            // recurringSettingsGroup
            // 
            this.recurringSettingsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.recurringSettingsGroup.Location = new System.Drawing.Point(0, 94);
            this.recurringSettingsGroup.Name = "recurringSettingsGroup";
            this.recurringSettingsGroup.Padding = new System.Windows.Forms.Padding(0);
            this.recurringSettingsGroup.Size = new System.Drawing.Size(403, 14);
            this.recurringSettingsGroup.TabIndex = 20;
            this.recurringSettingsGroup.TabStop = false;
            this.recurringSettingsGroup.Visible = false;
            // 
            // startTime
            // 
            this.startTime.CustomFormat = "h:mm tt";
            this.startTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startTime.Location = new System.Drawing.Point(145, 23);
            this.startTime.Name = "startTime";
            this.startTime.ShowUpDown = true;
            this.startTime.Size = new System.Drawing.Size(101, 21);
            this.startTime.TabIndex = 38;
            this.startTime.ValueChanged += new System.EventHandler(this.OnStartDateTimeChanged);
            // 
            // startDateTimeLabel
            // 
            this.startDateTimeLabel.AutoSize = true;
            this.startDateTimeLabel.Location = new System.Drawing.Point(3, 29);
            this.startDateTimeLabel.Name = "startDateTimeLabel";
            this.startDateTimeLabel.Size = new System.Drawing.Size(35, 13);
            this.startDateTimeLabel.TabIndex = 37;
            this.startDateTimeLabel.Text = "Start:";
            // 
            // startDate
            // 
            this.startDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startDate.Location = new System.Drawing.Point(41, 23);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(98, 21);
            this.startDate.TabIndex = 36;
            this.startDate.ValueChanged += new System.EventHandler(this.OnStartDateTimeChanged);
            // 
            // endsLabel
            // 
            this.endsLabel.AutoSize = true;
            this.endsLabel.Location = new System.Drawing.Point(9, 51);
            this.endsLabel.Name = "endsLabel";
            this.endsLabel.Size = new System.Drawing.Size(29, 13);
            this.endsLabel.TabIndex = 39;
            this.endsLabel.Text = "End:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.endsOnTime);
            this.panel1.Controls.Add(this.endsOnDate);
            this.panel1.Controls.Add(this.endsOnRadioButton);
            this.panel1.Controls.Add(this.endsNeverRadioButton);
            this.panel1.Location = new System.Drawing.Point(38, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(264, 49);
            this.panel1.TabIndex = 41;
            // 
            // endsOnTime
            // 
            this.endsOnTime.CustomFormat = "h:mm tt";
            this.endsOnTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endsOnTime.Location = new System.Drawing.Point(152, 24);
            this.endsOnTime.Name = "endsOnTime";
            this.endsOnTime.ShowUpDown = true;
            this.endsOnTime.Size = new System.Drawing.Size(101, 21);
            this.endsOnTime.TabIndex = 44;
            this.endsOnTime.ValueChanged += new System.EventHandler(this.OnEndDateTimeChanged);
            // 
            // endsOnDate
            // 
            this.endsOnDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.endsOnDate.Location = new System.Drawing.Point(48, 24);
            this.endsOnDate.Name = "endsOnDate";
            this.endsOnDate.Size = new System.Drawing.Size(98, 21);
            this.endsOnDate.TabIndex = 43;
            this.endsOnDate.ValueChanged += new System.EventHandler(this.OnEndDateTimeChanged);
            // 
            // endsOnRadioButton
            // 
            this.endsOnRadioButton.AutoSize = true;
            this.endsOnRadioButton.Location = new System.Drawing.Point(3, 28);
            this.endsOnRadioButton.Name = "endsOnRadioButton";
            this.endsOnRadioButton.Size = new System.Drawing.Size(39, 17);
            this.endsOnRadioButton.TabIndex = 42;
            this.endsOnRadioButton.TabStop = true;
            this.endsOnRadioButton.Text = "On";
            this.endsOnRadioButton.UseVisualStyleBackColor = true;
            this.endsOnRadioButton.CheckedChanged += new System.EventHandler(this.OnEndsRadioButtonsCheckedChanged);
            // 
            // endsNeverRadioButton
            // 
            this.endsNeverRadioButton.AutoSize = true;
            this.endsNeverRadioButton.Location = new System.Drawing.Point(3, 3);
            this.endsNeverRadioButton.Name = "endsNeverRadioButton";
            this.endsNeverRadioButton.Size = new System.Drawing.Size(54, 17);
            this.endsNeverRadioButton.TabIndex = 41;
            this.endsNeverRadioButton.TabStop = true;
            this.endsNeverRadioButton.Text = "Never";
            this.endsNeverRadioButton.UseVisualStyleBackColor = true;
            this.endsNeverRadioButton.CheckedChanged += new System.EventHandler(this.OnEndsRadioButtonsCheckedChanged);
            // 
            // ScheduledTriggerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.endsLabel);
            this.Controls.Add(this.startTime);
            this.Controls.Add(this.startDateTimeLabel);
            this.Controls.Add(this.startDate);
            this.Controls.Add(this.recurringSettingsGroup);
            this.Controls.Add(this.labelTriggerHeader);
            this.Controls.Add(this.actionScheduleTypeLink);
            this.Name = "ScheduledTriggerEditor";
            this.Size = new System.Drawing.Size(403, 115);
            this.Load += new System.EventHandler(this.OnLoad);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label actionScheduleTypeLink;
        private System.Windows.Forms.Label labelTriggerHeader;
        private System.Windows.Forms.GroupBox recurringSettingsGroup;
        private System.Windows.Forms.DateTimePicker startTime;
        private System.Windows.Forms.Label startDateTimeLabel;
        private System.Windows.Forms.DateTimePicker startDate;
        private System.Windows.Forms.Label endsLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker endsOnTime;
        private System.Windows.Forms.DateTimePicker endsOnDate;
        private System.Windows.Forms.RadioButton endsOnRadioButton;
        private System.Windows.Forms.RadioButton endsNeverRadioButton;
    }
}
