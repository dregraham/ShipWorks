using System.Drawing;

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
            this.requestedShippingOptions = new System.Windows.Forms.ComboBox();
            this.labelWeightUnitOfMeasure = new System.Windows.Forms.Label();
            this.shopifyNotifyCustomer = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // sectionTitle1
            // 
            this.sectionTitle1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitle1.Location = new System.Drawing.Point(0, 0);
            this.sectionTitle1.Name = "sectionTitle1";
            this.sectionTitle1.Size = new System.Drawing.Size(489, 23);
            this.sectionTitle1.TabIndex = 0;
            this.sectionTitle1.Text = "Shopify Store Settings";
            // 
            // requestedShippingOptions
            // 
            this.requestedShippingOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.requestedShippingOptions.FormattingEnabled = true;
            this.requestedShippingOptions.Location = new System.Drawing.Point(221, 38);
            this.requestedShippingOptions.Name = "requestedShippingOptions";
            this.requestedShippingOptions.Size = new System.Drawing.Size(135, 21);
            this.requestedShippingOptions.TabIndex = 23;
            // 
            // labelWeightUnitOfMeasure
            // 
            this.labelWeightUnitOfMeasure.AutoSize = true;
            this.labelWeightUnitOfMeasure.Location = new System.Drawing.Point(17, 41);
            this.labelWeightUnitOfMeasure.Name = "labelWeightUnitOfMeasure";
            this.labelWeightUnitOfMeasure.Size = new System.Drawing.Size(201, 13);
            this.labelWeightUnitOfMeasure.TabIndex = 22;
            this.labelWeightUnitOfMeasure.Text = "Requested shipping should be based on:";
            // 
            // shopifyNotifyCustomer
            // 
            this.shopifyNotifyCustomer.AutoSize = true;
            this.shopifyNotifyCustomer.Location = new System.Drawing.Point(20, 69);
            this.shopifyNotifyCustomer.Name = "shopifyNotifyCustomer";
            this.shopifyNotifyCustomer.Size = new System.Drawing.Size(316, 17);
            this.shopifyNotifyCustomer.TabIndex = 24;
            this.shopifyNotifyCustomer.Text = "Shopify should notify the customer when an order is shipped";
            this.shopifyNotifyCustomer.UseVisualStyleBackColor = true;
            // 
            // ShopifyStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.shopifyNotifyCustomer);
            this.Controls.Add(this.requestedShippingOptions);
            this.Controls.Add(this.labelWeightUnitOfMeasure);
            this.Controls.Add(this.sectionTitle1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ShopifyStoreSettingsControl";
            this.Size = new System.Drawing.Size(489, 96);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.SectionTitle sectionTitle1;
        private System.Windows.Forms.ComboBox requestedShippingOptions;
        private System.Windows.Forms.Label labelWeightUnitOfMeasure;
        private System.Windows.Forms.CheckBox shopifyNotifyCustomer;
    }
}
