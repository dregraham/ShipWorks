namespace ShipWorks.Actions.Scheduling.ActionSchedules.Editors
{
    partial class HourlyActionScheduleEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.recurrsEveryNumberOfHours = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Run every:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "hours.";
            // 
            // recurrsEveryNumberOfHours
            // 
            this.recurrsEveryNumberOfHours.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.recurrsEveryNumberOfHours.FormattingEnabled = true;
            this.recurrsEveryNumberOfHours.Location = new System.Drawing.Point(71, 4);
            this.recurrsEveryNumberOfHours.Name = "recurrsEveryNumberOfHours";
            this.recurrsEveryNumberOfHours.Size = new System.Drawing.Size(36, 21);
            this.recurrsEveryNumberOfHours.TabIndex = 3;
            this.recurrsEveryNumberOfHours.SelectedIndexChanged += new System.EventHandler(this.OnRecurrenceHoursChanged);
            // 
            // HourlyActionScheduleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.recurrsEveryNumberOfHours);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "HourlyActionScheduleEditor";
            this.Size = new System.Drawing.Size(156, 31);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox recurrsEveryNumberOfHours;
    }
}
