namespace ShipWorks.Shipping.Insurance
{
    partial class InsuranceSelectionControl
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
            this.useInsurance = new System.Windows.Forms.CheckBox();
            this.labelInsurance = new System.Windows.Forms.Label();
            this.infoTip = new ShipWorks.UI.Controls.InfoTip();
            this.linkSavings = new System.Windows.Forms.Label();
            this.labelCost = new System.Windows.Forms.Label();
            this.insuredValue = new ShipWorks.UI.Controls.MoneyTextBox();
            this.labelValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // useInsurance
            // 
            this.useInsurance.AutoSize = true;
            this.useInsurance.BackColor = System.Drawing.Color.Transparent;
            this.useInsurance.Location = new System.Drawing.Point(69, 1);
            this.useInsurance.Name = "useInsurance";
            this.useInsurance.Size = new System.Drawing.Size(127, 17);
            this.useInsurance.TabIndex = 5;
            this.useInsurance.Text = "ShipWorks Insurance";
            this.useInsurance.UseVisualStyleBackColor = false;
            this.useInsurance.CheckedChanged += new System.EventHandler(this.OnChangeUseInsurance);
            // 
            // labelInsurance
            // 
            this.labelInsurance.AutoSize = true;
            this.labelInsurance.BackColor = System.Drawing.Color.Transparent;
            this.labelInsurance.Location = new System.Drawing.Point(4, 1);
            this.labelInsurance.Name = "labelInsurance";
            this.labelInsurance.Size = new System.Drawing.Size(59, 13);
            this.labelInsurance.TabIndex = 4;
            this.labelInsurance.Text = "Insurance:";
            // 
            // infoTip
            // 
            this.infoTip.Location = new System.Drawing.Point(236, 27);
            this.infoTip.Name = "infoTip";
            this.infoTip.Size = new System.Drawing.Size(12, 12);
            this.infoTip.TabIndex = 22;
            this.infoTip.Title = "ShipWorks Insurance";
            // 
            // linkSavings
            // 
            this.linkSavings.AutoSize = true;
            this.linkSavings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkSavings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.linkSavings.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkSavings.Location = new System.Drawing.Point(292, 26);
            this.linkSavings.Name = "linkSavings";
            this.linkSavings.Size = new System.Drawing.Size(90, 13);
            this.linkSavings.TabIndex = 21;
            this.linkSavings.Text = "(You save $1.80)";
            this.linkSavings.Click += new System.EventHandler(this.OnClickSave);
            // 
            // labelCost
            // 
            this.labelCost.AutoSize = true;
            this.labelCost.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelCost.ForeColor = System.Drawing.Color.Green;
            this.labelCost.Location = new System.Drawing.Point(252, 26);
            this.labelCost.Name = "labelCost";
            this.labelCost.Size = new System.Drawing.Size(38, 13);
            this.labelCost.TabIndex = 20;
            this.labelCost.Text = "$0.45";
            // 
            // insuredValue
            // 
            this.insuredValue.Amount = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.insuredValue.Location = new System.Drawing.Point(148, 22);
            this.insuredValue.Name = "insuredValue";
            this.insuredValue.Size = new System.Drawing.Size(84, 21);
            this.insuredValue.TabIndex = 19;
            this.insuredValue.Text = "$0.00";
            this.insuredValue.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(66, 25);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(77, 13);
            this.labelValue.TabIndex = 18;
            this.labelValue.Text = "Insured value:";
            // 
            // InsuranceSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.infoTip);
            this.Controls.Add(this.linkSavings);
            this.Controls.Add(this.labelCost);
            this.Controls.Add(this.insuredValue);
            this.Controls.Add(this.labelValue);
            this.Controls.Add(this.useInsurance);
            this.Controls.Add(this.labelInsurance);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "InsuranceSelectionControl";
            this.Size = new System.Drawing.Size(465, 46);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox useInsurance;
        private System.Windows.Forms.Label labelInsurance;
        private UI.Controls.InfoTip infoTip;
        private System.Windows.Forms.Label linkSavings;
        private System.Windows.Forms.Label labelCost;
        private UI.Controls.MoneyTextBox insuredValue;
        private System.Windows.Forms.Label labelValue;
    }
}
