using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Validates imported rates.
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Carriers.Ups.LocalRating.IUpsImportedRateValidator" />
    [Component]
    public class UpsImportedRateValidator : IUpsImportedRateValidator
    {
        public const string PackageWeightOutOfRangeErrorMessageFormat =
            "Weights must be between 1-150. {0} sheet has a weight of this range.";

        public const string MissingPackageWeightErrorMessageFormat =
            "Weights required for all whole number weights between 1-150. {0} sheet is missing a weight.";

        public const string DuplicateWeightDetectedErrorMessageFormat = "Duplicate weight detected in sheet {0}.";
        public const string MissingPricePerPoundErrorMessageFormat = "{0} sheet is missing a value for Price Per Pound.";
        public const string LetterNotValidForServiceErrorMessageFormat = "{0} sheet cannot have a rate for Letter.";
        public const string ServiceMissingLetterErrorMessageFormat = "{0} sheet is missing a rate for Letter.";

        /// <summary>
        /// Validates rate information.
        /// </summary>
        /// <exception cref="UpsLocalRatingException">
        /// Message is compiled by replacing {0} in messageFormat and the service type description.
        /// </exception>
        public void Validate(List<IUpsPackageRateEntity> packageRates,
            List<IUpsLetterRateEntity> letterRates,
            List<IUpsPricePerPoundEntity> pricesPerPound)
        {
            var packageZonesAndServices = packageRates.Select(r => new {r.Zone, r.Service}).ToList();
            var letterZonesAndServices = letterRates.Select(r => new {r.Zone, r.Service}).ToList();
            var pricePerPoundZonesAndServices = pricesPerPound.Select(r => new {r.Zone, r.Service}).ToList();

            var zonesAndServices =
                packageZonesAndServices.Union(letterZonesAndServices).Union(pricePerPoundZonesAndServices).Distinct();

            foreach (var zoneAndService in zonesAndServices)
            {
                List<IUpsPackageRateEntity> zonePackageRates = packageRates
                    .Where(r => r.Zone == zoneAndService.Zone && r.Service == zoneAndService.Service).ToList();

                IEnumerable<IUpsLetterRateEntity> zoneLetterRates = letterRates
                    .Where(r => r.Zone == zoneAndService.Zone && r.Service == zoneAndService.Service);

                IEnumerable<IUpsPricePerPoundEntity> zonePricesPerPound = pricesPerPound
                    .Where(r => r.Zone == zoneAndService.Zone && r.Service == zoneAndService.Service);

                Validate(zonePackageRates, zoneLetterRates, zonePricesPerPound, (UpsServiceType) zoneAndService.Service);
            }
        }

        /// <summary>
        /// Validates the specified zone package rates.
        /// </summary>
        /// <remarks>
        /// It is assumed that there is only one zone and service type in all the passed in collections.
        /// </remarks>
        /// <exception cref="UpsLocalRatingException">
        /// Message is compiled by replacing {0} in messageFormat and the service type description.
        /// </exception>
        private void Validate(IList<IUpsPackageRateEntity> zonePackageRates,
            IEnumerable<IUpsLetterRateEntity> zoneLetterRates,
            IEnumerable<IUpsPricePerPoundEntity> zonePricesPerPound,
            UpsServiceType serviceType)
        {
            if (zonePackageRates.Any(r => r.WeightInPounds < 1 || r.WeightInPounds > 150))
            {
                ThrowError(PackageWeightOutOfRangeErrorMessageFormat, serviceType);
            }

            // At this point, we know there cannot be more than 150 distinct values, so we can just check the less than
            int distinctWeightCount = zonePackageRates.Select(r => r.WeightInPounds).Distinct().Count();

            if (distinctWeightCount != zonePackageRates.Count())
            {
                ThrowError(DuplicateWeightDetectedErrorMessageFormat, serviceType);
            }

            if (distinctWeightCount < 150)
            {
                ThrowError(MissingPackageWeightErrorMessageFormat, serviceType);
            }

            if (zonePricesPerPound.None())
            {
                ThrowError(MissingPricePerPoundErrorMessageFormat, serviceType);
            }

            if (IsLetterAllowed(serviceType))
            {
                if (zoneLetterRates.Any())
                {
                    ThrowError(LetterNotValidForServiceErrorMessageFormat, serviceType);
                }
            }
            else
            {
                if (zoneLetterRates.Count() != 1)
                {
                    ThrowError(ServiceMissingLetterErrorMessageFormat, serviceType);
                }
            }
        }

        /// <summary>
        /// Determines whether this service can send a letter.
        /// </summary>
        private static bool IsLetterAllowed(UpsServiceType serviceType)
        {
            return serviceType == UpsServiceType.UpsGround || serviceType == UpsServiceType.Ups3DaySelect;
        }

        /// <summary>
        /// Throws a UpsLocalRatingException
        /// </summary>
        /// <exception cref="UpsLocalRatingException">
        /// Message is compiled by replacing {0} in messageFormat and the service type description.
        /// </exception>
        private void ThrowError(string messageFormat, UpsServiceType serviceType)
        {
            throw new UpsLocalRatingException(string.Format(messageFormat, EnumHelper.GetDescription(serviceType)));
        }
    }
}