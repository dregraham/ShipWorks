using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// DhlExpress shipping profile application strategy
    /// </summary>
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        private readonly ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> accountRepository;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager,
            ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> accountRepository,
            ISqlAdapterFactory sqlAdapterFactory) :
            base(shipmentTypeManager)
        {
            this.accountRepository = accountRepository;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            DhlExpressShipmentEntity dhlShipment = shipment.DhlExpress;
            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);

            bool changedPackageWeights = ApplyDhlExpressPackageProfile(dhlShipment, profile);
            int profilePackageCount = profile.Packages.Count();

            // Remove any packages that are too many for the profile
            if (profilePackageCount > 0)
            {
                // Go through each package that needs removed
                foreach (DhlExpressPackageEntity package in dhlShipment.Packages.Skip(profilePackageCount).ToList())
                {
                    if (!package.Weight.IsEquivalentTo(0))
                    {
                        changedPackageWeights = true;
                    }

                    // Remove it from the list
                    dhlShipment.Packages.Remove(package);

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

            base.ApplyProfile(profile, shipment);

            ApplyDhlExpressProfile(dhlShipment, profile);

            if (changedPackageWeights)
            {
                shipmentType.UpdateTotalWeight(shipment);
            }

            shipmentType.UpdateDynamicShipmentData(shipment);
        }

         /// <summary>
        /// Apply the dhl express package profile
        /// </summary>
        /// <returns>bool if the weight of the package has changed</returns>
        private bool ApplyDhlExpressPackageProfile(DhlExpressShipmentEntity dhlShipment, IShippingProfileEntity profile)
        {
            bool changedPackageWeights = false;

            int profilePackageCount = profile.Packages.Count();

            // Apply all package profiles
            for (int i = 0; i < profilePackageCount; i++)
            {
                // Get the profile to apply
                IPackageProfileEntity packageProfile = profile.Packages.ElementAt(i);

                DhlExpressPackageEntity package;

                // Get the existing, or create a new package
                if (dhlShipment.Packages.Count > i)
                {
                    package = dhlShipment.Packages[i];
                }
                else
                {
                    package = DhlExpressShipmentType.CreateDefaultPackage();
                    dhlShipment.Packages.Add(package);
                }

                double originalPackageWeight = package.Weight;
                ApplyProfileValue(packageProfile.Weight, package, DhlExpressPackageFields.Weight);
                changedPackageWeights |= originalPackageWeight != package.Weight;

                ApplyProfileValue(packageProfile.DimsProfileID, package, DhlExpressPackageFields.DimsProfileID);
                if (packageProfile.DimsProfileID != null)
                {

                    if (packageProfile.DimsLength.GetValueOrDefault() > 0)
                    {
                        ApplyProfileValue(packageProfile.DimsLength, package, DhlExpressPackageFields.DimsLength);
                    }

                    if (packageProfile.DimsWidth.GetValueOrDefault() > 0)
                    {
                        ApplyProfileValue(packageProfile.DimsWidth, package, DhlExpressPackageFields.DimsWidth);
                    }

                    if (packageProfile.DimsHeight.GetValueOrDefault() > 0)
                    {
                        ApplyProfileValue(packageProfile.DimsHeight, package, DhlExpressPackageFields.DimsHeight);
                    }

                    ApplyProfileValue(packageProfile.DimsWeight, package, DhlExpressPackageFields.DimsWeight);
                    ApplyProfileValue(packageProfile.DimsAddWeight, package, DhlExpressPackageFields.DimsAddWeight);
                }
            }

            return changedPackageWeights;
        }

        /// <summary>
        /// Apply the DHL Express profile
        /// </summary>
        private void ApplyDhlExpressProfile(DhlExpressShipmentEntity dhlShipment, IShippingProfileEntity profile)
        {
            IDhlExpressProfileEntity source = profile.DhlExpress;

            long? accountID = (source.DhlExpressAccountID == 0 && accountRepository.Accounts.Any())
                ? accountRepository.Accounts.First().DhlExpressAccountID
                : source.DhlExpressAccountID;

            ApplyProfileValue(accountID, dhlShipment, DhlExpressShipmentFields.DhlExpressAccountID);
            ApplyProfileValue(source.Service, dhlShipment, DhlExpressShipmentFields.Service);
            ApplyProfileValue(source.DeliveryDutyPaid, dhlShipment, DhlExpressShipmentFields.DeliveredDutyPaid);
            ApplyProfileValue(source.NonMachinable, dhlShipment, DhlExpressShipmentFields.NonMachinable);
            ApplyProfileValue(source.SaturdayDelivery, dhlShipment, DhlExpressShipmentFields.SaturdayDelivery);
            ApplyProfileValue(source.NonDelivery, dhlShipment, DhlExpressShipmentFields.NonDelivery);
            ApplyProfileValue(source.Contents, dhlShipment, DhlExpressShipmentFields.Contents);
        }
    }
}