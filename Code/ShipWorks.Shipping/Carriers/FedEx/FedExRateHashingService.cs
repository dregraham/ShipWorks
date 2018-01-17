﻿using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// FedEx RateHashingService
    /// </summary>
    public class FedExRateHashingService : RateHashingService
    {
        /// <summary>
        /// Fields of a shipment used to calculate rates
        /// </summary>
        protected override RatingFields RatingFields
        {
            get
            {
                if (ratingField != null)
                {
                    return ratingField;
                }

                ratingField = base.RatingFields;
                ratingField.AddShipmentField(FedExShipmentFields.FedExAccountID, genericAccountIdFieldName);
                ratingField.AddShipmentField(FedExShipmentFields.WeightUnitType);
                ratingField.AddShipmentField(FedExShipmentFields.Signature);
                ratingField.AddShipmentField(FedExShipmentFields.Service);
                ratingField.AddShipmentField(FedExShipmentFields.PackagingType);
                ratingField.AddShipmentField(FedExShipmentFields.DropoffType);
                ratingField.AddShipmentField(FedExShipmentFields.SaturdayDelivery);
                ratingField.AddShipmentField(FedExShipmentFields.OriginResidentialDetermination);
                ratingField.AddShipmentField(FedExShipmentFields.SmartPostHubID);
                ratingField.AddShipmentField(FedExShipmentFields.SmartPostIndicia);
                ratingField.AddShipmentField(FedExShipmentFields.SmartPostEndorsement);
                ratingField.AddShipmentField(FedExShipmentFields.CodEnabled);
                ratingField.AddShipmentField(FedExShipmentFields.NonStandardContainer);
                ratingField.AddShipmentField(FedExShipmentFields.FreightSpecialServices);
                ratingField.AddShipmentField(FedExShipmentFields.FreightTotalHandlinUnits);
                ratingField.AddShipmentField(FedExShipmentFields.FreightClass);
                ratingField.AddShipmentField(FedExShipmentFields.FreightCollectTerms);
                ratingField.AddShipmentField(FedExShipmentFields.FreightRole);

                AddPackageValues();

                return ratingField;
            }
        }

        /// <summary>
        /// Add package values
        /// </summary>
        private void AddPackageValues()
        {
            ratingField.PackageFields.Add(FedExPackageFields.DimsWeight);
            ratingField.PackageFields.Add(FedExPackageFields.DimsAddWeight);
            ratingField.PackageFields.Add(FedExPackageFields.DeclaredValue);
            ratingField.PackageFields.Add(FedExPackageFields.DimsLength);
            ratingField.PackageFields.Add(FedExPackageFields.DimsHeight);
            ratingField.PackageFields.Add(FedExPackageFields.DimsWidth);
            ratingField.PackageFields.Add(FedExPackageFields.ContainsAlcohol);
            ratingField.PackageFields.Add(FedExPackageFields.DryIceWeight);
            ratingField.PackageFields.Add(FedExPackageFields.FreightPieces);
            ratingField.PackageFields.Add(FedExPackageFields.FreightPackaging);
        }

        /// <summary>
        /// Gets the rating hash based on the shipment's configuration.
        /// </summary>
        public override string GetRatingHash(ShipmentEntity shipment)
        {
            return RatingFields.GetRatingHash(shipment, shipment.FedEx.Packages);
        }
    }
}
