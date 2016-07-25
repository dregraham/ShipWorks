namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    partial class OdbcUploadDataSourcePage
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
            this.odbcDataSourceControl = new ShipWorks.Stores.UI.Platforms.Odbc.OdbcDataSourceControl();
            this.uploadsettings = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // odbcDataSourceControl
            //
            this.odbcDataSourceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.odbcDataSourceControl.Location = new System.Drawing.Point(30, 22);
            this.odbcDataSourceControl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.odbcDataSourceControl.Name = "odbcDataSourceControl";
            this.odbcDataSourceControl.Size = new System.Drawing.Size(487, 209);
            this.odbcDataSourceControl.TabIndex = 1;
            //
            // uploadsettings
            //
            this.uploadsettings.AutoSize = true;
            this.uploadsettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.uploadsettings.Location = new System.Drawing.Point(20, 10);
            this.uploadsettings.Name = "uploadsettings";
            this.uploadsettings.Size = new System.Drawing.Size(96, 13);
            this.uploadsettings.TabIndex = 2;
            this.uploadsettings.Text = "Upload Settings";
            //
            // OdbcUploadDataSourcePage
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uploadsettings);
            this.Controls.Add(this.odbcDataSourceControl);
            this.Description = "Setup the ODBC data source used for uploading your shipment details.";
            this.Name = "OdbcUploadDataSourcePage";
            this.Size = new System.Drawing.Size(547, 291);
            this.Title = "Setup Upload Data Source";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OdbcDataSourceControl odbcDataSourceControl;
        private System.Windows.Forms.Label uploadsettings;
    }
}
