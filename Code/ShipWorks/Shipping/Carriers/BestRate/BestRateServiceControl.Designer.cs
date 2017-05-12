using System.Security.AccessControl;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    partial class BestRateServiceControl
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
            this.sectionFrom = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.originControl = new ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl();
            this.labelShipDate = new System.Windows.Forms.Label();
            this.shipDate = new ShipWorks.UI.Controls.MultiValueDateTimePicker();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.labelWeight = new System.Windows.Forms.Label();
            this.labelDimensions = new System.Windows.Forms.Label();
            this.dimensionsControl = new ShipWorks.Shipping.Editing.DimensionsControl();
            this.serviceLevelLabel = new System.Windows.Forms.Label();
            this.serviceLevel = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceSelectionControl();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient.ContentPanel)).BeginInit();
            this.sectionRecipient.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns.ContentPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment.ContentPanel)).BeginInit();
            this.sectionShipment.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom.ContentPanel)).BeginInit();
            this.sectionFrom.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // sectionRecipient
            //
            this.sectionRecipient.Location = new System.Drawing.Point(3, 34);
            this.sectionRecipient.Size = new System.Drawing.Size(441, 24);
            //
            // personControl
            //
            this.personControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.personControl.Size = new System.Drawing.Size(431, 330);
            //
            // sectionReturns
            //
            this.sectionReturns.Location = new System.Drawing.Point(3, 321);
            this.sectionReturns.Size = new System.Drawing.Size(441, 24);

            this.sectionLabelOptions.Size = new System.Drawing.Size(441, 24);
            //
            // sectionShipment
            //
            //
            // sectionShipment.ContentPanel
            //
            this.sectionShipment.ContentPanel.Controls.Add(this.insuranceControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.serviceLevel);
            this.sectionShipment.ContentPanel.Controls.Add(this.serviceLevelLabel);
            this.sectionShipment.ContentPanel.Controls.Add(this.weight);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelWeight);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelDimensions);
            this.sectionShipment.ContentPanel.Controls.Add(this.dimensionsControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelShipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.shipDate);
            this.sectionShipment.Location = new System.Drawing.Point(3, 63);
            this.sectionShipment.Size = new System.Drawing.Size(441, 253);
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
            this.sectionFrom.ExpandedHeight = 431;
            this.sectionFrom.ExtraText = "";
            this.sectionFrom.Location = new System.Drawing.Point(3, 5);
            this.sectionFrom.Name = "sectionFrom";
            this.sectionFrom.SectionName = "From";
            this.sectionFrom.SettingsKey = "6306b47c-8029-44bc-8b97-9b9eb001a61a";
            this.sectionFrom.Size = new System.Drawing.Size(441, 24);
            this.sectionFrom.TabIndex = 0;
            //
            // originControl
            //
            this.originControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.originControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields) (((((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company)
            | ShipWorks.Data.Controls.PersonFields.Street)
            | ShipWorks.Data.Controls.PersonFields.City)
            | ShipWorks.Data.Controls.PersonFields.State)
            | ShipWorks.Data.Controls.PersonFields.Postal)
            | ShipWorks.Data.Controls.PersonFields.Country)
            | ShipWorks.Data.Controls.PersonFields.Residential)
            | ShipWorks.Data.Controls.PersonFields.Email)
            | ShipWorks.Data.Controls.PersonFields.Phone)
            | ShipWorks.Data.Controls.PersonFields.Website)));
            this.originControl.BackColor = System.Drawing.Color.Transparent;
            this.originControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.originControl.Location = new System.Drawing.Point(3, 5);
            this.originControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.originControl.Name = "originControl";
            this.originControl.Size = new System.Drawing.Size(431, 403);
            this.originControl.TabIndex = 8;
            this.originControl.OriginChanged += new System.EventHandler(this.OnOriginChanged);
            //
            // labelShipDate
            //
            this.labelShipDate.AutoSize = true;
            this.labelShipDate.BackColor = System.Drawing.Color.Transparent;
            this.labelShipDate.Location = new System.Drawing.Point(30, 14);
            this.labelShipDate.Name = "labelShipDate";
            this.labelShipDate.Size = new System.Drawing.Size(56, 13);
            this.labelShipDate.TabIndex = 66;
            this.labelShipDate.Text = "Ship date:";
            //
            // shipDate
            //
            this.shipDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.shipDate.Location = new System.Drawing.Point(92, 8);
            this.shipDate.Name = "shipDate";
            this.shipDate.Size = new System.Drawing.Size(144, 21);
            this.shipDate.TabIndex = 65;
            //
            // weight
            //
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.weight.Location = new System.Drawing.Point(92, 35);
            this.weight.Name = "weight";
            this.weight.RangeMax = 9999D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(238, 21);
            this.weight.TabIndex = 68;
            this.weight.Weight = 0D;
            this.weight.WeightChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            this.weight.WeightChanged += OnShipSenseFieldChanged;
            this.weight.ShowShortcutInfo = true;
            //
            // labelWeight
            //
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(41, 38);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 67;
            this.labelWeight.Text = "Weight:";
            //
            // labelDimensions
            //
            this.labelDimensions.AutoSize = true;
            this.labelDimensions.BackColor = System.Drawing.Color.Transparent;
            this.labelDimensions.Location = new System.Drawing.Point(22, 71);
            this.labelDimensions.Name = "labelDimensions";
            this.labelDimensions.Size = new System.Drawing.Size(64, 13);
            this.labelDimensions.TabIndex = 69;
            this.labelDimensions.Text = "Dimensions:";
            //
            // dimensionsControl
            //
            this.dimensionsControl.BackColor = System.Drawing.Color.White;
            this.dimensionsControl.Cleared = false;
            this.dimensionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.dimensionsControl.Location = new System.Drawing.Point(92, 62);
            this.dimensionsControl.Name = "dimensionsControl";
            this.dimensionsControl.Size = new System.Drawing.Size(210, 74);
            this.dimensionsControl.TabIndex = 70;
            this.dimensionsControl.DimensionsChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            this.dimensionsControl.DimensionsChanged += OnShipSenseFieldChanged;
            //
            // serviceLevelLabel
            //
            this.serviceLevelLabel.AutoSize = true;
            this.serviceLevelLabel.BackColor = System.Drawing.Color.Transparent;
            this.serviceLevelLabel.Location = new System.Drawing.Point(15, 141);
            this.serviceLevelLabel.Name = "serviceLevelLabel";
            this.serviceLevelLabel.Size = new System.Drawing.Size(71, 13);
            this.serviceLevelLabel.TabIndex = 71;
            this.serviceLevelLabel.Text = "Service level:";
            //
            // serviceLevel
            //
            this.serviceLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serviceLevel.FormattingEnabled = true;
            this.serviceLevel.Location = new System.Drawing.Point(92, 138);
            this.serviceLevel.Name = "serviceLevel";
            this.serviceLevel.PromptText = "(Multiple Values)";
            this.serviceLevel.Size = new System.Drawing.Size(121, 21);
            this.serviceLevel.TabIndex = 72;
            this.serviceLevel.SelectedIndexChanged += new System.EventHandler(this.OnServiceLevelChanged);
            //
            // insuranceControl
            //
            this.insuranceControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.insuranceControl.BackColor = System.Drawing.Color.Transparent;
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.insuranceControl.Location = new System.Drawing.Point(23, 167);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(647, 50);
            this.insuranceControl.TabIndex = 75;
            //
            // BestRateServiceControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.sectionFrom);
            this.Name = "BestRateServiceControl";
            this.Size = new System.Drawing.Size(447, 445);
            this.Controls.SetChildIndex(this.sectionReturns, 0);
            this.Controls.SetChildIndex(this.sectionShipment, 0);
            this.Controls.SetChildIndex(this.sectionRecipient, 0);
            this.Controls.SetChildIndex(this.sectionFrom, 0);
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient.ContentPanel)).EndInit();
            this.sectionRecipient.ContentPanel.ResumeLayout(false);
            this.sectionRecipient.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionLabelOptions.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionLabelOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment.ContentPanel)).EndInit();
            this.sectionShipment.ContentPanel.ResumeLayout(false);
            this.sectionShipment.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom.ContentPanel)).EndInit();
            this.sectionFrom.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionFrom;
        private ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl originControl;
        private System.Windows.Forms.Label labelShipDate;
        private UI.Controls.MultiValueDateTimePicker shipDate;
        private UI.Controls.WeightControl weight;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.Label labelDimensions;
        private Editing.DimensionsControl dimensionsControl;
        private ShipWorks.UI.Controls.MultiValueComboBox serviceLevel;
        private System.Windows.Forms.Label serviceLevelLabel;
        private Insurance.InsuranceSelectionControl insuranceControl;
    }
}
