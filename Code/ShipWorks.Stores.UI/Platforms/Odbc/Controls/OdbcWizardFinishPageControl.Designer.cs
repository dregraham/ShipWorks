namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls
{
    partial class OdbcWizardFinishPageControl
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
            this.label11 = new System.Windows.Forms.Label();
            this.linkUseABarcodeScanner = new ShipWorks.UI.Controls.LinkControl();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 58);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(185, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "To turbocharge your order searches,";
            // 
            // linkUseABarcodeScanner
            // 
            this.linkUseABarcodeScanner.AutoSize = true;
            this.linkUseABarcodeScanner.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkUseABarcodeScanner.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkUseABarcodeScanner.ForeColor = System.Drawing.Color.Blue;
            this.linkUseABarcodeScanner.Location = new System.Drawing.Point(198, 58);
            this.linkUseABarcodeScanner.Name = "linkUseABarcodeScanner";
            this.linkUseABarcodeScanner.Size = new System.Drawing.Size(120, 13);
            this.linkUseABarcodeScanner.TabIndex = 26;
            this.linkUseABarcodeScanner.Text = "use a barcode scanner.";
            this.linkUseABarcodeScanner.Click += new System.EventHandler(this.OnClickLinkUseABarcodeScanner);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(14, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(417, 37);
            this.label9.TabIndex = 25;
            this.label9.Text = "Shipworks waits until you search for an order number from this ODBC store, and th" +
    "en downloads and displays that order in real time.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(129)))), ((int)(((byte)(189)))));
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Search for Orders";
            // 
            // OdbcWizardFinishPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label11);
            this.Controls.Add(this.linkUseABarcodeScanner);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OdbcWizardFinishPageControl";
            this.Size = new System.Drawing.Size(456, 73);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label11;
        private ShipWorks.UI.Controls.LinkControl linkUseABarcodeScanner;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
    }
}
