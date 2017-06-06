using System;
using log4net;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval
{
    /// <summary>
    /// Set ship phone if blank
    /// </summary>
    [Order(typeof(ILabelRetrievalShipmentManipulator), Order.Unordered)]
    public class ShipmentPhoneManipulator : ILabelRetrievalShipmentManipulator
    {
        private readonly ILog log;
        readonly IShippingSettings shippingSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentPhoneManipulator(IShippingSettings shippingSettings, Func<Type, ILog> createLog)
        {
            this.shippingSettings = shippingSettings;
            log = createLog(GetType());
        }

        /// <summary>
        /// Manipulate the shipment
        /// </summary>
        public ShipmentEntity Manipulate(ShipmentEntity shipment)
        {
            // Apply the blank recipient phone# option.  We apply it right to the entity so that
            // its transparent to all the shipping carrier processing.  But we reset it back
            // after processing, so it doesn't look like that's the phone the customer entered for the shipment.
            if (string.IsNullOrWhiteSpace(shipment.ShipPhone))
            {
                IShippingSettingsEntity settings = shippingSettings.FetchReadOnly();

                shipment.ShipPhone = settings.BlankPhoneOption == (int) ShipmentBlankPhoneOption.SpecifiedPhone ?
                    settings.BlankPhoneNumber : shipment.OriginPhone;

                log.InfoFormat("Shipment {1} - Using phone '{0}' for  in place of blank phone.",
                    shipment.ShipPhone, shipment.ShipmentID);
            }

            return shipment;
        }
    }
}
