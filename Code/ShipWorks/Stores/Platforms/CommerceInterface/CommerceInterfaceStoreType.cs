using System;
using System.Collections.Generic;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Communication;
using Interapptive.Shared.Net;
using System.Net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Stores.Content;
using log4net;
using System.Windows.Forms;
using ShipWorks.Stores.Platforms.CommerceInterface.WizardPages;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores.Platforms.CommerceInterface.CoreExtensions.Filters;

namespace ShipWorks.Stores.Platforms.CommerceInterface
{
    /// <summary>
    /// CommerceInterface store integration
    /// </summary>
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
        public CommerceInterfaceStoreType(StoreEntity store)
            : base(store)
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
                GenericModuleStoreEntity genericStore = (GenericModuleStoreEntity)Store;

                return genericStore.ModuleUsername.Trim().ToLowerInvariant();
            }
        }

        /// <summary>
        /// Create the order downloader
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new CommerceInterfaceDownloader(Store);
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
        public override GenericStoreWebClient CreateWebClient()
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

            // create the legacy client instead of the regular genric one
            CommerceInterfaceWebClient client = new CommerceInterfaceWebClient((GenericModuleStoreEntity)Store, LegacyStyleSheets.CommerceInterface, capabilities, transformers);

            // CommerceInterface throws a 301 Moved Permanently when they receive a POST to their endpoint
            client.VersionProbeCompatibilityIndicator = HttpStatusCode.MovedPermanently;

            // compatiblity mode requires GETs instead of POSTs
            client.CompatibilityVerb = HttpVerb.Get;

            return client;
        }

        /// <summary>
        /// Create the condition grouip for searching
        /// </summary>
        public override ConditionGroup CreateBasicSearchOrderConditions(string search)
        {
            ConditionGroup group = new ConditionGroup();

            CommerceInterfaceNumberCondition numberCondition = new CommerceInterfaceNumberCondition();
            numberCondition.TargetValue = search;
            numberCondition.Operator = StringOperator.BeginsWith;

            group.Conditions.Add(numberCondition);

            return group;
        }

        /// <summary>
        /// Output Template XML for Orders
        /// </summary>
        public override void GenerateTemplateOrderElements(Templates.Processing.TemplateXml.ElementOutlines.ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<CommerceInterfaceOrderEntity>(() => (CommerceInterfaceOrderEntity)orderSource());

            ElementOutline outline = container.AddElement("CommerceInterface");
            outline.AddElement("OverstockNumber", () => order.Value.CommerceInterfaceOrderNumber);
            outline.AddElement("ChannelOrderNumber", () => order.Value.CommerceInterfaceOrderNumber);
        }

        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000065045";

        #region Online Update Commands

        /// <summary>
        /// Create online update commands
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            return new List<MenuCommand>
            {
                new MenuCommand("Upload Shipment Details...", new MenuCommandExecutor(OnUploadShipmentDetails))
            };
        }

        /// <summary>
        /// Command handler for setting online order status
        /// </summary>
        private void OnUploadShipmentDetails(MenuCommandExecutionContext context)
        {
            using (StatusCodeSelectionDlg dlg = new StatusCodeSelectionDlg(CreateStatusCodeProvider()))
            {
                if (dlg.ShowDialog(context.Owner) == DialogResult.OK)
                {
                    int selectedCode = dlg.SelectedStatusCode;
                    BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                                   "Upload Shipment",
                                   "ShipWorks is uploading shipment information",
                                   "Updating order {0} of {1}...");

                    executor.ExecuteCompleted += (o, e) =>
                    {
                        context.Complete(e.Issues, MenuCommandResult.Error);
                    };
                    executor.ExecuteAsync(ShipmentUploadCallback, context.SelectedKeys, selectedCode);
                }
            }
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private void ShipmentUploadCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            // unpackage the selected status code
            int selectedCode = (int)userState;

            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);
            if (shipment == null)
            {
                log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}", orderID);
            }
            else
            {
                try
                {
                    // upload
                    CommerceInterfaceOnlineUpdater updater = new CommerceInterfaceOnlineUpdater((GenericModuleStoreEntity)Store);
                    updater.UploadTrackingNumber(shipment.ShipmentID, selectedCode);
                }
                catch (GenericStoreException ex)
                {
                    // log it
                    log.ErrorFormat("Error uploading shipment details for orderID {0}: {1}", orderID, ex.Message);

                    // add the error to the issues so we can react later
                    issueAdder.Add(orderID, ex);
                }
            }
        }

        #endregion

    }
}
