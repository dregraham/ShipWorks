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
            this.sectionTitle1 = new ShipWorks.UI.Controls.SectionTitle();
            this.downloadAddendumCheckoutQuestions = new System.Windows.Forms.CheckBox();
            this.infoTipAddendumCheckoutQuestions = new ShipWorks.UI.Controls.InfoTip();
            this.SuspendLayout();
            // 
            // sebenzaOptions
            // 
            this.sebenzaOptions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sebenzaOptions.Location = new System.Drawing.Point(17, 132);
            this.sebenzaOptions.Name = "sebenzaOptions";
            this.sebenzaOptions.Size = new System.Drawing.Size(410, 21);
            this.sebenzaOptions.TabIndex = 0;
            // 
            // sectionHeader
            // 
            this.sectionHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionHeader.Location = new System.Drawing.Point(0, 104);
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
            this.internationalTitle.Location = new System.Drawing.Point(0, 214);
            this.internationalTitle.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.internationalTitle.Name = "internationalTitle";
            this.internationalTitle.Size = new System.Drawing.Size(435, 22);
            this.internationalTitle.TabIndex = 19;
            this.internationalTitle.Text = "International Characters";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 244);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(409, 28);
            this.label1.TabIndex = 20;
            this.label1.Text = "If international characters are not being downloaded correctly into ShipWorks, ch" +
    "ange the Encoding below to another value.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 287);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Module Encoding:";
            // 
            // encodingComboBox
            // 
            this.encodingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encodingComboBox.FormattingEnabled = true;
            this.encodingComboBox.Location = new System.Drawing.Point(136, 284);
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
            this.orderStatusControl.Location = new System.Drawing.Point(5, 25);
            this.orderStatusControl.Name = "orderStatusControl";
            this.orderStatusControl.Size = new System.Drawing.Size(404, 80);
            this.orderStatusControl.TabIndex = 23;
            // 
            // sectionTitle1
            // 
            this.sectionTitle1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitle1.Location = new System.Drawing.Point(0, 159);
            this.sectionTitle1.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.sectionTitle1.Name = "sectionTitle1";
            this.sectionTitle1.Size = new System.Drawing.Size(435, 22);
            this.sectionTitle1.TabIndex = 19;
            this.sectionTitle1.Text = "Addendum Module";
            // 
            // downloadAddendumCheckoutQuestions
            // 
            this.downloadAddendumCheckoutQuestions.AutoSize = true;
            this.downloadAddendumCheckoutQuestions.Location = new System.Drawing.Point(20, 190);
            this.downloadAddendumCheckoutQuestions.Name = "downloadAddendumCheckoutQuestions";
            this.downloadAddendumCheckoutQuestions.Size = new System.Drawing.Size(303, 17);
            this.downloadAddendumCheckoutQuestions.TabIndex = 24;
            this.downloadAddendumCheckoutQuestions.Text = "Download responses from Addendum Checkout Questions";
            this.downloadAddendumCheckoutQuestions.UseVisualStyleBackColor = true;
            // 
            // infoTipAddendumCheckoutQuestions
            // 
            this.infoTipAddendumCheckoutQuestions.Caption = "Each questions response will be saved as a note for the order, prefixed with \"Add" +
    "endumAnswer: \" at the beginning of the note.";
            this.infoTipAddendumCheckoutQuestions.Location = new System.Drawing.Point(320, 192);
            this.infoTipAddendumCheckoutQuestions.Name = "infoTipAddendumCheckoutQuestions";
            this.infoTipAddendumCheckoutQuestions.Size = new System.Drawing.Size(12, 12);
            this.infoTipAddendumCheckoutQuestions.TabIndex = 25;
            this.infoTipAddendumCheckoutQuestions.Title = "Additional Checkout Data";
            // 
            // MivaStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.infoTipAddendumCheckoutQuestions);
            this.Controls.Add(this.downloadAddendumCheckoutQuestions);
            this.Controls.Add(this.sectionTitle1);
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
            this.Size = new System.Drawing.Size(435, 307);
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
        private UI.Controls.SectionTitle sectionTitle1;
        private System.Windows.Forms.CheckBox downloadAddendumCheckoutQuestions;
        private UI.Controls.InfoTip infoTipAddendumCheckoutQuestions;
    }
}
