namespace ShipWorks.Shipping.Carriers.UPS
{
    partial class UpsAccountRateTypeControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpsAccountRateTypeControl));
            this.pictureBoxRateWarning = new System.Windows.Forms.PictureBox();
            this.labelRateWarning = new System.Windows.Forms.Label();
            this.labelRateType = new System.Windows.Forms.Label();
            this.rateType = new System.Windows.Forms.ComboBox();
            this.panelInvoiceAuthorizationHolder = new System.Windows.Forms.FlowLayoutPanel();
            this.panelAuthorizationInstructions = new System.Windows.Forms.Panel();
            this.labelNegotiated7 = new System.Windows.Forms.Label();
            this.labelNegotiated6 = new System.Windows.Forms.Label();
            this.labelNegotiated1 = new System.Windows.Forms.Label();
            this.labelNegotiated3 = new System.Windows.Forms.Label();
            this.labelNegotiated5 = new System.Windows.Forms.Label();
            this.labelNegotiated4 = new System.Windows.Forms.Label();
            this.labelNegotiated2 = new System.Windows.Forms.Label();
            this.panelNewAccount = new System.Windows.Forms.Panel();
            this.labelNewAccount = new System.Windows.Forms.Label();
            this.invoiceAuthNeededPanel = new System.Windows.Forms.Panel();
            this.invoiceAuthNeededLabel = new System.Windows.Forms.Label();
            this.authorizationControl = new ShipWorks.Shipping.Carriers.UPS.UpsInvoiceAuthorizationControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRateWarning)).BeginInit();
            this.panelInvoiceAuthorizationHolder.SuspendLayout();
            this.panelAuthorizationInstructions.SuspendLayout();
            this.panelNewAccount.SuspendLayout();
            this.invoiceAuthNeededPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxRateWarning
            // 
            this.pictureBoxRateWarning.Image = global::ShipWorks.Properties.Resources.exclamation16;
            this.pictureBoxRateWarning.Location = new System.Drawing.Point(6, 10);
            this.pictureBoxRateWarning.Name = "pictureBoxRateWarning";
            this.pictureBoxRateWarning.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxRateWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxRateWarning.TabIndex = 16;
            this.pictureBoxRateWarning.TabStop = false;
            // 
            // labelRateWarning
            // 
            this.labelRateWarning.Location = new System.Drawing.Point(25, 9);
            this.labelRateWarning.Name = "labelRateWarning";
            this.labelRateWarning.Size = new System.Drawing.Size(335, 45);
            this.labelRateWarning.TabIndex = 0;
            this.labelRateWarning.Text = "The option selected only affects rates displayed in the Rates section of the ship" +
    "ping window, and has no impact on the final cost of the shipment.";
            // 
            // labelRateType
            // 
            this.labelRateType.AutoSize = true;
            this.labelRateType.Location = new System.Drawing.Point(28, 63);
            this.labelRateType.Name = "labelRateType";
            this.labelRateType.Size = new System.Drawing.Size(61, 13);
            this.labelRateType.TabIndex = 17;
            this.labelRateType.Text = "Rate Type:";
            // 
            // rateType
            // 
            this.rateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rateType.FormattingEnabled = true;
            this.rateType.Location = new System.Drawing.Point(95, 60);
            this.rateType.Name = "rateType";
            this.rateType.Size = new System.Drawing.Size(246, 21);
            this.rateType.TabIndex = 18;
            this.rateType.SelectedIndexChanged += new System.EventHandler(this.OnRateTypeChanged);
            // 
            // panelInvoiceAuthorizationHolder
            // 
            this.panelInvoiceAuthorizationHolder.Controls.Add(this.panelAuthorizationInstructions);
            this.panelInvoiceAuthorizationHolder.Controls.Add(this.panelNewAccount);
            this.panelInvoiceAuthorizationHolder.Controls.Add(this.invoiceAuthNeededPanel);
            this.panelInvoiceAuthorizationHolder.Controls.Add(this.authorizationControl);
            this.panelInvoiceAuthorizationHolder.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelInvoiceAuthorizationHolder.Location = new System.Drawing.Point(0, 87);
            this.panelInvoiceAuthorizationHolder.Name = "panelInvoiceAuthorizationHolder";
            this.panelInvoiceAuthorizationHolder.Size = new System.Drawing.Size(377, 495);
            this.panelInvoiceAuthorizationHolder.TabIndex = 20;
            this.panelInvoiceAuthorizationHolder.Visible = false;
            // 
            // panelAuthorizationInstructions
            // 
            this.panelAuthorizationInstructions.Controls.Add(this.labelNegotiated7);
            this.panelAuthorizationInstructions.Controls.Add(this.labelNegotiated6);
            this.panelAuthorizationInstructions.Controls.Add(this.labelNegotiated1);
            this.panelAuthorizationInstructions.Controls.Add(this.labelNegotiated3);
            this.panelAuthorizationInstructions.Controls.Add(this.labelNegotiated5);
            this.panelAuthorizationInstructions.Controls.Add(this.labelNegotiated4);
            this.panelAuthorizationInstructions.Controls.Add(this.labelNegotiated2);
            this.panelAuthorizationInstructions.Location = new System.Drawing.Point(3, 3);
            this.panelAuthorizationInstructions.Name = "panelAuthorizationInstructions";
            this.panelAuthorizationInstructions.Size = new System.Drawing.Size(376, 140);
            this.panelAuthorizationInstructions.TabIndex = 20;
            // 
            // labelNegotiated7
            // 
            this.labelNegotiated7.ForeColor = System.Drawing.Color.Black;
            this.labelNegotiated7.Location = new System.Drawing.Point(34, 106);
            this.labelNegotiated7.Name = "labelNegotiated7";
            this.labelNegotiated7.Size = new System.Drawing.Size(302, 31);
            this.labelNegotiated7.TabIndex = 20;
            this.labelNegotiated7.Text = "You must validate your account by providing information from and invoice received" +
    " within the last 90 days.";
            // 
            // labelNegotiated6
            // 
            this.labelNegotiated6.AutoSize = true;
            this.labelNegotiated6.ForeColor = System.Drawing.Color.Black;
            this.labelNegotiated6.Location = new System.Drawing.Point(19, 106);
            this.labelNegotiated6.Name = "labelNegotiated6";
            this.labelNegotiated6.Size = new System.Drawing.Size(17, 13);
            this.labelNegotiated6.TabIndex = 19;
            this.labelNegotiated6.Text = "3.";
            // 
            // labelNegotiated1
            // 
            this.labelNegotiated1.ForeColor = System.Drawing.Color.Black;
            this.labelNegotiated1.Location = new System.Drawing.Point(4, 5);
            this.labelNegotiated1.Name = "labelNegotiated1";
            this.labelNegotiated1.Size = new System.Drawing.Size(323, 29);
            this.labelNegotiated1.TabIndex = 14;
            this.labelNegotiated1.Text = "To display UPS Negotiated Rates in ShipWorks the following conditions must be met" +
    ":";
            // 
            // labelNegotiated3
            // 
            this.labelNegotiated3.AutoSize = true;
            this.labelNegotiated3.ForeColor = System.Drawing.Color.Black;
            this.labelNegotiated3.Location = new System.Drawing.Point(34, 39);
            this.labelNegotiated3.Name = "labelNegotiated3";
            this.labelNegotiated3.Size = new System.Drawing.Size(327, 13);
            this.labelNegotiated3.TabIndex = 16;
            this.labelNegotiated3.Text = "Your UPS Account number must have a discount associated with it.";
            // 
            // labelNegotiated5
            // 
            this.labelNegotiated5.ForeColor = System.Drawing.Color.Black;
            this.labelNegotiated5.Location = new System.Drawing.Point(34, 68);
            this.labelNegotiated5.Name = "labelNegotiated5";
            this.labelNegotiated5.Size = new System.Drawing.Size(339, 38);
            this.labelNegotiated5.TabIndex = 18;
            this.labelNegotiated5.Text = "Your UPS Account number must be approved for display of Negotiated Rates.";
            // 
            // labelNegotiated4
            // 
            this.labelNegotiated4.AutoSize = true;
            this.labelNegotiated4.ForeColor = System.Drawing.Color.Black;
            this.labelNegotiated4.Location = new System.Drawing.Point(19, 68);
            this.labelNegotiated4.Name = "labelNegotiated4";
            this.labelNegotiated4.Size = new System.Drawing.Size(17, 13);
            this.labelNegotiated4.TabIndex = 17;
            this.labelNegotiated4.Text = "2.";
            // 
            // labelNegotiated2
            // 
            this.labelNegotiated2.AutoSize = true;
            this.labelNegotiated2.ForeColor = System.Drawing.Color.Black;
            this.labelNegotiated2.Location = new System.Drawing.Point(19, 39);
            this.labelNegotiated2.Name = "labelNegotiated2";
            this.labelNegotiated2.Size = new System.Drawing.Size(17, 13);
            this.labelNegotiated2.TabIndex = 15;
            this.labelNegotiated2.Text = "1.";
            // 
            // panelNewAccount
            // 
            this.panelNewAccount.Controls.Add(this.labelNewAccount);
            this.panelNewAccount.Location = new System.Drawing.Point(3, 149);
            this.panelNewAccount.Name = "panelNewAccount";
            this.panelNewAccount.Size = new System.Drawing.Size(373, 79);
            this.panelNewAccount.TabIndex = 22;
            this.panelNewAccount.Visible = false;
            // 
            // labelNewAccount
            // 
            this.labelNewAccount.Location = new System.Drawing.Point(3, 4);
            this.labelNewAccount.Name = "labelNewAccount";
            this.labelNewAccount.Size = new System.Drawing.Size(367, 42);
            this.labelNewAccount.TabIndex = 0;
            this.labelNewAccount.Text = "Negotiated Rates are not available when setting up a new account. Talk to a UPS r" +
    "epresentative to negotiate rates and go to your account settings to enable negot" +
    "iated rates in ShipWorks.";
            // 
            // invoiceAuthNeededPanel
            // 
            this.invoiceAuthNeededPanel.Controls.Add(this.invoiceAuthNeededLabel);
            this.invoiceAuthNeededPanel.Location = new System.Drawing.Point(3, 234);
            this.invoiceAuthNeededPanel.Name = "invoiceAuthNeededPanel";
            this.invoiceAuthNeededPanel.Size = new System.Drawing.Size(374, 70);
            this.invoiceAuthNeededPanel.TabIndex = 23;
            // 
            // invoiceAuthNeededLabel
            // 
            this.invoiceAuthNeededLabel.Location = new System.Drawing.Point(3, 3);
            this.invoiceAuthNeededLabel.Name = "invoiceAuthNeededLabel";
            this.invoiceAuthNeededLabel.Size = new System.Drawing.Size(367, 54);
            this.invoiceAuthNeededLabel.TabIndex = 0;
            this.invoiceAuthNeededLabel.Text = resources.GetString("invoiceAuthNeededLabel.Text");
            // 
            // authorizationControl
            // 
            this.authorizationControl.Location = new System.Drawing.Point(3, 310);
            this.authorizationControl.Name = "authorizationControl";
            this.authorizationControl.Size = new System.Drawing.Size(372, 128);
            this.authorizationControl.TabIndex = 10;
            this.authorizationControl.Visible = false;
            // 
            // UpsAccountRateTypeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelInvoiceAuthorizationHolder);
            this.Controls.Add(this.rateType);
            this.Controls.Add(this.labelRateType);
            this.Controls.Add(this.pictureBoxRateWarning);
            this.Controls.Add(this.labelRateWarning);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UpsAccountRateTypeControl";
            this.Size = new System.Drawing.Size(379, 587);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRateWarning)).EndInit();
            this.panelInvoiceAuthorizationHolder.ResumeLayout(false);
            this.panelAuthorizationInstructions.ResumeLayout(false);
            this.panelAuthorizationInstructions.PerformLayout();
            this.panelNewAccount.ResumeLayout(false);
            this.invoiceAuthNeededPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxRateWarning;
        private System.Windows.Forms.Label labelRateWarning;
        private System.Windows.Forms.Label labelRateType;
        private System.Windows.Forms.ComboBox rateType;
        private System.Windows.Forms.FlowLayoutPanel panelInvoiceAuthorizationHolder;
        private UpsInvoiceAuthorizationControl authorizationControl;
        private System.Windows.Forms.Panel panelAuthorizationInstructions;
        private System.Windows.Forms.Label labelNegotiated7;
        private System.Windows.Forms.Label labelNegotiated6;
        private System.Windows.Forms.Label labelNegotiated1;
        private System.Windows.Forms.Label labelNegotiated3;
        private System.Windows.Forms.Label labelNegotiated5;
        private System.Windows.Forms.Label labelNegotiated4;
        private System.Windows.Forms.Label labelNegotiated2;
        private System.Windows.Forms.Panel panelNewAccount;
        private System.Windows.Forms.Label labelNewAccount;
        private System.Windows.Forms.Panel invoiceAuthNeededPanel;
        private System.Windows.Forms.Label invoiceAuthNeededLabel;
    }
}
