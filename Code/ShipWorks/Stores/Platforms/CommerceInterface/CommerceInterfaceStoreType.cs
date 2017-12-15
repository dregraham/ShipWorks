﻿using System;
using System.Collections.Generic;
using System.Net;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.CommerceInterface.CoreExtensions.Filters;
using ShipWorks.Stores.Platforms.CommerceInterface.WizardPages;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Stores.Platforms.CommerceInterface
{
    /// <summary>
    /// CommerceInterface store integration
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.CommerceInterface)]
    [Component(RegistrationType.Self)]
    public class CommerceInterfaceStoreType : GenericModuleStoreType
    {
        // Logger
        static ILog log = LogManager.GetLogger(typeof(CommerceInterfaceStoreType));

        /// <summary>
        /// Identifying code for CommerceInterface
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.CommerceInterface;

        /// <summary>
        /// Logging source
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.CommerceInterface;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommerceInterfaceStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager)
            : base(store, messageHelper, orderManager)
        {

        }

        /// <summary>
        /// Create the order entity
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            CommerceInterfaceOrderEntity ciOrder = new CommerceInterfaceOrderEntity();
            ciOrder.CommerceInterfaceOrderNumber = "";

            return ciOrder;
        }

        /// <summary>
        /// Module version requirements
        /// </summary>
        public override Version GetRequiredModuleVersion()
        {
            return new Version("2.9.12");
        }

        /// <summary>
        /// CommerceInterface uses the username
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                GenericModuleStoreEntity genericStore = (GenericModuleStoreEntity) Store;

                return genericStore.ModuleUsername.Trim().ToLowerInvariant();
            }
        }

        /// <summary>
        /// Return capabilities
        /// </summary>
        protected override GenericModuleCapabilities ReadModuleCapabilities(GenericModuleResponse webResponse)
        {
            GenericModuleCapabilities capabilities = LegacyAdapterCapabilities.CreateOscDerivativeDefaults();

            capabilities.OnlineShipmentDetails = false;
            capabilities.OnlineStatusSupport = GenericOnlineStatusSupport.DownloadOnly;
            capabilities.OnlineCustomerSupport = false;

            return capabilities;
        }

        /// <summary>
        /// Create user control for configuring the tasks.
        /// This is customized because we're implementing our own shipment update task because CommerceInterface
        /// does a hybrid status/shipment upload.
        /// </summary>
        public override ShipWorks.Stores.Management.OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new CommerceInterfaceOnlineUpdateActionControl();
        }

        /// <summary>
        /// Create the new web client, which is legacy-capable
        /// </summary>
        public override IGenericStoreWebClient CreateWebClient()
        {
            // register parameter renaming and value transforming
            Dictionary<string, VariableTransformer> transformers = new Dictionary<string, VariableTransformer>
            {
                // updatestatus call
                {"status", new VariableTransformer("code")},

                // getorders, getcount
                {"start", new UnixTimeVariableTransformer()},
            };

            // get custom store capabilities. ClickCartPro is essentially OsCommerce
            GenericModuleCapabilities capabilities = LegacyAdapterCapabilities.CreateOscDerivativeDefaults();

            // CommerceInterface accepts shipment details
            capabilities.OnlineShipmentDetails = true;

            // CommerceInterface does not take status updates
            capabilities.OnlineStatusSupport = GenericOnlineStatusSupport.DownloadOnly;

            // create the legacy client instead of the regular generic one
            CommerceInterfaceWebClient client = new CommerceInterfaceWebClient((GenericModuleStoreEntity) Store, LegacyStyleSheets.CommerceInterface, capabilities, transformers);

            // CommerceInterface throws a 301 Moved Permanently when they receive a POST to their endpoint
            client.VersionProbeCompatibilityIndicator = HttpStatusCode.MovedPermanently;

            // compatibility mode requires GETs instead of POSTs
            client.CompatibilityVerb = HttpVerb.Get;

            return client;
        }

        /// <summary>
        /// Output Template XML for Orders
        /// </summary>
        public override void GenerateTemplateOrderElements(Templates.Processing.TemplateXml.ElementOutlines.ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<CommerceInterfaceOrderEntity>(() => (CommerceInterfaceOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("CommerceInterface");
            outline.AddElement("OverstockNumber", () => order.Value.CommerceInterfaceOrderNumber);
            outline.AddElement("ChannelOrderNumber", () => order.Value.CommerceInterfaceOrderNumber);
        }

        /// <summary>
        /// Account settings help url
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000065045";
    }
}
