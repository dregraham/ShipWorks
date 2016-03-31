using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.UI;
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
        private readonly IIndex<ShipmentTypeCode, ICarrierAccountRepository<EndiciaAccountEntity>> endiciaAccountRepository;
        private readonly IIndex<ShipmentTypeCode, ICarrierAccountRepository<UpsAccountEntity>> upsAccountRepository;
        private readonly IBrownEditionUtility brownEditionUtility;
        private readonly IPostalUtility postalUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeRestriction(IIndex<ShipmentTypeCode, ICarrierAccountRepository<EndiciaAccountEntity>> endiciaAccountRepository,
                                        IIndex<ShipmentTypeCode, ICarrierAccountRepository<UpsAccountEntity>> upsAccountRepository,
                                        IBrownEditionUtility brownEditionUtility, IPostalUtility postalUtility, IMessageHelper messageHelper) 
            : base(messageHelper)
        {
            this.endiciaAccountRepository = endiciaAccountRepository;
            this.upsAccountRepository = upsAccountRepository;
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
            return shipmentType == ShipmentTypeCode.Endicia && !endiciaAccountRepository[shipmentType].Accounts.Any();
        }

        /// <summary>
        /// Determines whether the specified shipment type is disabled.
        /// </summary>
        private bool IsShipmentTypeDisabled(ILicenseCapabilities capabilities, ShipmentTypeCode shipmentTypeCode)
        {
            if (shipmentTypeCode == ShipmentTypeCode.BestRate)
            {
                if (capabilities.IsInTrial)
                {
                    // All customers can use best rate when in trial as long as there
                    // aren't any UPS accounts in ShipWorks.
                    return upsAccountRepository[ShipmentTypeCode.UpsOnLineTools].Accounts.Any();
                }

                // Special checks for best rate as it is part of plan capabilities along with a 
                // check to see if there are any UPS accounts: Best rate is disabled if the 
                // plan tells us it's not or if there are any UPS accounts
                return !capabilities.IsBestRateAllowed || upsAccountRepository[ShipmentTypeCode.UpsOnLineTools].Accounts.Any();
            }

            return shipmentTypeCode != ShipmentTypeCode.None &&
                   capabilities.ShipmentTypeRestriction.ContainsKey(shipmentTypeCode) &&
                   capabilities.ShipmentTypeRestriction[shipmentTypeCode].Contains(ShipmentTypeRestrictionType.Disabled);


            // The code below is applicable if it is desired to continue to enforce best rate restrictions at
            // the server for the new pricing plans as well (i.e. Tango still needs to indicate best rate is 
            // available AND no UPS accounts are in ShiPWorks.

            //bool isDisabled = shipmentTypeCode != ShipmentTypeCode.None &&
            //       capabilities.ShipmentTypeRestriction.ContainsKey(shipmentTypeCode) &&
            //       capabilities.ShipmentTypeRestriction[shipmentTypeCode].Contains(ShipmentTypeRestrictionType.Disabled);

            //if (shipmentTypeCode == ShipmentTypeCode.BestRate && !isDisabled)
            //{
            //    // Best rate is not disabled on the server, but we have one additional check to perform:
            //    // best rate is disabled if there are any UPS accounts
            //    isDisabled = upsAccountRepository[ShipmentTypeCode.UpsOnLineTools].Accounts.Any();
            //}

            //return isDisabled;
        }
    }
}