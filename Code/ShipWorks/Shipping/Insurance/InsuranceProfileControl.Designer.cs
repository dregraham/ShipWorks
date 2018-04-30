namespace ShipWorks.Shipping.Insurance
{
    partial class InsuranceProfileControl
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
            this.labelInsuredValue = new System.Windows.Forms.Label();
            this.useInsurance = new System.Windows.Forms.CheckBox();
            this.source = new System.Windows.Forms.ComboBox();
            this.amount = new ShipWorks.UI.Controls.MoneyTextBox();
            this.SuspendLayout();
            // 
            // labelInsuredValue
            // 
            this.labelInsuredValue.AutoSize = true;
            this.labelInsuredValue.Location = new System.Drawing.Point(1, 26);
            this.labelInsuredValue.Name = "labelInsuredValue";
            this.labelInsuredValue.Size = new System.Drawing.Size(88, 13);
            this.labelInsuredValue.TabIndex = 1;
            this.labelInsuredValue.Text = "Insurance value:";
            // 
            // useInsurance
            // 
            this.useInsurance.AutoSize = true;
            this.useInsurance.Location = new System.Drawing.Point(3, 3);
            this.useInsurance.Name = "useInsurance";
            this.useInsurance.Size = new System.Drawing.Size(127, 17);
            this.useInsurance.TabIndex = 0;
            this.useInsurance.Text = "ShipWorks Insurance";
            this.useInsurance.UseVisualStyleBackColor = true;
            // 
            // source
            // 
            this.source.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.source.FormattingEnabled = true;
            this.source.Items.AddRange(new object[] {
            "Order Total",
            "Item Subtotal",
            "Other Amount"});
            this.source.Location = new System.Drawing.Point(95, 23);
            this.source.Name = "source";
            this.source.Size = new System.Drawing.Size(106, 21);
            this.source.TabIndex = 2;
            this.source.SelectedIndexChanged += new System.EventHandler(this.OnChangeSource);
            // 
            // amount
            // 
            this.amount.Amount = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.amount.Location = new System.Drawing.Point(207, 23);
            this.amount.Name = "amount";
            this.amount.Size = new System.Drawing.Size(81, 21);
            this.amount.TabIndex = 3;
            this.amount.Text = "$0.00";
            // 
            // InsuranceProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.source);
            this.Controls.Add(this.amount);
            this.Controls.Add(this.labelInsuredValue);
            this.Controls.Add(this.useInsurance);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "InsuranceProfileControl";
            this.Size = new System.Drawing.Size(316, 52);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelInsuredValue;
        private System.Windows.Forms.CheckBox useInsurance;
        private System.Windows.Forms.ComboBox source;
        private UI.Controls.MoneyTextBox amount;
    }
}
