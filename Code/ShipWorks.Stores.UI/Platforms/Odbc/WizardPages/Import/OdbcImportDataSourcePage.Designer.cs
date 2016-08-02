using ShipWorks.Stores.UI.Platforms.Odbc.Controls;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Import
{
    partial class OdbcImportDataSourcePage
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
            this.importsettings = new System.Windows.Forms.Label();
            this.odbcDataSourceControl = new OdbcDataSourceControl();
            this.SuspendLayout();
            // 
            // importsettings
            // 
            this.importsettings.AutoSize = true;
            this.importsettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.importsettings.Location = new System.Drawing.Point(20, 10);
            this.importsettings.Name = "importsettings";
            this.importsettings.Size = new System.Drawing.Size(97, 13);
            this.importsettings.TabIndex = 1;
            this.importsettings.Text = "Import Settings";
            // 
            // odbcDataSourceControl
            // 
            this.odbcDataSourceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.odbcDataSourceControl.Location = new System.Drawing.Point(30, 23);
            this.odbcDataSourceControl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.odbcDataSourceControl.Name = "odbcDataSourceControl";
            this.odbcDataSourceControl.Size = new System.Drawing.Size(487, 209);
            this.odbcDataSourceControl.TabIndex = 0;
            // 
            // OdbcImportDataSourcePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.importsettings);
            this.Controls.Add(this.odbcDataSourceControl);
            this.Description = "Setup the ODBC data source used for importing your orders.";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "OdbcImportDataSourcePage";
            this.Size = new System.Drawing.Size(532, 242);
            this.Title = "Setup Import Data Source";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OdbcDataSourceControl odbcDataSourceControl;
        private System.Windows.Forms.Label importsettings;
    }
}
