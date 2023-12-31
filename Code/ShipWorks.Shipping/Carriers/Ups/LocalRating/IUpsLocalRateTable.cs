﻿using System;
using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Generic;
using System.IO;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Interface that represents a Ups local rate table
    /// </summary>
    public interface IUpsLocalRateTable
    {
        /// <summary>
        /// The date of the rate upload
        /// </summary>
        DateTime? RateUploadDate { get; }

        /// <summary>
        /// The date the zones were uploaded
        /// </summary>
        DateTime? ZoneUploadDate { get; }

        /// <summary>
        /// Load the rate table from a stream
        /// </summary>
        void LoadRates(Stream stream);

        /// <summary>
        /// Loads the zones from a stream
        /// </summary>
        void LoadZones(Stream stream);

        /// <summary>
        /// Load the rate table from a UpsAccountEntity
        /// </summary>
        void Load(UpsAccountEntity account);

        /// <summary>
        /// Save the rate table
        /// </summary>
        void SaveRates(UpsAccountEntity accountEntity);

        /// <summary>
        /// Saves the zones.
        /// </summary>
        void SaveZones();

        /// <summary>
        /// Add a surcharge collection to the surcharge table
        /// </summary>
        void ReplaceSurcharges(IEnumerable<UpsRateSurchargeEntity> newSurcharges);

        /// <summary>
        /// Adds the rates to the rate tables
        /// </summary>
        void ReplaceRates(IEnumerable<UpsPackageRateEntity> packageRates,
            IEnumerable<UpsLetterRateEntity> letterRates,
            IEnumerable<UpsPricePerPoundEntity> pricesPerPound);

        /// <summary>
        /// Replaces the zones.
        /// </summary>
        void ReplaceZones(IEnumerable<UpsLocalRatingZoneEntity> zones);

        /// <summary>
        /// Replaces the delivery area surcharges.
        /// </summary>
        void ReplaceDeliveryAreaSurcharges(IEnumerable<UpsLocalRatingDeliveryAreaSurchargeEntity> localRatingDeliveryAreaSurcharges);

        /// <summary>
        /// Calculates shipment rates. Success is true only when rates are found.
        /// </summary>
        GenericResult<IEnumerable<UpsLocalServiceRate>> CalculateRates(ShipmentEntity shipment);
    }
}
