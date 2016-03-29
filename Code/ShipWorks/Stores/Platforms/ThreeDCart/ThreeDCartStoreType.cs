using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SD.LLBLGen.Pro.ORMSupportClasses;
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
using log4net;
using System.Globalization;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// 3DCart Store Type implementation
    /// </summary>
    public class ThreeDCartStoreType : StoreType
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ThreeDCartStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// StoreTypeCode enum value for ThreeD Cart Store Types
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.ThreeDCart;

        /// <summary>
        /// Whether or not the user is using the REST API
        /// </summary>
        private bool RestUser => ((ThreeDCartStoreEntity)Store).RestUser;

        /// <summary>
        /// This is a string that uniquely identifies the store.
        /// Since current customers can have the legacy implementation of 3D Cart, we need to support
        /// the old identifier as well, so use the same algorithm as before.
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                ThreeDCartStoreEntity store = (ThreeDCartStoreEntity)Store;

                string identifier = store.StoreUrl.ToLowerInvariant();

                identifier = Regex.Replace(identifier, "(admin/)?[^/]*(\\.)?[^/]+$", "", RegexOptions.IgnoreCase);

                // The regex above will return just the scheme if there's no ending /, so check for that and
                // reset to the StoreUrl
                if (identifier.EndsWith(Uri.SchemeDelimiter, StringComparison.OrdinalIgnoreCase))
                {
                    identifier = store.StoreUrl.ToLowerInvariant();
                }

                if (string.IsNullOrWhiteSpace(identifier))
                {
                    throw new ThreeDCartException("ShipWorks was unable to validate your unique license identifier for your store.");
                }

                return identifier;
            }
        }

        /// <summary>
        /// Initial download policy of the store
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy => new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack);

        /// <summary>
        /// Creates a new instance of an ThreeDCart store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            ThreeDCartStoreEntity threeDCartStore = new ThreeDCartStoreEntity();

            // Set the base store defaults
            InitializeStoreDefaults(threeDCartStore);

            // Set the threeD cart store specific defaults
            threeDCartStore.StoreUrl = string.Empty;
            threeDCartStore.ApiUserKey = string.Empty;
            threeDCartStore.StoreName = "3D Cart Store";
            threeDCartStore.DownloadModifiedNumberOfDaysBack = 7;

            // Default to the Eastern time zone, as that is the default when creating a new store on 3D Cart
            threeDCartStore.TimeZoneID = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").Id;

            // For any newly created stores, we want to use the REST API, so this is defaulted to true
            threeDCartStore.RestUser = true;

            return threeDCartStore;
        }

        /// <summary>
        /// Get the store-specifc OrderIdentifier that can be used to identify the specified order.
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            string orderNumberComplete = order.OrderNumberComplete;
            string orderNumber = order.OrderNumber.ToString(CultureInfo.InvariantCulture);

            string invoicePrefix = orderNumberComplete.Substring(0, orderNumberComplete.IndexOf(orderNumber, StringComparison.OrdinalIgnoreCase));

            string invoicePostfix = string.Empty;
            if (orderNumberComplete.Length > (orderNumberComplete.IndexOf(orderNumber, StringComparison.OrdinalIgnoreCase) + orderNumber.Length + 1))
            {
                invoicePostfix = orderNumberComplete.Substring(orderNumberComplete.IndexOf(orderNumber, StringComparison.OrdinalIgnoreCase) + orderNumber.Length + 1);
            }

            return new ThreeDCartOrderIdentifier(order.OrderNumber, invoicePrefix, invoicePostfix);
        }

        /// <summary>
        /// Creates a 3D Cart store-specific instance of an ThreeDCartOrderItemEntity
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            return new ThreeDCartOrderItemEntity();
        }

        /// <summary>
        /// Creates the order instance.
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            return new ThreeDCartOrderEntity();
        }

        /// <summary>
        /// Get a list of supported online ThreeDCart statuses
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            if (RestUser)
            {
                IEnumerable<EnumEntry<Enums.ThreeDCartOrderStatus>> statuses = EnumHelper.GetEnumList<Enums.ThreeDCartOrderStatus>();
                return statuses.Select(s => s.Description).ToList();
            }

            ThreeDCartStatusCodeProvider statusCodeProvider = new ThreeDCartStatusCodeProvider((ThreeDCartStoreEntity)Store);
            return statusCodeProvider.CodeNames;
        }

        /// <summary>
        /// Get the store-specific fields that are used to unqiuely identifiy an online cusotmer record.  Such
        /// as the eBay User ID or the osCommerce CustomerID.  If a particular store does not have any concept
        /// of a unique online customer, than this can return null.  If multiple fields are returned, they
        /// will be tested using OR.  If customer identifiers are unique per store instance,
        /// set instanceLookup to true.  If they are unique per store type, set instanceLookup to false;
        ///
        /// As per 3D Cart tech support chat, each store has a unique set of IDs, all starting at 1
        /// </summary>
        public override IEntityField2[] CreateCustomerIdentifierFields(out bool instanceLookup)
        {
            instanceLookup = true;

            return new IEntityField2[] { OrderFields.OnlineCustomerID };
        }

        /// <summary>
        /// Create a downloader for our current store instance
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            if (RestUser)
            {
                return new ThreeDCartRestDownloader((ThreeDCartStoreEntity)Store);
            }

            return new ThreeDCartSoapDownloader((ThreeDCartStoreEntity)Store);
        }

        /// <summary>
        /// Create the pages, in order, that will be displayed in the Add Store Wizard
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            List<WizardPage> pages = new List<WizardPage>
                {
                    new WizardPages.ThreeDCartAccountPage(),
                    new WizardPages.ThreeDCartDownloadCriteriaPage(),
                    new WizardPages.ThreeDCartTimeZonePage()
                };

            return pages;
        }

        /// <summary>
        /// Create the control used to configurd the actions for online update after shipping
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new WizardPages.ThreeDCartOnlineUpdateActionControl();
        }

        /// <summary>
        /// Create the control that is used for editing the account settings in the Store Settings window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new ThreeDCartAccountSettingsControl();
        }

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
        /// Generate the template XML output for the given order item
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<ThreeDCartOrderItemEntity>(() => (ThreeDCartOrderItemEntity)itemSource());

            ElementOutline outline = container.AddElement("ThreeDCart");
            outline.AddElement("ShipmentID", () => item.Value.ThreeDCartShipmentID);
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to this specific store instance
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();
            bool isOne = false;
            if (RestUser)
            {
                // create a menu item for each status
                foreach (string codeValue in GetOnlineStatusChoices())
                {
                    isOne = true;
                    MenuCommand command = new MenuCommand(codeValue, OnSetOnlineStatus);
                    command.Tag = EnumHelper.GetEnumByApiValue<Enums.ThreeDCartOrderStatus>(codeValue);
                    commands.Add(command);
                }
            }
            else
            {
                // get possible status codes from the provider
                ThreeDCartStatusCodeProvider codeProvider =
                    new ThreeDCartStatusCodeProvider((ThreeDCartStoreEntity) Store);

                // create a menu item for each status
                foreach (int codeValue in codeProvider.CodeValues)
                {
                    isOne = true;

                    MenuCommand command = new MenuCommand(codeProvider.GetCodeName(codeValue), OnSetOnlineStatus);
                    command.Tag = codeValue;

                    commands.Add(command);
                }
            }

            MenuCommand uploadCommand = new MenuCommand("Upload Shipment Details", OnUploadDetails);
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
                OrderEntity order = (OrderEntity)DataProvider.GetEntity(orderID);
                if (order == null)
                {
                    log.WarnFormat("Not uploading shipment details for order {0} as it went away.", orderID);
                    return;
                }

                if (RestUser)
                {
                    ThreeDCartRestOnlineUpdater updater = new ThreeDCartRestOnlineUpdater((ThreeDCartStoreEntity) Store);
                    updater.UpdateShipmentDetails(order);
                }
                else
                {
                    ThreeDCartSoapOnlineUpdater updater = new ThreeDCartSoapOnlineUpdater((ThreeDCartStoreEntity)Store);
                    updater.UpdateShipmentDetails(order);
                }
            }
            catch (ThreeDCartException ex)
            {
                // log it
                log.ErrorFormat("Error uploading shipment information for orders. Error message: {0}", ex.Message);

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
            int statusCode = (int)command.Tag;

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
                if (RestUser)
                {
                    ThreeDCartRestOnlineUpdater updater = new ThreeDCartRestOnlineUpdater((ThreeDCartStoreEntity)Store);
                    updater.UpdateOrderStatus(orderID, statusCode);
                }
                else
                {
                    ThreeDCartSoapOnlineUpdater updater = new ThreeDCartSoapOnlineUpdater((ThreeDCartStoreEntity)Store);
                    updater.UpdateOrderStatus(orderID, statusCode);
                }
            }
            catch (ThreeDCartException ex)
            {
                // log it
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);

                // add the error to issues so we can react later
                issueAdder.Add(orderID, ex);
            }
        }

        /// <summary>
        /// Create the control to use on the Store Settings dialog, for custom store-specific
        /// configuration options.
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return new ThreeDCartStoreSettingsControl();
        }
    }
}
