using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Communication;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Platforms.ProStores.WizardPages;
using System.Xml;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores.Platforms.ProStores.CoreExtensions.Filters;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Grid.Paging;
using log4net;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.Properties;
using ShipWorks.ApplicationCore.Dashboard;
using System.Windows.Forms;
using Autofac;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Data.Grid;

namespace ShipWorks.Stores.Platforms.ProStores
{
    /// <summary>
    /// StoreType class for ProStores
    /// </summary>
    public class ProStoresStoreType : StoreType
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ProStoresStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// The type code of the store
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.ProStores; }
        }

        /// <summary>
        /// Create a new instance of the ProStores store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            ProStoresStoreEntity store = new ProStoresStoreEntity();

            InitializeStoreDefaults(store);

            return store;
        }

        /// <summary>
        /// Load the store info from the given API Entry Point response
        /// </summary>
        public void LoadApiInfo(string apiEntryPointUrl, XmlDocument apiInfoResponse)
        {
            ProStoresStoreEntity store = (ProStoresStoreEntity) Store;

            store.LoginMethod = (int) ProStoresLoginMethod.ApiToken;
            store.ApiEntryPoint = apiEntryPointUrl;

            store.ShortName = apiInfoResponse.SelectSingleNode("//ShortName").InnerText;
            store.ApiStorefrontUrl = apiInfoResponse.SelectSingleNode("//Storefront").InnerText;
            store.ApiTokenLogonUrl = apiInfoResponse.SelectSingleNode("//ApiLogon").InnerText;
            store.ApiXteUrl = apiInfoResponse.SelectSingleNode("//XmlApi").InnerText;
            store.ApiRestSecureUrl = apiInfoResponse.SelectSingleNode("//RestApiSecure").InnerText;
            store.ApiRestNonSecureUrl = apiInfoResponse.SelectSingleNode("//RestApiNonSecure").InnerText;
            store.ApiRestScriptSuffix = apiInfoResponse.SelectSingleNode("//ScriptSuffix").InnerText;
        }

        /// <summary>
        /// Create the ProStores specific add store wizard pages
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage> 
            {
                new ProStoresProbeSettingsPage(),
                new ProStoresManualSettingsPage(),
                new ProStoresLegacyLoginPage(),
                new ProStoresTokenLoginPage()
            };
        }

        /// <summary>
        /// Create the control for creating online update actions in the add store wizard
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new ProStoresOnlineUpdateActionControl();
        }

        /// <summary>
        /// Create a new store-specific order instance
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            return new ProStoresOrderEntity();
        }

        /// <summary>
        /// Create the OrderIdentifier instance used to uniquely identify this prostores order
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new OrderNumberIdentifier(order.OrderNumber);
        }

        /// <summary>
        /// Returns the fields that identify the customer for an order
        /// </summary>
        public override IEntityField2[] CreateCustomerIdentifierFields(out bool instanceLookup)
        {
            instanceLookup = true;

            return new EntityField2[] { OrderFields.OnlineCustomerID };
        }

        /// <summary>
        /// Create the ProStores downloader
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new ProStoresDownloader((ProStoresStoreEntity) Store);
        }

        /// <summary>
        /// Create the account settings control
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            ProStoresLoginMethod loginMethod = (ProStoresLoginMethod) ((ProStoresStoreEntity) Store).LoginMethod;

            return (loginMethod == ProStoresLoginMethod.ApiToken) ? 
                (AccountSettingsControlBase) new ProStoresAccountSettingsTokenControl() : new ProStoresAccountSettingsLegacyControl();
        }

        /// <summary>
        /// Identifier to uniquely identify the store
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get { return ((ProStoresStoreEntity) Store).ShortName.ToLower(); }
        }

        /// <summary>
        /// Create store specific conditions to use for ProStores for basic search
        /// </summary>
        public override ConditionGroup CreateBasicSearchOrderConditions(string search)
        {
            ProStoresConfirmationCondition condition = new ProStoresConfirmationCondition();
            condition.Operator = StringOperator.BeginsWith;
            condition.TargetValue = search;
            
            ConditionGroup group = new ConditionGroup();
            group.Conditions.Add(condition);

            return group;
        }

        /// <summary>
        /// Create messages to be displayed in the dashboard
        /// </summary>
        public override IEnumerable<DashboardStoreItem> CreateDashboardMessages()
        {
            ProStoresStoreEntity store = (ProStoresStoreEntity) Store;

            if (store.LoginMethod == (int) ProStoresLoginMethod.LegacyUserPass && store.LegacyCanUpgrade)
            {
                return new List<DashboardStoreItem> {
                    new DashboardStoreItem(Store, "UpgradeAuth", Resources.exclamation16, "ShipWorks needs to be updated with the latest ProStores authentication method.",
                        new DashboardActionMethod("[link]Update Now[/link]", OnUpgradeToTokenAuthorization))
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Upgrade the store to using an authorization token
        /// </summary>
        private void OnUpgradeToTokenAuthorization(Control owner, object userState)
        {
            ProStoresStoreEntity store = (ProStoresStoreEntity) Store;

            using (ProStoresTokenWizard wizard = new ProStoresTokenWizard(store))
            {
                if (wizard.ShowDialog(owner) == DialogResult.OK)
                {
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        adapter.SaveAndRefetch(store);
                    }
                }
            }
        }

        /// <summary>
        /// ProStores has online update commands, but does not support OnlineStatus column
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.LastModified)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Policy for how far back to go on an initial download
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy
        {
            get
            {
                return new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack);
            }
        }

        /// <summary>
        /// Generate ProStores specific template XML output
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<ProStoresOrderEntity>(() => (ProStoresOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("ProStores");
            outline.AddElement("Confirmation", () => order.Value.ConfirmationNumber);
            outline.AddElement("Authorized", () => order.Value.AuthorizedDate.HasValue);
            outline.AddElement("AuthorizedDate", () => order.Value.AuthorizedDate);
            outline.AddElement("AuthorizedBy", () => order.Value.AuthorizedBy);
        }

        /// <summary>
        /// Create menu commands for upload shipment details
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateCommonCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            MenuCommand command = new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadDetails));
            commands.Add(command);

            return commands;
        }

        /// <summary>
        /// Command handler for uploading shipment details
        /// </summary>
        private void OnUploadDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<IEnumerable<long>> executor = new BackgroundExecutor<IEnumerable<long>>(context.Owner,
                "Upload Shipment Details",
                "ShipWorks is uploading shipment information.",
                string.Format("Updating {0} orders...", context.SelectedKeys.Count()));

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            executor.ExecuteAsync(ShipmentUploadCallback, new IEnumerable<long>[] { context.SelectedKeys }, null);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private void ShipmentUploadCallback(IEnumerable<long> orderKeys, object userState, BackgroundIssueAdder<IEnumerable<long>> issueAdder)
        {
            try
            {
                ProStoresOnlineUpdater shipmentUpdater = new ProStoresOnlineUpdater();
                shipmentUpdater.UploadOrderShipmentDetails(orderKeys);
            }
            catch (ProStoresException ex)
            {
                // log it
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);

                // add the error to issues for the user
                issueAdder.Add(orderKeys, ex);
            }
        }
    }
}
