﻿using System;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{
    /// <summary>
    /// Enforces shipment counts
    /// </summary>
    public class ShipmentCountEnforcer : ILicenseEnforcer
    {
        private readonly IUpgradePlanDlgFactory upgradePlanDlgFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentCountEnforcer(Func<Type, ILog> logFactory, IUpgradePlanDlgFactory upgradePlanDlgFactory)
        {
            this.upgradePlanDlgFactory = upgradePlanDlgFactory;
            log = logFactory(typeof(ShipmentCountEnforcer));
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
        /// Displays a dlg on the given owner to enforce the shipment limit
        /// </summary>
        public void Enforce(ILicenseCapabilities capabilities, EnforcementContext context, IWin32Window owner)
        {
            EnumResult<ComplianceLevel> compliant = Enforce(capabilities, context);

            if (compliant.Value == ComplianceLevel.NotCompliant)
            {
                try
                {
                    IDialog dialog = upgradePlanDlgFactory.Create(compliant.Message, owner);
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
            // If we are in trial or the ShipmentLimit is -1 we always return compliant
            if (capabilities.IsInTrial || capabilities.ShipmentLimit == -1)
            {
                return new EnumResult<ComplianceLevel>(ComplianceLevel.Compliant, string.Empty);
            }
            
            // If the context is create label and the processed shipments is greater or equal to the shipment limit return note compliant
            if ((context == EnforcementContext.CreateLabel) && (capabilities.ProcessedShipments >= capabilities.ShipmentLimit))
            {
                return new EnumResult<ComplianceLevel>(ComplianceLevel.NotCompliant,
                    "You have reached your shipment limit for this billing cycle. Please upgrade your plan to create labels.");
            }

            return new EnumResult<ComplianceLevel>(ComplianceLevel.Compliant, string.Empty);
        }
    }
}