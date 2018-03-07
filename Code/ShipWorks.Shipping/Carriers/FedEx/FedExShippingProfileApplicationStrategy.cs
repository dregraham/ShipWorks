using System;
using System.Linq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// FedEx shipping profile application strategy
    /// </summary>
    public class FedExShippingProfileApplicationStrategy : IShippingProfileApplicationStrategy
    {
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IShippingProfileApplicationStrategy baseShippingProfileApplicationStrategy;
        private readonly ICarrierAccountRetriever<FedExAccountEntity, IFedExAccountEntity> accountRetriever;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager,
            IShippingProfileApplicationStrategy baseShippingProfileApplicationStrategy,
            ICarrierAccountRetriever<FedExAccountEntity, IFedExAccountEntity> accountRetriever,
            ISqlAdapterFactory sqlAdapterFactory)
        {
            this.shipmentTypeManager = shipmentTypeManager;
            this.baseShippingProfileApplicationStrategy = baseShippingProfileApplicationStrategy;
            this.accountRetriever = accountRetriever;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Apply the FexEx profile to the shipment
        /// </summary>
        public void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            bool changedPackageWeights = ApplyProfilesPackages(profile, shipment);
            changedPackageWeights |= RemoveExcessPackages(shipment.FedEx, profile.Packages.Count());

            baseShippingProfileApplicationStrategy.ApplyProfile(profile, shipment);
            
            ShippingProfileUtility.ApplyProfileValue(profile.FedEx.ResidentialDetermination, shipment, ShipmentFields.ResidentialDetermination);
            ShippingProfileUtility.ApplyProfileValue(profile.ReturnShipment, shipment, ShipmentFields.ReturnShipment);

            ApplyProfile(profile.FedEx, shipment.FedEx);

            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);

            if (changedPackageWeights)
            {
                shipmentType.UpdateTotalWeight(shipment);
            }

            shipmentType.UpdateDynamicShipmentData(shipment);
        }

        /// <summary>
        /// Apply the FedExProfile to the FedExShipment
        /// </summary>
        private void ApplyProfile(IFedExProfileEntity fedExProfile, FedExShipmentEntity fedExShipment)
        {
            ShippingProfileUtility.ApplyProfileValue(GetAccountID(fedExProfile), fedExShipment, FedExShipmentFields.FedExAccountID);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.Service, fedExShipment, FedExShipmentFields.Service);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.PackagingType, fedExShipment, FedExShipmentFields.PackagingType);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.DropoffType, fedExShipment, FedExShipmentFields.DropoffType);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.ReturnsClearance, fedExShipment, FedExShipmentFields.ReturnsClearance);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.NonStandardContainer, fedExShipment, FedExShipmentFields.NonStandardContainer);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.OriginResidentialDetermination, fedExShipment, FedExShipmentFields.OriginResidentialDetermination);

            ShippingProfileUtility.ApplyProfileValue(fedExProfile.Signature, fedExShipment, FedExShipmentFields.Signature);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.ReferenceFIMS, fedExShipment, FedExShipmentFields.ReferenceFIMS);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.ReferenceCustomer, fedExShipment, FedExShipmentFields.ReferenceCustomer);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.ReferenceInvoice, fedExShipment, FedExShipmentFields.ReferenceInvoice);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.ReferencePO, fedExShipment, FedExShipmentFields.ReferencePO);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.ReferenceShipmentIntegrity, fedExShipment, FedExShipmentFields.ReferenceShipmentIntegrity);

            ShippingProfileUtility.ApplyProfileValue(fedExProfile.PayorTransportType, fedExShipment, FedExShipmentFields.PayorTransportType);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.PayorTransportAccount, fedExShipment, FedExShipmentFields.PayorTransportAccount);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.PayorDutiesType, fedExShipment, FedExShipmentFields.PayorDutiesType);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.PayorDutiesAccount, fedExShipment, FedExShipmentFields.PayorDutiesAccount);

            ShippingProfileUtility.ApplyProfileValue(fedExProfile.SaturdayDelivery, fedExShipment, FedExShipmentFields.SaturdayDelivery);

            ShippingProfileUtility.ApplyProfileValue(fedExProfile.EmailNotifySender, fedExShipment, FedExShipmentFields.EmailNotifySender);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.EmailNotifyRecipient, fedExShipment, FedExShipmentFields.EmailNotifyRecipient);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.EmailNotifyBroker, fedExShipment, FedExShipmentFields.EmailNotifyBroker);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.EmailNotifyOther, fedExShipment, FedExShipmentFields.EmailNotifyOther);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.EmailNotifyOtherAddress, fedExShipment, FedExShipmentFields.EmailNotifyOtherAddress);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.EmailNotifyMessage, fedExShipment, FedExShipmentFields.EmailNotifyMessage);

            ShippingProfileUtility.ApplyProfileValue(fedExProfile.SmartPostIndicia, fedExShipment, FedExShipmentFields.SmartPostIndicia);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.SmartPostEndorsement, fedExShipment, FedExShipmentFields.SmartPostEndorsement);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.SmartPostConfirmation, fedExShipment, FedExShipmentFields.SmartPostConfirmation);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.SmartPostCustomerManifest, fedExShipment, FedExShipmentFields.SmartPostCustomerManifest);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.SmartPostHubID, fedExShipment, FedExShipmentFields.SmartPostHubID);

            ShippingProfileUtility.ApplyProfileValue(fedExProfile.ReturnType, fedExShipment, FedExShipmentFields.ReturnType);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.RmaNumber, fedExShipment, FedExShipmentFields.RmaNumber);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.RmaReason, fedExShipment, FedExShipmentFields.RmaReason);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.ReturnSaturdayPickup, fedExShipment, FedExShipmentFields.ReturnSaturdayPickup);
            ShippingProfileUtility.ApplyProfileValue(fedExProfile.ThirdPartyConsignee, fedExShipment, FedExShipmentFields.ThirdPartyConsignee);
        }

        /// <summary>
        /// Remove any extra packages on the shipment
        /// </summary>
        private bool RemoveExcessPackages(FedExShipmentEntity shipment, int profilePackageCount)
        {
            bool changedPackageWeights = false;

            // Remove any packages that are too many for the profile
            if (profilePackageCount > 0)
            {
                // Go through each package that needs removed
                foreach (FedExPackageEntity package in shipment.Packages.Skip(profilePackageCount).ToList())
                {
                    if (package.Weight != 0)
                    {
                        changedPackageWeights = true;
                    }

                    // Remove it from the list
                    shipment.Packages.Remove(package);

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
        /// Apply the profiles packagges to the shipment
        /// </summary>
        private bool ApplyProfilesPackages(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            FedExShipmentEntity fedex = shipment.FedEx;

            bool changedPackageWeights = false;
            int profilePackageCount = profile.Packages.Count();

            // Apply all package profiles
            for (int i = 0; i < profilePackageCount; i++)
            {
                // Get the profile to apply
                IFedExProfilePackageEntity fedexPackageProfile = profile.Packages.ElementAt(i) as IFedExProfilePackageEntity;
                FedExPackageEntity package;

                // Get the existing, or create a new package
                if (fedex.Packages.Count > i)
                {
                    package = fedex.Packages[i];
                }
                else
                {
                    package = FedExUtility.CreateDefaultPackage();
                    fedex.Packages.Add(package);
                }

                IPackageProfileEntity packageProfile = fedexPackageProfile;

                ShippingProfileUtility.ApplyProfileValue(packageProfile.Weight, package, FedExPackageFields.Weight);
                changedPackageWeights |= (packageProfile.Weight != null);

                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsProfileID, package, FedExPackageFields.DimsProfileID);
                if (packageProfile.DimsProfileID != null)
                {
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsLength, package, FedExPackageFields.DimsLength);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWidth, package, FedExPackageFields.DimsWidth);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsHeight, package, FedExPackageFields.DimsHeight);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWeight, package, FedExPackageFields.DimsWeight);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsAddWeight, package, FedExPackageFields.DimsAddWeight);
                }

                if (fedexPackageProfile.DryIceWeight > 0)
                {
                    ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.DryIceWeight, package, FedExPackageFields.DryIceWeight);
                }

                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.ContainsAlcohol, package, FedExPackageFields.ContainsAlcohol);

                if (fedexPackageProfile.PriorityAlert.HasValue && fedexPackageProfile.PriorityAlert.Value)
                {
                    ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.PriorityAlertDetailContent, package, FedExPackageFields.PriorityAlertDetailContent);
                    ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.PriorityAlertEnhancementType, package, FedExPackageFields.PriorityAlertEnhancementType);
                }

                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.DangerousGoodsEnabled, package, FedExPackageFields.DangerousGoodsEnabled);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.DangerousGoodsType, package, FedExPackageFields.DangerousGoodsType);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.DangerousGoodsAccessibilityType, package, FedExPackageFields.DangerousGoodsAccessibilityType);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.DangerousGoodsCargoAircraftOnly, package, FedExPackageFields.DangerousGoodsCargoAircraftOnly);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.DangerousGoodsEmergencyContactPhone, package, FedExPackageFields.DangerousGoodsEmergencyContactPhone);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.DangerousGoodsOfferor, package, FedExPackageFields.DangerousGoodsOfferor);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.DangerousGoodsPackagingCount, package, FedExPackageFields.DangerousGoodsPackagingCount);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.ContainerType, package, FedExPackageFields.ContainerType);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.NumberOfContainers, package, FedExPackageFields.NumberOfContainers);

                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.SignatoryContactName, package, FedExPackageFields.SignatoryContactName);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.SignatoryTitle, package, FedExPackageFields.SignatoryTitle);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.SignatoryPlace, package, FedExPackageFields.SignatoryPlace);

                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.HazardousMaterialNumber, package, FedExPackageFields.HazardousMaterialNumber);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.HazardousMaterialClass, package, FedExPackageFields.HazardousMaterialClass);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.HazardousMaterialProperName, package, FedExPackageFields.HazardousMaterialProperName);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.HazardousMaterialPackingGroup, package, FedExPackageFields.HazardousMaterialPackingGroup);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.HazardousMaterialQuantityValue, package, FedExPackageFields.HazardousMaterialQuantityValue);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.HazardousMaterialQuanityUnits, package, FedExPackageFields.HazardousMaterialQuanityUnits);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.PackingDetailsCargoAircraftOnly, package, FedExPackageFields.PackingDetailsCargoAircraftOnly);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.PackingDetailsPackingInstructions, package, FedExPackageFields.PackingDetailsPackingInstructions);

                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.BatteryMaterial, package, FedExPackageFields.BatteryMaterial);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.BatteryPacking, package, FedExPackageFields.BatteryPacking);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.BatteryRegulatorySubtype, package, FedExPackageFields.BatteryRegulatorySubtype);
            }

            return changedPackageWeights;
        }

        /// <summary>
        /// Get the AccountID to use for the profile
        /// </summary>
        private long? GetAccountID(IFedExProfileEntity fedexProfile) =>
            (fedexProfile.FedExAccountID == 0 && accountRetriever.AccountsReadOnly.Any()) ?
                accountRetriever.AccountsReadOnly.First().FedExAccountID :
                fedexProfile.FedExAccountID;
    }
}
