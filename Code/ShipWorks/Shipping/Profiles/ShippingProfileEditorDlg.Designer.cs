namespace ShipWorks.Shipping.Profiles
{
    partial class ShippingProfileEditorDlg
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.labelName = new System.Windows.Forms.Label();
            this.profileName = new System.Windows.Forms.TextBox();
            this.labelAppliesTo = new System.Windows.Forms.Label();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.labelShipmentType = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(447, 486);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 6;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(366, 486);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 5;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(33, 10);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name:";
            // 
            // profileName
            // 
            this.profileName.Location = new System.Drawing.Point(74, 7);
            this.fieldLengthProvider.SetMaxLengthSource(this.profileName, ShipWorks.Data.Utility.EntityFieldLengthSource.ShippingProfileName);
            this.profileName.Name = "profileName";
            this.profileName.Size = new System.Drawing.Size(148, 21);
            this.profileName.TabIndex = 1;
            // 
            // labelAppliesTo
            // 
            this.labelAppliesTo.AutoSize = true;
            this.labelAppliesTo.Location = new System.Drawing.Point(12, 36);
            this.labelAppliesTo.Name = "labelAppliesTo";
            this.labelAppliesTo.Size = new System.Drawing.Size(58, 13);
            this.labelAppliesTo.TabIndex = 2;
            this.labelAppliesTo.Text = "Applies to:";
            // 
            // panelSettings
            // 
            this.panelSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSettings.Location = new System.Drawing.Point(15, 61);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(507, 419);
            this.panelSettings.TabIndex = 4;
            // 
            // labelShipmentType
            // 
            this.labelShipmentType.AutoSize = true;
            this.labelShipmentType.Location = new System.Drawing.Point(76, 36);
            this.labelShipmentType.Name = "labelShipmentType";
            this.labelShipmentType.Size = new System.Drawing.Size(78, 13);
            this.labelShipmentType.TabIndex = 3;
            this.labelShipmentType.Text = "Shipment Type";
            // 
            // ShippingProfileEditorDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(534, 521);
            this.Controls.Add(this.labelShipmentType);
            this.Controls.Add(this.panelSettings);
            this.Controls.Add(this.labelAppliesTo);
            this.Controls.Add(this.profileName);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.cancel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(550, 2400);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(550, 394);
            this.Name = "ShippingProfileEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Shipping Profile";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox profileName;
        private System.Windows.Forms.Label labelAppliesTo;
        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.Label labelShipmentType;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
    }
}