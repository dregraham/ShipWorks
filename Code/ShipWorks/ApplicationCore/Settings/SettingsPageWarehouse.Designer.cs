namespace ShipWorks.ApplicationCore.Settings
{
    partial class SettingsPageWarehouse
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
            this.sectionTitleDisplay = new ShipWorks.UI.Controls.SectionTitle();
            this.selectWarehouseButton = new System.Windows.Forms.Button();
            this.selectedWarehouseName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // sectionTitleDisplay
            // 
            this.sectionTitleDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitleDisplay.Location = new System.Drawing.Point(10, 10);
            this.sectionTitleDisplay.Name = "sectionTitleDisplay";
            this.sectionTitleDisplay.Size = new System.Drawing.Size(530, 22);
            this.sectionTitleDisplay.TabIndex = 41;
            this.sectionTitleDisplay.Text = "Warehouse Selection";
            // 
            // selectWarehouseButton
            // 
            this.selectWarehouseButton.Location = new System.Drawing.Point(399, 38);
            this.selectWarehouseButton.Name = "selectWarehouseButton";
            this.selectWarehouseButton.Size = new System.Drawing.Size(141, 23);
            this.selectWarehouseButton.TabIndex = 42;
            this.selectWarehouseButton.Text = "Select Warehouse";
            this.selectWarehouseButton.UseVisualStyleBackColor = true;
            this.selectWarehouseButton.Click += new System.EventHandler(this.OnSelectWarehouse);
            // 
            // selectedWarehouseName
            // 
            this.selectedWarehouseName.AutoSize = true;
            this.selectedWarehouseName.Location = new System.Drawing.Point(22, 43);
            this.selectedWarehouseName.Name = "selectedWarehouseName";
            this.selectedWarehouseName.Size = new System.Drawing.Size(119, 13);
            this.selectedWarehouseName.TabIndex = 43;
            this.selectedWarehouseName.Text = "No warehouse selected";
            // 
            // SettingsPageWarehouse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.selectedWarehouseName);
            this.Controls.Add(this.selectWarehouseButton);
            this.Controls.Add(this.sectionTitleDisplay);
            this.Name = "SettingsPageWarehouse";
            this.Size = new System.Drawing.Size(550, 300);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private UI.Controls.SectionTitle sectionTitleDisplay;
        private System.Windows.Forms.Button selectWarehouseButton;
        private System.Windows.Forms.Label selectedWarehouseName;
    }
}
