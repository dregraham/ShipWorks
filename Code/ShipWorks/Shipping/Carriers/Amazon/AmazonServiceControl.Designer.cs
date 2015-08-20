namespace ShipWorks.Shipping.Carriers.Amazon
{
    partial class AmazonServiceControl
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
            this.components = new System.ComponentModel.Container();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.label1 = new System.Windows.Forms.Label();
            this.cost = new ShipWorks.UI.Controls.MoneyTextBox();
            this.tracking = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelTracking = new System.Windows.Forms.Label();
            this.labelCost = new System.Windows.Forms.Label();
            this.labelShipDate = new System.Windows.Forms.Label();
            this.shipDate = new ShipWorks.UI.Controls.MultiValueDateTimePicker();
            this.service = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelService = new System.Windows.Forms.Label();
            this.carrier = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelCarrier = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.sectionFrom = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.originControl = new ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceSelectionControl();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).BeginInit();
            this.sectionRecipient.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).BeginInit();
            this.sectionShipment.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom)).BeginInit();
            this.sectionFrom.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // sectionRecipient
            // 
            this.sectionRecipient.Location = new System.Drawing.Point(3, 34);
            this.sectionRecipient.Size = new System.Drawing.Size(398, 24);
            this.sectionRecipient.TabIndex = 1;
            // 
            // personControl
            // 
            this.personControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.personControl.Size = new System.Drawing.Size(391, 330);
            // 
            // sectionReturns
            // 
            this.sectionReturns.Location = new System.Drawing.Point(3, 314);
            this.sectionReturns.Size = new System.Drawing.Size(398, 24);

            this.sectionLabelOptions.Size = new System.Drawing.Size(398, 24);
            this.sectionLabelOptions.Visible = false;
            // 
            // sectionShipment
            // 
            // 
            // sectionShipment.ContentPanel
            // 
            this.sectionShipment.ContentPanel.Controls.Add(this.insuranceControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.weight);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelCarrier);
            this.sectionShipment.ContentPanel.Controls.Add(this.label1);
            this.sectionShipment.ContentPanel.Controls.Add(this.carrier);
            this.sectionShipment.ContentPanel.Controls.Add(this.cost);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelService);
            this.sectionShipment.ContentPanel.Controls.Add(this.tracking);
            this.sectionShipment.ContentPanel.Controls.Add(this.service);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelTracking);
            this.sectionShipment.ContentPanel.Controls.Add(this.shipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelCost);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelShipDate);
            this.sectionShipment.Location = new System.Drawing.Point(3, 63);
            this.sectionShipment.Size = new System.Drawing.Size(398, 246);
            // 
            // weight
            // 
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.weight.Location = new System.Drawing.Point(85, 86);
            this.weight.Name = "weight";
            this.weight.RangeMax = 300D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(218, 21);
            this.weight.TabIndex = 7;
            this.weight.Weight = 0D;
            this.weight.WeightChanged += OnShipSenseFieldChanged;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(34, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Weight:";
            // 
            // cost
            // 
            this.cost.Amount = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.cost.Location = new System.Drawing.Point(85, 113);
            this.cost.Name = "cost";
            this.cost.Size = new System.Drawing.Size(94, 21);
            this.cost.TabIndex = 9;
            this.cost.Text = "$0.00";
            // 
            // tracking
            // 
            this.tracking.Location = new System.Drawing.Point(85, 140);
            this.fieldLengthProvider.SetMaxLengthSource(this.tracking, ShipWorks.Data.Utility.EntityFieldLengthSource.ShipmentTracking);
            this.tracking.Name = "tracking";
            this.tracking.Size = new System.Drawing.Size(198, 21);
            this.tracking.TabIndex = 11;
            // 
            // labelTracking
            // 
            this.labelTracking.AutoSize = true;
            this.labelTracking.BackColor = System.Drawing.Color.Transparent;
            this.labelTracking.Location = new System.Drawing.Point(17, 143);
            this.labelTracking.Name = "labelTracking";
            this.labelTracking.Size = new System.Drawing.Size(62, 13);
            this.labelTracking.TabIndex = 10;
            this.labelTracking.Text = "Tracking #:";
            // 
            // labelCost
            // 
            this.labelCost.AutoSize = true;
            this.labelCost.BackColor = System.Drawing.Color.Transparent;
            this.labelCost.Location = new System.Drawing.Point(46, 116);
            this.labelCost.Name = "labelCost";
            this.labelCost.Size = new System.Drawing.Size(33, 13);
            this.labelCost.TabIndex = 8;
            this.labelCost.Text = "Cost:";
            // 
            // labelShipDate
            // 
            this.labelShipDate.AutoSize = true;
            this.labelShipDate.BackColor = System.Drawing.Color.Transparent;
            this.labelShipDate.Location = new System.Drawing.Point(23, 65);
            this.labelShipDate.Name = "labelShipDate";
            this.labelShipDate.Size = new System.Drawing.Size(56, 13);
            this.labelShipDate.TabIndex = 4;
            this.labelShipDate.Text = "Ship date:";
            // 
            // shipDate
            // 
            this.shipDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.shipDate.Location = new System.Drawing.Point(85, 61);
            this.shipDate.Name = "shipDate";
            this.shipDate.Size = new System.Drawing.Size(124, 21);
            this.shipDate.TabIndex = 5;
            // 
            // service
            // 
            this.service.Location = new System.Drawing.Point(85, 34);
            this.fieldLengthProvider.SetMaxLengthSource(this.service, ShipWorks.Data.Utility.EntityFieldLengthSource.AmazonShipmentShippingServiceName);
            this.service.Name = "service";
            this.service.Size = new System.Drawing.Size(198, 21);
            this.service.TabIndex = 3;
            this.service.TextChanged += new System.EventHandler(this.OnShipmentDetailsChanged);
            // 
            // labelService
            // 
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.Transparent;
            this.labelService.Location = new System.Drawing.Point(33, 37);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 2;
            this.labelService.Text = "Service:";
            // 
            // carrier
            // 
            this.carrier.Location = new System.Drawing.Point(85, 8);
            this.fieldLengthProvider.SetMaxLengthSource(this.carrier, ShipWorks.Data.Utility.EntityFieldLengthSource.AmazonShipmentCarrierName);
            this.carrier.Name = "carrier";
            this.carrier.Size = new System.Drawing.Size(198, 21);
            this.carrier.TabIndex = 1;
            this.carrier.TextChanged += new System.EventHandler(this.OnShipmentDetailsChanged);
            // 
            // labelCarrier
            // 
            this.labelCarrier.AutoSize = true;
            this.labelCarrier.BackColor = System.Drawing.Color.Transparent;
            this.labelCarrier.Location = new System.Drawing.Point(8, 11);
            this.labelCarrier.Name = "labelCarrier";
            this.labelCarrier.Size = new System.Drawing.Size(73, 13);
            this.labelCarrier.TabIndex = 0;
            this.labelCarrier.Text = "Carrier name:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(159, 125);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // sectionFrom
            // 
            this.sectionFrom.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionFrom.Collapsed = true;
            // 
            // sectionFrom.ContentPanel
            // 
            this.sectionFrom.ContentPanel.Controls.Add(this.originControl);
            this.sectionFrom.ContentPanel.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.sectionFrom.ContentPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.sectionFrom.ExpandedHeight = 487;
            this.sectionFrom.ExtraText = "";
            this.sectionFrom.Location = new System.Drawing.Point(3, 5);
            this.sectionFrom.Name = "sectionFrom";
            this.sectionFrom.SectionName = "From";
            this.sectionFrom.SettingsKey = "6306b47c-8029-44bc-8b97-9b9eb001a61a";
            this.sectionFrom.Size = new System.Drawing.Size(398, 24);
            this.sectionFrom.TabIndex = 0;
            // 
            // originControl
            // 
            this.originControl.BackColor = System.Drawing.Color.Transparent;
            this.originControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.originControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.originControl.Location = new System.Drawing.Point(0, 5);
            this.originControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.originControl.Name = "originControl";
            this.originControl.Size = new System.Drawing.Size(394, 0);
            this.originControl.TabIndex = 1;
            this.originControl.OriginChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // insuranceControl
            // 
            this.insuranceControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.insuranceControl.BackColor = System.Drawing.Color.Transparent;
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.insuranceControl.Location = new System.Drawing.Point(16, 168);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(495, 48);
            this.insuranceControl.TabIndex = 12;
            // 
            // OtherServiceControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionFrom);
            this.Name = "OtherServiceControl";
            this.Size = new System.Drawing.Size(404, 420);
            this.Controls.SetChildIndex(this.sectionReturns, 0);
            this.Controls.SetChildIndex(this.sectionShipment, 0);
            this.Controls.SetChildIndex(this.sectionRecipient, 0);
            this.Controls.SetChildIndex(this.sectionFrom, 0);
            this.sectionRecipient.ContentPanel.ResumeLayout(false);
            this.sectionRecipient.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns)).EndInit();
            this.sectionShipment.ContentPanel.ResumeLayout(false);
            this.sectionShipment.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).EndInit();
            this.sectionFrom.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelCarrier;
        private ShipWorks.UI.Controls.MultiValueTextBox carrier;
        private ShipWorks.UI.Controls.MultiValueTextBox service;
        private System.Windows.Forms.Label labelService;
        private ShipWorks.UI.Controls.MultiValueDateTimePicker shipDate;
        private System.Windows.Forms.Label labelShipDate;
        private ShipWorks.UI.Controls.MultiValueTextBox tracking;
        private System.Windows.Forms.Label labelTracking;
        private System.Windows.Forms.Label labelCost;
        private ShipWorks.UI.Controls.MoneyTextBox cost;
        private System.Windows.Forms.Label label1;
        private ShipWorks.UI.Controls.WeightControl weight;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionFrom;
        private ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl originControl;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private Insurance.InsuranceSelectionControl insuranceControl;
    }
}
