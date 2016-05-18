namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    partial class OdbcImportFieldMappingPage
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
            scope?.Dispose();

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
            this.elementHost = new System.Windows.Forms.Integration.ElementHost();
            this.odbcImportFieldMappingControl = new ShipWorks.Stores.UI.Platforms.Odbc.OdbcImportFieldMappingControl();
            this.SuspendLayout();
            //
            // elementHost
            //
            this.elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost.Location = new System.Drawing.Point(0, 0);
            this.elementHost.Name = "elementHost";
            this.elementHost.Size = new System.Drawing.Size(540, 500);
            this.elementHost.TabIndex = 0;
            this.elementHost.Text = "elementHost1";
            this.elementHost.Child = this.odbcImportFieldMappingControl;
            //
            // OdbcImportFieldMappingPage
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.elementHost);
            this.Description = "Setup the database table and mappings of your columns into ShipWorks";
            this.Name = "OdbcImportFieldMappingPage";
            this.Size = new System.Drawing.Size(540, 500);
            this.Title = "Column Mappings";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost;
        private OdbcImportFieldMappingControl odbcImportFieldMappingControl;
    }
}
