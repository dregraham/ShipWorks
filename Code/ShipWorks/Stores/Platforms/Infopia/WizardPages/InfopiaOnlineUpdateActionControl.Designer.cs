namespace ShipWorks.Stores.Platforms.Infopia.WizardPages
{
    partial class InfopiaOnlineUpdateActionControl
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
            this.comboStatus = new System.Windows.Forms.ComboBox();
            this.statusUpdate = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // shipmentUpdate
            // 
            this.shipmentUpdate.AutoSize = true;
            this.shipmentUpdate.Checked = true;
            this.shipmentUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shipmentUpdate.Location = new System.Drawing.Point(15, 7);
            this.shipmentUpdate.Name = "shipmentUpdate";
            this.shipmentUpdate.Size = new System.Drawing.Size(288, 17);
            this.shipmentUpdate.TabIndex = 1;
            this.shipmentUpdate.Text = "Upload the shipment tracking number and service used";
            this.shipmentUpdate.UseVisualStyleBackColor = true;
            // 
            // comboStatus
            // 
            this.comboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.Location = new System.Drawing.Point(185, 29);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.Size = new System.Drawing.Size(153, 21);
            this.comboStatus.TabIndex = 5;
            // 
            // statusUpdate
            // 
            this.statusUpdate.AutoSize = true;
            this.statusUpdate.Checked = true;
            this.statusUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statusUpdate.Location = new System.Drawing.Point(15, 31);
            this.statusUpdate.Name = "statusUpdate";
            this.statusUpdate.Size = new System.Drawing.Size(171, 17);
            this.statusUpdate.TabIndex = 4;
            this.statusUpdate.Text = "Set the online order status to:";
            this.statusUpdate.UseVisualStyleBackColor = true;
            // 
            // InfopiaOnlineUpdateActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboStatus);
            this.Controls.Add(this.statusUpdate);
            this.Controls.Add(this.shipmentUpdate);
            this.Name = "InfopiaOnlineUpdateActionControl";
            this.Size = new System.Drawing.Size(459, 58);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox shipmentUpdate;
        private System.Windows.Forms.ComboBox comboStatus;
        private System.Windows.Forms.CheckBox statusUpdate;
    }
}
