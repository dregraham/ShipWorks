using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Filters;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Stores.Platforms.Amazon.WizardPages;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;
using ShipWorks.Shipping.Carriers.Amazon;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Amazon Store Integration
    /// </summary>
    public class AmazonStoreType : StoreType
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(AmazonStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Store Type
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.Amazon; }
        }

        /// <summary>
        /// Get the unique, store identifier
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                AmazonStoreEntity amazonStore = Store as AmazonStoreEntity;

                if (amazonStore.AmazonApi == (int)AmazonApi.MarketplaceWebService && amazonStore.MerchantToken.Length == 0)
                {
                    return String.Format("{0}_{1}", amazonStore.MerchantID, amazonStore.MarketplaceID);
                }
                else
                {
                    return amazonStore.MerchantToken;
                }
            }
        }

        /// <summary>
        /// Creates the usercontrol that sits in the Store Settings section of the Store Management
        /// window
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            AmazonStoreEntity amazonStore = Store as AmazonStoreEntity;

            if (amazonStore.AmazonApi == (int)AmazonApi.MarketplaceWebService)
            {
                // MWS doesn't have any store settings to configure
                return new AmazonMwsStoreSettingsControl();
            }
            else
            {
                // Legacy has Weights to configure (and inventory to import)
                return new AmazonStoreSettingsControl();
            }
        }

        /// <summary>
        /// Create the account settings control
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            AmazonStoreEntity amazonStore = Store as AmazonStoreEntity;

            if (amazonStore.AmazonApi == (int)AmazonApi.MarketplaceWebService)
            {
                return new AmazonMwsAccountSettingsControl();
            }
            else
            {
                // legacy
                return new AmazonAccountSettingsControl();
            }
        }

        /// <summary>
        /// Create the collection of setup wizard pages for configuring the integration
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            // show the old signup for as long as possible, 10/10/2011
            // if after 10/10/2011, someone needs to signup that has used the old api in the month bofore, allow it via magickeys
            bool showLegacy = (DateTime.UtcNow < new DateTime(2011, 10,10) && !InterapptiveOnly.MagicKeysDown) ||
                                (InterapptiveOnly.MagicKeysDown && DateTime.UtcNow >= new DateTime(2011, 10, 10));

            if (showLegacy)
            {
                return new List<WizardPage>()
                {
                    new AmazonCredentialsPage(),
                    new AmazonCertificatePage(),
                    new AmazonInventoryPage()
                };
            }
            else
            {
                return new List<WizardPage> ()
                {
                    new AmazonMwsCountryPage(),
                    new AmazonMwsPage(),
                    new AmazonMwsDownloadCriteriaPage()
                };
            }
        }

        /// <summary>
        /// Create the control for generating the online update shipment tasks
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new OnlineUpdateShipmentUpdateActionControl(typeof(AmazonShipmentUploadTask));
        }

        /// <summary>
        /// Get any filters that should be created as an initial filter set when the store is first created.  If the list is non-empty they will
        /// be automatically put in a folder that is filtered on the store... so their is no need to test for that in the generated filter conditions.
        /// </summary>
        public override List<FilterEntity> CreateInitialFilters()
        {
            return new List<FilterEntity>
                {
                    CreateFilterReadyToShip(),
                    CreateFilterShipped(),
                    CreateFilterFba()
                };
        }

        /// <summary>
        /// Creates the filter fba.
        /// </summary>
        private FilterEntity CreateFilterFba()
        {
                FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
                definition.RootContainer.JoinType = ConditionGroupJoinType.And;

                //      [Store] == this store
                StoreCondition storeCondition = new StoreCondition();
                storeCondition.Operator = EqualityOperator.Equals;
                storeCondition.Value = Store.StoreID;
                definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

                // All the order items are not FBA
                AmazonFulfillmentChannelCondition fullfillmentCondition = new AmazonFulfillmentChannelCondition();
                fullfillmentCondition.Operator = EqualityOperator.Equals;
                fullfillmentCondition.Value = AmazonMwsFulfillmentChannel.AFN;
                definition.RootContainer.FirstGroup.Conditions.Add(fullfillmentCondition);

                return new FilterEntity
                {
                    Name = "Fulfilled By Amazon",
                    Definition = definition.GetXml(),
                    IsFolder = false,
                    FilterTarget = (int)FilterTarget.Orders
                };
        }

        /// <summary>
        /// Creates the filter shipped.
        /// </summary>
        private FilterEntity CreateFilterShipped()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;


            //      [Store] == this store
            StoreCondition storeCondition = new StoreCondition();
            storeCondition.Operator = EqualityOperator.Equals;
            storeCondition.Value = Store.StoreID;
            definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);


            // [AND]
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;
            ConditionGroupContainer shippedDefinition = new ConditionGroupContainer();
            definition.RootContainer.SecondGroup = shippedDefinition;

            shippedDefinition.FirstGroup = new ConditionGroup();
            shippedDefinition.FirstGroup.JoinType = ConditionJoinType.Any;

            OnlineStatusCondition shippingStatus = new OnlineStatusCondition();
            shippingStatus.Operator = StringOperator.Equals;
            shippingStatus.TargetValue = "shipped";
            shippedDefinition.FirstGroup.Conditions.Add(shippingStatus);

            shippedDefinition.SecondGroup = InitialDataLoader.CreateDefinitionShipped().RootContainer;

            return new FilterEntity
            {
                Name = "Shipped",
                Definition = definition.GetXml(),
                IsFolder = false,
                FilterTarget = (int)FilterTarget.Orders
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

            // Channel advisor says it has to be unshipped
            OnlineStatusCondition onlineStatus = new OnlineStatusCondition();
            onlineStatus.Operator = StringOperator.Equals;
            onlineStatus.TargetValue = "unshipped";
            definition.RootContainer.FirstGroup.Conditions.Add(onlineStatus);

            // All the order items are not FBA
            AmazonFulfillmentChannelCondition fullfillmentCondition= new AmazonFulfillmentChannelCondition();
            fullfillmentCondition.Operator = EqualityOperator.Equals;
            fullfillmentCondition.Value = AmazonMwsFulfillmentChannel.MFN;
            definition.RootContainer.FirstGroup.Conditions.Add(fullfillmentCondition);

            StoreCondition storeCondition = new StoreCondition();
            storeCondition.Operator = EqualityOperator.Equals;
            storeCondition.Value = Store.StoreID;
            definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

            definition.RootContainer.SecondGroup = InitialDataLoader.CreateDefinitionNotShipped().RootContainer;

            return new FilterEntity
            {
                Name = "Ready to Ship",
                Definition = definition.GetXml(),
                IsFolder = false,
                FilterTarget = (int)FilterTarget.Orders
            };
        }

        /// <summary>
        /// Due to Amazon
        /// </summary>
        public override int AutoDownloadMinimumMinutes
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// Create the Amazon downloader
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            AmazonStoreEntity amazonStore = Store as AmazonStoreEntity;
            if (amazonStore.AmazonApi == (int)AmazonApi.MarketplaceWebService)
            {
                return new AmazonMwsDownloader(Store);
            }
            else
            {
                return new AmazonDownloader(Store);
            }
        }

        /// <summary>
        /// Create the custom Amazon entity
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            AmazonOrderEntity order = new AmazonOrderEntity();

            order.AmazonOrderID = "";
            order.AmazonCommission = 0.0m;
            order.FulfillmentChannel = (int) AmazonMwsFulfillmentChannel.Unknown;
            order.IsPrime = (int)AmazonMwsIsPrime.Unknown;

            return order;
        }

        /// <summary>
        /// Create the identifier for locating amazon orders in the SW database
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new AmazonOrderIdentifier(((AmazonOrderEntity)order).AmazonOrderID);
        }

        /// <summary>
        /// Create the fields that can be used to compare for the same amazon customer
        /// </summary>
        public override IEntityField2[] CreateCustomerIdentifierFields(out bool instanceLookup)
        {
            instanceLookup = false;
            return new EntityField2[] { OrderFields.OnlineCustomerID };
        }

        /// <summary>
        /// Create the store Entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            AmazonStoreEntity storeEntity = new AmazonStoreEntity();

            InitializeStoreDefaults(storeEntity);

            storeEntity.AmazonApi = (int)AmazonApi.MarketplaceWebService;

            // set amazon defaults
            storeEntity.SellerCentralUsername = "";
            storeEntity.SellerCentralPassword = "";
            storeEntity.MerchantName = "";
            storeEntity.MerchantToken = "";
            storeEntity.AccessKeyID = "";
            storeEntity.AuthToken = "";
            storeEntity.Cookie = "";
            storeEntity.CookieExpires = SqlDateTime.MinValue.Value;
            storeEntity.CookieWaitUntil = storeEntity.CookieExpires;
            storeEntity.Certificate = new byte[0];
            storeEntity.WeightDownloads = "";
            storeEntity.MerchantID = "";
            storeEntity.MarketplaceID = "";
            storeEntity.ExcludeFBA = true;
            storeEntity.DomainName = string.Empty;

            storeEntity.SetShippingToken(new AmazonShippingToken()
            {
                ErrorDate = new DateTime(2001, 1, 1),
                ErrorReason = string.Empty
            });


            // Assign the default weight downloading priority
            List<AmazonWeightField> weightPriority = new List<AmazonWeightField>()
            {
                AmazonWeightField.PackagingWeight,
                AmazonWeightField.ItemWeight,
                AmazonWeightField.TotalMetalWeight
            };
            AmazonWeights.SetWeightsPriority(storeEntity, weightPriority);

            return storeEntity;
        }

        /// <summary>
        /// Create the condition group for searching on Amazon Order ID
        /// </summary>
        public override ConditionGroup CreateBasicSearchOrderConditions(string search)
        {
            ConditionGroup group = new ConditionGroup();

            AmazonOrderNumberCondition condition = new AmazonOrderNumberCondition();
            condition.TargetValue = search;
            condition.Operator = StringOperator.BeginsWith;
            group.Conditions.Add(condition);

            return group;
        }

        /// <summary>
        /// MWS has online status
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            AmazonStoreEntity amazonStore = Store as AmazonStoreEntity;

            // Amazon MWS has last modified and status fields
            if (amazonStore.AmazonApi == (int)AmazonApi.MarketplaceWebService)
            {
                if (column == OnlineGridColumnSupport.LastModified)
                {
                    return true;
                }
                else if (column == OnlineGridColumnSupport.OnlineStatus)
                {
                    return true;
                }
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Get the initial download polciy of amazon
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy
        {
            get
            {
                return new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack) { DefaultDaysBack = 14, MaxDaysBack = 30 };
            }
        }

        /// <summary>
        /// Creates the custom OrderItem entity.
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            return new AmazonOrderItemEntity();
        }

        /// <summary>
        /// Generate the template XML output for the given order
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<AmazonOrderEntity>(() => (AmazonOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("Amazon");
            outline.AddElement("AmazonOrderID", () => order.Value.AmazonOrderID);
            outline.AddElement("Commission", () => order.Value.AmazonCommission);
            outline.AddElement("FulfilledBy", () => EnumHelper.GetDescription((AmazonMwsFulfillmentChannel) order.Value.FulfillmentChannel));
            outline.AddElement("Prime", () => EnumHelper.GetDescription((AmazonMwsIsPrime)order.Value.IsPrime));
            outline.AddElement("LatestDeliveryDate", () => order.Value.LatestExpectedDeliveryDate);
            outline.AddElement("EarliestDeliveryDate", () => order.Value.EarliestExpectedDeliveryDate);
        }

        /// <summary>
        /// Create the customer Order Item Xml for the order item provided
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<AmazonOrderItemEntity>(() => (AmazonOrderItemEntity) itemSource());

            ElementOutline outline = container.AddElement("Amazon");
            outline.AddElement("ASIN", () => item.Value.ASIN);
            outline.AddElement("OrderItemCode", () => item.Value.AmazonOrderItemCode);
            outline.AddElement("ConditionNote", () => item.Value.ConditionNote);
        }

        /// <summary>
        /// Create menu commands for upload shipment details
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
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

            // kick off the execution
            executor.ExecuteAsync(ShipmentUploadCallback, new IEnumerable<long>[] { context.SelectedKeys }, null);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private void ShipmentUploadCallback(IEnumerable<long> headers, object userState, BackgroundIssueAdder<IEnumerable<long>> issueAdder)
        {
            // upload the tracking number for the most recent processed, not voided shipment
            try
            {
                AmazonOnlineUpdater shipmentUpdater = new AmazonOnlineUpdater((AmazonStoreEntity)Store);
                shipmentUpdater.UploadOrderShipmentDetails(headers);
            }
            catch (AmazonException ex)
            {
                // log it
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);

                // add the error to issues for the user
                issueAdder.Add(headers, ex);
            }
        }

        /// <summary>
        /// Gets the Amazon domain name associated with this store.
        /// </summary>
        /// <returns>The domain name for the store (e.g. amazon.com, amazon.ca, etc.)</returns>
        /// <exception cref="AmazonException">Thrown when an error occurs when the domain name needs to be looked
        /// up via Amazon MWS</exception>
        public string GetDomainName()
        {
            AmazonStoreEntity amazonStore = Store as AmazonStoreEntity;

            if (string.IsNullOrWhiteSpace(amazonStore.DomainName))
            {
                try
                {
                    // The domain name has not been retrieved from Amazon yet (the store was registered before
                    // this functionality was added), so we need to try to look it up
                    using (AmazonMwsClient client = new AmazonMwsClient(amazonStore))
                    {
                        List<AmazonMwsMarketplace> marketplaces = client.GetMarketplaces();
                        if (marketplaces != null)
                        {
                            // Lookup the marketplace based on the marketplace ID, so we get the correct domain name
                            // in the event the merchant ID (aka Seller ID) is setup with  multiple marketplaces
                            AmazonMwsMarketplace marketplace = marketplaces.FirstOrDefault(m => m.MarketplaceID.ToUpperInvariant() == amazonStore.MarketplaceID.ToUpperInvariant());
                            string domainName = marketplace == null || string.IsNullOrWhiteSpace(marketplace.DomainName) ? string.Empty : marketplace.DomainName;

                            if (!string.IsNullOrWhiteSpace(domainName))
                            {
                                // We're just interested in the domain name without the "www." (e.g. amazon.com instead of www.amazon.com)
                                Uri url = new UriBuilder(domainName).Uri;
                                domainName = url.Host.Replace("www.", string.Empty);

                                // Save the domain to the store, so we don't have to retrieve it from Amazon next time
                                amazonStore.DomainName = domainName;
                                StoreManager.SaveStore(amazonStore);
                            }
                            else
                            {
                                // There are some cases where the seller account is associated with a marketplace (US, CA, UK...), but that
                                // marketplace is not returned in the above GetMarketplaces call. If that happens, instead of defualting to amazon.com,
                                // use the endpoint associated with the country entered when the store was setup.
                                if (!string.IsNullOrWhiteSpace(amazonStore.AmazonApiRegion))
                                {
                                    amazonStore.DomainName = GetDomainNameFromApiRegion(amazonStore);
                                    StoreManager.SaveStore(amazonStore);
                                }
                            }
                        }
                    }
                }
                catch (AmazonException ex)
                {
                    log.ErrorFormat("The domain name could not be retrieved from Amazon MWS due to an error: {0}.", ex.Message);
                    throw;
                }
            }

            return amazonStore.DomainName;
        }

        /// <summary>
        /// Gets an Amazon domain name, base of off the API region
        /// </summary>
        /// <param name="amazonStore">The amazon store.</param>
        /// <returns></returns>
        private static string GetDomainNameFromApiRegion(AmazonStoreEntity amazonStore)
        {
            // There are some marketplaces in here we don't currently support.
            // Figured it wouldn't hurt to plan for the future.
            switch (amazonStore.AmazonApiRegion)
            {
                case "US":
                    return "amazon.com";
                case "CA":
                    return "amazon.ca";
                case "MX":
                    return "amazon.com.mx";
                case "UK":
                    return "amazon.co.uk";
                case "DE":
                    return "amazon.de";
                case "FR":
                    return "amazon.fr";
                case "IT":
                    return "amazon.it";
                case "ES":
                    return "amazon.es";
                case "JP":
                    return "amazon.co.jp";
                case "CN":
                    return "amazon.cn";
                case "IN":
                    return "amazon.in";
                default:
                    return "amazon.com";
            }
        }

        /// <summary>
        /// Gets the default validation setting.
        /// </summary>
        protected override AddressValidationStoreSettingType GetDefaultValidationSetting()
        {
            return AddressValidationStoreSettingType.ValidateAndNotify;
        }
    }
}
