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
    public class PostalShippingProfileApplicationStrategy : IShippingProfileApplicationStrategy
    {
        private readonly IShippingProfileApplicationStrategy baseStrategy;
        private readonly ShipmentType shipmentType;

        /// <summary>
        /// Applies Constructor
        /// </summary>
        public PostalShippingProfileApplicationStrategy(IShippingProfileApplicationStrategy baseStrategy,
            ShipmentType shipmentType)
        {
            this.baseStrategy = baseStrategy;
            this.shipmentType = shipmentType;
        }
        
        /// <summary>
        /// Applies a profile 
        /// </summary>
        public void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            baseStrategy.ApplyProfile(profile, shipment);

            PostalShipmentEntity postalShipment = shipment.Postal;
            IPostalProfileEntity postalProfile = profile.Postal;
            IPackageProfileEntity packageProfile = profile.Packages.Single();
            
            ShippingProfileUtility.ApplyProfileValue(postalProfile.Service, postalShipment, PostalShipmentFields.Service);
            ShippingProfileUtility.ApplyProfileValue(postalProfile.Confirmation, postalShipment, PostalShipmentFields.Confirmation);

            // Special case - only apply if the weight is not zero.  This prevents the weight entry from the default profile from overwriting the prefilled weight from products.
            if (packageProfile.Weight != null && packageProfile.Weight.Value != 0)
            {
                ShippingProfileUtility.ApplyProfileValue(packageProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }

            ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsProfileID, postalShipment, PostalShipmentFields.DimsProfileID);
            if (packageProfile.DimsProfileID != null)
            {
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsLength, postalShipment, PostalShipmentFields.DimsLength);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWidth, postalShipment, PostalShipmentFields.DimsWidth);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsHeight, postalShipment, PostalShipmentFields.DimsHeight);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWeight, postalShipment, PostalShipmentFields.DimsWeight);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsAddWeight, postalShipment, PostalShipmentFields.DimsAddWeight);
            }

            ShippingProfileUtility.ApplyProfileValue(postalProfile.PackagingType, postalShipment, PostalShipmentFields.PackagingType);
            ShippingProfileUtility.ApplyProfileValue(postalProfile.NonRectangular, postalShipment, PostalShipmentFields.NonRectangular);
            ShippingProfileUtility.ApplyProfileValue(postalProfile.NonMachinable, postalShipment, PostalShipmentFields.NonMachinable);

            ShippingProfileUtility.ApplyProfileValue(postalProfile.CustomsContentType, postalShipment, PostalShipmentFields.CustomsContentType);
            ShippingProfileUtility.ApplyProfileValue(postalProfile.CustomsContentDescription, postalShipment, PostalShipmentFields.CustomsContentDescription);

            ShippingProfileUtility.ApplyProfileValue(postalProfile.ExpressSignatureWaiver, postalShipment, PostalShipmentFields.ExpressSignatureWaiver);

            ShippingProfileUtility.ApplyProfileValue(postalProfile.SortType, postalShipment, PostalShipmentFields.SortType);
            ShippingProfileUtility.ApplyProfileValue(postalProfile.EntryFacility, postalShipment, PostalShipmentFields.EntryFacility);

            ShippingProfileUtility.ApplyProfileValue(postalProfile.Memo1, postalShipment, PostalShipmentFields.Memo1);
            ShippingProfileUtility.ApplyProfileValue(postalProfile.Memo2, postalShipment, PostalShipmentFields.Memo2);
            ShippingProfileUtility.ApplyProfileValue(postalProfile.Memo3, postalShipment, PostalShipmentFields.Memo3);

            ShippingProfileUtility.ApplyProfileValue(postalProfile.NoPostage, postalShipment, PostalShipmentFields.NoPostage);
            ShippingProfileUtility.ApplyProfileValue(postalProfile.Profile.Insurance, postalShipment, PostalShipmentFields.Insurance);

            shipmentType.UpdateDynamicShipmentData(shipment);
            shipmentType.UpdateTotalWeight(shipment);
        }
    }
}