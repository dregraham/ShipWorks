namespace ShipWorks.Shipping.Settings.Printing
{
    partial class PrintRuleInstallMissingTemplateDlg
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
            this.choose = new System.Windows.Forms.Button();
            this.recreate = new System.Windows.Forms.Button();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // choose
            // 
            this.choose.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.choose.Location = new System.Drawing.Point(269, 149);
            this.choose.Name = "choose";
            this.choose.Size = new System.Drawing.Size(143, 23);
            this.choose.TabIndex = 1;
            this.choose.Text = "Choose Existing Template";
            this.choose.UseVisualStyleBackColor = true;
            this.choose.Click += new System.EventHandler(this.OnChooseTemplate);
            // 
            // recreate
            // 
            this.recreate.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.recreate.Location = new System.Drawing.Point(113, 149);
            this.recreate.Name = "recreate";
            this.recreate.Size = new System.Drawing.Size(150, 23);
            this.recreate.TabIndex = 0;
            this.recreate.Text = "Recreate Default Template";
            this.recreate.UseVisualStyleBackColor = true;
            this.recreate.Click += new System.EventHandler(this.OnRecreateTemplate);
            // 
            // labelInfo1
            // 
            this.labelInfo1.Location = new System.Drawing.Point(12, 9);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(398, 35);
            this.labelInfo1.TabIndex = 2;
            this.labelInfo1.Text = "ShipWorks needs to create a default rule for printing and needs the following tem" +
                "plate that came with ShipWorks:\r\n";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.template_general_16;
            this.pictureBox1.Location = new System.Drawing.Point(25, 45);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelName.Location = new System.Drawing.Point(43, 47);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(96, 13);
            this.labelName.TabIndex = 4;
            this.labelName.Text = "Template Name";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(398, 55);
            this.label3.TabIndex = 5;
            this.label3.Text = "The template has been renamed, moved, or deleted.\r\n\r\nShipWorks can recreate the t" +
                "emplate that was originally installed with ShipWorks, or you can choose an exist" +
                "ing template.";
            // 
            // PrintRuleInstallMissingTemplateDlg
            // 
            this.AcceptButton = this.recreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 184);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelInfo1);
            this.Controls.Add(this.recreate);
            this.Controls.Add(this.choose);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintRuleInstallMissingTemplateDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Template";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button choose;
        private System.Windows.Forms.Button recreate;
        private System.Windows.Forms.Label labelInfo1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label label3;
    }
}