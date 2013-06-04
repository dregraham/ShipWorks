namespace ShipWorks.Templates.Media
{
    partial class PageCopiesControl
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
            this.collate = new System.Windows.Forms.CheckBox();
            this.copies = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.collateImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize) (this.copies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.collateImage)).BeginInit();
            this.SuspendLayout();
            // 
            // collate
            // 
            this.collate.AutoSize = true;
            this.collate.Enabled = false;
            this.collate.Location = new System.Drawing.Point(114, 41);
            this.collate.Name = "collate";
            this.collate.Size = new System.Drawing.Size(58, 17);
            this.collate.TabIndex = 7;
            this.collate.Text = "Collate";
            this.collate.UseVisualStyleBackColor = true;
            this.collate.CheckedChanged += new System.EventHandler(this.OnChangeCollate);
            // 
            // copies
            // 
            this.copies.Location = new System.Drawing.Point(114, 2);
            this.copies.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.copies.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.copies.Name = "copies";
            this.copies.Size = new System.Drawing.Size(53, 20);
            this.copies.TabIndex = 5;
            this.copies.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.copies.ValueChanged += new System.EventHandler(this.OnChangeCopies);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Number of copies:";
            // 
            // collateImage
            // 
            this.collateImage.Image = global::ShipWorks.Properties.Resources.print_collate_off;
            this.collateImage.Location = new System.Drawing.Point(4, 29);
            this.collateImage.Name = "collateImage";
            this.collateImage.Size = new System.Drawing.Size(101, 41);
            this.collateImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.collateImage.TabIndex = 6;
            this.collateImage.TabStop = false;
            // 
            // PageCopiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.collate);
            this.Controls.Add(this.collateImage);
            this.Controls.Add(this.copies);
            this.Controls.Add(this.label1);
            this.Name = "PageCopiesControl";
            this.Size = new System.Drawing.Size(177, 78);
            ((System.ComponentModel.ISupportInitialize) (this.copies)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.collateImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox collate;
        private System.Windows.Forms.PictureBox collateImage;
        private System.Windows.Forms.NumericUpDown copies;
        private System.Windows.Forms.Label label1;
    }
}
