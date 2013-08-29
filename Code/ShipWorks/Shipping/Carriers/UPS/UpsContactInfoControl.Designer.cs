using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.UPS
{
    partial class UpsContactInfoControl
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label nameLabel;
            System.Windows.Forms.Label phoneLabel;
            System.Windows.Forms.Label phoneExtensionLabel;
            ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.verbalConfirmationRequired = new System.Windows.Forms.CheckBox();
            this.labelVerbalConfirmation = new System.Windows.Forms.Label();
            this.nameTextBox = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.phoneNumberTextBox = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.phoneExtensionTextBox = new ShipWorks.UI.Controls.MultiValueTextBox();
            nameLabel = new System.Windows.Forms.Label();
            phoneLabel = new System.Windows.Forms.Label();
            phoneExtensionLabel = new System.Windows.Forms.Label();
            fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            nameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            nameLabel.AutoSize = true;
            nameLabel.Location = new System.Drawing.Point(6, 9);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(78, 13);
            nameLabel.TabIndex = 0;
            nameLabel.Text = "Contact Name:";
            nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // phoneLabel
            // 
            phoneLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            phoneLabel.AutoSize = true;
            phoneLabel.Location = new System.Drawing.Point(3, 40);
            phoneLabel.Name = "phoneLabel";
            phoneLabel.Size = new System.Drawing.Size(81, 13);
            phoneLabel.TabIndex = 0;
            phoneLabel.Text = "Phone Number:";
            phoneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // phoneExtensionLabel
            // 
            phoneExtensionLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            phoneExtensionLabel.Location = new System.Drawing.Point(177, 40);
            phoneExtensionLabel.Name = "phoneExtensionLabel";
            phoneExtensionLabel.Size = new System.Drawing.Size(18, 13);
            phoneExtensionLabel.TabIndex = 0;
            phoneExtensionLabel.Text = "x";
            phoneExtensionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(nameLabel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.nameTextBox, 1, 0);
            this.tableLayoutPanel.Controls.Add(phoneLabel, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.phoneNumberTextBox, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.phoneExtensionTextBox, 3, 1);
            this.tableLayoutPanel.Controls.Add(phoneExtensionLabel, 2, 1);
            this.tableLayoutPanel.Location = new System.Drawing.Point(104, 15);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(247, 62);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // verbalConfirmationRequired
            // 
            this.verbalConfirmationRequired.AutoSize = true;
            this.verbalConfirmationRequired.Location = new System.Drawing.Point(110, 0);
            this.verbalConfirmationRequired.Name = "verbalConfirmationRequired";
            this.verbalConfirmationRequired.Size = new System.Drawing.Size(201, 17);
            this.verbalConfirmationRequired.TabIndex = 0;
            this.verbalConfirmationRequired.Text = "Package requires verbal confirmation";
            this.verbalConfirmationRequired.UseVisualStyleBackColor = true;
            this.verbalConfirmationRequired.CheckedChanged += new System.EventHandler(this.OnVerbalConfirmationChanged);
            // 
            // labelVerbalConfirmation
            // 
            this.labelVerbalConfirmation.AutoSize = true;
            this.labelVerbalConfirmation.Location = new System.Drawing.Point(3, 1);
            this.labelVerbalConfirmation.Name = "labelVerbalConfirmation";
            this.labelVerbalConfirmation.Size = new System.Drawing.Size(101, 13);
            this.labelVerbalConfirmation.TabIndex = 11;
            this.labelVerbalConfirmation.Text = "Verbal Confirmation:";
            this.labelVerbalConfirmation.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.SetColumnSpan(this.nameTextBox, 3);
            this.nameTextBox.Location = new System.Drawing.Point(90, 5);
            fieldLengthProvider.SetMaxLengthSource(this.nameTextBox, ShipWorks.Data.Utility.EntityFieldLengthSource.UpsContactName);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(154, 20);
            this.nameTextBox.TabIndex = 1;
            // 
            // phoneNumberTextBox
            // 
            this.phoneNumberTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.phoneNumberTextBox.Location = new System.Drawing.Point(90, 36);
            fieldLengthProvider.SetMaxLengthSource(this.phoneNumberTextBox, ShipWorks.Data.Utility.EntityFieldLengthSource.UpsContactPhoneNumber);
            this.phoneNumberTextBox.Name = "phoneNumberTextBox";
            this.phoneNumberTextBox.Size = new System.Drawing.Size(81, 20);
            this.phoneNumberTextBox.TabIndex = 2;
            this.phoneNumberTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // phoneExtensionTextBox
            // 
            this.phoneExtensionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.phoneExtensionTextBox.Location = new System.Drawing.Point(201, 36);
            fieldLengthProvider.SetMaxLengthSource(this.phoneExtensionTextBox, ShipWorks.Data.Utility.EntityFieldLengthSource.UpsContactPhoneExtension);
            this.phoneExtensionTextBox.MinimumSize = new System.Drawing.Size(30, 4);
            this.phoneExtensionTextBox.Name = "phoneExtensionTextBox";
            this.phoneExtensionTextBox.Size = new System.Drawing.Size(43, 20);
            this.phoneExtensionTextBox.TabIndex = 3;
            // 
            // UpsContactInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.verbalConfirmationRequired);
            this.Controls.Add(this.labelVerbalConfirmation);
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UpsContactInfoControl";
            this.Size = new System.Drawing.Size(353, 80);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MultiValueTextBox nameTextBox;
        private MultiValueTextBox phoneNumberTextBox;
        private MultiValueTextBox phoneExtensionTextBox;
        private System.Windows.Forms.CheckBox verbalConfirmationRequired;
        private System.Windows.Forms.Label labelVerbalConfirmation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}
