namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    partial class UpsPickupScheduleControl
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
            this.labelPickupOptions = new System.Windows.Forms.Label();
            this.pickupOption = new System.Windows.Forms.ComboBox();
            this.labelEarliestPickupTime = new System.Windows.Forms.Label();
            this.labelPreferredPickupTime = new System.Windows.Forms.Label();
            this.labelLatestPickupTime = new System.Windows.Forms.Label();
            this.labelPickupStartDate = new System.Windows.Forms.Label();
            this.labelPickupDays = new System.Windows.Forms.Label();
            this.monday = new System.Windows.Forms.CheckBox();
            this.tuesday = new System.Windows.Forms.CheckBox();
            this.wednesday = new System.Windows.Forms.CheckBox();
            this.thursday = new System.Windows.Forms.CheckBox();
            this.friday = new System.Windows.Forms.CheckBox();
            this.earliestPickup = new System.Windows.Forms.DateTimePicker();
            this.preferredPickup = new System.Windows.Forms.DateTimePicker();
            this.latestPickup = new System.Windows.Forms.DateTimePicker();
            this.pickUpDay = new System.Windows.Forms.Panel();
            this.pickupDateTimePanel = new System.Windows.Forms.Panel();
            this.pickupStartDate = new System.Windows.Forms.ComboBox();
            this.pickupLocation = new System.Windows.Forms.ComboBox();
            this.labelPickupLocation = new System.Windows.Forms.Label();
            this.labelPickupLocationHeader = new System.Windows.Forms.Label();
            this.feeInfoPanel = new System.Windows.Forms.Panel();
            this.feeLink = new System.Windows.Forms.LinkLabel();
            this.feesMayApplyLabel = new System.Windows.Forms.Label();
            this.pickUpDay.SuspendLayout();
            this.pickupDateTimePanel.SuspendLayout();
            this.feeInfoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPickupOptions
            // 
            this.labelPickupOptions.AutoSize = true;
            this.labelPickupOptions.Location = new System.Drawing.Point(36, 23);
            this.labelPickupOptions.Name = "labelPickupOptions";
            this.labelPickupOptions.Size = new System.Drawing.Size(81, 13);
            this.labelPickupOptions.TabIndex = 1;
            this.labelPickupOptions.Text = "Pickup Options:";
            // 
            // pickupOption
            // 
            this.pickupOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pickupOption.FormattingEnabled = true;
            this.pickupOption.Location = new System.Drawing.Point(124, 20);
            this.pickupOption.Name = "pickupOption";
            this.pickupOption.Size = new System.Drawing.Size(121, 21);
            this.pickupOption.TabIndex = 2;
            this.pickupOption.SelectedIndexChanged += new System.EventHandler(this.OnChangedPickupOption);
            // 
            // labelEarliestPickupTime
            // 
            this.labelEarliestPickupTime.AutoSize = true;
            this.labelEarliestPickupTime.Location = new System.Drawing.Point(13, 33);
            this.labelEarliestPickupTime.Name = "labelEarliestPickupTime";
            this.labelEarliestPickupTime.Size = new System.Drawing.Size(104, 13);
            this.labelEarliestPickupTime.TabIndex = 3;
            this.labelEarliestPickupTime.Text = "Earliest Pickup Time:";
            // 
            // labelPreferredPickupTime
            // 
            this.labelPreferredPickupTime.AutoSize = true;
            this.labelPreferredPickupTime.Location = new System.Drawing.Point(2, 60);
            this.labelPreferredPickupTime.Name = "labelPreferredPickupTime";
            this.labelPreferredPickupTime.Size = new System.Drawing.Size(115, 13);
            this.labelPreferredPickupTime.TabIndex = 4;
            this.labelPreferredPickupTime.Text = "Preferred Pickup Time:";
            // 
            // labelLatestPickupTime
            // 
            this.labelLatestPickupTime.AutoSize = true;
            this.labelLatestPickupTime.Location = new System.Drawing.Point(18, 87);
            this.labelLatestPickupTime.Name = "labelLatestPickupTime";
            this.labelLatestPickupTime.Size = new System.Drawing.Size(99, 13);
            this.labelLatestPickupTime.TabIndex = 5;
            this.labelLatestPickupTime.Text = "Latest Pickup Time:";
            // 
            // labelPickupStartDate
            // 
            this.labelPickupStartDate.AutoSize = true;
            this.labelPickupStartDate.Location = new System.Drawing.Point(23, 109);
            this.labelPickupStartDate.Name = "labelPickupStartDate";
            this.labelPickupStartDate.Size = new System.Drawing.Size(94, 13);
            this.labelPickupStartDate.TabIndex = 6;
            this.labelPickupStartDate.Text = "Pickup Start Date:";
            // 
            // labelPickupDays
            // 
            this.labelPickupDays.AutoSize = true;
            this.labelPickupDays.Location = new System.Drawing.Point(49, 4);
            this.labelPickupDays.Name = "labelPickupDays";
            this.labelPickupDays.Size = new System.Drawing.Size(68, 13);
            this.labelPickupDays.TabIndex = 7;
            this.labelPickupDays.Text = "Pickup Days:";
            // 
            // monday
            // 
            this.monday.AutoSize = true;
            this.monday.Location = new System.Drawing.Point(125, 3);
            this.monday.Name = "monday";
            this.monday.Size = new System.Drawing.Size(64, 17);
            this.monday.TabIndex = 8;
            this.monday.Text = "Monday";
            this.monday.UseVisualStyleBackColor = true;
            // 
            // tuesday
            // 
            this.tuesday.AutoSize = true;
            this.tuesday.Location = new System.Drawing.Point(125, 26);
            this.tuesday.Name = "tuesday";
            this.tuesday.Size = new System.Drawing.Size(67, 17);
            this.tuesday.TabIndex = 9;
            this.tuesday.Text = "Tuesday";
            this.tuesday.UseVisualStyleBackColor = true;
            // 
            // wednesday
            // 
            this.wednesday.AutoSize = true;
            this.wednesday.Location = new System.Drawing.Point(125, 49);
            this.wednesday.Name = "wednesday";
            this.wednesday.Size = new System.Drawing.Size(83, 17);
            this.wednesday.TabIndex = 10;
            this.wednesday.Text = "Wednesday";
            this.wednesday.UseVisualStyleBackColor = true;
            // 
            // thursday
            // 
            this.thursday.AutoSize = true;
            this.thursday.Location = new System.Drawing.Point(125, 73);
            this.thursday.Name = "thursday";
            this.thursday.Size = new System.Drawing.Size(71, 17);
            this.thursday.TabIndex = 11;
            this.thursday.Text = "Thursday";
            this.thursday.UseVisualStyleBackColor = true;
            // 
            // friday
            // 
            this.friday.AutoSize = true;
            this.friday.Location = new System.Drawing.Point(125, 96);
            this.friday.Name = "friday";
            this.friday.Size = new System.Drawing.Size(56, 17);
            this.friday.TabIndex = 12;
            this.friday.Text = "Friday";
            this.friday.UseVisualStyleBackColor = true;
            // 
            // earliestPickup
            // 
            this.earliestPickup.CustomFormat = "h:mm tt";
            this.earliestPickup.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.earliestPickup.Location = new System.Drawing.Point(125, 27);
            this.earliestPickup.Name = "earliestPickup";
            this.earliestPickup.ShowUpDown = true;
            this.earliestPickup.Size = new System.Drawing.Size(83, 21);
            this.earliestPickup.TabIndex = 13;
            this.earliestPickup.Value = new System.DateTime(2013, 4, 8, 8, 0, 0, 0);
            this.earliestPickup.ValueChanged += new System.EventHandler(this.OnTimeChanged);
            // 
            // preferredPickup
            // 
            this.preferredPickup.CustomFormat = "h:mm tt";
            this.preferredPickup.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.preferredPickup.Location = new System.Drawing.Point(125, 54);
            this.preferredPickup.Name = "preferredPickup";
            this.preferredPickup.ShowUpDown = true;
            this.preferredPickup.Size = new System.Drawing.Size(83, 21);
            this.preferredPickup.TabIndex = 14;
            this.preferredPickup.Value = new System.DateTime(2013, 4, 8, 12, 0, 0, 0);
            this.preferredPickup.ValueChanged += new System.EventHandler(this.OnTimeChanged);
            // 
            // latestPickup
            // 
            this.latestPickup.CustomFormat = "h:mm tt";
            this.latestPickup.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.latestPickup.Location = new System.Drawing.Point(125, 81);
            this.latestPickup.Name = "latestPickup";
            this.latestPickup.ShowUpDown = true;
            this.latestPickup.Size = new System.Drawing.Size(83, 21);
            this.latestPickup.TabIndex = 15;
            this.latestPickup.Value = new System.DateTime(2013, 4, 8, 17, 0, 0, 0);
            this.latestPickup.ValueChanged += new System.EventHandler(this.OnTimeChanged);
            // 
            // pickUpDay
            // 
            this.pickUpDay.Controls.Add(this.labelPickupDays);
            this.pickUpDay.Controls.Add(this.monday);
            this.pickUpDay.Controls.Add(this.tuesday);
            this.pickUpDay.Controls.Add(this.wednesday);
            this.pickUpDay.Controls.Add(this.thursday);
            this.pickUpDay.Controls.Add(this.friday);
            this.pickUpDay.Location = new System.Drawing.Point(0, 179);
            this.pickUpDay.Name = "pickUpDay";
            this.pickUpDay.Size = new System.Drawing.Size(257, 122);
            this.pickUpDay.TabIndex = 17;
            // 
            // pickupDateTimePanel
            // 
            this.pickupDateTimePanel.Controls.Add(this.pickupStartDate);
            this.pickupDateTimePanel.Controls.Add(this.pickupLocation);
            this.pickupDateTimePanel.Controls.Add(this.labelPickupLocation);
            this.pickupDateTimePanel.Controls.Add(this.labelEarliestPickupTime);
            this.pickupDateTimePanel.Controls.Add(this.labelPreferredPickupTime);
            this.pickupDateTimePanel.Controls.Add(this.labelLatestPickupTime);
            this.pickupDateTimePanel.Controls.Add(this.latestPickup);
            this.pickupDateTimePanel.Controls.Add(this.labelPickupStartDate);
            this.pickupDateTimePanel.Controls.Add(this.preferredPickup);
            this.pickupDateTimePanel.Controls.Add(this.earliestPickup);
            this.pickupDateTimePanel.Location = new System.Drawing.Point(0, 47);
            this.pickupDateTimePanel.Name = "pickupDateTimePanel";
            this.pickupDateTimePanel.Size = new System.Drawing.Size(311, 136);
            this.pickupDateTimePanel.TabIndex = 18;
            // 
            // pickupStartDate
            // 
            this.pickupStartDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pickupStartDate.FormattingEnabled = true;
            this.pickupStartDate.Location = new System.Drawing.Point(124, 106);
            this.pickupStartDate.Name = "pickupStartDate";
            this.pickupStartDate.Size = new System.Drawing.Size(184, 21);
            this.pickupStartDate.TabIndex = 20;
            // 
            // pickupLocation
            // 
            this.pickupLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pickupLocation.FormattingEnabled = true;
            this.pickupLocation.Location = new System.Drawing.Point(124, 0);
            this.pickupLocation.Name = "pickupLocation";
            this.pickupLocation.Size = new System.Drawing.Size(121, 21);
            this.pickupLocation.TabIndex = 19;
            // 
            // labelPickupLocation
            // 
            this.labelPickupLocation.AutoSize = true;
            this.labelPickupLocation.Location = new System.Drawing.Point(33, 3);
            this.labelPickupLocation.Name = "labelPickupLocation";
            this.labelPickupLocation.Size = new System.Drawing.Size(84, 13);
            this.labelPickupLocation.TabIndex = 17;
            this.labelPickupLocation.Text = "Pickup Location:";
            // 
            // labelPickupLocationHeader
            // 
            this.labelPickupLocationHeader.AutoSize = true;
            this.labelPickupLocationHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPickupLocationHeader.Location = new System.Drawing.Point(3, 0);
            this.labelPickupLocationHeader.Name = "labelPickupLocationHeader";
            this.labelPickupLocationHeader.Size = new System.Drawing.Size(166, 13);
            this.labelPickupLocationHeader.TabIndex = 19;
            this.labelPickupLocationHeader.Text = "Schedule Your Pickups";
            // 
            // feeInfoPanel
            // 
            this.feeInfoPanel.Controls.Add(this.feeLink);
            this.feeInfoPanel.Controls.Add(this.feesMayApplyLabel);
            this.feeInfoPanel.Location = new System.Drawing.Point(252, 22);
            this.feeInfoPanel.Name = "feeInfoPanel";
            this.feeInfoPanel.Size = new System.Drawing.Size(200, 19);
            this.feeInfoPanel.TabIndex = 20;
            // 
            // feeLink
            // 
            this.feeLink.AutoSize = true;
            this.feeLink.Location = new System.Drawing.Point(103, 0);
            this.feeLink.Margin = new System.Windows.Forms.Padding(0);
            this.feeLink.Name = "feeLink";
            this.feeLink.Size = new System.Drawing.Size(29, 13);
            this.feeLink.TabIndex = 1;
            this.feeLink.TabStop = true;
            this.feeLink.Text = "here";
            this.feeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnFeeLinkClicked);
            // 
            // feesMayApplyLabel
            // 
            this.feesMayApplyLabel.AutoSize = true;
            this.feesMayApplyLabel.Location = new System.Drawing.Point(0, 0);
            this.feesMayApplyLabel.Name = "feesMayApplyLabel";
            this.feesMayApplyLabel.Size = new System.Drawing.Size(202, 13);
            this.feesMayApplyLabel.TabIndex = 0;
            this.feesMayApplyLabel.Text = "Fees may apply, click here for more info.";
            // 
            // UpsPickupScheduleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.feeInfoPanel);
            this.Controls.Add(this.labelPickupLocationHeader);
            this.Controls.Add(this.pickupDateTimePanel);
            this.Controls.Add(this.pickUpDay);
            this.Controls.Add(this.pickupOption);
            this.Controls.Add(this.labelPickupOptions);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UpsPickupScheduleControl";
            this.Size = new System.Drawing.Size(581, 338);
            this.pickUpDay.ResumeLayout(false);
            this.pickUpDay.PerformLayout();
            this.pickupDateTimePanel.ResumeLayout(false);
            this.pickupDateTimePanel.PerformLayout();
            this.feeInfoPanel.ResumeLayout(false);
            this.feeInfoPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPickupOptions;
        private System.Windows.Forms.ComboBox pickupOption;
        private System.Windows.Forms.Label labelEarliestPickupTime;
        private System.Windows.Forms.Label labelPreferredPickupTime;
        private System.Windows.Forms.Label labelLatestPickupTime;
        private System.Windows.Forms.Label labelPickupStartDate;
        private System.Windows.Forms.Label labelPickupDays;
        private System.Windows.Forms.CheckBox monday;
        private System.Windows.Forms.CheckBox tuesday;
        private System.Windows.Forms.CheckBox wednesday;
        private System.Windows.Forms.CheckBox thursday;
        private System.Windows.Forms.CheckBox friday;
        private System.Windows.Forms.DateTimePicker earliestPickup;
        private System.Windows.Forms.DateTimePicker preferredPickup;
        private System.Windows.Forms.DateTimePicker latestPickup;
        private System.Windows.Forms.Panel pickUpDay;
        private System.Windows.Forms.Panel pickupDateTimePanel;
        private System.Windows.Forms.ComboBox pickupLocation;
        private System.Windows.Forms.Label labelPickupLocation;
        private System.Windows.Forms.ComboBox pickupStartDate;
        private System.Windows.Forms.Label labelPickupLocationHeader;
        private System.Windows.Forms.Panel feeInfoPanel;
        private System.Windows.Forms.LinkLabel feeLink;
        private System.Windows.Forms.Label feesMayApplyLabel;

    }
}
