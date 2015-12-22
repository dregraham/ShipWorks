namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration
{
    partial class YahooEmailStoreSettingsControl
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
            this.sectionHeader = new ShipWorks.UI.Controls.SectionTitle();
            this.importProductsControl = new YahooEmailImportProductsControl();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // sectionHeader
            // 
            this.sectionHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.sectionHeader.Location = new System.Drawing.Point(0, 0);
            this.sectionHeader.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.sectionHeader.Name = "sectionHeader";
            this.sectionHeader.Size = new System.Drawing.Size(653, 22);
            this.sectionHeader.TabIndex = 19;
            this.sectionHeader.Text = "Product Weights";
            // 
            // importProductsControl
            // 
            this.importProductsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.importProductsControl.Location = new System.Drawing.Point(6, 51);
            this.importProductsControl.Name = "importProductsControl";
            this.importProductsControl.Size = new System.Drawing.Size(513, 105);
            this.importProductsControl.TabIndex = 21;
            // 
            // labelInfo1
            // 
            this.labelInfo1.CausesValidation = false;
            this.labelInfo1.Location = new System.Drawing.Point(8, 30);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(510, 18);
            this.labelInfo1.TabIndex = 20;
            this.labelInfo1.Text = "To retrieve product weights ShipWorks needs to download the Product Catalog for y" +
                "our store.";
            // 
            // YahooStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.importProductsControl);
            this.Controls.Add(this.labelInfo1);
            this.Controls.Add(this.sectionHeader);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "YahooStoreSettingsControl";
            this.Size = new System.Drawing.Size(653, 204);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.SectionTitle sectionHeader;
        private YahooEmailImportProductsControl importProductsControl;
        private System.Windows.Forms.Label labelInfo1;
    }
}
