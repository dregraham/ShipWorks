namespace ShipWorks.ApplicationCore.MessageBoxes
{
    partial class EnglishCultureRequiredDlg
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelProblem = new System.Windows.Forms.Label();
            this.close = new System.Windows.Forms.Button();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkForum = new ShipWorks.UI.Controls.LinkControl();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.error16;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.Location = new System.Drawing.Point(34, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(234, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "English (United States) Culture Required";
            // 
            // labelProblem
            // 
            this.labelProblem.Location = new System.Drawing.Point(34, 40);
            this.labelProblem.Name = "labelProblem";
            this.labelProblem.Size = new System.Drawing.Size(369, 34);
            this.labelProblem.TabIndex = 2;
            this.labelProblem.Text = "ShipWorks currently requires your computer\'s regional settings to be set to use t" +
                "he \'English (United States)\' culture.";
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(322, 132);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 3;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // labelInfo1
            // 
            this.labelInfo1.Location = new System.Drawing.Point(34, 83);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(333, 28);
            this.labelInfo1.TabIndex = 4;
            this.labelInfo1.Text = "If you need help making this adjustment, please contact us through our \r\n\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(126, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "or by calling 1-800-95-APPTIVE.";
            // 
            // linkForum
            // 
            this.linkForum.AutoSize = true;
            this.linkForum.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkForum.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkForum.ForeColor = System.Drawing.Color.Blue;
            this.linkForum.Location = new System.Drawing.Point(54, 96);
            this.linkForum.Name = "linkForum";
            this.linkForum.Size = new System.Drawing.Size(75, 13);
            this.linkForum.TabIndex = 5;
            this.linkForum.Text = "support forum";
            this.linkForum.Click += new System.EventHandler(this.OnClickSupportForum);
            // 
            // EnglishCultureRequiredDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(409, 167);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.linkForum);
            this.Controls.Add(this.labelInfo1);
            this.Controls.Add(this.close);
            this.Controls.Add(this.labelProblem);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EnglishCultureRequiredDlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShipWorks";
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelProblem;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Label labelInfo1;
        private UI.Controls.LinkControl linkForum;
        private System.Windows.Forms.Label label2;
    }
}