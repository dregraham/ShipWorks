namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration.WizardPages
{
    partial class YahooOnlineUpdateActionControl
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
            this.statusUpdate = new System.Windows.Forms.CheckBox();
            this.labelDisabled = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // statusUpdate
            // 
            this.statusUpdate.AutoSize = true;
            this.statusUpdate.Checked = true;
            this.statusUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statusUpdate.Location = new System.Drawing.Point(24, 3);
            this.statusUpdate.Name = "statusUpdate";
            this.statusUpdate.Size = new System.Drawing.Size(268, 17);
            this.statusUpdate.TabIndex = 0;
            this.statusUpdate.Text = "Update the Yahoo! order with the shipment details";
            this.statusUpdate.UseVisualStyleBackColor = true;
            // 
            // labelDisabled
            // 
            this.labelDisabled.AutoSize = true;
            this.labelDisabled.ForeColor = System.Drawing.Color.Red;
            this.labelDisabled.Location = new System.Drawing.Point(39, 23);
            this.labelDisabled.Name = "labelDisabled";
            this.labelDisabled.Size = new System.Drawing.Size(361, 13);
            this.labelDisabled.TabIndex = 1;
            this.labelDisabled.Text = "(This is disabled because you did not enter an \"Email Tracking Password\".)";
            // 
            // YahooOnlineUpdateActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelDisabled);
            this.Controls.Add(this.statusUpdate);
            this.Name = "YahooOnlineUpdateActionControl";
            this.Size = new System.Drawing.Size(459, 55);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox statusUpdate;
        private System.Windows.Forms.Label labelDisabled;
    }
}
