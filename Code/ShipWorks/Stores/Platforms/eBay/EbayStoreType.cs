using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.ApplicationCore.Dashboard;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Editions.Freemium;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.OrderItems;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Messaging.Messages;
using ShipWorks.Properties;
using ShipWorks.Shipping;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Filters;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Stores.Platforms.Ebay.OrderCombining;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.WizardPages;
using ShipWorks.Stores.Platforms.PayPal;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Templates.Tokens;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Entrypoint for the eBay integration.
    /// </summary>
    public class EbayStoreType : StoreType
    {
        /// <summary>
        /// The possible Update Online Status options
        /// </summary>
        enum EbayOnlineAction
        {
            Paid,
            Shipped,
            NotPaid,
            NotShipped
        }

        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(EbayStoreType));

        /// <summary>
        /// Static constructor
        /// </summary>
        static EbayStoreType()
        {
            EbayOrderItemEntity.SetEffectiveCheckoutStatusAlgorithm(e => (int) EbayUtility.GetEffectivePaymentStatus(e));
            EbayOrderItemEntity.SetEffectivePaymentMethodAlgorithm(e => (int) EbayUtility.GetEffectivePaymentMethod(e));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Gets the TypeCode for this store
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.Ebay; }
        }

        /// <summary>
        /// Unique identifier for the account
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get { return ((EbayStoreEntity) Store).EBayUserID; }
        }

        /// <summary>
        /// Creates the order downloader
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new EbayDownloader((EbayStoreEntity) Store);
        }

        /// <summary>
        /// Creates the account settings control displayed in the Manage Stores window.
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return new EbayStoreSettingsControl();
        }

        /// <summary>
        /// Creates the account settings control displayed in the the Manage
        /// Stores window
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new EbayAccountSettingsControl();
        }

        /// <summary>
        /// Create the setup wizard pages
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            List<WizardPage> pages = new List<WizardPage>();

            bool isFreemium = FreemiumFreeEdition.IsActive;

            if (isFreemium)
            {
                pages.Add(new FreemiumStoreWizardWelcomePage());
            }

            pages.Add(new EBayAccountPage());

            if (isFreemium)
            {
                pages.Add(new FreemiumStoreWizardValidateAccountPage());
            }

            // People end up thinking they have to download paypal details and images, when really it just takes forever.  Don't show these options by default
            // pages.Add(new EBayPayPalPage());
            // pages.Add(new EBayOptionsPage());

            return pages;
        }

        /// <summary>
        /// Create the control for displaying the add store wizard online update action configuration
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new EbayOnlineUpdateActionControl();
        }

        /// <summary>
        /// Get the initial set of filters supported by ebay
        /// </summary>
        public override List<FilterEntity> CreateInitialFilters()
        {
            return new List<FilterEntity>
                {
                    CreateFilterAwaitingPayment(),
                    CreateFilterReadyToShip(),
                    CreateFilterNeedsFeedback()
                };
        }

        /// <summary>
        /// Create the "Awaiting Payment" filter entity
        /// </summary>
        private FilterEntity CreateFilterAwaitingPayment()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);

            ForAnyItemCondition anyItem = new ForAnyItemCondition();
            definition.RootContainer.FirstGroup.Conditions.Add(anyItem);

            EbayItemPaymentStatusCondition checkoutStatus = new EbayItemPaymentStatusCondition();
            checkoutStatus.Operator = EqualityOperator.NotEqual;
            checkoutStatus.Value = EbayEffectivePaymentStatus.Paid;
            anyItem.Container.FirstGroup.Conditions.Add(checkoutStatus);

            return new FilterEntity
            {
                Name = "Awaiting Payment",
                Definition = definition.GetXml(),
                IsFolder = false,
                FilterTarget = (int) FilterTarget.Orders
            };
        }

        /// <summary>
        /// Create the "Ready to Ship" filter entity
        /// </summary>
        private FilterEntity CreateFilterReadyToShip()
        {
            // First it has to be not shipped
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;

            // It has to be paid
            ForEveryItemCondition everyItem = new ForEveryItemCondition();
            definition.RootContainer.FirstGroup.Conditions.Add(everyItem);

            EbayItemPaymentStatusCondition checkoutStatus = new EbayItemPaymentStatusCondition();
            checkoutStatus.Operator = EqualityOperator.Equals;
            checkoutStatus.Value = EbayEffectivePaymentStatus.Paid;
            everyItem.Container.FirstGroup.Conditions.Add(checkoutStatus);

            // It also has to be not yet shipped
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
        /// Create the "Needs Feedback" filter entity
        /// </summary>
        private FilterEntity CreateFilterNeedsFeedback()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);

            ForEveryItemCondition everyItem = new ForEveryItemCondition();
            everyItem.Container.FirstGroup.JoinType = ConditionJoinType.All;
            definition.RootContainer.FirstGroup.Conditions.Add(everyItem);

            EbayFeedbackCondition buyerLeft = new EbayFeedbackCondition();
            buyerLeft.Operator = EqualityOperator.Equals;
            buyerLeft.Value = EbayFeedbackConditionStatusType.BuyerLeftPositive;
            everyItem.Container.FirstGroup.Conditions.Add(buyerLeft);

            EbayFeedbackCondition sellerNot = new EbayFeedbackCondition();
            sellerNot.Operator = EqualityOperator.Equals;
            sellerNot.Value = EbayFeedbackConditionStatusType.SellerNotLeftForBuyer;
            everyItem.Container.FirstGroup.Conditions.Add(sellerNot);

            return new FilterEntity
            {
                Name = "Needs Feeback",
                Definition = definition.GetXml(),
                IsFolder = false,
                FilterTarget = (int) FilterTarget.Orders
            };
        }

        /// <summary>
        /// Creates an eBay-specific order instance
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            return new EbayOrderEntity()
            {
                EbayOrderID = 0,
                EbayBuyerID = "",

                SelectedShippingMethod = (int) EbayShippingMethod.DirectToBuyer,

                GspEligible = false,
                GspFirstName = "",
                GspLastName = "",
                GspStreet1 = "",
                GspStreet2 = "",
                GspCity = "",
                GspStateProvince = "",
                GspPostalCode = "",
                GspCountryCode = "",
                GspReferenceID = "",

                RollupEbayItemCount = 0,

                RollupEffectiveCheckoutStatus = null,
                RollupEffectivePaymentMethod = null,

                RollupFeedbackReceivedComments = null,
                RollupFeedbackReceivedType = null,

                RollupFeedbackLeftComments = null,
                RollupFeedbackLeftType = null,

                RollupPayPalAddressStatus = null,

                CombinedLocally = false
            };
        }

        /// <summary>
        /// Create the eBay order item
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            return new EbayOrderItemEntity()
            {
                FeedbackLeftType = (int) EbayFeedbackType.None,
                FeedbackLeftComments = "",
                FeedbackReceivedType = (int) EbayFeedbackType.None,
                FeedbackReceivedComments = "",

                MyEbayPaid = false,
                MyEbayShipped = false,

                PaymentStatus = (int) PaymentStatusCodeType.NoPaymentFailure,
                PaymentMethod = (int) BuyerPaymentMethodCodeType.None,

                PayPalAddressStatus = (int) AddressStatusCodeType.None,
                PayPalTransactionID = "",

                SellingManagerRecord = 0,
            };
        }

        /// <summary>
        /// Creates the idnetifier to locate a particular order
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            EbayOrderEntity ebayOrder = order as EbayOrderEntity;
            if (ebayOrder == null)
            {
                throw new InvalidOperationException("A non Ebay order was passed to the EbayStoreType.");
            }

            long itemId = 0;
            long transactionId = 0;
            long ebayOrderId = ebayOrder.EbayOrderID;

            if (ebayOrderId == 0)
            {
                EbayOrderItemEntity orderItem = ebayOrder.OrderItems.FirstOrDefault() as EbayOrderItemEntity;
                if (orderItem != null)
                {
                    itemId = orderItem.EbayItemID;
                    transactionId = orderItem.EbayTransactionID;
                }
            }

            // create the identifier
            return new EbayOrderIdentifier(ebayOrderId, itemId, transactionId);
        }

        /// <summary>
        /// Returns the order field(s) that uniquely identifies a customer
        /// </summary>
        public override IEntityField2[] CreateCustomerIdentifierFields(out bool instanceLookup)
        {
            // ebay user ids are global
            instanceLookup = false;

            return new IEntityField2[] { EbayOrderFields.EbayBuyerID };
        }

        /// <summary>
        /// Creates an initializes a new Store Entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            EbayStoreEntity ebayStore = new EbayStoreEntity();

            InitializeStoreDefaults(ebayStore);

            ebayStore.EBayUserID = "";
            ebayStore.EBayToken = "";
            ebayStore.EBayTokenExpire = SqlDateTime.MinValue.Value;
            ebayStore.PayPalApiCertificate = null;
            ebayStore.PayPalApiUserName = "";
            ebayStore.PayPalApiPassword = "";
            ebayStore.PayPalApiSignature = "";
            ebayStore.PayPalApiCredentialType = (short) PayPalCredentialType.Signature;
            ebayStore.DownloadItemDetails = false;
            ebayStore.DownloadPayPalDetails = false;
            ebayStore.DownloadOlderOrders = false;

            List<BuyerPaymentMethodCodeType> defaults = new List<BuyerPaymentMethodCodeType>
            {
                BuyerPaymentMethodCodeType.PayPal,
                BuyerPaymentMethodCodeType.CCAccepted,
            };
            ebayStore.AcceptedPaymentList = AcceptedPayments.AssembleValue(defaults);

            // default shipping methods used when creating Combined Payments
            ebayStore.DomesticShippingService = "USPSExpressFlatRateEnvelope";
            ebayStore.InternationalShippingService = "USPSPriorityFlatRateEnvelope";

            return ebayStore;
        }

        /// <summary>
        /// Create the condition group for searching
        /// </summary>
        public override ConditionGroup CreateBasicSearchOrderConditions(string search)
        {
            ConditionGroup group = new ConditionGroup();

            EbayBuyerIDCondition buyerCondition = new EbayBuyerIDCondition();
            buyerCondition.TargetValue = search;
            buyerCondition.Operator = StringOperator.BeginsWith;

            EbayOrderSellingManagerRecordCondition orderRecordCondition = new EbayOrderSellingManagerRecordCondition();
            orderRecordCondition.IsNumeric = false;
            orderRecordCondition.StringOperator = StringOperator.BeginsWith;
            orderRecordCondition.StringValue = search;

            OrderItemCodeCondition codeCondition = new OrderItemCodeCondition();
            codeCondition.Operator = StringOperator.BeginsWith;
            codeCondition.TargetValue = search;

            EbayItemSellingManagerRecordCondition recordCondition = new EbayItemSellingManagerRecordCondition();
            recordCondition.IsNumeric = false;
            recordCondition.StringOperator = StringOperator.BeginsWith;
            recordCondition.StringValue = search;

            ForAnyItemCondition anyItemCondition = new ForAnyItemCondition();
            anyItemCondition.Container.FirstGroup.JoinType = ConditionJoinType.Any;
            anyItemCondition.Container.FirstGroup.Conditions.Add(codeCondition);
            anyItemCondition.Container.FirstGroup.Conditions.Add(recordCondition);

            group.JoinType = ConditionJoinType.Any;
            group.Conditions.Add(buyerCondition);
            group.Conditions.Add(orderRecordCondition);
            group.Conditions.Add(anyItemCondition);

            return group;
        }

        /// <summary>
        /// Indicates what basic grid fields we support hyperlinking for
        /// </summary>
        public override bool GridHyperlinkSupported(EntityBase2 entity, EntityField2 field)
        {
            return
                EntityUtility.IsSameField(field, OrderItemFields.Code) ||
                EntityUtility.IsSameField(field, OrderItemFields.Name);
        }

        /// <summary>
        /// Handle a link click for the given field
        /// </summary>
        public override void GridHyperlinkClick(EntityField2 field, EntityBase2 entity, IWin32Window owner)
        {
            WebHelper.OpenUrl(EbayUrlUtilities.GetItemUrl(((EbayOrderItemEntity) entity).EbayItemID), owner);
        }

        /// <summary>
        /// eBay has online status and online last modified
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.LastModified || column == OnlineGridColumnSupport.OnlineStatus)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Create the policy for the ebay initial download
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy
        {
            get
            {
                return new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack) { DefaultDaysBack = 7, MaxDaysBack = 7 };
            }
        }

        /// <summary>
        /// Create dashboard message for updating token if necessary
        /// </summary>
        public override IEnumerable<DashboardStoreItem> CreateDashboardMessages()
        {
            EbayStoreEntity store = (EbayStoreEntity) Store;

            TimeSpan timeToExpire = store.EBayTokenExpire - DateTime.UtcNow;
            int daysToExpire = (int) timeToExpire.TotalDays;

            if (daysToExpire > 30)
            {
                return null;
            }

            string message;

            if (daysToExpire < 0)
            {
                message = "Your eBay login token has expired.";
            }
            else if (daysToExpire == 0)
            {
                message = "Your eBay login token expires today.";
            }
            else
            {
                message = string.Format("Your eBay login token will expire in {0} days.", daysToExpire);
            }

            return new List<DashboardStoreItem> {
                new DashboardStoreItem(store, "EbayToken", Resources.warning16, message,
                    new DashboardActionMethod("[link]Update Now[/link]", OnUpdateToken))
            };
        }

        /// <summary>
        /// Update the auth token
        /// </summary>
        private void OnUpdateToken(Control owner, object userState)
        {
            using (EbayTokenUpdateDlg dlg = new EbayTokenUpdateDlg((EbayStoreEntity) Store))
            {
                dlg.ShowDialog(owner);
            }
        }

        /// <summary>
        /// Generate ShipWorks XML elements for the order
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<EbayOrderEntity>(() => (EbayOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("eBay");
            outline.AddElement("LastModifiedDate", () => order.Value.OnlineLastModified);
            outline.AddElement("BuyerID", () => order.Value.EbayBuyerID);
            outline.AddElement("RecordNumber", () => order.Value.SellingManagerRecord);

            // Selling Manager record (legacy way)
            ElementOutline recordElement = outline.AddElement("SellingManager");
            recordElement.AddAttributeLegacy2x();
            recordElement.AddElement("RecordNumber", () => order.Value.SellingManagerRecord);

            outline.AddElement("EligibleForGSP", () => order.Value.GspEligible);
            outline.AddElement("ShippingMethod", () => EnumHelper.GetDescription((EbayShippingMethod) order.Value.SelectedShippingMethod));

            // Since typical ebay orders will have just a single item, and to ease template migration from v2, include
            // the auction details here like with v2
            var item = new Lazy<EbayOrderItemEntity>(() =>
                {
                    List<EntityBase2> eBayItems = DataProvider.GetRelatedEntities(order.Value.OrderID, EntityType.EbayOrderItemEntity);
                    EbayOrderItemEntity firstItem = eBayItems.OfType<EbayOrderItemEntity>().FirstOrDefault();
                    if (firstItem != null)
                    {
                        firstItem.Order = order.Value;
                    }

                    return firstItem;
                });

            // Generate all the elemnts into a temporary container
            ElementOutline commonContainer = new ElementOutline(outline.Context);
            GenerateTemplateCommonElements(commonContainer, item);

            // Copy to the real outline, with the condition that the item exists
            outline.AddElements(commonContainer, ElementOutline.If(() => item.Value != null));
        }

        /// <summary>
        /// Generate ShipWorks XML elements for the item
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<EbayOrderItemEntity>(() => (EbayOrderItemEntity) itemSource());

            ElementOutline outline = container.AddElement("eBay");
            outline.AddElement("ItemID", () => item.Value.EbayItemID);
            outline.AddElement("TransactionID", () => item.Value.EbayTransactionID);
            outline.AddElement("RecordNumber", () => item.Value.SellingManagerRecord);

            // Selling Manager record (legacy way)
            ElementOutline recordElement = outline.AddElement("SellingManager");
            recordElement.AddAttributeLegacy2x();
            recordElement.AddElement("RecordNumber", () => item.Value.SellingManagerRecord);

            // generate auction-specific xml
            GenerateTemplateCommonElements(outline, item);
        }

        /// <summary>
        /// Generates for a single EbayOrderItemEntity
        /// </summary>
        private static void GenerateTemplateCommonElements(ElementOutline outline, Lazy<EbayOrderItemEntity> item)
        {
            outline.AddElement("PaymentMethod", () => EnumHelper.GetDescription(EbayUtility.GetEffectivePaymentMethod(item.Value)));

            // feedback
            outline.AddElement("FeedbackSellerLeft", () => item.Value.FeedbackLeftType != (int) EbayFeedbackType.None ? "true" : "false");
            outline.AddElement("FeedbackBuyerLeft", () => item.Value.FeedbackReceivedComments.Length > 0 ? "true" : "false");
            outline.AddElement("FeedbackBuyerComments", () => item.Value.FeedbackReceivedComments);
            outline.AddElement("FeedbackBuyerType", () => EbayUtility.GetFeedbackTypeString((CommentTypeCodeType) item.Value.FeedbackReceivedType));

            // checkout status
            outline.AddElement("CheckoutStatus", () => EnumHelper.GetDescription((EbayEffectivePaymentStatus) item.Value.EffectiveCheckoutStatus));
            outline.AddElement("CheckoutComplete", () => EbayUtility.IsCheckoutStatusComplete(item.Value) ? "true" : "false");

            // only write out paypal stuff if we know the paypal transaction id
            ElementOutline paypalElement = outline.AddElement("PayPal", ElementOutline.If(() => item.Value.PayPalTransactionID.Length > 0));
            paypalElement.AddElement("TransactionID", () => item.Value.PayPalTransactionID);
            paypalElement.AddElement("AddressStatus", () => EbayUtility.GetAddressStatusName((AddressStatusCodeType) item.Value.PayPalAddressStatus));
        }

        #region Online Update Commands

        /// <summary>
        /// Create the menu commands for updating the ebay order online
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateCommonCommands()
        {

            List<MenuCommand> commands = new List<MenuCommand>()
            {
                new MenuCommand("Send Message...", OnSendMessage),
                new MenuCommand("Leave Positive Feedback...", OnLeaveFeedback) { BreakAfter = true },

                new MenuCommand("Mark as Paid", OnUpdateShipment){ Tag = EbayOnlineAction.Paid },
                new MenuCommand("Mark as Shipped", OnUpdateShipment) { Tag = EbayOnlineAction.Shipped },

                new MenuCommand("Mark as Not Paid", OnUpdateShipment) { Tag = EbayOnlineAction.NotPaid, BreakBefore = true },
                new MenuCommand("Mark as Not Shipped", OnUpdateShipment) { Tag = EbayOnlineAction.NotShipped },

                new MenuCommand("Combine orders locally...", OnCombineOrders) { BreakBefore = true, Tag = EbayCombinedOrderType.Local },
                new MenuCommand("Combine orders on eBay...", OnCombineOrders) { Tag = EbayCombinedOrderType.Ebay },

                new MenuCommand("Ship to GSP Facility", OnShipToGspFacility) { BreakBefore = true },
                new MenuCommand("Ship to Buyer", OnShipToBuyer)
            };

            return commands;
        }

        /// <summary>
        /// Handler for the Ship to GSP Facility menu command
        /// </summary>
        /// <param name="context">The context.</param>
        private void OnShipToGspFacility(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Updating selected shipping method to Global Shipping Program",
                "ShipWorks is updating the selected shipment method.",
                "Updating order {0} of {1}...");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);

                SendOrderSelectionChangingMessage(context.SelectedKeys);
            };

            executor.ExecuteAsync(ShipToGspFacilityCallback, context.SelectedKeys);
        }

        /// <summary>
        /// Callback for designating an order to be shipped via GSP
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <param name="userState">State of the user.</param>
        /// <param name="issueAdder">The issue adder.</param>
        private void ShipToGspFacilityCallback(long orderId, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            try
            {
                EbayOrderEntity ebayOrder = DataProvider.GetEntity(orderId) as EbayOrderEntity;
                if (ebayOrder == null)
                {
                    throw new EbayException(string.Format("Could not find order {0}. It may have been deleted.", orderId));
                }

                if (ebayOrder.GspEligible)
                {
                    if (ebayOrder.SelectedShippingMethod != (int) EbayShippingMethod.GlobalShippingProgram)
                    {
                        // We have an eBay order that is eligible for the GSP program that needs to have the
                        // shipping method changed
                        ebayOrder.SelectedShippingMethod = (int) EbayShippingMethod.GlobalShippingProgram;
                        using (SqlAdapter adapter = new SqlAdapter())
                        {
                            adapter.SaveAndRefetch(ebayOrder);
                        }
                    }
                }
                else
                {
                    // The order is not eligible for the GSP program (determined by eBay)
                    throw new EbayException(string.Format("Order number {0} is not eligible for the Global Shipping Program", ebayOrder.OrderNumber));
                }
            }
            catch (EbayException ex)
            {
                log.ErrorFormat("Could not change order ID {0} to be shipped to GSP facility: ", orderId, ex.Message);
                issueAdder.Add(orderId, ex);
            }
        }

        /// <summary>
        /// Sends an OrderSelectionChangedMessage so that other panels can update appropriately.
        /// </summary>
        private static void SendOrderSelectionChangingMessage(IEnumerable<long> orderIds)
        {
            OrderSelectionChangingMessage orderSelectionChangingMessage = new OrderSelectionChangingMessage(new object(), orderIds.ToList());
            Messenger.Current.Send(orderSelectionChangingMessage);
        }

        /// <summary>
        /// Handler for the Ship to Buyer menu command
        /// </summary>
        /// <param name="context">The context.</param>
        private void OnShipToBuyer(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Updating selected shipping method to Standard",
                "ShipWorks is updating the selected shipment method.",
                "Updating order {0} of {1}...");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);

                SendOrderSelectionChangingMessage(context.SelectedKeys);
            };

            executor.ExecuteAsync(ShipToBuyerCallback, context.SelectedKeys);
        }

        /// <summary>
        /// Callback for designating an order to be shipped to buyer
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <param name="userState">State of the user.</param>
        /// <param name="issueAdder">The issue adder.</param>
        private void ShipToBuyerCallback(long orderId, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            try
            {
                EbayOrderEntity ebayOrder = DataProvider.GetEntity(orderId) as EbayOrderEntity;
                if (ebayOrder == null)
                {
                    throw new EbayException(string.Format("Could not find order {0}. It may have been deleted.", orderId));
                }

                // Only perform the update if the selected shipping method has not already been overridden to direct to buyer
                if (ebayOrder.SelectedShippingMethod != (int) EbayShippingMethod.DirectToBuyerOverridden)
                {
                    ebayOrder.SelectedShippingMethod = (int) EbayShippingMethod.DirectToBuyerOverridden;
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        adapter.SaveAndRefetch(ebayOrder);
                    }
                }
            }
            catch (EbayException ex)
            {
                log.ErrorFormat("Error designating order {0} to be shipped to buyer: ", orderId, ex.Message);
                issueAdder.Add(orderId, ex);
            }
        }

        /// <summary>
        /// MenuCommand handler for allowing the user to combine eBay orders
        /// </summary>
        private void OnCombineOrders(MenuCommandExecutionContext context)
        {
            EbayPotentialCombinedOrderFinder finder = new EbayPotentialCombinedOrderFinder(context.Owner);
            finder.SearchComplete += new EbayPotentialCombinedOrdersFoundEventHandler(OnPotentialCombinedOrdersFound);

            List<long> orderIDs = context.SelectedKeys.ToList();

            try
            {
                finder.SearchAsync(orderIDs, context, (EbayCombinedOrderType) context.MenuCommand.Tag);
            }
            catch (EbayException ex)
            {
                context.Complete(MenuCommandResult.Error, ex.Message);
            }
        }

        /// <summary>
        /// Loading of possible combined payments is complete
        /// </summary>
        void OnPotentialCombinedOrdersFound(object sender, EbayPotentialCombinedOrdersFoundEventArgs e)
        {
            // unpack the user state
            MenuCommandExecutionContext context = (MenuCommandExecutionContext) e.UserState;

            // handle completing the menu command
            if (e.Error != null)
            {
                context.Complete(MenuCommandResult.Error, e.Error.Message);
                return;
            }
            else if (e.Cancelled)
            {
                context.Complete();
                return;
            }
            else if (e.Candidates.Count == 0)
            {
                context.Complete(MenuCommandResult.Warning, string.Format("None of the selected orders are able to be combined {0}.", e.CombinedOrderType == EbayCombinedOrderType.Local ? "locally" : "on eBay"));
                return;
            }

            // continue to allow selection of which order(s) to combine
            using (EbayCombineOrdersDlg dlg = new EbayCombineOrdersDlg(e.CombinedOrderType, e.Candidates))
            {
                if (dlg.ShowDialog(e.Owner) != DialogResult.OK)
                {
                    context.Complete();
                    return;
                }

                List<OrderCombining.EbayCombinedOrderCandidate> selectedOrders = dlg.SelectedOrders;

                // create another background worker for combining
                BackgroundExecutor<OrderCombining.EbayCombinedOrderCandidate> executor = new BackgroundExecutor<OrderCombining.EbayCombinedOrderCandidate>(e.Owner,
                    "Combining eBay Orders",
                    "ShipWorks is combining eBay Orders.",
                    "Combining Order {0} of {1}...");

                executor.ExecuteCompleted += (o, ea) =>
                {
                    context.Complete(ea.Issues, MenuCommandResult.Error);
                };

                // perform the order combining
                executor.ExecuteAsync(CombineOrdersCallback, selectedOrders);
            }
        }

        /// <summary>
        /// Worker method for combining eBay orders
        /// </summary>
        private void CombineOrdersCallback(OrderCombining.EbayCombinedOrderCandidate toCombine, object userState, BackgroundIssueAdder<OrderCombining.EbayCombinedOrderCandidate> issueAdder)
        {
            try
            {
                toCombine.Combine();
            }
            catch (EbayException ex)
            {
                // log it
                log.ErrorFormat("Error creating combined order: {0}", ex.Message);

                // add the error to the issues so we can react later
                issueAdder.Add(toCombine, ex);
            }
        }

        /// <summary>
        /// MenuCommand handler for sending messages to the eBay buyer
        /// </summary>
        private void OnSendMessage(MenuCommandExecutionContext context)
        {
            List<long> selectedIds = context.SelectedKeys.ToList();

            using (EbayMessagingDlg dlg = new EbayMessagingDlg(selectedIds))
            {
                if (dlg.ShowDialog(context.Owner) != DialogResult.OK)
                {
                    context.Complete();
                    return;
                }

                // kick off the message sending
                Dictionary<string, object> state = new Dictionary<string, object>();
                state.Add("MessageType", dlg.MessageType);
                state.Add("Message", dlg.Message);
                state.Add("Subject", dlg.Subject);
                state.Add("CopyMe", dlg.CopyMe);

                // if the user selected to send a message relating to a Single item, we only use that key
                if (dlg.SelectedItemID > 0)
                {
                    selectedIds.Clear();
                    selectedIds.Add(dlg.SelectedItemID);
                }

                BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                    "Sending Messages",
                    "ShipWorks is sending eBay Messages.",
                    "Sending message for {0} of {1}...");

                executor.ExecuteCompleted += (o, e) =>
                    {
                        context.Complete(e.Issues, MenuCommandResult.Error);
                    };

                // execute, passing the state which tells what message information to send
                executor.ExecuteAsync(SendMessageCallback, selectedIds, state);
            }
        }

        /// <summary>
        /// Worker method for sending eBay messages.
        /// </summary>
        private void SendMessageCallback(long entityId, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            // get the store instance
            EbayStoreEntity ebayStore = StoreManager.GetRelatedStore(entityId) as EbayStoreEntity;

            // unpack the user state
            Dictionary<string, object> state = userState as Dictionary<string, object>;

            EbaySendMessageType messageType = (EbaySendMessageType) state["MessageType"];
            string subject = (string) state["Subject"];
            string message = (string) state["Message"];
            bool copyMe = (bool) state["CopyMe"];

            // Perform token processing on the message to be sent
            string processedSubject = TemplateTokenProcessor.ProcessTokens(subject, entityId);
            string processedMessage = TemplateTokenProcessor.ProcessTokens(message, entityId);

            try
            {
                EbayOnlineUpdater updater = new EbayOnlineUpdater(ebayStore);
                updater.SendMessage(entityId, messageType, processedSubject, processedMessage, copyMe);
            }
            catch (EbayException ex)
            {
                // log it
                log.ErrorFormat("Error sending eBay message for entityID {0}: {1}", entityId, ex.Message);

                // add the error to the issues so we can react later
                issueAdder.Add(entityId, ex);
            }
        }

        /// <summary>
        /// MenuCommand handler for leaving ebay feedback
        /// </summary>
        private void OnLeaveFeedback(MenuCommandExecutionContext context)
        {
            // get the list of orderIds selected
            List<long> selectedIds = context.SelectedKeys.ToList();

            using (LeaveFeedbackDlg dlg = new LeaveFeedbackDlg(selectedIds))
            {
                if (dlg.ShowDialog(context.Owner) != DialogResult.OK)
                {
                    context.Complete();
                    return;
                }

                string comments = dlg.Comments;
                CommentTypeCodeType feedbackType = dlg.FeedbackType;

                // if the user selected to leave feedback for a single item, use it only
                if (dlg.SelectedItemID > 0)
                {
                    selectedIds.Clear();
                    selectedIds.Add(dlg.SelectedItemID);
                }

                // execute
                BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                    "Leaving Feedback",
                    "ShipWorks is leaving eBay Feedback.",
                    "Leaving feedback for order {0} of {1}...");

                executor.ExecuteCompleted += (o, e) =>
                    {
                        context.Complete(e.Issues, MenuCommandResult.Error);
                    };

                // execute, passing the menu command state which tells how to perform the update
                Dictionary<string, object> state = new Dictionary<string, object>();
                state["feedback"] = comments;
                state["feedbackType"] = feedbackType;

                executor.ExecuteAsync(LeaveFeedbackCallback, selectedIds, state);
            }
        }

        /// <summary>
        /// Worker method for leaving feedback.
        /// </summary>
        private void LeaveFeedbackCallback(long entityId, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            EbayStoreEntity ebayStore = StoreManager.GetRelatedStore(entityId) as EbayStoreEntity;

            // unpack the user state
            Dictionary<string, object> state = userState as Dictionary<string, object>;

            try
            {
                EbayOnlineUpdater updater = new EbayOnlineUpdater(ebayStore);
                updater.LeaveFeedback(entityId, (CommentTypeCodeType) state["feedbackType"], (string) state["feedback"]);
            }
            catch (EbayException ex)
            {
                // log it
                log.ErrorFormat("Error updating online status for entity Id {0}: {1}", entityId, ex.Message);

                // add the error to issues so we can react later
                issueAdder.Add(entityId, ex);
            }
        }

        /// <summary>
        /// MenuCommand handler for marking an order as paid/shipped
        /// </summary>
        private void OnUpdateShipment(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Updating Shipment Status",
                "ShipWorks is updating shipment status.",
                "Updating order {0} of {1}...");

            executor.ExecuteCompleted += (o, e) =>
                {
                    context.Complete(e.Issues, MenuCommandResult.Error);
                };

            // execute, passing the menu command state which tells how to perform the update
            executor.ExecuteAsync(UpdateStatusCallback, context.SelectedKeys, context.MenuCommand.Tag);
        }

        /// <summary>
        /// Worker thread method for updated order status
        /// </summary>
        private void UpdateStatusCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            // need to get the store instance for this order
            EbayStoreEntity ebayStore = StoreManager.GetRelatedStore(orderID) as EbayStoreEntity;

            // unpack the user state
            EbayOnlineAction action = (EbayOnlineAction) userState;

            bool? paid = null;
            if (action == EbayOnlineAction.Paid || action == EbayOnlineAction.NotPaid)
            {
                paid = action == EbayOnlineAction.Paid;
            }

            bool? shipped = null;
            if (action == EbayOnlineAction.Shipped || action == EbayOnlineAction.NotShipped)
            {
                shipped = action == EbayOnlineAction.Shipped;
            }

            try
            {
                EbayOnlineUpdater updater = new EbayOnlineUpdater(ebayStore);
                updater.UpdateOnlineStatus(orderID, paid, shipped);
            }
            catch (EbayException ex)
            {
                // log it
                log.ErrorFormat("Error updating online status for orderID {0}: {1}", orderID, ex.Message);

                // add the error to issues so we can react later
                issueAdder.Add(orderID, ex);
            }
        }

        #endregion

        /// <summary>
        /// Intended to be called during the processing of a shipment to allow the store to the
        /// chance to override the shipment details for any functionality that may be specific
        /// to a store's shipment processing scenario(s). This allows us to inspect the shipment
        /// to determine if it's a GSP shipment and update the shipping details accordingly.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A list of ShipmentFieldIndex objects indicating to the caller which fields
        /// of the shipment were changed.</returns>
        public override List<ShipmentFieldIndex> OverrideShipmentDetails(ShipmentEntity shipment)
        {
            // Overridden so we can change the shipping address if this is a Global Shipping Program order
            List<ShipmentFieldIndex> modifiedFieldList = new List<ShipmentFieldIndex>();

            if (shipment != null)
            {
                // Fetch the eBay order details so we have the latest GSP related data about the order
                EbayOrderEntity ebayOrder = DataProvider.GetEntity(shipment.OrderID) as EbayOrderEntity;

                // We're going to use the the GSP policy to inspect the order and configure the shipment
                // to be shipped to the GSP domestic facility if it's eligible
                Ebay.Shipping.GlobalShippingProgram.Policy policy = new Shipping.GlobalShippingProgram.Policy();

                if (policy.IsEligibleForGlobalShippingProgram(ebayOrder))
                {
                    // The order is an GSP order, so we'll need to change the address on the
                    // shipment to go to the GSP facility address on the eBay order
                    modifiedFieldList = policy.ConfigureShipmentForGlobalShippingProgram(shipment, ebayOrder);
                }
            }

            return modifiedFieldList;
        }

        /// <summary>
        /// Determines whether the shipping address is editable for the specified shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public override ShippingAddressEditStateType ShippingAddressEditableState(OrderEntity order, ShipmentEntity shipment)
        {
            ShippingAddressEditStateType editable = base.ShippingAddressEditableState(order, shipment);

            if (editable == ShippingAddressEditStateType.Editable)
            {
                // Grab the order details and check with the GSP policy to see if
                // the order is currently flagged as a GSP order
                EbayOrderEntity ebayOrder = order as EbayOrderEntity;
                Shipping.GlobalShippingProgram.Policy gspPolicy = new Shipping.GlobalShippingProgram.Policy();

                // Global Shipping Program shipments should not have an editable shipping address
                //isEditable = !gspPolicy.IsEligibleForGlobalShippingProgram(ebayOrder);
                if (gspPolicy.IsEligibleForGlobalShippingProgram(ebayOrder))
                {
                    editable = ShippingAddressEditStateType.GspFulfilled;
                }
            }

            return editable;
        }

        /// <summary>
        /// Determines whether customs is required for the specified shipment. A pre-determined recommendation
        /// whether to require customs is provided to provide the store context for any precursory checks that
        /// have already occurred.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="customsRequiredRecommendation">The recommendation to the store as to whether customs is required.</param>
        /// <returns>
        ///   <c>true</c> if [is customs required] [the specified shipment]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsCustomsRequired(ShipmentEntity shipment, bool customsRequiredRecommendation)
        {
            bool requiresCustoms = customsRequiredRecommendation;

            if (requiresCustoms)
            {
                // Inspect a shipment that has been recommended for requiring customs to see if
                // it is being shipped via eBay's Global Shipping Program - these orders do not
                // require customs since they are going to a domestic facility
                EbayOrderEntity ebayOrder = DataProvider.GetEntity(shipment.OrderID) as EbayOrderEntity;
                if (ebayOrder != null)
                {
                    // Check with the GSP policy to see if this order meets the criteria
                    Shipping.GlobalShippingProgram.Policy gspPolicy = new Shipping.GlobalShippingProgram.Policy();
                    if (gspPolicy.IsEligibleForGlobalShippingProgram(ebayOrder))
                    {
                        // This is a GSP order, so customs is not required
                        requiresCustoms = false;
                    }
                }
            }

            return requiresCustoms;
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
