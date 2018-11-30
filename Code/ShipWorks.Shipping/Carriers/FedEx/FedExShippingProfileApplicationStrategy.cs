using System.Linq;
using Interapptive.Shared.ComponentRegistration;
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
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.FedEx)]
    public class FedExShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        private readonly ICarrierAccountRetriever<FedExAccountEntity, IFedExAccountEntity> accountRetriever;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager,
            ICarrierAccountRetriever<FedExAccountEntity, IFedExAccountEntity> accountRetriever,
            ISqlAdapterFactory sqlAdapterFactory)
            :base(shipmentTypeManager)
        {
            this.accountRetriever = accountRetriever;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Apply the FexEx profile to the shipment
        /// </summary>
        public override void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            bool changedPackageWeights = ApplyProfilesPackages(profile, shipment);
            changedPackageWeights |= RemoveExcessPackages(shipment.FedEx, profile.Packages.Count());

            base.ApplyProfile(profile, shipment);

            ApplyProfileValue(profile.FedEx.ResidentialDetermination, shipment, ShipmentFields.ResidentialDetermination);
            ApplyProfileValue(profile.ReturnShipment, shipment, ShipmentFields.ReturnShipment);

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
            ApplyProfileValue(GetAccountID(fedExProfile), fedExShipment, FedExShipmentFields.FedExAccountID);
            ApplyProfileValue(fedExProfile.Service, fedExShipment, FedExShipmentFields.Service);
            ApplyProfileValue(fedExProfile.PackagingType, fedExShipment, FedExShipmentFields.PackagingType);
            ApplyProfileValue(fedExProfile.DropoffType, fedExShipment, FedExShipmentFields.DropoffType);
            ApplyProfileValue(fedExProfile.ReturnsClearance, fedExShipment, FedExShipmentFields.ReturnsClearance);
            ApplyProfileValue(fedExProfile.NonStandardContainer, fedExShipment, FedExShipmentFields.NonStandardContainer);
            ApplyProfileValue(fedExProfile.OriginResidentialDetermination, fedExShipment, FedExShipmentFields.OriginResidentialDetermination);

            ApplyProfileValue(fedExProfile.Signature, fedExShipment, FedExShipmentFields.Signature);
            ApplyProfileValue(fedExProfile.ReferenceFIMS, fedExShipment, FedExShipmentFields.ReferenceFIMS);
            ApplyProfileValue(fedExProfile.ReferenceCustomer, fedExShipment, FedExShipmentFields.ReferenceCustomer);
            ApplyProfileValue(fedExProfile.ReferenceInvoice, fedExShipment, FedExShipmentFields.ReferenceInvoice);
            ApplyProfileValue(fedExProfile.ReferencePO, fedExShipment, FedExShipmentFields.ReferencePO);
            ApplyProfileValue(fedExProfile.ReferenceShipmentIntegrity, fedExShipment, FedExShipmentFields.ReferenceShipmentIntegrity);

            ApplyProfilePayor(fedExProfile, fedExShipment);

            ApplyProfileValue(fedExProfile.SaturdayDelivery, fedExShipment, FedExShipmentFields.SaturdayDelivery);

            ApplyProfileEmailNotify(fedExProfile, fedExShipment);
            ApplyProfileSmartPost(fedExProfile, fedExShipment);

            ApplyProfileValue(fedExProfile.ReturnType, fedExShipment, FedExShipmentFields.ReturnType);
            ApplyProfileValue(fedExProfile.RmaNumber, fedExShipment, FedExShipmentFields.RmaNumber);
            ApplyProfileValue(fedExProfile.RmaReason, fedExShipment, FedExShipmentFields.RmaReason);
            ApplyProfileValue(fedExProfile.ReturnSaturdayPickup, fedExShipment, FedExShipmentFields.ReturnSaturdayPickup);
            ApplyProfileValue(fedExProfile.ThirdPartyConsignee, fedExShipment, FedExShipmentFields.ThirdPartyConsignee);
        }

        /// <summary>
        /// Apply the profiles payor properties to the FexEx shipment
        /// </summary>
        private void ApplyProfilePayor(IFedExProfileEntity fedExProfile, FedExShipmentEntity fedExShipment)
        {
            ApplyProfileValue(fedExProfile.PayorTransportType, fedExShipment, FedExShipmentFields.PayorTransportType);
            ApplyProfileValue(fedExProfile.PayorTransportAccount, fedExShipment, FedExShipmentFields.PayorTransportAccount);
            ApplyProfileValue(fedExProfile.PayorDutiesType, fedExShipment, FedExShipmentFields.PayorDutiesType);
            ApplyProfileValue(fedExProfile.PayorDutiesAccount, fedExShipment, FedExShipmentFields.PayorDutiesAccount);
        }

        /// <summary>
        /// Apply the profiles email notify properties to the FexEx shipment
        /// </summary>
        private void ApplyProfileEmailNotify(IFedExProfileEntity fedExProfile, FedExShipmentEntity fedExShipment)
        {
            ApplyProfileValue(fedExProfile.EmailNotifySender, fedExShipment, FedExShipmentFields.EmailNotifySender);
            ApplyProfileValue(fedExProfile.EmailNotifyRecipient, fedExShipment, FedExShipmentFields.EmailNotifyRecipient);
            ApplyProfileValue(fedExProfile.EmailNotifyBroker, fedExShipment, FedExShipmentFields.EmailNotifyBroker);
            ApplyProfileValue(fedExProfile.EmailNotifyOther, fedExShipment, FedExShipmentFields.EmailNotifyOther);
            ApplyProfileValue(fedExProfile.EmailNotifyOtherAddress, fedExShipment, FedExShipmentFields.EmailNotifyOtherAddress);
            ApplyProfileValue(fedExProfile.EmailNotifyMessage, fedExShipment, FedExShipmentFields.EmailNotifyMessage);
        }

        /// <summary>
        /// Apply the profiles smart post properties to the FexEx shipment
        /// </summary>
        private void ApplyProfileSmartPost(IFedExProfileEntity fedExProfile, FedExShipmentEntity fedExShipment)
        {
            ApplyProfileValue(fedExProfile.SmartPostIndicia, fedExShipment, FedExShipmentFields.SmartPostIndicia);
            ApplyProfileValue(fedExProfile.SmartPostEndorsement, fedExShipment, FedExShipmentFields.SmartPostEndorsement);
            ApplyProfileValue(fedExProfile.SmartPostConfirmation, fedExShipment, FedExShipmentFields.SmartPostConfirmation);
            ApplyProfileValue(fedExProfile.SmartPostCustomerManifest, fedExShipment, FedExShipmentFields.SmartPostCustomerManifest);
            ApplyProfileValue(fedExProfile.SmartPostHubID, fedExShipment, FedExShipmentFields.SmartPostHubID);
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
            bool changedPackageWeights = false;
            int profilePackageCount = profile.Packages.Count();

            // Apply all package profiles
            for (int i = 0; i < profilePackageCount; i++)
            {
                FedExPackageEntity package;

                // Get the existing, or create a new package
                if (shipment.FedEx.Packages.Count > i)
                {
                    package = shipment.FedEx.Packages[i];
                }
                else
                {
                    package = FedExUtility.CreateDefaultPackage();
                    shipment.FedEx.Packages.Add(package);
                }

                IFedExProfilePackageEntity fedexPackageProfile = profile.Packages.ElementAt(i) as IFedExProfilePackageEntity;
                ApplyProfileValue(fedexPackageProfile.Weight, package, FedExPackageFields.Weight);
                changedPackageWeights |= (fedexPackageProfile.Weight != null);
                ApplyProfilePackagesDims(fedexPackageProfile, package);

                if (fedexPackageProfile.DryIceWeight > 0)
                {
                    ApplyProfileValue(fedexPackageProfile.DryIceWeight, package, FedExPackageFields.DryIceWeight);
                }

                ApplyProfileValue(fedexPackageProfile.ContainsAlcohol, package, FedExPackageFields.ContainsAlcohol);

                if (fedexPackageProfile.PriorityAlert.HasValue && fedexPackageProfile.PriorityAlert.Value)
                {
                    ApplyProfileValue(fedexPackageProfile.PriorityAlertDetailContent, package, FedExPackageFields.PriorityAlertDetailContent);
                    ApplyProfileValue(fedexPackageProfile.PriorityAlertEnhancementType, package, FedExPackageFields.PriorityAlertEnhancementType);
                }

                ApplyProfilePackagesDangerousGoods(fedexPackageProfile, package);
                ApplyProfilePackagesSignatory(fedexPackageProfile, package);
                ApplyProfilePackagesHazardousMaterial(fedexPackageProfile, package);
                ApplyProfilePackagesBattery(fedexPackageProfile, package);
            }

            return changedPackageWeights;
        }

        /// <summary>
        /// Apply the package profiles battery to the fedex package
        /// </summary>
        private static void ApplyProfilePackagesSignatory(IFedExProfilePackageEntity fedexPackageProfile, FedExPackageEntity package)
        {
            ApplyProfileValue(fedexPackageProfile.SignatoryContactName, package, FedExPackageFields.SignatoryContactName);
            ApplyProfileValue(fedexPackageProfile.SignatoryTitle, package, FedExPackageFields.SignatoryTitle);
            ApplyProfileValue(fedexPackageProfile.SignatoryPlace, package, FedExPackageFields.SignatoryPlace);
        }

        /// <summary>
        /// Apply the package profiles battery to the fedex package
        /// </summary>
        private static void ApplyProfilePackagesBattery(IFedExProfilePackageEntity fedexPackageProfile, FedExPackageEntity package)
        {
            ApplyProfileValue(fedexPackageProfile.BatteryMaterial, package, FedExPackageFields.BatteryMaterial);
            ApplyProfileValue(fedexPackageProfile.BatteryPacking, package, FedExPackageFields.BatteryPacking);
            ApplyProfileValue(fedexPackageProfile.BatteryRegulatorySubtype, package, FedExPackageFields.BatteryRegulatorySubtype);
        }

        /// <summary>
        /// Apply the package profiles dims to the fedex package
        /// </summary>
        private static void ApplyProfilePackagesDims(IFedExProfilePackageEntity fedexPackageProfile, FedExPackageEntity package)
        {
            ApplyProfileValue(fedexPackageProfile.DimsProfileID, package, FedExPackageFields.DimsProfileID);
            if (fedexPackageProfile.DimsProfileID != null)
            {
                if (fedexPackageProfile.DimsLength.GetValueOrDefault() > 0)
                {
                    ApplyProfileValue(fedexPackageProfile.DimsLength, package, FedExPackageFields.DimsLength);
                }

                if (fedexPackageProfile.DimsWidth.GetValueOrDefault() > 0)
                {
                    ApplyProfileValue(fedexPackageProfile.DimsWidth, package, FedExPackageFields.DimsWidth);
                }

                if (fedexPackageProfile.DimsHeight.GetValueOrDefault() > 0)
                {
                    ApplyProfileValue(fedexPackageProfile.DimsHeight, package, FedExPackageFields.DimsHeight);
                }

                ApplyProfileValue(fedexPackageProfile.DimsWeight, package, FedExPackageFields.DimsWeight);
                ApplyProfileValue(fedexPackageProfile.DimsAddWeight, package, FedExPackageFields.DimsAddWeight);
            }
        }

        /// <summary>
        /// Apply the package profiles haz mat to the fedex package
        /// </summary>
        private static void ApplyProfilePackagesHazardousMaterial(IFedExProfilePackageEntity fedexPackageProfile, FedExPackageEntity package)
        {
            ApplyProfileValue(fedexPackageProfile.HazardousMaterialNumber, package, FedExPackageFields.HazardousMaterialNumber);
            ApplyProfileValue(fedexPackageProfile.HazardousMaterialClass, package, FedExPackageFields.HazardousMaterialClass);
            ApplyProfileValue(fedexPackageProfile.HazardousMaterialProperName, package, FedExPackageFields.HazardousMaterialProperName);
            ApplyProfileValue(fedexPackageProfile.HazardousMaterialPackingGroup, package, FedExPackageFields.HazardousMaterialPackingGroup);
            ApplyProfileValue(fedexPackageProfile.HazardousMaterialQuantityValue, package, FedExPackageFields.HazardousMaterialQuantityValue);
            ApplyProfileValue(fedexPackageProfile.HazardousMaterialQuanityUnits, package, FedExPackageFields.HazardousMaterialQuanityUnits);
            ApplyProfileValue(fedexPackageProfile.PackingDetailsCargoAircraftOnly, package, FedExPackageFields.PackingDetailsCargoAircraftOnly);
            ApplyProfileValue(fedexPackageProfile.PackingDetailsPackingInstructions, package, FedExPackageFields.PackingDetailsPackingInstructions);
        }

        /// <summary>
        /// Apply the package profiles dangerous goods to the fedex package
        /// </summary>
        private static void ApplyProfilePackagesDangerousGoods(IFedExProfilePackageEntity fedexPackageProfile, FedExPackageEntity package)
        {
            ApplyProfileValue(fedexPackageProfile.DangerousGoodsEnabled, package, FedExPackageFields.DangerousGoodsEnabled);
            ApplyProfileValue(fedexPackageProfile.DangerousGoodsType, package, FedExPackageFields.DangerousGoodsType);
            ApplyProfileValue(fedexPackageProfile.DangerousGoodsAccessibilityType, package, FedExPackageFields.DangerousGoodsAccessibilityType);
            ApplyProfileValue(fedexPackageProfile.DangerousGoodsCargoAircraftOnly, package, FedExPackageFields.DangerousGoodsCargoAircraftOnly);
            ApplyProfileValue(fedexPackageProfile.DangerousGoodsEmergencyContactPhone, package, FedExPackageFields.DangerousGoodsEmergencyContactPhone);
            ApplyProfileValue(fedexPackageProfile.DangerousGoodsOfferor, package, FedExPackageFields.DangerousGoodsOfferor);
            ApplyProfileValue(fedexPackageProfile.DangerousGoodsPackagingCount, package, FedExPackageFields.DangerousGoodsPackagingCount);
            ApplyProfileValue(fedexPackageProfile.ContainerType, package, FedExPackageFields.ContainerType);
            ApplyProfileValue(fedexPackageProfile.NumberOfContainers, package, FedExPackageFields.NumberOfContainers);
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
