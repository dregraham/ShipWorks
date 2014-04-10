namespace ShipWorks.Stores.Platforms.ProStores.WizardPages
{
    partial class ProStoresOnlineUpdateActionControl
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
            this.shipmentUpdate = new System.Windows.Forms.CheckBox();
            this.labelOldVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // shipmentUpdate
            // 
            this.shipmentUpdate.AutoSize = true;
            this.shipmentUpdate.Checked = true;
            this.shipmentUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shipmentUpdate.Location = new System.Drawing.Point(18, 4);
            this.shipmentUpdate.Name = "shipmentUpdate";
            this.shipmentUpdate.Size = new System.Drawing.Size(262, 17);
            this.shipmentUpdate.TabIndex = 0;
            this.shipmentUpdate.Text = "Update the online order with the shipment details";
            this.shipmentUpdate.UseVisualStyleBackColor = true;
            // 
            // labelOldVersion
            // 
            this.labelOldVersion.AutoSize = true;
            this.labelOldVersion.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelOldVersion.Location = new System.Drawing.Point(32, 23);
            this.labelOldVersion.Name = "labelOldVersion";
            this.labelOldVersion.Size = new System.Drawing.Size(410, 13);
            this.labelOldVersion.TabIndex = 8;
            this.labelOldVersion.Text = "(You are using an older version of ProStores that does not support status updates" +
    ".)";
            // 
            // ProStoresOnlineUpdateActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelOldVersion);
            this.Controls.Add(this.shipmentUpdate);
            this.Name = "ProStoresOnlineUpdateActionControl";
            this.Size = new System.Drawing.Size(459, 43);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox shipmentUpdate;
        private System.Windows.Forms.Label labelOldVersion;
    }
}
