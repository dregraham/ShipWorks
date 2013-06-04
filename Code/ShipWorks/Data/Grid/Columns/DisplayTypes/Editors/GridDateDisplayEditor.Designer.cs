namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Editors
{
    partial class GridDateDisplayEditor
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
            this.showTime = new System.Windows.Forms.CheckBox();
            this.dateFormat = new System.Windows.Forms.ListBox();
            this.radio12Hour = new System.Windows.Forms.RadioButton();
            this.radio24hour = new System.Windows.Forms.RadioButton();
            this.showDate = new System.Windows.Forms.CheckBox();
            this.todayYesterday = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // showTime
            // 
            this.showTime.AutoSize = true;
            this.showTime.Checked = true;
            this.showTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showTime.Location = new System.Drawing.Point(6, 206);
            this.showTime.Name = "showTime";
            this.showTime.Size = new System.Drawing.Size(77, 17);
            this.showTime.TabIndex = 5;
            this.showTime.Text = "Show Time";
            this.showTime.UseVisualStyleBackColor = true;
            // 
            // dateFormat
            // 
            this.dateFormat.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dateFormat.FormattingEnabled = true;
            this.dateFormat.Items.AddRange(new object[] {
            "MM/dd/yyyy",
            "MM/dd/yy",
            "M/d/yyyy",
            "M/d/yy",
            "dd-MMM-yy",
            "dd-MMM-yyy",
            "d-MMM-yy",
            "MMMM dd, yyyy",
            "dddd, MMMM dd, yyyy"});
            this.dateFormat.Location = new System.Drawing.Point(24, 55);
            this.dateFormat.Name = "dateFormat";
            this.dateFormat.Size = new System.Drawing.Size(193, 121);
            this.dateFormat.TabIndex = 3;
            this.dateFormat.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDrawDateItem);
            // 
            // radio12Hour
            // 
            this.radio12Hour.AutoSize = true;
            this.radio12Hour.Location = new System.Drawing.Point(24, 225);
            this.radio12Hour.Name = "radio12Hour";
            this.radio12Hour.Size = new System.Drawing.Size(89, 17);
            this.radio12Hour.TabIndex = 6;
            this.radio12Hour.TabStop = true;
            this.radio12Hour.Text = "12-hour clock";
            this.radio12Hour.UseVisualStyleBackColor = true;
            // 
            // radio24hour
            // 
            this.radio24hour.AutoSize = true;
            this.radio24hour.Location = new System.Drawing.Point(24, 248);
            this.radio24hour.Name = "radio24hour";
            this.radio24hour.Size = new System.Drawing.Size(89, 17);
            this.radio24hour.TabIndex = 7;
            this.radio24hour.TabStop = true;
            this.radio24hour.Text = "24-hour clock";
            this.radio24hour.UseVisualStyleBackColor = true;
            // 
            // showDate
            // 
            this.showDate.AutoSize = true;
            this.showDate.Checked = true;
            this.showDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showDate.Location = new System.Drawing.Point(6, 36);
            this.showDate.Name = "showDate";
            this.showDate.Size = new System.Drawing.Size(78, 17);
            this.showDate.TabIndex = 2;
            this.showDate.Text = "Show Date";
            this.showDate.UseVisualStyleBackColor = true;
            // 
            // todayYesterday
            // 
            this.todayYesterday.AutoSize = true;
            this.todayYesterday.Location = new System.Drawing.Point(24, 182);
            this.todayYesterday.Name = "todayYesterday";
            this.todayYesterday.Size = new System.Drawing.Size(166, 17);
            this.todayYesterday.TabIndex = 4;
            this.todayYesterday.Text = "Use \"Today\" and \"Yesterday\"";
            this.todayYesterday.UseVisualStyleBackColor = true;
            // 
            // GridDateDisplayEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.todayYesterday);
            this.Controls.Add(this.showDate);
            this.Controls.Add(this.radio24hour);
            this.Controls.Add(this.dateFormat);
            this.Controls.Add(this.radio12Hour);
            this.Controls.Add(this.showTime);
            this.Name = "GridDateDisplayEditor";
            this.Size = new System.Drawing.Size(220, 306);
            this.Controls.SetChildIndex(this.showTime, 0);
            this.Controls.SetChildIndex(this.radio12Hour, 0);
            this.Controls.SetChildIndex(this.dateFormat, 0);
            this.Controls.SetChildIndex(this.radio24hour, 0);
            this.Controls.SetChildIndex(this.showDate, 0);
            this.Controls.SetChildIndex(this.todayYesterday, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox showTime;
        private System.Windows.Forms.ListBox dateFormat;
        private System.Windows.Forms.RadioButton radio12Hour;
        private System.Windows.Forms.RadioButton radio24hour;
        private System.Windows.Forms.CheckBox showDate;
        private System.Windows.Forms.CheckBox todayYesterday;
    }
}
