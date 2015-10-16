using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users.Security;
using ShipWorks.Users;
using ShipWorks.Stores.Platforms;
using ShipWorks.Stores;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Stores.Platforms.Ebay;
using EbayWebServices = ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Grid;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Stores.Platforms.PayPal.WebServices;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores.Platforms.PayPal;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;
using ShipWorks.Data.Model;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the builtin column definitions
    /// </summary>
    public static class OrderItemColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            GridColumnDefinitionCollection definitions = new GridColumnDefinitionCollection
                     {
                        new GridColumnDefinition("{49FC4BA3-09F2-445C-8753-E0EC884733F3}",
                            new GridEntityDisplayType(), "Order", new GridEntityDisplayInfo(6, EntityType.OrderEntity, "Order 1028"),
                            OrderItemFields.OrderID,
                            OrderFields.OrderNumber),

                        new GridColumnDefinition("{AB8135D7-C3F6-4d05-96CF-0E887D495DD3}",
                            new GridTextDisplayType().Decorate(new GridStoreSpecificHyperlinkDecorator()), "Code", "WHT-SHRT",
                            OrderItemFields.Code),

                        new GridColumnDefinition("{5BB1F029-222B-4b84-BD49-54AA325B9DBE}", true,
                            new GridTextDisplayType().Decorate(new GridStoreSpecificHyperlinkDecorator()), "Name", "White Shirt",
                            OrderItemFields.Name) { DefaultWidth = 250 },

                        new GridColumnDefinition("{F710718C-56DB-4518-BDB5-67D7B7474BC3}",
                            new GridTextDisplayType(), "SKU", "WHT-SHRT",
                            OrderItemFields.SKU),

                        new GridColumnDefinition("{80337158-A06E-47ac-B47C-C89F0F5C4FDB}",
                            new GridTextDisplayType(), "UPC", "012345678905",
                            OrderItemFields.UPC),  
                        
                        new GridColumnDefinition("{E7A16F7F-605B-43d6-9E34-ED57048CCD2B}",
                            new GridTextDisplayType(), "ISBN", "1601750285",
                            OrderItemFields.ISBN),  

                        new GridColumnDefinition("{89547B80-5254-45f5-9113-CB23890EA1A9}",
                            new GridTextDisplayType(), "Description", "A white T-Shirt",
                            OrderItemFields.Description) { AutoWrap = true },

                        new GridColumnDefinition("{DE74DE4B-C85E-4942-A77A-25ACC6AFDE67}",
                            new GridTextDisplayType(), "Location", "Aisle 14",
                            OrderItemFields.Location),
                 
                        new GridColumnDefinition("{CDDEFBA1-C576-4290-A530-12F770136EB8}", true,
                            new GridTextDisplayType(), "Qty", "1",
                            OrderItemFields.Quantity) { DefaultWidth = 40 },

                        new GridColumnDefinition("{2934184B-4A54-4bde-AE31-4A592CF09698}", true, 
                            new GridMoneyDisplayType(), "Price", 6.18m,
                            OrderItemFields.UnitPrice) { DefaultWidth = 65 },      

                        new GridColumnDefinition("{48B3D675-C83A-4728-B94B-EC422EA959E2}",
                            new GridMoneyDisplayType(), "Cost", 4.18m,
                            OrderItemFields.UnitCost) { DefaultWidth = 65 },    
  
                        new GridColumnDefinition("{33B4F684-0BE7-4c9c-A27D-F9C6CBD66311}",
                            new GridWeightDisplayType(), "Weight", 3.1,
                            OrderItemFields.Weight),    

                        new GridColumnDefinition("{47FACE52-9D1C-45ad-B0E6-4217517935EB}", true,
                            new GridTextDisplayType(), "Status", "In Stock",
                            OrderItemFields.LocalStatus),    

                        #region Infopia

                        new GridColumnDefinition("{E0286DBF-5663-46cc-A368-2C9898360B03}",
                            new GridTextDisplayType(), "Marketplace", "Transact Sale",
                            InfopiaOrderItemFields.Marketplace)
                            {
                                StoreTypeCode = StoreTypeCode.Infopia,
                            },    

                        new GridColumnDefinition("{503BD6B9-4453-46e2-98D7-7ABE0AD8F384}",
                            new GridTextDisplayType(), "Buyer ID", "john.smith@interapptive.com",
                            InfopiaOrderItemFields.BuyerID)
                            {
                                StoreTypeCode = StoreTypeCode.Infopia,
                            },    

                        new GridColumnDefinition("{78DC9172-4A94-4371-A4BD-40861F5BAFF5}",
                            new GridTextDisplayType(), "Item ID", "a10004",
                            InfopiaOrderItemFields.MarketplaceItemID)
                            {
                                StoreTypeCode = StoreTypeCode.Infopia,
                            },  

                        #endregion

                        #region ChannelAdvisor

                        new GridColumnDefinition("{6CBBF6A1-230B-4d12-99B8-3ACA668D1781}",
                            new GridTextDisplayType(), "Marketplace", "CHANNELADVISOR_STORE",
                            ChannelAdvisorOrderItemFields.MarketplaceName)
                            {
                                StoreTypeCode = StoreTypeCode.ChannelAdvisor,
                            },    

                        new GridColumnDefinition("{4D7FC12F-11D1-4751-B8E4-7FEB4DFB4077}",
                            new GridTextDisplayType(), "Buyer ID", "john.smith@interapptive.com",
                            ChannelAdvisorOrderItemFields.MarketplaceBuyerID)
                            {
                                StoreTypeCode = StoreTypeCode.ChannelAdvisor,
                            },  

                        new GridColumnDefinition("{BFDB0A70-5F25-49a4-BBD0-9884675DBB98}",
                            new GridTextDisplayType(), "Sales ID", "11551",
                            ChannelAdvisorOrderItemFields.MarketplaceSalesID)
                            {
                                StoreTypeCode = StoreTypeCode.ChannelAdvisor,
                            },  

                        new GridColumnDefinition("{BED1ABAB-4730-4A83-B644-66B43E1D68D2}",
                            new GridTextDisplayType(), "Distribution Center", "Chicago",
                            ChannelAdvisorOrderItemFields.DistributionCenter)
                            {
                                StoreTypeCode = StoreTypeCode.ChannelAdvisor,
                            },

                        new GridColumnDefinition("{6C8862F3-BA65-4F00-8327-0A8C1C7CB4A0}",
                            new GridTextDisplayType(), "Classification", "Custom",
                            ChannelAdvisorOrderItemFields.Classification)
                            {
                                StoreTypeCode = StoreTypeCode.ChannelAdvisor,
                            },

                        new GridColumnDefinition("{81B29591-CEB3-4927-8F51-CECE05F9A8AB}",
                            new GridTextDisplayType(), "Harmonized Code", "1006.3010",
                            ChannelAdvisorOrderItemFields.HarmonizedCode)
                            {
                                StoreTypeCode = StoreTypeCode.ChannelAdvisor,
                            },

                        new GridColumnDefinition("{5B7D8F90-1951-424E-98DA-A16D88FA421B}", 
                            new GridBooleanDisplayType() { TrueText = "Yes", FalseText = "No" }, 
                            "Fulfilled by Amazon", "Yes", ChannelAdvisorOrderItemFields.IsFBA)
                            {
                                StoreTypeCode = StoreTypeCode.ChannelAdvisor
                            },

                        new GridColumnDefinition("{492CD77E-61D6-48D4-B72F-2BB611313373}",
                            new GridTextDisplayType(), "MPN", "ACME-11551-AI45",
                            ChannelAdvisorOrderItemFields.MPN)
                            {
                                StoreTypeCode = StoreTypeCode.ChannelAdvisor,
                            },  

                        new GridColumnDefinition("{232C89AC-750A-4E79-B240-FAC0E83F976D}",
                            new GridTextDisplayType(), "Marketplace Store Name", "ACME Store",
                            ChannelAdvisorOrderItemFields.MarketplaceStoreName)
                            {
                                StoreTypeCode = StoreTypeCode.ChannelAdvisor,
                            }, 

                        #endregion

                        #region Amazon

                        new GridColumnDefinition("{348BE9E5-35C0-4f1f-AE8C-6CD596C9D253}",
                            new GridTextDisplayType(), "ASIN", "234234234",
                            AmazonOrderItemFields.ASIN)
                            {
                                StoreTypeCode = StoreTypeCode.Amazon,
                            },

                        new GridColumnDefinition("{9516F607-9B67-4c8c-8EDF-303FE7D13633}",
                            new GridTextDisplayType(), "Condition", "Brand New", 
                            AmazonOrderItemFields.ConditionNote)
                            {
                                StoreTypeCode = StoreTypeCode.Amazon,
                            },

                        new GridColumnDefinition("{060EF3C1-A195-4983-B5A9-460852D698B1}",
                            new GridTextDisplayType(), "Amazon Code", "93294934231", 
                            AmazonOrderItemFields.AmazonOrderItemCode)
                            {
                                StoreTypeCode = StoreTypeCode.Amazon,
                            },

                        #endregion

                        #region eBay

                        new GridColumnDefinition("{2BFEE377-3D9C-477d-9B3E-F0ED055CBDC8}",
                            new GridEnumDisplayType<EbayEffectivePaymentMethod>(EnumSortMethod.Description), "Payment Method", EbayEffectivePaymentMethod.PayPal,
                            EbayOrderItemFields.EffectivePaymentMethod)
                            {
                                StoreTypeCode = StoreTypeCode.Ebay
                            },

                        new GridColumnDefinition("{85CFDCF5-4243-4dc0-90CC-777CEC004768}",
                            new GridEnumDisplayType<EbayEffectivePaymentStatus>(EnumSortMethod.Description), "Payment Status", EbayEffectivePaymentStatus.AwaitingPayment,
                            EbayOrderItemFields.EffectiveCheckoutStatus)
                            {
                                StoreTypeCode = StoreTypeCode.Ebay
                            },

                        new GridColumnDefinition("{B79CDE81-97FA-4F09-B900-5F26DA061232}",
                            new GridBooleanDisplayType() { TrueText = "Shipped", FalseText = "Not Shipped" }, "My eBay (Shipped)", "Shipped",
                            EbayOrderItemFields.MyEbayShipped)
                            {
                                StoreTypeCode = StoreTypeCode.Ebay
                            },

                        new GridColumnDefinition("{8E552A78-A06F-457E-80D9-415BEAFC5143}",
                            new GridBooleanDisplayType() { TrueText = "Paid", FalseText = "Not Paid" }, "My eBay (Paid)", "Paid",
                            EbayOrderItemFields.MyEbayPaid)
                            {
                                StoreTypeCode = StoreTypeCode.Ebay
                            },

                        new GridColumnDefinition("{859E7BE3-9571-499f-A23E-FE0CFC6A285A}",
                            new GridEbayFeedbackDisplayType(), "Feedback Left", new GridEbayFeedbackData(GridEbayFeedbackDirection.Left, EbayFeedbackType.Positive, "Thanks!"),
                            new GridColumnFunctionValueProvider(e => CreateEbayFeedbackData(e, GridEbayFeedbackDirection.Left)),
                            new GridColumnSortProvider(EbayOrderItemFields.FeedbackLeftType))
                            {
                                StoreTypeCode = StoreTypeCode.Ebay
                            },

                        new GridColumnDefinition("{D6A8C5AE-CA7C-46be-88B2-E808CF9B983D}",
                            new GridEbayFeedbackDisplayType(), "Feedback Received", new GridEbayFeedbackData(GridEbayFeedbackDirection.Received, EbayFeedbackType.Positive, "Thanks!"),
                            new GridColumnFunctionValueProvider(e => CreateEbayFeedbackData(e, GridEbayFeedbackDirection.Received)),
                            new GridColumnSortProvider(EbayOrderItemFields.FeedbackReceivedComments))
                            {
                                StoreTypeCode = StoreTypeCode.Ebay
                            },

                        new GridColumnDefinition("{F95830FD-AE3F-48b9-995D-014F796B847B}",
                            new GridEnumDisplayType<PayPalAddressStatus>(EnumSortMethod.Value), "PayPal Address", PayPalAddressStatus.Confirmed,
                            EbayOrderItemFields.PayPalAddressStatus)
                            {
                                StoreTypeCode = StoreTypeCode.Ebay
                            }, 

                        new GridColumnDefinition("{B00E4A3E-D224-4ea7-B6AF-172FA93BBC8C}",
                            new GridTextDisplayType(), "Sales Record #", "64505",
                            EbayOrderItemFields.SellingManagerRecord)
                            {
                                StoreTypeCode = StoreTypeCode.Ebay,
                            },

                        #endregion

                        #region Buy.com

                        new GridColumnDefinition("{744C8CFA-C193-47A1-986D-F12C2015FADA}",
                            new GridTextDisplayType(), "Listing ID", "23789", 
                            BuyDotComOrderItemFields.ListingID)
                            {
                                StoreTypeCode = StoreTypeCode.BuyDotCom
                            },

                        new GridColumnDefinition("{B331A0E3-CEBE-4184-8D00-DC345E74166B}", 
                            new GridMoneyDisplayType(), "Shipping", 1.18m,
                            BuyDotComOrderItemFields.Shipping) 
                            {
                                StoreTypeCode = StoreTypeCode.BuyDotCom,
                                DefaultWidth = 65 
                            },      

                        new GridColumnDefinition("{8F2C7144-280E-4D66-BFC1-3366F15EC04C}", 
                            new GridMoneyDisplayType(), "Tax", 3.05m,
                            BuyDotComOrderItemFields.Tax)
                            {
                                StoreTypeCode = StoreTypeCode.BuyDotCom,
                                DefaultWidth = 65 
                            },      

                        new GridColumnDefinition("{65DAB0C9-53C7-4527-826B-D1D5D54F482F}", 
                            new GridMoneyDisplayType(), "Commission", 0.45m,
                            BuyDotComOrderItemFields.Commission)
                            {
                                StoreTypeCode = StoreTypeCode.BuyDotCom,
                                DefaultWidth = 65 
                            },      

                        new GridColumnDefinition("{C4623B7A-D22F-4053-920B-24E38281E262}", 
                            new GridMoneyDisplayType(), "Item Fee", 1.98m,
                            BuyDotComOrderItemFields.ItemFee)
                            {
                                StoreTypeCode = StoreTypeCode.BuyDotCom,
                                DefaultWidth = 65 
                            },      

                        #endregion

                        #region Sears

                        new GridColumnDefinition("{CDA16197-FCBE-4855-863F-65615717652C}", true,
                            new GridTextDisplayType(), "Online Status", "NEW", 
                            SearsOrderItemFields.OnlineStatus)
                            {
                                StoreTypeCode = StoreTypeCode.Sears
                            },

                        new GridColumnDefinition("{44E0FBE9-6A34-4D8D-A740-AE80BA492D99}", 
                            new GridMoneyDisplayType(), "Shipping", 1.18m,
                            SearsOrderItemFields.Shipping) 
                            {
                                StoreTypeCode = StoreTypeCode.Sears,
                                DefaultWidth = 65 
                            },      

                        new GridColumnDefinition("{972C8829-E2D9-4786-BC9F-C306DC8BB8C6}", 
                            new GridMoneyDisplayType(), "Commission", 3.18m,
                            SearsOrderItemFields.Commission) 
                            {
                                StoreTypeCode = StoreTypeCode.Sears,
                                DefaultWidth = 65 
                            },      

                        #endregion
                           
                        #region BigCommerce 

                        new GridColumnDefinition("{A6555016-825A-4751-AE26-BCF05735EA45}", true,
                            new GridBooleanDisplayType() { TrueText = "Yes", FalseText = "No" }, "Digital Item", "Yes",
                            BigCommerceOrderItemFields.IsDigitalItem)
                            {
                                StoreTypeCode = StoreTypeCode.BigCommerce
                            },

                        new GridColumnDefinition("{4BE527B8-E0E2-4471-B6B0-5CA22146A4A9}", true,
                            new GridTextDisplayType(), "Event Name", "Delivery Date",
                            BigCommerceOrderItemFields.EventName)
                            {
                                StoreTypeCode = StoreTypeCode.BigCommerce
                            },

                        new GridColumnDefinition("{FD5585BB-7B41-49DB-BA66-7CD4708CD1A6}", true,
                            new GridDateDisplayType() { TimeDisplayFormat = TimeDisplayFormat.None }, "Event Date", new DateTime(2012, 11, 26),
                            BigCommerceOrderItemFields.EventDate)
                            {
                                StoreTypeCode = StoreTypeCode.BigCommerce
                            },

                #endregion

                #region Groupon

                        new GridColumnDefinition("{4D8AB7C4-E166-4C52-A00D-44BC3B53D8DD}", true,
                            new GridTextDisplayType(), "BOM SKU", "A123456",
                            GrouponOrderItemFields.BomSKU)
                        {
                            StoreTypeCode = StoreTypeCode.Groupon
                        },

                #endregion


<<<<<<< HEAD
                #region LemonStand
                        new GridColumnDefinition("{39eefe98-96e4-4a31-9732-dc92ec83e155}", true,
                            new GridTextDisplayType(), "Url Name", "cap",
                            LemonStandOrderItemFields.UrlName)
                            {
                                StoreTypeCode = StoreTypeCode.LemonStand
                            },

                        new GridColumnDefinition("{39f2da7e-ec0d-4e9c-b98c-2547d8252e2d}", true,
                            new GridTextDisplayType(), "Short Description", "A short description",
                            LemonStandOrderItemFields.ShortDescription)
                            {
                                StoreTypeCode = StoreTypeCode.LemonStand
                            },

                        new GridColumnDefinition("{967932c3-462e-458a-8f91-294009bff650}", true,
                            new GridTextDisplayType(), "Category", "Sporting Goods",
                            LemonStandOrderItemFields.Category)
                            {
                                StoreTypeCode = StoreTypeCode.LemonStand
                            },
                        
                #endregion

=======
>>>>>>> feature-groupon-add-bomsku-to-grid
                new GridColumnDefinition("{5D0135AC-ECE9-47e5-AB02-D91FAF91EA84}", true,
                            new GridActionDisplayType(o => UserSession.Security.HasPermission(PermissionType.OrdersModify, (long) o) ? "Edit" : "", GridLinkAction.Edit), "Edit", "Edit",
                            OrderItemFields.OrderItemID) 
                            { 
                                DefaultWidth = 31,
                                ApplicableTest = (data) => data != null ? UserSession.Security.HasPermission(PermissionType.OrdersModify, (long) data) : true
                            },

                        new GridColumnDefinition("{E102F97E-5C0F-4fa2-A0CF-47FC852C0B57}", true,
                            new GridActionDisplayType(o => UserSession.Security.HasPermission(PermissionType.OrdersModify, (long) o) ? "Delete" : "", GridLinkAction.Delete), "Delete", "Delete",
                            OrderItemFields.OrderItemID) 
                            { 
                                DefaultWidth = 45 ,
                                ApplicableTest = (data) => data != null ? UserSession.Security.HasPermission(PermissionType.OrdersModify, (long) data) : true
                            },

                     };

            return definitions;
        }

        /// <summary>
        /// Create the data object to use as for ebay feedback display
        /// </summary>
        private static GridEbayFeedbackData CreateEbayFeedbackData(EntityBase2 item, GridEbayFeedbackDirection direction)
        {
            EbayOrderItemEntity ebayItem = item as EbayOrderItemEntity;
            if (ebayItem != null)
            {
                if (direction == GridEbayFeedbackDirection.Left)
                {
                    return new GridEbayFeedbackData(direction, (EbayFeedbackType) ebayItem.FeedbackLeftType, ebayItem.FeedbackLeftComments);
                }
                else
                {
                    return new GridEbayFeedbackData(direction, (EbayFeedbackType) ebayItem.FeedbackReceivedType, ebayItem.FeedbackReceivedComments);
                }
            }

            return null;
        }
    }
}
