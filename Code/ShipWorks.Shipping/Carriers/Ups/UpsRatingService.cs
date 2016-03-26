using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// Rating service for Ups carrier
    /// </summary>
    public class UpsRatingService : IRatingService
    {
        private readonly ICarrierAccountRepository<UpsAccountEntity> accountRepository;
        private readonly UpsApiTransitTimeClient transitTimeClient;
        private readonly UpsApiRateClient upsApiRateClient;
        private readonly UpsOltShipmentType shipmentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsRatingService"/> class.
        /// </summary>
        public UpsRatingService(
            ICarrierAccountRepository<UpsAccountEntity> accountRepository,
            UpsApiTransitTimeClient transitTimeClient,
            UpsApiRateClient upsApiRateClient,
            UpsOltShipmentType shipmentType)
        {
            this.accountRepository = accountRepository;
            this.transitTimeClient = transitTimeClient;
            this.upsApiRateClient = upsApiRateClient;
            this.shipmentType = shipmentType;
        }

        /// <summary>
        /// Get the UPS rates for the given shipment
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            // Determine if the user is hoping to get negotiated rates back
            bool wantedNegotiated = false;

            // Indicates if any of the rates returned were negotiated.
            bool anyNegotiated = false;
            bool allNegotiated = false;

            IEnumerable<UpsServiceRate> serviceRates;
            IEnumerable<UpsTransitTime> transitTimes;

            try
            {
                // If there are no UPS Accounts then use the counter rates
                if (!accountRepository.Accounts.Any() && !shipmentType.IsShipmentTypeRestricted)
                {
                    CounterRatesOriginAddressValidator.EnsureValidAddress(shipment);

                    // Get the transit times and services
                    //transitTimes = transitTimeClient.GetTransitTimes(shipment, true);
                    //serviceRates = upsApiRateClient.GetRates(shipment, true);
                    return RateGroup.ShippingAccountRequiredRateGroup();
                }
                else
                {
                    // Get the transit times and services
                    transitTimes = transitTimeClient.GetTransitTimes(shipment, false);
                    serviceRates = upsApiRateClient.GetRates(shipment, false);

                    // Determine if the user is hoping to get negotiated rates back
                    wantedNegotiated = UpsApiCore.GetUpsAccount(shipment, accountRepository).RateType == (int)UpsRateType.Negotiated;

                    // Indicates if any of the rates returned were negotiated.
                    anyNegotiated = serviceRates.Any(s => s.Negotiated);
                    allNegotiated = serviceRates.All(s => s.Negotiated);
                }

                // Validate each of the packages dimensions.
                ValidatePackageDimensions(shipment);

                List<RateResult> rates = AddRateForEachService(allNegotiated, serviceRates, transitTimes);
                AddMessageResult(shipment, wantedNegotiated, anyNegotiated, allNegotiated, rates);

                // Filter out any excluded services, but always include the service that the shipment is configured with
                List<RateResult> finalRatesFilteredByAvailableServices = FilterRatesByExcludedServices(shipment, rates);

                RateGroup finalGroup = new RateGroup(finalRatesFilteredByAvailableServices);
                return finalGroup;
            }
            catch (CounterRatesOriginAddressException)
            {
                RateGroup errorRates = new RateGroup(new List<RateResult>());
                errorRates.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(shipmentType));

                return errorRates;
            }
            catch (Exception ex) when (ex is UpsException || ex is InvalidPackageDimensionsException)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds the rate for each service.
        /// </summary>
        private List<RateResult> AddRateForEachService(bool allNegotiated, IEnumerable<UpsServiceRate> serviceRates, IEnumerable<UpsTransitTime> transitTimes)
        {
            List<RateResult> rates = new List<RateResult>();

            // Add a rate for each service
            foreach (UpsServiceRate serviceRate in serviceRates)
            {
                UpsServiceType service = serviceRate.Service;

                UpsTransitTime transitTime = transitTimes.SingleOrDefault(t => t.Service == service);

                RateResult rateResult = new RateResult(
                    (serviceRate.Negotiated && !allNegotiated ? "* " : "") + EnumHelper.GetDescription(service),
                    GetServiceTransitDays(transitTime) + " " + GetServiceEstimatedArrivalTime(transitTime),
                    serviceRate.Amount,
                    service)
                {
                    ServiceLevel = UpsServiceLevelConverter.GetServiceLevel(serviceRate, transitTime),
                    ExpectedDeliveryDate = transitTime?.ArrivalDate ?? ShippingManager.CalculateExpectedDeliveryDate(serviceRate.GuaranteedDaysToDelivery, DayOfWeek.Saturday, DayOfWeek.Sunday),
                    ShipmentType = ShipmentTypeCode.UpsOnLineTools,
                    ProviderLogo = EnumHelper.GetImage(ShipmentTypeCode.UpsOnLineTools)
                };

                rates.Add(rateResult);
            }

            return rates;
        }

        /// <summary>
        /// Adds the message result.
        /// </summary>
        private static void AddMessageResult(ShipmentEntity shipment, bool wantedNegotiated, bool anyNegotiated, bool allNegotiated, IList<RateResult> rates)
        {
            if (rates.None())
            {
                rates.Add(new RateResult("* No rates were returned for the selected Service.", ""));
                return;
            }

            // If they wanted negotiated rates, we have to show some results
            if (wantedNegotiated)
            {
                if (allNegotiated)
                {
                    rates.Add(new RateResult("* All rates are negotiated rates.", ""));
                }
                else if (anyNegotiated)
                {
                    rates.Add(new RateResult("* Indicates a negotiated rate.", ""));
                }
                else
                {
                    rates.Add(new RateResult("* Negotiated rates were not returned. Contact Interapptive.", ""));
                }
            }

            if (shipment.ReturnShipment)
            {
                rates.Add(new RateResult("* Rates reflect the service charge only. This does not include additional fees for returns.", ""));
            }
        }

        /// <summary>
        /// Get the number of days of transit it takes for the given service.  The transit time can be looked up in the given list.  If not present, then 
        /// empty string is returned.
        /// </summary>
        private string GetServiceTransitDays(UpsTransitTime transitTime)
        {
            if (transitTime != null)
            {
                return transitTime.BusinessDays.ToString(CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the service estimated arrival time for the given service type if it's available.
        /// </summary>
        /// <param name="transitTime">The transit time.</param>
        /// <returns>A string value of the arrival time in the format of "DayOfWeek h:mm tt" (e.g. Friday 4:00 PM)</returns>
        private static string GetServiceEstimatedArrivalTime(UpsTransitTime transitTime)
        {
            string arrivalInfo = string.Empty;

            if (transitTime != null)
            {
                DateTime localArrival = transitTime.ArrivalDate.ToLocalTime();
                arrivalInfo = ShippingManager.GetArrivalDescription(localArrival);
            }

            return arrivalInfo;
        }

        /// <summary>
        /// Gets the filtered rates based on any excluded services configured for this ups shipment type.
        /// </summary>
        private List<RateResult> FilterRatesByExcludedServices(ShipmentEntity shipment, List<RateResult> rates)
        {
            IEnumerable<UpsServiceType> availableServices = shipmentType.GetAvailableServiceTypes()
                .Select(s => (UpsServiceType)s).Union(new List<UpsServiceType> { (UpsServiceType)shipment.Ups.Service });

            return rates.Where(r => !(r.Tag is UpsServiceType) || availableServices.Contains(((UpsServiceType)r.Tag))).ToList();
        }

        /// <summary>
        /// Checks each packages dimensions, making sure that each is valid.  If one or more packages have invalid dimensions, 
        /// a ShippingException is thrown informing the user.
        /// </summary>
        private void ValidatePackageDimensions(ShipmentEntity shipment)
        {
            string exceptionMessage = string.Empty;
            int packageIndex = 1;

            foreach (UpsPackageEntity upsPackage in shipment.Ups.Packages)
            {
                if (upsPackage.PackagingType == (int)UpsPackagingType.Custom && !DimensionsAreValid(upsPackage))
                {
                    exceptionMessage += $"Package {packageIndex} has invalid dimensions.{Environment.NewLine}";
                }

                packageIndex++;
            }

            if (exceptionMessage.Length > 0)
            {
                exceptionMessage += "Package dimensions must be greater than 0 and not 1x1x1.  ";
                throw new InvalidPackageDimensionsException(exceptionMessage);
            }
        }

        /// <summary>
        /// Check to see if a package dimensions are valid for carriers that require dimensions.
        /// </summary>
        /// <returns>True if the dimensions are valid.  False otherwise.</returns>
        private static bool DimensionsAreValid(UpsPackageEntity package)
        {
            // Only check the dimensions if the package type is custom 
            if (package.PackagingType != (int) UpsPackagingType.Custom)
            {
                return true;
            }
                
            if (package.DimsLength <= 0 || package.DimsWidth <= 0 || package.DimsHeight <= 0)
            {
                return false;
            }

            // Some customers may have 1x1x1 in a profile to get around carriers that used to require dimensions.
            // This is no longer valid due to new dimensional weight requirements.
            return !(package.DimsLength.IsEquivalentTo(1) &&
                     package.DimsWidth.IsEquivalentTo(1) &&
                     package.DimsHeight.IsEquivalentTo(1));
        }
    }
}
