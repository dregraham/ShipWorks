using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.ApplicationCore.Dashboard;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Editions.Freemium;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.OrderItems;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Properties;
using ShipWorks.Shipping;
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
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Entry point for the eBay integration.
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Ebay)]
    [Component(RegistrationType.Self)]
    public class EbayStoreType : StoreType
    {
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

        private readonly IConfigurationData configuration;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayStoreType(StoreEntity store, IConfigurationData configuration, IMessageHelper messageHelper)
            : base(store)
        {
            this.messageHelper = messageHelper;
            this.configuration = configuration;
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
        /// Creates the account settings control displayed in the Manage Stores window.
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return new EbayStoreSettingsControl();
        }

        /// <summary>
        /// Creates the account settings control displayed in the Manage
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

            // People end up thinking they have to download PayPal details and images, when really it just takes forever.  Don't show these options by default
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

                SellingManagerRecord = 0
            };
        }

        /// <summary>
        /// Creates the identifier to locate a particular order
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

            // Generate all the elements into a temporary container
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

            // only write out PayPal stuff if we know the PayPal transaction id
            ElementOutline paypalElement = outline.AddElement("PayPal", ElementOutline.If(() => item.Value.PayPalTransactionID.Length > 0));
            paypalElement.AddElement("TransactionID", () => item.Value.PayPalTransactionID);
            paypalElement.AddElement("AddressStatus", () => EbayUtility.GetAddressStatusName((AddressStatusCodeType) item.Value.PayPalAddressStatus));
        }

        /// <summary>
        /// Will calling OverrideShipmentDetails change the specified shipment
        /// </summary>
        public override bool WillOverrideShipmentDetailsChangeShipment(IShipmentEntity shipment)
        {
            if (shipment == null)
            {
                return false;
            }

            // Fetch the eBay order details so we have the latest GSP related data about the order
            IEbayOrderEntity ebayOrder = shipment.Order as IEbayOrderEntity ??
                DataProvider.GetEntity(shipment.OrderID) as IEbayOrderEntity;

            // We're going to use the GSP policy to inspect the order and configure the shipment
            // to be shipped to the GSP domestic facility if it's eligible
            Shipping.GlobalShippingProgram.Policy policy = new Shipping.GlobalShippingProgram.Policy();

            return policy.IsEligibleForGlobalShippingProgram(ebayOrder);
        }

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
            if (shipment != null)
            {
                // Fetch the eBay order details so we have the latest GSP related data about the order
                IEbayOrderEntity ebayOrder = shipment.Order as IEbayOrderEntity ??
                    DataProvider.GetEntity(shipment.OrderID) as IEbayOrderEntity;

                // We're going to use the GSP policy to inspect the order and configure the shipment
                // to be shipped to the GSP domestic facility if it's eligible
                Shipping.GlobalShippingProgram.Policy policy = new Shipping.GlobalShippingProgram.Policy();

                if (policy.IsEligibleForGlobalShippingProgram(ebayOrder))
                {
                    // The order is an GSP order, so we'll need to change the address on the
                    // shipment to go to the GSP facility address on the eBay order
                    return policy.ConfigureShipmentForGlobalShippingProgram(shipment, ebayOrder);
                }
            }

            return new List<ShipmentFieldIndex>();
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
