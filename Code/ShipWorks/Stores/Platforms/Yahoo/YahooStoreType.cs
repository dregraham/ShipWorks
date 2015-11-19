﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using log4net;
using Quartz.Util;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email.Accounts;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Communication;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Email;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.WizardPages;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration;
using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration.WizardPages;

namespace ShipWorks.Stores.Platforms.Yahoo
{
    /// <summary>
    /// StoreType for Yahoo! stores
    /// </summary>
    public class YahooStoreType : StoreType
    {
        static readonly ILog log = LogManager.GetLogger(typeof(YahooStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public YahooStoreType(StoreEntity store)
            : base(store)
        {
            if (store != null && !(store is YahooStoreEntity))
            {
                throw new ArgumentException("StoreEntity is not instance of YahooStoreEntity.");
            }
        }

        /// <summary>
        /// The type code of the store.
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Yahoo;

        /// <summary>
        /// Gets a value indicating API user or not.
        /// </summary>
        /// <value>
        /// <c>true</c> if API user; otherwise, <c>false</c>.
        /// </value>
        public bool ApiUser { get; private set; }

        /// <summary>
        /// License identifier to uniquely identify the store
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                if (ApiUser)
                {
                    return ((YahooStoreEntity)Store).YahooStoreID;
                }

                EmailAccountEntity account =
                    EmailAccountManager.GetAccount(((YahooStoreEntity) Store).YahooEmailAccountID);

                // If the account was deleted we have to create a made up license that obviously will not be activated to them
                return account == null ? $"{Guid.NewGuid()}@noaccount.com" : account.IncomingUsername;
            }
        }

        /// <summary>
        /// Gets or sets the account settings help URL.
        /// </summary>
        /// <value>
        /// The account settings help URL.
        /// </value>
        public string AccountSettingsHelpUrl => ApiUser ? "" : "http://www.shipworks.com/shipworks/help/Yahoo_Email_Account.html";
    
        /// <summary>
        /// Create a new default initialized instance of the store type
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            YahooStoreEntity store = new YahooStoreEntity();

            InitializeStoreDefaults(store);

            store.StoreName = "My Yahoo Store";
            store.YahooEmailAccountID = 0;
            store.TrackingUpdatePassword = "";
            store.YahooStoreID = "";
            store.AccessToken = "";
            ApiUser = true;
            
            return store;
        }

        /// <summary>
        /// Create the wizard pages that are used to create the store
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            return new List<WizardPage>
                {
                    new YahooAccountPageHost()
                };
        }

        public override InitialDownloadPolicy InitialDownloadPolicy
            =>
                new InitialDownloadPolicy(InitialDownloadRestrictionType.OrderNumber)
                {
                    DefaultStartingOrderNumber = 0
                };
        
        /// <summary>
        /// Create the identifier to uniquely identify the order
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new YahooOrderIdentifier(((YahooOrderEntity) order).YahooOrderID);
        }

        /// <summary>
        /// Create a new instance of a yahoo order
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            return new YahooOrderEntity();
        }

        /// <summary>
        /// Create a new instance of a yahoo order item
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            return new YahooOrderItemEntity();
        }

        /// <summary>
        /// Create the downloader for the store
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            YahooStoreEntity store = (YahooStoreEntity) Store;

            if (ApiUser)
            {
                return new YahooApiDownloader(store);
            }

            return new YahooEmailDownloader(store);
        }

        /// <summary>
        /// Create the control for editing the account settings
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new YahooEmailAccountSettingsControl();
        }

        /// <summary>
        /// Create the store settings control
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return new YahooEmailStoreSettingsControl();
        }

        /// <summary>
        /// Delete any additional data associated with the store
        /// </summary>
        public override void DeleteStoreAdditionalData(SqlAdapter adapter)
        {
            base.DeleteStoreAdditionalData(adapter);

            // Delete the email account used to check orders
            adapter.DeleteEntity(new EmailAccountEntity(((YahooStoreEntity) Store).YahooEmailAccountID));
        }

        /// <summary>
        /// Generate ProStores specific template XML output
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<YahooOrderEntity>(() => (YahooOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("Yahoo");
            outline.AddElement("OrderID", () => order.Value.YahooOrderID);
        }

        /// <summary>
        /// Create specialized template output for yahoo
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<YahooOrderItemEntity>(() => (YahooOrderItemEntity) itemSource());

            ElementOutline outline = container.AddElement("Yahoo");
            outline.AddElement("ProductID", () => item.Value.YahooProductID);
        }

        /// <summary>
        /// Create the commands for updating online
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateCommonCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            if (ApiUser)
            {
                
            }

            MenuCommand uploadCommand = new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadShipmentDetails));
            commands.Add(uploadCommand);

            return commands;
        }


        
        
        /// <summary>
        /// Upload shipment details for the selected orders
        /// </summary>
        private void OnUploadShipmentDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Upload Shipment Details",
                "ShipWorks is uploading the tracking number.",
                "Updating order {0} of {1}...");

            List<EmailOutboundEntity> generatedEmail = new List<EmailOutboundEntity>();

            executor.ExecuteCompleted += (o, e) =>
            {
                // Start emailing for the yahoo email account after generating the online update emails
                EmailCommunicator.StartEmailingMessages(generatedEmail);

                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            executor.ExecuteAsync(UploadShipmentDetailsCallback, context.SelectedKeys, generatedEmail);
        }

        /// <summary>
        /// The worker thread function that does the actual details uploading
        /// </summary>
        private void UploadShipmentDetailsCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            List<EmailOutboundEntity> generatedEmail = (List<EmailOutboundEntity>) userState;

            try
            {
                YahooEmailOnlineUpdater updater = new YahooEmailOnlineUpdater();
                EmailOutboundEntity email = updater.GenerateOrderShipmentUpdateEmail(orderID);

                if (email != null)
                {
                    generatedEmail.Add(email);
                }
            }
            catch (YahooException ex)
            {
                // log it
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);

                // add the error to issues so we can react later
                issueAdder.Add(orderID, ex);
            }
        }

