namespace ShipWorks.Shipping.ScanForms
{
    partial class ScanFormSuccessDlg
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
            this.pictureSuccess = new System.Windows.Forms.PictureBox();
            this.labelSuccess = new System.Windows.Forms.Label();
            this.print = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize) (this.pictureSuccess)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureSuccess
            // 
            this.pictureSuccess.Image = global::ShipWorks.Properties.Resources.check16;
            this.pictureSuccess.Location = new System.Drawing.Point(12, 12);
            this.pictureSuccess.Name = "pictureSuccess";
            this.pictureSuccess.Size = new System.Drawing.Size(16, 16);
            this.pictureSuccess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureSuccess.TabIndex = 0;
            this.pictureSuccess.TabStop = false;
            // 
            // labelSuccess
            // 
            this.labelSuccess.AutoSize = true;
            this.labelSuccess.Location = new System.Drawing.Point(32, 14);
            this.labelSuccess.Name = "labelSuccess";
            this.labelSuccess.Size = new System.Drawing.Size(221, 13);
            this.labelSuccess.TabIndex = 1;
            this.labelSuccess.Text = "The SCAN Form was generated successfully.";
            // 
            // print
            // 
            this.print.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.print.Location = new System.Drawing.Point(115, 54);
            this.print.Name = "print";
            this.print.Size = new System.Drawing.Size(75, 23);
            this.print.TabIndex = 2;
            this.print.Text = "Print Now...";
            this.print.UseVisualStyleBackColor = true;
            this.print.Click += new System.EventHandler(this.OnPrint);
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(196, 54);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 3;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // EndiciaScanFormSuccessDlg
            // 
            this.AcceptButton = this.print;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(286, 91);
            this.Controls.Add(this.close);
            this.Controls.Add(this.print);
            this.Controls.Add(this.labelSuccess);
            this.Controls.Add(this.pictureSuccess);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EndiciaScanFormSuccessDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SCAN Form";
            ((System.ComponentModel.ISupportInitialize) (this.pictureSuccess)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureSuccess;
        private System.Windows.Forms.Label labelSuccess;
        private System.Windows.Forms.Button print;
        private System.Windows.Forms.Button close;
    }
}