using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.ShipSense.Packaging;
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
                new PostalPackageAdapter(shipment)
            };
        }

        /// <summary>
        /// Ensures that the USPS specific data for the shipment is loaded.  If the data already exists, nothing is done.  It is not refreshed.
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "Postal", typeof(PostalShipmentEntity), refreshIfPresent);
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadProfileData(profile, "Postal", typeof(PostalProfileEntity), refreshIfPresent);
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
                .Select(service => (int)service);

            return allServiceTypes.Except(GetExcludedServiceTypes(repository));
        }

        ///// <summary>
        ///// Gets the AvailableServiceTypes for this shipment type and shipment along with their descriptions.
        ///// </summary>
        //public override Dictionary<int, string> BuildServiceTypeDictionary(List<ShipmentEntity> shipments, IExcludedServiceTypeRepository excludedServiceTypeRepository)
        //{
        //    List<ShipmentEntity> overriddenShipments = shipments.Select(ShippingManager.GetOverriddenStoreShipment).ToList();

        //    bool allInternational = overriddenShipments.None(s => s.ShipPerson.IsDomesticCountry());
        //    bool allDomestic = overriddenShipments.All(s => s.ShipPerson.IsDomesticCountry());

        //    List<PostalServiceType> availableServices = GetAvailableServiceTypes(excludedServiceTypeRepository).Select(s => (PostalServiceType)s).ToList();

        //    // If they are all international we can load up all the international services
        //    if (allInternational)
        //    {
        //        // We need to build the list of international services to use as the data source taking into account only the available 
        //        // service types as well as the service type that the shipment is already configured with
        //        List<PostalServiceType> allInternationalServices = PostalUtility.GetInternationalServices(ShipmentTypeCode);
        //        List<PostalServiceType> internationalServicesToLoad = allInternationalServices.Intersect(availableServices).ToList();
        //        if (shipments.Any())
        //        {
        //            // Always include the service type that the shipment is currently configured in the 
        //            // event the shipment was configured prior to a service being excluded
        //            internationalServicesToLoad = internationalServicesToLoad.Union(allInternationalServices.Intersect(new List<PostalServiceType> { (PostalServiceType)shipments.First().Postal.Service })).ToList();
        //        }

        //        // Bind the drop down to the international services
        //        return internationalServicesToLoad.ToDictionary(s => (int) s, PostalUtility.GetPostalServiceTypeDescription);
        //    }
        //    // If they are all domestic we can load up all the domestic services
        //    else if (allDomestic)
        //    {
        //        // We need to build the list of domestic services to use as the data source taking into account only the available 
        //        // service types as well as the service type that the shipment is already configured with
        //        List<PostalServiceType> allDomesticServices = PostalUtility.GetDomesticServices(ShipmentTypeCode);
        //        List<PostalServiceType> domesticServicesToLoad = allDomesticServices.Intersect(availableServices).ToList();
        //        if (shipments.Any())
        //        {
        //            // Always include the service type that the shipment is currently configured in the 
        //            // event the shipment was configured prior to a service being excluded
        //            domesticServicesToLoad = domesticServicesToLoad.Union(allDomesticServices.Intersect(new List<PostalServiceType> { (PostalServiceType)shipments.First().Postal.Service })).ToList();
        //        }

        //        return domesticServicesToLoad.ToDictionary(s => (int) s, PostalUtility.GetPostalServiceTypeDescription);

        //    }
        //    else
        //    {
        //        // Otherwise there is nothing to choose from
        //        return new Dictionary<int, string>();
        //    }
        //}

        /// <summary>
        /// Gets the package types that have been available for this shipment type
        /// </summary>
        public override IEnumerable<int> GetAvailablePackageTypes(IExcludedPackageTypeRepository repository)
        {
            IEnumerable<PostalPackagingType> packageTypes = EnumHelper.GetEnumList<PostalPackagingType>()
                .Select(x => x.Value)
                .Except(GetExcludedPackageTypes(repository).Cast<PostalPackagingType>());
            
            // The cubic packaging type is only used by Express1/Endicia
            return packageTypes.Except(new List<PostalPackagingType> { PostalPackagingType.Cubic }).Cast<int>();
        }

        /// <summary>
        /// Gets the AvailablePackageTypes for this shipment type and shipment along with their descriptions.
        /// </summary>
        public override Dictionary<int, string> BuildPackageTypeDictionary(List<ShipmentEntity> shipments, IExcludedPackageTypeRepository excludedPackageTypeRepository)
        {
            return GetAvailablePackageTypes(excludedPackageTypeRepository)
                .Cast<PostalPackagingType>()
                .Union(shipments.Select(x => x.Postal)
                    .Where(x => x != null)
                    .Select(x => (PostalPackagingType) x.PackagingType))
                // Only Express 1 Endicia should see the cubic packaging type
                .Where(p => (p != PostalPackagingType.Cubic || ShipmentTypeCode == ShipmentTypeCode.Express1Endicia))
                .ToDictionary(t => (int) t, t => EnumHelper.GetDescription(t));
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        protected override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
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
            postal.Weight = 0;

            postal.DimsProfileID = 0;
            postal.DimsLength = 0;
            postal.DimsWidth = 0;
            postal.DimsHeight = 0;
            postal.DimsWeight = 0;
            postal.DimsAddWeight = true;

            postal.PackagingType = (int) PostalPackagingType.Package;
            postal.NonRectangular = false;
            postal.NonMachinable = false;

            postal.CustomsContentType = (int) PostalCustomsContentType.Merchandise;
            postal.CustomsContentDescription = "Other";

            postal.ExpressSignatureWaiver = false;

            postal.SortType = (int) PostalSortType.Nonpresorted;
            postal.EntryFacility = (int) PostalEntryFacility.Other;

            postal.Memo1 = String.Empty;
            postal.Memo2 = String.Empty;
            postal.Memo3 = String.Empty;

            postal.NoPostage = false;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            base.ApplyProfile(shipment, profile);

            PostalShipmentEntity postalShipment = shipment.Postal;
            PostalProfileEntity postalProfile = profile.Postal;

            ShippingProfileUtility.ApplyProfileValue(postalProfile.Service, postalShipment, PostalShipmentFields.Service);
            ShippingProfileUtility.ApplyProfileValue(postalProfile.Confirmation, postalShipment, PostalShipmentFields.Confirmation);

            // Special case - only apply if the weight is not zero.  This prevents the weight entry from the default profile from overwriting the prefilled weight from products.
            if (postalProfile.Weight != null && postalProfile.Weight.Value != 0)
            {
                ShippingProfileUtility.ApplyProfileValue(postalProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }

            ShippingProfileUtility.ApplyProfileValue(postalProfile.DimsProfileID, postalShipment, PostalShipmentFields.DimsProfileID);
            if (postalProfile.DimsProfileID != null)
            {
                ShippingProfileUtility.ApplyProfileValue(postalProfile.DimsLength, postalShipment, PostalShipmentFields.DimsLength);
                ShippingProfileUtility.ApplyProfileValue(postalProfile.DimsWidth, postalShipment, PostalShipmentFields.DimsWidth);
                ShippingProfileUtility.ApplyProfileValue(postalProfile.DimsHeight, postalShipment, PostalShipmentFields.DimsHeight);
                ShippingProfileUtility.ApplyProfileValue(postalProfile.DimsWeight, postalShipment, PostalShipmentFields.DimsWeight);
                ShippingProfileUtility.ApplyProfileValue(postalProfile.DimsAddWeight, postalShipment, PostalShipmentFields.DimsAddWeight);
            }

            ShippingProfileUtility.ApplyProfileValue(postalProfile.PackagingType, postalShipment, PostalShipmentFields.PackagingType);
            ShippingProfileUtility.ApplyProfileValue(postalProfile.NonRectangular, postalShipment, PostalShipmentFields.NonRectangular);
            ShippingProfileUtility.ApplyProfileValue(postalProfile.NonMachinable, postalShipment, PostalShipmentFields.NonMachinable);

            ShippingProfileUtility.ApplyProfileValue(postalProfile.CustomsContentType, postalShipment, PostalShipmentFields.CustomsContentType);
            ShippingProfileUtility.ApplyProfileValue(postalProfile.CustomsContentDescription, postalShipment, PostalShipmentFields.CustomsContentDescription);

            ShippingProfileUtility.ApplyProfileValue(postalProfile.ExpressSignatureWaiver, postalShipment, PostalShipmentFields.ExpressSignatureWaiver);

            ShippingProfileUtility.ApplyProfileValue(postalProfile.SortType, postalShipment, PostalShipmentFields.SortType);
            ShippingProfileUtility.ApplyProfileValue(postalProfile.EntryFacility, postalShipment, PostalShipmentFields.EntryFacility);

            ShippingProfileUtility.ApplyProfileValue(postalProfile.Memo1, postalShipment, PostalShipmentFields.Memo1);
            ShippingProfileUtility.ApplyProfileValue(postalProfile.Memo2, postalShipment, PostalShipmentFields.Memo2);
            ShippingProfileUtility.ApplyProfileValue(postalProfile.Memo3, postalShipment, PostalShipmentFields.Memo3);

            ShippingProfileUtility.ApplyProfileValue(postalProfile.NoPostage, postalShipment, PostalShipmentFields.NoPostage);

            UpdateDynamicShipmentData(shipment);

            UpdateTotalWeight(shipment);
        }

        /// <summary>
        /// Update the dyamic data of the shipment
        /// </summary>
        [NDependIgnoreLongMethod]
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            // Need to check with the store  to see if anything about the shipment was overridden in case
            // it may have effected the shipping services available (i.e. the eBay GSP program)
            ShipmentEntity overriddenShipment = ShippingManager.GetOverriddenStoreShipment(shipment);

            // A null reference error was being thrown.  Discoverred by Crash Reports.
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
                    shipment.Postal.Service = (int)serviceType;
                }
            }
            else
            {
                // If its international ensure an internatinoal service - use the overridden shipment for comparing the ShipToCountry

                List<PostalServiceType> internationalServices = PostalUtility.GetInternationalServices(ShipmentTypeCode);

                if (internationalServices == null)
                {
                    throw new NullReferenceException("internationalServices was null.");
                }

                if (!internationalServices.Contains(serviceType))
                {
                    serviceType = PostalServiceType.InternationalPriority;
                    shipment.Postal.Service = (int)serviceType;
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
                shipment.Postal.Confirmation = (int)availableConfirmationTypes.First();
            }

            // Update the dimensions info
            DimensionsManager.UpdateDimensions(new DimensionsAdapter(shipment.Postal));

            // Postal only has the option to use ShipWorks Insurance
            shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;
        }

        /// <summary>
        /// Update the total weight of the shipment
        /// </summary>
        public override void UpdateTotalWeight(ShipmentEntity shipment)
        {
            shipment.TotalWeight = shipment.ContentWeight;

            if (shipment.Postal.DimsAddWeight)
            {
                shipment.TotalWeight += shipment.Postal.DimsWeight;
            }
        }

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
                new InsuranceChoice(shipment, shipment, shipment.Postal, null),
                new DimensionsAdapter(shipment.Postal))
            {
                TotalWeight = shipment.TotalWeight
            };
        }

        /// <summary>
        /// Get the service description for the shipment
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            return string.Format("USPS {0}", EnumHelper.GetDescription((PostalServiceType) shipment.Postal.Service));
        }

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
        public override bool IsDomestic(ShipmentEntity shipmentEntity)
        {
            return base.IsDomestic(shipmentEntity) || IsShipmentBetweenUnitedStatesAndPuertoRico(shipmentEntity);
        }
		
        /// <summary>
        /// Gets an instance to the best rate shipping broker for the USPS web tools shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of a WebToolsBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            // We want to return the null broker if there is already an Endicia or USPS
            // account setup, so postal rates for Web Tools aren't used as well (i.e. just use
            // the provider that has an account instead of rates from web tools).
            IBestRateShippingBroker broker = new NullShippingBroker();

            bool uspsExpeditedAccountsExist = UspsAccountManager.UspsAccounts.Any();
            bool uspsAccountsExist = UspsAccountManager.GetAccounts(UspsResellerType.None).Any();

            if (!uspsAccountsExist && !uspsExpeditedAccountsExist)
            {
                // There aren't any postal based accounts setup, so we want to see if we should 
                // show counter rates (depending whether USPS has been excluded)

                ShippingSettingsEntity shippingSettings = ShippingSettings.Fetch();

                if (!shippingSettings.BestRateExcludedTypes.Contains((int)ShipmentTypeCode.Usps))
                {
                    // USPS has not been excluded from Best Rate, and there aren't any 
                    // USPS accounts, so use the counter rates broker for USPS
                    broker = new UspsCounterRatesBroker(new UspsCounterRateAccountRepository(TangoCredentialStore.Instance));
                }
            }

            return broker;
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
        public bool IsFreeInternationalDeliveryConfirmation(string countryCode, PostalServiceType serviceType, PostalPackagingType packagingType)
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
        /// Returns a list of countries eligible for free international delivery confirmation.
        /// </summary>
        protected virtual List<string> CountriesEligibleForFreeInternationalDeliveryConfirmation()
        {
            // Allowable country codes include Australia, Belgium, Brazil, Canada, Croatia, Denmark, Estonia, Finland,
            // France, Germany, Gibraltar, Great Britain, Hungary, Northern Ireland, Israel, Italy, Latvia, Lithuania, Luxembourg, Malaysia, 
            // Malta, Netherlands, New Zealand, Portugal, Singapore, Spain, Switzerland
            List<string> eligibleCountryCodes = new List<string>
            {
                "AU", "BE", "BR", "CA", "HR", "DK", "FR", "DE", "GB", "NB", "IL", "NL", "NZ", "ES", "CH",
                "EE", "FI", "GI", "HU", "IT", "LV", "LT", "LU", "MY", "MT", "PT", "SG"
            };

            return eligibleCountryCodes;
        }
    }
}
