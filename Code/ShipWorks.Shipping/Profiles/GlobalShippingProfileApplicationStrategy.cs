using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Profiles
{
    [Component]
    public class GlobalShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        public GlobalShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager) : base(shipmentTypeManager)
        {
        }

        public override void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            base.ApplyProfile(profile, shipment);

            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);
            IEnumerable<IPackageAdapter> packages = shipmentType.GetPackageAdapters(shipment);

            IPackageProfileEntity packageProfile = profile.Packages.Single();


            foreach (IPackageAdapter package in packages)
            {
                if (packageProfile.Weight.HasValue)
                {
                    package.Weight = packageProfile.Weight.Value;
                }

                if (packageProfile.DimsProfileID.HasValue)
                {
                    package.DimsProfileID = packageProfile.DimsProfileID.Value;
                }

                if (packageProfile.DimsLength.HasValue)
                {
                    package.DimsLength = packageProfile.DimsLength.Value;
                }
                
                if (packageProfile.DimsWidth.HasValue)
                {
                    package.DimsWidth = packageProfile.DimsWidth.Value;
                }
                
                if (packageProfile.DimsHeight.HasValue)
                {
                    package.DimsHeight = packageProfile.DimsHeight.Value;
                }
                
                if (packageProfile.Weight.HasValue)
                {
                    package.Weight = packageProfile.Weight.Value;
                }
                
                if (packageProfile.DimsAddWeight.HasValue)
                {
                    package.ApplyAdditionalWeight = packageProfile.DimsAddWeight.Value;
                }
            }
        }
    }
}