﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate;
using ShipWorks.Shipping.Editing;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Stores;
using ShipWorks.Data;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Base class for ShipmentTypes that are for the postal service
    /// </summary>
    public abstract class PostalShipmentType : ShipmentType
    {
        /// <summary>
        /// Create the stamps.com specific customs control
        /// </summary>
        public override CustomsControlBase CreateCustomsControl()
        {
            return new PostalCustomsControl();
        }

        /// <summary>
        /// Ensures that the USPS specific data for the shipment is loaded.  If the data already exists, nothing is done.  It is not refreshed.
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "Postal", typeof(PostalShipmentEntity), refreshIfPresent);
        }

        /// <summary>
        /// Configure properties of a newly created shipment that are not taken care of by profile
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadProfileData(profile, "Postal", typeof(PostalProfileEntity), refreshIfPresent);
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

            UpdateDynamicShipmentData(shipment);

            UpdateTotalWeight(shipment);
        }

        /// <summary>
        /// Update the dyamic data of the shipment
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            // Need to check with the store  to see if anything about the shipment was overridden in case
            // it may have effected the shipping services available (i.e. the eBay GSP program)
            ShipmentEntity overriddenShipment = ShippingManager.GetOverriddenStoreShipment(shipment);

            PostalServiceType serviceType = (PostalServiceType) overriddenShipment.Postal.Service;
            PostalPackagingType packagingType = (PostalPackagingType) overriddenShipment.Postal.PackagingType;
            string countryCode = overriddenShipment.ShipCountryCode;

            // If its domestic ensure a domestic service - use the overridden shipment for comparing the ShipToCountry
            if (PostalUtility.IsDomesticCountry(countryCode) &&
                !PostalUtility.GetDomesticServices(ShipmentTypeCode).Contains(serviceType))
            {
                serviceType = PostalServiceType.PriorityMail;
                shipment.Postal.Service = (int) serviceType;
            }

            // If its international ensure an internatinoal service - use the overridden shipment for comparing the ShipToCountry
            if (!PostalUtility.IsDomesticCountry(countryCode) &&
                !PostalUtility.GetInternationalServices(ShipmentTypeCode).Contains(serviceType))
            {
                serviceType = PostalServiceType.InternationalPriority;
                shipment.Postal.Service = (int) serviceType;
            }

            // Make sure a valid confirmation is selected
            if (!GetAvailableConfirmationTypes(countryCode, serviceType, packagingType).Contains((PostalConfirmationType) shipment.Postal.Confirmation))
            {
                shipment.Postal.Confirmation = (int) GetAvailableConfirmationTypes(countryCode, serviceType, packagingType).First();
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
                new DimensionsAdapter(shipment.Postal));
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
        /// Determines if delivery\signature confirmation is available for the given service
        /// </summary>
        public virtual List<PostalConfirmationType> GetAvailableConfirmationTypes(string countryCode, PostalServiceType service, PostalPackagingType? packaging)
        {
            bool isAvailable = false;

            var always = new List<PostalServiceType>
                {                        
                    PostalServiceType.PriorityMail,
                    PostalServiceType.StandardPost,
                    PostalServiceType.MediaMail,
                    PostalServiceType.LibraryMail,
                    PostalServiceType.CriticalMail,
                    PostalServiceType.ParcelSelect
                };

            // All the DHL services require confirmation
            always.AddRange(EnumHelper.GetEnumList<PostalServiceType>().Where(entry => ShipmentTypeManager.IsEndiciaDhl(entry.Value)).Select(entry => entry.Value));

            if (always.Contains(service))
            {
                isAvailable = true;
            }

            else if (service == PostalServiceType.FirstClass)
            {
                if (packaging == PostalPackagingType.Envelope || packaging == PostalPackagingType.LargeEnvelope)
                {

                }
                else
                {
                    isAvailable = true;
                }
            }

            List<PostalConfirmationType> confirmationTypes = new List<PostalConfirmationType>();

            if (isAvailable)
            {
                confirmationTypes.Add(PostalConfirmationType.Delivery);
                confirmationTypes.Add(PostalConfirmationType.Signature);
            }
            else
            {
                confirmationTypes.Add(PostalConfirmationType.None);
            }

            return confirmationTypes;
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the USPS web tools shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of a WebToolsBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            // We want to return the null broker if there is already an Endicia or Stamps.com
            // account setup, so postal rates for Web Tools aren't used as well (i.e. just use
            // the provider that has an account instead of rates from web tools).
            IBestRateShippingBroker broker = new NullShippingBroker();

            bool stampsAccountsExist = StampsAccountManager.GetAccounts(false).Any();
            bool endiciaAccountsExist = EndiciaAccountManager.GetAccounts(EndiciaReseller.None).Any();

            if (!stampsAccountsExist && !endiciaAccountsExist)
            {
                // There aren't any postal based accounts setup, so we want to see if we should 
                // show counter rates (depending whether Endicia or Stamps.com have been excluded)

                // We need to see which Postal provider to show when signing up for a postal account
                // based on the global shipping settings and best rate settings with preference for Endicia
                ShippingSettingsEntity shippingSettings = ShippingSettings.Fetch();

                if (!shippingSettings.BestRateExcludedTypes.Contains((int)ShipmentTypeCode.Endicia))
                {
                    // Endicia has not been excluded from Best Rate, and there aren't any 
                    // Endicia accounts, so use the counter rates broker for Endicia
                    broker = new EndiciaCounterRatesBroker(new EndiciaAccountRepository());
                }
                else if (!shippingSettings.BestRateExcludedTypes.Contains((int)ShipmentTypeCode.Stamps))
                {
                    // Endicia is not being used in best rate (for whatever reason), and Stamps.com has
                    // not been excluded from Best Rate, so use the counter rates broker for Stamps.com
                    broker = new StampsCounterRatesBroker(new StampsAccountRepository());
                }

                // If neither of the above conditions were satisfied, Endicia and Stamps have both been excluded from Best Rate, so do nothing
                // and just return the null broker
            }

            return broker;
        }

        /// <summary>
        /// Gets the fields used for rating a shipment.
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        protected override IEnumerable<IEntityField2> GetRatingFields(ShipmentEntity shipment)
        {
            List<IEntityField2> fields = new List<IEntityField2>(base.GetRatingFields(shipment));

            fields.AddRange
                (
                    new List<IEntityField2>()
                    {
                        shipment.Postal.Fields[PostalShipmentFields.PackagingType.FieldIndex],
                        shipment.Postal.Fields[PostalShipmentFields.DimsHeight.FieldIndex],
                        shipment.Postal.Fields[PostalShipmentFields.DimsLength.FieldIndex],
                        shipment.Postal.Fields[PostalShipmentFields.DimsWidth.FieldIndex],
                        shipment.Postal.Fields[PostalShipmentFields.DimsAddWeight.FieldIndex],
                        shipment.Postal.Fields[PostalShipmentFields.DimsWeight.FieldIndex],                        
                        shipment.Postal.Fields[PostalShipmentFields.NonMachinable.FieldIndex],
                        shipment.Postal.Fields[PostalShipmentFields.NonRectangular.FieldIndex],
                        shipment.Postal.Fields[PostalShipmentFields.InsuranceValue.FieldIndex]
                    }
                );

            return fields;
        }

        /// <summary>
        /// Builds a RateGroup from a list of express 1 rates
        /// </summary>
        /// <param name="rates">List of rates that should be filtered and added to the group</param>
        /// <param name="express1ShipmentType">Express1 shipment type</param>
        /// <param name="baseShipmentType">Base type of the shipment</param>
        /// <returns></returns>
        protected static RateGroup BuildExpress1RateGroup(IEnumerable<RateResult> rates, ShipmentTypeCode express1ShipmentType, ShipmentTypeCode baseShipmentType)
        {
            // Express1 rates - return rates filtered by what is available to the user
            List<PostalServiceType> availabelServiceTypes =
                PostalUtility.GetDomesticServices(express1ShipmentType)
                    .Concat(PostalUtility.GetInternationalServices(express1ShipmentType))
                    .ToList();

            var validExpress1Rates = rates
                .Where(e => availabelServiceTypes.Contains(((PostalRateSelection)e.OriginalTag).ServiceType))
                .ToList();

            validExpress1Rates.ForEach(e => e.ShipmentType = baseShipmentType);

            return new RateGroup(validExpress1Rates);
        }
    }
}
