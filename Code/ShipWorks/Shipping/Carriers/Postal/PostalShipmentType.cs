﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Editing;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
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
    }
}
