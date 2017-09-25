﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Filters;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Store implementation for ChannelAdvisor
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.ChannelAdvisor)]
    [Component(RegistrationType.Self)]
    public class ChannelAdvisorStoreType : StoreType
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ChannelAdvisorStoreType));
        public readonly string RedirectUrl = WebUtility.UrlEncode("https://www.interapptive.com/channeladvisor/subscribe.php");
        public const string ApplicationID = "wx76dgzjcwlfy1ck3nb8oke7ql2ukv05";

        /// <summary>
        /// Store Type
        /// </summary>
        public override StoreTypeCode TypeCode =>
            StoreTypeCode.ChannelAdvisor;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Gets the Authorization URL parameters
        /// </summary>
        public string AuthorizeUrlParameters =>
            $"?client_id={ApplicationID}&response_type=code&access_type=offline&scope=orders+inventory&approval_prompt=force&redirect_uri={RedirectUrl}";

        /// <summary>
        /// String uniquely identifying a store instance
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                ChannelAdvisorStoreEntity caStore = (ChannelAdvisorStoreEntity) Store;

                return caStore.ProfileID.ToString();
            }
        }

        /// <summary>
        /// Creates a store entity instance
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            ChannelAdvisorStoreEntity caStore = new ChannelAdvisorStoreEntity();

            InitializeStoreDefaults(caStore);

            caStore.AccountKey = string.Empty;
            caStore.ProfileID = 0;
            caStore.AttributesToDownload = "<Attributes></Attributes>";
            caStore.ConsolidatorAsUsps = false;
            caStore.AmazonApiRegion = string.Empty;
            caStore.AmazonAuthToken = string.Empty;
            caStore.AmazonMerchantID = string.Empty;

            return caStore;
        }

        /// <summary>
        /// Create the CA order entity
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            ChannelAdvisorOrderEntity entity = new ChannelAdvisorOrderEntity();

            entity.CustomOrderIdentifier = "";
            entity.ResellerID = "";
            entity.OnlinePaymentStatus = (int) ChannelAdvisorPaymentStatus.NoChange;
            entity.OnlineCheckoutStatus = (int) ChannelAdvisorCheckoutStatus.NoChange;
            entity.OnlineShippingStatus = (int) ChannelAdvisorShippingStatus.NoChange;
            entity.FlagStyle = "";
            entity.FlagDescription = "";
            entity.FlagType = (int) ChannelAdvisorFlagType.NoFlag;
            entity.MarketplaceNames = "";

            return entity;
        }

        /// <summary>
        /// Creates a custom order item entity
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            ChannelAdvisorOrderItemEntity entity = new ChannelAdvisorOrderItemEntity();

            entity.MarketplaceName = "";
            entity.MarketplaceStoreName = "";
            entity.MarketplaceBuyerID = "";
            entity.MarketplaceSalesID = "";
            entity.Classification = "";
            entity.DistributionCenter = "";
            entity.IsFBA = false;
            entity.DistributionCenterID = -1;

            return entity;
        }

        /// <summary>
        /// Creates the order identifier
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order) =>
            new OrderNumberIdentifier(order.OrderNumber);

        /// <summary>
        /// Create the control for generating the online update shipment tasks
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl() =>
            new OnlineUpdateShipmentUpdateActionControl(typeof(ChannelAdvisorShipmentUploadTask));

        /// <summary>
        /// Get any filters that should be created as an initial filter set when the store is first created.  If the list is non-empty they will
        /// be automatically put in a folder that is filtered on the store... so their is no need to test for that in the generated filter conditions.
        /// </summary>
        public override List<FilterEntity> CreateInitialFilters()
        {
            List<FilterEntity> filters = new List<FilterEntity>
                {
                    CreateFilterReadyToShip(),
                    CreateFilterShipped(),
                };

            if (ShipmentTypeManager.EnabledShipmentTypeCodes.Contains(ShipmentTypeCode.Amazon))
            {
                filters.Add(CreateFilterAmazonPrime());
            }

            return filters;
        }

        /// <summary>
        /// Creates the filter for Amazon Prime orders
        /// </summary>
        private FilterEntity CreateFilterAmazonPrime()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;

            // [Store] == this store
            StoreCondition storeCondition = new StoreCondition();
            storeCondition.Operator = EqualityOperator.Equals;
            storeCondition.Value = Store.StoreID;
            definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

            // Order is Amazon Prime
            ChannelAdvisorIsPrimeCondition primeCondition = new ChannelAdvisorIsPrimeCondition();
            primeCondition.Operator = EqualityOperator.Equals;
            primeCondition.Value = ChannelAdvisorIsAmazonPrime.Yes;
            definition.RootContainer.FirstGroup.Conditions.Add(primeCondition);

            // All the order items are not FBA
            ForEveryItemCondition everyItem = new ForEveryItemCondition();
            definition.RootContainer.FirstGroup.Conditions.Add(everyItem);

            ChannelAdvisorIsFBACondition fbaCondition = new ChannelAdvisorIsFBACondition();
            fbaCondition.Operator = EqualityOperator.Equals;
            fbaCondition.Value = false;
            everyItem.Container.FirstGroup.Conditions.Add(fbaCondition);

            return new FilterEntity
            {
                Name = "Amazon Prime",
                Definition = definition.GetXml(),
                IsFolder = false,
                FilterTarget = (int) FilterTarget.Orders
            };
        }

        /// <summary>
        /// Creates the filter shipped.
        /// </summary>
        private FilterEntity CreateFilterShipped()
        {
            // [All]
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.FirstGroup.JoinType = ConditionJoinType.All;

            //      [Store] == this store
            StoreCondition storeCondition = new StoreCondition();
            storeCondition.Operator = EqualityOperator.Equals;
            storeCondition.Value = Store.StoreID;
            definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

            // [AND]
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;
            ConditionGroupContainer shippedDefinition = new ConditionGroupContainer();
            definition.RootContainer.SecondGroup = shippedDefinition;

            //      [Any]
            shippedDefinition.FirstGroup = new ConditionGroup();
            shippedDefinition.FirstGroup.JoinType = ConditionJoinType.Any;

            // ChannelAdvisor Shipping Status == Shipped
            ChannelAdvisorShippingStatusCondition shippingStatus = new ChannelAdvisorShippingStatusCondition();
            shippingStatus.Operator = EqualityOperator.Equals;
            shippingStatus.Value = ChannelAdvisorShippingStatus.Shipped;
            shippedDefinition.FirstGroup.Conditions.Add(shippingStatus);

            // Is FBA
            ForEveryItemCondition everyItem = new ForEveryItemCondition();
            shippedDefinition.FirstGroup.Conditions.Add(everyItem);

            ChannelAdvisorIsFBACondition isFbaCondition = new ChannelAdvisorIsFBACondition();
            isFbaCondition.Operator = EqualityOperator.Equals;
            isFbaCondition.Value = true;
            everyItem.Container.FirstGroup.Conditions.Add(isFbaCondition);

            // [OR]
            shippedDefinition.JoinType = ConditionGroupJoinType.Or;

            // Shipped within Shipworks
            shippedDefinition.SecondGroup = InitialDataLoader.CreateDefinitionShipped().RootContainer;

            return new FilterEntity
            {
                Name = "Shipped",
                Definition = definition.GetXml(),
                IsFolder = false,
                FilterTarget = (int) FilterTarget.Orders
            };
        }

        /// <summary>
        /// Creates the filter ready to ship.
        /// </summary>
        /// <returns></returns>
        private FilterEntity CreateFilterReadyToShip()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;
            definition.RootContainer.FirstGroup.JoinType = ConditionJoinType.All;

            // For this store
            StoreCondition storeCondition = new StoreCondition();
            storeCondition.Operator = EqualityOperator.Equals;
            storeCondition.Value = Store.StoreID;
            definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

            // Channel advisor says it has to be unshipped
            ChannelAdvisorShippingStatusCondition channelAdvisorShippingStatusCondition = new ChannelAdvisorShippingStatusCondition();
            channelAdvisorShippingStatusCondition.Operator = EqualityOperator.Equals;
            channelAdvisorShippingStatusCondition.Value = ChannelAdvisorShippingStatus.Unshipped;
            definition.RootContainer.FirstGroup.Conditions.Add(channelAdvisorShippingStatusCondition);

            // All the order items are not FBA
            ForEveryItemCondition everyItem = new ForEveryItemCondition();
            definition.RootContainer.FirstGroup.Conditions.Add(everyItem);

            ChannelAdvisorIsFBACondition notFba = new ChannelAdvisorIsFBACondition();
            notFba.Operator = EqualityOperator.Equals;
            notFba.Value = false;
            everyItem.Container.FirstGroup.Conditions.Add(notFba);


            ChannelAdvisorPaymentStatusCondition channelAdvisorPaymentStatus = new ChannelAdvisorPaymentStatusCondition();
            channelAdvisorPaymentStatus.Operator = EqualityOperator.Equals;
            channelAdvisorPaymentStatus.Value = ChannelAdvisorPaymentStatus.Cleared;
            definition.RootContainer.FirstGroup.Conditions.Add(channelAdvisorPaymentStatus);

            definition.RootContainer.SecondGroup = InitialDataLoader.CreateDefinitionNotShipped().RootContainer;

            return new FilterEntity
            {
                Name = "Ready to Ship",
                Definition = definition.GetXml(),
                IsFolder = false,
                FilterTarget = (int) FilterTarget.Orders
            };
        }

        /// <summary>
        /// Create the CA store settings
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl() =>
            new ChannelAdvisorSettingsControl();

        /// <summary>
        /// Create the condition group for searching on Channel Advisor Order ID
        /// </summary>
        public override ConditionGroup CreateBasicSearchOrderConditions(string search)
        {
            ConditionGroup group = new ConditionGroup();

            ChannelAdvisorOrderIDCondition orderIdConditon = new ChannelAdvisorOrderIDCondition();
            orderIdConditon.TargetValue = search;
            orderIdConditon.Operator = StringOperator.BeginsWith;

            ChannelAdvisorMarketplaceBuyerCondition buyerCondition = new ChannelAdvisorMarketplaceBuyerCondition();
            buyerCondition.TargetValue = search;
            buyerCondition.Operator = StringOperator.BeginsWith;

            ForAnyItemCondition anyItemCondition = new ForAnyItemCondition();
            anyItemCondition.Container.FirstGroup.JoinType = ConditionJoinType.Any;
            anyItemCondition.Container.FirstGroup.Conditions.Add(buyerCondition);

            group.JoinType = ConditionJoinType.Any;
            group.Conditions.Add(orderIdConditon);
            group.Conditions.Add(anyItemCondition);

            return group;
        }

        /// <summary>
        /// ChannelAdvisor does not have an Online Status
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
        /// Create the CA download policy
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy =>
            new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack);

        /// <summary>
        /// Generate CA specific template order elements
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<ChannelAdvisorOrderEntity>(() => (ChannelAdvisorOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("ChannelAdvisor");
            outline.AddElement("ResellerID", () => order.Value.ResellerID);
            outline.AddElement("OrderID", () => order.Value.CustomOrderIdentifier);
            outline.AddElement("FlagStyle", () => order.Value.FlagStyle);
            outline.AddElement("FlagDescription", () => order.Value.FlagDescription);
            outline.AddElement("IsPrime", () => EnumHelper.GetDescription((ChannelAdvisorIsAmazonPrime) order.Value.IsPrime));
        }

        /// <summary>
        /// Generate CA specific template item elements
        /// </summary>
        [NDependIgnoreLongMethod]
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<ChannelAdvisorOrderItemEntity>(() => (ChannelAdvisorOrderItemEntity) itemSource());

            ElementOutline outline = container.AddElement("ChannelAdvisor");
            outline.AddElement("Classification", () => item.Value.Classification);
            outline.AddElement("DistributionCenterID", () => item.Value.DistributionCenterID);
            outline.AddElement("DistributionCenter", () => item.Value.DistributionCenter);
            outline.AddElement("DistributionCenterName", () => item.Value.DistributionCenterName);
            outline.AddElement("HarmonizedCode", () => item.Value.HarmonizedCode);
            outline.AddElement("FulfilledByAmazon", () => item.Value.IsFBA);
            outline.AddElement("MPN", () => item.Value.MPN);

            // These are a child of the "ChannelAdvisor" element
            ElementOutline marketplace = outline.AddElement("Marketplace");
            marketplace.AddElement("Name", () => item.Value.MarketplaceName);
            marketplace.AddElement("StoreName", () => item.Value.MarketplaceStoreName);
            marketplace.AddElement("BuyerID", () => item.Value.MarketplaceBuyerID);
            marketplace.AddElement("SalesID", () => item.Value.MarketplaceSalesID);

            // The legacy ones were a child of the Order element
            ElementOutline legacy = container.AddElement("Marketplace");
            legacy.AddAttributeLegacy2x();
            legacy.AddElement("Name", () => item.Value.MarketplaceName);
            legacy.AddElement("StoreName", () => item.Value.MarketplaceStoreName);
            legacy.AddElement("BuyerID", () => item.Value.MarketplaceBuyerID);
            legacy.AddElement("ItemID", () => item.Value.MarketplaceSalesID);
        }

        /// <summary>
        /// Create menu commands for uploading shipment details
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            // shipment details
            MenuCommand command = new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadDetails));
            commands.Add(command);

            return commands;
        }

        /// <summary>
        /// MenuCommand handler for uploading shipment details
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

            executor.ExecuteAsync(ShipmentUploadCallback, context.SelectedKeys, null);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private void ShipmentUploadCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);
            if (shipment == null)
            {
                log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}", orderID);
            }
            else
            {
                try
                {
                    ChannelAdvisorOnlineUpdater updater = new ChannelAdvisorOnlineUpdater((ChannelAdvisorStoreEntity) Store);
                    updater.UploadTrackingNumber(shipment);
                }
                catch (ChannelAdvisorException ex)
                {
                    // log it
                    log.ErrorFormat("Error uploading shipment information for orderID {0}: {1}", orderID, ex.Message);

                    // add the error to issues so we can react later
                    issueAdder.Add(orderID, ex);
                }
            }
        }
    }
}
