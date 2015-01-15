namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    partial class UpsPickupLocationControl
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
            this.labelPickupLocationHeader = new System.Windows.Forms.Label();
            this.pickupLocationPersonControl = new ShipWorks.Data.Controls.AutofillPersonControl();
            this.SuspendLayout();
            // 
            // labelPickupLocationHeader
            // 
            this.labelPickupLocationHeader.AutoSize = true;
            this.labelPickupLocationHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPickupLocationHeader.Location = new System.Drawing.Point(3, 0);
            this.labelPickupLocationHeader.Name = "labelPickupLocationHeader";
            this.labelPickupLocationHeader.Size = new System.Drawing.Size(93, 13);
            this.labelPickupLocationHeader.TabIndex = 0;
            this.labelPickupLocationHeader.Text = "Pickup Address";
            // 
            // pickupLocationPersonControl
            // 
            this.pickupLocationPersonControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)(((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.pickupLocationPersonControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pickupLocationPersonControl.Location = new System.Drawing.Point(6, 14);
            this.pickupLocationPersonControl.Name = "pickupLocationPersonControl";
            this.pickupLocationPersonControl.RequiredFields = ((ShipWorks.Data.Controls.PersonFields)(((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.pickupLocationPersonControl.Size = new System.Drawing.Size(355, 357);
            this.pickupLocationPersonControl.TabIndex = 1;
            // 
            // UpsPickupLocationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pickupLocationPersonControl);
            this.Controls.Add(this.labelPickupLocationHeader);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UpsPickupLocationControl";
            this.Size = new System.Drawing.Size(369, 375);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPickupLocationHeader;
        private Data.Controls.AutofillPersonControl pickupLocationPersonControl;
    }
}