#region Api Integration

        /// <summary>
        /// Creates the initial filters from yahoo's online statuses, if using the Api integration
        /// </summary>
        /// <returns>The list of initial filters</returns>
        public override List<FilterEntity> CreateInitialFilters()
        {
            List<FilterEntity> filters = new List<FilterEntity>();

            if (!((YahooStoreEntity)Store).AccessToken.IsNullOrWhiteSpace())
            {
                Type type = typeof (YahooApiOrderStatus);
                
                foreach (YahooApiOrderStatus status in Enum.GetValues(type))
                {
                    // We want to display the status with spaces, so get the description attribute
                    string description = ((DescriptionAttribute)type.GetMember(status.ToString())[0].GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description;
                    
                    filters.Add(CreateOrderStatusFilter(description));
                }
            }

            return filters;
        }

        /// <summary>
        /// Creates an order status filter for the given order status
        /// </summary>
        /// <param name="orderStatus">The order status to create a filter for</param>
        /// <returns>A filter entity for the given order status</returns>
        private FilterEntity CreateOrderStatusFilter(string orderStatus)
        {
            // [All]
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.FirstGroup.JoinType = ConditionJoinType.All;

            //      [Store] == this store
            StoreCondition storeCondition = new StoreCondition
            {
                Operator = EqualityOperator.Equals,
                Value = Store.StoreID
            };
            definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

            // [AND]
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;
            ConditionGroupContainer shippedDefinition = new ConditionGroupContainer();
            definition.RootContainer.SecondGroup = shippedDefinition;

            //      [Any]
            shippedDefinition.FirstGroup = new ConditionGroup { JoinType = ConditionJoinType.Any };

            OnlineStatusCondition onlineStatus = new OnlineStatusCondition
            {
                Operator = StringOperator.Equals,
                TargetValue = orderStatus
            };
            shippedDefinition.FirstGroup.Conditions.Add(onlineStatus);

            return new FilterEntity
            {
                Name = orderStatus,
                Definition = definition.GetXml(),
                IsFolder = false,
                FilterTarget = (int)FilterTarget.Orders
            };
        }

        /// <summary>
        ///     Indicates if the StoreType supports the display of the given "Online" column.
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
        ///     Indicates what basic grid fields we support hyperlinking for
        /// </summary>
        public override bool GridHyperlinkSupported(EntityField2 field)
        {
            return EntityUtility.IsSameField(field, OrderItemFields.Name);
        }

        /// <summary>
        ///     Handle a link click for the given field
        /// </summary>
        public override void GridHyperlinkClick(EntityField2 field, EntityBase2 entity, IWin32Window owner)
        {
            YahooOrderItemEntity item = entity as YahooOrderItemEntity;

            if (item != null && !item.Url.IsNullOrWhiteSpace())
            {
                WebHelper.OpenUrl(item.Url, owner);
            }
        }
        

        /// <summary>
        ///     Worker thread method for uploading shipment details
        /// </summary>
        private void ShipmentUploadCallback(IEnumerable<long> headers, object userState,
            BackgroundIssueAdder<IEnumerable<long>> issueAdder)
        {
            // upload the tracking number for the most recent processed, not voided shipment
            try
            {
                YahooApiOnlineUpdater shipmentUpdater = new YahooApiOnlineUpdater((YahooStoreEntity)Store);
                shipmentUpdater.UpdateShipmentDetails(headers);
            }
            catch (YahooException ex)
            {
                // log it
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);

                // add the error to issues for the user
                issueAdder.Add(headers, ex);
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

            int statusCode = (int)userState;
            try
            {
                YahooApiOnlineUpdater updater = new YahooApiOnlineUpdater((YahooStoreEntity)Store);
                updater.UpdateOrderStatus(orderID, statusCode);
            }
            catch (YahooException ex)
            {
                // log it
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);

                // add the error to issues so we can react later
                issueAdder.Add(orderID, ex);
            }
        }


        #endregion
    }
}
