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
    /// Enforces shipment counts
    /// </summary>
    public class ExceedingShipmentLimitEnforcer : ILicenseEnforcer
    {
        private const int UnlimitedShipments = -1;
        private const string ShipmentLimitExceeded = "http://www.shipworks.com/shipworks/notifications/shipment-limit/exceeded/259854_ShipWorks_Nudge_ShipmentLimit_Exceed.html";
        private const string Title = "Shipment Limit Exceeded";
        
        private readonly IWebBrowserFactory webBrowserFactory;
        private readonly ILog log;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ExceedingShipmentLimitEnforcer(Func<Type, ILog> logFactory, IWebBrowserFactory webBrowserFactory)
        {
            this.webBrowserFactory = webBrowserFactory;
            log = logFactory(typeof(ExceedingShipmentLimitEnforcer));
        }

        /// <summary>
        /// High priority
        /// </summary>
        public EnforcementPriority Priority => EnforcementPriority.High;

        /// <summary>
        /// Enforces the Shipment Count feature
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.ShipmentCount;

        /// <summary>
        /// Shipment Limit doesn't apply to trails.
        /// </summary>
        public bool AppliesToTrial => false;

        /// <summary>
        /// Displays a dlg on the given owner to enforce the shipment limit
        /// </summary>
        public void Enforce(ILicenseCapabilities capabilities, EnforcementContext context, IWin32Window owner)
        {
            EnumResult<ComplianceLevel> compliant = Enforce(capabilities, context);

            if (compliant.Value == ComplianceLevel.NotCompliant)
            {
                try
                {
                    Size size = new Size(920, 500);
                    Uri uri = new Uri(ShipmentLimitExceeded);

                    IDialog dialog = webBrowserFactory.Create(uri, Title, owner, size);
                    dialog.ShowDialog();
                }
                catch (ShipWorksLicenseException ex)
                {
                    log.Error("Error thrown when displaying shipment limit dialog", ex);
                }
            }
        }
        
        /// <summary>
        /// Returns an enum result and message about compliance 
        /// </summary>
        public EnumResult<ComplianceLevel> Enforce(ILicenseCapabilities capabilities, EnforcementContext context)
        {
            // Need to check at login and when labels are created
            if (context == EnforcementContext.CreateLabel || context == EnforcementContext.Login)
            {
                if (capabilities.ShipmentLimit != UnlimitedShipments && capabilities.ProcessedShipments >= capabilities.ShipmentLimit)
                {
                    return new EnumResult<ComplianceLevel>(ComplianceLevel.NotCompliant,
                        "You have reached your shipment limit for this billing cycle. Please upgrade your plan to create labels.");
                }                
            }

            return new EnumResult<ComplianceLevel>(ComplianceLevel.Compliant, string.Empty);
        }
    }
}