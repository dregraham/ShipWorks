using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public class UspsAutomaticDiscountControlAdapterFactory
    {
        /// <summary>
        /// Creates the adapter to use based on the shipment type.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>An instance of IUspsAutomaticDiscountControlAdapter.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public IUspsAutomaticDiscountControlAdapter CreateAdapter(ShippingSettingsEntity settings, ShipmentEntity shipmentEntity)
        {
            if (shipmentEntity.ShipmentType == (int) ShipmentTypeCode.Endicia || shipmentEntity.ShipmentType == (int) ShipmentTypeCode.Express1Endicia)
            {
                return new EndiciaUspsAutomaticDiscountControlAdapter(settings);
            }

            if (shipmentEntity.ShipmentType == (int) ShipmentTypeCode.Usps || shipmentEntity.ShipmentType == (int) ShipmentTypeCode.Express1Stamps)
            {
                return new StampsUspsAutomaticDiscountControlAdapter(settings);
            }

            throw new InvalidOperationException(string.Format("An unexpected shipment type was provided: {0}.", shipmentEntity.ShipmentType));
        }
    }
}
