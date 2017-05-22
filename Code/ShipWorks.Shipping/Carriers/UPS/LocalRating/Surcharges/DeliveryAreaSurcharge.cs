using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
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
        private readonly UpsLocalRatingZoneFileEntity zoneFile;
        private readonly IResidentialDeterminationService residentialDeterminationService;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryAreaSurcharge"/> class.
        /// </summary>
        public DeliveryAreaSurcharge(IDictionary<UpsSurchargeType, double> surcharges,
            UpsLocalRatingZoneFileEntity zoneFile, IResidentialDeterminationService residentialDeterminationService, ISqlAdapterFactory sqlAdapterFactory)
        {
            this.zoneFile = zoneFile;
            this.residentialDeterminationService = residentialDeterminationService;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the surcharge to the service rate based on the shipment
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="serviceRate"></param>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            string shipPostalCode = shipment.Shipment.ShipPostalCode;
            int destinationZip;

            if (shipPostalCode.Length >= 5 && int.TryParse(shipPostalCode.Substring(0, 5), out destinationZip))
            {
                IUpsLocalRatingDeliveryAreaSurchargeEntity deliveryAreaSurcharge =
                    GetDeliveryAreaSurcharge(destinationZip);
                bool isResidential = residentialDeterminationService.IsResidentialAddress(shipment.Shipment);
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
        }

        /// <summary>
        /// Get the delivery area surcharge from the database
        /// </summary>
        private IUpsLocalRatingDeliveryAreaSurchargeEntity GetDeliveryAreaSurcharge(int destinationZip)
        {
            if (zoneFile.UpsLocalRatingDeliveryAreaSurcharge.All(d => d.DestinationZip != destinationZip))
            {
                RelationPredicateBucket bucket = new RelationPredicateBucket();
                bucket.PredicateExpression.Add(UpsLocalRatingDeliveryAreaSurchargeFields.DestinationZip == destinationZip);

                try
                {
                    using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                    {
                        adapter.FetchEntityCollection(zoneFile.UpsLocalRatingDeliveryAreaSurcharge, bucket);
                    }
                }
                catch (Exception ex) when (ex is ORMException || ex is SqlException)
                {
                    throw new UpsLocalRatingException($"Error retrieving collection:\r\n\r\n{ex.Message}", ex);
                }
            }

            return zoneFile.UpsLocalRatingDeliveryAreaSurcharge.FirstOrDefault(
                das => das.DestinationZip == destinationZip);
        }

        /// <summary>
        /// Adds the delivery area surcharge.
        /// </summary>
        private void AddDeliveryAreaSurcharge(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate, UpsDeliveryAreaSurchargeType deliveryAreaSurchargeType, bool isResidential, bool isGround)
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
        private void AddSurcharge(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate, UpsSurchargeType surchargeType)
        {
            decimal surchargeAmount = (decimal) surcharges[surchargeType] * shipment.Packages.Count;

            serviceRate.AddAmount(surchargeAmount, EnumHelper.GetDescription(surchargeType));
        }
    }
}