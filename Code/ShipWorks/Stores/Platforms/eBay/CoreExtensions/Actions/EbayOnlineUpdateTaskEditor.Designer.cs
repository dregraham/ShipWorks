namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Actions
{
    partial class EbayOnlineUpdateTaskEditor
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
            this.shippedCheckBox = new System.Windows.Forms.CheckBox();
            this.paidCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // shippedCheckBox
            // 
            this.shippedCheckBox.AutoSize = true;
            this.shippedCheckBox.Location = new System.Drawing.Point(27, 0);
            this.shippedCheckBox.Name = "shippedCheckBox";
            this.shippedCheckBox.Size = new System.Drawing.Size(104, 17);
            this.shippedCheckBox.TabIndex = 0;
            this.shippedCheckBox.Text = "Mark as &Shipped";
            this.shippedCheckBox.UseVisualStyleBackColor = true;
            this.shippedCheckBox.CheckedChanged += new System.EventHandler(this.OnShippedCheckedChanged);
            // 
            // paidCheckBox
            // 
            this.paidCheckBox.AutoSize = true;
            this.paidCheckBox.Location = new System.Drawing.Point(27, 18);
            this.paidCheckBox.Name = "paidCheckBox";
            this.paidCheckBox.Size = new System.Drawing.Size(86, 17);
            this.paidCheckBox.TabIndex = 1;
            this.paidCheckBox.Text = "Mark as &Paid";
            this.paidCheckBox.UseVisualStyleBackColor = true;
            this.paidCheckBox.CheckedChanged += new System.EventHandler(this.OnPaidCheckedChanged);
            // 
            // EbayOnlineUpdateTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.paidCheckBox);
            this.Controls.Add(this.shippedCheckBox);
            this.Name = "EbayOnlineUpdateTaskEditor";
            this.Size = new System.Drawing.Size(152, 37);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox shippedCheckBox;
        private System.Windows.Forms.CheckBox paidCheckBox;
    }
}
