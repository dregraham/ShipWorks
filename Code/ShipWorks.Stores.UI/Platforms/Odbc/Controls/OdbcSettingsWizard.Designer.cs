namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls
{
    partial class OdbcSettingsWizard
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(366, 576);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(447, 576);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(285, 576);
            // 
            // mainPanel
            // 
            this.mainPanel.Size = new System.Drawing.Size(534, 504);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 566);
            this.etchBottom.Size = new System.Drawing.Size(538, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(481, 3);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(534, 56);
            // 
            // OdbcSettingsWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 611);
            this.LastPageCancelable = true;
            this.Name = "OdbcSettingsWizard";
            this.NextVisible = true;
            this.Text = "Odbc Store Setup Wizard";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}