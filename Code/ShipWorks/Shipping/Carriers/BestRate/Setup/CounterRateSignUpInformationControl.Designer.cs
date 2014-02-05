namespace ShipWorks.Shipping.Carriers.BestRate.Setup
{
    partial class CounterRateSignUpInformationControl
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
            this.heading = new System.Windows.Forms.Label();
            this.rateFoundDescription = new System.Windows.Forms.Label();
            this.carrierLogo = new System.Windows.Forms.PictureBox();
            this.carrierName = new System.Windows.Forms.Label();
            this.rateAmount = new System.Windows.Forms.Label();
            this.carrierAccountDescription = new System.Windows.Forms.Label();
            this.signUpButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.carrierLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // heading
            // 
            this.heading.AutoSize = true;
            this.heading.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.heading.Location = new System.Drawing.Point(-3, 0);
            this.heading.Name = "heading";
            this.heading.Size = new System.Drawing.Size(155, 13);
            this.heading.TabIndex = 0;
            this.heading.Text = "Create a Shipping Account";
            // 
            // rateFoundDescription
            // 
            this.rateFoundDescription.AutoSize = true;
            this.rateFoundDescription.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rateFoundDescription.Location = new System.Drawing.Point(14, 23);
            this.rateFoundDescription.Name = "rateFoundDescription";
            this.rateFoundDescription.Size = new System.Drawing.Size(258, 13);
            this.rateFoundDescription.TabIndex = 1;
            this.rateFoundDescription.Text = "The best rate ShipWorks found for your shipment is:";
            // 
            // carrierLogo
            // 
            this.carrierLogo.Image = global::ShipWorks.Properties.Resources.box_closed16;
            this.carrierLogo.Location = new System.Drawing.Point(27, 45);
            this.carrierLogo.Name = "carrierLogo";
            this.carrierLogo.Size = new System.Drawing.Size(24, 24);
            this.carrierLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.carrierLogo.TabIndex = 2;
            this.carrierLogo.TabStop = false;
            // 
            // carrierName
            // 
            this.carrierName.AutoSize = true;
            this.carrierName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.carrierName.Location = new System.Drawing.Point(57, 51);
            this.carrierName.Name = "carrierName";
            this.carrierName.Size = new System.Drawing.Size(107, 13);
            this.carrierName.TabIndex = 3;
            this.carrierName.Text = "USPS Priority Mail";
            // 
            // rateAmount
            // 
            this.rateAmount.AutoSize = true;
            this.rateAmount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.rateAmount.ForeColor = System.Drawing.Color.Green;
            this.rateAmount.Location = new System.Drawing.Point(57, 65);
            this.rateAmount.Name = "rateAmount";
            this.rateAmount.Size = new System.Drawing.Size(38, 13);
            this.rateAmount.TabIndex = 4;
            this.rateAmount.Text = "$5.45";
            // 
            // carrierAccountDescription
            // 
            this.carrierAccountDescription.Location = new System.Drawing.Point(14, 90);
            this.carrierAccountDescription.Name = "carrierAccountDescription";
            this.carrierAccountDescription.Size = new System.Drawing.Size(421, 50);
            this.carrierAccountDescription.TabIndex = 5;
            this.carrierAccountDescription.Text = "USPS partners with Express1 to enable printing USPS shipping labels directly from" +
    " your printer. To continue you\'ll need an account with Express1. There is no mon" +
    "thly fee for the account.";
            // 
            // signUpButton
            // 
            this.signUpButton.AutoSize = true;
            this.signUpButton.Location = new System.Drawing.Point(27, 143);
            this.signUpButton.Name = "signUpButton";
            this.signUpButton.Size = new System.Drawing.Size(165, 28);
            this.signUpButton.TabIndex = 6;
            this.signUpButton.Text = "Continue and sign up >";
            this.signUpButton.UseVisualStyleBackColor = true;
            // 
            // CounterRateSignUpInformationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.signUpButton);
            this.Controls.Add(this.carrierAccountDescription);
            this.Controls.Add(this.rateAmount);
            this.Controls.Add(this.carrierName);
            this.Controls.Add(this.carrierLogo);
            this.Controls.Add(this.rateFoundDescription);
            this.Controls.Add(this.heading);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "CounterRateSignUpInformationControl";
            this.Size = new System.Drawing.Size(463, 176);
            ((System.ComponentModel.ISupportInitialize)(this.carrierLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label heading;
        private System.Windows.Forms.Label rateFoundDescription;
        private System.Windows.Forms.PictureBox carrierLogo;
        private System.Windows.Forms.Label carrierName;
        private System.Windows.Forms.Label rateAmount;
        private System.Windows.Forms.Label carrierAccountDescription;
        private System.Windows.Forms.Button signUpButton;
    }
}
