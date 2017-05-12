namespace ShipWorks.Shipping.Carriers.Postal
{
    partial class PostalServiceControlBase
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
            this.dimensionsControl = new ShipWorks.Shipping.Editing.DimensionsControl();
            this.nonMachinable = new System.Windows.Forms.CheckBox();
            this.nonRectangular = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.packagingType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPackaging = new System.Windows.Forms.Label();
            this.confirmation = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelService = new System.Windows.Forms.Label();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.labelWeight = new System.Windows.Forms.Label();
            this.labelShipDate = new System.Windows.Forms.Label();
            this.shipDate = new ShipWorks.UI.Controls.MultiValueDateTimePicker();
            this.labelConfirmation = new System.Windows.Forms.Label();
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceSelectionControl();
            this.sectionExpress = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.expressSignatureWaiver = new System.Windows.Forms.CheckBox();
            this.labelExpressSignatureWaiver = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient.ContentPanel)).BeginInit();
            this.sectionRecipient.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns.ContentPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment.ContentPanel)).BeginInit();
            this.sectionShipment.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionExpress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionExpress.ContentPanel)).BeginInit();
            this.sectionExpress.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // sectionRecipient
            //
            this.sectionRecipient.Location = new System.Drawing.Point(3, 5);
            this.sectionRecipient.Size = new System.Drawing.Size(390, 24);
            //
            // personControl
            //
            this.personControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.personControl.Size = new System.Drawing.Size(395, 330);
            //
            // sectionReturns
            //
            this.sectionReturns.Location = new System.Drawing.Point(3, 383);
            this.sectionReturns.Size = new System.Drawing.Size(390, 24);
            //
            // sectionShipment
            //
            //
            // sectionShipment.ContentPanel
            //
            this.sectionShipment.ContentPanel.Controls.Add(this.insuranceControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.dimensionsControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelService);
            this.sectionShipment.ContentPanel.Controls.Add(this.nonMachinable);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelConfirmation);
            this.sectionShipment.ContentPanel.Controls.Add(this.nonRectangular);
            this.sectionShipment.ContentPanel.Controls.Add(this.shipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.label3);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelShipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.packagingType);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelWeight);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelPackaging);
            this.sectionShipment.ContentPanel.Controls.Add(this.weight);
            this.sectionShipment.ContentPanel.Controls.Add(this.confirmation);
            this.sectionShipment.ContentPanel.Controls.Add(this.service);
            this.sectionShipment.Location = new System.Drawing.Point(3, 34);
            this.sectionShipment.Size = new System.Drawing.Size(390, 315);
            //
            // dimensionsControl
            //
            this.dimensionsControl.BackColor = System.Drawing.Color.Transparent;
            this.dimensionsControl.Cleared = false;
            this.dimensionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dimensionsControl.Location = new System.Drawing.Point(87, 161);
            this.dimensionsControl.Name = "dimensionsControl";
            this.dimensionsControl.Size = new System.Drawing.Size(210, 74);
            this.dimensionsControl.TabIndex = 12;
            this.dimensionsControl.DimensionsChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            this.dimensionsControl.DimensionsChanged += OnShipSenseFieldChanged;
            //
            // nonMachinable
            //
            this.nonMachinable.AutoSize = true;
            this.nonMachinable.BackColor = System.Drawing.Color.Transparent;
            this.nonMachinable.Location = new System.Drawing.Point(90, 143);
            this.nonMachinable.Name = "nonMachinable";
            this.nonMachinable.Size = new System.Drawing.Size(102, 17);
            this.nonMachinable.TabIndex = 9;
            this.nonMachinable.Text = "Non-Machinable";
            this.nonMachinable.UseVisualStyleBackColor = false;
            this.nonMachinable.CheckedChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            //
            // nonRectangular
            //
            this.nonRectangular.AutoSize = true;
            this.nonRectangular.BackColor = System.Drawing.Color.Transparent;
            this.nonRectangular.Location = new System.Drawing.Point(192, 143);
            this.nonRectangular.Name = "nonRectangular";
            this.nonRectangular.Size = new System.Drawing.Size(107, 17);
            this.nonRectangular.TabIndex = 10;
            this.nonRectangular.Text = "Non-Rectangular";
            this.nonRectangular.UseVisualStyleBackColor = false;
            this.nonRectangular.CheckedChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(20, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Dimensions:";
            //
            // packagingType
            //
            this.packagingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.packagingType.FormattingEnabled = true;
            this.packagingType.Location = new System.Drawing.Point(90, 117);
            this.packagingType.Name = "packagingType";
            this.packagingType.PromptText = "(Multiple Values)";
            this.packagingType.Size = new System.Drawing.Size(160, 21);
            this.packagingType.TabIndex = 8;
            this.packagingType.SelectedIndexChanged += new System.EventHandler(this.OnChangePackaging);
            //
            // labelPackaging
            //
            this.labelPackaging.AutoSize = true;
            this.labelPackaging.BackColor = System.Drawing.Color.Transparent;
            this.labelPackaging.Location = new System.Drawing.Point(25, 120);
            this.labelPackaging.Name = "labelPackaging";
            this.labelPackaging.Size = new System.Drawing.Size(59, 13);
            this.labelPackaging.TabIndex = 49;
            this.labelPackaging.Text = "Packaging:";
            //
            // confirmation
            //
            this.confirmation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.confirmation.FormattingEnabled = true;
            this.confirmation.Location = new System.Drawing.Point(90, 37);
            this.confirmation.Name = "confirmation";
            this.confirmation.PromptText = "(Multiple Values)";
            this.confirmation.Size = new System.Drawing.Size(300, 21);
            this.confirmation.TabIndex = 3;
            this.confirmation.SelectedIndexChanged += new System.EventHandler(this.OnConfirmationChanged);
            //
            // service
            //
            this.service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.service.FormattingEnabled = true;
            this.service.Location = new System.Drawing.Point(90, 10);
            this.service.Name = "service";
            this.service.PromptText = "(Multiple Values)";
            this.service.Size = new System.Drawing.Size(300, 21);
            this.service.TabIndex = 1;
            this.service.SelectedIndexChanged += new System.EventHandler(this.OnConfirmationChanged);
            //
            // labelService
            //
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.Transparent;
            this.labelService.Location = new System.Drawing.Point(38, 13);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 0;
            this.labelService.Text = "Service:";
            //
            // weight
            //
            this.weight.AutoSize = true;
            this.weight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weight.Location = new System.Drawing.Point(90, 91);
            this.weight.Name = "weight";
            this.weight.RangeMax = 9999D;
            this.weight.RangeMin = 0D;
            this.weight.ShowShortcutInfo = true;
            this.weight.Size = new System.Drawing.Size(238, 24);
            this.weight.TabIndex = 7;
            this.weight.Weight = 0D;
            this.weight.WeightChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            this.weight.WeightChanged += OnShipSenseFieldChanged;
            //
            // labelWeight
            //
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(39, 94);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 6;
            this.labelWeight.Text = "Weight:";
            //
            // labelShipDate
            //
            this.labelShipDate.AutoSize = true;
            this.labelShipDate.BackColor = System.Drawing.Color.Transparent;
            this.labelShipDate.Location = new System.Drawing.Point(28, 68);
            this.labelShipDate.Name = "labelShipDate";
            this.labelShipDate.Size = new System.Drawing.Size(56, 13);
            this.labelShipDate.TabIndex = 4;
            this.labelShipDate.Text = "Ship date:";
            //
            // shipDate
            //
            this.shipDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.shipDate.Location = new System.Drawing.Point(90, 64);
            this.shipDate.Name = "shipDate";
            this.shipDate.Size = new System.Drawing.Size(124, 21);
            this.shipDate.TabIndex = 5;
            //
            // labelConfirmation
            //
            this.labelConfirmation.AutoSize = true;
            this.labelConfirmation.BackColor = System.Drawing.Color.Transparent;
            this.labelConfirmation.Location = new System.Drawing.Point(14, 40);
            this.labelConfirmation.Name = "labelConfirmation";
            this.labelConfirmation.Size = new System.Drawing.Size(72, 13);
            this.labelConfirmation.TabIndex = 2;
            this.labelConfirmation.Text = "Confirmation:";
            //
            // insuranceControl
            //
            this.insuranceControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.insuranceControl.BackColor = System.Drawing.Color.Transparent;
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceControl.Location = new System.Drawing.Point(21, 236);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(464, 50);
            this.insuranceControl.TabIndex = 50;
            this.insuranceControl.InsuranceOptionsChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            //
            // sectionExpress
            //
            this.sectionExpress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionExpress.Collapsed = true;
            //
            // sectionExpress.ContentPanel
            //
            this.sectionExpress.ContentPanel.Controls.Add(this.expressSignatureWaiver);
            this.sectionExpress.ContentPanel.Controls.Add(this.labelExpressSignatureWaiver);
            this.sectionExpress.ExpandedHeight = 62;
            this.sectionExpress.ExtraText = "";
            this.sectionExpress.Location = new System.Drawing.Point(3, 354);
            this.sectionExpress.Name = "sectionExpress";
            this.sectionExpress.SectionName = "Priority Mail Express";
            this.sectionExpress.SettingsKey = "{1883602a-4bec-4004-9ce5-7fba05b99e17}";
            this.sectionExpress.Size = new System.Drawing.Size(390, 24);
            this.sectionExpress.TabIndex = 12;
            //
            // expressSignatureWaiver
            //
            this.expressSignatureWaiver.AutoSize = true;
            this.expressSignatureWaiver.BackColor = System.Drawing.Color.Transparent;
            this.expressSignatureWaiver.Location = new System.Drawing.Point(121, 8);
            this.expressSignatureWaiver.Name = "expressSignatureWaiver";
            this.expressSignatureWaiver.Size = new System.Drawing.Size(165, 17);
            this.expressSignatureWaiver.TabIndex = 1;
            this.expressSignatureWaiver.Text = "Waive signature requirement";
            this.expressSignatureWaiver.UseVisualStyleBackColor = false;
            //
            // labelExpressSignatureWaiver
            //
            this.labelExpressSignatureWaiver.AutoSize = true;
            this.labelExpressSignatureWaiver.BackColor = System.Drawing.Color.Transparent;
            this.labelExpressSignatureWaiver.Location = new System.Drawing.Point(18, 9);
            this.labelExpressSignatureWaiver.Name = "labelExpressSignatureWaiver";
            this.labelExpressSignatureWaiver.Size = new System.Drawing.Size(97, 13);
            this.labelExpressSignatureWaiver.TabIndex = 0;
            this.labelExpressSignatureWaiver.Text = "Signature Waiver: ";
            //
            // PostalServiceControlBase
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sectionExpress);
            this.Name = "PostalServiceControlBase";
            this.Size = new System.Drawing.Size(396, 782);
            this.Controls.SetChildIndex(this.sectionReturns, 0);
            this.Controls.SetChildIndex(this.sectionExpress, 0);
            this.Controls.SetChildIndex(this.sectionShipment, 0);
            this.Controls.SetChildIndex(this.sectionRecipient, 0);
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient.ContentPanel)).EndInit();
            this.sectionRecipient.ContentPanel.ResumeLayout(false);
            this.sectionRecipient.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment.ContentPanel)).EndInit();
            this.sectionShipment.ContentPanel.ResumeLayout(false);
            this.sectionShipment.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionExpress.ContentPanel)).EndInit();
            this.sectionExpress.ContentPanel.ResumeLayout(false);
            this.sectionExpress.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionExpress)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.Shipping.Editing.DimensionsControl dimensionsControl;
        private System.Windows.Forms.CheckBox nonMachinable;
        private System.Windows.Forms.CheckBox nonRectangular;
        private System.Windows.Forms.Label label3;
        private ShipWorks.UI.Controls.MultiValueComboBox packagingType;
        private System.Windows.Forms.Label labelPackaging;
        private ShipWorks.UI.Controls.MultiValueComboBox confirmation;
        private System.Windows.Forms.Label labelService;
        private ShipWorks.UI.Controls.WeightControl weight;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.Label labelShipDate;
        private ShipWorks.UI.Controls.MultiValueDateTimePicker shipDate;
        private System.Windows.Forms.Label labelConfirmation;
        protected Insurance.InsuranceSelectionControl insuranceControl;
        private System.Windows.Forms.CheckBox expressSignatureWaiver;
        private System.Windows.Forms.Label labelExpressSignatureWaiver;
        protected UI.Controls.CollapsibleGroupControl sectionExpress;
        protected UI.Controls.MultiValueComboBox service;
    }
}
