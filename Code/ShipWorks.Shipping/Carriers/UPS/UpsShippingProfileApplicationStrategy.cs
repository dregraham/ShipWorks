using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// Ups shipping profile application strategy
    /// </summary>
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.UpsOnLineTools)]
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.UpsWorldShip)]
    public class UpsShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        private readonly ICarrierAccountRetriever<UpsAccountEntity, IUpsAccountEntity> accountRetriever;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IInsuranceUtility insuranceUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipmentTypeManager"></param>
        public UpsShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager,
            ICarrierAccountRetriever<UpsAccountEntity, IUpsAccountEntity> accountRetriever,
            ISqlAdapterFactory sqlAdapterFactory,
            IInsuranceUtility insuranceUtility)
            : base(shipmentTypeManager)
        {
            this.accountRetriever = accountRetriever;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.insuranceUtility = insuranceUtility;
        }

        /// <summary>
        /// Apply the profile to the shipment
        /// </summary>
        public override void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            bool changedPackageWeights = ApplyProfilesPackages(profile, shipment);
            changedPackageWeights |= RemoveExcessPackages(shipment.Ups, profile.Packages.Count());

            base.ApplyProfile(profile, shipment);
            ApplyProfileValue(profile.Ups.ResidentialDetermination, shipment, ShipmentFields.ResidentialDetermination);

            // Apply the UPS specific profile stuff to the UPS Shipment
            ApplyProfile(profile.Ups, shipment.Ups);

            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);

            if (changedPackageWeights)
            {
                shipmentType.UpdateTotalWeight(shipment);
            }

            shipmentType.UpdateDynamicShipmentData(shipment);
        }

        /// <summary>
        /// Apply the UpsProfile to the UpsShipment
        /// </summary>
        private void ApplyProfile(IUpsProfileEntity upsProfile, UpsShipmentEntity upsShipment)
        {
            ApplyProfileValue(GetAccountID(upsProfile), upsShipment, UpsShipmentFields.UpsAccountID);

            ApplyProfileValue(upsProfile.DeliveryConfirmation, upsShipment, UpsShipmentFields.DeliveryConfirmation);
            ApplyProfileValue(upsProfile.ReferenceNumber, upsShipment, UpsShipmentFields.ReferenceNumber);
            ApplyProfileValue(upsProfile.ReferenceNumber2, upsShipment, UpsShipmentFields.ReferenceNumber2);

            ApplyProfileValue(upsProfile.Service, upsShipment, UpsShipmentFields.Service);
            ApplyProfileValue(upsProfile.SaturdayDelivery, upsShipment, UpsShipmentFields.SaturdayDelivery);

            ApplyProfileValue(upsProfile.PayorType, upsShipment, UpsShipmentFields.PayorType);
            ApplyProfileValue(upsProfile.PayorAccount, upsShipment, UpsShipmentFields.PayorAccount);
            ApplyProfileValue(upsProfile.PayorPostalCode, upsShipment, UpsShipmentFields.PayorPostalCode);
            ApplyProfileValue(upsProfile.PayorCountryCode, upsShipment, UpsShipmentFields.PayorCountryCode);

            ApplyProfileEmailNotify(upsProfile, upsShipment);

            ApplyProfileValue(upsProfile.ReturnService, upsShipment, UpsShipmentFields.ReturnService);
            ApplyProfileValue(upsProfile.ReturnContents, upsShipment, UpsShipmentFields.ReturnContents);
            ApplyProfileValue(upsProfile.ReturnUndeliverableEmail, upsShipment, UpsShipmentFields.ReturnUndeliverableEmail);

            ApplyProfileValue(upsProfile.Subclassification, upsShipment, UpsShipmentFields.Subclassification);
            ApplyProfileValue(upsProfile.Endorsement, upsShipment, UpsShipmentFields.Endorsement);

            ApplyProfileValue(upsProfile.PaperlessAdditionalDocumentation, upsShipment, UpsShipmentFields.PaperlessAdditionalDocumentation);
            ApplyProfileValue(upsProfile.CommercialPaperlessInvoice, upsShipment, UpsShipmentFields.CommercialPaperlessInvoice);
            ApplyProfileValue(upsProfile.ShipperRelease, upsShipment, UpsShipmentFields.ShipperRelease);
            ApplyProfileValue(upsProfile.CarbonNeutral, upsShipment, UpsShipmentFields.CarbonNeutral);

            ApplyProfileValue(upsProfile.UspsPackageID, upsShipment, UpsShipmentFields.UspsPackageID);
            ApplyProfileValue(upsProfile.CostCenter, upsShipment, UpsShipmentFields.CostCenter);
            ApplyProfileValue(upsProfile.IrregularIndicator, upsShipment, UpsShipmentFields.IrregularIndicator);
            ApplyProfileValue(upsProfile.Cn22Number, upsShipment, UpsShipmentFields.Cn22Number);

            ApplyProfileShipmentCharge(upsProfile, upsShipment);

            ApplyProfileValue(upsProfile.CustomsDescription, upsShipment, UpsShipmentFields.CustomsDescription);
        }

        /// <summary>
        /// Apply the Shipment email notify properties from the profile to the shipment
        /// </summary>
        private void ApplyProfileEmailNotify(IUpsProfileEntity upsProfile, UpsShipmentEntity upsShipment)
        {
            ApplyProfileValue(upsProfile.EmailNotifySender, upsShipment, UpsShipmentFields.EmailNotifySender);
            ApplyProfileValue(upsProfile.EmailNotifyRecipient, upsShipment, UpsShipmentFields.EmailNotifyRecipient);
            ApplyProfileValue(upsProfile.EmailNotifyOther, upsShipment, UpsShipmentFields.EmailNotifyOther);
            ApplyProfileValue(upsProfile.EmailNotifyOtherAddress, upsShipment, UpsShipmentFields.EmailNotifyOtherAddress);
            ApplyProfileValue(upsProfile.EmailNotifyFrom, upsShipment, UpsShipmentFields.EmailNotifyFrom);
            ApplyProfileValue(upsProfile.EmailNotifySubject, upsShipment, UpsShipmentFields.EmailNotifySubject);
            ApplyProfileValue(upsProfile.EmailNotifyMessage, upsShipment, UpsShipmentFields.EmailNotifyMessage);
        }

        /// <summary>
        /// Apply the ShipmentCharge properties from the profile to the shipment
        /// </summary>
        private void ApplyProfileShipmentCharge(IUpsProfileEntity upsProfile, UpsShipmentEntity upsShipment)
        {
            ApplyProfileValue(upsProfile.ShipmentChargeType, upsShipment, UpsShipmentFields.ShipmentChargeType);
            ApplyProfileValue(upsProfile.ShipmentChargePostalCode, upsShipment, UpsShipmentFields.ShipmentChargePostalCode);
            ApplyProfileValue(upsProfile.ShipmentChargeCountryCode, upsShipment, UpsShipmentFields.ShipmentChargeCountryCode);
            ApplyProfileValue(upsProfile.ShipmentChargeAccount, upsShipment, UpsShipmentFields.ShipmentChargeAccount);
        }

        /// <summary>
        /// Apply the profiles packagges to the shipment
        /// </summary>
        private bool ApplyProfilesPackages(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            UpsShipmentEntity ups = shipment.Ups;
            int profilePackageCount = profile.Packages.Count();
            bool changedPackageWeights = false;

            // Apply all package profiles
            for (int i = 0; i < profilePackageCount; i++)
            {
                // Get the profile to apply
                IUpsProfilePackageEntity packageProfile = profile.Packages.ElementAt(i) as IUpsProfilePackageEntity;

                UpsPackageEntity package;

                // Get the existing, or create a new package
                if (ups.Packages.Count > i)
                {
                    package = ups.Packages[i];
                }
                else
                {
                    package = UpsUtility.CreateDefaultPackage();
                    ups.Packages.Add(package);

                    if (ups.Packages.Count == 1)
                    {
                        // Weight of the first package equals the total shipment content weight
                        package.Weight = shipment.ContentWeight;
                        changedPackageWeights = true;

                        package.InsuranceValue = insuranceUtility.GetInsuranceValue(shipment);
                        package.DeclaredValue = 0;
                    }
                }

                ApplyProfileValue(packageProfile.PackagingType, package, UpsPackageFields.PackagingType);

                ApplyProfileValue(packageProfile.Weight, package, UpsPackageFields.Weight);
                changedPackageWeights |= (packageProfile.Weight != null);

                ApplyProfileValue(packageProfile.DimsProfileID, package, UpsPackageFields.DimsProfileID);
                if (packageProfile.DimsProfileID != null)
                {
                    ApplyProfilePackageDims(packageProfile, package);
                }

                ApplyProfileValue(packageProfile.AdditionalHandlingEnabled, package, UpsPackageFields.AdditionalHandlingEnabled);

                ApplyProfilePackageDryIce(packageProfile, package);
                ApplyProfilePackageVerbalConfirmation(packageProfile, package);
            }

            return changedPackageWeights;
        }

        /// <summary>
        /// Apply the package verbal confirmation
        /// </summary>
        private static void ApplyProfilePackageVerbalConfirmation(IUpsProfilePackageEntity packageProfile, UpsPackageEntity package)
        {
            ApplyProfileValue(packageProfile.VerbalConfirmationEnabled, package, UpsPackageFields.VerbalConfirmationEnabled);
            ApplyProfileValue(packageProfile.VerbalConfirmationName, package, UpsPackageFields.VerbalConfirmationName);
            ApplyProfileValue(packageProfile.VerbalConfirmationPhone, package, UpsPackageFields.VerbalConfirmationPhone);
            ApplyProfileValue(packageProfile.VerbalConfirmationPhoneExtension, package, UpsPackageFields.VerbalConfirmationPhoneExtension);
        }

        /// <summary>
        /// Apply the package dry ice
        /// </summary>
        private static void ApplyProfilePackageDryIce(IUpsProfilePackageEntity packageProfile, UpsPackageEntity package)
        {
            ApplyProfileValue(packageProfile.DryIceEnabled, package, UpsPackageFields.DryIceEnabled);
            ApplyProfileValue(packageProfile.DryIceIsForMedicalUse, package, UpsPackageFields.DryIceIsForMedicalUse);
            ApplyProfileValue(packageProfile.DryIceRegulationSet, package, UpsPackageFields.DryIceRegulationSet);
            ApplyProfileValue(packageProfile.DryIceWeight, package, UpsPackageFields.DryIceWeight);
        }

        /// <summary>
        /// Apply the package dims
        /// </summary>
        private static void ApplyProfilePackageDims(IUpsProfilePackageEntity packageProfile, UpsPackageEntity package)
        {
            if (packageProfile.DimsLength.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfile.DimsLength, package, UpsPackageFields.DimsLength);
            }

            if (packageProfile.DimsWidth.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfile.DimsWidth, package, UpsPackageFields.DimsWidth);
            }

            if (packageProfile.DimsHeight.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfile.DimsHeight, package, UpsPackageFields.DimsHeight);
            }

            ApplyProfileValue(packageProfile.DimsWeight, package, UpsPackageFields.DimsWeight);
            ApplyProfileValue(packageProfile.DimsAddWeight, package, UpsPackageFields.DimsAddWeight);
        }

        /// <summary>
        /// Remove any extra packages in the shipment
        /// </summary>
        private bool RemoveExcessPackages(UpsShipmentEntity upsShipment, int profilePackageCount)
        {
            bool changedPackageWeights = false;

            // Remove any packages that are too many for the profile
            if (profilePackageCount > 0)
            {
                // Go through each package that needs removed
                foreach (UpsPackageEntity package in upsShipment.Packages.Skip(profilePackageCount).ToList())
                {
                    if (package.Weight != 0)
                    {
                        changedPackageWeights = true;
                    }

                    // Remove it from the list
                    upsShipment.Packages.Remove(package);

                    // If its saved in the database, we have to delete it
                    if (!package.IsNew)
                    {
                        using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                        {
                            adapter.DeleteEntity(package);
                        }
                    }
                }
            }

            return changedPackageWeights;
        }

        /// <summary>
        /// Get the AccountID to use for the profile
        /// </summary>
        private long? GetAccountID(IUpsProfileEntity upsProfile) =>
            (upsProfile.UpsAccountID == 0 && accountRetriever.AccountsReadOnly.Any()) ?
                accountRetriever.AccountsReadOnly.First().UpsAccountID :
                upsProfile.UpsAccountID;
    }
}
