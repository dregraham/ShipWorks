﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Platforms.Amazon.WizardPages;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using System.Data.SqlTypes;
using ShipWorks.Data.Connection;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Grid.Paging;
using System.Windows.Forms;
using ShipWorks.Templates.Processing.TemplateXml;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Filters;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Data.Grid;
using ShipWorks.ApplicationCore;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using Interapptive.Shared.Utility;

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
        public override OrderEntity CreateOrderInstance()
        {
            AmazonOrderEntity order = new AmazonOrderEntity();

            order.AmazonOrderID = "";
            order.AmazonCommission = 0.0m;
            order.FulfillmentChannel = (int) AmazonMwsFulfillmentChannel.Unknown;

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
            storeEntity.Cookie = "";
            storeEntity.CookieExpires = SqlDateTime.MinValue.Value;
            storeEntity.CookieWaitUntil = storeEntity.CookieExpires;
            storeEntity.Certificate = new byte[0];
            storeEntity.WeightDownloads = "";
            storeEntity.MerchantID = "";
            storeEntity.MarketplaceID = "";
            storeEntity.ExcludeFBA = true;
            storeEntity.DomainName = string.Empty;

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
                        List<AmazonMwsMarketplace> marketplaces = client.GetMarketplaces(amazonStore.MerchantID);
                        if (marketplaces != null)
                        {
                            // Lookup the marketplace based on the marketplace ID, so we get the correct domain name
                            // in the event the merchant ID is setup with  multiple marketplaces
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
        /// Gets the default validation setting.
        /// </summary>
        protected override AddressValidationStoreSettingType GetDefaultValidationSetting()
        {
            return AddressValidationStoreSettingType.ValidateAndNotify;
        }
    }
}
