namespace ShipWorks.Stores.Platforms.Ebay.WizardPages
{
    partial class EbayOnlineUpdateActionControl
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
            this.markShipped = new System.Windows.Forms.CheckBox();
            this.markPaid = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // markShipped
            // 
            this.markShipped.AutoSize = true;
            this.markShipped.Checked = true;
            this.markShipped.CheckState = System.Windows.Forms.CheckState.Checked;
            this.markShipped.Location = new System.Drawing.Point(8, 3);
            this.markShipped.Name = "markShipped";
            this.markShipped.Size = new System.Drawing.Size(205, 17);
            this.markShipped.TabIndex = 0;
            this.markShipped.Text = "Mark the item as \'Shipped\' in My eBay";
            this.markShipped.UseVisualStyleBackColor = true;
            // 
            // markPaid
            // 
            this.markPaid.AutoSize = true;
            this.markPaid.Checked = true;
            this.markPaid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.markPaid.Location = new System.Drawing.Point(8, 26);
            this.markPaid.Name = "markPaid";
            this.markPaid.Size = new System.Drawing.Size(187, 17);
            this.markPaid.TabIndex = 1;
            this.markPaid.Text = "Mark the item as \'Paid\' in My eBay";
            this.markPaid.UseVisualStyleBackColor = true;
            // 
            // EbayOnlineUpdateActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.markPaid);
            this.Controls.Add(this.markShipped);
            this.Name = "EbayOnlineUpdateActionControl";
            this.Size = new System.Drawing.Size(459, 44);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox markShipped;
        private System.Windows.Forms.CheckBox markPaid;
    }
}
