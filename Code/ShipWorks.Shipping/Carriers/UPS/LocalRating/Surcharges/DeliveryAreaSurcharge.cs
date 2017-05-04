﻿using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Surcharge for Delivery Area, Delivery Area Extended, Remote AK and Remote HI
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges.IUpsSurcharge" />
    public class DeliveryAreaSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;
        private readonly IUpsLocalRatingZoneFileEntity zoneFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryAreaSurcharge"/> class.
        /// </summary>
        public DeliveryAreaSurcharge(IDictionary<UpsSurchargeType, double> surcharges,
            IUpsLocalRatingZoneFileEntity zoneFile)
        {
            this.zoneFile = zoneFile;
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the surcharge to the service rate based on the shipment
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="serviceRate"></param>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            int destinationZip = int.Parse(shipment.Shipment.ShipPostalCode);

            IUpsLocalRatingDeliveryAreaSurchargeEntity deliveryAreaSurcharge =
                zoneFile.UpsLocalRatingDeliveryAreaSurcharge.FirstOrDefault(
                    das => das.DestinationZip == destinationZip);

            bool isResidential = shipment.Shipment.ResidentialResult;
            bool isGround = serviceRate.Service == (int) UpsServiceType.UpsGround;

            if (deliveryAreaSurcharge == null)
            {
                if (isResidential)
                {
                    AddSurcharge(shipment, serviceRate,
                        isGround ? UpsSurchargeType.ResidentialGround : UpsSurchargeType.ResidentialAir);
                }
            }
            else
            {
                UpsDeliveryAreaSurchargeType deliveryAreaType =
                    (UpsDeliveryAreaSurchargeType) deliveryAreaSurcharge.DeliveryAreaType;
                switch (deliveryAreaType)
                {
                    case UpsDeliveryAreaSurchargeType.Us48Das:
                    case UpsDeliveryAreaSurchargeType.Us48DasExtended:
                        AddDeliveryAreaSurcharge(shipment, serviceRate, deliveryAreaType, isResidential, isGround);
                        break;
                    case UpsDeliveryAreaSurchargeType.UsRemoteHi:
                        AddSurcharge(shipment, serviceRate, UpsSurchargeType.RemoteAreaHawaii);
                        break;
                    case UpsDeliveryAreaSurchargeType.UsRemoteAk:
                        AddSurcharge(shipment, serviceRate, UpsSurchargeType.RemoteAreaAlaska);
                        break;
                }
            }
        }

        /// <summary>
        /// Adds the delivery area surcharge.
        /// </summary>
        private void AddDeliveryAreaSurcharge(UpsShipmentEntity shipment, UpsLocalServiceRate serviceRate, UpsDeliveryAreaSurchargeType deliveryAreaSurchargeType, bool isResidential, bool isGround)
        {
            if (deliveryAreaSurchargeType == UpsDeliveryAreaSurchargeType.Us48Das)
            {
                if (isResidential)
                {
                    AddSurcharge(shipment, serviceRate, isGround ?
                        UpsSurchargeType.DeliveryAreaResidentialGround :
                        UpsSurchargeType.DeliveryAreaResidentialAir);
                }
                else
                {
                    AddSurcharge(shipment, serviceRate, isGround ?
                        UpsSurchargeType.DeliveryAreaCommercialGround :
                        UpsSurchargeType.DeliveryAreaCommercialAir);
                }
            }
            else
            {
                if (isResidential)
                {
                    AddSurcharge(shipment, serviceRate, isGround ?
                        UpsSurchargeType.DeliveryAreaResidentialExtendedGround :
                        UpsSurchargeType.DeliveryAreaResidentialExtendedAir);
                }
                else
                {
                    AddSurcharge(shipment, serviceRate, isGround ?
                        UpsSurchargeType.DeliveryAreaCommercialExtendedGround :
                        UpsSurchargeType.DeliveryAreaCommercialExtendedAir);
                }
            }
        }
        
        /// <summary>
        /// Adds the surcharge to the service rate
        /// </summary>
        private void AddSurcharge(UpsShipmentEntity shipment, UpsLocalServiceRate serviceRate, UpsSurchargeType surchargeType)
        {
            decimal surchargeAmount = (decimal) surcharges[surchargeType] * shipment.Packages.Count;

            serviceRate.AddAmount(surchargeAmount, EnumHelper.GetDescription(surchargeType));
        }
    }
}