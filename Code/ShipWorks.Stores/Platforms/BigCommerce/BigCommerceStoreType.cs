using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Autofac;
using Autofac.Features.Indexed;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared;
using Interapptive.Shared.Enums;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// BigCommerce Store Type implementation
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.BigCommerce)]
    public class BigCommerceStoreType : StoreType
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BigCommerceStoreType));
        readonly Func<BigCommerceStoreEntity, BigCommerceDownloader> createDownloader;
        readonly Func<BigCommerceStoreEntity, IBigCommerceOnlineUpdater> createOnlineUpdater;
        readonly Func<BigCommerceStoreEntity, BigCommerceStatusCodeProvider> createStatusCodeProvider;
        readonly IIndex<StoreTypeCode, StoreSettingsControlBase> storeSettingsControlIndex;
        readonly IIndex<StoreTypeCode, OnlineUpdateActionControlBase> updateActionIndex;
        readonly IIndex<StoreTypeCode, AccountSettingsControlBase> accountSettingsControlIndex;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// We're ignoring the parameter count because these dependencies were not added,
        /// they were just made explicit in the constructor.
        /// </remarks>
        [NDependIgnoreTooManyParams]
        public BigCommerceStoreType(StoreEntity store,
            Func<BigCommerceStoreEntity, BigCommerceDownloader> createDownloader,
            Func<BigCommerceStoreEntity, IBigCommerceOnlineUpdater> createOnlineUpdater,
            Func<BigCommerceStoreEntity, BigCommerceStatusCodeProvider> createStatusCodeProvider,
            IIndex<StoreTypeCode, StoreSettingsControlBase> storeSettingsControlIndex,
            IIndex<StoreTypeCode, OnlineUpdateActionControlBase> updateActionIndex,
            IIndex<StoreTypeCode, AccountSettingsControlBase> accountSettingsControlIndex)
            : base(store)
        {
            this.accountSettingsControlIndex = accountSettingsControlIndex;
            this.updateActionIndex = updateActionIndex;
            this.storeSettingsControlIndex = storeSettingsControlIndex;
            this.createStatusCodeProvider = createStatusCodeProvider;
            this.createOnlineUpdater = createOnlineUpdater;
            this.createDownloader = createDownloader;
        }

        /// <summary>
        /// StoreTypeCode enum value for BigCommerce Store Types
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.BigCommerce;

        /// <summary>
        /// Gets a typed version of the store
        /// </summary>
        public BigCommerceStoreEntity TypedStore => Store as BigCommerceStoreEntity;

        /// <summary>
        /// This is a string that uniquely identifies the store.
        /// Since current customers can have the legacy implementation of BigCommerce, we need to support
        /// the old identifier as well, so use the same algorithm as before.
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                string identifier = TypedStore.ApiUrl.ToLowerInvariant();

                // When upgrading legacy BigCommerce stores, we changed from store module to ApiUrl
                // So it now ends in api/v2/ which doesn't match up with what the license is registered to,
                // so remove api/v2/ to mimic the old license scheme
                int indexOfApiPath = identifier.IndexOf("api/v2/", 0, StringComparison.OrdinalIgnoreCase);
                if (indexOfApiPath > 0)
                {
                    identifier = identifier.Substring(0, indexOfApiPath);
                }

                identifier = Regex.Replace(identifier, "(admin/)?[^/]*(\\.)?[^/]+$", "", RegexOptions.IgnoreCase);

                // The regex above will return just the scheme if there's no ending /, so check for that and
                // reset to the StoreUrl
                if (identifier.EndsWith(Uri.SchemeDelimiter, StringComparison.OrdinalIgnoreCase))
                {
                    identifier = TypedStore.ApiUrl.ToLowerInvariant();
                }

                return identifier;
            }
        }

        /// <summary>
        /// Initial download policy of the store
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy => new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack);

        /// <summary>
        /// Creates a new instance of an BigCommerce store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            BigCommerceStoreEntity bigCommerceStore = new BigCommerceStoreEntity();

            // Set the base store defaults
            InitializeStoreDefaults(bigCommerceStore);

            // Set the BigCommerce store specific defaults
            bigCommerceStore.ApiUrl = string.Empty;
            bigCommerceStore.ApiUserName = string.Empty;
            bigCommerceStore.ApiToken = string.Empty;
            bigCommerceStore.StoreName = "BigCommerce Store";
            bigCommerceStore.WeightUnitOfMeasure = (int) WeightUnitOfMeasure.Pounds;
            bigCommerceStore.DownloadModifiedNumberOfDaysBack = 7;
            bigCommerceStore.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;
            bigCommerceStore.OauthClientId = string.Empty;
            bigCommerceStore.OauthToken = string.Empty;

            return bigCommerceStore;
        }

        /// <summary>
        /// Get the store-specific OrderIdentifier that can be used to identify the specified order.
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            string orderNumberComplete = order.OrderNumberComplete;
            string orderNumber = order.OrderNumber.ToString(CultureInfo.InvariantCulture);

            string invoicePostfix = string.Empty;
            if (orderNumberComplete.Length > (orderNumberComplete.IndexOf(orderNumber, StringComparison.OrdinalIgnoreCase) + orderNumber.Length + 1))
            {
                invoicePostfix = orderNumberComplete.Substring(orderNumberComplete.IndexOf(orderNumber, StringComparison.OrdinalIgnoreCase) + orderNumber.Length + 1);
            }

            return new BigCommerceOrderIdentifier(order.OrderNumber, invoicePostfix);
        }

        /// <summary>
        /// Creates a BigCommerce store-specific instance of an BigCommerceOrderItemEntity
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            return new BigCommerceOrderItemEntity();
        }

        /// <summary>
        /// Create the customer Order Item Xml for the order item provided
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<BigCommerceOrderItemEntity>(() => (BigCommerceOrderItemEntity) itemSource());

            ElementOutline outline = container.AddElement("BigCommerce");
            outline.AddElement("DigitalItem", () => item.Value.IsDigitalItem);
            outline.AddElement("EventDate", () => item.Value.EventDate);
            outline.AddElement("EventName", () => item.Value.EventName);
        }

        /// <summary>
        /// Get a list of supported online BigCommerce statuses
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            BigCommerceStatusCodeProvider statusCodeProvider = createStatusCodeProvider(TypedStore);

            return statusCodeProvider.CodeNames;
        }

        /// <summary>
        /// Get the store-specific fields that are used to uniquely identify an online customer record.  Such
        /// as the eBay User ID or the osCommerce CustomerID.  If a particular store does not have any concept
        /// of a unique online customer, than this can return null.  If multiple fields are returned, they
        /// will be tested using OR.  If customer identifiers are unique per store instance,
        /// set instanceLookup to true.  If they are unique per store type, set instanceLookup to false;
        /// </summary>
        public override IEntityField2[] CreateCustomerIdentifierFields(out bool instanceLookup)
        {
            instanceLookup = true;

            return new IEntityField2[] { OrderFields.OnlineCustomerID };
        }

        /// <summary>
        /// Create a downloader for our current store instance
        /// </summary>
        public override StoreDownloader CreateDownloader() => createDownloader(Store as BigCommerceStoreEntity);

        /// <summary>
        /// Create the control used to configured the actions for online update after shipping
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl() =>
            updateActionIndex[StoreTypeCode.BigCommerce];

        /// <summary>
        /// Create the control that is used for editing the account settings in the Store Settings window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl() =>
            accountSettingsControlIndex[StoreTypeCode.BigCommerce];

        /// <summary>
        /// Create the control to use on the Store Settings dialog, for custom store-specific
        /// configuration options.
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl() =>
            storeSettingsControlIndex[StoreTypeCode.BigCommerce];

        /// <summary>
        /// Indicates if the StoreType supports the display of the given "Online" column.
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.OnlineStatus || column == OnlineGridColumnSupport.LastModified)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to this specific store instance
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            // get possible status codes from the provider
            BigCommerceStatusCodeProvider codeProvider = createStatusCodeProvider(TypedStore);

            // create a menu item for each status
            ICollection<string> statusCodeNames = GetOnlineStatusChoices();

            // Filter out Deleted online status (it isn't a real status)
            statusCodeNames = statusCodeNames.Where(codeName => codeName != BigCommerceConstants.OnlineStatusDeletedName).ToList();

            bool isOne = false;
            foreach (string codeName in statusCodeNames)
            {
                isOne = true;

                MenuCommand command = new MenuCommand(codeName, new MenuCommandExecutor(OnSetOnlineStatus));
                command.Tag = codeProvider.GetCodeValue(codeName);

                commands.Add(command);
            }

            MenuCommand uploadCommand = new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadDetails));
            uploadCommand.BreakBefore = isOne;
            commands.Add(uploadCommand);

            return commands;
        }

        /// <summary>
        /// Command handler for uploading shipment details
        /// </summary>
        private void OnUploadDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Upload Shipment Details",
                "ShipWorks is uploading shipment information.",
                "Updating order {0} of {1}...");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            // kick off the execution
            executor.ExecuteAsync(UploadDetailsCallback, context.SelectedKeys, null);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private void UploadDetailsCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            // upload the tracking number for the most recent processed, not voided shipment
            try
            {
                OrderEntity order = (OrderEntity) DataProvider.GetEntity(orderID);
                if (order == null)
                {
                    log.WarnFormat("Not uploading shipment details for order {0} as it went away.", orderID);
                    return;
                }

                IBigCommerceOnlineUpdater updater = createOnlineUpdater(TypedStore);
                updater.UpdateShipmentDetails(order);
            }
            catch (BigCommerceException ex)
            {
                // log it
                log.ErrorFormat("Error uploading shipment information for orders.  Error message: {0}", ex.Message);

                // add the error to issues for the user
                issueAdder.Add(orderID, ex);
            }
        }

        /// <summary>
        /// Command handler for setting online order status
        /// </summary>
        private void OnSetOnlineStatus(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
               "Set Status",
               "ShipWorks is setting the online status.",
               "Updating order {0} of {1}...");

            MenuCommand command = context.MenuCommand;
            int statusCode = (int) command.Tag;

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };
            executor.ExecuteAsync(SetOnlineStatusCallback, context.SelectedKeys, statusCode);
        }

        /// <summary>
        /// Worker thread method for updating online order status
        /// </summary>
        private void SetOnlineStatusCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            log.Debug(Store.StoreName);

            int statusCode = (int) userState;
            try
            {
                IBigCommerceOnlineUpdater updater = createOnlineUpdater(TypedStore);
                updater.UpdateOrderStatus(orderID, statusCode);
            }
            catch (BigCommerceException ex)
            {
                // log it
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);

                // add the error to issues so we can react later
                issueAdder.Add(orderID, ex);
            }
        }
    }
}
