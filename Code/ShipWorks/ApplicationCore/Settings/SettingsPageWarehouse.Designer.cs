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
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.SuspendLayout();
            // 
            // sectionTitleDisplay
            // 
            this.sectionTitleDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitleDisplay.Location = new System.Drawing.Point(15, 15);
            this.sectionTitleDisplay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sectionTitleDisplay.Name = "sectionTitleDisplay";
            this.sectionTitleDisplay.Size = new System.Drawing.Size(392, 34);
            this.sectionTitleDisplay.TabIndex = 41;
            this.sectionTitleDisplay.Text = "Warehouse Selection";
            // 
            // selectWarehouseButton
            // 
            this.selectWarehouseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectWarehouseButton.Location = new System.Drawing.Point(195, 58);
            this.selectWarehouseButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectWarehouseButton.Name = "selectWarehouseButton";
            this.selectWarehouseButton.Size = new System.Drawing.Size(212, 35);
            this.selectWarehouseButton.TabIndex = 42;
            this.selectWarehouseButton.Text = "Select Warehouse";
            this.selectWarehouseButton.UseVisualStyleBackColor = true;
            this.selectWarehouseButton.Click += new System.EventHandler(this.OnSelectWarehouse);
            // 
            // selectedWarehouseName
            // 
            this.selectedWarehouseName.AutoSize = true;
            this.selectedWarehouseName.Location = new System.Drawing.Point(33, 66);
            this.selectedWarehouseName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.selectedWarehouseName.Name = "selectedWarehouseName";
            this.selectedWarehouseName.Size = new System.Drawing.Size(175, 20);
            this.selectedWarehouseName.TabIndex = 43;
            this.selectedWarehouseName.Text = "No warehouse selected";
            // 
            // elementHost1
            // 
            this.elementHost1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.elementHost1.Location = new System.Drawing.Point(15, 133);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(392, 208);
            this.elementHost1.TabIndex = 44;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = null;
            // 
            // SettingsPageWarehouse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.elementHost1);
            this.Controls.Add(this.selectedWarehouseName);
            this.Controls.Add(this.selectWarehouseButton);
            this.Controls.Add(this.sectionTitleDisplay);
            this.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Name = "SettingsPageWarehouse";
            this.Size = new System.Drawing.Size(422, 360);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private UI.Controls.SectionTitle sectionTitleDisplay;
        private System.Windows.Forms.Button selectWarehouseButton;
        private System.Windows.Forms.Label selectedWarehouseName;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
    }
}
