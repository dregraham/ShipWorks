namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    partial class ChannelAdvisorStoreSettingsControl
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
            this.ordersLabel = new System.Windows.Forms.Label();
            this.criteriaComboBox = new System.Windows.Forms.ComboBox();
            this.sectionHeader = new ShipWorks.UI.Controls.SectionTitle();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // ordersLabel
            // 
            this.ordersLabel.AutoSize = true;
            this.ordersLabel.Location = new System.Drawing.Point(9, 7);
            this.ordersLabel.Name = "ordersLabel";
            this.ordersLabel.Size = new System.Drawing.Size(193, 13);
            this.ordersLabel.TabIndex = 1;
            this.ordersLabel.Text = "Download orders which are marked as:";
            // 
            // criteriaComboBox
            // 
            this.criteriaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.criteriaComboBox.FormattingEnabled = true;
            this.criteriaComboBox.Location = new System.Drawing.Point(10, 26);
            this.criteriaComboBox.Name = "criteriaComboBox";
            this.criteriaComboBox.Size = new System.Drawing.Size(294, 21);
            this.criteriaComboBox.TabIndex = 2;
            // 
            // sectionHeader
            // 
            this.sectionHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.sectionHeader.Location = new System.Drawing.Point(0, 0);
            this.sectionHeader.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.sectionHeader.Name = "sectionHeader";
            this.sectionHeader.Size = new System.Drawing.Size(440, 22);
            this.sectionHeader.TabIndex = 13;
            this.sectionHeader.Text = "Order Downloads";
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.ordersLabel);
            this.panelContent.Controls.Add(this.criteriaComboBox);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelContent.Location = new System.Drawing.Point(0, 22);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(440, 52);
            this.panelContent.TabIndex = 14;
            // 
            // ChannelAdvisorStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.sectionHeader);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "ChannelAdvisorStoreSettingsControl";
            this.Size = new System.Drawing.Size(440, 82);
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ordersLabel;
        private System.Windows.Forms.ComboBox criteriaComboBox;
        private ShipWorks.UI.Controls.SectionTitle sectionHeader;
        private System.Windows.Forms.Panel panelContent;
    }
}
