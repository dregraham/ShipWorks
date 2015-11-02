using System.Drawing;
using System.Windows.Forms;

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
            this.labelWeight = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.sectionFrom = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.originControl = new ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceSelectionControl();
            this.dimensionsControl = new ShipWorks.Shipping.Editing.DimensionsControl();
            this.label2 = new System.Windows.Forms.Label();
            this.labelDimensions = new System.Windows.Forms.Label();
            this.mustArriveByDate = new System.Windows.Forms.Label();
            this.deliveryConfirmation = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelDeliveryConf = new System.Windows.Forms.Label();
            this.labelService = new System.Windows.Forms.Label();
            this.carrierWillPickUp = new System.Windows.Forms.CheckBox();
            this.sendDeliverBy = new System.Windows.Forms.CheckBox();
            this.labelCarrierWillPickUp = new System.Windows.Forms.Label();
            this.labelDeliverBy = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient.ContentPanel)).BeginInit();
            this.sectionRecipient.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns.ContentPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment.ContentPanel)).BeginInit();
            this.sectionShipment.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions.ContentPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom.ContentPanel)).BeginInit();
            this.sectionFrom.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // sectionRecipient
            // 
            this.sectionRecipient.Location = new System.Drawing.Point(3, 341);
            this.sectionRecipient.Size = new System.Drawing.Size(454, 24);
            this.sectionRecipient.TabIndex = 1;
            this.sectionRecipient.Visible = false;
            // 
            // personControl
            // 
            this.personControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.personControl.Size = new System.Drawing.Size(447, 330);
            // 
            // sectionReturns
            // 
            this.sectionReturns.Location = new System.Drawing.Point(3, 399);
            this.sectionReturns.Size = new System.Drawing.Size(454, 24);
            this.sectionReturns.Visible = false;
            // 
            // sectionShipment
            // 
            // 
            // sectionShipment.ContentPanel
            // 
            this.sectionShipment.ContentPanel.Controls.Add(this.insuranceControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.weight);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelWeight);
            this.sectionShipment.ContentPanel.Controls.Add(this.mustArriveByDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelDimensions);
            this.sectionShipment.ContentPanel.Controls.Add(this.dimensionsControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelCarrierWillPickUp);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelDeliverBy);
            this.sectionShipment.ContentPanel.Controls.Add(this.carrierWillPickUp);
            this.sectionShipment.ContentPanel.Controls.Add(this.sendDeliverBy);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelDeliveryConf);
            this.sectionShipment.ContentPanel.Controls.Add(this.deliveryConfirmation);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelService);
            this.sectionShipment.ContentPanel.Controls.Add(this.service);
            this.sectionShipment.Location = new System.Drawing.Point(3, 34);
            this.sectionShipment.Size = new System.Drawing.Size(454, 302);
            // 
            // sectionLabelOptions
            // 
            this.sectionLabelOptions.Location = new System.Drawing.Point(3, 370);
            this.sectionLabelOptions.Size = new System.Drawing.Size(454, 24);
            this.sectionLabelOptions.Visible = false;
            // 
            // weight
            // 
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weight.Location = new System.Drawing.Point(91, 33);
            this.weight.Name = "weight";
            this.weight.RangeMax = 300D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(218, 21);
            this.weight.TabIndex = 7;
            this.weight.Weight = 0D;
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(40, 37);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 6;
            this.labelWeight.Text = "Weight:";
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
            this.sectionFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
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
            this.sectionFrom.Size = new System.Drawing.Size(454, 24);
            this.sectionFrom.TabIndex = 0;
            // 
            // originControl
            // 
            this.originControl.BackColor = System.Drawing.Color.Transparent;
            this.originControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.originControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originControl.Location = new System.Drawing.Point(0, 5);
            this.originControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.originControl.Name = "originControl";
            this.originControl.Size = new System.Drawing.Size(450, 0);
            this.originControl.TabIndex = 1;
            this.originControl.OriginChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // insuranceControl
            // 
            this.insuranceControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.insuranceControl.BackColor = System.Drawing.Color.Transparent;
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceControl.Location = new System.Drawing.Point(22, 198);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(425, 48);
            this.insuranceControl.TabIndex = 12;
            // 
            // dimensionsControl
            // 
            this.dimensionsControl.BackColor = System.Drawing.Color.White;
            this.dimensionsControl.Cleared = false;
            this.dimensionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dimensionsControl.Location = new System.Drawing.Point(88, 56);
            this.dimensionsControl.Name = "dimensionsControl";
            this.dimensionsControl.Size = new System.Drawing.Size(210, 77);
            this.dimensionsControl.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(34, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Weight:";
            // 
            // labelDimensions
            // 
            this.labelDimensions.AutoSize = true;
            this.labelDimensions.BackColor = System.Drawing.Color.White;
            this.labelDimensions.Location = new System.Drawing.Point(21, 63);
            this.labelDimensions.Name = "labelDimensions";
            this.labelDimensions.Size = new System.Drawing.Size(64, 13);
            this.labelDimensions.TabIndex = 7;
            this.labelDimensions.Text = "Dimensions:";
            // 
            // mustArriveByDate
            // 
            this.mustArriveByDate.BackColor = System.Drawing.Color.White;
            this.mustArriveByDate.Location = new System.Drawing.Point(225, 179);
            this.mustArriveByDate.Name = "mustArriveByDate";
            this.mustArriveByDate.Size = new System.Drawing.Size(73, 17);
            this.mustArriveByDate.TabIndex = 8;
            this.mustArriveByDate.Text = "(01/01/0001)";
            this.mustArriveByDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // deliveryConfirmation
            // 
            this.deliveryConfirmation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deliveryConfirmation.FormattingEnabled = true;
            this.deliveryConfirmation.Location = new System.Drawing.Point(91, 133);
            this.deliveryConfirmation.Name = "deliveryConfirmation";
            this.deliveryConfirmation.PromptText = "(Multiple Values)";
            this.deliveryConfirmation.Size = new System.Drawing.Size(220, 21);
            this.deliveryConfirmation.TabIndex = 9;
            // 
            // service
            // 
            this.service.DisplayMember = "Description";
            this.service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.service.FormattingEnabled = true;
            this.service.Location = new System.Drawing.Point(91, 7);
            this.service.Name = "service";
            this.service.PromptText = "(Multiple Values)";
            this.service.Size = new System.Drawing.Size(220, 21);
            this.service.TabIndex = 6;
            this.service.ValueMember = "ShippingServiceId";
            // 
            // labelDeliveryConf
            // 
            this.labelDeliveryConf.AutoSize = true;
            this.labelDeliveryConf.BackColor = System.Drawing.Color.White;
            this.labelDeliveryConf.Location = new System.Drawing.Point(13, 136);
            this.labelDeliveryConf.Name = "labelDeliveryConf";
            this.labelDeliveryConf.Size = new System.Drawing.Size(72, 13);
            this.labelDeliveryConf.TabIndex = 7;
            this.labelDeliveryConf.Text = "Confirmation:";
            // 
            // labelService
            // 
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.White;
            this.labelService.Location = new System.Drawing.Point(39, 9);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 7;
            this.labelService.Text = "Service:";
            // 
            // carrierWillPickUp
            // 
            this.carrierWillPickUp.AutoSize = true;
            this.carrierWillPickUp.BackColor = System.Drawing.Color.White;
            this.carrierWillPickUp.Location = new System.Drawing.Point(91, 161);
            this.carrierWillPickUp.Name = "carrierWillPickUp";
            this.carrierWillPickUp.Size = new System.Drawing.Size(177, 17);
            this.carrierWillPickUp.TabIndex = 10;
            this.carrierWillPickUp.Text = "Carrier will pick up the shipment";
            this.carrierWillPickUp.UseVisualStyleBackColor = false;
            // 
            // sendDeliverBy
            // 
            this.sendDeliverBy.AutoSize = true;
            this.sendDeliverBy.BackColor = System.Drawing.Color.White;
            this.sendDeliverBy.Location = new System.Drawing.Point(91, 180);
            this.sendDeliverBy.Name = "sendDeliverBy";
            this.sendDeliverBy.Size = new System.Drawing.Size(142, 17);
            this.sendDeliverBy.TabIndex = 11;
            this.sendDeliverBy.Text = "Deliver by required date";
            this.sendDeliverBy.UseVisualStyleBackColor = false;
            // 
            // labelCarrierWillPickUp
            // 
            this.labelCarrierWillPickUp.AutoSize = true;
            this.labelCarrierWillPickUp.BackColor = System.Drawing.Color.White;
            this.labelCarrierWillPickUp.Location = new System.Drawing.Point(5, 161);
            this.labelCarrierWillPickUp.Name = "labelCarrierWillPickUp";
            this.labelCarrierWillPickUp.Size = new System.Drawing.Size(80, 13);
            this.labelCarrierWillPickUp.TabIndex = 9;
            this.labelCarrierWillPickUp.Text = "Carrier pick up:";
            // 
            // labelDeliverBy
            // 
            this.labelDeliverBy.AutoSize = true;
            this.labelDeliverBy.BackColor = System.Drawing.Color.White;
            this.labelDeliverBy.Location = new System.Drawing.Point(10, 180);
            this.labelDeliverBy.Name = "labelDeliverBy";
            this.labelDeliverBy.Size = new System.Drawing.Size(75, 13);
            this.labelDeliverBy.TabIndex = 9;
            this.labelDeliverBy.Text = "Delivery date:";
            // 
            // AmazonServiceControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionFrom);
            this.Name = "AmazonServiceControl";
            this.Size = new System.Drawing.Size(460, 440);
            this.Controls.SetChildIndex(this.sectionReturns, 0);
            this.Controls.SetChildIndex(this.sectionLabelOptions, 0);
            this.Controls.SetChildIndex(this.sectionRecipient, 0);
            this.Controls.SetChildIndex(this.sectionShipment, 0);
            this.Controls.SetChildIndex(this.sectionFrom, 0);
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient.ContentPanel)).EndInit();
            this.sectionRecipient.ContentPanel.ResumeLayout(false);
            this.sectionRecipient.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment.ContentPanel)).EndInit();
            this.sectionShipment.ContentPanel.ResumeLayout(false);
            this.sectionShipment.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom.ContentPanel)).EndInit();
            this.sectionFrom.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelWeight;
        private ShipWorks.UI.Controls.WeightControl weight;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionFrom;
        private ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl originControl;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private Insurance.InsuranceSelectionControl insuranceControl;
        private System.Windows.Forms.Label mustArriveByDate;
        private System.Windows.Forms.Label labelDimensions;
        private Editing.DimensionsControl dimensionsControl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelDeliveryConf;
        private ShipWorks.UI.Controls.MultiValueComboBox deliveryConfirmation;
        private System.Windows.Forms.Label labelCarrierWillPickUp;
        private System.Windows.Forms.Label labelDeliverBy;
        private System.Windows.Forms.CheckBox carrierWillPickUp;
        private System.Windows.Forms.CheckBox sendDeliverBy;
        private System.Windows.Forms.Label labelService;
        private ShipWorks.UI.Controls.MultiValueComboBox service;
    }
}
