﻿namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    partial class OdbcDataSourcePage
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
            this.odbcDataSourceControl1 = new ShipWorks.Stores.UI.Platforms.Odbc.OdbcDataSourceControl();
            this.SuspendLayout();
            // 
            // odbcDataSourceControl1
            // 
            this.odbcDataSourceControl1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.odbcDataSourceControl1.Location = new System.Drawing.Point(0, 0);
            this.odbcDataSourceControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.odbcDataSourceControl1.Name = "odbcDataSourceControl1";
            this.odbcDataSourceControl1.Size = new System.Drawing.Size(433, 209);
            this.odbcDataSourceControl1.TabIndex = 0;
            // 
            // OdbcDataSourcePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.odbcDataSourceControl1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "OdbcDataSourcePage";
            this.Size = new System.Drawing.Size(487, 209);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);

        }

        #endregion

        private OdbcDataSourceControl odbcDataSourceControl1;
    }
}
