using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.UPS
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
            UpsServiceType upsServiceType = (UpsServiceType) upsShipmentEntity.Service;

            if (UpsUtility.IsUpsSurePostService(upsServiceType) &&
                (shipment.InsuranceProvider == (int) InsuranceProvider.Carrier) &&
                upsShipmentEntity.Packages.Any(p => p.Insurance && p.InsuranceValue > 0))
            {
                throw new CarrierException("UPS declared value is not supported for SurePost shipments. For insurance coverage, go to Shipping Settings and enable ShipWorks Insurance for this carrier.");
            }

            // Make sure package dimensions are valid.
            ValidatePackageDimensions(shipment);

            ConfigureNewUpsPostalLabel(shipment, upsShipmentEntity, upsServiceType);
        }

        /// <summary>
        /// Clear out any values that aren't allowed for SurePost or MI
        /// </summary>
        private static void ConfigureNewUpsPostalLabel(ShipmentEntity shipment, UpsShipmentEntity upsShipmentEntity, UpsServiceType upsServiceType)
        {
            if (UpsUtility.IsUpsMiOrSurePostService(upsServiceType))
            {
                shipment.ReturnShipment = false;
                upsShipmentEntity.ReturnContents = string.Empty;
                upsShipmentEntity.ReturnService = (int) UpsReturnServiceType.ElectronicReturnLabel;
                upsShipmentEntity.ReturnUndeliverableEmail = string.Empty;

                upsShipmentEntity.CodEnabled = false;
                upsShipmentEntity.CodAmount = 0;
                upsShipmentEntity.CodPaymentType = (int) UpsCodPaymentType.Cash;

                upsShipmentEntity.ShipperRelease = false;

                UpsPackageEntity upsPackageEntity = upsShipmentEntity.Packages.FirstOrDefault();
                upsPackageEntity.AdditionalHandlingEnabled = false;
                upsPackageEntity.DryIceEnabled = false;
                upsPackageEntity.DryIceIsForMedicalUse = false;
                upsPackageEntity.DryIceRegulationSet = (int) UpsDryIceRegulationSet.Cfr;
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
                    upsShipmentEntity.DeliveryConfirmation = (int) UpsDeliveryConfirmationType.None;
                    upsShipmentEntity.PayorAccount = string.Empty;
                    upsShipmentEntity.PayorCountryCode = string.Empty;
                    upsShipmentEntity.PayorPostalCode = string.Empty;
                    upsShipmentEntity.PayorType = (int) UpsPayorType.Sender;
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
                if (!UpsUtility.IsUpsMiService((UpsServiceType) shipment.Ups.Service))
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
        private static void ValidatePackageDimensions(ShipmentEntity shipment)
        {
            StringBuilder exceptionMessage = new StringBuilder();
            int packageIndex = 1;

            foreach (UpsPackageEntity upsPackage in shipment.Ups.Packages)
            {
                if (!DimensionsAreValid(upsPackage))
                {
                    exceptionMessage.AppendLine($"Package {packageIndex} has invalid dimensions.");
                }

                packageIndex++;
            }

            if (exceptionMessage.Length > 0)
            {
                exceptionMessage.Append("Package dimensions must be greater than 0 and not 1x1x1.  ");
                throw new InvalidPackageDimensionsException(exceptionMessage.ToString());
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