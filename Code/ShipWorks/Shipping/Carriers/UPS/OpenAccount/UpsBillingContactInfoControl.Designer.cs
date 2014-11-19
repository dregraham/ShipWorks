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
            this.billingContactPersonControl = new ShipWorks.Data.Controls.PersonControl();
            this.labelSameAsPickup = new System.Windows.Forms.Label();
            this.labelPickupQuestion = new System.Windows.Forms.Label();
            this.sameAsPickupYes = new System.Windows.Forms.RadioButton();
            this.sameAsPickupNo = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // labelBillingContactInfoHeader
            // 
            this.labelBillingContactInfoHeader.AutoSize = true;
            this.labelBillingContactInfoHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBillingContactInfoHeader.Location = new System.Drawing.Point(3, 0);
            this.labelBillingContactInfoHeader.Name = "labelBillingContactInfoHeader";
            this.labelBillingContactInfoHeader.Size = new System.Drawing.Size(158, 13);
            this.labelBillingContactInfoHeader.TabIndex = 0;
            this.labelBillingContactInfoHeader.Text = "Billing Contact Information";
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
            this.billingContactPersonControl.Size = new System.Drawing.Size(355, 336);
            this.billingContactPersonControl.TabIndex = 1;
            // 
            // labelSameAsPickup
            // 
            this.labelSameAsPickup.AutoSize = true;
            this.labelSameAsPickup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSameAsPickup.Location = new System.Drawing.Point(8, 353);
            this.labelSameAsPickup.Name = "labelSameAsPickup";
            this.labelSameAsPickup.Size = new System.Drawing.Size(95, 13);
            this.labelSameAsPickup.TabIndex = 2;
            this.labelSameAsPickup.Text = "Same as Pickup";
            // 
            // labelPickupQuestion
            // 
            this.labelPickupQuestion.AutoSize = true;
            this.labelPickupQuestion.Location = new System.Drawing.Point(33, 371);
            this.labelPickupQuestion.Name = "labelPickupQuestion";
            this.labelPickupQuestion.Size = new System.Drawing.Size(203, 13);
            this.labelPickupQuestion.TabIndex = 3;
            this.labelPickupQuestion.Text = "Is this address also your pickup address?";
            // 
            // sameAsPickupYes
            // 
            this.sameAsPickupYes.AutoSize = true;
            this.sameAsPickupYes.Location = new System.Drawing.Point(82, 387);
            this.sameAsPickupYes.Name = "sameAsPickupYes";
            this.sameAsPickupYes.Size = new System.Drawing.Size(42, 17);
            this.sameAsPickupYes.TabIndex = 4;
            this.sameAsPickupYes.TabStop = true;
            this.sameAsPickupYes.Text = "Yes";
            this.sameAsPickupYes.UseVisualStyleBackColor = true;
            this.sameAsPickupYes.Checked = true;
            // 
            // sameAsPickupNo
            // 
            this.sameAsPickupNo.AutoSize = true;
            this.sameAsPickupNo.Location = new System.Drawing.Point(82, 410);
            this.sameAsPickupNo.Name = "sameAsPickupNo";
            this.sameAsPickupNo.Size = new System.Drawing.Size(230, 17);
            this.sameAsPickupNo.TabIndex = 5;
            this.sameAsPickupNo.TabStop = true;
            this.sameAsPickupNo.Text = "No (If not, we\'ll ask you for it in a moment)";
            this.sameAsPickupNo.UseVisualStyleBackColor = true;
            // 
            // UpsBillingContactInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sameAsPickupNo);
            this.Controls.Add(this.sameAsPickupYes);
            this.Controls.Add(this.labelPickupQuestion);
            this.Controls.Add(this.labelSameAsPickup);
            this.Controls.Add(this.billingContactPersonControl);
            this.Controls.Add(this.labelBillingContactInfoHeader);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UpsBillingContactInfoControl";
            this.Size = new System.Drawing.Size(369, 451);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelBillingContactInfoHeader;
        private Data.Controls.PersonControl billingContactPersonControl;
        private System.Windows.Forms.Label labelSameAsPickup;
        private System.Windows.Forms.Label labelPickupQuestion;
        private System.Windows.Forms.RadioButton sameAsPickupYes;
        private System.Windows.Forms.RadioButton sameAsPickupNo;
    }
}
