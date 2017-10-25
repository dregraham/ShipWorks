namespace ShipWorks.Shipping.Editing
{
    partial class ShippingDateCutoffControl
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
            this.cutoffEnabled = new System.Windows.Forms.CheckBox();
            this.cutoffTime = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // cutoffEnabled
            // 
            this.cutoffEnabled.AutoSize = true;
            this.cutoffEnabled.Location = new System.Drawing.Point(3, 3);
            this.cutoffEnabled.Name = "cutoffEnabled";
            this.cutoffEnabled.Size = new System.Drawing.Size(386, 17);
            this.cutoffEnabled.TabIndex = 0;
            this.cutoffEnabled.Text = "Use the next business day as the Ship Date for shipments processed after:";
            this.cutoffEnabled.UseVisualStyleBackColor = true;
            this.cutoffEnabled.CheckedChanged += new System.EventHandler(this.OnCutoffEnabledCheckedChanged);
            // 
            // cutoffTime
            // 
            this.cutoffTime.CustomFormat = "h:mm tt";
            this.cutoffTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.cutoffTime.Location = new System.Drawing.Point(387, 0);
            this.cutoffTime.Name = "cutoffTime";
            this.cutoffTime.ShowUpDown = true;
            this.cutoffTime.Size = new System.Drawing.Size(78, 21);
            this.cutoffTime.TabIndex = 1;
            // 
            // ShippingDateCutoffControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cutoffTime);
            this.Controls.Add(this.cutoffEnabled);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "ShippingDateCutoffControl";
            this.Size = new System.Drawing.Size(474, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cutoffEnabled;
        private System.Windows.Forms.DateTimePicker cutoffTime;
    }
}
