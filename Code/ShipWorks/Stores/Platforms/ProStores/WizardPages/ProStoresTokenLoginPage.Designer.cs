namespace ShipWorks.Stores.Platforms.ProStores.WizardPages
{
    partial class ProStoresTokenLoginPage
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
            this.panelCreate = new System.Windows.Forms.Panel();
            this.createTokenControl = new ShipWorks.Stores.Platforms.ProStores.ProStoresTokenCreateControl();
            this.label7 = new System.Windows.Forms.Label();
            this.linkImportTokenFile = new ShipWorks.UI.Controls.LinkControl();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.manageTokenControl = new ShipWorks.Stores.Platforms.ProStores.ProStoresTokenManageControl();
            this.panelCreate.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCreate
            // 
            this.panelCreate.Controls.Add(this.createTokenControl);
            this.panelCreate.Controls.Add(this.label7);
            this.panelCreate.Controls.Add(this.linkImportTokenFile);
            this.panelCreate.Location = new System.Drawing.Point(16, 47);
            this.panelCreate.Name = "panelCreate";
            this.panelCreate.Size = new System.Drawing.Size(479, 81);
            this.panelCreate.TabIndex = 16;
            // 
            // createTokenControl
            // 
            this.createTokenControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.createTokenControl.Location = new System.Drawing.Point(36, 5);
            this.createTokenControl.Name = "createTokenControl";
            this.createTokenControl.Size = new System.Drawing.Size(411, 31);
            this.createTokenControl.TabIndex = 10;
            this.createTokenControl.TokenImported += new System.EventHandler(this.OnCreateTokenCompleted);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label7.Location = new System.Drawing.Point(8, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(397, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "If you have previously created and saved a token and need to import a token file\r" +
                "\n";
            // 
            // linkImportTokenFile
            // 
            this.linkImportTokenFile.AutoSize = true;
            this.linkImportTokenFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkImportTokenFile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkImportTokenFile.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkImportTokenFile.Location = new System.Drawing.Point(402, 56);
            this.linkImportTokenFile.Name = "linkImportTokenFile";
            this.linkImportTokenFile.Size = new System.Drawing.Size(55, 13);
            this.linkImportTokenFile.TabIndex = 9;
            this.linkImportTokenFile.Text = "click here.";
            this.linkImportTokenFile.Click += new System.EventHandler(this.OnImportTokenFile);
            // 
            // labelInfo1
            // 
            this.labelInfo1.Location = new System.Drawing.Point(24, 14);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(528, 34);
            this.labelInfo1.TabIndex = 15;
            this.labelInfo1.Text = "ProStores requires you to authorize ShipWorks to connect to your store.  This is " +
                "done by logging into a special ProStores page that creates an Login Token for Sh" +
                "ipWorks.";
            // 
            // manageTokenControl
            // 
            this.manageTokenControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.manageTokenControl.Location = new System.Drawing.Point(16, 134);
            this.manageTokenControl.Name = "manageTokenControl";
            this.manageTokenControl.Size = new System.Drawing.Size(524, 109);
            this.manageTokenControl.TabIndex = 17;
            this.manageTokenControl.Visible = false;
            // 
            // ProStoresTokenLoginPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.manageTokenControl);
            this.Controls.Add(this.panelCreate);
            this.Controls.Add(this.labelInfo1);
            this.Name = "ProStoresTokenLoginPage";
            this.Size = new System.Drawing.Size(616, 315);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.panelCreate.ResumeLayout(false);
            this.panelCreate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelCreate;
        private System.Windows.Forms.Label label7;
        private ShipWorks.UI.Controls.LinkControl linkImportTokenFile;
        private System.Windows.Forms.Label labelInfo1;
        private ProStoresTokenCreateControl createTokenControl;
        private ProStoresTokenManageControl manageTokenControl;
    }
}
