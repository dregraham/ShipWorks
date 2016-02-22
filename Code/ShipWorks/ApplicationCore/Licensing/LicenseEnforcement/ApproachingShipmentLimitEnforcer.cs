using System;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{
    /// <summary>
    /// Displays approaching shipment limit warning
    /// </summary>
    public class ApproachingShipmentLimitEnforcer : ILicenseEnforcer
    {
        private const float ShipmentLimitWarningThreshold = 0.8f;

        private readonly IUpgradePlanDlgFactory upgradePlanDlgFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApproachingShipmentLimitEnforcer(IUpgradePlanDlgFactory upgradePlanDlgFactory, Func<Type, ILog> logFactory)
        {
            this.upgradePlanDlgFactory = upgradePlanDlgFactory;
            log = logFactory(typeof(ApproachingShipmentLimitEnforcer));
        }

        /// <summary>
        /// The priority
        /// </summary>
        public EnforcementPriority Priority => EnforcementPriority.Low;

        /// <summary>
        /// The edition feature being enforced
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.ShipmentCount;

        /// <summary>
        /// Displays upgrade dialog if the customer license is within 80% of its shipment limit
        /// </summary>
        public void Enforce(ILicenseCapabilities capabilities, EnforcementContext context, IWin32Window owner)
        {
            EnumResult<ComplianceLevel> enforcementResult = Enforce(capabilities, context);

            try
            {
                if (context == EnforcementContext.Login && enforcementResult.Message != string.Empty)
                {
                    IDialog upgradeLimitDlg = upgradePlanDlgFactory.Create(enforcementResult.Message, owner);
                    upgradeLimitDlg.ShowDialog();
                }
            }
            catch (ShipWorksLicenseException ex)
            {
                log.Error("Error thrown when displaying upgrade plan dialog", ex);
            }
        }

        /// <summary>
        /// Returns a message to display when the customer license is within 80% of its shipment limit
        /// </summary>
        public EnumResult<ComplianceLevel> Enforce(ILicenseCapabilities capabilities, EnforcementContext context)
        {
            float currentShipmentPercentage = (float) capabilities.ProcessedShipments / capabilities.ShipmentLimit;

            string message = string.Empty;

            if (!capabilities.IsInTrial && currentShipmentPercentage >= ShipmentLimitWarningThreshold)
            {
                message = $"You are nearing your shipment limit for the current billing cycle (ending {capabilities.BillingEndDate.ToString("M/d")}).";
            }

            return new EnumResult<ComplianceLevel>(ComplianceLevel.Compliant, message);
        }
    }
}