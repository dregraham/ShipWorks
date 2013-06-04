namespace ShipWorks.Stores.Platforms.CommerceInterface.WizardPages
{
    partial class CommerceInterfaceOnlineUpdateActionControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboStatus = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // shipmentUpdate
            // 
            this.shipmentUpdate.AutoSize = true;
            this.shipmentUpdate.Checked = true;
            this.shipmentUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shipmentUpdate.Location = new System.Drawing.Point(3, 3);
            this.shipmentUpdate.Name = "shipmentUpdate";
            this.shipmentUpdate.Size = new System.Drawing.Size(204, 17);
            this.shipmentUpdate.TabIndex = 1;
            this.shipmentUpdate.Text = "Upload the shipment tracking number";
            this.shipmentUpdate.UseVisualStyleBackColor = true;
            this.shipmentUpdate.CheckedChanged += new System.EventHandler(this.OnUploadCheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Set Order Status to:";
            // 
            // comboStatus
            // 
            this.comboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.Location = new System.Drawing.Point(150, 26);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.Size = new System.Drawing.Size(153, 21);
            this.comboStatus.TabIndex = 4;
            // 
            // CommerceInterfaceOnlineUpdateActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.shipmentUpdate);
            this.Name = "CommerceInterfaceOnlineUpdateActionControl";
            this.Size = new System.Drawing.Size(318, 57);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox shipmentUpdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboStatus;

    }
}
