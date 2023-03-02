namespace ShipWorks.Stores.Platforms.Shopify
{
    partial class ShopifyStoreSettingsControl
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
            this.sectionTitle1 = new ShipWorks.UI.Controls.SectionTitle();
            this.shopifyNotifyCustomer = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // sectionTitle1
            // 
            this.sectionTitle1.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitle1.Location = new System.Drawing.Point(0, 0);
            this.sectionTitle1.Name = "sectionTitle1";
            this.sectionTitle1.Size = new System.Drawing.Size(564, 23);
            this.sectionTitle1.TabIndex = 0;
            this.sectionTitle1.Text = "Shopify Store Settings";
            // 
            // shopifyNotifyCustomer
            // 
            this.shopifyNotifyCustomer.AutoSize = true;
            this.shopifyNotifyCustomer.Location = new System.Drawing.Point(20, 41);
            this.shopifyNotifyCustomer.Name = "shopifyNotifyCustomer";
            this.shopifyNotifyCustomer.Size = new System.Drawing.Size(316, 17);
            this.shopifyNotifyCustomer.TabIndex = 22;
            this.shopifyNotifyCustomer.Text = "Shopify should notify the customer when an order is shipped.";
            this.shopifyNotifyCustomer.UseVisualStyleBackColor = true;
            // 
            // ShopifyStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.shopifyNotifyCustomer);
            this.Controls.Add(this.sectionTitle1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ShopifyStoreSettingsControl";
            this.MinimumSize = new System.Drawing.Size(564, 65);
            this.Size = new System.Drawing.Size(564, 65);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.SectionTitle sectionTitle1;
        private System.Windows.Forms.CheckBox shopifyNotifyCustomer;
    }
}
