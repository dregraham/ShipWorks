namespace ShipWorks.Shipping.Carriers.UPS
{
    partial class UpsAccountEditorDlg
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
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelOptional = new System.Windows.Forms.Label();
            this.labelAccount = new System.Windows.Forms.Label();
            this.labelUpsAccount = new System.Windows.Forms.Label();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageAccount = new System.Windows.Forms.TabPage();
            this.tabPageRates = new System.Windows.Forms.TabPage();
            this.tabPageLocalRating = new System.Windows.Forms.TabPage();
            this.LocalRateControlHost = new System.Windows.Forms.Integration.ElementHost();
            this.accountNumber = new System.Windows.Forms.TextBox();
            this.description = new ShipWorks.UI.Controls.PromptTextBox();
            this.personControl = new ShipWorks.Data.Controls.PersonControl();
            this.upsRateTypeControl = new ShipWorks.Shipping.Carriers.UPS.UpsAccountRateTypeControl();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.tabControl.SuspendLayout();
            this.tabPageAccount.SuspendLayout();
            this.tabPageRates.SuspendLayout();
            this.tabPageLocalRating.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(14, 55);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(64, 13);
            this.labelDescription.TabIndex = 3;
            this.labelDescription.Text = "Description:";
            // 
            // labelOptional
            // 
            this.labelOptional.AutoSize = true;
            this.labelOptional.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelOptional.Location = new System.Drawing.Point(302, 55);
            this.labelOptional.Name = "labelOptional";
            this.labelOptional.Size = new System.Drawing.Size(53, 13);
            this.labelOptional.TabIndex = 24;
            this.labelOptional.Text = "(optional)";
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Location = new System.Drawing.Point(28, 28);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(50, 13);
            this.labelAccount.TabIndex = 1;
            this.labelAccount.Text = "Account:";
            // 
            // labelUpsAccount
            // 
            this.labelUpsAccount.AutoSize = true;
            this.labelUpsAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUpsAccount.Location = new System.Drawing.Point(9, 7);
            this.labelUpsAccount.Name = "labelUpsAccount";
            this.labelUpsAccount.Size = new System.Drawing.Size(78, 13);
            this.labelUpsAccount.TabIndex = 0;
            this.labelUpsAccount.Text = "UPS Account";
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(256, 492);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 1;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(337, 492);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 2;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageAccount);
            this.tabControl.Controls.Add(this.tabPageRates);
            this.tabControl.Controls.Add(this.tabPageLocalRating);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(400, 473);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageAccount
            // 
            this.tabPageAccount.Controls.Add(this.labelUpsAccount);
            this.tabPageAccount.Controls.Add(this.labelDescription);
            this.tabPageAccount.Controls.Add(this.accountNumber);
            this.tabPageAccount.Controls.Add(this.description);
            this.tabPageAccount.Controls.Add(this.personControl);
            this.tabPageAccount.Controls.Add(this.labelOptional);
            this.tabPageAccount.Controls.Add(this.labelAccount);
            this.tabPageAccount.Location = new System.Drawing.Point(4, 22);
            this.tabPageAccount.Name = "tabPageAccount";
            this.tabPageAccount.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAccount.Size = new System.Drawing.Size(392, 447);
            this.tabPageAccount.TabIndex = 0;
            this.tabPageAccount.Text = "Account";
            this.tabPageAccount.UseVisualStyleBackColor = true;
            // 
            // tabPageRates
            // 
            this.tabPageRates.Controls.Add(this.upsRateTypeControl);
            this.tabPageRates.Location = new System.Drawing.Point(4, 22);
            this.tabPageRates.Name = "tabPageRates";
            this.tabPageRates.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRates.Size = new System.Drawing.Size(392, 447);
            this.tabPageRates.TabIndex = 1;
            this.tabPageRates.Text = "Rates";
            this.tabPageRates.UseVisualStyleBackColor = true;
            // 
            // tabPageLocalRating
            // 
            this.tabPageLocalRating.Controls.Add(this.LocalRateControlHost);
            this.tabPageLocalRating.Location = new System.Drawing.Point(4, 22);
            this.tabPageLocalRating.Name = "tabPageLocalRating";
            this.tabPageLocalRating.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLocalRating.Size = new System.Drawing.Size(392, 447);
            this.tabPageLocalRating.TabIndex = 2;
            this.tabPageLocalRating.Text = "Local Rating";
            this.tabPageLocalRating.UseVisualStyleBackColor = true;
            // 
            // LocalRateControlHost
            // 
            this.LocalRateControlHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LocalRateControlHost.Location = new System.Drawing.Point(3, 3);
            this.LocalRateControlHost.Margin = new System.Windows.Forms.Padding(0);
            this.LocalRateControlHost.Name = "LocalRateControlHost";
            this.LocalRateControlHost.Size = new System.Drawing.Size(386, 441);
            this.LocalRateControlHost.TabIndex = 0;
            this.LocalRateControlHost.Text = "elementHost1";
            this.LocalRateControlHost.Child = null;
            // 
            // accountNumber
            // 
            this.accountNumber.Location = new System.Drawing.Point(85, 25);
            this.accountNumber.Name = "accountNumber";
            this.accountNumber.ReadOnly = true;
            this.accountNumber.Size = new System.Drawing.Size(165, 21);
            this.accountNumber.TabIndex = 2;
            // 
            // description
            // 
            this.description.Location = new System.Drawing.Point(85, 52);
            this.fieldLengthProvider.SetMaxLengthSource(this.description, ShipWorks.Data.Utility.EntityFieldLengthSource.UpsAccountDescription);
            this.description.Name = "description";
            this.description.PromptColor = System.Drawing.SystemColors.GrayText;
            this.description.PromptText = null;
            this.description.Size = new System.Drawing.Size(211, 21);
            this.description.TabIndex = 4;
            // 
            // personControl
            // 
            this.personControl.AddressSelector = null;
            this.personControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)(((((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Residential) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone) 
            | ShipWorks.Data.Controls.PersonFields.Website)));
            this.personControl.EnableValidationControls = false;
            this.personControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.personControl.FullName = "";
            this.personControl.Location = new System.Drawing.Point(6, 79);
            this.personControl.Name = "personControl";
            this.personControl.Size = new System.Drawing.Size(358, 362);
            this.personControl.TabIndex = 5;
            this.personControl.ValidatedAddressScope = null;
            this.personControl.ContentChanged += new System.EventHandler(this.OnPersonContentChanged);
            // 
            // upsRateTypeControl
            // 
            this.upsRateTypeControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upsRateTypeControl.Location = new System.Drawing.Point(3, 3);
            this.upsRateTypeControl.Name = "upsRateTypeControl";
            this.upsRateTypeControl.Size = new System.Drawing.Size(389, 444);
            this.upsRateTypeControl.TabIndex = 0;
            // 
            // UpsAccountEditorDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(424, 527);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.cancel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpsAccountEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UPS Account";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.tabControl.ResumeLayout(false);
            this.tabPageAccount.ResumeLayout(false);
            this.tabPageAccount.PerformLayout();
            this.tabPageRates.ResumeLayout(false);
            this.tabPageLocalRating.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelDescription;
        private ShipWorks.UI.Controls.PromptTextBox description;
        private System.Windows.Forms.Label labelOptional;
        private System.Windows.Forms.Label labelAccount;
        private ShipWorks.Data.Controls.PersonControl personControl;
        private System.Windows.Forms.TextBox accountNumber;
        private System.Windows.Forms.Label labelUpsAccount;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageAccount;
        private System.Windows.Forms.TabPage tabPageRates;
        private ShipWorks.Shipping.Carriers.UPS.UpsAccountRateTypeControl upsRateTypeControl;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.TabPage tabPageLocalRating;
        private System.Windows.Forms.Integration.ElementHost LocalRateControlHost;
    }
}
