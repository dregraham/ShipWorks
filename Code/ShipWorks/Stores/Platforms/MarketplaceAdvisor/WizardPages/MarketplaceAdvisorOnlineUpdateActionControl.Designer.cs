namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.WizardPages
{
    partial class MarketplaceAdvisorOnlineUpdateActionControl
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
            this.promote = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // shipmentUpdate
            // 
            this.shipmentUpdate.AutoSize = true;
            this.shipmentUpdate.Checked = true;
            this.shipmentUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shipmentUpdate.Location = new System.Drawing.Point(17, 4);
            this.shipmentUpdate.Name = "shipmentUpdate";
            this.shipmentUpdate.Size = new System.Drawing.Size(308, 17);
            this.shipmentUpdate.TabIndex = 3;
            this.shipmentUpdate.Text = "Update the online order with the shipment tracking number";
            this.shipmentUpdate.UseVisualStyleBackColor = true;
            // 
            // promote
            // 
            this.promote.AutoSize = true;
            this.promote.Checked = true;
            this.promote.CheckState = System.Windows.Forms.CheckState.Checked;
            this.promote.Location = new System.Drawing.Point(17, 27);
            this.promote.Name = "promote";
            this.promote.Size = new System.Drawing.Size(272, 17);
            this.promote.TabIndex = 4;
            this.promote.Text = "Promote the online order to the next workflow step";
            this.promote.UseVisualStyleBackColor = true;
            // 
            // MarketplaceAdvisorOnlineUpdateActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.promote);
            this.Controls.Add(this.shipmentUpdate);
            this.Name = "MarketplaceAdvisorOnlineUpdateActionControl";
            this.Size = new System.Drawing.Size(459, 47);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox shipmentUpdate;
        private System.Windows.Forms.CheckBox promote;
    }
}
