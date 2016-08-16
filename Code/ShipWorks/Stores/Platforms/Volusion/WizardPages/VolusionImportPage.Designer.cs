namespace ShipWorks.Stores.Platforms.Volusion.WizardPages
{
    partial class VolusionImportPage
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
            this.labelInfo = new System.Windows.Forms.Label();
            this.labelBlank = new System.Windows.Forms.Label();
            this.labelHelpShipping = new System.Windows.Forms.Label();
            this.linkHelpShipping = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.labelShippingFile = new System.Windows.Forms.Label();
            this.importShipping = new System.Windows.Forms.Button();
            this.openShippingFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.labelImport = new System.Windows.Forms.Label();
            this.linkHelpPayments = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.labelHelpPayments = new System.Windows.Forms.Label();
            this.importPayments = new System.Windows.Forms.Button();
            this.labelPaymentsFile = new System.Windows.Forms.Label();
            this.openPaymentsFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.shippingSuccess = new System.Windows.Forms.PictureBox();
            this.paymentSuccess = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize) (this.shippingSuccess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.paymentSuccess)).BeginInit();
            this.SuspendLayout();
            // 
            // labelInfo
            // 
            this.labelInfo.Location = new System.Drawing.Point(18, 31);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(507, 26);
            this.labelInfo.TabIndex = 1;
            this.labelInfo.Text = "ShipWorks needs the lists of shipping and payment methods from your Volusion stor" +
                "e.  Download CSV exports from your store\'s Admin Area and import the files below" +
                ".";
            // 
            // labelBlank
            // 
            this.labelBlank.AutoSize = true;
            this.labelBlank.Location = new System.Drawing.Point(18, 70);
            this.labelBlank.Name = "labelBlank";
            this.labelBlank.Size = new System.Drawing.Size(316, 13);
            this.labelBlank.TabIndex = 2;
            this.labelBlank.Text = "You can do this later from the ShipWorks Store Settings window.";
            // 
            // labelHelpShipping
            // 
            this.labelHelpShipping.AutoSize = true;
            this.labelHelpShipping.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelHelpShipping.Location = new System.Drawing.Point(180, 124);
            this.labelHelpShipping.Name = "labelHelpShipping";
            this.labelHelpShipping.Size = new System.Drawing.Size(251, 13);
            this.labelHelpShipping.TabIndex = 6;
            this.labelHelpShipping.Text = "for help with creating and downloading the export.";
            // 
            // linkHelpShipping
            // 
            this.linkHelpShipping.AutoSize = true;
            this.linkHelpShipping.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkHelpShipping.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkHelpShipping.ForeColor = System.Drawing.Color.RoyalBlue;
            this.linkHelpShipping.Location = new System.Drawing.Point(131, 123);
            this.linkHelpShipping.Name = "linkHelpShipping";
            this.linkHelpShipping.Size = new System.Drawing.Size(53, 13);
            this.linkHelpShipping.TabIndex = 5;
            this.linkHelpShipping.Text = "Click here";
            this.linkHelpShipping.Url = "http://support.shipworks.com/support/solutions/articles/129346-importing-shipping-methods-and-payment-methods-for-a-volusion-store";
            // 
            // labelShippingFile
            // 
            this.labelShippingFile.AutoSize = true;
            this.labelShippingFile.Location = new System.Drawing.Point(18, 99);
            this.labelShippingFile.Name = "labelShippingFile";
            this.labelShippingFile.Size = new System.Drawing.Size(177, 13);
            this.labelShippingFile.TabIndex = 3;
            this.labelShippingFile.Text = "Shipping methods file (CSV format):";
            // 
            // importShipping
            // 
            this.importShipping.Location = new System.Drawing.Point(52, 118);
            this.importShipping.Name = "importShipping";
            this.importShipping.Size = new System.Drawing.Size(75, 23);
            this.importShipping.TabIndex = 4;
            this.importShipping.Text = "Import...";
            this.importShipping.UseVisualStyleBackColor = true;
            this.importShipping.Click += new System.EventHandler(this.OnImportShippingMethods);
            // 
            // openShippingFileDlg
            // 
            this.openShippingFileDlg.Filter = "Volusion CSV Export files|*.csv|All files|*.*";
            this.openShippingFileDlg.Title = "Import Shipping Methods";
            // 
            // labelImport
            // 
            this.labelImport.AutoSize = true;
            this.labelImport.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelImport.Location = new System.Drawing.Point(17, 9);
            this.labelImport.Name = "labelImport";
            this.labelImport.Size = new System.Drawing.Size(228, 13);
            this.labelImport.TabIndex = 0;
            this.labelImport.Text = "Import Shipping and Payment Methods";
            // 
            // linkHelpPayments
            // 
            this.linkHelpPayments.AutoSize = true;
            this.linkHelpPayments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkHelpPayments.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkHelpPayments.ForeColor = System.Drawing.Color.RoyalBlue;
            this.linkHelpPayments.Location = new System.Drawing.Point(131, 182);
            this.linkHelpPayments.Name = "linkHelpPayments";
            this.linkHelpPayments.Size = new System.Drawing.Size(53, 13);
            this.linkHelpPayments.TabIndex = 9;
            this.linkHelpPayments.Text = "Click here";
            this.linkHelpPayments.Url = "http://support.shipworks.com/support/solutions/articles/129346-importing-shipping-methods-and-payment-methods-for-a-volusion-store";
            // 
            // labelHelpPayments
            // 
            this.labelHelpPayments.AutoSize = true;
            this.labelHelpPayments.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelHelpPayments.Location = new System.Drawing.Point(180, 182);
            this.labelHelpPayments.Name = "labelHelpPayments";
            this.labelHelpPayments.Size = new System.Drawing.Size(251, 13);
            this.labelHelpPayments.TabIndex = 10;
            this.labelHelpPayments.Text = "for help with creating and downloading the export.";
            // 
            // importPayments
            // 
            this.importPayments.Location = new System.Drawing.Point(52, 177);
            this.importPayments.Name = "importPayments";
            this.importPayments.Size = new System.Drawing.Size(75, 23);
            this.importPayments.TabIndex = 8;
            this.importPayments.Text = "Import...";
            this.importPayments.UseVisualStyleBackColor = true;
            this.importPayments.Click += new System.EventHandler(this.OnImportPayments);
            // 
            // labelPaymentsFile
            // 
            this.labelPaymentsFile.AutoSize = true;
            this.labelPaymentsFile.Location = new System.Drawing.Point(18, 159);
            this.labelPaymentsFile.Name = "labelPaymentsFile";
            this.labelPaymentsFile.Size = new System.Drawing.Size(179, 13);
            this.labelPaymentsFile.TabIndex = 7;
            this.labelPaymentsFile.Text = "Payment methods file (CSV format):";
            // 
            // openPaymentsFileDlg
            // 
            this.openPaymentsFileDlg.Filter = "Volusion CSV Export files|*.csv|All files|*.*";
            this.openPaymentsFileDlg.Title = "Import Payment Methods";
            // 
            // shippingSuccess
            // 
            this.shippingSuccess.Image = global::ShipWorks.Properties.Resources.check16;
            this.shippingSuccess.Location = new System.Drawing.Point(30, 121);
            this.shippingSuccess.Name = "shippingSuccess";
            this.shippingSuccess.Size = new System.Drawing.Size(16, 16);
            this.shippingSuccess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.shippingSuccess.TabIndex = 21;
            this.shippingSuccess.TabStop = false;
            this.shippingSuccess.Visible = false;
            // 
            // paymentSuccess
            // 
            this.paymentSuccess.Image = global::ShipWorks.Properties.Resources.check16;
            this.paymentSuccess.Location = new System.Drawing.Point(30, 179);
            this.paymentSuccess.Name = "paymentSuccess";
            this.paymentSuccess.Size = new System.Drawing.Size(16, 16);
            this.paymentSuccess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.paymentSuccess.TabIndex = 22;
            this.paymentSuccess.TabStop = false;
            this.paymentSuccess.Visible = false;
            // 
            // VolusionImportPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.paymentSuccess);
            this.Controls.Add(this.shippingSuccess);
            this.Controls.Add(this.linkHelpPayments);
            this.Controls.Add(this.labelHelpPayments);
            this.Controls.Add(this.importPayments);
            this.Controls.Add(this.labelPaymentsFile);
            this.Controls.Add(this.labelImport);
            this.Controls.Add(this.importShipping);
            this.Controls.Add(this.labelShippingFile);
            this.Controls.Add(this.linkHelpShipping);
            this.Controls.Add(this.labelHelpShipping);
            this.Controls.Add(this.labelBlank);
            this.Controls.Add(this.labelInfo);
            this.Name = "VolusionImportPage";
            this.Size = new System.Drawing.Size(544, 292);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            ((System.ComponentModel.ISupportInitialize) (this.shippingSuccess)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.paymentSuccess)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label labelBlank;
        private System.Windows.Forms.Label labelHelpShipping;
        private ShipWorks.ApplicationCore.Interaction.HelpLink linkHelpShipping;
        private System.Windows.Forms.Label labelShippingFile;
        private System.Windows.Forms.Button importShipping;
        private System.Windows.Forms.OpenFileDialog openShippingFileDlg;
        private System.Windows.Forms.Label labelImport;
        private ShipWorks.ApplicationCore.Interaction.HelpLink linkHelpPayments;
        private System.Windows.Forms.Label labelHelpPayments;
        private System.Windows.Forms.Button importPayments;
        private System.Windows.Forms.Label labelPaymentsFile;
        private System.Windows.Forms.OpenFileDialog openPaymentsFileDlg;
        private System.Windows.Forms.PictureBox shippingSuccess;
        private System.Windows.Forms.PictureBox paymentSuccess;
    }
}
