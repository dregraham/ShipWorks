﻿using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Tracking;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Base class for ShipmentTypes that are for the postal service
    /// </summary>
    public abstract class PostalShipmentType : ShipmentType
    {
        /// <summary>
        /// Create the USPS specific customs control
        /// </summary>
        public override CustomsControlBase CreateCustomsControl()
        {
            return new PostalCustomsControl();
        }

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            if (shipment.Postal == null)
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }

            return new List<IPackageAdapter>()
            {
                new PostalPackageAdapter(shipment, shipment.Postal)
            };
        }

        /// <summary>
        /// Configure the properties of a newly created shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            if (shipment.Postal == null)
            {
                shipment.Postal = new PostalShipmentEntity(shipment.ShipmentID);
            }

            shipment.Postal.Insurance = false;

            shipment.Postal.CustomsRecipientTin = string.Empty;

            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Ensures that the USPS specific data for the shipment is loaded.  If the data already exists, nothing is done.  It is not refreshed.
        /// </summary>
        protected override void LoadShipmentDataInternal(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "Postal", typeof(PostalShipmentEntity), refreshIfPresent);
        }

        /// <summary>
        /// Gets the service types that have been available for this shipment type (i.e have not
        /// been excluded). The integer values are intended to correspond to the appropriate
        /// enumeration values of the specific shipment type (i.e. the integer values would
        /// correspond to PostalServiceType values for this shipment type)
        /// </summary>
        public override IEnumerable<int> GetAvailableServiceTypes(IExcludedServiceTypeRepository repository)
        {
            IEnumerable<int> allServiceTypes = PostalUtility.GetDomesticServices(ShipmentTypeCode)
                .Union(PostalUtility.GetInternationalServices(ShipmentTypeCode))
                .Select(service => (int) service);

            return allServiceTypes.Except(GetExcludedServiceTypes(repository));
        }

        /// <summary>
        /// Gets the package types that have been available for this shipment type
        /// </summary>
        public override IEnumerable<int> GetAvailablePackageTypes(IExcludedPackageTypeRepository repository)
        {
            IEnumerable<PostalPackagingType> packageTypes = EnumHelper.GetEnumList<PostalPackagingType>()
                .Select(x => x.Value)
                .Where(x => !EnumHelper.GetDeprecated(x))
                .Except(GetExcludedPackageTypes(repository).Cast<PostalPackagingType>());

            // The cubic packaging type is only used by Express1/Endicia
            return packageTypes.Except(new List<PostalPackagingType> { PostalPackagingType.Cubic }).Cast<int>();
        }

        /// <summary>
        /// Gets the AvailablePackageTypes for this shipment type and shipment along with their descriptions.
        /// </summary>
        public override Dictionary<int, string> BuildPackageTypeDictionary(List<ShipmentEntity> shipments, IExcludedPackageTypeRepository excludedPackageTypeRepository)
        {
            List<PostalPackagingType> selectedPackagingTypes = new List<PostalPackagingType>();
            bool isFirstClass = false;
            foreach (var postalShipment in shipments.Select(x => x.Postal).Where(x => x != null))
            {
                selectedPackagingTypes.Add((PostalPackagingType) postalShipment.PackagingType);
                isFirstClass |= (PostalServiceType) postalShipment.Service == PostalServiceType.FirstClass;
            }

            var exclusions = new List<PostalPackagingType>();
            if (ShipmentTypeCode != ShipmentTypeCode.Express1Endicia)
            {
                // Only Express 1 Endicia should see the cubic packaging type
                exclusions.Add(PostalPackagingType.Cubic);
            }

            if (isFirstClass)
            {
                //Package is not available with FirstClass anymore
                exclusions.Add(PostalPackagingType.Package);
            }

            return GetAvailablePackageTypes(excludedPackageTypeRepository)
                .Cast<PostalPackagingType>()
                .Union(selectedPackagingTypes)
                .Except(exclusions)
                .ToDictionary(t => (int) t, t => EnumHelper.GetDescription(t));
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        public override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            if (SupportsAccountAsOrigin)
            {
                profile.OriginID = (int) ShipmentOriginSource.Account;
            }
            else
            {
                long originID = ShippingOriginManager.Origins.Count > 0 ? ShippingOriginManager.Origins[0].ShippingOriginID : (long) ShipmentOriginSource.Store;
                profile.OriginID = originID;
            }

            PostalProfileEntity postal = profile.Postal;

            postal.Service = (int) PostalServiceType.PriorityMail;
            postal.Confirmation = (int) PostalConfirmationType.Delivery;

            postal.PackagingType = (int) PostalPackagingType.Package;
            postal.NonRectangular = false;
            postal.NonMachinable = false;

            postal.CustomsContentType = (int) PostalCustomsContentType.Merchandise;
            postal.CustomsContentDescription = "Other";
            postal.CustomsRecipientTin = string.Empty;

            postal.ExpressSignatureWaiver = false;

            postal.SortType = (int) PostalSortType.Nonpresorted;
            postal.EntryFacility = (int) PostalEntryFacility.Other;

            postal.Memo1 = String.Empty;
            postal.Memo2 = String.Empty;
            postal.Memo3 = String.Empty;

            postal.NoPostage = false;
        }

        /// <summary>
        /// Update the dynamic data of the shipment
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            RectifyCarrierSpecificData(shipment);
        }

        /// <summary>
        /// Update the postal details of a shipment
        /// </summary>
        public override void RectifyCarrierSpecificData(ShipmentEntity shipment)
        {
            // Need to check with the store  to see if anything about the shipment was overridden in case
            // it may have effected the shipping services available (i.e. the eBay GSP program)
            ShipmentEntity overriddenShipment = ShippingManager.GetOverriddenStoreShipment(shipment);

            // A null reference error was being thrown.  Discovered by Crash Reports.
            // Let's figure out what is null....
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            if (shipment.Postal == null)
            {
                throw new NullReferenceException("shipment.Postal cannot be null.");
            }

            if (overriddenShipment == null)
            {
                throw new NullReferenceException("overriddenShipment cannot be null.");
            }

            if (overriddenShipment.Postal == null)
            {
                throw new NullReferenceException("overriddenShipment.Postal cannot be null.");
            }

            PostalServiceType serviceType = (PostalServiceType) overriddenShipment.Postal.Service;
            PostalPackagingType packagingType = (PostalPackagingType) overriddenShipment.Postal.PackagingType;

            // If its domestic ensure a domestic service - use the overridden shipment for comparing the ShipToCountry
            if (overriddenShipment.ShipPerson.IsDomesticCountry())
            {
                List<PostalServiceType> domesticServices = PostalUtility.GetDomesticServices(ShipmentTypeCode);

                if (domesticServices == null)
                {
                    throw new NullReferenceException("domesticServices was null.");
                }

                if (!domesticServices.Contains(serviceType))
                {
                    serviceType = PostalServiceType.PriorityMail;
                    shipment.Postal.Service = (int) serviceType;
                }
            }
            else
            {
                // If its international ensure an international service - use the overridden shipment for comparing the ShipToCountry
                List<PostalServiceType> internationalServices = PostalUtility.GetInternationalServices(ShipmentTypeCode);

                if (internationalServices == null)
                {
                    throw new NullReferenceException("internationalServices was null.");
                }

                if (!internationalServices.Contains(serviceType))
                {
                    serviceType = PostalServiceType.InternationalPriority;
                    shipment.Postal.Service = (int) serviceType;
                }
            }

            List<PostalConfirmationType> availableConfirmationTypes = GetAvailableConfirmationTypes(overriddenShipment.ShipCountryCode, serviceType, packagingType);
            if (availableConfirmationTypes == null)
            {
                throw new NullReferenceException("availableConfirmationTypes was null.");
            }

            // Make sure a valid confirmation is selected
            if (!availableConfirmationTypes.Contains((PostalConfirmationType) shipment.Postal.Confirmation))
            {
                shipment.Postal.Confirmation = (int) availableConfirmationTypes.First();
            }

            // Update the dimensions info
            DimensionsManager.UpdateDimensions(new DimensionsAdapter(shipment.Postal));
        }

        /// <summary>
        /// Get the dims weight from a shipment, if any
        /// </summary>
        protected override double GetDimsWeight(IShipmentEntity shipment) =>
            shipment.Postal?.DimsAddWeight == true ? shipment.Postal.DimsWeight : 0;

        /// <summary>
        /// Get the parcel data for the shipment
        /// </summary>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            return new ShipmentParcel(shipment, null,
                new InsuranceChoice(shipment, shipment.Postal, shipment.Postal, null),
                new DimensionsAdapter(shipment.Postal))
            {
                TotalWeight = shipment.TotalWeight
            };
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment) =>
            GetServiceDescriptionInternal((PostalServiceType) shipment.Postal.Service);

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(string serviceCode) =>
            Functional.ParseInt(serviceCode)
                .Match(x => GetServiceDescriptionInternal((PostalServiceType) x), _ => "Unknown");

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        private string GetServiceDescriptionInternal(PostalServiceType service) =>
            string.Format("USPS {0}", EnumHelper.GetDescription(service));

        /// <summary>
        /// Get the USPS shipment details
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            ShipmentCommonDetail commonDetail = new ShipmentCommonDetail();

            PostalShipmentEntity postal = shipment.Postal;

            commonDetail.ServiceType = postal.Service;

            commonDetail.PackagingType = postal.PackagingType;
            commonDetail.PackageLength = postal.DimsLength;
            commonDetail.PackageWidth = postal.DimsWidth;
            commonDetail.PackageHeight = postal.DimsHeight;

            return commonDetail;
        }

        /// <summary>
        /// Track the given usps shipment
        /// </summary>
        public override TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            return PostalWebClientTracking.TrackShipment(shipment.TrackingNumber);
        }

        /// <summary>
        /// Get postal Tracking link
        /// </summary>
        protected override string GetCarrierTrackingUrlInternal(ShipmentEntity shipment)
        {
            if (ShipmentTypeManager.IsDhlSmartMail((PostalServiceType) shipment.Postal.Service))
            {
                return $"http://webtrack.dhlglobalmail.com/?mobile=&amp;trackingnumber={shipment.TrackingNumber}";
            }

            return $"https://tools.usps.com/go/TrackConfirmAction.action?tLabels={shipment.TrackingNumber}";
        }

        /// <summary>
        /// Gets all of the confirmation types that are available to a particular implementation of PostalShipmentType. The types available
        /// to all postal implementations are available here. Derived classes may have additional confirmation types.
        /// </summary>
        /// <returns>A collection of all the confirmation types that are available to a Express1 (USPS) shipment.</returns>
        public virtual IEnumerable<PostalConfirmationType> GetAllConfirmationTypes()
        {
            return new List<PostalConfirmationType>
            {
                PostalConfirmationType.None,
                PostalConfirmationType.Delivery,
                PostalConfirmationType.Signature,
                PostalConfirmationType.AdultSignatureRequired,
                PostalConfirmationType.AdultSignatureRestricted
            };
        }

        /// <summary>
        /// Determines if delivery\signature confirmation is available for the given service
        /// </summary>
        public virtual List<PostalConfirmationType> GetAvailableConfirmationTypes(string countryCode, PostalServiceType service, PostalPackagingType? packaging)
        {
            bool confirmationTypesAvailable = false;

            var servicesThatAllowConfirmationTypes = new List<PostalServiceType>
            {
                PostalServiceType.PriorityMail,
                PostalServiceType.StandardPost,
                PostalServiceType.MediaMail,
                PostalServiceType.LibraryMail,
                PostalServiceType.CriticalMail,
                PostalServiceType.ParcelSelect
            };

            // All the DHL services require confirmation
            servicesThatAllowConfirmationTypes.AddRange(EnumHelper.GetEnumList<PostalServiceType>().Where(entry => ShipmentTypeManager.IsEndiciaDhl(entry.Value)).Select(entry => entry.Value));

            if (servicesThatAllowConfirmationTypes.Contains(service))
            {
                confirmationTypesAvailable = true;
            }
            else if (service == PostalServiceType.FirstClass && packaging != PostalPackagingType.Envelope && packaging != PostalPackagingType.LargeEnvelope)
            {
                confirmationTypesAvailable = true;
            }

            List<PostalConfirmationType> confirmationTypes = new List<PostalConfirmationType>();

            if (confirmationTypesAvailable)
            {
                confirmationTypes.Add(PostalConfirmationType.Delivery);
                confirmationTypes.Add(PostalConfirmationType.Signature);
            }

            IEnumerable<PostalServicePackagingCombination> adultSignatureAllowed = GetAdultSignatureServiceAndPackagingCombinations();
            if (packaging != null && adultSignatureAllowed.Any(asr => asr.ServiceType == service && asr.PackagingType == packaging))
            {
                if (!confirmationTypesAvailable) // No confirmation types available, they should have an option other than adult signature
                {
                    confirmationTypes.Add(PostalConfirmationType.None);
                }
                confirmationTypes.Add(PostalConfirmationType.AdultSignatureRequired);
                confirmationTypes.Add(PostalConfirmationType.AdultSignatureRestricted);
            }

            if (confirmationTypes.None())
            {
                confirmationTypes.Add(PostalConfirmationType.None);
            }

            return confirmationTypes;
        }

        /// <summary>
        /// Is the given shipment domestic?
        /// </summary>
        public override bool IsDomestic(IShipmentEntity shipmentEntity)
        {
            return base.IsDomestic(shipmentEntity) || IsShipmentBetweenUnitedStatesAndPuertoRico(shipmentEntity);
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the USPS web tools shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of a WebToolsBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment, IBestRateExcludedAccountRepository bestRateExcludedAccountRepository)
        {
            return new NullShippingBroker();
        }

        /// <summary>
        /// Add adult signature restricted values
        /// </summary>
        private static IEnumerable<PostalServicePackagingCombination> GetAdultSignatureServiceAndPackagingCombinations()
        {
            List<PostalServicePackagingCombination> adultSignatureAllowed = new List<PostalServicePackagingCombination>();

            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.LargeEnvelope));
            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRateEnvelope));
            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.Package));
            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRateSmallBox));
            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRateMediumBox));
            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRateLargeBox));
            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRatePaddedEnvelope));
            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRateLegalEnvelope));
            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.RateRegionalBoxA));
            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.RateRegionalBoxB));
            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.RateRegionalBoxC));

            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.LargeEnvelope));
            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.Package));
            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.FlatRateEnvelope));
            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.FlatRateMediumBox));
            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.FlatRatePaddedEnvelope));
            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.FlatRateLegalEnvelope));

            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ParcelSelect, PostalPackagingType.Package));

            adultSignatureAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.CriticalMail, PostalPackagingType.LargeEnvelope));

            return adultSignatureAllowed;
        }

        /// <summary>
        /// Indicates if the combination of country, service, and packaging qualifies for the free international delivery confirmation
        /// </summary>
        public bool IsFreeInternationalDeliveryConfirmation(string countryCode, PostalServiceType serviceType, PostalPackagingType? packagingType)
        {
            if (CountriesEligibleForFreeInternationalDeliveryConfirmation().Contains(countryCode, StringComparer.OrdinalIgnoreCase))
            {
                if (packagingType == PostalPackagingType.FlatRateSmallBox)
                {
                    return true;
                }

                if (serviceType == PostalServiceType.InternationalPriority)
                {
                    switch (packagingType)
                    {
                        case PostalPackagingType.FlatRateEnvelope:
                        case PostalPackagingType.FlatRateLegalEnvelope:
                        case PostalPackagingType.FlatRatePaddedEnvelope:
                            return true;
                    }
                }

                if (serviceType == PostalServiceType.InternationalFirst)
                {
                    if (!PostalUtility.IsEnvelopeOrFlat(packagingType))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Does the given rate match the specified service and packaging
        /// </summary>
        public bool DoesRateMatchServiceAndPackaging(PostalRateSelection rate, PostalServiceType serviceType) =>
            rate.ServiceType == serviceType;

        /// <summary>
        /// Returns a list of countries eligible for free international delivery confirmation.
        /// </summary>
        protected virtual List<string> CountriesEligibleForFreeInternationalDeliveryConfirmation()
        {
            // Countries taken from this list on 10/26/2020:
            // https://faq.usps.com/s/article/Electronic-USPS-Delivery-Confirmation-International
            List<string> eligibleCountryCodes = new List<string>
            {
                "AU", // Australia
                "BE", // Belgium
                "CA", // Canada 
                "HR", // Croatia
                "CY", // Cyprus
                "DK", // Denmark 
                "EE", // Estonia
                "FR", // France
                "DE", // Germany
                "GI", // Gibralter
                "HK", // Hong Kong
                "IS", // Iceland
                "ID", // Indonesia
                "IL", // Israel
                "JP", // Japan
                "LT", // Lithuania
                "LU", // Luxumbourg
                "MY", // Malaysia
                "NL", // Netherlands
                "NZ", // New Zealand
                "PL", // Poland
                "PT", // Portugal
                "SG", // Singapore
                "TH", // Thailand
                "GB", // Great Britain
                "UK", // United Kingdom
                "NB", // Northern Ireland
            };

            return eligibleCountryCodes;
        }
    }
}
