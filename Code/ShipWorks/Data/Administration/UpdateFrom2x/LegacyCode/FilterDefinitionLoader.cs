using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters;
using System.Xml.Linq;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Filters.Content.Conditions.Customers;
using ShipWorks.Filters.Content.Conditions.Customers.Address;
using System.Diagnostics;
using ShipWorks.Filters.Content.Conditions.Special;
using ShipWorks.Filters.Content.Conditions.Customers.PersonName;
using ShipWorks.Filters.Content.Conditions.OrderItems;
using log4net;
using ShipWorks.Filters.Content.Conditions.Orders.Address;
using ShipWorks.Filters.Content.Conditions.OrderCharges;
using ShipWorks.Filters.Content.Conditions.Orders.PersonName;
using ShipWorks.Filters.Content.Conditions.Notes;
using ShipWorks.Filters.Content.Conditions.PaymentDetails;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.ProStores.CoreExtensions.Filters;
using ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Filters;
using ShipWorks.Stores.Platforms.OrderMotion.CoreExtensions.Filters;
using ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Filters;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Stores;
using System.Data.SqlClient;
using Interapptive.Shared;
using Interapptive.Shared.Data;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.Specialized.Utility;
using ShipWorks.Stores.Platforms.PayPal.CoreExtensions.Filters;
using ShipWorks.Stores.Platforms.PayPal;
using ShipWorks.Filters.Content.Conditions.Emails;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration;

