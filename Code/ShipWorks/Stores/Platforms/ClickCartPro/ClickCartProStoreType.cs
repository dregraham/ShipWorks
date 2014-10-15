using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Communication;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Stores.Platforms.ClickCartPro
{
    /// <summary>
    /// Click Cart Pro integration store type
    /// </summary>
    public class ClickCartProStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Identifying code for Click Cart Pro
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.ClickCartPro; }
        }

        /// <summary>
        /// Gets the logging source
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.ClickCartPro;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ClickCartProStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Gets the module ersion we're requiring
        /// </summary>
        public override Version GetRequiredModuleVersion()
        {
            return new Version("2.4.1");
        }

        /// <summary>
        /// Creates a new order entity
        /// </summary>
        /// <returns></returns>
        protected override OrderEntity CreateOrderInstance()
        {
            ClickCartProOrderEntity clickCartOrder = new ClickCartProOrderEntity();
            clickCartOrder.ClickCartProOrderID = "";

            return clickCartOrder;
        }

        /// <summary>
        /// Creates order identifier
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            ClickCartProOrderEntity ccpOrder = order as ClickCartProOrderEntity;
            if (ccpOrder == null)
            {
                throw new InvalidOperationException("A non ClickCartPro order was provided to CreateOrderIdentifier");
            }

            return new ClickCartProOrderIdentifier(ccpOrder.ClickCartProOrderID);
        }

        /// <summary>
        /// Create a downloader
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new ClickCartProDownloader(Store);
        }

        /// <summary>
        /// ClickCartPro identifies orders by the ClickCartProID
        /// </summary>
        public override string GetOnlineOrderIdentifier(OrderEntity order)
        {
            ClickCartProOrderEntity ccpOrder = order as ClickCartProOrderEntity;
            if (ccpOrder == null)
            {
                throw new InvalidOperationException("A non ClickCartPro order was provided to CreateOrderIdentifier");
            }

            return ccpOrder.ClickCartProOrderID;
        }

        /// <summary>
        /// Create the new web client
        /// </summary>
        public override GenericStoreWebClient CreateWebClient()
        {
            // register parameter renaming and value transforming
            Dictionary<string, VariableTransformer> transformers = new Dictionary<string, VariableTransformer>
            {
                // updatestatus call 
                {"status", new VariableTransformer("code")},

                // getorders, getcount
                {"start", new UnixTimeVariableTransformer()},

                // make sure a value is sent for the "comments" parameter.  Server side fails if it is missing or blank.
                {"comments", new VariableTransformer( (originalValue) => String.IsNullOrEmpty(originalValue) ? "(none)" : originalValue ) }
            };

            // get custom store capabilities. ClickCartPro is essentially OsCommerce
            GenericModuleCapabilities capabilities = LegacyAdapterCapabilities.CreateOscDerivativeDefaults();
            capabilities.OnlineStatusDataType = GenericVariantDataType.Text;

            // create the legacy client instead of the regular genric one
            LegacyAdapterStoreWebClient client = new LegacyAdapterStoreWebClient((GenericModuleStoreEntity)Store, LegacyStyleSheets.ClickCartProStyleSheet, capabilities, transformers);

            // CCP returns MovedPermanently on POSTs
            client.VersionProbeCompatibilityIndicator = HttpStatusCode.MovedPermanently;

            // ClickCartPro requires these variables
            client.AdditionalVariables.Add("ns", "expshipworks");
            client.AdditionalVariables.Add("app", "ccp0");

            // compatiblity mode requires GETs instead of POSTs
            client.CompatibilityVerb = HttpVerb.Get;

            return client;
        }

        /// <summary>
        /// Generate the template XML output for the given order
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<ClickCartProOrderEntity>(() => (ClickCartProOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("ClickCartPro");
            outline.AddElement("ClickCartProOrderID", () => order.Value.ClickCartProOrderID);
        }
    }
}
