namespace ShipWorks.Stores.Platforms.Shopify
{
    partial class ShopifyCreateTokenControl
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
            this.labelStatus = new System.Windows.Forms.Label();
            this.imageStatus = new System.Windows.Forms.PictureBox();
            this.createToken = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize) (this.imageStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.ForeColor = System.Drawing.Color.DimGray;
            this.labelStatus.Location = new System.Drawing.Point(166, 8);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(279, 13);
            this.labelStatus.TabIndex = 23;
            this.labelStatus.Text = "Authenticated to connect to mybigcoolstore.shopify.com";
            this.labelStatus.Visible = false;
            // 
            // imageStatus
            // 
            this.imageStatus.Image = global::ShipWorks.Properties.Resources.check16;
            this.imageStatus.Location = new System.Drawing.Point(149, 6);
            this.imageStatus.Name = "imageStatus";
            this.imageStatus.Size = new System.Drawing.Size(16, 16);
            this.imageStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageStatus.TabIndex = 22;
            this.imageStatus.TabStop = false;
            this.imageStatus.Visible = false;
            // 
            // createToken
            // 
            this.createToken.Location = new System.Drawing.Point(3, 3);
            this.createToken.Name = "createToken";
            this.createToken.Size = new System.Drawing.Size(137, 23);
            this.createToken.TabIndex = 1;
            this.createToken.Text = "Create Login Token...";
            this.createToken.UseVisualStyleBackColor = true;
            this.createToken.Click += new System.EventHandler(this.OnCreateToken);
            // 
            // ShopifyCreateTokenControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.imageStatus);
            this.Controls.Add(this.createToken);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ShopifyCreateTokenControl";
            this.Size = new System.Drawing.Size(455, 32);
            ((System.ComponentModel.ISupportInitialize) (this.imageStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.PictureBox imageStatus;
        private System.Windows.Forms.Button createToken;

        //private System.Windows.Forms.Label tokenStatus;
    }
}
