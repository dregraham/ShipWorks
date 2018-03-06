using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Base shipping profile application strategy
    /// </summary>
    public class BaseShippingProfileApplicationStrategy : IShippingProfileApplicationStrategy
    {
        protected readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipmentTypeManager"></param>
        public BaseShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager)
        {
            this.shipmentTypeManager = shipmentTypeManager;
        }

        /// <summary>
        /// Apply the given profile to the ShipmentEntity
        /// </summary>
        public virtual void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);

            ShippingProfileUtility.ApplyProfileValue(profile.OriginID, shipment, ShipmentFields.OriginOriginID);
            ShippingProfileUtility.ApplyProfileValue(profile.ReturnShipment, shipment, ShipmentFields.ReturnShipment);

            ShippingProfileUtility.ApplyProfileValue(profile.RequestedLabelFormat, shipment, ShipmentFields.RequestedLabelFormat);
            shipmentType.SaveRequestedLabelFormat((ThermalLanguage) shipment.RequestedLabelFormat, shipment);

            // Special case for insurance
            for (int i = 0; i < shipmentType.GetParcelCount(shipment); i++)
            {
                IInsuranceChoice insuranceChoice = shipmentType.GetParcelDetail(shipment, i).Insurance;

                if (profile.Insurance != null)
                {
                    insuranceChoice.Insured = profile.Insurance.Value;
                }

                if (profile.InsuranceInitialValueSource != null)
                {
                    // Don't apply the value to the subsequent parcels - that would probably end up over-ensuring the whole shipment.
                    if (i == 0)
                    {
                        InsuranceInitialValueSource source = (InsuranceInitialValueSource) profile.InsuranceInitialValueSource;
                        insuranceChoice.InsuranceValue = InsuranceUtility.GetInsuranceValue(shipment, source, profile.InsuranceInitialValueAmount);
                    }
                }
            }
        }
    }
}
