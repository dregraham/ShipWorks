using System;
using System.Collections.Generic;
using ShipWorks.Shipping.Editing;
using ShipWorks.UI.Controls;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    public partial class UpsOltReturnsControl : ReturnsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsOltReturnsControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<UpsReturnServiceType>(returnService);
        }

        /// <summary>
        /// Load the UI from the provided shipments
        /// </summary>
        public override void LoadShipments(List<ShipmentEntity> shipments)
        {
            base.LoadShipments(shipments);

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    UpsShipmentEntity ups = shipment.Ups;

                    returnContents.ApplyMultiText(ups.ReturnContents);
                    returnService.ApplyMultiValue((UpsReturnServiceType)ups.ReturnService);
                    returnUndeliverableEmail.ApplyMultiText(ups.ReturnUndeliverableEmail);
                }
            }
        }

        /// <summary>
        /// Save UI values back to the shipments
        /// </summary>
        public override void SaveToShipments()
        {
            base.SaveToShipments();

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    UpsShipmentEntity ups = shipment.Ups;

                    returnContents.ReadMultiText(t => ups.ReturnContents = t);
                    returnService.ReadMultiValue(v => ups.ReturnService = (int)v);
                    returnUndeliverableEmail.ReadMultiText(t => ups.ReturnUndeliverableEmail = t);
                }
            }
        }

        /// <summary>
        /// Selected Return Service changed
        /// </summary>
        private void OnReturnServiceChanged(object sender, EventArgs e)
        {
            UpsReturnServiceType? selectedService = null;
            returnService.ReadMultiValue(v => selectedService = (UpsReturnServiceType)v);

            // only enable the email panel if it's electronic delivery, which requires email info
            returnEmailPanel.Enabled = selectedService.HasValue && (selectedService == UpsReturnServiceType.ElectronicReturnLabel);

            if (selectedService.HasValue)
            {
                // update the description
                returnDescription.Text = UpsUtility.GetReturnServiceExplanation(selectedService.Value);
            }
            else
            {
                returnDescription.Text = "";
            }

            RaiseRateCriteriaChanged();
        }
    }
}
