namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    partial class ThreeDCartAccountSettingsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThreeDCartAccountSettingsControl));
            this.apiUserKey = new System.Windows.Forms.TextBox();
            this.lblSecretKey = new System.Windows.Forms.Label();
            this.storeUrl = new System.Windows.Forms.TextBox();
            this.lblStoreUrl = new System.Windows.Forms.Label();
            this.lblEnterCredentials = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // apiUserKey
            // 
            this.apiUserKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.apiUserKey.Location = new System.Drawing.Point(97, 77);
            this.apiUserKey.Name = "apiUserKey";
            this.apiUserKey.Size = new System.Drawing.Size(292, 21);
            this.apiUserKey.TabIndex = 19;
            // 
            // lblSecretKey
            // 
            this.lblSecretKey.AutoSize = true;
            this.lblSecretKey.Location = new System.Drawing.Point(19, 80);
            this.lblSecretKey.Name = "lblSecretKey";
            this.lblSecretKey.Size = new System.Drawing.Size(74, 13);
            this.lblSecretKey.TabIndex = 18;
            this.lblSecretKey.Text = "API User Key:";
            this.lblSecretKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // storeUrl
            // 
            this.storeUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.storeUrl.Location = new System.Drawing.Point(97, 31);
            this.storeUrl.Name = "storeUrl";
            this.storeUrl.Size = new System.Drawing.Size(292, 21);
            this.storeUrl.TabIndex = 17;
            // 
            // lblStoreUrl
            // 
            this.lblStoreUrl.AutoSize = true;
            this.lblStoreUrl.Location = new System.Drawing.Point(40, 34);
            this.lblStoreUrl.Name = "lblStoreUrl";
            this.lblStoreUrl.Size = new System.Drawing.Size(53, 13);
            this.lblStoreUrl.TabIndex = 16;
            this.lblStoreUrl.Text = "Store Url:";
            this.lblStoreUrl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEnterCredentials
            // 
            this.lblEnterCredentials.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEnterCredentials.AutoSize = true;
            this.lblEnterCredentials.Location = new System.Drawing.Point(3, 6);
            this.lblEnterCredentials.Name = "lblEnterCredentials";
            this.lblEnterCredentials.Size = new System.Drawing.Size(347, 13);
            this.lblEnterCredentials.TabIndex = 15;
            this.lblEnterCredentials.Text = "Enter your Store Url and the API User Key provided to you by 3D Cart:";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.ForeColor = System.Drawing.Color.DimGray;
            this.label7.Location = new System.Drawing.Point(96, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(292, 72);
            this.label7.TabIndex = 20;
            this.label7.Text = resources.GetString("label7.Text");
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(95, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(292, 17);
            this.label1.TabIndex = 21;
            this.label1.Text = "For example: http://MyStore.3dcartstores.com";
            // 
            // ThreeDCartAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.apiUserKey);
            this.Controls.Add(this.lblSecretKey);
            this.Controls.Add(this.storeUrl);
            this.Controls.Add(this.lblStoreUrl);
            this.Controls.Add(this.lblEnterCredentials);
            this.Name = "ThreeDCartAccountSettingsControl";
            this.Size = new System.Drawing.Size(398, 183);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox apiUserKey;
        private System.Windows.Forms.Label lblSecretKey;
        private System.Windows.Forms.TextBox storeUrl;
        private System.Windows.Forms.Label lblStoreUrl;
        private System.Windows.Forms.Label lblEnterCredentials;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;





    }
}
