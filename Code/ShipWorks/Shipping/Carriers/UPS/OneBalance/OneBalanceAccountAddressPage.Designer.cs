namespace ShipWorks.Shipping.Carriers.UPS.OneBalance
{
    partial class OneBalanceAccountAddressPage
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
            this.personControl = new ShipWorks.Data.Controls.AutofillPersonControl();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // personControl
            // 
            this.personControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)(((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Residential) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.personControl.Location = new System.Drawing.Point(21, 8);
            this.personControl.Name = "personControl";
            this.personControl.RequiredFields = ((ShipWorks.Data.Controls.PersonFields)(((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.personControl.Size = new System.Drawing.Size(346, 339);
            this.personControl.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label5.Location = new System.Drawing.Point(22, 442);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(533, 22);
            this.label5.TabIndex = 8;
            this.label5.Text = "UPS, the UPS Shield trademark, the UPS Ready mark, the UPS Developer Kit mark and" +
    " the Color Brown are trademarks of United Parcel Service of America, Inc. All Ri" +
    "ghts Reserved.";
            // 
            // OneBalanceAccountAddressPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.personControl);
            this.Description = "Please confirm your pickup address information for this account.";
            this.Name = "OneBalanceAccountAddressPage";
            this.Size = new System.Drawing.Size(579, 474);
            this.Title = "Account Registration";
            this.ResumeLayout(false);

        }

        #endregion

        private Data.Controls.AutofillPersonControl personControl;
        private System.Windows.Forms.Label label5;
    }
}
