using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Editions;
using ShipWorks.Editions.Brown;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using System;
using System.Linq;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    /// <summary>
    /// ShipmentType restriction
    /// </summary>
    public class ShipmentTypeRestriction : FeatureRestriction
    {
        private const string BestRateUpsRestrictionMessage =
            "There is a UPS account in ShipWorks. Best Rate can no longer be used. " +
            "Please choose another shipping provider.";

        private const string BestRateDisabledMessage =
            "Your plan does not support Best Rate.  You’ll need to upgrade your plan to continue using Best Rate.  " +
            "Please contact our Enterprise Sales Team to learn more about this and other Enterprise features.  " +
            "You can contact us at 1-877-890-4226";

        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IBrownEditionUtility brownEditionUtility;
        private readonly IPostalUtility postalUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeRestriction(IShipmentTypeManager shipmentTypeManager,
            IBrownEditionUtility brownEditionUtility, IPostalUtility postalUtility, IMessageHelper messageHelper) :
            base(messageHelper)
        {
            this.shipmentTypeManager = shipmentTypeManager;
            this.brownEditionUtility = brownEditionUtility;
            this.postalUtility = postalUtility;
        }

        /// <summary>
        /// Works on the ShipmentType edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.ShipmentType;

        /// <summary>
        /// Checks the restriction for this feature
        /// </summary>
        public override EnumResult<EditionRestrictionLevel> CheckWithReason(ILicenseCapabilities capabilities, object data)
        {
            ShipmentTypeCode shipmentType = data as ShipmentTypeCode? ?? ShipmentTypeCode.None;
            Tuple<bool, string> isShipmentTypeDisabled = IsShipmentTypeDisabled(capabilities, shipmentType);
            if (isShipmentTypeDisabled.Item1)
            {
                return EditionRestrictionLevel.Hidden.AsEnumResult(isShipmentTypeDisabled.Item2);
            }

            if (capabilities.UpsStatus == UpsStatus.None)
            {
                return EditionRestrictionLevel.None.AsEnumResult();
            }

            //After this point, we know it is a UPS restricted edition
            if (brownEditionUtility.IsShipmentTypeAllowed(shipmentType))
            {
                return EditionRestrictionLevel.None.AsEnumResult();
            }

            if (shipmentType == ShipmentTypeCode.Other)
            {
                return GetBrownOtherRestriction(capabilities).AsEnumResult();
            }

            if (CheckBrownPostalRestriction(capabilities, shipmentType))
            {
                return EditionRestrictionLevel.None.AsEnumResult();
            }

            return EditionRestrictionLevel.Hidden.AsEnumResult();
        }

        /// <summary>
        /// Checks to see if the given ShipmentTypeCode is restricted
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return CheckWithReason(capabilities, data).Value;
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
        /// Determines whether the specified shipment type is disabled.
        /// </summary>
        private Tuple<bool, string> IsShipmentTypeDisabled(ILicenseCapabilities capabilities, ShipmentTypeCode shipmentTypeCode)
        {
            // We do not restrict the none shipment type
            if (shipmentTypeCode == ShipmentTypeCode.None)
            {
                return Result(false);
            }

            // Check to see if best rate is disabled
            if (shipmentTypeCode == ShipmentTypeCode.BestRate)
            {
                return IsBestRateDisabled(capabilities);
            }

            // Check and see if the given shipment type is in the capabilities shipment type restrictions
            if (capabilities.ShipmentTypeRestriction.ContainsKey(shipmentTypeCode))
            {
                ShipmentType shipmentType = shipmentTypeManager.Get(shipmentTypeCode);

                // If the given shipmen type is disabled return true
                if (capabilities.ShipmentTypeRestriction[shipmentTypeCode].Contains(ShipmentTypeRestrictionType.Disabled))
                {
                    return Result(true);
                }

                // If the given shipment type has AccountRegistration disabled and has no accounts it is as good as disabled
                if (capabilities.ShipmentTypeRestriction[shipmentTypeCode].Contains(ShipmentTypeRestrictionType.AccountRegistration) &&
                    !shipmentType.HasAccounts)
                {
                    return Result(true);
                }
            }

            // The ShipmentTypeCode is not disabled nor is account registration disabled
            return Result(false);
        }

        /// <summary>
        /// Check to see if best rate is disabled
        /// </summary>
        /// <remarks>
        /// There are two conditions that will allow best rate to show in the application
        /// 1. they are in trial and there is no ups account
        /// 2. their account is enabled for best rate and there is no ups account
        /// </remarks>
        private Tuple<bool, string> IsBestRateDisabled(ILicenseCapabilities capabilities)
        {
            // Get the ups shipment type so we can see if there are any ups accounts in the application
            ShipmentType uspShipmentType = shipmentTypeManager.Get(ShipmentTypeCode.UpsOnLineTools);

            if (capabilities.IsInTrial)
            {
                // All customers can use best rate when in trial as long as there
                // aren't any UPS accounts in ShipWorks.
                return Result(uspShipmentType.HasAccounts, BestRateUpsRestrictionMessage);
            }

            // Special checks for best rate as it is part of plan capabilities along with a
            // check to see if there are any UPS accounts: Best rate is disabled if the
            // plan tells us it's not or if there are any UPS accounts
            if (!capabilities.IsBestRateAllowed)
            {
                return Result(true, BestRateDisabledMessage);
            }

            return Result(uspShipmentType.HasAccounts, BestRateUpsRestrictionMessage);
        }

        /// <summary>
        /// Return a boolean result with an empty reason
        /// </summary>
        private Tuple<bool, string> Result(bool value) => Tuple.Create(value, string.Empty);

        /// <summary>
        /// Return a boolean result with the specified reason
        /// </summary>
        private Tuple<bool, string> Result(bool value, string reason) => Tuple.Create(value, reason);
    }
}