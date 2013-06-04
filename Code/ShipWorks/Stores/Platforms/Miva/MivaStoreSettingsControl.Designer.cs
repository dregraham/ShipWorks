namespace ShipWorks.Stores.Platforms.Miva
{
    partial class MivaStoreSettingsControl
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
            this.sebenzaOptions = new ShipWorks.Stores.Platforms.Miva.MivaSebenzaOptionsControl();
            this.sectionHeader = new ShipWorks.UI.Controls.SectionTitle();
            this.internationalTitle = new ShipWorks.UI.Controls.SectionTitle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.encodingComboBox = new System.Windows.Forms.ComboBox();
            this.orderStatusSection = new ShipWorks.UI.Controls.SectionTitle();
            this.orderStatusControl = new ShipWorks.Stores.Platforms.Miva.MivaOrderStatusControl();
            this.SuspendLayout();
            // 
            // sebenzaOptions
            // 
            this.sebenzaOptions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sebenzaOptions.Location = new System.Drawing.Point(16, 142);
            this.sebenzaOptions.Name = "sebenzaOptions";
            this.sebenzaOptions.Size = new System.Drawing.Size(425, 21);
            this.sebenzaOptions.TabIndex = 0;
            // 
            // sectionHeader
            // 
            this.sectionHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionHeader.Location = new System.Drawing.Point(0, 114);
            this.sectionHeader.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.sectionHeader.Name = "sectionHeader";
            this.sectionHeader.Size = new System.Drawing.Size(435, 22);
            this.sectionHeader.TabIndex = 18;
            this.sectionHeader.Text = "Sebenza Modules";
            // 
            // internationalTitle
            // 
            this.internationalTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.internationalTitle.Location = new System.Drawing.Point(1, 184);
            this.internationalTitle.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.internationalTitle.Name = "internationalTitle";
            this.internationalTitle.Size = new System.Drawing.Size(435, 22);
            this.internationalTitle.TabIndex = 19;
            this.internationalTitle.Text = "International Characters";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 216);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(409, 28);
            this.label1.TabIndex = 20;
            this.label1.Text = "If international characters are not being downloaded correctly into ShipWorks, ch" +
                "ange the Encoding below to another value.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 259);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Module Encoding:";
            // 
            // encodingComboBox
            // 
            this.encodingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encodingComboBox.FormattingEnabled = true;
            this.encodingComboBox.Location = new System.Drawing.Point(131, 256);
            this.encodingComboBox.Name = "encodingComboBox";
            this.encodingComboBox.Size = new System.Drawing.Size(206, 21);
            this.encodingComboBox.TabIndex = 22;
            // 
            // orderStatusSection
            // 
            this.orderStatusSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.orderStatusSection.Location = new System.Drawing.Point(0, 0);
            this.orderStatusSection.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.orderStatusSection.Name = "orderStatusSection";
            this.orderStatusSection.Size = new System.Drawing.Size(435, 22);
            this.orderStatusSection.TabIndex = 20;
            this.orderStatusSection.Text = "Online Order Status";
            // 
            // orderStatusControl
            // 
            this.orderStatusControl.Location = new System.Drawing.Point(3, 28);
            this.orderStatusControl.Name = "orderStatusControl";
            this.orderStatusControl.Size = new System.Drawing.Size(404, 78);
            this.orderStatusControl.TabIndex = 23;
            // 
            // MivaStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.orderStatusControl);
            this.Controls.Add(this.orderStatusSection);
            this.Controls.Add(this.encodingComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.internationalTitle);
            this.Controls.Add(this.sectionHeader);
            this.Controls.Add(this.sebenzaOptions);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MivaStoreSettingsControl";
            this.Size = new System.Drawing.Size(435, 299);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MivaSebenzaOptionsControl sebenzaOptions;
        private ShipWorks.UI.Controls.SectionTitle sectionHeader;
        private UI.Controls.SectionTitle internationalTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox encodingComboBox;
        private UI.Controls.SectionTitle orderStatusSection;
        private MivaOrderStatusControl orderStatusControl;
    }
}
