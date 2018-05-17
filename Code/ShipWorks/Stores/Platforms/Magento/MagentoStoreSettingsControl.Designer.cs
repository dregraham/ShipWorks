using System.Windows.Forms;

namespace ShipWorks.Stores.Platforms.Magento
{
    partial class MagentoStoreSettingsControl
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
            this.sendMailCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // sectionHeader
            // 
            this.sectionHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionHeader.Location = new System.Drawing.Point(0, 0);
            this.sectionHeader.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.sectionHeader.Name = "sectionHeader";
            this.sectionHeader.Size = new System.Drawing.Size(1959, 22);
            this.sectionHeader.TabIndex = 14;
            this.sectionHeader.Text = "Magento Email";
            // 
            // sendMailCheckBox
            // 
            this.sendMailCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sendMailCheckBox.AutoSize = true;
            this.sendMailCheckBox.Location = new System.Drawing.Point(20, 32);
            this.sendMailCheckBox.Name = "sendMailCheckBox";
            this.sendMailCheckBox.Size = new System.Drawing.Size(390, 17);
            this.sendMailCheckBox.TabIndex = 15;
            this.sendMailCheckBox.Text = "Send customer email from Magento when an order is marked as Completed. ";
            this.sendMailCheckBox.UseVisualStyleBackColor = true;
            // 
            // MagentoStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sendMailCheckBox);
            this.Controls.Add(this.sectionHeader);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MagentoStoreSettingsControl";
            this.Size = new System.Drawing.Size(1519, 65);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.UI.Controls.SectionTitle sectionHeader;
        private System.Windows.Forms.CheckBox sendMailCheckBox;
    }
}
