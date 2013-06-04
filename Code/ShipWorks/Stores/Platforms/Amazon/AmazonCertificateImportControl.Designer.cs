namespace ShipWorks.Stores.Platforms.Amazon
{
    partial class AmazonCertificateImportControl
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.accessKey = new System.Windows.Forms.TextBox();
            this.publicCertificateFile = new System.Windows.Forms.TextBox();
            this.privateKeyFile = new System.Windows.Forms.TextBox();
            this.browseCertButton = new System.Windows.Forms.Button();
            this.browseKeyButton = new System.Windows.Forms.Button();
            this.awsLink = new ShipWorks.UI.Controls.LinkControl();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(465, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Amazon requires the use of a secure certificate to download your orders.  This is" +
                " obtained from the Amazon Web Services site at:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(42, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(410, 29);
            this.label2.TabIndex = 2;
            this.label2.Text = "1. If you receive an error while logging in, you may need to first sign-up for an " +
                "AWS account.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(238, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "2. Find \"Your Access Key ID\" and enter it below.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(421, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "3. Find the \"X.509 Certificate\" section.  Click \"Create New\" to create a new cert" +
                "ificate.";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(42, 162);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(390, 28);
            this.label5.TabIndex = 5;
            this.label5.Text = "4. Download both the Private Key and X.509 Certificate and enter the paths to tho" +
                "se files below.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 208);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Access Key ID:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 234);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "X.509 Certificate:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(44, 260);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Private Key:";
            // 
            // accessKey
            // 
            this.accessKey.Location = new System.Drawing.Point(126, 205);
            this.fieldLengthProvider.SetMaxLengthSource(this.accessKey, ShipWorks.Data.Utility.EntityFieldLengthSource.AmazonAccessKey);
            this.accessKey.Name = "accessKey";
            this.accessKey.Size = new System.Drawing.Size(187, 21);
            this.accessKey.TabIndex = 0;
            // 
            // publicCertificateFile
            // 
            this.publicCertificateFile.Location = new System.Drawing.Point(126, 231);
            this.publicCertificateFile.Name = "publicCertificateFile";
            this.publicCertificateFile.Size = new System.Drawing.Size(330, 21);
            this.publicCertificateFile.TabIndex = 1;
            // 
            // privateKeyFile
            // 
            this.privateKeyFile.Location = new System.Drawing.Point(126, 257);
            this.privateKeyFile.Name = "privateKeyFile";
            this.privateKeyFile.Size = new System.Drawing.Size(330, 21);
            this.privateKeyFile.TabIndex = 3;
            // 
            // browseCertButton
            // 
            this.browseCertButton.Location = new System.Drawing.Point(462, 229);
            this.browseCertButton.Name = "browseCertButton";
            this.browseCertButton.Size = new System.Drawing.Size(29, 23);
            this.browseCertButton.TabIndex = 2;
            this.browseCertButton.Text = "...";
            this.browseCertButton.UseVisualStyleBackColor = true;
            this.browseCertButton.Click += new System.EventHandler(this.OnBrowseCertClick);
            // 
            // browseKeyButton
            // 
            this.browseKeyButton.Location = new System.Drawing.Point(462, 255);
            this.browseKeyButton.Name = "browseKeyButton";
            this.browseKeyButton.Size = new System.Drawing.Size(29, 23);
            this.browseKeyButton.TabIndex = 4;
            this.browseKeyButton.Text = "...";
            this.browseKeyButton.UseVisualStyleBackColor = true;
            this.browseKeyButton.Click += new System.EventHandler(this.OnBrowseKeyClick);
            // 
            // awsLink
            // 
            this.awsLink.AutoSize = true;
            this.awsLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.awsLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.awsLink.ForeColor = System.Drawing.Color.Blue;
            this.awsLink.Location = new System.Drawing.Point(16, 40);
            this.awsLink.Name = "awsLink";
            this.awsLink.Size = new System.Drawing.Size(436, 13);
            this.awsLink.TabIndex = 1;
            this.awsLink.Text = "http://aws-portal.amazon.com/gp/aws/developer/account/index.html?action=access-ke" +
                "y";
            this.awsLink.Click += new System.EventHandler(this.OnAwsLinkClicked);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "PEM Files (*.pem)|*.pem";
            this.openFileDialog.Title = "Select Amazon PEM File";
            // 
            // AmazonCertificateImportControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.browseKeyButton);
            this.Controls.Add(this.browseCertButton);
            this.Controls.Add(this.privateKeyFile);
            this.Controls.Add(this.publicCertificateFile);
            this.Controls.Add(this.accessKey);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.awsLink);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "AmazonCertificateImportControl";
            this.Size = new System.Drawing.Size(516, 291);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private ShipWorks.UI.Controls.LinkControl awsLink;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox accessKey;
        private System.Windows.Forms.TextBox publicCertificateFile;
        private System.Windows.Forms.TextBox privateKeyFile;
        private System.Windows.Forms.Button browseCertButton;
        private System.Windows.Forms.Button browseKeyButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
    }
}
