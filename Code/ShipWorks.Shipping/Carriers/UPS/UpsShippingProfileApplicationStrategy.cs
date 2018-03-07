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
    public class UpsShippingProfileApplicationStrategy : IShippingProfileApplicationStrategy
    {
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IShippingProfileApplicationStrategy baseShippingProfileApplicationStrategy;
        private readonly ICarrierAccountRetriever<UpsAccountEntity, IUpsAccountEntity> accountRetriever;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IInsuranceUtility insuranceUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipmentTypeManager"></param>
        public UpsShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager, 
            IShippingProfileApplicationStrategy baseShippingProfileApplicationStrategy, 
            ICarrierAccountRetriever<UpsAccountEntity, IUpsAccountEntity> accountRetriever, 
            ISqlAdapterFactory sqlAdapterFactory,
            IInsuranceUtility insuranceUtility)
        {
            this.shipmentTypeManager = shipmentTypeManager;
            this.baseShippingProfileApplicationStrategy = baseShippingProfileApplicationStrategy;
            this.accountRetriever = accountRetriever;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.insuranceUtility = insuranceUtility;
        }
        
        /// <summary>
        /// Apply the profile to the shipment
        /// </summary>
        public void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            bool changedPackageWeights = ApplyProfilesPackages(profile, shipment);
            changedPackageWeights |= RemoveExcessPackages(shipment.Ups, profile.Packages.Count());
            
            baseShippingProfileApplicationStrategy.ApplyProfile(profile, shipment);
            ShippingProfileUtility.ApplyProfileValue(profile.Ups.ResidentialDetermination, shipment, ShipmentFields.ResidentialDetermination);

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
            ShippingProfileUtility.ApplyProfileValue(GetAccountID(upsProfile), upsShipment, UpsShipmentFields.UpsAccountID);
            
            ShippingProfileUtility.ApplyProfileValue(upsProfile.DeliveryConfirmation, upsShipment, UpsShipmentFields.DeliveryConfirmation);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.ReferenceNumber, upsShipment, UpsShipmentFields.ReferenceNumber);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.ReferenceNumber2, upsShipment, UpsShipmentFields.ReferenceNumber2);

            ShippingProfileUtility.ApplyProfileValue(upsProfile.Service, upsShipment, UpsShipmentFields.Service);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.SaturdayDelivery, upsShipment, UpsShipmentFields.SaturdayDelivery);

            ShippingProfileUtility.ApplyProfileValue(upsProfile.PayorType, upsShipment, UpsShipmentFields.PayorType);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.PayorAccount, upsShipment, UpsShipmentFields.PayorAccount);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.PayorPostalCode, upsShipment, UpsShipmentFields.PayorPostalCode);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.PayorCountryCode, upsShipment, UpsShipmentFields.PayorCountryCode);

            ShippingProfileUtility.ApplyProfileValue(upsProfile.EmailNotifySender, upsShipment, UpsShipmentFields.EmailNotifySender);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.EmailNotifyRecipient, upsShipment, UpsShipmentFields.EmailNotifyRecipient);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.EmailNotifyOther, upsShipment, UpsShipmentFields.EmailNotifyOther);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.EmailNotifyOtherAddress, upsShipment, UpsShipmentFields.EmailNotifyOtherAddress);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.EmailNotifyFrom, upsShipment, UpsShipmentFields.EmailNotifyFrom);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.EmailNotifySubject, upsShipment, UpsShipmentFields.EmailNotifySubject);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.EmailNotifyMessage, upsShipment, UpsShipmentFields.EmailNotifyMessage);

            ShippingProfileUtility.ApplyProfileValue(upsProfile.ReturnService, upsShipment, UpsShipmentFields.ReturnService);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.ReturnContents, upsShipment, UpsShipmentFields.ReturnContents);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.ReturnUndeliverableEmail, upsShipment, UpsShipmentFields.ReturnUndeliverableEmail);

            ShippingProfileUtility.ApplyProfileValue(upsProfile.Subclassification, upsShipment, UpsShipmentFields.Subclassification);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.Endorsement, upsShipment, UpsShipmentFields.Endorsement);

            ShippingProfileUtility.ApplyProfileValue(upsProfile.PaperlessAdditionalDocumentation, upsShipment, UpsShipmentFields.PaperlessAdditionalDocumentation);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.CommercialPaperlessInvoice, upsShipment, UpsShipmentFields.CommercialPaperlessInvoice);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.ShipperRelease, upsShipment, UpsShipmentFields.ShipperRelease);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.CarbonNeutral, upsShipment, UpsShipmentFields.CarbonNeutral);

            ShippingProfileUtility.ApplyProfileValue(upsProfile.UspsPackageID, upsShipment, UpsShipmentFields.UspsPackageID);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.CostCenter, upsShipment, UpsShipmentFields.CostCenter);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.IrregularIndicator, upsShipment, UpsShipmentFields.IrregularIndicator);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.Cn22Number, upsShipment, UpsShipmentFields.Cn22Number);

            ShippingProfileUtility.ApplyProfileValue(upsProfile.ShipmentChargeType, upsShipment, UpsShipmentFields.ShipmentChargeType);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.ShipmentChargePostalCode, upsShipment, UpsShipmentFields.ShipmentChargePostalCode);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.ShipmentChargeCountryCode, upsShipment, UpsShipmentFields.ShipmentChargeCountryCode);
            ShippingProfileUtility.ApplyProfileValue(upsProfile.ShipmentChargeAccount, upsShipment, UpsShipmentFields.ShipmentChargeAccount);

            ShippingProfileUtility.ApplyProfileValue(upsProfile.CustomsDescription, upsShipment, UpsShipmentFields.CustomsDescription);
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

                ShippingProfileUtility.ApplyProfileValue(packageProfile.PackagingType, package, UpsPackageFields.PackagingType);

                ShippingProfileUtility.ApplyProfileValue(packageProfile.Weight, package, UpsPackageFields.Weight);
                changedPackageWeights |= (packageProfile.Weight != null);

                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsProfileID, package, UpsPackageFields.DimsProfileID);
                if (packageProfile.DimsProfileID != null)
                {
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsLength, package, UpsPackageFields.DimsLength);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWidth, package, UpsPackageFields.DimsWidth);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsHeight, package, UpsPackageFields.DimsHeight);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWeight, package, UpsPackageFields.DimsWeight);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsAddWeight, package, UpsPackageFields.DimsAddWeight);
                }

                ShippingProfileUtility.ApplyProfileValue(packageProfile.AdditionalHandlingEnabled, package, UpsPackageFields.AdditionalHandlingEnabled);

                ShippingProfileUtility.ApplyProfileValue(packageProfile.DryIceEnabled, package, UpsPackageFields.DryIceEnabled);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DryIceIsForMedicalUse, package, UpsPackageFields.DryIceIsForMedicalUse);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DryIceRegulationSet, package, UpsPackageFields.DryIceRegulationSet);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DryIceWeight, package, UpsPackageFields.DryIceWeight);

                ShippingProfileUtility.ApplyProfileValue(packageProfile.VerbalConfirmationEnabled, package, UpsPackageFields.VerbalConfirmationEnabled);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.VerbalConfirmationName, package, UpsPackageFields.VerbalConfirmationName);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.VerbalConfirmationPhone, package, UpsPackageFields.VerbalConfirmationPhone);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.VerbalConfirmationPhoneExtension, package, UpsPackageFields.VerbalConfirmationPhoneExtension);
            }

            return changedPackageWeights;
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
