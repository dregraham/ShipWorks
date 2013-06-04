namespace ShipWorks.Stores.Platforms.Ebay.WizardPages
{
    partial class EBayPayPalPage
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.usePayPal = new System.Windows.Forms.CheckBox();
            this.paypalGroupBox = new System.Windows.Forms.GroupBox();
            this.payPalCredentials = new ShipWorks.Stores.Platforms.PayPal.PayPalCredentialsControl();
            this.paypalGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(37, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(523, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "When buyers pay through PayPal, ShipWorks can download any notes left by the buye" +
                "r and the confirmed status of the shipping address.";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(37, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(521, 35);
            this.label2.TabIndex = 1;
            this.label2.Text = "This feature is optional.  ShipWorks downloads PayPal payment status, and any cha" +
                "nges buyers make to their address, regardless of this feature being enabled.";
            // 
            // usePayPal
            // 
            this.usePayPal.AutoSize = true;
            this.usePayPal.Location = new System.Drawing.Point(42, 96);
            this.usePayPal.Name = "usePayPal";
            this.usePayPal.Size = new System.Drawing.Size(272, 17);
            this.usePayPal.TabIndex = 3;
            this.usePayPal.Text = "Download PayPal details for items paid with PayPal.";
            this.usePayPal.UseVisualStyleBackColor = true;
            this.usePayPal.CheckedChanged += new System.EventHandler(this.OnUsePayPalCheckedChanged);
            // 
            // paypalGroupBox
            // 
            this.paypalGroupBox.Controls.Add(this.payPalCredentials);
            this.paypalGroupBox.Location = new System.Drawing.Point(66, 123);
            this.paypalGroupBox.Name = "paypalGroupBox";
            this.paypalGroupBox.Size = new System.Drawing.Size(505, 174);
            this.paypalGroupBox.TabIndex = 4;
            this.paypalGroupBox.TabStop = false;
            this.paypalGroupBox.Text = "PayPal Account Access";
            // 
            // payPalCredentials
            // 
            this.payPalCredentials.Enabled = false;
            this.payPalCredentials.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.payPalCredentials.Location = new System.Drawing.Point(10, 16);
            this.payPalCredentials.MinimumSize = new System.Drawing.Size(427, 142);
            this.payPalCredentials.Name = "payPalCredentials";
            this.payPalCredentials.Size = new System.Drawing.Size(471, 142);
            this.payPalCredentials.TabIndex = 3;
            // 
            // EBayPayPalPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.paypalGroupBox);
            this.Controls.Add(this.usePayPal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Description = "Configure ShipWorks to work with PayPal";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "EBayPayPalPage";
            this.Size = new System.Drawing.Size(615, 300);
            this.Title = "PayPal Setup";
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.paypalGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox usePayPal;
        private System.Windows.Forms.GroupBox paypalGroupBox;
        private ShipWorks.Stores.Platforms.PayPal.PayPalCredentialsControl payPalCredentials;
    }
}
