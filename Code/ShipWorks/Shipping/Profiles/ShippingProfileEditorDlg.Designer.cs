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
            this.providerLabel = new System.Windows.Forms.Label();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.shortcutLabel = new System.Windows.Forms.Label();
            this.barcodeLabel = new System.Windows.Forms.Label();
            this.provider = new System.Windows.Forms.ComboBox();
            this.keyboardShortcut = new System.Windows.Forms.ComboBox();
            this.barcode = new System.Windows.Forms.TextBox();
            this.profileName = new System.Windows.Forms.TextBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(450, 489);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 6;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(367, 489);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 5;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOk);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(27, 13);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name:";
            // 
            // providerLabel
            // 
            this.providerLabel.AutoSize = true;
            this.providerLabel.Location = new System.Drawing.Point(14, 75);
            this.providerLabel.Name = "providerLabel";
            this.providerLabel.Size = new System.Drawing.Size(51, 13);
            this.providerLabel.TabIndex = 2;
            this.providerLabel.Text = "Provider:";
            // 
            // panelSettings
            // 
            this.panelSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSettings.Location = new System.Drawing.Point(10, 103);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(516, 378);
            this.panelSettings.TabIndex = 4;
            // 
            // shortcutLabel
            // 
            this.shortcutLabel.AutoSize = true;
            this.shortcutLabel.Location = new System.Drawing.Point(254, 44);
            this.shortcutLabel.Name = "shortcutLabel";
            this.shortcutLabel.Size = new System.Drawing.Size(101, 13);
            this.shortcutLabel.TabIndex = 10;
            this.shortcutLabel.Text = "Keyboard Shortcut:";
            // 
            // barcodeLabel
            // 
            this.barcodeLabel.AutoSize = true;
            this.barcodeLabel.Location = new System.Drawing.Point(15, 44);
            this.barcodeLabel.Name = "barcodeLabel";
            this.barcodeLabel.Size = new System.Drawing.Size(50, 13);
            this.barcodeLabel.TabIndex = 8;
            this.barcodeLabel.Text = "Barcode:";
            // 
            // provider
            // 
            this.provider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.provider.FormattingEnabled = true;
            this.provider.Location = new System.Drawing.Point(69, 72);
            this.provider.Name = "provider";
            this.provider.Size = new System.Drawing.Size(165, 21);
            this.provider.TabIndex = 3;
            this.provider.SelectedValueChanged += new System.EventHandler(this.OnChangeProvider);
            // 
            // keyboardShortcut
            // 
            this.keyboardShortcut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.keyboardShortcut.FormattingEnabled = true;
            this.keyboardShortcut.Location = new System.Drawing.Point(359, 41);
            this.keyboardShortcut.Name = "keyboardShortcut";
            this.keyboardShortcut.Size = new System.Drawing.Size(165, 21);
            this.keyboardShortcut.TabIndex = 2;
            // 
            // barcode
            // 
            this.barcode.Location = new System.Drawing.Point(69, 41);
            this.fieldLengthProvider.SetMaxLengthSource(this.barcode, ShipWorks.Data.Utility.EntityFieldLengthSource.ShippingProfileName);
            this.barcode.Name = "barcode";
            this.barcode.Size = new System.Drawing.Size(165, 21);
            this.barcode.TabIndex = 1;
            this.barcode.Enter += OnEnterBarcode;
            this.barcode.Leave += OnLeaveBarcode;
            // 
            // profileName
            // 
            this.profileName.Location = new System.Drawing.Point(69, 10);
            this.fieldLengthProvider.SetMaxLengthSource(this.profileName, ShipWorks.Data.Utility.EntityFieldLengthSource.ShippingProfileName);
            this.profileName.Name = "profileName";
            this.profileName.Size = new System.Drawing.Size(165, 21);
            this.profileName.TabIndex = 0;
            this.profileName.Enter += OnEnterBarcode;
            this.profileName.Leave += OnLeaveBarcode;
            // 
            // ShippingProfileEditorDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(534, 521);
            this.Controls.Add(this.provider);
            this.Controls.Add(this.keyboardShortcut);
            this.Controls.Add(this.providerLabel);
            this.Controls.Add(this.shortcutLabel);
            this.Controls.Add(this.barcode);
            this.Controls.Add(this.barcodeLabel);
            this.Controls.Add(this.panelSettings);
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
        private System.Windows.Forms.Label providerLabel;
        private System.Windows.Forms.Panel panelSettings;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.ComboBox provider;
        private System.Windows.Forms.ComboBox keyboardShortcut;
        private System.Windows.Forms.Label shortcutLabel;
        private System.Windows.Forms.TextBox barcode;
        private System.Windows.Forms.Label barcodeLabel;
    }
}