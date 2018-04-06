using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Endicia shipping profile application strategy
    /// </summary>
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.Endicia)]
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.Express1Endicia)]
    public class EndiciaShippingProfileApplicationStrategy : PostalShippingProfileApplicationStrategy
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseStrategy"></param>
        public EndiciaShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager)
            :base(shipmentTypeManager)
        {
        }

        /// <summary>
        /// Apply the Endicia profile to the shipment
        /// </summary>
        public override void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            base.ApplyProfile(profile, shipment);

            // We can be called during the creation of the base Postal shipment, before the endicia one exists
            if (shipment.Postal.Endicia != null)
            {
                EndiciaShipmentEntity endiciaShipment = shipment.Postal.Endicia;
                IEndiciaProfileEntity endiciaProfile = profile.Postal.Endicia;

                ApplyProfileValue(endiciaProfile.EndiciaAccountID, endiciaShipment, EndiciaShipmentFields.EndiciaAccountID);
                ApplyProfileValue(endiciaProfile.StealthPostage, endiciaShipment, EndiciaShipmentFields.StealthPostage);
                ApplyProfileValue(endiciaProfile.ReferenceID, endiciaShipment, EndiciaShipmentFields.ReferenceID);
                ApplyProfileValue(endiciaProfile.ScanBasedReturn, endiciaShipment, EndiciaShipmentFields.ScanBasedReturn);
                ApplyProfileValue(endiciaProfile.PostalProfile.Profile.Insurance, endiciaShipment, EndiciaShipmentFields.Insurance);
            }
        }
    }
}
