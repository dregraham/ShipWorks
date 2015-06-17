using ShipWorks.Common.Net;

namespace ShipWorks.Stores.Platforms.Shopify
{
    partial class ShopifyCreateTokenWizard
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
            this.wizardPageShopAddress = new ShipWorks.UI.Wizard.WizardPage();
            this.label1 = new System.Windows.Forms.Label();
            this.labelShopifyCom = new System.Windows.Forms.Label();
            this.shopUrlName = new System.Windows.Forms.TextBox();
            this.labelAddress = new System.Windows.Forms.Label();
            this.wizardPageAuthenticate = new ShipWorks.UI.Wizard.WizardPage();
            this.panelBrowser = new System.Windows.Forms.Panel();
            this.webBrowser = new ExtendedWebBrowser();
            this.wizardPageSuccess = new ShipWorks.UI.Wizard.WizardPage();
            this.imageStatus = new System.Windows.Forms.PictureBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageShopAddress.SuspendLayout();
            this.wizardPageAuthenticate.SuspendLayout();
            this.panelBrowser.SuspendLayout();
            this.wizardPageSuccess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(565, 538);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(646, 538);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(484, 538);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageAuthenticate);
            this.mainPanel.Size = new System.Drawing.Size(733, 466);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 528);
            this.etchBottom.Size = new System.Drawing.Size(737, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.sw_cubes_big;
            this.pictureBox.Location = new System.Drawing.Point(680, 3);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(733, 56);
            // 
            // wizardPageShopAddress
            // 
            this.wizardPageShopAddress.Controls.Add(this.label1);
            this.wizardPageShopAddress.Controls.Add(this.labelShopifyCom);
            this.wizardPageShopAddress.Controls.Add(this.shopUrlName);
            this.wizardPageShopAddress.Controls.Add(this.labelAddress);
            this.wizardPageShopAddress.Description = "Enter the the shop name of your Shopify store.";
            this.wizardPageShopAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageShopAddress.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageShopAddress.Location = new System.Drawing.Point(0, 0);
            this.wizardPageShopAddress.Name = "wizardPageShopAddress";
            this.wizardPageShopAddress.Size = new System.Drawing.Size(733, 466);
            this.wizardPageShopAddress.TabIndex = 0;
            this.wizardPageShopAddress.Title = "Shopify Shop Address";
            this.wizardPageShopAddress.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextShopAddress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "http://";
            // 
            // labelShopifyCom
            // 
            this.labelShopifyCom.AutoSize = true;
            this.labelShopifyCom.Location = new System.Drawing.Point(317, 37);
            this.labelShopifyCom.Name = "labelShopifyCom";
            this.labelShopifyCom.Size = new System.Drawing.Size(83, 13);
            this.labelShopifyCom.TabIndex = 8;
            this.labelShopifyCom.Text = ".myshopify.com";
            // 
            // shopUrlName
            // 
            this.shopUrlName.Location = new System.Drawing.Point(78, 34);
            this.shopUrlName.Name = "shopUrlName";
            this.shopUrlName.Size = new System.Drawing.Size(238, 21);
            this.shopUrlName.TabIndex = 7;
            // 
            // labelAddress
            // 
            this.labelAddress.AutoSize = true;
            this.labelAddress.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAddress.Location = new System.Drawing.Point(24, 10);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(192, 13);
            this.labelAddress.TabIndex = 6;
            this.labelAddress.Text = "Fill in your Shopify Shop Address:";
            // 
            // wizardPageAuthenticate
            // 
            this.wizardPageAuthenticate.Controls.Add(this.panelBrowser);
            this.wizardPageAuthenticate.Description = "Login to allow ShipWorks to connect to Shopify.";
            this.wizardPageAuthenticate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageAuthenticate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageAuthenticate.Location = new System.Drawing.Point(0, 0);
            this.wizardPageAuthenticate.Name = "wizardPageAuthenticate";
            this.wizardPageAuthenticate.Size = new System.Drawing.Size(733, 466);
            this.wizardPageAuthenticate.TabIndex = 0;
            this.wizardPageAuthenticate.Title = "Login to Shopify";
            this.wizardPageAuthenticate.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoAuthenticatePage);
            this.wizardPageAuthenticate.PageShown += new System.EventHandler<ShipWorks.UI.Wizard.WizardPageShownEventArgs>(this.OnPageShownAuthenticatePage);
            // 
            // panelBrowser
            // 
            this.panelBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBrowser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelBrowser.Controls.Add(this.webBrowser);
            this.panelBrowser.Location = new System.Drawing.Point(23, 7);
            this.panelBrowser.Name = "panelBrowser";
            this.panelBrowser.Size = new System.Drawing.Size(688, 445);
            this.panelBrowser.TabIndex = 1;
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.Size = new System.Drawing.Size(684, 441);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.OnWebBrowserNavigated);
            // 
            // wizardPageSuccess
            // 
            this.wizardPageSuccess.Controls.Add(this.imageStatus);
            this.wizardPageSuccess.Controls.Add(this.labelStatus);
            this.wizardPageSuccess.Description = "ShipWorks can now connect to your Shopify store.";
            this.wizardPageSuccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageSuccess.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageSuccess.Location = new System.Drawing.Point(0, 0);
            this.wizardPageSuccess.Name = "wizardPageSuccess";
            this.wizardPageSuccess.Size = new System.Drawing.Size(756, 524);
            this.wizardPageSuccess.TabIndex = 0;
            this.wizardPageSuccess.Title = "Login Successful";
            // 
            // imageStatus
            // 
            this.imageStatus.Image = global::ShipWorks.Properties.Resources.check16;
            this.imageStatus.Location = new System.Drawing.Point(26, 10);
            this.imageStatus.Name = "imageStatus";
            this.imageStatus.Size = new System.Drawing.Size(16, 16);
            this.imageStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageStatus.TabIndex = 25;
            this.imageStatus.TabStop = false;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.ForeColor = System.Drawing.Color.DimGray;
            this.labelStatus.Location = new System.Drawing.Point(44, 11);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(250, 13);
            this.labelStatus.TabIndex = 24;
            this.labelStatus.Text = "ShipWorks can now connect to your Shopify store!";
            // 
            // ShopifyCreateTokenWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 573);
            this.MinimumSize = new System.Drawing.Size(575, 450);
            this.Name = "ShopifyCreateTokenWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageShopAddress,
            this.wizardPageAuthenticate,
            this.wizardPageSuccess});
            this.Text = "Shopify Login Token";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageShopAddress.ResumeLayout(false);
            this.wizardPageShopAddress.PerformLayout();
            this.wizardPageAuthenticate.ResumeLayout(false);
            this.panelBrowser.ResumeLayout(false);
            this.wizardPageSuccess.ResumeLayout(false);
            this.wizardPageSuccess.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageSuccess;
        private UI.Wizard.WizardPage wizardPageShopAddress;
        private UI.Wizard.WizardPage wizardPageAuthenticate;
        private System.Windows.Forms.Label labelShopifyCom;
        private System.Windows.Forms.TextBox shopUrlName;
        private System.Windows.Forms.Label labelAddress;
        private System.Windows.Forms.Panel panelBrowser;
        private ExtendedWebBrowser webBrowser;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.PictureBox imageStatus;
        private System.Windows.Forms.Label label1;
    }
}
