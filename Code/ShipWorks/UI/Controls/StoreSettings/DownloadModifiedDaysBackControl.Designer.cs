namespace ShipWorks.UI.Controls.StoreSettings
{
    partial class DownloadModifiedDaysBackControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadModifiedDaysBackControl));
            this.daysBackLabel = new System.Windows.Forms.Label();
            this.daysBack = new System.Windows.Forms.ComboBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // daysBackLabel
            // 
            this.daysBackLabel.AutoSize = true;
            this.daysBackLabel.Location = new System.Drawing.Point(10, 10);
            this.daysBackLabel.Name = "daysBackLabel";
            this.daysBackLabel.Size = new System.Drawing.Size(297, 13);
            this.daysBackLabel.TabIndex = 0;
            this.daysBackLabel.Text = "Number of days back to check for modified or skipped orders:";
            // 
            // daysBack
            // 
            this.daysBack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.daysBack.FormattingEnabled = true;
            this.daysBack.Location = new System.Drawing.Point(313, 7);
            this.daysBack.Name = "daysBack";
            this.daysBack.Size = new System.Drawing.Size(75, 21);
            this.daysBack.TabIndex = 1;
            // 
            // infoLabel
            // 
            this.infoLabel.ForeColor = System.Drawing.Color.DimGray;
            this.infoLabel.Location = new System.Drawing.Point(10, 31);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(386, 42);
            this.infoLabel.TabIndex = 2;
            this.infoLabel.Text = resources.GetString("infoLabel.Text");
            // 
            // DownloadModifiedDaysBackControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.daysBack);
            this.Controls.Add(this.daysBackLabel);
            this.Name = "DownloadModifiedDaysBackControl";
            this.Size = new System.Drawing.Size(549, 75);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label daysBackLabel;
        private System.Windows.Forms.ComboBox daysBack;
        private System.Windows.Forms.Label infoLabel;
    }
}
