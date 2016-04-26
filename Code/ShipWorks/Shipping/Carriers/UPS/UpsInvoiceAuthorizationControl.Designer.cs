namespace ShipWorks.Shipping.Carriers.UPS
{
    partial class UpsInvoiceAuthorizationControl
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
            this.authInvoiceAmount = new ShipWorks.UI.Controls.MoneyTextBox();
            this.authInvoiceDate = new System.Windows.Forms.DateTimePicker();
            this.authControlID = new System.Windows.Forms.TextBox();
            this.authInvoiceNumber = new System.Windows.Forms.TextBox();
            this.labelAuthControlID = new System.Windows.Forms.Label();
            this.labelAuthInvoiceAmount = new System.Windows.Forms.Label();
            this.labelAuthInvoiceDate = new System.Windows.Forms.Label();
            this.labelAuthInvoiceNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // authInvoiceAmount
            // 
            this.authInvoiceAmount.Amount = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.authInvoiceAmount.IgnoreSet = false;
            this.authInvoiceAmount.Location = new System.Drawing.Point(95, 57);
            this.authInvoiceAmount.Name = "authInvoiceAmount";
            this.authInvoiceAmount.Size = new System.Drawing.Size(96, 20);
            this.authInvoiceAmount.TabIndex = 7;
            this.authInvoiceAmount.Text = "$0.00";
            // 
            // authInvoiceDate
            // 
            this.authInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.authInvoiceDate.Location = new System.Drawing.Point(95, 30);
            this.authInvoiceDate.Name = "authInvoiceDate";
            this.authInvoiceDate.Size = new System.Drawing.Size(96, 20);
            this.authInvoiceDate.TabIndex = 5;
            // 
            // authControlID
            // 
            this.authControlID.Location = new System.Drawing.Point(95, 84);
            this.authControlID.Name = "authControlID";
            this.authControlID.Size = new System.Drawing.Size(167, 20);
            this.authControlID.TabIndex = 9;
            // 
            // authInvoiceNumber
            // 
            this.authInvoiceNumber.Location = new System.Drawing.Point(95, 3);
            this.authInvoiceNumber.Name = "authInvoiceNumber";
            this.authInvoiceNumber.Size = new System.Drawing.Size(167, 20);
            this.authInvoiceNumber.TabIndex = 3;
            // 
            // labelAuthControlID
            // 
            this.labelAuthControlID.AutoSize = true;
            this.labelAuthControlID.Location = new System.Drawing.Point(29, 87);
            this.labelAuthControlID.Name = "labelAuthControlID";
            this.labelAuthControlID.Size = new System.Drawing.Size(57, 13);
            this.labelAuthControlID.TabIndex = 8;
            this.labelAuthControlID.Text = "Control ID:";
            // 
            // labelAuthInvoiceAmount
            // 
            this.labelAuthInvoiceAmount.AutoSize = true;
            this.labelAuthInvoiceAmount.Location = new System.Drawing.Point(4, 60);
            this.labelAuthInvoiceAmount.Name = "labelAuthInvoiceAmount";
            this.labelAuthInvoiceAmount.Size = new System.Drawing.Size(83, 13);
            this.labelAuthInvoiceAmount.TabIndex = 6;
            this.labelAuthInvoiceAmount.Text = "Invoice amount:";
            // 
            // labelAuthInvoiceDate
            // 
            this.labelAuthInvoiceDate.AutoSize = true;
            this.labelAuthInvoiceDate.Location = new System.Drawing.Point(18, 32);
            this.labelAuthInvoiceDate.Name = "labelAuthInvoiceDate";
            this.labelAuthInvoiceDate.Size = new System.Drawing.Size(69, 13);
            this.labelAuthInvoiceDate.TabIndex = 4;
            this.labelAuthInvoiceDate.Text = "Invoice date:";
            // 
            // labelAuthInvoiceNumber
            // 
            this.labelAuthInvoiceNumber.AutoSize = true;
            this.labelAuthInvoiceNumber.Location = new System.Drawing.Point(4, 6);
            this.labelAuthInvoiceNumber.Name = "labelAuthInvoiceNumber";
            this.labelAuthInvoiceNumber.Size = new System.Drawing.Size(83, 13);
            this.labelAuthInvoiceNumber.TabIndex = 2;
            this.labelAuthInvoiceNumber.Text = "Invoice number:";
            // 
            // UpsInvoiceAuthorizationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.authInvoiceAmount);
            this.Controls.Add(this.authInvoiceDate);
            this.Controls.Add(this.authControlID);
            this.Controls.Add(this.authInvoiceNumber);
            this.Controls.Add(this.labelAuthControlID);
            this.Controls.Add(this.labelAuthInvoiceAmount);
            this.Controls.Add(this.labelAuthInvoiceDate);
            this.Controls.Add(this.labelAuthInvoiceNumber);
            this.Name = "UpsInvoiceAuthorizationControl";
            this.Size = new System.Drawing.Size(357, 135);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.MoneyTextBox authInvoiceAmount;
        private System.Windows.Forms.DateTimePicker authInvoiceDate;
        private System.Windows.Forms.TextBox authControlID;
        private System.Windows.Forms.TextBox authInvoiceNumber;
        private System.Windows.Forms.Label labelAuthControlID;
        private System.Windows.Forms.Label labelAuthInvoiceAmount;
        private System.Windows.Forms.Label labelAuthInvoiceDate;
        private System.Windows.Forms.Label labelAuthInvoiceNumber;

    }
}
