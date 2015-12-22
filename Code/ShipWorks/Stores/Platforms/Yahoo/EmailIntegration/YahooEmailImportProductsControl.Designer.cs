namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration
{
    partial class YahooEmailImportProductsControl
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
            this.import = new System.Windows.Forms.Button();
            this.catalogUrl = new System.Windows.Forms.TextBox();
            this.labelCatalogUrl = new System.Windows.Forms.Label();
            this.labelInfo2 = new System.Windows.Forms.Label();
            this.labelExample = new System.Windows.Forms.Label();
            this.labelNoteContent = new System.Windows.Forms.Label();
            this.labelNote = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkHelp = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // import
            // 
            this.import.Location = new System.Drawing.Point(407, 45);
            this.import.Name = "import";
            this.import.Size = new System.Drawing.Size(75, 23);
            this.import.TabIndex = 19;
            this.import.Text = "Import";
            this.import.UseVisualStyleBackColor = true;
            this.import.Click += new System.EventHandler(this.OnImportInventory);
            // 
            // catalogUrl
            // 
            this.catalogUrl.Location = new System.Drawing.Point(119, 20);
            this.catalogUrl.Name = "catalogUrl";
            this.catalogUrl.Size = new System.Drawing.Size(363, 21);
            this.catalogUrl.TabIndex = 14;
            // 
            // labelCatalogUrl
            // 
            this.labelCatalogUrl.AutoSize = true;
            this.labelCatalogUrl.Location = new System.Drawing.Point(3, 23);
            this.labelCatalogUrl.Name = "labelCatalogUrl";
            this.labelCatalogUrl.Size = new System.Drawing.Size(110, 13);
            this.labelCatalogUrl.TabIndex = 13;
            this.labelCatalogUrl.Text = "Product Catalog URL:";
            // 
            // labelInfo2
            // 
            this.labelInfo2.AutoSize = true;
            this.labelInfo2.Location = new System.Drawing.Point(52, 2);
            this.labelInfo2.Name = "labelInfo2";
            this.labelInfo2.Size = new System.Drawing.Size(284, 13);
            this.labelInfo2.TabIndex = 12;
            this.labelInfo2.Text = "to learn how to enable and get your product catalog URL.";
            // 
            // labelExample
            // 
            this.labelExample.AutoSize = true;
            this.labelExample.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelExample.Location = new System.Drawing.Point(116, 44);
            this.labelExample.Name = "labelExample";
            this.labelExample.Size = new System.Drawing.Size(239, 13);
            this.labelExample.TabIndex = 15;
            this.labelExample.Text = "(Example: http://store.domain.com/catalog.xml)";
            // 
            // labelNoteContent
            // 
            this.labelNoteContent.AutoSize = true;
            this.labelNoteContent.Location = new System.Drawing.Point(61, 74);
            this.labelNoteContent.Name = "labelNoteContent";
            this.labelNoteContent.Size = new System.Drawing.Size(402, 26);
            this.labelNoteContent.TabIndex = 18;
            this.labelNoteContent.Text = "- ShipWorks uses the Ship-weight field from Yahoo!\r\n- Any time you add or change " +
                "product weights in Yahoo! you will need to re-import";
            // 
            // labelNote
            // 
            this.labelNote.AutoSize = true;
            this.labelNote.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelNote.Location = new System.Drawing.Point(23, 74);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(36, 13);
            this.labelNote.TabIndex = 16;
            this.labelNote.Text = "Note:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox1.Location = new System.Drawing.Point(5, 73);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // linkHelp
            // 
            this.linkHelp.AutoSize = true;
            this.linkHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkHelp.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkHelp.ForeColor = System.Drawing.Color.Blue;
            this.linkHelp.Location = new System.Drawing.Point(3, 2);
            this.linkHelp.Name = "linkHelp";
            this.linkHelp.Size = new System.Drawing.Size(53, 13);
            this.linkHelp.TabIndex = 11;
            this.linkHelp.Text = "Click here";
            // 
            // YahooImportProductsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.import);
            this.Controls.Add(this.catalogUrl);
            this.Controls.Add(this.labelCatalogUrl);
            this.Controls.Add(this.labelInfo2);
            this.Controls.Add(this.linkHelp);
            this.Controls.Add(this.labelExample);
            this.Controls.Add(this.labelNoteContent);
            this.Controls.Add(this.labelNote);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "YahooImportProductsControl";
            this.Size = new System.Drawing.Size(513, 122);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button import;
        private System.Windows.Forms.TextBox catalogUrl;
        private System.Windows.Forms.Label labelCatalogUrl;
        private System.Windows.Forms.Label labelInfo2;
        private ShipWorks.ApplicationCore.Interaction.HelpLink linkHelp;
        private System.Windows.Forms.Label labelExample;
        private System.Windows.Forms.Label labelNoteContent;
        private System.Windows.Forms.Label labelNote;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
