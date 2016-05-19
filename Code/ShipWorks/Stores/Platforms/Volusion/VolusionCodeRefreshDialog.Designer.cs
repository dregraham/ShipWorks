namespace ShipWorks.Stores.Platforms.Volusion
{
    partial class VolusionCodeRefreshDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.instructionsLabel = new System.Windows.Forms.Label();
            this.fileHeaderLabel = new System.Windows.Forms.Label();
            this.import = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.linkHelp = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.labelHelp = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // instructionsLabel
            // 
            this.instructionsLabel.Location = new System.Drawing.Point(13, 13);
            this.instructionsLabel.Name = "instructionsLabel";
            this.instructionsLabel.Size = new System.Drawing.Size(479, 27);
            this.instructionsLabel.TabIndex = 0;
            this.instructionsLabel.Text = "ShipWorks needs the lists of <type> from your Volusion store.  Download the CSV e" +
                "xport from your store\'s Admin Area and import the file below.";
            // 
            // fileHeaderLabel
            // 
            this.fileHeaderLabel.AutoSize = true;
            this.fileHeaderLabel.Location = new System.Drawing.Point(13, 54);
            this.fileHeaderLabel.Name = "fileHeaderLabel";
            this.fileHeaderLabel.Size = new System.Drawing.Size(133, 13);
            this.fileHeaderLabel.TabIndex = 3;
            this.fileHeaderLabel.Text = "<type> file (CSV Format):";
            // 
            // import
            // 
            this.import.Location = new System.Drawing.Point(37, 72);
            this.import.Name = "import";
            this.import.Size = new System.Drawing.Size(75, 23);
            this.import.TabIndex = 5;
            this.import.Text = "Import...";
            this.import.UseVisualStyleBackColor = true;
            this.import.Click += new System.EventHandler(this.OnImport);
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(393, 116);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 6;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Volusion CSV Export files|*.csv|All files|*.*";
            this.openFileDialog.Title = "Volusion Import";
            // 
            // linkHelp
            // 
            this.linkHelp.AutoSize = true;
            this.linkHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkHelp.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkHelp.ForeColor = System.Drawing.Color.RoyalBlue;
            this.linkHelp.Location = new System.Drawing.Point(118, 77);
            this.linkHelp.Name = "linkHelp";
            this.linkHelp.Size = new System.Drawing.Size(53, 13);
            this.linkHelp.TabIndex = 11;
            this.linkHelp.Text = "Click here";
            this.linkHelp.Url = "http://support.shipworks.com/support/solutions/articles/129346-importing-shipping-methods-and-payment-methods-for-a-volusion-store";
            // 
            // labelHelp
            // 
            this.labelHelp.AutoSize = true;
            this.labelHelp.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelHelp.Location = new System.Drawing.Point(167, 77);
            this.labelHelp.Name = "labelHelp";
            this.labelHelp.Size = new System.Drawing.Size(251, 13);
            this.labelHelp.TabIndex = 12;
            this.labelHelp.Text = "for help with creating and downloading the export.";
            // 
            // VolusionCodeRefreshDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(480, 151);
            this.Controls.Add(this.linkHelp);
            this.Controls.Add(this.labelHelp);
            this.Controls.Add(this.close);
            this.Controls.Add(this.import);
            this.Controls.Add(this.fileHeaderLabel);
            this.Controls.Add(this.instructionsLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VolusionCodeRefreshDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Volusion Import";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label instructionsLabel;
        private System.Windows.Forms.Label fileHeaderLabel;
        private System.Windows.Forms.Button import;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private ShipWorks.ApplicationCore.Interaction.HelpLink linkHelp;
        private System.Windows.Forms.Label labelHelp;
    }
}