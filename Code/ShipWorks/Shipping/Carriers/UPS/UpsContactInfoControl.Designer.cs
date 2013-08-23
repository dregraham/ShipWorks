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
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
            System.Windows.Forms.Label nameLabel;
            System.Windows.Forms.Label phoneLabel;
            System.Windows.Forms.Label phoneExtensionLabel;
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.phoneNumberTextBox = new System.Windows.Forms.TextBox();
            this.phoneExtensionTextBox = new System.Windows.Forms.TextBox();
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            nameLabel = new System.Windows.Forms.Label();
            phoneLabel = new System.Windows.Forms.Label();
            phoneExtensionLabel = new System.Windows.Forms.Label();
            tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 5;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(nameLabel, 0, 0);
            tableLayoutPanel.Controls.Add(this.nameTextBox, 1, 0);
            tableLayoutPanel.Controls.Add(phoneLabel, 0, 1);
            tableLayoutPanel.Controls.Add(this.phoneNumberTextBox, 1, 1);
            tableLayoutPanel.Controls.Add(this.phoneExtensionTextBox, 3, 1);
            tableLayoutPanel.Controls.Add(phoneExtensionLabel, 2, 1);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 2;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel.Size = new System.Drawing.Size(354, 62);
            tableLayoutPanel.TabIndex = 0;
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
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            tableLayoutPanel.SetColumnSpan(this.nameTextBox, 4);
            this.nameTextBox.Location = new System.Drawing.Point(90, 5);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(261, 20);
            this.nameTextBox.TabIndex = 1;
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
            // phoneNumberTextBox
            // 
            this.phoneNumberTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.phoneNumberTextBox.Location = new System.Drawing.Point(90, 36);
            this.phoneNumberTextBox.Name = "phoneNumberTextBox";
            this.phoneNumberTextBox.Size = new System.Drawing.Size(116, 20);
            this.phoneNumberTextBox.TabIndex = 1;
            this.phoneNumberTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // phoneExtensionTextBox
            // 
            this.phoneExtensionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.phoneExtensionTextBox.Location = new System.Drawing.Point(236, 36);
            this.phoneExtensionTextBox.MinimumSize = new System.Drawing.Size(30, 4);
            this.phoneExtensionTextBox.Name = "phoneExtensionTextBox";
            this.phoneExtensionTextBox.Size = new System.Drawing.Size(43, 20);
            this.phoneExtensionTextBox.TabIndex = 1;
            // 
            // phoneExtensionLabel
            // 
            phoneExtensionLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            phoneExtensionLabel.Location = new System.Drawing.Point(212, 40);
            phoneExtensionLabel.Name = "phoneExtensionLabel";
            phoneExtensionLabel.Size = new System.Drawing.Size(18, 13);
            phoneExtensionLabel.TabIndex = 0;
            phoneExtensionLabel.Text = "x";
            phoneExtensionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // UpsContactInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(tableLayoutPanel);
            this.Name = "UpsContactInfoControl";
            this.Size = new System.Drawing.Size(354, 62);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox phoneNumberTextBox;
        private System.Windows.Forms.TextBox phoneExtensionTextBox;
    }
}
