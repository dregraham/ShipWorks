namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    partial class PaymentAndBillingAddressControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaymentAndBillingAddressControl));
            this.creditCardPanel = new System.Windows.Forms.Panel();
            this.cardholderName = new System.Windows.Forms.TextBox();
            this.labelCardholderName = new System.Windows.Forms.Label();
            this.cardType = new System.Windows.Forms.ComboBox();
            this.labelCardType = new System.Windows.Forms.Label();
            this.creditCardExpirationYear = new System.Windows.Forms.NumericUpDown();
            this.creditCardExpirationMonth = new System.Windows.Forms.NumericUpDown();
            this.labelYear = new System.Windows.Forms.Label();
            this.labelMonth = new System.Windows.Forms.Label();
            this.creditCardNumber = new System.Windows.Forms.TextBox();
            this.labelExpirationDate = new System.Windows.Forms.Label();
            this.labelCreditCardNumber = new System.Windows.Forms.Label();
            this.labelNote = new System.Windows.Forms.Label();
            this.labelAch = new System.Windows.Forms.Label();
            this.labelPaymentInfo = new System.Windows.Forms.Label();
            this.labelPaymentHeading = new System.Windows.Forms.Label();
            this.labelBillingAddress = new System.Windows.Forms.Label();
            this.billingAddress = new ShipWorks.Data.Controls.AutofillPersonControl();
            this.creditCardPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.creditCardExpirationYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.creditCardExpirationMonth)).BeginInit();
            this.SuspendLayout();
            // 
            // creditCardPanel
            // 
            this.creditCardPanel.Controls.Add(this.cardholderName);
            this.creditCardPanel.Controls.Add(this.labelCardholderName);
            this.creditCardPanel.Controls.Add(this.cardType);
            this.creditCardPanel.Controls.Add(this.labelCardType);
            this.creditCardPanel.Controls.Add(this.creditCardExpirationYear);
            this.creditCardPanel.Controls.Add(this.creditCardExpirationMonth);
            this.creditCardPanel.Controls.Add(this.labelYear);
            this.creditCardPanel.Controls.Add(this.labelMonth);
            this.creditCardPanel.Controls.Add(this.creditCardNumber);
            this.creditCardPanel.Controls.Add(this.labelExpirationDate);
            this.creditCardPanel.Controls.Add(this.labelCreditCardNumber);
            this.creditCardPanel.Location = new System.Drawing.Point(14, 64);
            this.creditCardPanel.Name = "creditCardPanel";
            this.creditCardPanel.Size = new System.Drawing.Size(383, 123);
            this.creditCardPanel.TabIndex = 5;
            // 
            // cardholderName
            // 
            this.cardholderName.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cardholderName.Location = new System.Drawing.Point(123, 31);
            this.cardholderName.Name = "cardholderName";
            this.cardholderName.Size = new System.Drawing.Size(148, 21);
            this.cardholderName.TabIndex = 4;
            // 
            // labelCardholderName
            // 
            this.labelCardholderName.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelCardholderName.Location = new System.Drawing.Point(5, 30);
            this.labelCardholderName.Name = "labelCardholderName";
            this.labelCardholderName.Size = new System.Drawing.Size(114, 21);
            this.labelCardholderName.TabIndex = 3;
            this.labelCardholderName.Text = "Cardholder Name:";
            this.labelCardholderName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cardType
            // 
            this.cardType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cardType.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cardType.FormattingEnabled = true;
            this.cardType.Location = new System.Drawing.Point(123, 4);
            this.cardType.Name = "cardType";
            this.cardType.Size = new System.Drawing.Size(148, 21);
            this.cardType.TabIndex = 2;
            // 
            // labelCardType
            // 
            this.labelCardType.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelCardType.Location = new System.Drawing.Point(28, 5);
            this.labelCardType.Name = "labelCardType";
            this.labelCardType.Size = new System.Drawing.Size(91, 17);
            this.labelCardType.TabIndex = 1;
            this.labelCardType.Text = "Card Type:";
            this.labelCardType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // creditCardExpirationYear
            // 
            this.creditCardExpirationYear.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.creditCardExpirationYear.Location = new System.Drawing.Point(161, 85);
            this.creditCardExpirationYear.Name = "creditCardExpirationYear";
            this.creditCardExpirationYear.Size = new System.Drawing.Size(45, 21);
            this.creditCardExpirationYear.TabIndex = 9;
            this.creditCardExpirationYear.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // creditCardExpirationMonth
            // 
            this.creditCardExpirationMonth.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.creditCardExpirationMonth.Location = new System.Drawing.Point(123, 85);
            this.creditCardExpirationMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.creditCardExpirationMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.creditCardExpirationMonth.Name = "creditCardExpirationMonth";
            this.creditCardExpirationMonth.Size = new System.Drawing.Size(32, 21);
            this.creditCardExpirationMonth.TabIndex = 8;
            this.creditCardExpirationMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelYear
            // 
            this.labelYear.AutoSize = true;
            this.labelYear.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelYear.Location = new System.Drawing.Point(163, 108);
            this.labelYear.Name = "labelYear";
            this.labelYear.Size = new System.Drawing.Size(29, 13);
            this.labelYear.TabIndex = 11;
            this.labelYear.Text = "Year";
            this.labelYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMonth
            // 
            this.labelMonth.AutoSize = true;
            this.labelMonth.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelMonth.Location = new System.Drawing.Point(120, 108);
            this.labelMonth.Name = "labelMonth";
            this.labelMonth.Size = new System.Drawing.Size(37, 13);
            this.labelMonth.TabIndex = 10;
            this.labelMonth.Text = "Month";
            this.labelMonth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // creditCardNumber
            // 
            this.creditCardNumber.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.creditCardNumber.Location = new System.Drawing.Point(123, 58);
            this.creditCardNumber.Name = "creditCardNumber";
            this.creditCardNumber.Size = new System.Drawing.Size(148, 21);
            this.creditCardNumber.TabIndex = 6;
            // 
            // labelExpirationDate
            // 
            this.labelExpirationDate.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelExpirationDate.Location = new System.Drawing.Point(28, 85);
            this.labelExpirationDate.Name = "labelExpirationDate";
            this.labelExpirationDate.Size = new System.Drawing.Size(91, 19);
            this.labelExpirationDate.TabIndex = 7;
            this.labelExpirationDate.Text = "Expiration Date:";
            this.labelExpirationDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCreditCardNumber
            // 
            this.labelCreditCardNumber.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelCreditCardNumber.Location = new System.Drawing.Point(4, 59);
            this.labelCreditCardNumber.Name = "labelCreditCardNumber";
            this.labelCreditCardNumber.Size = new System.Drawing.Size(115, 17);
            this.labelCreditCardNumber.TabIndex = 5;
            this.labelCreditCardNumber.Text = "Credit Card Number:";
            this.labelCreditCardNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelNote
            // 
            this.labelNote.AutoSize = true;
            this.labelNote.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelNote.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelNote.Location = new System.Drawing.Point(21, 395);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(34, 13);
            this.labelNote.TabIndex = 12;
            this.labelNote.Text = "Note:";
            // 
            // labelAch
            // 
            this.labelAch.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelAch.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelAch.Location = new System.Drawing.Point(61, 395);
            this.labelAch.Name = "labelAch";
            this.labelAch.Size = new System.Drawing.Size(380, 71);
            this.labelAch.TabIndex = 0;
            this.labelAch.Text = resources.GetString("labelAch.Text");
            // 
            // labelPaymentInfo
            // 
            this.labelPaymentInfo.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelPaymentInfo.Location = new System.Drawing.Point(3, 23);
            this.labelPaymentInfo.Name = "labelPaymentInfo";
            this.labelPaymentInfo.Size = new System.Drawing.Size(447, 28);
            this.labelPaymentInfo.TabIndex = 7;
            this.labelPaymentInfo.Text = "Stamps.com requires a payment method for buying postage. The payment information " +
    "entered here will not be used by Interapptive for billing of any ShipWorks relat" +
    "ed services.";
            // 
            // labelPaymentHeading
            // 
            this.labelPaymentHeading.AutoSize = true;
            this.labelPaymentHeading.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPaymentHeading.Location = new System.Drawing.Point(3, 0);
            this.labelPaymentHeading.Name = "labelPaymentHeading";
            this.labelPaymentHeading.Size = new System.Drawing.Size(202, 13);
            this.labelPaymentHeading.TabIndex = 6;
            this.labelPaymentHeading.Text = "Stamps.com Payment Information";
            // 
            // labelBillingAddress
            // 
            this.labelBillingAddress.AutoSize = true;
            this.labelBillingAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBillingAddress.Location = new System.Drawing.Point(38, 199);
            this.labelBillingAddress.Name = "labelBillingAddress";
            this.labelBillingAddress.Size = new System.Drawing.Size(90, 13);
            this.labelBillingAddress.TabIndex = 45;
            this.labelBillingAddress.Text = "Billing Address";
            // 
            // billingAddress
            // 
            this.billingAddress.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)((((ShipWorks.Data.Controls.PersonFields.Street | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal)));
            this.billingAddress.Location = new System.Drawing.Point(61, 215);
            this.billingAddress.MaxStreetLines = 1;
            this.billingAddress.Name = "billingAddress";
            this.billingAddress.Size = new System.Drawing.Size(346, 165);
            this.billingAddress.TabIndex = 46;
            // 
            // PaymentAndBillingAddressControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.billingAddress);
            this.Controls.Add(this.labelBillingAddress);
            this.Controls.Add(this.labelNote);
            this.Controls.Add(this.labelPaymentInfo);
            this.Controls.Add(this.labelAch);
            this.Controls.Add(this.labelPaymentHeading);
            this.Controls.Add(this.creditCardPanel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "PaymentAndBillingAddressControl";
            this.Size = new System.Drawing.Size(450, 500);
            this.Load += new System.EventHandler(this.OnLoad);
            this.creditCardPanel.ResumeLayout(false);
            this.creditCardPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.creditCardExpirationYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.creditCardExpirationMonth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel creditCardPanel;
        private System.Windows.Forms.Label labelNote;
        private System.Windows.Forms.Label labelAch;
        private System.Windows.Forms.TextBox cardholderName;
        private System.Windows.Forms.Label labelCardholderName;
        private System.Windows.Forms.ComboBox cardType;
        private System.Windows.Forms.Label labelCardType;
        private System.Windows.Forms.NumericUpDown creditCardExpirationYear;
        private System.Windows.Forms.NumericUpDown creditCardExpirationMonth;
        private System.Windows.Forms.Label labelYear;
        private System.Windows.Forms.Label labelMonth;
        private System.Windows.Forms.TextBox creditCardNumber;
        private System.Windows.Forms.Label labelExpirationDate;
        private System.Windows.Forms.Label labelCreditCardNumber;
        private System.Windows.Forms.Label labelPaymentInfo;
        private System.Windows.Forms.Label labelPaymentHeading;
        private System.Windows.Forms.Label labelBillingAddress;
        private Data.Controls.AutofillPersonControl billingAddress;
    }
}
