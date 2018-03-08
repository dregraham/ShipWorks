using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// iParcel shipping profile application strategy
    /// </summary>
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.iParcel)]
    public class iParcelShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        private readonly ICarrierAccountRetriever<IParcelAccountEntity, IIParcelAccountEntity> accountRepo;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private bool shipmentTotalWeightChanged;
        private readonly ShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public iParcelShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager,
                                                         ICarrierAccountRetriever<IParcelAccountEntity, IIParcelAccountEntity> accountRepo,
                                                         ISqlAdapterFactory sqlAdapterFactory) :
            base(shipmentTypeManager)
        {
            shipmentType = shipmentTypeManager.Get(ShipmentTypeCode.iParcel);
            this.accountRepo = accountRepo;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Apply the specified profile to the given shipment
        /// </summary>
        public override void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            IParcelShipmentEntity iParcel = shipment.IParcel;
            IIParcelProfileEntity source = profile.IParcel;

            shipmentTotalWeightChanged = false;
            int profilePackageCount = profile.Packages.Count();

            ApplyPackageProfiles(profile, profilePackageCount, iParcel);
            RemoveExtraPackages(profilePackageCount, iParcel);
            ApplyShipmentProfile(profile, shipment, iParcel, source);

            if (shipmentTotalWeightChanged)
            {
                shipmentType.UpdateTotalWeight(shipment);
            }

            shipmentType.UpdateDynamicShipmentData(shipment);        
        }
        
        /// <summary>
        /// Apply the profile to the package fields
        /// </summary>
        private void ApplyPackageProfiles(IShippingProfileEntity profile, int profilePackageCount, IParcelShipmentEntity iParcel)
        {
            // Apply all package profiles
            for (int i = 0; i < profilePackageCount; i++)
            {
                // Get the profile to apply
                IPackageProfileEntity packageProfile = profile.Packages.ElementAt(i);

                IParcelPackageEntity package;

                // Get the existing, or create a new package
                if (iParcel.Packages.Count > i)
                {
                    package = iParcel.Packages[i];
                }
                else
                {
                    package = iParcelShipmentType.CreateDefaultPackage();
                    iParcel.Packages.Add(package);
                }

                ApplyProfileValue(packageProfile.Weight, package, IParcelPackageFields.Weight);
                shipmentTotalWeightChanged |= packageProfile.Weight != null;

                ApplyProfileValue(packageProfile.DimsProfileID, package, IParcelPackageFields.DimsProfileID);
                if (packageProfile.DimsProfileID != null)
                {
                    ApplyProfileValue(packageProfile.DimsLength, package, IParcelPackageFields.DimsLength);
                    ApplyProfileValue(packageProfile.DimsWidth, package, IParcelPackageFields.DimsWidth);
                    ApplyProfileValue(packageProfile.DimsHeight, package, IParcelPackageFields.DimsHeight);
                    ApplyProfileValue(packageProfile.DimsWeight, package, IParcelPackageFields.DimsWeight);
                    ApplyProfileValue(packageProfile.DimsAddWeight, package, IParcelPackageFields.DimsAddWeight);
                }
            }
        }

        /// <summary>
        /// Apply the profile to the shipment fields
        /// </summary>
        private void ApplyShipmentProfile(IShippingProfileEntity profile, ShipmentEntity shipment,
                                          IParcelShipmentEntity iParcel, IIParcelProfileEntity source)
        {
            // Apply value stored at shipment level which applies to each package.
            foreach (IParcelPackageEntity package in iParcel.Packages)
            {
                ApplyProfileValue(profile.IParcel.SkuAndQuantities, package, IParcelPackageFields.SkuAndQuantities);
            }

            base.ApplyProfile(profile, shipment);

            long? accountID = source.IParcelAccountID == 0 && accountRepo.AccountsReadOnly.Any() ?
                accountRepo.AccountsReadOnly.First().IParcelAccountID :
                source.IParcelAccountID;

            ApplyProfileValue(accountID, iParcel, IParcelShipmentFields.IParcelAccountID);
            ApplyProfileValue(source.Service, iParcel, IParcelShipmentFields.Service);
            ApplyProfileValue(source.Reference, iParcel, IParcelShipmentFields.Reference);
            ApplyProfileValue(source.TrackByEmail, iParcel, IParcelShipmentFields.TrackByEmail);
            ApplyProfileValue(source.TrackBySMS, iParcel, IParcelShipmentFields.TrackBySMS);
            ApplyProfileValue(source.IsDeliveryDutyPaid, iParcel, IParcelShipmentFields.IsDeliveryDutyPaid);
        }
        
        /// <summary>
        /// Remove extra packages from the shipment that were not in the profile.
        /// </summary>
        private void RemoveExtraPackages(int profilePackageCount, IParcelShipmentEntity iParcel)
        {
            // Remove any packages that are too many for the profile
            if (profilePackageCount > 0)
            {
                // Go through each package that needs removed
                foreach (IParcelPackageEntity package in iParcel.Packages.Skip(profilePackageCount).ToList())
                {
                    if (package.Weight != 0)
                    {
                        shipmentTotalWeightChanged = true;
                    }

                    // Remove it from the list
                    iParcel.Packages.Remove(package);

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
        }
    }
}