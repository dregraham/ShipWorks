using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Applies a postal shipping profile
    /// </summary>
    public abstract class PostalShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        /// <summary>
        /// Applies Constructor
        /// </summary>
        public PostalShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager)
            :base(shipmentTypeManager)
        {
        }

        /// <summary>
        /// Applies a profile
        /// </summary>
        public override void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            base.ApplyProfile(profile, shipment);

            PostalShipmentEntity postalShipment = shipment.Postal;
            IPostalProfileEntity postalProfile = profile.Postal;
            IPackageProfileEntity packageProfile = profile.Packages.Single();

            ApplyProfileValue(postalProfile.Service, postalShipment, PostalShipmentFields.Service);
            ApplyProfileValue(postalProfile.Confirmation, postalShipment, PostalShipmentFields.Confirmation);

            // Special case - only apply if the weight is not zero.  This prevents the weight entry from the default profile from overwriting the prefilled weight from products.
            if (packageProfile.Weight != null && packageProfile.Weight.Value != 0)
            {
                ApplyProfileValue(packageProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }

            ApplyProfilesPackageDims(packageProfile, postalShipment);

            ApplyProfileValue(postalProfile.PackagingType, postalShipment, PostalShipmentFields.PackagingType);
            ApplyProfileValue(postalProfile.NonRectangular, postalShipment, PostalShipmentFields.NonRectangular);
            ApplyProfileValue(postalProfile.NonMachinable, postalShipment, PostalShipmentFields.NonMachinable);

            ApplyProfileValue(postalProfile.CustomsContentType, postalShipment, PostalShipmentFields.CustomsContentType);
            ApplyProfileValue(postalProfile.CustomsContentDescription, postalShipment, PostalShipmentFields.CustomsContentDescription);

            ApplyProfileValue(postalProfile.ExpressSignatureWaiver, postalShipment, PostalShipmentFields.ExpressSignatureWaiver);

            ApplyProfileValue(postalProfile.SortType, postalShipment, PostalShipmentFields.SortType);
            ApplyProfileValue(postalProfile.EntryFacility, postalShipment, PostalShipmentFields.EntryFacility);

            ApplyProfileValue(postalProfile.Memo1, postalShipment, PostalShipmentFields.Memo1);
            ApplyProfileValue(postalProfile.Memo2, postalShipment, PostalShipmentFields.Memo2);
            ApplyProfileValue(postalProfile.Memo3, postalShipment, PostalShipmentFields.Memo3);

            ApplyProfileValue(postalProfile.NoPostage, postalShipment, PostalShipmentFields.NoPostage);
            ApplyProfileValue(postalProfile.Profile.Insurance, postalShipment, PostalShipmentFields.Insurance);

            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);

            shipmentType.UpdateDynamicShipmentData(shipment);
            shipmentType.UpdateTotalWeight(shipment);
        }

        /// <summary>
        /// Apply the profiles dims
        /// </summary>
        private static void ApplyProfilesPackageDims(IPackageProfileEntity packageProfile, PostalShipmentEntity postalShipment)
        {
            ApplyProfileValue(packageProfile.DimsProfileID, postalShipment, PostalShipmentFields.DimsProfileID);
            if (packageProfile.DimsProfileID != null)
            {
                if (packageProfile.DimsLength.GetValueOrDefault() > 0)
                {
                    ApplyProfileValue(packageProfile.DimsLength, postalShipment, PostalShipmentFields.DimsLength);
                }

                if (packageProfile.DimsWidth.GetValueOrDefault() > 0)
                {
                    ApplyProfileValue(packageProfile.DimsWidth, postalShipment, PostalShipmentFields.DimsWidth);
                }

                if (packageProfile.DimsHeight.GetValueOrDefault() > 0)
                {
                    ApplyProfileValue(packageProfile.DimsHeight, postalShipment, PostalShipmentFields.DimsHeight);
                }

                ApplyProfileValue(packageProfile.DimsWeight, postalShipment, PostalShipmentFields.DimsWeight);
                ApplyProfileValue(packageProfile.DimsAddWeight, postalShipment, PostalShipmentFields.DimsAddWeight);
            }
        }
    }
}