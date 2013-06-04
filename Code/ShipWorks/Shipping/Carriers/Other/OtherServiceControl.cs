﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.Other
{
    /// <summary>
    /// Service options editor for the "Other" shipment type
    /// </summary>
    public partial class OtherServiceControl : ServiceControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OtherServiceControl() 
            : base(ShipmentTypeCode.Other)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the comboboxes
        /// </summary>
        public override void Initialize()
        {
            originControl.Initialize(ShipmentTypeCode.Other);
        }

        /// <summary>
        /// Load the shipment entity data into the control
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            base.LoadShipments(shipments, enableEditing, enableShippingAddress);

            originControl.LoadShipments(shipments);

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    carrier.ApplyMultiText(shipment.Other.Carrier);
                    service.ApplyMultiText(shipment.Other.Service);

                    shipDate.ApplyMultiDate(shipment.ShipDate);
                    weight.ApplyMultiWeight(shipment.ContentWeight);
                    cost.ApplyMultiText(shipment.ShipmentCost.ToString());
                    tracking.ApplyMultiText(shipment.TrackingNumber);
                }
            }

            UpdateInsuranceDisplay();

            UpdateSectionDescription();
        }

        /// <summary>
        /// Update the insurance display for the given shipments
        /// </summary>
        public override void UpdateInsuranceDisplay()
        {
            insuranceControl.LoadInsuranceChoices(LoadedShipments.Select(shipment => ShipmentTypeManager.GetType(shipment).GetParcelInsuranceChoice(shipment, 0)));
        }

        /// <summary>
        /// Refresh the weight box with the latest weight information from the loaded shipments
        /// </summary>
        public override void RefreshContentWeight()
        {
            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    weight.ApplyMultiWeight(shipment.ContentWeight);
                }
            }
        }

        /// <summary>
        /// Save the content of the control to the entities
        /// </summary>
        public override void SaveToShipments()
        {
            SuspendRateCriteriaChangeEvent();

            base.SaveToShipments();

            originControl.SaveToEntities();

            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                carrier.ReadMultiText(s => shipment.Other.Carrier = s);
                service.ReadMultiText(s => shipment.Other.Service = s);

                shipDate.ReadMultiDate(d => shipment.ShipDate = d.Date.AddHours(12));
                weight.ReadMultiWeight(v => shipment.ContentWeight = v);
                cost.ReadMultiText(s => shipment.ShipmentCost = cost.Amount);
                tracking.ReadMultiText(s => shipment.TrackingNumber = s);
            }

            insuranceControl.SaveToInsuranceChoices();

            ResumeRateCriteriaChangeEvent();
        }

        /// <summary>
        /// Origin data has changed
        /// </summary>
        private void OnOriginChanged(object sender, EventArgs e)
        {
            sectionFrom.ExtraText = originControl.OriginDescription;
        }

        /// <summary>
        /// Important parts of the shipment details have changed
        /// </summary>
        private void OnShipmentDetailsChanged(object sender, EventArgs e)
        {
            UpdateSectionDescription();
        }

        /// <summary>
        /// Update the description of the section
        /// </summary>
        private void UpdateSectionDescription()
        {
            string text;

            if (service.MultiValued || carrier.MultiValued)
            {
                text = "(Multiple)";
            }
            else
            {
                text = carrier.Text;

                if (service.Text.Length > 0)
                {
                    if (text.Length > 0)
                    {
                        text += ", ";
                    }

                    text += service.Text;
                }
            }

            sectionShipment.ExtraText = text;
        }
    }
}
