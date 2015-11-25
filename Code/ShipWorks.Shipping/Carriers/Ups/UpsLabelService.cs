﻿using System;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// Base Label service used by both Ups Online Tools and Ups Worldship
    /// </summary>
    public class UpsLabelService : ILabelService
    {
        /// <summary>
        /// Creates the label
        /// </summary>
        public virtual void Create(ShipmentEntity shipment)
        {
            UpsShipmentEntity upsShipmentEntity = shipment.Ups;
            UpsServiceType upsServiceType = (UpsServiceType)upsShipmentEntity.Service;

            if (UpsUtility.IsUpsSurePostService(upsServiceType) &&
                (shipment.InsuranceProvider == (int)InsuranceProvider.Carrier) &&
                upsShipmentEntity.Packages.Any(p => p.Insurance && p.InsuranceValue > 0))
            {
                throw new CarrierException("UPS declared value is not supported for SurePost shipments. For insurance coverage, go to Shipping Settings and enable ShipWorks Insurance for this carrier.");
            }

            // Make sure package dimensions are valid.
            ValidatePackageDimensions(shipment);

            // Clear out any values that aren't allowed for SurePost or MI
            if (UpsUtility.IsUpsMiOrSurePostService(upsServiceType))
            {
                shipment.ReturnShipment = false;
                upsShipmentEntity.ReturnContents = string.Empty;
                upsShipmentEntity.ReturnService = (int)UpsReturnServiceType.ElectronicReturnLabel;
                upsShipmentEntity.ReturnUndeliverableEmail = string.Empty;

                upsShipmentEntity.CodEnabled = false;
                upsShipmentEntity.CodAmount = 0;
                upsShipmentEntity.CodPaymentType = (int)UpsCodPaymentType.Cash;

                upsShipmentEntity.ShipperRelease = false;

                UpsPackageEntity upsPackageEntity = upsShipmentEntity.Packages.FirstOrDefault();
                upsPackageEntity.AdditionalHandlingEnabled = false;
                upsPackageEntity.DryIceEnabled = false;
                upsPackageEntity.DryIceIsForMedicalUse = false;
                upsPackageEntity.DryIceRegulationSet = (int)UpsDryIceRegulationSet.Cfr;
                upsPackageEntity.DryIceWeight = 0;
                upsPackageEntity.VerbalConfirmationEnabled = false;
                upsPackageEntity.VerbalConfirmationName = string.Empty;
                upsPackageEntity.VerbalConfirmationPhone = string.Empty;
                upsPackageEntity.VerbalConfirmationPhoneExtension = string.Empty;

                // Clear out any specific to MI
                if (UpsUtility.IsUpsMiService(upsServiceType))
                {
                    upsShipmentEntity.CarbonNeutral = false;
                    upsShipmentEntity.ReferenceNumber = string.Empty;
                    upsShipmentEntity.ReferenceNumber2 = string.Empty;
                }

                // Clear out any specific to SurePost
                if (UpsUtility.IsUpsSurePostService(upsServiceType))
                {
                    upsShipmentEntity.DeliveryConfirmation = (int)UpsDeliveryConfirmationType.None;
                    upsShipmentEntity.PayorAccount = string.Empty;
                    upsShipmentEntity.PayorCountryCode = string.Empty;
                    upsShipmentEntity.PayorPostalCode = string.Empty;
                    upsShipmentEntity.PayorType = (int)UpsPayorType.Sender;
                }
            }
        }

        /// <summary>
        /// Voids the label
        /// </summary>
        public virtual void Void(ShipmentEntity shipment)
        {
            try
            {
                if (!UpsUtility.IsUpsMiService((UpsServiceType)shipment.Ups.Service))
                {
                    UpsApiVoidClient.VoidShipment(shipment);
                }
            }
            catch (UpsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
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
                    if (!DimensionsAreValid(upsPackage.DimsLength, upsPackage.DimsWidth, upsPackage.DimsHeight))
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

        /// <summary>
        /// Check to see if a package dimensions are valid for carriers that require dimensions.
        /// </summary>
        /// <returns>True if the dimensions are valid.  False otherwise.</returns>
        public virtual bool DimensionsAreValid(double length, double width, double height)
        {
            if (length <= 0 || width <= 0 || height <= 0)
            {
                return false;
            }

            // Some customers may have 1x1x1 in a profile to get around carriers that used to require dimensions.
            // This is no longer valid due to new dimensional weight requirements.
            if (length == 1.0 && width == 1.0 && height == 1.0)
            {
                return false;
            }

            return true;
        }
    }
}