namespace ShipWorks.Data.Administration.UpdateFrom2x.LegacyCode
{
    /// <summary>
    /// Responsible for loading a v2 filter XML into a v3 FilterDefinition
    /// </summary>
    [NDependIgnoreLongTypes]
    public static class FilterDefinitionLoader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FilterDefinitionLoader));

        /// <summary>
        /// Class to provide loading\parsing directly of status code XML
        /// </summary>
        class LocalXmlStatusCodeProvider : StatusCodeProvider<string>
        {
            string xml;

            public LocalXmlStatusCodeProvider(string xml)
            {
                this.xml = xml;

            }
            protected override string GetLocalStatusCodesXml()
            {
                return xml;
            }
        }

        // Maps StoreTypes to their status codes
        static Dictionary<StoreTypeCode, LocalXmlStatusCodeProvider> storeTypeStatusCodesMap = null;

        /// <summary>
        /// Initialize and retrieve any required data from the given connection
        /// </summary>
        public static void Initialize(SqlConnection con)
        {
            storeTypeStatusCodesMap = new Dictionary<StoreTypeCode, LocalXmlStatusCodeProvider>();

            using (SqlCommand cmd = SqlCommandProvider.Create(con,
                @"SELECT s.TypeCode, g.ModuleStatusCodes FROM Store s INNER JOIN GenericModuleStore g ON s.StoreID = g.StoreID
                    UNION ALL
                  SELECT s.TypeCode, g.StatusCodes FROM Store s INNER JOIN AmeriCommerceStore g ON s.StoreID = g.StoreID
                    UNION ALL
                  SELECT s.TypeCode, g.StatusCodes FROM Store s INNER JOIN NetworkSolutionsStore g ON s.StoreID = g.StoreID"))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int typeCode = (int) reader[0];

                        // All these store types use the "osCommerceStatusCondition", so when reading those conditions thats the type code they'd get looked up under
                        switch ((StoreTypeCode) typeCode)
                        {
                            case StoreTypeCode.CreLoaded:
                            case StoreTypeCode.ZenCart:
                            case StoreTypeCode.VirtueMart:
                            case StoreTypeCode.OrderDynamics:
                                typeCode = (int) StoreTypeCode.osCommerce;
                                break;
                        }

                        storeTypeStatusCodesMap[(StoreTypeCode) typeCode] = new LocalXmlStatusCodeProvider((string) reader[1]);
                    }
                }
            }
        }

        /// <summary>
        /// Create a V3 FilterDefinition based on the v2 definition XML format
        /// </summary>
        public static FilterDefinition CreateDefinition(string xml)
        {
            XElement v2Xml = XElement.Parse(xml);

            FilterTarget target = ((string) v2Xml.Attribute("Target") == "Customers") ? FilterTarget.Customers : FilterTarget.Orders;
            FilterDefinition definition = new FilterDefinition(target);

            // Get the root GroupSet
            XElement groupSet = v2Xml.Element("Set");
            definition.RootContainer = LoadGroupSet(groupSet, target);

            return definition;
        }

        /// <summary>
        /// Parse the V2 "Set" into a V3 ConditionGroupContainer
        /// </summary>
        private static ConditionGroupContainer LoadGroupSet(XElement v2groupSet, FilterTarget target)
        {
            ConditionGroupContainer container = new ConditionGroupContainer();

            // JoinType
            container.JoinType = ((string) v2groupSet.Attribute("Operator") == "Or") ? ConditionGroupJoinType.Or : ConditionGroupJoinType.And;

            // Required FirstGroup
            container.FirstGroup = LoadGroup(v2groupSet.Element("Group"), target);

            // Optional second group
            XElement v2childGroupSet = v2groupSet.Element("Set");
            if (v2childGroupSet != null)
            {
                container.SecondGroup = LoadGroupSet(v2childGroupSet, target);
            }

            return container;
        }

        /// <summary>
        /// Parse the V2 group into a V3 ConditionGroup
        /// </summary>
        private static ConditionGroup LoadGroup(XElement v2group, FilterTarget target)
        {
            ConditionGroup group = new ConditionGroup();

            // JoinType
            group.JoinType = DetermineGroupJoinType((string) v2group.Attribute("Type"));

            // Conditions
            foreach (XElement v2Condition in v2group.Elements("Condition"))
            {
                Condition condition = LoadCondition(v2Condition, target);

                // Some conditions just aren't mappable to 3 and arent needed, such as the WasArchived condition
                if (condition != null)
                {
                    group.Conditions.Add(condition);
                }
            }

            return group;
        }

        /// <summary>
        /// Determine the v3 JoinType based on the given V2 type string
        /// </summary>
        private static ConditionJoinType DetermineGroupJoinType(string type)
        {
            if (type == "All")
            {
                return ConditionJoinType.All;
            }

            if (type == "None")
            {
                return ConditionJoinType.None;
            }

            return ConditionJoinType.Any;
        }

        /// <summary>
        /// Parse the given v2Condition and create its 3x counterpart
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        private static Condition LoadCondition(XElement v2Condition, FilterTarget target)
        {
            string conditionType = (string) v2Condition.Element("Type");

            switch (conditionType)
            {
                case "ArchivedCondition": return new NotSupportedV2Condition { Detail = "'Restored from Archive' is not supported in ShipWorks 3." };

                case "CustomerCityCondition": return WrapConditionForTarget(ReadBillShipAddressCondition(v2Condition, new CustomerAddressCityCondition()), ConditionEntityTarget.Customer, target);
                case "CustomerCompanyCondition": return WrapConditionForTarget(ReadBillShipAddressCondition(v2Condition, new CustomerCompanyNameCondition()), ConditionEntityTarget.Customer, target);
                case "CustomerCountryCondition": return WrapConditionForTarget(ReadBillShipAddressCondition(v2Condition, new CustomerAddressCountryCondition()), ConditionEntityTarget.Customer, target);
                case "CustomerNameCondition": return CreateCustomerNameCondition(v2Condition, target);
                case "CustomerStateCondition": return WrapConditionForTarget(ReadBillShipAddressCondition(v2Condition, new CustomerAddressStateProvinceCondition()), ConditionEntityTarget.Customer, target);
                case "CustomerZipCondition": return WrapConditionForTarget(ReadBillShipAddressCondition(v2Condition, new CustomerAddressPostalCodeCondition()), ConditionEntityTarget.Customer, target);
                case "CustomerNotesCondition": return CreateNotesCondition(v2Condition, target);
                case "OrderCountCondition": return CreateOrderCountCondition(v2Condition, target);

                case "OrderTotalCondition": return CreateOrderTotalCondition(v2Condition, target);
                case "OrderDateCondition": return WrapConditionForTarget(ReadDateCondition(v2Condition, new OrderDateCondition()), ConditionEntityTarget.Order, target);
                case "OrderStatusCondition": return CreateLocalStatusCondition(v2Condition, target);
                case "OrderAddressCondition": return CreateOrderAddressCondition(v2Condition, target);
                case "OrderCityCondition": return WrapConditionForTarget(ReadBillShipAddressCondition(v2Condition, new OrderAddressCityCondition()), ConditionEntityTarget.Order, target);
                case "OrderCompanyCondition": return WrapConditionForTarget(ReadBillShipAddressCondition(v2Condition, new OrderCompanyNameCondition()), ConditionEntityTarget.Order, target);
                case "OrderCountryCondition": return WrapConditionForTarget(ReadBillShipAddressCondition(v2Condition, new OrderAddressCountryCondition()), ConditionEntityTarget.Order, target);
                case "OrderStateCondition": return WrapConditionForTarget(ReadBillShipAddressCondition(v2Condition, new OrderAddressStateProvinceCondition()), ConditionEntityTarget.Order, target);
                case "OrderZipCondition": return WrapConditionForTarget(ReadBillShipAddressCondition(v2Condition, new OrderAddressPostalCodeCondition()), ConditionEntityTarget.Order, target);
                case "OrderNameCondition": return CreateOrderNameCondition(v2Condition, target);
                case "OrderNotesCondition": return CreateNotesCondition(v2Condition, target);
                case "OrderTypeCondition": return CreateOrderTypeCondition(v2Condition, target);
                case "OrderWeightCondition": return WrapConditionForTarget(ReadValueCondition(v2Condition, new TotalWeightCondition()), ConditionEntityTarget.Order, target);
                case "ShippingMethodCondition": return WrapConditionForTarget(ReadStringCondition(v2Condition, new RequestedShippingCondition()), ConditionEntityTarget.Order, target);
                case "EmailedWithCondition": return CreateEmailedWithCondition(v2Condition, target);

                case "ItemCodeCondition": return WrapConditionForTarget(ReadStringCondition(v2Condition, new OrderItemCodeCondition()), ConditionEntityTarget.OrderItem, target);
                case "ItemLocationCondition": return WrapConditionForTarget(ReadStringCondition(v2Condition, new OrderItemLocationCondition()), ConditionEntityTarget.OrderItem, target);
                case "ItemNameCondition": return WrapConditionForTarget(ReadStringCondition(v2Condition, new OrderItemNameCondition()), ConditionEntityTarget.OrderItem, target);
                case "ItemSKUCondition": return WrapConditionForTarget(ReadStringCondition(v2Condition, new OrderItemSkuCondition()), ConditionEntityTarget.OrderItem, target);
                case "ItemStatusCondition": return WrapConditionForTarget(ReadStringCondition(v2Condition, new ShipWorks.Filters.Content.Conditions.OrderItems.LocalStatusCondition()), ConditionEntityTarget.OrderItem, target);
                case "ItemsOrderedCondition": return CreateItemsOrderedCondition(v2Condition, target);

                case "OrderChargeCondition": return CreateOrderChargeCondition(v2Condition, target);
                case "PaymentDetailCondition": return CreatePaymentDetailCondition(v2Condition, target);

                case "ShipmentProcessedDateCondition": return WrapShipmentConditionForTarget(WrapConditionForTarget(ReadDateCondition(v2Condition, new ProcessedDateCondition()), ConditionEntityTarget.Shipment, FilterTarget.Orders), target);
                case "ShipmentShippedDateCondition": return WrapShipmentConditionForTarget(CreateShippedDateCondition(v2Condition), target);
                case "ShipmentCondition": return WrapShipmentConditionForTarget(CreateShipmentStatusCondition(v2Condition), target);

                case "ProStoresStatusCondition": return WrapConditionForTarget(ReadStringCondition(v2Condition, new OnlineStatusCondition()), ConditionEntityTarget.Order, target);
                case "ProStoresAuthorizationCondition": return CreateProStoresAuthorizationCondition(v2Condition, target);

                case "ChannelAdvisorMarketplaceCondition": return WrapConditionForTarget(ReadStringCondition(v2Condition, new ChannelAdvisorMarketplaceNameCondition()), ConditionEntityTarget.OrderItem, target);
                case "ChannelAdvisorDistributionCondition": return WrapConditionForTarget(ReadStringCondition(v2Condition, new ChannelAdvisorDistributionCenterCondition()), ConditionEntityTarget.OrderItem, target);

                case "OrderMotionPromotionCondition": return WrapConditionForTarget(ReadStringCondition(v2Condition, new OrderMotionOrderPromotionCondition()), ConditionEntityTarget.Order, target);

                case "eBayBuyerCondition": return WrapConditionForTarget(ReadStringCondition(v2Condition, new EbayBuyerIDCondition()), ConditionEntityTarget.Order, target);
                case "eBayItemTitleCondition": return WrapConditionForTarget(ReadStringCondition(v2Condition, new OrderItemNameCondition()), ConditionEntityTarget.OrderItem, target);
                case "eBayFeedbackCondition": return CreateEbayFeedbackCondition(v2Condition, target);
                case "eBayPayPalAddressStatusCondition": return CreateEbayPaypalAddressStatusCondition(v2Condition, target);
                case "eBaySoldDateCondition": return WrapConditionForTarget(ReadDateCondition(v2Condition, new OrderDateCondition()), ConditionEntityTarget.Order, target);
                case "eBayPaymentMethodCondition": return CreateEbayPaymentMethodCondition(v2Condition, target);
                case "eBayCheckoutStatusCondition": return CreateEbayCheckoutStatusCondition(v2Condition, target);
                case "eBayLastModifiedCondition": return WrapConditionForTarget(ReadDateCondition(v2Condition, new OnlineLastModifiedCondition()), ConditionEntityTarget.Order, target);

                case "PayPalPaymentStatusCondition": return CreatePayPalPaymentStatusCondition(v2Condition, target);

                case "osCommerceStatusCondition": return WrapConditionForTarget(ReadOnlineStatusCondition(v2Condition, StoreTypeCode.osCommerce), ConditionEntityTarget.Order, target);
                case "XCartStatusCondition": return WrapConditionForTarget(ReadOnlineStatusCondition(v2Condition, StoreTypeCode.XCart), ConditionEntityTarget.Order, target);
                case "InfopiaStatusCondition": return CreateInfopiaOnlineStatusCondition(v2Condition, target);
                case "MivaStatusCondition": return WrapConditionForTarget(ReadOnlineStatusCondition(v2Condition, StoreTypeCode.Miva), ConditionEntityTarget.Order, target);
                case "VolusionStatusCondition": return CreateVolusionOnlineStatusCondition(v2Condition, target);
                case "AmeriCommerceStatusCondition": return WrapConditionForTarget(ReadOnlineStatusCondition(v2Condition, StoreTypeCode.AmeriCommerce), ConditionEntityTarget.Order, target);
                case "NetworkSolutionsStatusCondition": return WrapConditionForTarget(ReadOnlineStatusCondition(v2Condition, StoreTypeCode.NetworkSolutions), ConditionEntityTarget.Order, target);
                case "MagentoStatusCondition": return WrapConditionForTarget(ReadOnlineStatusCondition(v2Condition, StoreTypeCode.Magento), ConditionEntityTarget.Order, target);
                case "ClickCartProStatusCondition": return WrapConditionForTarget(ReadOnlineStatusCondition(v2Condition, StoreTypeCode.ClickCartPro), ConditionEntityTarget.Order, target);
                case "SearchFitStatusCondition": return WrapConditionForTarget(ReadOnlineStatusCondition(v2Condition, StoreTypeCode.SearchFit), ConditionEntityTarget.Order, target);
            }

            // TODO: Replace with MigrationException when all are (thought to be) done
            return null;
        }

        /// <summary>
        /// Create a "Local Status" condition
        /// </summary>
        private static Condition CreateLocalStatusCondition(XElement v2Condition, FilterTarget target)
        {
            // Read in the condition properties
            var condition = new ShipWorks.Filters.Content.Conditions.Orders.LocalStatusCondition();
            ReadStringCondition(v2Condition, condition);

            // What can happen is that in 2x the user enters a filter condition text that is a case-insensitive equivalent of a builtin status string.  The filter
            // would work fine that way, but when they open the editor and close it (if they did) then it would "fix" it automatically, which would trigger the filter
            // to look dirty even if they didnt change anythign.
            string realCasing = StatusPresetManager.GetAllPresets(StatusPresetTarget.Order).FirstOrDefault(p => string.Compare(p, condition.TargetValue, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (realCasing != null)
            {
                condition.TargetValue = realCasing;
            }

            return WrapConditionForTarget(condition, ConditionEntityTarget.Order, target);
        }

        /// <summary>
        /// Create a replica of a v2 "Emailed With" condition
        /// </summary>
        [NDependIgnoreLongMethod]
        private static Condition CreateEmailedWithCondition(XElement v2Condition, FilterTarget target)
        {
            StringOperator stringOperator = ParseV2StringOperator((string) v2Condition.Element("Operator"));
            string templateNamePart = (string) v2Condition.Element("TargetValue");

            List<long> templates = DetermineEmailedWithTemplates(stringOperator, templateNamePart);
            if (templates.Count == 0)
            {
                return new NotSupportedV2Condition() { Detail = string.Format("Emailed With: Upgrader could not find template with name '{0}'.", templateNamePart) };
            }
            else
            {
                Condition condition;
                ConditionGroup group;

                if (target == FilterTarget.Orders)
                {
                    ForAnyEmailCondition anyEmail = new ForAnyEmailCondition();

                    condition = anyEmail;
                    group = anyEmail.Container.FirstGroup;
                }
                else
                {
                    ForAnyEmailedCondition anyEmail = new ForAnyEmailedCondition();

                    condition = anyEmail;
                    group = anyEmail.Container.FirstGroup;
                }

                EmailStatusCondition statusCondition = new EmailStatusCondition();
                statusCondition.Operator = EqualityOperator.Equals;
                statusCondition.Value = Email.EmailOutboundStatus.Sent;
                group.Conditions.Add(statusCondition);

                if (templates.Count == 1 && (stringOperator != StringOperator.NotEqual && stringOperator != StringOperator.NotContains))
                {
                    EmailTemplateCondition templateCondition = new EmailTemplateCondition();
                    templateCondition.TemplateID = templates[0];
                    group.Conditions.Add(templateCondition);
                }
                else
                {
                    CombinedResultCondition templateGroup = new CombinedResultCondition();
                    templateGroup.Container.FirstGroup.JoinType = ConditionJoinType.Any;

                    if (stringOperator == StringOperator.NotContains || stringOperator == StringOperator.NotEqual)
                    {
                        templateGroup.Container.FirstGroup.JoinType = ConditionJoinType.None;
                    }

                    foreach (long templateID in templates)
                    {
                        EmailTemplateCondition templateCondition = new EmailTemplateCondition();
                        templateCondition.TemplateID = templateID;
                        templateGroup.Container.FirstGroup.Conditions.Add(templateCondition);
                    }

                    group.Conditions.Add(templateGroup);
                }

                return condition;
            }
        }

        /// <summary>
        /// Determine the template that the user is probably talking about with the given filter on a template
        /// </summary>
        private static List<long> DetermineEmailedWithTemplates(StringOperator stringOperator, string templateNamePart)
        {
            IEnumerable<TemplateEntity> matchingTemplates = new List<TemplateEntity>();
            var v2Templates = TemplateManager.Tree.AllTemplates.Where(t => t.FullName.StartsWith(ImportTemplatesWizardPage.ImportRootFolderName));

            switch (stringOperator)
            {
                case StringOperator.BeginsWith:
                    matchingTemplates = v2Templates.Where(t => t.FullName.StartsWith(ImportTemplatesWizardPage.ImportRootFolderName + @"\" + templateNamePart, StringComparison.InvariantCultureIgnoreCase));
                    break;

                case StringOperator.Contains:
                case StringOperator.NotContains:
                case StringOperator.Matches:
                    matchingTemplates = v2Templates.Where(t => t.FullName.Contains(templateNamePart));
                    break;

                case StringOperator.EndsWith:
                    matchingTemplates = v2Templates.Where(t => t.FullName.EndsWith(templateNamePart, StringComparison.InvariantCultureIgnoreCase));
                    break;

                case StringOperator.Equals:
                case StringOperator.NotEqual:
                    matchingTemplates = v2Templates.Where(t => string.Compare(t.FullName, ImportTemplatesWizardPage.ImportRootFolderName + @"\" + templateNamePart, StringComparison.InvariantCultureIgnoreCase) == 0);
                    break;
            }

            return matchingTemplates.Select(t => t.TemplateID).ToList();
        }

        /// <summary>
        /// Create condition to match V2 PayPal payment status condition
        /// </summary>
        [NDependIgnoreLongMethod]
        private static Condition CreatePayPalPaymentStatusCondition(XElement v2Condition, FilterTarget target)
        {
            PayPalPaymentStatusCondition condition = new PayPalPaymentStatusCondition();
            condition.Operator = EqualityOperator.Equals;

            int v2Value = (int) v2Condition.Element("TargetValue");
            switch (v2Value)
            {
                case 0: condition.Value = PayPalPaymentStatus.None; break;
                case 1: condition.Value = PayPalPaymentStatus.Completed; break;
                case 2: condition.Value = PayPalPaymentStatus.Failed; break;
                case 3: condition.Value = PayPalPaymentStatus.Pending; break;
                case 4: condition.Value = PayPalPaymentStatus.Denied; break;
                case 5: condition.Value = PayPalPaymentStatus.Refunded; break;
                case 6: condition.Value = PayPalPaymentStatus.Reversed; break;
                case 7: condition.Value = PayPalPaymentStatus.CanceledReversal; break;
                case 8: condition.Value = PayPalPaymentStatus.Processed; break;
                case 9: condition.Value = PayPalPaymentStatus.PartiallyRefunded; break;
                case 10: condition.Value = PayPalPaymentStatus.Voided; break;
                case 11: condition.Value = PayPalPaymentStatus.Expired; break;
                case 12: condition.Value = PayPalPaymentStatus.InProgress; break;
                case 13: condition.Value = PayPalPaymentStatus.UnderReview; break;
                default: throw new InvalidOperationException("Unknown PayPal v2 status code value");
            }

            return WrapConditionForTarget(condition, ConditionEntityTarget.Order, target);
        }

        /// <summary>
        /// Create the Online Status condition to match v2 volusion
        /// </summary>
        [NDependIgnoreLongMethod]
        private static Condition CreateVolusionOnlineStatusCondition(XElement v2Condition, FilterTarget target)
        {
            string code = (string) v2Condition.Element("TargetValue");
            string status;

            switch (code)
            {
                case "0": status = "New"; break;
                case "1": status = "Pending"; break;
                case "2": status = "Processing"; break;
                case "3": status = "Payment Declined"; break;
                case "4": status = "Awaiting Payment"; break;
                case "5": status = "Ready to Ship"; break;
                case "6": status = "Pending Shipment"; break;
                case "7": status = "Partially Shipped"; break;
                case "8": status = "Shipped"; break;
                case "9": status = "Partially Backordered"; break;
                case "10": status = "Backordered"; break;
                case "11": status = "See Line Items"; break;
                case "12": status = "See Order Notes"; break;
                case "13": status = "Partially Returned"; break;
                case "14": status = "Returned"; break;
                default: throw new InvalidOperationException("Unknown Infopia code value: " + code);
            }

            OnlineStatusCondition condition = new OnlineStatusCondition();
            condition.Operator = StringOperator.Equals;
            condition.TargetValue = status;

            return WrapConditionForTarget(condition, ConditionEntityTarget.Order, target);
        }

        /// <summary>
        /// Create the Online Status condition to match v2 infopia
        /// </summary>
        [NDependIgnoreLongMethod]
        private static Condition CreateInfopiaOnlineStatusCondition(XElement v2Condition, FilterTarget target)
        {
            string code = (string) v2Condition.Element("TargetValue");
            string status;

            switch (code)
            {
                case "0": status = "New"; break;
                case "1": status = "Processed"; break;
                case "2": status = "Incomplete"; break;
                case "3": status = "Failed"; break;
                case "4": status = "Manually Processed"; break;
                case "5": status = "Pending"; break;
                case "6": status = "Pending From Incomplete"; break;
                case "7": status = "To Ship"; break;
                case "8": status = "Flagged"; break;
                case "9": status = "Temp"; break;
                case "10": status = "Fraud"; break;
                case "11": status = "Returns"; break;
                default: throw new InvalidOperationException("Unknown Infopia code value: " + code);
            }

            OnlineStatusCondition condition = new OnlineStatusCondition();
            condition.Operator = StringOperator.Equals;
            condition.TargetValue = status;

            return WrapConditionForTarget(condition, ConditionEntityTarget.Order, target);
        }

        /// <summary>
        /// Read a condition to match an "Online Status" condition from the given v2 store type
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        private static Condition ReadOnlineStatusCondition(XElement v2Condition, StoreTypeCode storeType)
        {
            string code = (string) v2Condition.Element("TargetValue");

            // Special case XCart conversion
            if (storeType == StoreTypeCode.XCart)
            {
                switch (code)
                {
                    case "0": code = "Q"; break;
                    case "1": code = "P"; break;
                    case "2": code = "C"; break;
                    case "3": code = "B"; break;
                    case "4": code = "D"; break;
                    case "5": code = "F"; break;
                    case "6": code = "I"; break;
                    default: throw new InvalidOperationException("Unknown XCart code value: " + code);
                }
            }

            OnlineStatusCondition condition = new OnlineStatusCondition();
            condition.Operator = StringOperator.Equals;

            LocalXmlStatusCodeProvider provider = null;
            if (storeTypeStatusCodesMap.TryGetValue(storeType, out provider))
            {
                // Special case Miva conversion
                if (storeType == StoreTypeCode.Miva)
                {
                    int hashValue = Convert.ToInt32(code);

                    // The default \ known mappings
                    switch (hashValue)
                    {
                        case 180032982: code = "SHIPPED"; break;
                        case 2089405099: code = "PROC"; break;
                    }
                }

                // Special case for Magento conversion
                if (storeType == StoreTypeCode.Magento)
                {
                    int hashValue = Convert.ToInt32(code);

                    // The default \ known mappings
                    switch (hashValue)
                    {
                        case 1542999543: code = "closed"; break;
                        case -1897311302: code = "pending"; break;
                        case -1187810258: code = "processing"; break;
                        case -672062288: code = "pending_paypal"; break;
                        case -2084022644: code = "complete"; break;
                        case -761182302: code = "canceled"; break;
                        case 1569749995: code = "holded"; break;
                    }
                }

                // TODO (maybe) ClickCart and SearchFit both need hashcode -> code conversions, but we don't have very many users of them,
                // and don't currently have any way to create a test store to see what the default codes are.
            }

            if (provider != null && provider.CodeValues.Contains(code))
            {
                condition.TargetValue = provider.GetCodeName(code);
            }
            else
            {
                log.WarnFormat("Could not properly migrate online status condition for type {0} code {1}", storeType, code);
                condition.TargetValue = "Unknown (V2 Upgrade)";
            }

            return condition;
        }

        /// <summary>
        /// Create condition for ebay v2 checkout status condition
        /// </summary>
        private static Condition CreateEbayCheckoutStatusCondition(XElement v2Condition, FilterTarget target)
        {
            int v2Status = (int) v2Condition.Element("TargetValue");

            EbayItemPaymentStatusCondition condition = new EbayItemPaymentStatusCondition();
            condition.Operator = EqualityOperator.Equals;

            switch (v2Status)
            {
                case 0: condition.Value = EbayEffectivePaymentStatus.Incomplete; break;
                case 1: condition.Value = EbayEffectivePaymentStatus.PaymentPendingPayPal; break;
                case 2: condition.Value = EbayEffectivePaymentStatus.BuyerRequestsTotal; break;
                case 3: condition.Value = EbayEffectivePaymentStatus.AwaitingPayment; break;
                case 4: condition.Value = EbayEffectivePaymentStatus.Paid; break;
                case 5: condition.Value = EbayEffectivePaymentStatus.Failed; break;
                case 6: condition.Value = EbayEffectivePaymentStatus.SellerResponded; break;
                case 7: condition.Value = EbayEffectivePaymentStatus.PaymentPending; break;
                case 8: condition.Value = EbayEffectivePaymentStatus.PaymentPendingEscrow; break;
                case 9: condition.Value = EbayEffectivePaymentStatus.EscrowCanceled; break;
            }

            return WrapConditionForTarget(condition, ConditionEntityTarget.OrderItem, target);
        }

        /// <summary>
        /// Create condition to match v2 Payment Method condition
        /// </summary>
        [NDependIgnoreLongMethod]
        private static Condition CreateEbayPaymentMethodCondition(XElement v2Condition, FilterTarget target)
        {
            int v2Method = (int) v2Condition.Element("TargetValue");

            EbayPaymentMethodCondition condition = new EbayPaymentMethodCondition();
            condition.Operator = EqualityOperator.Equals;

            switch (v2Method)
            {
                case 0: condition.Value = EbayEffectivePaymentMethod.NotChosen; break;
                case 1: condition.Value = EbayEffectivePaymentMethod.PayPal; break;
                case 2: condition.Value = EbayEffectivePaymentMethod.MoneyOrderOrCashiersCheck; break;
                case 3: condition.Value = EbayEffectivePaymentMethod.PersonalCheck; break;
                case 4: condition.Value = EbayEffectivePaymentMethod.Cod; break;
                case 5: condition.Value = EbayEffectivePaymentMethod.CreditCard; break;
                case 6: condition.Value = EbayEffectivePaymentMethod.Other; break;
                case 7: condition.Value = EbayEffectivePaymentMethod.NotSpecified; break;
                case 8: condition.Value = EbayEffectivePaymentMethod.Unknown; break;
                case 9: condition.Value = EbayEffectivePaymentMethod.MoneyTransferCip; break;
                case 10: condition.Value = EbayEffectivePaymentMethod.MoneyTransferCipPlus; break;
                case 11: condition.Value = EbayEffectivePaymentMethod.CashOnPickup; break;
                case 12: condition.Value = EbayEffectivePaymentMethod.VisaMastercard; break;
                case 13: condition.Value = EbayEffectivePaymentMethod.AmericanExpress; break;
                case 14: condition.Value = EbayEffectivePaymentMethod.DiscoverCard; break;
                case 15: condition.Value = EbayEffectivePaymentMethod.SeeDescription; break;
                case 16: condition.Value = EbayEffectivePaymentMethod.OnlinePayment; break;
            }

            return WrapConditionForTarget(condition, ConditionEntityTarget.OrderItem, target);
        }

        /// <summary>
        /// Create condition for v2 paypal address status filtering
        /// </summary>
        private static Condition CreateEbayPaypalAddressStatusCondition(XElement v2Condition, FilterTarget target)
        {
            int v2StatusType = (int) v2Condition.Element("TargetValue");

            EbayPayPalAddressStatusCondition condition = new EbayPayPalAddressStatusCondition();
            condition.Operator = EqualityOperator.Equals;

            switch (v2StatusType)
            {
                case 1: condition.Value = Stores.Platforms.PayPal.PayPalAddressStatus.Confirmed; break;
                case 2: condition.Value = Stores.Platforms.PayPal.PayPalAddressStatus.Unconfirmed; break;
            }

            return WrapConditionForTarget(condition, ConditionEntityTarget.OrderItem, target);
        }

        /// <summary>
        /// Create a condition for ebay feedback that matches v2
        /// </summary>
        private static Condition CreateEbayFeedbackCondition(XElement v2Condition, FilterTarget target)
        {
            int v2FeedbackType = (int) v2Condition.Element("TargetValue");

            EbayFeedbackCondition condition = new EbayFeedbackCondition();
            condition.Operator = EqualityOperator.Equals;

            switch (v2FeedbackType)
            {
                case 0: condition.Value = EbayFeedbackConditionStatusType.SellerLeftForBuyer; break;
                case 1: condition.Value = EbayFeedbackConditionStatusType.SellerNotLeftForBuyer; break;
                case 2: condition.Value = EbayFeedbackConditionStatusType.BuyerLeftPositive; break;
                case 3: condition.Value = EbayFeedbackConditionStatusType.BuyerLeftNegative; break;
                case 4: condition.Value = EbayFeedbackConditionStatusType.BuyerLeftNeutral; break;
                case 5: condition.Value = EbayFeedbackConditionStatusType.BuyerNotLeft; break;
            }

            return WrapConditionForTarget(condition, ConditionEntityTarget.OrderItem, target);
        }

        /// <summary>
        /// Create copndition for ProStores auth
        /// </summary>
        private static Condition CreateProStoresAuthorizationCondition(XElement v2Condition, FilterTarget target)
        {
            bool authorized = (bool) v2Condition.Element("TargetValue");

            ProStoresAuthorizationCondition condition = new ProStoresAuthorizationCondition();
            condition.AuthorizationStatus = authorized ? ProStoresAuthorizationStatus.Authorized : ProStoresAuthorizationStatus.NotAuthorized;

            return WrapConditionForTarget(condition, ConditionEntityTarget.Order, target);
        }

        /// <summary>
        /// Wrap the given condition, known to be targetted to orders, keeping in mind how customer-shipments are converted for 3x
        /// </summary>
        private static Condition WrapShipmentConditionForTarget(Condition condition, FilterTarget target)
        {
            // Nothing to do if already for orders
            if (target == FilterTarget.Orders)
            {
                return condition;
            }

            // This is to support selecting the same customer-related shipments as v2. In v3 we don't support customer related shipments so we create fake orders.
            else
            {
                ForAnyOrderCondition anyOrder = new ForAnyOrderCondition();
                anyOrder.Container.FirstGroup.JoinType = ConditionJoinType.All;
                anyOrder.Container.FirstGroup.Conditions.Add(new OrderNumberCondition() { IsNumeric = false, StringOperator = StringOperator.BeginsWith, StringValue = "CustomerShipment-" });
                anyOrder.Container.FirstGroup.Conditions.Add(condition);

                return anyOrder;
            }
        }

        /// <summary>
        /// Create condition to match v2 status condition
        /// </summary>
        private static Condition CreateShipmentStatusCondition(XElement v2Condition)
        {
            ShipmentStatusType statusType = ShipmentStatusType.Processed;

            string statusValue = (string) v2Condition.Element("Status");

            // New format, with void added
            if (statusValue != "")
            {
                switch (Convert.ToInt32(statusValue))
                {
                    case 1: statusType = ShipmentStatusType.None; break;
                    case 2: statusType = ShipmentStatusType.Processed; break;
                    case 4: statusType = ShipmentStatusType.Voided; break;
                }
            }
            // Old format, true\valse, no void
            else
            {
                bool processed = (bool) v2Condition.Element("Processed");

                statusType = processed ? ShipmentStatusType.Processed : ShipmentStatusType.None;
            }

            ShipmentTypeCode shipmentType = (ShipmentTypeCode) (int) v2Condition.Element("ShipmentType");

            // DHL is now Other
            if ((int) shipmentType == 7)
            {
                shipmentType = ShipmentTypeCode.Other;
            }

            return (statusType != ShipmentStatusType.None) ?
                CreateShipmentStatusConditionForNone(statusType, shipmentType) :
                CreateShipmentStatusConditionForOther(shipmentType);
        }

        /// <summary>
        /// Create condition to match v2 status condition
        /// </summary>
        private static Condition CreateShipmentStatusConditionForOther(ShipmentTypeCode shipmentType)
        {
            ForAnyShipmentCondition anyShipment = new ForAnyShipmentCondition();
            anyShipment.Container.FirstGroup.JoinType = ConditionJoinType.Any;
            anyShipment.Container.FirstGroup.Conditions.Add(new ShipmentStatusCondition { Operator = EqualityOperator.Equals, Value = ShipmentStatusType.None });

            // In v2 -1 meant any type
            if ((int)shipmentType != -1)
            {
                anyShipment.Container.FirstGroup.Conditions.Add(new CarrierCondition { Operator = EqualityOperator.NotEqual, Value = shipmentType });
            }

            CombinedResultCondition combined = new CombinedResultCondition();
            combined.Container.FirstGroup.JoinType = ConditionJoinType.Any;
            combined.Container.FirstGroup.Conditions.Add(new OrderShipmentsCondition() { Operator = NumericOperator.Equal, Value1 = 0 });
            combined.Container.FirstGroup.Conditions.Add(anyShipment);

            return combined;
        }

        /// <summary>
        /// Create condition to match v2 status condition
        /// </summary>
        private static Condition CreateShipmentStatusConditionForNone(ShipmentStatusType statusType, ShipmentTypeCode shipmentType)
        {
            ForAnyShipmentCondition anyShipment = new ForAnyShipmentCondition();
            anyShipment.Container.FirstGroup.JoinType = ConditionJoinType.All;
            anyShipment.Container.FirstGroup.Conditions.Add(new ShipmentStatusCondition { Operator = EqualityOperator.Equals, Value = statusType });

            // In v2 -1 meant any type
            if ((int)shipmentType != -1)
            {
                anyShipment.Container.FirstGroup.Conditions.Add(new CarrierCondition { Operator = EqualityOperator.Equals, Value = shipmentType });
            }

            return anyShipment;
        }

        /// <summary>
        /// Create condition to match v2 Shipped date condition
        /// </summary>
        private static Condition CreateShippedDateCondition(XElement v2Condition)
        {
            ShipDateCondition dateCondition = new ShipDateCondition();
            ReadDateCondition(v2Condition, dateCondition);

            CombinedResultCondition combined = new CombinedResultCondition();
            combined.Container.FirstGroup.JoinType = ConditionJoinType.All;
            combined.Container.FirstGroup.Conditions.Add(new ShipmentStatusCondition { Operator = EqualityOperator.NotEqual, Value = ShipmentStatusType.None });
            combined.Container.FirstGroup.Conditions.Add(dateCondition);

            return WrapConditionForTarget(combined, ConditionEntityTarget.Shipment, FilterTarget.Orders);
        }

        /// <summary>
        /// Create a condition that corresponds to "Order Type" in v2
        /// </summary>
        private static Condition CreateOrderTypeCondition(XElement v2Condition, FilterTarget target)
        {
            Condition condition;

            bool downloaded = (bool) v2Condition.Element("TargetValue");
            if (downloaded)
            {
                condition = new DownloadedCondition { Presence = DownloadedPresenceType.Either, DateRangeSpecified = false };
            }
            else
            {
                condition = new ManualCondition();
            }

            return WrapConditionForTarget(condition, ConditionEntityTarget.Order, target);
        }

        /// <summary>
        /// Create a condition that corresponds to the "Payment Detail" condition in v2
        /// </summary>
        private static Condition CreatePaymentDetailCondition(XElement v2Condition, FilterTarget target)
        {
            PaymentDetailValueCondition valueCondition = new PaymentDetailValueCondition();
            ReadStringCondition(v2Condition, valueCondition);

            PaymentDetailLabelCondition labelCondition = new PaymentDetailLabelCondition();
            labelCondition.Operator = StringOperator.Equals;
            labelCondition.TargetValue = (string) v2Condition.Element("DetailName");

            CombinedResultCondition combined = new CombinedResultCondition();
            combined.Container.FirstGroup.JoinType = ConditionJoinType.All;
            combined.Container.FirstGroup.Conditions.Add(labelCondition);
            combined.Container.FirstGroup.Conditions.Add(valueCondition);

            return WrapConditionForTarget(combined, ConditionEntityTarget.PaymentDetail, target);
        }

        /// <summary>
        /// Create a condition to match the v2 notes condition
        /// </summary>
        private static Condition CreateNotesCondition(XElement v2Condition, FilterTarget target)
        {
            NoteTextCondition noteCondition = new NoteTextCondition();
            ReadStringCondition(v2Condition, noteCondition);

            ContainerCondition forAnyNote;
            NumericCondition<int> noteCount;

            // Note's are a strage beast in v3 b\c customer note filters also search order notes, and order not filters find for the customers.  So we just
            // create a note condition based on the final wanted target, rather than the actual note type used by v2
            if (target == FilterTarget.Customers)
            {
                forAnyNote = new ShipWorks.Filters.Content.Conditions.Customers.ForAnyNoteCondition();
                noteCount = new ShipWorks.Filters.Content.Conditions.Customers.NoteCountCondition();
            }
            else
            {
                forAnyNote = new ShipWorks.Filters.Content.Conditions.Orders.ForAnyNoteCondition();
                noteCount = new ShipWorks.Filters.Content.Conditions.Orders.NoteCountCondition();
            }

            forAnyNote.Container.FirstGroup.Conditions.Add(noteCondition);
            noteCount.Operator = NumericOperator.Equal;
            noteCount.Value1 = 0;

            // Have to special case for negation b\c in V2 every order had a default blank note, so every order was considered.
            // If we just did a standard "For any" - only orders that had real V3 notes would be considered
            if (noteCondition.Operator == StringOperator.NotContains || noteCondition.Operator == StringOperator.NotEqual)
            {
                CombinedResultCondition combined = new CombinedResultCondition();
                combined.Container.FirstGroup.JoinType = ConditionJoinType.Any;
                combined.Container.FirstGroup.Conditions.Add(noteCount);
                combined.Container.FirstGroup.Conditions.Add(forAnyNote);

                return combined;
            }
            else
            {
                return forAnyNote;
            }
        }

        /// <summary>
        /// Create a condition to match the v2 Order Charge condition
        /// </summary>
        private static Condition CreateOrderChargeCondition(XElement v2Condition, FilterTarget target)
        {
            OrderChargeTypeCondition typeCondition = new OrderChargeTypeCondition();
            typeCondition.TargetValue = (string) v2Condition.Element("ChargeName");
            typeCondition.Operator = StringOperator.Equals;

            OrderChargeAmountCondition amountCondition = new OrderChargeAmountCondition();
            ReadValueCondition(v2Condition, amountCondition);

            ForAnyChargeCondition anyCharge = new ForAnyChargeCondition();
            anyCharge.Container.FirstGroup.JoinType = ConditionJoinType.All;
            anyCharge.Container.FirstGroup.Conditions.Add(typeCondition);
            anyCharge.Container.FirstGroup.Conditions.Add(amountCondition);

            return WrapConditionForTarget(anyCharge, ConditionEntityTarget.Order, target);
        }

        /// <summary>
        /// Create a condition that simulates the v2 CustomerName condition
        /// </summary>
        private static Condition CreateCustomerNameCondition(XElement v2Condition, FilterTarget target)
        {
            BillShipAddressStringCondition firstNameCondition = ReadBillShipAddressCondition(v2Condition, new CustomerFirstNameCondition());
            BillShipAddressStringCondition lastNameCondition = ReadBillShipAddressCondition(v2Condition, new CustomerLastNameCondition());

            CombinedResultCondition combined = new CombinedResultCondition();
            combined.Container.FirstGroup.Conditions.Add(firstNameCondition);
            combined.Container.FirstGroup.Conditions.Add(lastNameCondition);

            if (firstNameCondition.Operator == StringOperator.NotContains || firstNameCondition.Operator == StringOperator.NotEqual)
            {
                combined.Container.FirstGroup.JoinType = ConditionJoinType.All;
            }
            else
            {
                combined.Container.FirstGroup.JoinType = ConditionJoinType.Any;
            }

            return WrapConditionForTarget(combined, ConditionEntityTarget.Customer, target);
        }

        /// <summary>
        /// Create a condition that simulates the v2 OrderName condition
        /// </summary>
        private static Condition CreateOrderNameCondition(XElement v2Condition, FilterTarget target)
        {
            BillShipAddressStringCondition firstNameCondition = ReadBillShipAddressCondition(v2Condition, new OrderFirstNameCondition());
            BillShipAddressStringCondition lastNameCondition = ReadBillShipAddressCondition(v2Condition, new OrderLastNameCondition());

            CombinedResultCondition combined = new CombinedResultCondition();
            combined.Container.FirstGroup.Conditions.Add(firstNameCondition);
            combined.Container.FirstGroup.Conditions.Add(lastNameCondition);

            if (firstNameCondition.Operator == StringOperator.NotContains || firstNameCondition.Operator == StringOperator.NotEqual)
            {
                combined.Container.FirstGroup.JoinType = ConditionJoinType.All;
            }
            else
            {
                combined.Container.FirstGroup.JoinType = ConditionJoinType.Any;
            }

            return WrapConditionForTarget(combined, ConditionEntityTarget.Order, target);
        }

        /// <summary>
        /// Create condition based on v2 "OrderAddress" condition
        /// </summary>
        private static Condition CreateOrderAddressCondition(XElement v2Condition, FilterTarget target)
        {
            BillShipAddressStringCondition street1Condition = ReadBillShipAddressCondition(v2Condition, new OrderAddressStreet1Condition());
            BillShipAddressStringCondition street2Condition = ReadBillShipAddressCondition(v2Condition, new OrderAddressStreet2Condition());
            BillShipAddressStringCondition street3Condition = ReadBillShipAddressCondition(v2Condition, new OrderAddressStreet3Condition());

            CombinedResultCondition combined = new CombinedResultCondition();
            combined.Container.FirstGroup.Conditions.Add(street1Condition);
            combined.Container.FirstGroup.Conditions.Add(street2Condition);
            combined.Container.FirstGroup.Conditions.Add(street3Condition);

            if (street1Condition.Operator == StringOperator.NotContains || street1Condition.Operator == StringOperator.NotEqual)
            {
                combined.Container.FirstGroup.JoinType = ConditionJoinType.All;
            }
            else
            {
                combined.Container.FirstGroup.JoinType = ConditionJoinType.Any;
            }

            return WrapConditionForTarget(combined, ConditionEntityTarget.Order, target);
        }

        /// <summary>
        /// OrderCountCondition
        /// </summary>
        private static Condition CreateOrderCountCondition(XElement v2Condition, FilterTarget target)
        {
            OrderCountCondition condition = new OrderCountCondition();

            // V2 had this condition as a decimal - so we can't use our handy ReadValueCondition - which would potentially try to read a decimal as an int
            condition.Operator = ParseV2ValueOperator((string)v2Condition.Element("Operator"));
            condition.Value1 = (int)Math.Round((decimal)v2Condition.Element("FirstValue"));
            condition.Value2 = (int)Math.Round((decimal)v2Condition.Element("SecondValue"));

            return WrapConditionForTarget(condition, ConditionEntityTarget.Customer, target);
        }

        /// <summary>
        /// OrderTotalCondition
        /// </summary>
        private static Condition CreateOrderTotalCondition(XElement v2Condition, FilterTarget target)
        {
            if (target == FilterTarget.Customers && (bool) v2Condition.Element("Aggregate"))
            {
                return ReadValueCondition(v2Condition, new AmountSpentCondition());
            }
            else
            {
                OrderTotalCondition condition = new OrderTotalCondition();
                ReadValueCondition(v2Condition, condition);

                return WrapConditionForTarget(condition, ConditionEntityTarget.Order, target);
            }
        }

        /// <summary>
        /// TotalItemsCondition
        /// </summary>
        private static Condition CreateItemsOrderedCondition(XElement v2Condition, FilterTarget target)
        {
            if (target == FilterTarget.Customers && (bool) v2Condition.Element("Aggregate"))
            {
                log.WarnFormat("Cannot migrate ItemsOrderedCondition when aggregating from a customer target.");
                return new NotSupportedV2Condition { Detail = "'Total Items Ordered' is not supported for customer filters in ShipWorks 3." };
            }
            else
            {
                OrderTotalItemsCondition condition = new OrderTotalItemsCondition();

                // V2 had this condition as a decimal - so we can't use our handy ReadValueCondition - which would potentially try to read a decimal as an int
                condition.Operator = ParseV2ValueOperator((string) v2Condition.Element("Operator"));
                condition.Value1 = (int) Math.Round((decimal) v2Condition.Element("FirstValue"));
                condition.Value2 = (int) Math.Round((decimal) v2Condition.Element("SecondValue"));

                return WrapConditionForTarget(condition, ConditionEntityTarget.Order, target);
            }
        }

        /// <summary>
        /// Wrap the given condition in an appropriate parent condition if necessary to get from the condition target to the target that the filter is for.  actionTarget
        /// can only be Customer or Order, since that's the only top-level filters supported by V2
        /// </summary>
        private static Condition WrapConditionForTarget(Condition condition, ConditionEntityTarget conditionTarget, FilterTarget filterTarget)
        {
            // If it's the same, it can be returned as is
            if ((conditionTarget == ConditionEntityTarget.Customer && filterTarget == FilterTarget.Customers) ||
                (conditionTarget == ConditionEntityTarget.Order && filterTarget == FilterTarget.Orders))
            {
                return condition;
            }

            if (conditionTarget == ConditionEntityTarget.Customer)
            {
                Debug.Assert(filterTarget == FilterTarget.Orders);

                CustomerCondition customerCondition = new CustomerCondition();
                customerCondition.Container.FirstGroup.Conditions.Add(condition);

                return customerCondition;
            }

            if (conditionTarget == ConditionEntityTarget.Order)
            {
                Debug.Assert(filterTarget == FilterTarget.Customers);

                ForAnyOrderCondition anyOrderCondition = new ForAnyOrderCondition();
                anyOrderCondition.Container.FirstGroup.Conditions.Add(condition);

                return anyOrderCondition;
            }

            if (conditionTarget == ConditionEntityTarget.OrderItem ||
                conditionTarget == ConditionEntityTarget.OrderCharge ||
                conditionTarget == ConditionEntityTarget.PaymentDetail ||
                conditionTarget == ConditionEntityTarget.Shipment)
            {
                ContainerCondition anyChildCondition;

                switch (conditionTarget)
                {
                    case ConditionEntityTarget.OrderItem: anyChildCondition = new ForAnyItemCondition(); break;
                    case ConditionEntityTarget.OrderCharge: anyChildCondition = new ForAnyChargeCondition(); break;
                    case ConditionEntityTarget.PaymentDetail: anyChildCondition = new ForAnyPaymentDetailCondition(); break;
                    case ConditionEntityTarget.Shipment: anyChildCondition = new ForAnyShipmentCondition(); break;
                    default: throw new InvalidOperationException("Unhandled conditionTarget: " + conditionTarget);
                }

                anyChildCondition.Container.FirstGroup.Conditions.Add(condition);

                if (filterTarget == FilterTarget.Orders)
                {
                    return anyChildCondition;
                }
                else
                {
                    return WrapConditionForTarget(anyChildCondition, ConditionEntityTarget.Order, filterTarget);
                }
            }

            throw new InvalidOperationException("Invalid targets");
        }

        /// <summary>
        /// Read data from the given v2 condition into the specified v3 object
        /// </summary>
        private static DateCondition ReadDateCondition(XElement v2Condition, DateCondition condition)
        {
            condition.Operator = ParseV2DateOperator((string) v2Condition.Element("Operator"));
            condition.WithinRangeType = DateWithinRangeType.FromToday;

            condition.Value1 = DateTime.Parse((string) v2Condition.Element("FirstDate")).ToUniversalTime();
            condition.Value2 = DateTime.Parse((string) v2Condition.Element("SecondDate")).ToUniversalTime();

            condition.WithinAmount = (int) v2Condition.Element("WithinAmount");
            condition.WithinUnit = ParseV2DateWithinUnits((string) v2Condition.Element("WithinUnits"));

            return condition;
        }

        /// <summary>
        /// Read data from the given v2 ValueCondition into the v3 version
        /// </summary>
        private static Condition ReadValueCondition<T>(XElement v2Condition, NumericCondition<T> condition) where T : struct, IComparable
        {
            condition.Operator = ParseV2ValueOperator((string) v2Condition.Element("Operator"));
            condition.Value1 = (T) Convert.ChangeType((string) v2Condition.Element("FirstValue"), typeof(T));
            condition.Value2 = (T) Convert.ChangeType((string) v2Condition.Element("SecondValue"), typeof(T));

            return condition;
        }

        /// <summary>
        /// Read the values from the given v2 condition into the v3 version
        /// </summary>
        private static BillShipAddressStringCondition ReadBillShipAddressCondition(XElement v2Condition, BillShipAddressStringCondition condition)
        {
            condition.AddressOperator = ParseV2AddressType((string) v2Condition.Element("AddressType"));

            ReadStringCondition(v2Condition, condition);

            if (condition.Operator == StringOperator.NotContains || condition.Operator == StringOperator.NotEqual)
            {
                // V2 implements "not" in this context as not ship and not bill
                if (condition.AddressOperator == BillShipAddressOperator.ShipOrBill)
                {
                    condition.AddressOperator = BillShipAddressOperator.ShipAndBill;
                }
            }

            return condition;
        }

        /// <summary>
        /// Read the values form the given v2 condition into the v3 version
        /// </summary>
        private static StringCondition ReadStringCondition(XElement v2Condition, StringCondition condition)
        {
            condition.Operator = ParseV2StringOperator((string) v2Condition.Element("Operator"));
            condition.TargetValue = (string) v2Condition.Element("TargetValue");

            return condition;
        }

        /// <summary>
        /// Read the given v2 ValueOperator value into a v3 NumericOperator
        /// </summary>
        private static NumericOperator ParseV2ValueOperator(string value)
        {
            switch (value)
            {
                case "GreaterThan": return NumericOperator.GreaterThan;
                case "GreaterThanOrEqual": return NumericOperator.GreaterThanOrEqual;
                case "LessThan": return NumericOperator.LessThan;
                case "LessThanOrEqual": return NumericOperator.LessThanOrEqual;
                case "Equal": return NumericOperator.Equal;
                case "NotEqual": return NumericOperator.NotEqual;
                case "Between": return NumericOperator.Between;
                case "NotBetween": return NumericOperator.NotBetween;
            }

            throw new InvalidOperationException("Unsupported v2 ValueOperator: " + value);
        }

        /// <summary>
        /// Parse the V2 AddressType into a v3 BillShipAddressOperator
        /// </summary>
        private static BillShipAddressOperator ParseV2AddressType(string value)
        {
            switch (value)
            {
                case "Shipping": return BillShipAddressOperator.Ship;
                case "Billing": return BillShipAddressOperator.Bill;
                case "Either": return BillShipAddressOperator.ShipOrBill;
            }

            throw new InvalidOperationException("Unsupported v2 AddressType: " + value);
        }

        /// <summary>
        /// Parse the V2 DateOperator into a v3 date operator
        /// </summary>
        private static DateOperator ParseV2DateOperator(string value)
        {
            switch (value)
            {
                case "Today": return DateOperator.Today;
                case "Yesterday": return DateOperator.Yesterday;
                case "WithinTheLast": return DateOperator.WithinTheLast;
                case "GreaterThan": return DateOperator.GreaterThan;
                case "GreaterThanOrEqual": return DateOperator.GreaterThanOrEqual;
                case "LessThan": return DateOperator.LessThan;
                case "LessThanOrEqual": return DateOperator.LessThanOrEqual;
                case "Equal": return DateOperator.Equal;
                case "NotEqual": return DateOperator.NotEqual;
                case "Between": return DateOperator.Between;
                case "NotBetween": return DateOperator.NotBetween;
            }

            throw new InvalidOperationException("Unsupported v2 DateOperator: " + value);
        }

        /// <summary>
        /// Parse the V2 Date WithinUnits value into a v3 enum
        /// </summary>
        private static DateWithinUnit ParseV2DateWithinUnits(string value)
        {
            switch (value)
            {
                case "Days": return DateWithinUnit.Days;
                case "Weeks": return DateWithinUnit.Weeks;
                case "Months": return DateWithinUnit.Months;
                case "Years": return DateWithinUnit.Years;
            }

            throw new InvalidOperationException("Unsupported v2 DateWithinUnit: " + value);
        }

        /// <summary>
        /// Parse the
        /// </summary>
        private static StringOperator ParseV2StringOperator(string value)
        {
            switch (value)
            {
                case "Contains": return StringOperator.Contains;
                case "NotContains": return StringOperator.NotContains;
                case "Equals": return StringOperator.Equals;
                case "NotEqual": return StringOperator.NotEqual;
                case "BeginsWith": return StringOperator.BeginsWith;
                case "EndsWith": return StringOperator.EndsWith;
                case "Matches": return StringOperator.Matches;
            }

            throw new InvalidOperationException("Unsupported v2 StringOperator: " + value);
        }
    }
}
