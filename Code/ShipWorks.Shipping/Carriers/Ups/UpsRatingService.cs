using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Ups
{
    public class UpsRatingService : IRatingService
    {
        private readonly ICachedRatesService cachedRatesService;
        private ICarrierSettingsRepository settingsRepository;
        private ICarrierAccountRepository<UpsAccountEntity> accountRepository;
        private ICertificateInspector certificateInspector;
        private readonly UpsShipmentType shipmentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsRatingService"/> class.
        /// </summary>
        public UpsRatingService(
            ICachedRatesService cachedRatesService,
            ICarrierSettingsRepository settingsRepository, 
            ICarrierAccountRepository<UpsAccountEntity> accountRepository,
            ICertificateInspector certificateInspector,
            UpsShipmentType shipmentType)
        {
            this.cachedRatesService = cachedRatesService;
            this.settingsRepository = settingsRepository;
            this.accountRepository = accountRepository;
            this.certificateInspector = certificateInspector;
            this.shipmentType = shipmentType;
        }

        /// <summary>
        /// Get the UPS rates for the given shipment
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            ICarrierSettingsRepository originalSettings = settingsRepository;
            ICertificateInspector originalInspector = certificateInspector;
            ICarrierAccountRepository<UpsAccountEntity> originalAccountRepository = accountRepository;

            try
            {
                // Check with the SettingsRepository here rather than UpsAccountManager, so getting 
                // counter rates from the broker is not impacted
                if (!settingsRepository.GetAccounts().Any() && !shipmentType.IsShipmentTypeRestricted)
                {
                    CounterRatesOriginAddressValidator.EnsureValidAddress(shipment);

                    // We need to swap out the SettingsRepository and certificate inspector 
                    // to get UPS counter rates
                    settingsRepository = new UpsCounterRateSettingsRepository(TangoCredentialStore.Instance);
                    certificateInspector = new CertificateInspector(TangoCredentialStore.Instance.UpsCertificateVerificationData);
                    accountRepository = new UpsCounterRateAccountRepository(TangoCredentialStore.Instance);
                }

                return cachedRatesService.GetCachedRates<UpsException>(shipment, GetRatesFromApi);
            }
            catch (CounterRatesOriginAddressException)
            {
                RateGroup errorRates = new RateGroup(new List<RateResult>());
                errorRates.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(shipmentType));
                return errorRates;
            }
            finally
            {
                // Switch the settings repository back to the original now that we have counter rates
                settingsRepository = originalSettings;
                certificateInspector = originalInspector;
                accountRepository = originalAccountRepository;
            }
        }

        /// <summary>
        /// Get the UPS rates from the UPS api
        /// </summary>
        private RateGroup GetRatesFromApi(ShipmentEntity shipment)
        {
            try
            {
                // Validate each of the packages dimensions.
                ValidatePackageDimensions(shipment);

                List<RateResult> rates = new List<RateResult>();

                // Get the transit times and services
                List<UpsTransitTime> transitTimes = UpsApiTransitTimeClient.GetTransitTimes(shipment, accountRepository, settingsRepository, certificateInspector);

                UpsApiRateClient upsApiRateClient = new UpsApiRateClient(accountRepository, settingsRepository, certificateInspector);
                List<UpsServiceRate> serviceRates = upsApiRateClient.GetRates(shipment);

                if (!serviceRates.Any())
                {
                    rates.Add(new RateResult("* No rates were returned for the selected Service.", ""));
                }
                else
                {
                    // Determine if the user is hoping to get negotiated rates back
                    bool wantedNegotiated = UpsApiCore.GetUpsAccount(shipment, accountRepository).RateType == (int)UpsRateType.Negotiated;

                    // Indicates if any of the rates returned were negotiated.
                    bool anyNegotiated = serviceRates.Any(s => s.Negotiated);
                    bool allNegotiated = serviceRates.All(s => s.Negotiated);

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
                            ExpectedDeliveryDate = transitTime == null ? ShippingManager.CalculateExpectedDeliveryDate(serviceRate.GuaranteedDaysToDelivery, DayOfWeek.Saturday, DayOfWeek.Sunday) : transitTime.ArrivalDate,
                            ShipmentType = ShipmentTypeCode.UpsOnLineTools,
                            ProviderLogo = EnumHelper.GetImage(ShipmentTypeCode.UpsOnLineTools)
                        };

                        rates.Add(rateResult);
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

                // Filter out any excluded services, but always include the service that the shipment is configured with
                List<RateResult> finalRatesFilteredByAvailableServices = FilterRatesByExcludedServices(shipment, rates);

                RateGroup finalGroup = new RateGroup(finalRatesFilteredByAvailableServices);

                return finalGroup;
            }
            catch (InvalidPackageDimensionsException ex)
            {
                throw new UpsException(ex.Message, ex);
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
            else
            {
                return "";
            }
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
            List<UpsServiceType> availableServices = shipmentType.GetAvailableServiceTypes()
                .Select(s => (UpsServiceType)s).Union(new List<UpsServiceType> { (UpsServiceType)shipment.Ups.Service }).ToList();

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
                if (upsPackage.PackagingType == (int)UpsPackagingType.Custom)
                {
                    if (!shipmentType.DimensionsAreValid(upsPackage.DimsLength, upsPackage.DimsWidth, upsPackage.DimsHeight))
                    {
                        exceptionMessage += string.Format("Package {0} has invalid dimensions.{1}", packageIndex, Environment.NewLine);
                    }
                }

                packageIndex++;
            }

            if (exceptionMessage.Length > 0)
            {
                exceptionMessage += "Package dimensions must be greater than 0 and not 1x1x1.  ";
                throw new InvalidPackageDimensionsException(exceptionMessage);
            }
        }
    }
}
