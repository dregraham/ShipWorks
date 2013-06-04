namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    partial class UpsAccountCharacteristicsControl
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
            this.labelAccountTypeHeader = new System.Windows.Forms.Label();
            this.labelCustomerClassification = new System.Windows.Forms.Label();
            this.customerClassification = new System.Windows.Forms.ComboBox();
            this.primaryReason = new System.Windows.Forms.ComboBox();
            this.labelPrimaryReason = new System.Windows.Forms.Label();
            this.carrierType = new System.Windows.Forms.ComboBox();
            this.labelCarrierType = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelAccountTypeHeader
            // 
            this.labelAccountTypeHeader.AutoSize = true;
            this.labelAccountTypeHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccountTypeHeader.Location = new System.Drawing.Point(3, 0);
            this.labelAccountTypeHeader.Name = "labelAccountTypeHeader";
            this.labelAccountTypeHeader.Size = new System.Drawing.Size(141, 13);
            this.labelAccountTypeHeader.TabIndex = 0;
            this.labelAccountTypeHeader.Text = "Account Characteristics";
            // 
            // labelCustomerClassification
            // 
            this.labelCustomerClassification.AutoSize = true;
            this.labelCustomerClassification.Location = new System.Drawing.Point(22, 23);
            this.labelCustomerClassification.Name = "labelCustomerClassification";
            this.labelCustomerClassification.Size = new System.Drawing.Size(122, 13);
            this.labelCustomerClassification.TabIndex = 1;
            this.labelCustomerClassification.Text = "Customer Classification:";
            // 
            // customerClassification
            // 
            this.customerClassification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.customerClassification.FormattingEnabled = true;
            this.customerClassification.Location = new System.Drawing.Point(150, 20);
            this.customerClassification.Name = "customerClassification";
            this.customerClassification.Size = new System.Drawing.Size(208, 21);
            this.customerClassification.TabIndex = 2;
            // 
            // primaryReason
            // 
            this.primaryReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.primaryReason.FormattingEnabled = true;
            this.primaryReason.Location = new System.Drawing.Point(150, 47);
            this.primaryReason.Name = "primaryReason";
            this.primaryReason.Size = new System.Drawing.Size(208, 21);
            this.primaryReason.TabIndex = 4;
            this.primaryReason.SelectedIndexChanged += new System.EventHandler(this.OnPrimaryReasonChanged);
            // 
            // labelPrimaryReason
            // 
            this.labelPrimaryReason.AutoSize = true;
            this.labelPrimaryReason.Location = new System.Drawing.Point(58, 50);
            this.labelPrimaryReason.Name = "labelPrimaryReason";
            this.labelPrimaryReason.Size = new System.Drawing.Size(86, 13);
            this.labelPrimaryReason.TabIndex = 3;
            this.labelPrimaryReason.Text = "Primary Reason:";
            // 
            // carrierType
            // 
            this.carrierType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.carrierType.FormattingEnabled = true;
            this.carrierType.Location = new System.Drawing.Point(150, 74);
            this.carrierType.Name = "carrierType";
            this.carrierType.Size = new System.Drawing.Size(208, 21);
            this.carrierType.TabIndex = 6;
            this.carrierType.Visible = false;
            // 
            // labelCarrierType
            // 
            this.labelCarrierType.AutoSize = true;
            this.labelCarrierType.Location = new System.Drawing.Point(73, 77);
            this.labelCarrierType.Name = "labelCarrierType";
            this.labelCarrierType.Size = new System.Drawing.Size(71, 13);
            this.labelCarrierType.TabIndex = 5;
            this.labelCarrierType.Text = "Carrier Type:";
            this.labelCarrierType.Visible = false;
            // 
            // UpsAccountCharacteristicsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.carrierType);
            this.Controls.Add(this.labelCarrierType);
            this.Controls.Add(this.primaryReason);
            this.Controls.Add(this.labelPrimaryReason);
            this.Controls.Add(this.customerClassification);
            this.Controls.Add(this.labelCustomerClassification);
            this.Controls.Add(this.labelAccountTypeHeader);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UpsAccountCharacteristicsControl";
            this.Size = new System.Drawing.Size(369, 129);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAccountTypeHeader;
        private System.Windows.Forms.Label labelCustomerClassification;
        private System.Windows.Forms.ComboBox customerClassification;
        private System.Windows.Forms.ComboBox primaryReason;
        private System.Windows.Forms.Label labelPrimaryReason;
        private System.Windows.Forms.ComboBox carrierType;
        private System.Windows.Forms.Label labelCarrierType;
    }
}
