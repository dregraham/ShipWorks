using System.Linq;
using Autofac.Features.Indexed;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Editions.Brown;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    /// <summary>
    /// ShipmentType restriction
    /// </summary>
    public class ShipmentTypeRestriction : FeatureRestriction
    {
        private readonly IIndex<ShipmentTypeCode, ICarrierAccountRepository<EndiciaAccountEntity>> accountRepository;
        private readonly IBrownEditionUtility brownEditionUtility;
        private readonly IPostalUtility postalUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeRestriction(
            IIndex<ShipmentTypeCode, ICarrierAccountRepository<EndiciaAccountEntity>> accountRepository, 
            IBrownEditionUtility brownEditionUtility, 
            IPostalUtility postalUtility)
        {
            this.accountRepository = accountRepository;
            this.brownEditionUtility = brownEditionUtility;
            this.postalUtility = postalUtility;
        }

        /// <summary>
        /// Works on the ShipmentType edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.ShipmentType;

        /// <summary>
        /// Checks to see if the given ShipmentTypeCode is restricted
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            ShipmentTypeCode shipmentType = data as ShipmentTypeCode? ?? ShipmentTypeCode.None;

            if (IsShipmentTypeDisabled(capabilities, shipmentType) || CheckEndiciaRestriction(shipmentType))
            {
                return EditionRestrictionLevel.Hidden;
            }

            if (capabilities.UpsStatus == UpsStatus.None)
            {
                return EditionRestrictionLevel.None;
            }

            //After this point, we know it is a UPS restricted edition

            if (brownEditionUtility.IsShipmentTypeAllowed(shipmentType))
            {
                return EditionRestrictionLevel.None;
            }

            if (shipmentType == ShipmentTypeCode.Other)
            {
                return GetBrownOtherRestriction(capabilities);
            }

            if (CheckBrownPostalRestriction(capabilities, shipmentType))
            {
                return EditionRestrictionLevel.None;
            }

            return EditionRestrictionLevel.Hidden;
        }

        /// <summary>
        /// If Discount, "Other" is forbidden.
        /// </summary>
        private static EditionRestrictionLevel GetBrownOtherRestriction(ILicenseCapabilities capabilities)
        {
            return capabilities.UpsStatus == UpsStatus.Discount ?
                EditionRestrictionLevel.Forbidden :
                EditionRestrictionLevel.None;
        }

        /// <summary>
        /// For Brown Edition, return true if PostalAvailability isn't none and we have a postal shipment type
        /// </summary>
        private bool CheckBrownPostalRestriction(ILicenseCapabilities capabilities, ShipmentTypeCode shipmentType)
        {
            // WebTools is the only postal service that a user cannot access even if BrownPostalAvailability is AllServices
            if (shipmentType == ShipmentTypeCode.PostalWebTools)
            {
                return false;
            }

            return capabilities.PostalAvailability != BrownPostalAvailability.None &&
                            (postalUtility.IsPostalShipmentType(shipmentType));
        }

        /// <summary>
        /// If the ShipmentTypeCode is Endicia and there are no accounts in ShipWorks
        /// return hidden to hide the Endicia shipment type from the application
        /// </summary>
        private bool CheckEndiciaRestriction(ShipmentTypeCode shipmentType)
        {
            return shipmentType == ShipmentTypeCode.Endicia && !accountRepository[shipmentType].Accounts.Any();
        }

        /// <summary>
        /// Determines whether [is shipment type disabled]
        /// </summary>
        private static bool IsShipmentTypeDisabled(ILicenseCapabilities capabilities, ShipmentTypeCode shipmentType)
        {
            return shipmentType != ShipmentTypeCode.None &&
                            capabilities.ShipmentTypeRestriction.ContainsKey(shipmentType) &&
                            capabilities.ShipmentTypeRestriction[shipmentType].Contains(ShipmentTypeRestrictionType.Disabled);
        }
    }
}