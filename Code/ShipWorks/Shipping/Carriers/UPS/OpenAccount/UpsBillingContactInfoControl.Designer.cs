namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    partial class UpsBillingContactInfoControl
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
            this.labelBillingContactInfoHeader = new System.Windows.Forms.Label();
            this.billingContactPersonControl = new ShipWorks.Data.Controls.AutofillPersonControl();
            this.sameAsPickup = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelBillingContactInfoHeader
            // 
            this.labelBillingContactInfoHeader.AutoSize = true;
            this.labelBillingContactInfoHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBillingContactInfoHeader.Location = new System.Drawing.Point(3, 0);
            this.labelBillingContactInfoHeader.Name = "labelBillingContactInfoHeader";
            this.labelBillingContactInfoHeader.Size = new System.Drawing.Size(89, 13);
            this.labelBillingContactInfoHeader.TabIndex = 0;
            this.labelBillingContactInfoHeader.Text = "Billing Address";
            // 
            // billingContactPersonControl
            // 
            this.billingContactPersonControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)(((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.billingContactPersonControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.billingContactPersonControl.Location = new System.Drawing.Point(6, 14);
            this.billingContactPersonControl.Name = "billingContactPersonControl";
            this.billingContactPersonControl.RequiredFields = ((ShipWorks.Data.Controls.PersonFields)(((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.billingContactPersonControl.Size = new System.Drawing.Size(355, 352);
            this.billingContactPersonControl.TabIndex = 1;
            this.billingContactPersonControl.Resize += new System.EventHandler(this.OnBillingContactPersonControlResize);
            // 
            // sameAsPickup
            // 
            this.sameAsPickup.AutoSize = true;
            this.sameAsPickup.Checked = true;
            this.sameAsPickup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sameAsPickup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sameAsPickup.Location = new System.Drawing.Point(11, 371);
            this.sameAsPickup.Name = "sameAsPickup";
            this.sameAsPickup.Size = new System.Drawing.Size(244, 17);
            this.sameAsPickup.TabIndex = 3;
            this.sameAsPickup.Text = "This address is also my pickup address";
            this.sameAsPickup.UseVisualStyleBackColor = true;
            // 
            // UpsBillingContactInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sameAsPickup);
            this.Controls.Add(this.billingContactPersonControl);
            this.Controls.Add(this.labelBillingContactInfoHeader);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UpsBillingContactInfoControl";
            this.Size = new System.Drawing.Size(369, 399);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelBillingContactInfoHeader;
        private Data.Controls.AutofillPersonControl billingContactPersonControl;
        private System.Windows.Forms.CheckBox sameAsPickup;
    }
}
