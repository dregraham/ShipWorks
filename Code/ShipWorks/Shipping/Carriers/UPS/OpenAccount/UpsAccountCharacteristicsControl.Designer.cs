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
            this.labelAccountTypeHeader.Size = new System.Drawing.Size(127, 13);
            this.labelAccountTypeHeader.TabIndex = 0;
            this.labelAccountTypeHeader.Text = "Business Information";
            // 
            // primaryReason
            // 
            this.primaryReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.primaryReason.FormattingEnabled = true;
            this.primaryReason.Location = new System.Drawing.Point(162, 16);
            this.primaryReason.Name = "primaryReason";
            this.primaryReason.Size = new System.Drawing.Size(208, 21);
            this.primaryReason.TabIndex = 0;
            this.primaryReason.SelectedIndexChanged += new System.EventHandler(this.OnPrimaryReasonChanged);
            // 
            // labelPrimaryReason
            // 
            this.labelPrimaryReason.AutoSize = true;
            this.labelPrimaryReason.Location = new System.Drawing.Point(6, 19);
            this.labelPrimaryReason.Name = "labelPrimaryReason";
            this.labelPrimaryReason.Size = new System.Drawing.Size(150, 13);
            this.labelPrimaryReason.TabIndex = 3;
            this.labelPrimaryReason.Text = "Reason for Creating Account:";
            // 
            // carrierType
            // 
            this.carrierType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.carrierType.FormattingEnabled = true;
            this.carrierType.Location = new System.Drawing.Point(426, 16);
            this.carrierType.Name = "carrierType";
            this.carrierType.Size = new System.Drawing.Size(78, 21);
            this.carrierType.TabIndex = 1;
            // 
            // labelCarrierType
            // 
            this.labelCarrierType.AutoSize = true;
            this.labelCarrierType.Location = new System.Drawing.Point(376, 19);
            this.labelCarrierType.Name = "labelCarrierType";
            this.labelCarrierType.Size = new System.Drawing.Size(44, 13);
            this.labelCarrierType.TabIndex = 5;
            this.labelCarrierType.Text = "Carrier:";
            // 
            // UpsAccountCharacteristicsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.carrierType);
            this.Controls.Add(this.labelCarrierType);
            this.Controls.Add(this.primaryReason);
            this.Controls.Add(this.labelPrimaryReason);
            this.Controls.Add(this.labelAccountTypeHeader);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UpsAccountCharacteristicsControl";
            this.Size = new System.Drawing.Size(546, 40);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAccountTypeHeader;
        private System.Windows.Forms.ComboBox primaryReason;
        private System.Windows.Forms.Label labelPrimaryReason;
        private System.Windows.Forms.ComboBox carrierType;
        private System.Windows.Forms.Label labelCarrierType;
    }
}
