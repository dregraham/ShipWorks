namespace ShipWorks.Stores.Platforms.NetworkSolutions.WizardPages
{
    partial class NetworkSolutionsAccountPage
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelCreate = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.linkImportTokenFile = new ShipWorks.UI.Controls.LinkControl();
            this.tokenImport = new ShipWorks.Stores.Platforms.NetworkSolutions.NetworkSolutionsCreateTokenControl();
            this.manageTokenControl = new ShipWorks.Stores.Platforms.NetworkSolutions.NetworkSolutionsManageTokenControl();
            this.panelCreate.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(462, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "Network Solutions requires you to authorize ShipWorks to connect to your Network " +
                "Solutions account.";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(462, 31);
            this.label2.TabIndex = 2;
            this.label2.Text = "Create a login token by logging into a special Network Solutions web page.  Once " +
                "completed, ShipWorks will automatically import the token.\r\n";
            // 
            // panelCreate
            // 
            this.panelCreate.Controls.Add(this.label7);
            this.panelCreate.Controls.Add(this.linkImportTokenFile);
            this.panelCreate.Controls.Add(this.tokenImport);
            this.panelCreate.Location = new System.Drawing.Point(15, 86);
            this.panelCreate.Name = "panelCreate";
            this.panelCreate.Size = new System.Drawing.Size(459, 65);
            this.panelCreate.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label7.Location = new System.Drawing.Point(3, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(397, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "If you have previously created and saved a token and need to import a token file\r" +
                "\n";
            // 
            // linkImportTokenFile
            // 
            this.linkImportTokenFile.AutoSize = true;
            this.linkImportTokenFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkImportTokenFile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkImportTokenFile.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkImportTokenFile.Location = new System.Drawing.Point(397, 43);
            this.linkImportTokenFile.Name = "linkImportTokenFile";
            this.linkImportTokenFile.Size = new System.Drawing.Size(55, 13);
            this.linkImportTokenFile.TabIndex = 11;
            this.linkImportTokenFile.Text = "click here.";
            this.linkImportTokenFile.Click += new System.EventHandler(this.OnImportTokenFile);
            // 
            // tokenImport
            // 
            this.tokenImport.Location = new System.Drawing.Point(26, 3);
            this.tokenImport.Name = "tokenImport";
            this.tokenImport.Size = new System.Drawing.Size(410, 28);
            this.tokenImport.SuccessText = "Your NetworkSolutions Token has been imported!";
            this.tokenImport.TabIndex = 1;
            this.tokenImport.TokenImported += new System.EventHandler(this.OnCreateTokenCompleted);
            // 
            // manageTokenControl
            // 
            this.manageTokenControl.Location = new System.Drawing.Point(15, 157);
            this.manageTokenControl.Name = "manageTokenControl";
            this.manageTokenControl.Size = new System.Drawing.Size(524, 106);
            this.manageTokenControl.TabIndex = 4;
            this.manageTokenControl.Visible = false;
            // 
            // NetworkSolutionsAccountPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.manageTokenControl);
            this.Controls.Add(this.panelCreate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "NetworkSolutionsAccountPage";
            this.Size = new System.Drawing.Size(544, 292);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.panelCreate.ResumeLayout(false);
            this.panelCreate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelCreate;
        private NetworkSolutionsCreateTokenControl tokenImport;
        private System.Windows.Forms.Label label7;
        private ShipWorks.UI.Controls.LinkControl linkImportTokenFile;
        private NetworkSolutionsManageTokenControl manageTokenControl;
    }
}
