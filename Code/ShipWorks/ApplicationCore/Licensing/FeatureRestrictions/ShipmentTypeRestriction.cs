using System.Linq;
using Autofac.Features.Indexed;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    /// <summary>
    /// ShipmentType restriction
    /// </summary>
    public class ShipmentTypeRestriction : FeatureRestriction
    {
        private readonly IIndex<ShipmentTypeCode, ICarrierAccountRepository<EndiciaAccountEntity>> accountRepository;

        public ShipmentTypeRestriction(IIndex<ShipmentTypeCode, ICarrierAccountRepository<EndiciaAccountEntity>> accountRepository)
        {
            this.accountRepository = accountRepository;
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

            if (shipmentType != ShipmentTypeCode.None &&
                capabilities.ShipmentTypeRestriction.ContainsKey(shipmentType) &&
                capabilities.ShipmentTypeRestriction[shipmentType].Contains(ShipmentTypeRestrictionType.Disabled))
            {
                return EditionRestrictionLevel.Hidden;
            }

            // If the ShipmentTypeCode is Endicia and there are no accounts in ShipWorks
            // return hidden to hide the Endicia shipment type from the application
            if (shipmentType == ShipmentTypeCode.Endicia && !accountRepository[shipmentType].Accounts.Any())
            {
                return EditionRestrictionLevel.Hidden;
            }

            return EditionRestrictionLevel.None;
        }
    }
}