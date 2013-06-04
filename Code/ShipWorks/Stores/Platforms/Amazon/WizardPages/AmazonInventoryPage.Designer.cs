namespace ShipWorks.Stores.Platforms.Amazon.WizardPages
{
    partial class AmazonInventoryPage
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.importInventoryControl = new ShipWorks.Stores.Platforms.Amazon.AmazonImportInventoryControl();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(464, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "To retrieve product images and weights ShipWorks requires the ASIN of each item s" +
                "old.  To find the ASIN ShipWorks uses your Amazon inventory report to map each S" +
                "KU to its ASIN.";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(464, 38);
            this.label2.TabIndex = 1;
            this.label2.Text = "It is not necessary to complete this step, but without it ShipWorks will not be a" +
                "ble to retrieve product weights or images.";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(35, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(395, 43);
            this.label3.TabIndex = 2;
            this.label3.Text = "1. Login to Seller Central and navigate to the Reports -> Inventory section.  Thi" +
                "s is also sometimes found under the \"Advanced Features\" section of Reports.";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(35, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(464, 34);
            this.label4.TabIndex = 3;
            this.label4.Text = "2. Click \"Request Report\".  The report can take from a few minutes to 45 minutes " +
                "to complete.";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(35, 146);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(441, 34);
            this.label5.TabIndex = 4;
            this.label5.Text = "3. Return to the Reports -> Inventory section to download the completed report.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(35, 236);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Important:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.warning16;
            this.pictureBox1.Location = new System.Drawing.Point(15, 234);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(18, 18);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(110, 236);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(336, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "- Only active items with quantity greater than zero are in the report.";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(110, 252);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(383, 42);
            this.label9.TabIndex = 11;
            this.label9.Text = "- Amazon does not provide access to the Shipping Weight.  ShipWorks first tries t" +
                "o use the Packaging Weight if present, and if not looks for the Item Weight.";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Text Files (*.txt)|*.txt";
            // 
            // importInventoryControl
            // 
            this.importInventoryControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.importInventoryControl.Location = new System.Drawing.Point(36, 171);
            this.importInventoryControl.Name = "importInventoryControl";
            this.importInventoryControl.Size = new System.Drawing.Size(129, 24);
            this.importInventoryControl.TabIndex = 12;
            // 
            // InventoryPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.importInventoryControl);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Description = "Download your Amazon Inventory Report.";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "InventoryPage";
            this.Size = new System.Drawing.Size(519, 299);
            this.Title = "Amazon Inventory";
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private AmazonImportInventoryControl importInventoryControl;
    }
}
