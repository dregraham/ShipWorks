namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    partial class UspsPaymentControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UspsPaymentControl));
            this.labelPaymentHeading = new System.Windows.Forms.Label();
            this.labelPaymentInfo = new System.Windows.Forms.Label();
            this.labelPaymentMethod = new System.Windows.Forms.Label();
            this.paymentMethod = new System.Windows.Forms.ComboBox();
            this.panelCreditCardInfo = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.labelAch = new System.Windows.Forms.Label();
            this.cardholderName = new System.Windows.Forms.TextBox();
            this.labelCardholderName = new System.Windows.Forms.Label();
            this.cardType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.creditCardExpirationYear = new System.Windows.Forms.NumericUpDown();
            this.creditCardExpirationMonth = new System.Windows.Forms.NumericUpDown();
            this.labelYear = new System.Windows.Forms.Label();
            this.labelMonth = new System.Windows.Forms.Label();
            this.creditCardNumber = new System.Windows.Forms.TextBox();
            this.labelExpirationDate = new System.Windows.Forms.Label();
            this.labelCreditCardNumber = new System.Windows.Forms.Label();
            this.panelAchAccount = new System.Windows.Forms.Panel();
            this.accountNumber = new System.Windows.Forms.TextBox();
            this.accountHolderName = new System.Windows.Forms.TextBox();
            this.labelAccountNumber = new System.Windows.Forms.Label();
            this.labelAccountHolder = new System.Windows.Forms.Label();
            this.bankName = new System.Windows.Forms.TextBox();
            this.routingNumber = new System.Windows.Forms.TextBox();
            this.labelBank = new System.Windows.Forms.Label();
            this.labelRoutingNumber = new System.Windows.Forms.Label();
            this.achAccountType = new System.Windows.Forms.ComboBox();
            this.labelAccountType = new System.Windows.Forms.Label();
            this.panelCreditCardInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.creditCardExpirationYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.creditCardExpirationMonth)).BeginInit();
            this.panelAchAccount.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPaymentHeading
            // 
            this.labelPaymentHeading.AutoSize = true;
            this.labelPaymentHeading.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPaymentHeading.Location = new System.Drawing.Point(3, 0);
            this.labelPaymentHeading.Name = "labelPaymentHeading";
            this.labelPaymentHeading.Size = new System.Drawing.Size(202, 13);
            this.labelPaymentHeading.TabIndex = 0;
            this.labelPaymentHeading.Text = "Stamps.com Payment Information";
            // 
            // labelPaymentInfo
            // 
            this.labelPaymentInfo.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelPaymentInfo.Location = new System.Drawing.Point(21, 21);
            this.labelPaymentInfo.Name = "labelPaymentInfo";
            this.labelPaymentInfo.Size = new System.Drawing.Size(377, 56);
            this.labelPaymentInfo.TabIndex = 1;
            this.labelPaymentInfo.Text = resources.GetString("labelPaymentInfo.Text");
            // 
            // labelPaymentMethod
            // 
            this.labelPaymentMethod.AutoSize = true;
            this.labelPaymentMethod.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelPaymentMethod.Location = new System.Drawing.Point(11, 482);
            this.labelPaymentMethod.Name = "labelPaymentMethod";
            this.labelPaymentMethod.Size = new System.Drawing.Size(105, 13);
            this.labelPaymentMethod.TabIndex = 2;
            this.labelPaymentMethod.Text = "Method of Payment:";
            this.labelPaymentMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelPaymentMethod.Visible = false;
            // 
            // paymentMethod
            // 
            this.paymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.paymentMethod.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.paymentMethod.FormattingEnabled = true;
            this.paymentMethod.Location = new System.Drawing.Point(120, 478);
            this.paymentMethod.Name = "paymentMethod";
            this.paymentMethod.Size = new System.Drawing.Size(148, 21);
            this.paymentMethod.TabIndex = 3;
            this.paymentMethod.Visible = false;
            this.paymentMethod.SelectedIndexChanged += new System.EventHandler(this.OnPaymentMethodChanged);
            // 
            // panelCreditCardInfo
            // 
            this.panelCreditCardInfo.Controls.Add(this.label2);
            this.panelCreditCardInfo.Controls.Add(this.labelAch);
            this.panelCreditCardInfo.Controls.Add(this.cardholderName);
            this.panelCreditCardInfo.Controls.Add(this.labelCardholderName);
            this.panelCreditCardInfo.Controls.Add(this.cardType);
            this.panelCreditCardInfo.Controls.Add(this.label1);
            this.panelCreditCardInfo.Controls.Add(this.creditCardExpirationYear);
            this.panelCreditCardInfo.Controls.Add(this.creditCardExpirationMonth);
            this.panelCreditCardInfo.Controls.Add(this.labelYear);
            this.panelCreditCardInfo.Controls.Add(this.labelMonth);
            this.panelCreditCardInfo.Controls.Add(this.creditCardNumber);
            this.panelCreditCardInfo.Controls.Add(this.labelExpirationDate);
            this.panelCreditCardInfo.Controls.Add(this.labelCreditCardNumber);
            this.panelCreditCardInfo.Location = new System.Drawing.Point(15, 80);
            this.panelCreditCardInfo.Name = "panelCreditCardInfo";
            this.panelCreditCardInfo.Size = new System.Drawing.Size(383, 207);
            this.panelCreditCardInfo.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label2.Location = new System.Drawing.Point(17, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Note:";
            // 
            // labelAch
            // 
            this.labelAch.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelAch.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelAch.Location = new System.Drawing.Point(53, 135);
            this.labelAch.Name = "labelAch";
            this.labelAch.Size = new System.Drawing.Size(326, 71);
            this.labelAch.TabIndex = 0;
            this.labelAch.Text = resources.GetString("labelAch.Text");
            // 
            // cardholderName
            // 
            this.cardholderName.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cardholderName.Location = new System.Drawing.Point(123, 31);
            this.cardholderName.Name = "cardholderName";
            this.cardholderName.Size = new System.Drawing.Size(149, 21);
            this.cardholderName.TabIndex = 4;
            // 
            // labelCardholderName
            // 
            this.labelCardholderName.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelCardholderName.Location = new System.Drawing.Point(2, 30);
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
            this.cardType.Location = new System.Drawing.Point(122, 4);
            this.cardType.Name = "cardType";
            this.cardType.Size = new System.Drawing.Size(148, 21);
            this.cardType.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label1.Location = new System.Drawing.Point(25, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Card Type:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // panelAchAccount
            // 
            this.panelAchAccount.Controls.Add(this.accountNumber);
            this.panelAchAccount.Controls.Add(this.accountHolderName);
            this.panelAchAccount.Controls.Add(this.labelAccountNumber);
            this.panelAchAccount.Controls.Add(this.labelAccountHolder);
            this.panelAchAccount.Controls.Add(this.bankName);
            this.panelAchAccount.Controls.Add(this.routingNumber);
            this.panelAchAccount.Controls.Add(this.labelBank);
            this.panelAchAccount.Controls.Add(this.labelRoutingNumber);
            this.panelAchAccount.Controls.Add(this.achAccountType);
            this.panelAchAccount.Controls.Add(this.labelAccountType);
            this.panelAchAccount.Location = new System.Drawing.Point(14, 293);
            this.panelAchAccount.Name = "panelAchAccount";
            this.panelAchAccount.Size = new System.Drawing.Size(383, 173);
            this.panelAchAccount.TabIndex = 5;
            this.panelAchAccount.Visible = false;
            // 
            // accountNumber
            // 
            this.accountNumber.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.accountNumber.Location = new System.Drawing.Point(121, 66);
            this.accountNumber.Name = "accountNumber";
            this.accountNumber.Size = new System.Drawing.Size(149, 21);
            this.accountNumber.TabIndex = 5;
            // 
            // accountHolderName
            // 
            this.accountHolderName.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.accountHolderName.Location = new System.Drawing.Point(122, 123);
            this.accountHolderName.Name = "accountHolderName";
            this.accountHolderName.Size = new System.Drawing.Size(149, 21);
            this.accountHolderName.TabIndex = 9;
            // 
            // labelAccountNumber
            // 
            this.labelAccountNumber.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelAccountNumber.Location = new System.Drawing.Point(13, 65);
            this.labelAccountNumber.Name = "labelAccountNumber";
            this.labelAccountNumber.Size = new System.Drawing.Size(102, 21);
            this.labelAccountNumber.TabIndex = 4;
            this.labelAccountNumber.Text = "Account Number:";
            this.labelAccountNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAccountHolder
            // 
            this.labelAccountHolder.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelAccountHolder.Location = new System.Drawing.Point(1, 122);
            this.labelAccountHolder.Name = "labelAccountHolder";
            this.labelAccountHolder.Size = new System.Drawing.Size(114, 21);
            this.labelAccountHolder.TabIndex = 8;
            this.labelAccountHolder.Text = "Account Holder Name:";
            this.labelAccountHolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bankName
            // 
            this.bankName.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.bankName.Location = new System.Drawing.Point(121, 38);
            this.bankName.Name = "bankName";
            this.bankName.Size = new System.Drawing.Size(149, 21);
            this.bankName.TabIndex = 3;
            // 
            // routingNumber
            // 
            this.routingNumber.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.routingNumber.Location = new System.Drawing.Point(122, 96);
            this.routingNumber.Name = "routingNumber";
            this.routingNumber.Size = new System.Drawing.Size(149, 21);
            this.routingNumber.TabIndex = 7;
            // 
            // labelBank
            // 
            this.labelBank.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelBank.Location = new System.Drawing.Point(47, 37);
            this.labelBank.Name = "labelBank";
            this.labelBank.Size = new System.Drawing.Size(68, 21);
            this.labelBank.TabIndex = 2;
            this.labelBank.Text = "Bank name:";
            this.labelBank.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelRoutingNumber
            // 
            this.labelRoutingNumber.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelRoutingNumber.Location = new System.Drawing.Point(20, 95);
            this.labelRoutingNumber.Name = "labelRoutingNumber";
            this.labelRoutingNumber.Size = new System.Drawing.Size(95, 21);
            this.labelRoutingNumber.TabIndex = 6;
            this.labelRoutingNumber.Text = "Routing Number:";
            this.labelRoutingNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // achAccountType
            // 
            this.achAccountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.achAccountType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.achAccountType.FormattingEnabled = true;
            this.achAccountType.Location = new System.Drawing.Point(121, 9);
            this.achAccountType.Name = "achAccountType";
            this.achAccountType.Size = new System.Drawing.Size(149, 21);
            this.achAccountType.TabIndex = 1;
            // 
            // labelAccountType
            // 
            this.labelAccountType.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelAccountType.Location = new System.Drawing.Point(31, 9);
            this.labelAccountType.Name = "labelAccountType";
            this.labelAccountType.Size = new System.Drawing.Size(84, 21);
            this.labelAccountType.TabIndex = 0;
            this.labelAccountType.Text = "Account Type:";
            this.labelAccountType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StampsPaymentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelAchAccount);
            this.Controls.Add(this.paymentMethod);
            this.Controls.Add(this.labelPaymentMethod);
            this.Controls.Add(this.labelPaymentInfo);
            this.Controls.Add(this.labelPaymentHeading);
            this.Controls.Add(this.panelCreditCardInfo);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "StampsPaymentControl";
            this.Size = new System.Drawing.Size(401, 532);
            this.Load += new System.EventHandler(this.OnLoad);
            this.panelCreditCardInfo.ResumeLayout(false);
            this.panelCreditCardInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.creditCardExpirationYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.creditCardExpirationMonth)).EndInit();
            this.panelAchAccount.ResumeLayout(false);
            this.panelAchAccount.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPaymentHeading;
        private System.Windows.Forms.Label labelPaymentInfo;
        private System.Windows.Forms.Label labelPaymentMethod;
        private System.Windows.Forms.ComboBox paymentMethod;
        private System.Windows.Forms.Panel panelCreditCardInfo;
        private System.Windows.Forms.Label labelYear;
        private System.Windows.Forms.Label labelMonth;
        private System.Windows.Forms.TextBox creditCardNumber;
        private System.Windows.Forms.Label labelExpirationDate;
        private System.Windows.Forms.Label labelCreditCardNumber;
        private System.Windows.Forms.NumericUpDown creditCardExpirationMonth;
        private System.Windows.Forms.NumericUpDown creditCardExpirationYear;
        private System.Windows.Forms.Panel panelAchAccount;
        private System.Windows.Forms.Label labelAccountType;
        private System.Windows.Forms.ComboBox achAccountType;
        private System.Windows.Forms.TextBox bankName;
        private System.Windows.Forms.Label labelBank;
        private System.Windows.Forms.TextBox accountNumber;
        private System.Windows.Forms.Label labelAccountNumber;
        private System.Windows.Forms.TextBox routingNumber;
        private System.Windows.Forms.Label labelRoutingNumber;
        private System.Windows.Forms.TextBox accountHolderName;
        private System.Windows.Forms.Label labelAccountHolder;
        private System.Windows.Forms.ComboBox cardType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox cardholderName;
        private System.Windows.Forms.Label labelCardholderName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelAch;
    }
}
