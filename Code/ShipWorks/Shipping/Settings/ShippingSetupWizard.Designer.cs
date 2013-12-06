namespace ShipWorks.Shipping.Settings
{
    partial class ShippingSetupWizard
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
            this.wizardPagePackingSlip = new ShipWorks.UI.Wizard.WizardPage();
            this.wizardPageCarrier = new ShipWorks.UI.Wizard.WizardPage();
            this.picturePackingSlip = new System.Windows.Forms.PictureBox();
            this.includePackingSlip = new System.Windows.Forms.CheckBox();
            this.labelPackingSlip = new System.Windows.Forms.Label();
            this.labelPicturePackingSlip = new System.Windows.Forms.PictureBox();
            this.wizardPage1 = new ShipWorks.UI.Wizard.WizardPage();
            this.labelShippingHelp = new System.Windows.Forms.Label();
            this.comboShippingCarrier = new ShipWorks.UI.Controls.ImageComboBox();
            this.labelSelectCarriers = new System.Windows.Forms.Label();
            this.pictureTruck = new System.Windows.Forms.PictureBox();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPagePackingSlip.SuspendLayout();
            this.wizardPageCarrier.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePackingSlip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.labelPicturePackingSlip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTruck)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageCarrier);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.box_closed;
            this.pictureBox.Location = new System.Drawing.Point(469, 4);
            this.pictureBox.Size = new System.Drawing.Size(48, 48);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            // 
            // wizardPagePackingSlip
            // 
            this.wizardPagePackingSlip.Controls.Add(this.picturePackingSlip);
            this.wizardPagePackingSlip.Controls.Add(this.includePackingSlip);
            this.wizardPagePackingSlip.Controls.Add(this.labelPackingSlip);
            this.wizardPagePackingSlip.Controls.Add(this.labelPicturePackingSlip);
            this.wizardPagePackingSlip.Description = "Configure ShipWorks to print packing slips with shipping labels.";
            this.wizardPagePackingSlip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPagePackingSlip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPagePackingSlip.Location = new System.Drawing.Point(0, 0);
            this.wizardPagePackingSlip.Name = "wizardPagePackingSlip";
            this.wizardPagePackingSlip.Size = new System.Drawing.Size(526, 278);
            this.wizardPagePackingSlip.TabIndex = 0;
            this.wizardPagePackingSlip.Title = "Packing Slips";
            this.wizardPagePackingSlip.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoPackingSlips);
            // 
            // wizardPageCarrier
            // 
            this.wizardPageCarrier.Controls.Add(this.labelShippingHelp);
            this.wizardPageCarrier.Controls.Add(this.comboShippingCarrier);
            this.wizardPageCarrier.Controls.Add(this.labelSelectCarriers);
            this.wizardPageCarrier.Controls.Add(this.pictureTruck);
            this.wizardPageCarrier.Description = "Select the shipping carriers you\'ll use to ship.";
            this.wizardPageCarrier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageCarrier.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageCarrier.Location = new System.Drawing.Point(0, 0);
            this.wizardPageCarrier.Name = "wizardPageCarrier";
            this.wizardPageCarrier.Size = new System.Drawing.Size(526, 278);
            this.wizardPageCarrier.TabIndex = 0;
            this.wizardPageCarrier.Title = "Shipping Setup";
            // 
            // picturePackingSlip
            // 
            this.picturePackingSlip.Image = global::ShipWorks.Properties.Resources.document_plain_shipping_labels;
            this.picturePackingSlip.Location = new System.Drawing.Point(92, 56);
            this.picturePackingSlip.Name = "picturePackingSlip";
            this.picturePackingSlip.Size = new System.Drawing.Size(175, 175);
            this.picturePackingSlip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picturePackingSlip.TabIndex = 71;
            this.picturePackingSlip.TabStop = false;
            // 
            // includePackingSlip
            // 
            this.includePackingSlip.AutoSize = true;
            this.includePackingSlip.Checked = true;
            this.includePackingSlip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.includePackingSlip.Location = new System.Drawing.Point(81, 31);
            this.includePackingSlip.Name = "includePackingSlip";
            this.includePackingSlip.Size = new System.Drawing.Size(239, 17);
            this.includePackingSlip.TabIndex = 70;
            this.includePackingSlip.Text = "Print a packing slip with every shipping label.";
            this.includePackingSlip.UseVisualStyleBackColor = true;
            this.includePackingSlip.CheckedChanged += new System.EventHandler(this.OnChangeIncludePackingSlip);
            // 
            // labelPackingSlip
            // 
            this.labelPackingSlip.AutoSize = true;
            this.labelPackingSlip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPackingSlip.Location = new System.Drawing.Point(78, 11);
            this.labelPackingSlip.Name = "labelPackingSlip";
            this.labelPackingSlip.Size = new System.Drawing.Size(80, 13);
            this.labelPackingSlip.TabIndex = 69;
            this.labelPackingSlip.Text = "Packing Slips";
            // 
            // labelPicturePackingSlip
            // 
            this.labelPicturePackingSlip.Image = global::ShipWorks.Properties.Resources.form_blue1;
            this.labelPicturePackingSlip.Location = new System.Drawing.Point(23, 9);
            this.labelPicturePackingSlip.Name = "labelPicturePackingSlip";
            this.labelPicturePackingSlip.Size = new System.Drawing.Size(48, 48);
            this.labelPicturePackingSlip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.labelPicturePackingSlip.TabIndex = 68;
            this.labelPicturePackingSlip.TabStop = false;
            // 
            // wizardPage1
            // 
            this.wizardPage1.Description = "The description of the page.";
            this.wizardPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPage1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPage1.Location = new System.Drawing.Point(0, 0);
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.Size = new System.Drawing.Size(526, 278);
            this.wizardPage1.TabIndex = 0;
            this.wizardPage1.Title = "Wizard page 3.";
            // 
            // labelShippingHelp
            // 
            this.labelShippingHelp.AutoSize = true;
            this.labelShippingHelp.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelShippingHelp.Location = new System.Drawing.Point(94, 57);
            this.labelShippingHelp.Name = "labelShippingHelp";
            this.labelShippingHelp.Size = new System.Drawing.Size(338, 13);
            this.labelShippingHelp.TabIndex = 55;
            this.labelShippingHelp.Text = "(If you use more than one just pick one. It\'s easy to add more later.)";
            // 
            // comboShippingCarrier
            // 
            this.comboShippingCarrier.FormattingEnabled = true;
            this.comboShippingCarrier.Items.AddRange(new object[] {
            "US Postal Service",
            "FedEx",
            "UPS",
            "Something else..."});
            this.comboShippingCarrier.Location = new System.Drawing.Point(97, 30);
            this.comboShippingCarrier.MaxDropDownItems = 20;
            this.comboShippingCarrier.Name = "comboShippingCarrier";
            this.comboShippingCarrier.Size = new System.Drawing.Size(223, 21);
            this.comboShippingCarrier.TabIndex = 54;
            // 
            // labelSelectCarriers
            // 
            this.labelSelectCarriers.AutoSize = true;
            this.labelSelectCarriers.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSelectCarriers.Location = new System.Drawing.Point(79, 10);
            this.labelSelectCarriers.Name = "labelSelectCarriers";
            this.labelSelectCarriers.Size = new System.Drawing.Size(198, 13);
            this.labelSelectCarriers.TabIndex = 53;
            this.labelSelectCarriers.Text = "What shipping carrier do you use?";
            // 
            // pictureTruck
            // 
            this.pictureTruck.Image = global::ShipWorks.Properties.Resources.truck_blue1;
            this.pictureTruck.Location = new System.Drawing.Point(23, 10);
            this.pictureTruck.Name = "pictureTruck";
            this.pictureTruck.Size = new System.Drawing.Size(48, 48);
            this.pictureTruck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureTruck.TabIndex = 52;
            this.pictureTruck.TabStop = false;
            // 
            // ShippingSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ShippingSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageCarrier,
            this.wizardPagePackingSlip,
            this.wizardPage1});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Shipping Setup";
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPagePackingSlip.ResumeLayout(false);
            this.wizardPagePackingSlip.PerformLayout();
            this.wizardPageCarrier.ResumeLayout(false);
            this.wizardPageCarrier.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePackingSlip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.labelPicturePackingSlip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTruck)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPagePackingSlip;
        private UI.Wizard.WizardPage wizardPageCarrier;
        private System.Windows.Forms.PictureBox picturePackingSlip;
        private System.Windows.Forms.CheckBox includePackingSlip;
        private System.Windows.Forms.Label labelPackingSlip;
        private System.Windows.Forms.PictureBox labelPicturePackingSlip;
        private UI.Wizard.WizardPage wizardPage1;
        private System.Windows.Forms.Label labelShippingHelp;
        private UI.Controls.ImageComboBox comboShippingCarrier;
        private System.Windows.Forms.Label labelSelectCarriers;
        private System.Windows.Forms.PictureBox pictureTruck;

    }
}