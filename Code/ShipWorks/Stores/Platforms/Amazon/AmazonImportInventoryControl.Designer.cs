namespace ShipWorks.Stores.Platforms.Amazon
{
    partial class AmazonImportInventoryControl
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
            this.importInventory = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // importInventory
            // 
            this.importInventory.Location = new System.Drawing.Point(0, 0);
            this.importInventory.Name = "importInventory";
            this.importInventory.Size = new System.Drawing.Size(126, 23);
            this.importInventory.TabIndex = 19;
            this.importInventory.Text = "Import Inventory...";
            this.importInventory.UseVisualStyleBackColor = true;
            this.importInventory.Click += new System.EventHandler(this.OnImportInventory);
            // 
            // AmazonImportInventoryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.importInventory);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "AmazonImportInventoryControl";
            this.Size = new System.Drawing.Size(129, 24);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button importInventory;
    }
}
