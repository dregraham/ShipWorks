namespace ShipWorks.Shipping.Carriers.EquaShip
{
    partial class EquaShipSettingsControl
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
            this.accountsControl = new ShipWorks.Shipping.Carriers.EquaShip.EquaShipAccountManagerControl();
            this.labelAccounts = new System.Windows.Forms.Label();
            this.optionsControl = new ShipWorks.Shipping.Carriers.EquaShip.EquaShipOptionsControl();
            this.SuspendLayout();
            // 
            // accountsControl
            // 
            this.accountsControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.accountsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountsControl.Location = new System.Drawing.Point(31, 112);
            this.accountsControl.Name = "accountsControl";
            this.accountsControl.Size = new System.Drawing.Size(400, 168);
            this.accountsControl.TabIndex = 0;
            // 
            // labelAccounts
            // 
            this.labelAccounts.AutoSize = true;
            this.labelAccounts.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccounts.Location = new System.Drawing.Point(18, 90);
            this.labelAccounts.Name = "labelAccounts";
            this.labelAccounts.Size = new System.Drawing.Size(112, 13);
            this.labelAccounts.TabIndex = 1;
            this.labelAccounts.Text = "EquaShip Accounts";
            // 
            // optionsControl
            // 
            this.optionsControl.Location = new System.Drawing.Point(12, 3);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(323, 72);
            this.optionsControl.TabIndex = 2;
            // 
            // EquashipSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.optionsControl);
            this.Controls.Add(this.labelAccounts);
            this.Controls.Add(this.accountsControl);
            this.Name = "EquashipSettingsControl";
            this.Size = new System.Drawing.Size(445, 297);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private EquaShipAccountManagerControl accountsControl;
        private System.Windows.Forms.Label labelAccounts;
        private EquaShipOptionsControl optionsControl;
    }
}
