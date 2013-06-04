namespace ShipWorks.Data.Import.Spreadsheet.Editing
{
    partial class GenericSpreadsheetDateSettingsDlg
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.labelTimezone2 = new System.Windows.Forms.Label();
            this.comboTimezone = new System.Windows.Forms.ComboBox();
            this.dateTimeFormat = new System.Windows.Forms.ComboBox();
            this.labelCombined = new System.Windows.Forms.Label();
            this.labelTimeFormat = new System.Windows.Forms.Label();
            this.timeFormat = new System.Windows.Forms.ComboBox();
            this.labelCombinedTitle = new System.Windows.Forms.Label();
            this.labelSeparate = new System.Windows.Forms.Label();
            this.labelDateFormat = new System.Windows.Forms.Label();
            this.dateFormat = new System.Windows.Forms.ComboBox();
            this.labelTimeZoneTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(264, 247);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 6;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(183, 247);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 5;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // labelTimezone2
            // 
            this.labelTimezone2.Location = new System.Drawing.Point(37, 158);
            this.labelTimezone2.Name = "labelTimezone2";
            this.labelTimezone2.Size = new System.Drawing.Size(280, 32);
            this.labelTimezone2.TabIndex = 32;
            this.labelTimezone2.Text = "If the timezone is not apart of the date or time, it will be assumed to be:";
            // 
            // comboTimezone
            // 
            this.comboTimezone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTimezone.FormattingEnabled = true;
            this.comboTimezone.Location = new System.Drawing.Point(57, 193);
            this.comboTimezone.Name = "comboTimezone";
            this.comboTimezone.Size = new System.Drawing.Size(131, 21);
            this.comboTimezone.TabIndex = 31;
            // 
            // dateTimeFormat
            // 
            this.dateTimeFormat.FormattingEnabled = true;
            this.dateTimeFormat.Location = new System.Drawing.Point(156, 34);
            this.dateTimeFormat.Name = "dateTimeFormat";
            this.dateTimeFormat.Size = new System.Drawing.Size(146, 21);
            this.dateTimeFormat.TabIndex = 29;
            this.dateTimeFormat.Text = "Automatic";
            // 
            // labelCombined
            // 
            this.labelCombined.AutoSize = true;
            this.labelCombined.Location = new System.Drawing.Point(37, 37);
            this.labelCombined.Name = "labelCombined";
            this.labelCombined.Size = new System.Drawing.Size(113, 13);
            this.labelCombined.TabIndex = 33;
            this.labelCombined.Text = "Date and time format:";
            // 
            // labelTimeFormat
            // 
            this.labelTimeFormat.AutoSize = true;
            this.labelTimeFormat.Location = new System.Drawing.Point(37, 114);
            this.labelTimeFormat.Name = "labelTimeFormat";
            this.labelTimeFormat.Size = new System.Drawing.Size(68, 13);
            this.labelTimeFormat.TabIndex = 35;
            this.labelTimeFormat.Text = "Time format:";
            // 
            // timeFormat
            // 
            this.timeFormat.FormattingEnabled = true;
            this.timeFormat.Location = new System.Drawing.Point(112, 111);
            this.timeFormat.Name = "timeFormat";
            this.timeFormat.Size = new System.Drawing.Size(146, 21);
            this.timeFormat.TabIndex = 34;
            this.timeFormat.Text = "Automatic";
            // 
            // labelCombinedTitle
            // 
            this.labelCombinedTitle.AutoSize = true;
            this.labelCombinedTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelCombinedTitle.Location = new System.Drawing.Point(16, 13);
            this.labelCombinedTitle.Name = "labelCombinedTitle";
            this.labelCombinedTitle.Size = new System.Drawing.Size(148, 13);
            this.labelCombinedTitle.TabIndex = 36;
            this.labelCombinedTitle.Text = "Combined Date and Time";
            // 
            // labelSeparate
            // 
            this.labelSeparate.AutoSize = true;
            this.labelSeparate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelSeparate.Location = new System.Drawing.Point(16, 63);
            this.labelSeparate.Name = "labelSeparate";
            this.labelSeparate.Size = new System.Drawing.Size(144, 13);
            this.labelSeparate.TabIndex = 37;
            this.labelSeparate.Text = "Separate Date and Time";
            // 
            // labelDateFormat
            // 
            this.labelDateFormat.AutoSize = true;
            this.labelDateFormat.Location = new System.Drawing.Point(37, 87);
            this.labelDateFormat.Name = "labelDateFormat";
            this.labelDateFormat.Size = new System.Drawing.Size(69, 13);
            this.labelDateFormat.TabIndex = 39;
            this.labelDateFormat.Text = "Date format:";
            // 
            // dateFormat
            // 
            this.dateFormat.FormattingEnabled = true;
            this.dateFormat.Location = new System.Drawing.Point(112, 84);
            this.dateFormat.Name = "dateFormat";
            this.dateFormat.Size = new System.Drawing.Size(146, 21);
            this.dateFormat.TabIndex = 38;
            this.dateFormat.Text = "Automatic";
            // 
            // labelTimeZoneTitle
            // 
            this.labelTimeZoneTitle.AutoSize = true;
            this.labelTimeZoneTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelTimeZoneTitle.Location = new System.Drawing.Point(16, 139);
            this.labelTimeZoneTitle.Name = "labelTimeZoneTitle";
            this.labelTimeZoneTitle.Size = new System.Drawing.Size(66, 13);
            this.labelTimeZoneTitle.TabIndex = 40;
            this.labelTimeZoneTitle.Text = "Time Zone";
            // 
            // GenericCsvDateSettingsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(351, 282);
            this.Controls.Add(this.labelTimeZoneTitle);
            this.Controls.Add(this.labelDateFormat);
            this.Controls.Add(this.dateFormat);
            this.Controls.Add(this.labelSeparate);
            this.Controls.Add(this.labelCombinedTitle);
            this.Controls.Add(this.labelTimeFormat);
            this.Controls.Add(this.timeFormat);
            this.Controls.Add(this.labelCombined);
            this.Controls.Add(this.labelTimezone2);
            this.Controls.Add(this.comboTimezone);
            this.Controls.Add(this.dateTimeFormat);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenericCsvDateSettingsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Date Format";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Label labelTimezone2;
        private System.Windows.Forms.ComboBox comboTimezone;
        private System.Windows.Forms.ComboBox dateTimeFormat;
        private System.Windows.Forms.Label labelCombined;
        private System.Windows.Forms.Label labelTimeFormat;
        private System.Windows.Forms.ComboBox timeFormat;
        private System.Windows.Forms.Label labelCombinedTitle;
        private System.Windows.Forms.Label labelSeparate;
        private System.Windows.Forms.Label labelDateFormat;
        private System.Windows.Forms.ComboBox dateFormat;
        private System.Windows.Forms.Label labelTimeZoneTitle;
    }
}