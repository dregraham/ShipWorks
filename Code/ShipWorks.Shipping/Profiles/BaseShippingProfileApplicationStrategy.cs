using SD.LLBLGen.Pro.ORMSupportClasses;
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
    public abstract class BaseShippingProfileApplicationStrategy : IShippingProfileApplicationStrategy
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

            ApplyProfileValue(profile.OriginID, shipment, ShipmentFields.OriginOriginID);
            ApplyProfileValue(profile.ReturnShipment, shipment, ShipmentFields.ReturnShipment);

            ApplyProfileValue(profile.RequestedLabelFormat, shipment, ShipmentFields.RequestedLabelFormat);
            shipmentType.SaveRequestedLabelFormat((ThermalLanguage) shipment.RequestedLabelFormat, shipment);

            // Special case for insurance
            for (int i = 0; i < shipmentType.GetParcelCount(shipment); i++)
            {
                IInsuranceChoice insuranceChoice = shipmentType.GetParcelDetail(shipment, i).Insurance;

                if (profile.Insurance != null)
                {
                    insuranceChoice.Insured = profile.Insurance.Value;
                }

                if (profile.InsuranceInitialValueSource != null && i == 0)
                {
                    // Don't apply the value to the subsequent parcels - that would probably end up over-ensuring the whole shipment.
                    InsuranceInitialValueSource source = (InsuranceInitialValueSource) profile.InsuranceInitialValueSource;
                    insuranceChoice.InsuranceValue = InsuranceUtility.GetInsuranceValue(shipment, source, profile.InsuranceInitialValueAmount);
                }
            }
        }

        /// <summary>
        /// Apply the given value to the specified entity and field, but only if the value is non-null
        /// </summary>
        protected static void ApplyProfileValue<T>(T? value, EntityBase2 entity, EntityField2 field) where T : struct
        {
            if (value.HasValue)
            {
                entity.SetNewFieldValue(field.FieldIndex, value.Value);
            }
        }

        /// <summary>
        /// Apply the given value to the specified entity and field, but only if the value is non-null
        /// </summary>
        protected static void ApplyProfileValue(string value, EntityBase2 entity, EntityField2 field)
        {
            if (value != null)
            {
                entity.SetNewFieldValue(field.FieldIndex, value);
            }
        }
    }
}
