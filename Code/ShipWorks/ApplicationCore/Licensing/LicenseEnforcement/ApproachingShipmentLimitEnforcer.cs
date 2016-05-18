using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Editions;
using ShipWorks.UI;
using System;
using System.Windows;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{
    /// <summary>
    /// Displays approaching shipment limit warning
    /// </summary>
    public class ApproachingShipmentLimitEnforcer : ILicenseEnforcer
    {
        private const int UnlimitedShipments = -1;
        private const float ShipmentLimitWarningThreshold = 0.8f;
        private const float ShipmentLimitExceededThreshold = 1f;
        private const string ApproachingShipmentLimitUrl = "https://www.interapptive.com/shipworks/notifications/shipment-limit/approaching/259854_ShipWorks_Nudge_ShipmentLimit_Approching.html";
        private const string ApproachingShipmentLimitTitle = "Approaching Shipment Limit";

        private readonly IWebBrowserFactory webBrowserDlgFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApproachingShipmentLimitEnforcer(IWebBrowserFactory webBrowserDlgFactory, Func<Type, ILog> logFactory)
        {
            this.webBrowserDlgFactory = webBrowserDlgFactory;
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
        /// Simpment Limit doesn't apply to trials.
        /// </summary>
        public bool AppliesTo(ILicenseCapabilities capabilities) => !capabilities.IsInTrial;

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
                    Size size = new Size(920, 500);
                    Uri uri = new Uri(ApproachingShipmentLimitUrl);

                    IDialog browserDialog = webBrowserDlgFactory.Create(uri, ApproachingShipmentLimitTitle, owner, size);
                    browserDialog.ShowDialog();
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
            string message = string.Empty;

            // unlimited shipment limit is always in compliance
            if (capabilities.ShipmentLimit != UnlimitedShipments)
            {
                float currentShipmentPercentage = (float) capabilities.ProcessedShipments / capabilities.ShipmentLimit;

                if (currentShipmentPercentage >= ShipmentLimitWarningThreshold && currentShipmentPercentage < ShipmentLimitExceededThreshold)
                {
                    // The current shipment percentage is greater than or equal to the threshold
                    message = $"You are nearing your shipment limit for the current billing cycle ending {capabilities.BillingEndDate.ToString("M/d")}.";
                }
            }

            return new EnumResult<ComplianceLevel>(ComplianceLevel.Compliant, message);
        }
    }
}