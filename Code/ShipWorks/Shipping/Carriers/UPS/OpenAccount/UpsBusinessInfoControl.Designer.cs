namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    partial class UpsBusinessInfoControl
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
            this.labelCustomerClassification = new System.Windows.Forms.Label();
            this.industry = new System.Windows.Forms.ComboBox();
            this.numberOfEmployees = new System.Windows.Forms.ComboBox();
            this.labelNumberOfEmployees = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelCustomerClassification
            // 
            this.labelCustomerClassification.AutoSize = true;
            this.labelCustomerClassification.Location = new System.Drawing.Point(104, 3);
            this.labelCustomerClassification.Name = "labelCustomerClassification";
            this.labelCustomerClassification.Size = new System.Drawing.Size(52, 13);
            this.labelCustomerClassification.TabIndex = 1;
            this.labelCustomerClassification.Text = "Industry:";
            // 
            // industry
            // 
            this.industry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.industry.FormattingEnabled = true;
            this.industry.Location = new System.Drawing.Point(162, 0);
            this.industry.Name = "industry";
            this.industry.Size = new System.Drawing.Size(208, 21);
            this.industry.TabIndex = 2;
            this.industry.SelectedIndexChanged += new System.EventHandler(this.OnIndustryChanged);
            // 
            // numberOfEmployees
            // 
            this.numberOfEmployees.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.numberOfEmployees.FormattingEnabled = true;
            this.numberOfEmployees.Location = new System.Drawing.Point(162, 27);
            this.numberOfEmployees.Name = "numberOfEmployees";
            this.numberOfEmployees.Size = new System.Drawing.Size(208, 21);
            this.numberOfEmployees.TabIndex = 4;
            // 
            // labelNumberOfEmployees
            // 
            this.labelNumberOfEmployees.AutoSize = true;
            this.labelNumberOfEmployees.Location = new System.Drawing.Point(41, 30);
            this.labelNumberOfEmployees.Name = "labelNumberOfEmployees";
            this.labelNumberOfEmployees.Size = new System.Drawing.Size(115, 13);
            this.labelNumberOfEmployees.TabIndex = 3;
            this.labelNumberOfEmployees.Text = "Number of Employees:";
            // 
            // UpsBusinessInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numberOfEmployees);
            this.Controls.Add(this.labelNumberOfEmployees);
            this.Controls.Add(this.industry);
            this.Controls.Add(this.labelCustomerClassification);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UpsBusinessInfoControl";
            this.Size = new System.Drawing.Size(402, 54);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCustomerClassification;
        private System.Windows.Forms.ComboBox industry;
        private System.Windows.Forms.ComboBox numberOfEmployees;
        private System.Windows.Forms.Label labelNumberOfEmployees;
    }
}
