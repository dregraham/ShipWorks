namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    partial class UpsCreateAccount
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
            this.createUPSAccount = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(457, 47);
            this.label1.TabIndex = 0;
            this.label1.Text = "All the information required to create a new UPS account has been collected. To c" +
    "reate your UPS account, click the button below.";
            // 
            // createUPSAccount
            // 
            this.createUPSAccount.BackColor = System.Drawing.SystemColors.Control;
            this.createUPSAccount.Location = new System.Drawing.Point(308, 50);
            this.createUPSAccount.Name = "createUPSAccount";
            this.createUPSAccount.Size = new System.Drawing.Size(152, 23);
            this.createUPSAccount.TabIndex = 1;
            this.createUPSAccount.Text = "Create UPS Account";
            this.createUPSAccount.UseVisualStyleBackColor = false;
            this.createUPSAccount.Click += new System.EventHandler(this.ClickCreateUpsAccount);
            // 
            // UpsCreateAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.createUPSAccount);
            this.Controls.Add(this.label1);
            this.Name = "UpsCreateAccount";
            this.Size = new System.Drawing.Size(500, 109);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button createUPSAccount;
    }
}
