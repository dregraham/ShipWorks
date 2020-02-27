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
            // OneBalanceAccountAddressPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.personControl);
            this.Description = "Please confirm your pickup address information for this account.";
            this.Name = "OneBalanceAccountAddressPage";
            this.Size = new System.Drawing.Size(579, 474);
            this.Title = "Account Registration";
            this.ResumeLayout(false);

        }

        #endregion

        private Data.Controls.AutofillPersonControl personControl;
    }
}
