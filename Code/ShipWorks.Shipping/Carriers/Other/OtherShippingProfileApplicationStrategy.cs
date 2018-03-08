﻿using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.Other
{
    /// <summary>
    /// Profile application strategy for Other carrier
    /// </summary>
    public class OtherShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        private readonly ShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public OtherShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager) : base(shipmentTypeManager)
        {
            shipmentType = shipmentTypeManager.Get(ShipmentTypeCode.Other);
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            base.ApplyProfile(profile, shipment);

            OtherShipmentEntity otherShipment = shipment.Other;
            IOtherProfileEntity otherProfile = profile.Other;

            ShippingProfileUtility.ApplyProfileValue(otherProfile.Service, otherShipment, OtherShipmentFields.Service);
            ShippingProfileUtility.ApplyProfileValue(otherProfile.Carrier, otherShipment, OtherShipmentFields.Carrier);
            ShippingProfileUtility.ApplyProfileValue(otherProfile.ShippingProfile.Insurance, otherShipment, OtherShipmentFields.Insurance);

            shipmentType.UpdateDynamicShipmentData(shipment);
        }
    }
}