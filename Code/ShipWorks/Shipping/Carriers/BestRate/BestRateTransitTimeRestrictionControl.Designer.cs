namespace ShipWorks.Shipping.Carriers.BestRate
{
    partial class BestRateTransitTimeRestrictionControl
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
            this.anyTime = new System.Windows.Forms.RadioButton();
            this.expected = new System.Windows.Forms.RadioButton();
            this.expectedNumberOfDays = new ShipWorks.UI.Controls.NumericTextBox();
            this.SuspendLayout();
            // 
            // anyTime
            // 
            this.anyTime.AutoSize = true;
            this.anyTime.Checked = true;
            this.anyTime.Location = new System.Drawing.Point(4, 9);
            this.anyTime.Name = "anyTime";
            this.anyTime.Size = new System.Drawing.Size(69, 17);
            this.anyTime.TabIndex = 1;
            this.anyTime.TabStop = true;
            this.anyTime.Text = "Any Time";
            this.anyTime.UseVisualStyleBackColor = true;
            // 
            // expected
            // 
            this.expected.AutoSize = true;
            this.expected.Location = new System.Drawing.Point(4, 32);
            this.expected.Name = "expected";
            this.expected.Size = new System.Drawing.Size(148, 17);
            this.expected.TabIndex = 2;
            this.expected.Text = "Expected number of days:";
            this.expected.UseVisualStyleBackColor = true;
            // 
            // numericTextBox1
            // 
            this.expectedNumberOfDays.Location = new System.Drawing.Point(152, 32);
            this.expectedNumberOfDays.Name = "numericTextBox1";
            this.expectedNumberOfDays.Size = new System.Drawing.Size(32, 20);
            this.expectedNumberOfDays.TabIndex = 3;
            // 
            // BestRateTransitTimeRestictionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.expectedNumberOfDays);
            this.Controls.Add(this.expected);
            this.Controls.Add(this.anyTime);
            this.Name = "BestRateTransitTimeRestrictionControl";
            this.Size = new System.Drawing.Size(201, 77);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton anyTime;
        private System.Windows.Forms.RadioButton expected;
        private UI.Controls.NumericTextBox expectedNumberOfDays;
    }
}
