using System;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Grid;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;
using ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Grid;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Stores.Platforms.PayPal;
using ShipWorks.Stores.Platforms.ProStores.CoreExtensions.Grid;
using ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Grid;
using ShipWorks.Stores.Platforms.Newegg.CoreExtensions.Grid;
using ShipWorks.Stores.Platforms.Shopify.Enums;
using ShipWorks.Properties;
using ShipWorks.Shipping.ShipSense;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the builtin column definitions
    /// </summary>
    public static class OrderColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible order columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            EntityGridAddressSelector shippingAddressSelector = new EntityGridAddressSelector("Ship");
            EntityGridAddressSelector billingAddressSelector = new EntityGridAddressSelector("Bill");

            GridColumnDefinitionCollection definitions = new GridColumnDefinitionCollection
                {
                    new GridColumnDefinition("{33BBD10C-63E1-4f8c-9EFD-05875DCAA9A9}", true, 
                        new GridDateDisplayType { UseDescriptiveDates = true, TimeDisplayFormat = TimeDisplayFormat.None, DateFormat = "MMMM dd, yyyy"}, 
                        "Date", DateTimeUtility.ParseEnUS("03/04/2001 1:30 PM").ToUniversalTime(),
                        OrderFields.OrderDate)
                    {
                        DefaultWidth = 100
                    },

                    new GridColumnDefinition("{BCBE522D-76E6-4a37-90A3-50BA62E51DC2}", true,
                        new GridStoreDisplayType(StoreProperty.StoreName) { ShowIcon = true }, "Store Name", new GridStoreDisplayType.DisplayData { StoreText = "My Store", StoreIcon = StoreIcons.genericmodule },
                        OrderFields.StoreID,
                        StoreFields.StoreName)
                    {
                        DefaultWidth = 100
                    },

                    new GridColumnDefinition("{13E940CA-945B-4c23-83F5-50F758AD4456}", true, 
                        new GridOrderNumberDisplayType { ShowStoreIcon = false }, "Order #", GridOrderNumberDisplayType.SampleData(StoreTypeCode.GenericModule),
                        OrderFields.OrderNumberComplete, 
                        OrderFields.OrderNumber) { DefaultWidth = 75 },

                    new GridColumnDefinition("{E0E6E248-30BE-4eb7-B486-891321313207}", true,
                        new GridTextDisplayType()
                            .Decorate(new GridRollupDecorator(OrderFields.RollupItemCount))
                                .Decorate(new GridStoreSpecificHyperlinkDecorator()), "Item Name", "Dress Shirt",
                        OrderFields.RollupItemName)
                    {
                        DefaultWidth = 150
                    },

                    new GridColumnDefinition("{5999DE0A-A38C-4627-A0B9-DF7B3D0F60CC}", true,
                        new GridTextDisplayType().Decorate(new GridRollupDecorator(OrderFields.RollupItemCount) { MultipleVariedFormat = "#" }), "Qty", "3",
                        OrderFields.RollupItemQuantity)
                    {
                        DefaultWidth = 40
                    },

                    new GridColumnDefinition("{0600A750-E364-4042-A301-9E777E413394}", true,
                        new GridNoteDisplayType(), "Notes", 2,
                        OrderFields.RollupNoteCount) { DefaultWidth = 40 },

                    new GridColumnDefinition("{0614E4C9-AE87-4f77-A759-5E6D44FDD821}", true,
                        new GridTextDisplayType(), "Requested Shipping", "UPS Ground",
                        OrderFields.RequestedShipping),

                    new GridColumnDefinition("{603A9AF5-94B0-4FCC-8832-CAC54BFCFDFD}", true,
                    new GridEnumDisplayType<AddressType>(EnumSortMethod.Description),
                    "Address Type", AddressType.Residential,
                    OrderFields.ShipAddressType)
                    {DefaultWidth = 100},

                    new GridColumnDefinition("{8E9F3D01-98CF-4574-9CF0-9A6A7FE5C86E}", true,
                        new GridCountryDisplayType() { ShowFlag = true, AbbreviationFormat = AbbreviationFormat.Abbreviated }, "S: Country", "US",
                        OrderFields.ShipCountryCode)
                    {
                        DefaultWidth = 60
                    },

                    new GridColumnDefinition("{008B6234-DB5F-40e0-9B18-AF34CC2FD7BA}", true,
                        new GridTextDisplayType(), "S: First Name", "John",
                        OrderFields.ShipFirstName),

                    new GridColumnDefinition("{8775E5C5-0634-40e9-9940-492D36CDEDD7}", true,
                        new GridTextDisplayType(), "S: Last Name", "Smith",
                        OrderFields.ShipLastName),

                    new GridColumnDefinition("{4DEE3C8C-B54B-496b-BD39-429509ECA791}", true,
                        new GridTextDisplayType(), "Store Status", "Pending",
                        OrderFields.OnlineStatus)
                        {
                            DefaultWidth = 80,
                            ApplicableTest = data => StoreManager.GetStoreTypeInstances().Any(st => st.GridOnlineColumnSupported(OnlineGridColumnSupport.OnlineStatus))
                        },

                    new GridColumnDefinition("{6766679C-B0FC-4cdb-B3F8-A73AD4DE87EB}", true,
                        new GridMoneyDisplayType(), "Total", 1024.18m,
                        OrderFields.OrderTotal),
                        
                    new GridColumnDefinition("{B65D5682-EEE7-40FC-BE26-06F9D5A16ABE}", true,
                        new NeweggInvoiceNumberDisplayType(), "Invoice #", "87448975", 
                        NeweggOrderFields.InvoiceNumber)
                        {
                            StoreTypeCode = StoreTypeCode.NeweggMarketplace,
                            DefaultWidth = 70
                        },
                    
                    new GridColumnDefinition("{E93AA547-6F6C-46c5-AEA1-9090127F2C70}", true,
                        new GridOrderNumberDisplayType(), "Client Order #", GridOrderNumberDisplayType.SampleData(StoreTypeCode.ChannelAdvisor),
                        ChannelAdvisorOrderFields.CustomOrderIdentifier)                        
                        {  
                            StoreTypeCode = StoreTypeCode.ChannelAdvisor
                        },
                    
                    new GridColumnDefinition("{DC90E6D4-70F8-465c-B182-5F081DF42782}", true,
                        new GridOrderNumberDisplayType(), "Transaction ID", GridOrderNumberDisplayType.SampleData(StoreTypeCode.PayPal, "X239493824"),
                        PayPalOrderFields.TransactionID)
                        {
                            StoreTypeCode = StoreTypeCode.PayPal 
                        },

                    new GridColumnDefinition("{99CB4A0C-56B5-411a-A2D6-3547E4DA92BB}", true,
                        new GridAmazonOrderDisplayType(), "Amazon Order #", GridOrderNumberDisplayType.SampleData(StoreTypeCode.Amazon, "123-1234567-1234567"),
                        AmazonOrderFields.AmazonOrderID)
                        {
                            StoreTypeCode = StoreTypeCode.Amazon
                        },

                    new GridColumnDefinition("{D7B9F3D4-ADEC-45F4-9596-CDB9886DD7FF}", true,
                        new GridEnumDisplayType<AmazonMwsFulfillmentChannel>(EnumSortMethod.Description), "Fulfilled By", AmazonMwsFulfillmentChannel.MFN,
                        AmazonOrderFields.FulfillmentChannel)
                        {
                            StoreTypeCode = StoreTypeCode.Amazon
                        },

                    new GridColumnDefinition("{74CFD9FC-21DF-45D6-8F58-E1F72901EE44}", true,
                        new GridEnumDisplayType<AmazonMwsIsPrime>(EnumSortMethod.Description), "Amazon Prime", AmazonMwsIsPrime.Yes,
                        AmazonOrderFields.IsPrime)
                        {
                            StoreTypeCode = StoreTypeCode.Amazon
                        },

                    new GridColumnDefinition("{D529CF7A-B27B-4C0C-97A0-8E72FA966B71}", true,
                        new GridDateDisplayType { UseDescriptiveDates = true, TimeDisplayFormat = TimeDisplayFormat.None, DateFormat = "MMMM dd, yyyy"}, 
                        "Latest Delivery", DateTimeUtility.ParseEnUS("03/04/2001 1:30 PM").ToUniversalTime(),
                        AmazonOrderFields.LatestExpectedDeliveryDate)
                        {
                            StoreTypeCode = StoreTypeCode.Amazon
                        },

                    new GridColumnDefinition("{1D15FDDE-6D09-4B74-BB34-32031EB89C08}", true,
                        new GridDateDisplayType { UseDescriptiveDates = true, TimeDisplayFormat = TimeDisplayFormat.None, DateFormat = "MMMM dd, yyyy"}, 
                        "Earliest Delivery", DateTimeUtility.ParseEnUS("03/04/2001 1:30 PM").ToUniversalTime(),
                        AmazonOrderFields.EarliestExpectedDeliveryDate)
                        {
                            StoreTypeCode = StoreTypeCode.Amazon
                        },

                    new GridColumnDefinition("{7edcceda-7330-4af1-82a5-15b0642907db}", true,
                        new GridOrderNumberDisplayType(), "LemonStand Order ID", GridOrderNumberDisplayType.SampleData(StoreTypeCode.LemonStand, "123456789"),
                        LemonStandOrderFields.LemonStandOrderID)
                        {
                            StoreTypeCode = StoreTypeCode.LemonStand
                        },

                    new GridColumnDefinition("{94454F08-0E76-4777-96DD-F184ED77AFFD}", true,
                        new GridOrderNumberDisplayType(), "Groupon Order #", GridOrderNumberDisplayType.SampleData(StoreTypeCode.Groupon, "AB-1234567-1234567"),
                        GrouponOrderFields.GrouponOrderID)
                        {
                            StoreTypeCode = StoreTypeCode.Groupon
                        },

                    new GridColumnDefinition("{CA3ECAB1-B96A-4c17-BC66-9EC7D0DF1035}", true,
                        new GridOrderNumberDisplayType(), "ClickCartPro #", GridOrderNumberDisplayType.SampleData(StoreTypeCode.ClickCartPro, "ORD100002"), 
                        ClickCartProOrderFields.ClickCartProOrderID)
                        {
                            StoreTypeCode = StoreTypeCode.ClickCartPro
                        },

                    new GridColumnDefinition("{24BD1068-74DD-4a0f-9736-08AA8A01065B}", true,
                        new GridOrderNumberDisplayType(), "CommerceInterface #", GridOrderNumberDisplayType.SampleData(StoreTypeCode.CommerceInterface), 
                        CommerceInterfaceOrderFields.CommerceInterfaceOrderNumber)
                        {
                            StoreTypeCode = StoreTypeCode.CommerceInterface
                        },

                    new GridColumnDefinition("{4FC7E6AD-8FA5-4fe3-9F0E-DBF792959E80}", true,
                        new GridOrderNumberDisplayType(), "Seller Order #", GridOrderNumberDisplayType.SampleData(StoreTypeCode.MarketplaceAdvisor),
                        MarketplaceAdvisorOrderFields.SellerOrderNumber)
                        {
                            StoreTypeCode = StoreTypeCode.MarketplaceAdvisor
                        },

                    new GridColumnDefinition("{113FF63F-5081-42a5-9CF9-3C7DB7970656}",
                        new GridOrderNumberDisplayType(), "Invoice #", GridOrderNumberDisplayType.SampleData(StoreTypeCode.MarketplaceAdvisor, "XJ-10645"),
                        MarketplaceAdvisorOrderFields.InvoiceNumber)
                        {
                            StoreTypeCode = StoreTypeCode.MarketplaceAdvisor
                        },

                    new GridColumnDefinition("{0194877F-62AD-4715-8E9D-BB80DBB613B9}",
                        new GridOrderNumberDisplayType(), "Confirmation #", GridOrderNumberDisplayType.SampleData(StoreTypeCode.MarketplaceAdvisor, "AXJ10645"),
                        ProStoresOrderFields.ConfirmationNumber)
                        {
                            StoreTypeCode = StoreTypeCode.ProStores
                        },

                    new GridColumnDefinition("{CE36E374-3B10-4F33-9911-B38768CBC505}", true,
                        new GridOrderNumberDisplayType(), "PO Number", GridOrderNumberDisplayType.SampleData(StoreTypeCode.Sears), 
                        SearsOrderFields.PoNumber)
                        {
                            StoreTypeCode = StoreTypeCode.Sears
                        },

                    new GridColumnDefinition("{419FC10D-48A3-4c97-8CEC-AAB625BC99F7}", true,
                        new GridTextDisplayType(), "Sales Record #", "10645",
                        EbayOrderFields.SellingManagerRecord)
                        {
                            StoreTypeCode = StoreTypeCode.Ebay
                        },

                    new GridColumnDefinition("{AAE328FA-95E4-4846-9D85-CD8A6214E6B0}",
                        new GridDateDisplayType(), "Last Modified (Online)", DateTimeUtility.ParseEnUS("11/28/2007 7:32 AM").ToUniversalTime(),
                        OrderFields.OnlineLastModified)
                        { 
                            ApplicableTest = (data) => 
                            { 
                                return StoreManager.GetStoreTypeInstances().Any(st => st.GridOnlineColumnSupported(OnlineGridColumnSupport.LastModified));
                            } 
                        },

                    new GridColumnDefinition("{8E082567-DEBA-4F43-ACFD-A8C184526D8B}",
                        new GridEnumDisplayType<ShipSenseOrderRecognitionStatus>(EnumSortMethod.Description),
                        "ShipSense", ShipSenseOrderRecognitionStatus.Recognized,
                        OrderFields.ShipSenseRecognitionStatus),

                    new GridColumnDefinition("{00B66937-CB98-4FE5-B916-8DA5BACC06B1}",
                        new GridEnumDisplayType<ChannelAdvisorCheckoutStatus>(EnumSortMethod.Description), "Checkout Status", ChannelAdvisorCheckoutStatus.Completed,
                        ChannelAdvisorOrderFields.OnlineCheckoutStatus)
                        {
                            StoreTypeCode = StoreTypeCode.ChannelAdvisor
                        },

                    new GridColumnDefinition("{B49AE487-3094-411B-B9DC-4484A754378E}",
                        new GridEnumDisplayType<ChannelAdvisorShippingStatus>(EnumSortMethod.Description), "Shipping Status", ChannelAdvisorShippingStatus.Shipped,
                        ChannelAdvisorOrderFields.OnlineShippingStatus)
                        {
                            StoreTypeCode = StoreTypeCode.ChannelAdvisor
                        },

                    new GridColumnDefinition("{8CC4C60C-7ABF-4B40-8009-7993DC976F03}", true,
                        new GridEnumDisplayType<ChannelAdvisorPaymentStatus>(EnumSortMethod.Description), "Payment Status", ChannelAdvisorPaymentStatus.Cleared,
                        ChannelAdvisorOrderFields.OnlinePaymentStatus)
                        {
                            StoreTypeCode = StoreTypeCode.ChannelAdvisor
                        },

                    new GridColumnDefinition("{453187C1-95E1-4343-8EDA-1DE6C434A18B}", 
                        new GridChannelAdvisorFlagDisplayType(), "Flag", Tuple.Create(ChannelAdvisorFlagType.BlueFlag, "Blue Flag"),
                        ChannelAdvisorOrderFields.OrderID)
                        {
                            StoreTypeCode = StoreTypeCode.ChannelAdvisor,
                            DefaultWidth = 120
                        },

                    new GridColumnDefinition("{CFCA474B-D209-4867-BB8C-3F5AD37FD290}", 
                        new GridTextDisplayType(), "Marketplace(s)", "CHANNELADVISOR_STORE", ChannelAdvisorOrderFields.MarketplaceNames)
                        {
                            StoreTypeCode = StoreTypeCode.ChannelAdvisor
                        },

                    new GridColumnDefinition("{CDC41FA5-B652-4E1D-B0E6-7908443249D7}", true,
                        new GridEnumDisplayType<ChannelAdvisorIsAmazonPrime>(EnumSortMethod.Description), "Amazon Prime", ChannelAdvisorIsAmazonPrime.Yes,
                        ChannelAdvisorOrderFields.IsPrime)
                        {
                            StoreTypeCode = StoreTypeCode.ChannelAdvisor
                        },

                    new GridColumnDefinition("{74EF7153-8DFC-4afb-B9A7-0ABD5359B983}", true, 
                        new ProStoresAuthorizationDisplayType(), "Authorized", DateTimeUtility.ParseEnUS("03/04/2001 1:30 PM").ToUniversalTime(),
                        ProStoresOrderFields.AuthorizedDate)
                        {
                            DefaultWidth = 135,
                            StoreTypeCode = StoreTypeCode.ProStores
                        },

                    new GridColumnDefinition("{D82CB513-4956-48ac-B2BB-5D0AC45F4CE4}",
                        new GridStoreDisplayType(StoreProperty.StoreType), "Store Type", new GridStoreDisplayType.DisplayData { StoreText = "Magento", StoreIcon = StoreIcons.magento },
                        OrderFields.StoreID,
                        CreateStoreTypeSortField()),

                    new GridColumnDefinition("{EEECA57B-8BF5-4703-888F-E95930652010}",
                        new GridTextDisplayType()
                            .Decorate(new GridRollupDecorator(OrderFields.RollupItemCount))
                                .Decorate(new GridStoreSpecificHyperlinkDecorator()), "Item Code", "DS-1065",
                        OrderFields.RollupItemCode),

                    new GridColumnDefinition("{BFCD6B98-F483-43d8-AB51-FB38E435046D}",
                        new GridTextDisplayType().Decorate(new GridRollupDecorator(OrderFields.RollupItemCount)), "Item SKU", "1-4656-79798",
                        OrderFields.RollupItemSKU),

                    new GridColumnDefinition("{FC9AC5BE-9BA2-44f8-B045-FD52C38F55A8}",
                        new GridTextDisplayType().Decorate(new GridRollupDecorator(OrderFields.RollupItemCount)), "Item Location", "Shelf 2A",
                        OrderFields.RollupItemLocation),

                    new GridColumnDefinition("{9407F718-C4B6-4d19-96D7-975BF73F84AA}",
                        new GridTextDisplayType(), "Local Status", "Backordered",
                        OrderFields.LocalStatus) { DefaultWidth = 80 },

                    new GridColumnDefinition("{B5362720-6411-46DC-993D-E1E71386EE3E}", true,
                        new GridEnumDisplayType<ShopifyPaymentStatus>(EnumSortMethod.Description), "Payment Status", ShopifyPaymentStatus.Paid,
                        ShopifyOrderFields.PaymentStatusCode)
                        {
                            StoreTypeCode = StoreTypeCode.Shopify,
                            DefaultWidth = 92
                        },

                    new GridColumnDefinition("{B719FDB8-3B17-4846-99FC-D31051BDBA53}", true,
                        new GridEnumDisplayType<ShopifyFulfillmentStatus>(EnumSortMethod.Description), "Fulfillment Status ", ShopifyFulfillmentStatus.Fulfilled,
                        ShopifyOrderFields.FulfillmentStatusCode)
                        {
                            StoreTypeCode = StoreTypeCode.Shopify,
                            DefaultWidth = 100
                        },

                    new GridColumnDefinition("{C214E531-ABDD-4CA7-9104-2B5F7BEB55B6}", true,
                        new GridBooleanDisplayType()
                        {
                            FalseText = "Not Paid",
                            TrueText = "Paid"
                        },
                        "Payment Status", "Paid",
                        EtsyOrderFields.WasPaid)
                        {
                            StoreTypeCode = StoreTypeCode.Etsy
                        },
                
                    new GridColumnDefinition("{4CC71DD4-173F-4E3C-A092-37A466D876F6}", true,
                        new GridBooleanDisplayType()
                        {
                            FalseText = "Not Shipped",
                            TrueText = "Shipped"
                        },
                        "Shipment Status", "Shipped",
                        EtsyOrderFields.WasShipped)
                        {
                            StoreTypeCode = StoreTypeCode.Etsy
                        },

                    new GridColumnDefinition("{6FBF9CB5-EB45-49BF-8274-3A389E99A0CC}", 
                        new GridTextDisplayType(), "Location ID", "1564", SearsOrderFields.LocationID)
                        {
                            StoreTypeCode = StoreTypeCode.Sears
                        },

                    new GridColumnDefinition("{8FF2AC8D-6E44-4FAC-B0B4-EAA2FDF0B9B9}",
                        new GridBooleanDisplayType() { FalseText = "No", TrueText = "Yes" },
                        "Customer Pickup", "No",
                        SearsOrderFields.CustomerPickup)
                        {
                            StoreTypeCode = StoreTypeCode.Sears
                        },

                    new GridColumnDefinition("{C34521DE-001F-41BA-8BE6-FFF6593CE43A}",
                        new GridWeightDisplayType(), "Total Weight", 2.3,
                        OrderFields.RollupItemTotalWeight),

                    new GridColumnDefinition("{DDBC14E2-90B1-4eba-97F1-1A9A4EC0AA08}",
                        new GridTextDisplayType(), "S: Middle Name", "Edward",
                        OrderFields.ShipMiddleName),

                    new GridColumnDefinition("{68FC2428-94E4-4b99-B23A-2921A6F3D267}",
                        new GridTextDisplayType(), "S: Company", "Interapptive, Inc.",
                        OrderFields.ShipCompany),

                    new GridColumnDefinition("{4FE49194-BFED-49fb-B003-B2023B2F170A}",
                        new GridTextDisplayType(), "S: Street1", "14 Main St.",
                        OrderFields.ShipStreet1),

                    new GridColumnDefinition("{E335A9BF-4BAC-4847-8AAD-E5A3F59C39DF}",
                        new GridTextDisplayType(), "S: Street2", "Apt. 1203",
                        OrderFields.ShipStreet2),

                    new GridColumnDefinition("{8490505D-5897-42ec-A1CE-8AB6D4FF7230}",
                        new GridTextDisplayType(), "S: Street3", "Attn: Jane Smith",
                        OrderFields.ShipStreet3),

                    new GridColumnDefinition("{9AC69F78-51A4-480d-855B-64FBB2FF3287}",
                        new GridTextDisplayType(), "S: City", "St. Louis",
                        OrderFields.ShipCity),

                    new GridColumnDefinition("{5B1AA611-DE88-42b6-9B9D-B8D85BF81C9D}",
                        new GridStateDisplayType("Ship"), "S: State", "MO",
                        new GridColumnFunctionValueProvider(e => e),
                        new GridColumnSortProvider(OrderFields.ShipStateProvCode)),

                    new GridColumnDefinition("{4596119E-9F5A-49b7-97F5-B1A572AD3BF6}",
                        new GridTextDisplayType(), "S: Postal Code", "63132",
                        OrderFields.ShipPostalCode),

                    new GridColumnDefinition("{6113F98C-D82D-44ba-900D-3EA50F2078CC}",
                        new GridTextDisplayType(), "S: Phone", "1-314-555-0554",
                        OrderFields.ShipPhone),

                    new GridColumnDefinition("{5B8CAA4D-08CF-4c4e-892F-D0AE7B447EA7}",
                        new GridTextDisplayType(), "S: Fax", "1-314-555-0554",
                        OrderFields.ShipFax),

                    new GridColumnDefinition("{C684A4AF-D582-4281-B14A-7DAF79094A7A}",
                        new GridTextDisplayType(), "S: Email", "john.smith@interapptive.com",
                        OrderFields.ShipEmail),

                    new GridColumnDefinition("{BB026DFD-FF93-4678-A369-7493C7295FB4}",
                        new GridTextDisplayType(), "S: Website", "www.interapptive.com",
                        OrderFields.ShipWebsite),

                    new GridColumnDefinition("{B1ECCC57-1135-48C8-B438-D2B31637AA9A}",
                        new GridEnumDisplayType<AddressValidationStatusType>(EnumSortMethod.Description),
                        "S: Validation Status", AddressValidationStatusType.Valid,
                        OrderFields.ShipAddressValidationStatus) 
                        { DefaultWidth = 100 },

                    new GridColumnDefinition("{8E8261DD-3950-4A63-B58D-BF18607C7EC9}",
                        new GridActionDisplayType(shippingAddressSelector.DisplayValidationSuggestionLabel, 
                            shippingAddressSelector.ShowAddressOptionMenu, shippingAddressSelector.IsValidationSuggestionLinkEnabled), 
                        "S: Validation Suggestions", "2 Suggestions",
                        new GridColumnFunctionValueProvider(x => x),
                        new GridColumnSortProvider(OrderFields.ShipAddressValidationSuggestionCount, OrderFields.ShipAddressValidationStatus))
                        { DefaultWidth = 120 }, 

                    new GridColumnDefinition("{70DDFF53-64AB-406F-A48A-F91A7FEBC402}", 
                        new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                        "S: Residential Status", ValidationDetailStatusType.Yes,
                        OrderFields.ShipResidentialStatus) 
                        { DefaultWidth = 100 }, 

                    new GridColumnDefinition("{B548B1DC-CFF1-4679-B4C4-10B86FA17DE5}",
                        new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                        "S: PO Box", ValidationDetailStatusType.Yes,
                        OrderFields.ShipPOBox) 
                        { DefaultWidth = 72 }, 

                    new GridColumnDefinition("{13598E92-9602-4D48-9E9E-1F7BB2E49FA3}",
                        new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                        "S: US Territory", ValidationDetailStatusType.Yes,
                        OrderFields.ShipUSTerritory) 
                        { DefaultWidth = 145 }, 

                    new GridColumnDefinition("{FB979A60-0C09-4AE9-8383-338745D9C075}",
                        new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                        "S: Military Address",  ValidationDetailStatusType.Yes,
                        OrderFields.ShipMilitaryAddress) 
                        { DefaultWidth = 115 }, 

                    new GridColumnDefinition("{BCC268B7-FE0C-4244-8BEA-E07ADABB90F7}",
                        new GridTextDisplayType(), "B: First Name", "John",
                        OrderFields.BillFirstName),

                    new GridColumnDefinition("{4761C520-8FA4-430b-B52A-95F07C16AC46}",
                        new GridTextDisplayType(), "B: Middle Name", "Edward",
                        OrderFields.BillMiddleName),

                    new GridColumnDefinition("{9C1C8FEF-8C6B-4daa-A58D-0A482A3FC118}",
                        new GridTextDisplayType(), "B: Last Name", "Smith",
                        OrderFields.BillLastName),

                    new GridColumnDefinition("{7A90B1B2-E14D-4c1d-AFE6-6FE5CFF6B31B}",
                        new GridTextDisplayType(), "B: Company", "Interapptive, Inc.",
                        OrderFields.BillCompany),

                    new GridColumnDefinition("{1A78FAD8-C3F6-40f7-B2ED-28D3BF91AA71}",
                        new GridTextDisplayType(), "B: Street1", "14 Main St.",
                        OrderFields.BillStreet1),

                    new GridColumnDefinition("{07830296-3DB8-4d7e-8315-9F5DBF8D36D6}",
                        new GridTextDisplayType(), "B: Street2", "Apt. 1203",
                        OrderFields.BillStreet2),

                    new GridColumnDefinition("{E4BD8DCF-9B95-4220-9C44-527AE0F135E8}",
                        new GridTextDisplayType(), "B: Street3", "Attn: Jane Smith",
                        OrderFields.BillStreet3),

                    new GridColumnDefinition("{67C449C7-A0FE-4474-BE9B-08FF73B09A00}",
                        new GridTextDisplayType(), "B: City", "St. Louis",
                        OrderFields.BillCity),

                    new GridColumnDefinition("{61F3C9CC-2238-4283-91E9-64D70108AC8A}",
                        new GridStateDisplayType("Bill"), "B: State", "MO",
                        new GridColumnFunctionValueProvider(e => e),
                        new GridColumnSortProvider(OrderFields.BillStateProvCode)),
                    
                    new GridColumnDefinition("{D7BF24C1-3E0A-4421-A654-1FAF26EC572E}",
                        new GridTextDisplayType(), "B: Postal Code", "63132",
                        OrderFields.BillPostalCode),

                    new GridColumnDefinition("{75F848C5-294B-40b7-B59C-C64D4924B1FD}",
                        new GridCountryDisplayType() { ShowFlag = true }, "B: Country", "US",
                        OrderFields.BillCountryCode),

                    new GridColumnDefinition("{9939A9F3-E2D5-4f76-92CE-B591E546BA3A}",
                        new GridTextDisplayType(), "B: Phone", "1-314-555-0555",
                        OrderFields.BillPhone),

                    new GridColumnDefinition("{855B171C-380A-46b8-9FE2-2D5F1E9F6E11}",
                        new GridTextDisplayType(), "B: Fax", "1-314-555-0554",
                        OrderFields.BillFax),

                    new GridColumnDefinition("{E25F7036-95D3-4a14-BDBC-3C527A664A63}",
                        new GridTextDisplayType(), "B: Email", "john.smith@interapptive.com",
                        OrderFields.BillEmail),

                    new GridColumnDefinition("{E6BCD158-F666-403c-B589-B8D4F5260161}",
                        new GridTextDisplayType(), "B: Website", "www.interapptive.com",
                        OrderFields.BillWebsite), 

                    new GridColumnDefinition("{EF6A55D6-2F5C-4CF5-93B5-38D4A98DF4BA}",
                        new GridEnumDisplayType<AddressValidationStatusType>(EnumSortMethod.Description),
                        "B: Validation Status", AddressValidationStatusType.Valid,
                        OrderFields.BillAddressValidationStatus) 
                        { DefaultWidth = 100 },

                    new GridColumnDefinition("{0DF6411F-884E-49D2-A8AC-7452EF2DC506}",
                        new GridActionDisplayType(billingAddressSelector.DisplayValidationSuggestionLabel, 
                            billingAddressSelector.ShowAddressOptionMenu, billingAddressSelector.IsValidationSuggestionLinkEnabled), 
                        "B: Validation Suggestions", "2 Suggestions",
                        new GridColumnFunctionValueProvider(x => x),
                        new GridColumnSortProvider(OrderFields.BillAddressValidationSuggestionCount, OrderFields.BillAddressValidationStatus))
                        { DefaultWidth = 120 }, 

                    new GridColumnDefinition("{8FC356A6-3682-4B93-9046-DE3D3947AC69}",
                        new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                        "B: Residential Status", ValidationDetailStatusType.Yes,
                        OrderFields.BillResidentialStatus) 
                        { DefaultWidth = 100 }, 

                    new GridColumnDefinition("{AD6DCADA-4B68-4BB9-BF06-277BFD28EFE3}",
                        new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                        "B: PO Box", ValidationDetailStatusType.Yes,
                        OrderFields.BillPOBox) 
                        { DefaultWidth = 72 }, 

                    new GridColumnDefinition("{A791FFD5-D49B-41C6-A66B-94B6222AB8B8}",
                        new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                        "B: US Territory", ValidationDetailStatusType.Yes,
                        OrderFields.BillUSTerritory) 
                        { DefaultWidth = 145 }, 

                    new GridColumnDefinition("{7DDB96D8-1675-4C34-8F37-EA29D6F3E853}",
                        new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                        "B: Military Address",  ValidationDetailStatusType.Yes,
                        OrderFields.BillMilitaryAddress) 
                        { DefaultWidth = 115 },

                    new GridColumnDefinition("{6CB7D102-B286-426d-80E7-82C7F6C81150}", true,
                        new GridEnumDisplayType<PayPalAddressStatus>(EnumSortMethod.Value), "Address Status", PayPalAddressStatus.Confirmed,
                        PayPalOrderFields.AddressStatus)
                        {
                            StoreTypeCode = StoreTypeCode.PayPal
                        },

                    new GridColumnDefinition("{1AF651ED-3222-465f-921D-485A002AA8C4}",
                        new GridMoneyDisplayType(), "PayPal Fee", 21.23m,
                        PayPalOrderFields.PayPalFee)
                        {
                            StoreTypeCode = StoreTypeCode.PayPal
                        },

                    new GridColumnDefinition("{75EDCBB9-51FA-46fd-B89A-6E86F1A05E62}", true,
                        new GridEnumDisplayType<PayPalPaymentStatus>(EnumSortMethod.Description), "Payment Status", PayPalPaymentStatus.InProgress,
                        PayPalOrderFields.PaymentStatus)
                        {
                            StoreTypeCode = StoreTypeCode.PayPal
                        },

                    new GridColumnDefinition("{F107C50E-BE60-406b-A6C5-373B35A5A749}",
                        new GridMoneyDisplayType(), "Commission", 12.21m,
                        AmazonOrderFields.AmazonCommission)
                        {
                            StoreTypeCode = StoreTypeCode.Amazon
                        },

                    new GridColumnDefinition("{D374C87D-1B91-46F8-8ACE-36224EB35061}", true,
                        new GridMoneyDisplayType(), "Commission", 6.15m, SearsOrderFields.Commission)
                        {
                            StoreTypeCode = StoreTypeCode.Sears
                        },

                    new GridColumnDefinition("{BB87F9D6-8FB9-4842-8148-517737B7918C}", true,
                        new GridEbayBuyerIDDisplayType(), "Buyer", "john_shipworks",
                        EbayOrderFields.EbayBuyerID)
                        {
                            StoreTypeCode = StoreTypeCode.Ebay
                        },

                    new GridColumnDefinition("{053FF282-5FDE-472e-8BB4-C9D984FA8041}", true,
                        new GridEnumDisplayType<EbayEffectivePaymentMethod>(EnumSortMethod.Description).Decorate(new GridRollupDecorator(EbayOrderFields.RollupEbayItemCount, GridRollupStrategy.SameValueOrNull)), "Payment Method", EbayEffectivePaymentMethod.PayPal,
                        EbayOrderFields.RollupEffectivePaymentMethod)
                        {
                            StoreTypeCode = StoreTypeCode.Ebay
                        },

                    new GridColumnDefinition("{067841B1-C098-4d4e-A0FB-ACABA420B135}", true,
                        new GridEnumDisplayType<EbayEffectivePaymentStatus>(EnumSortMethod.Description).Decorate(new GridRollupDecorator(EbayOrderFields.RollupEbayItemCount, GridRollupStrategy.SameValueOrNull)), "Payment Status", EbayEffectivePaymentStatus.AwaitingPayment,
                        EbayOrderFields.RollupEffectiveCheckoutStatus)
                        {
                            StoreTypeCode = StoreTypeCode.Ebay
                        },

                    new GridColumnDefinition("{05926408-40DA-4c4f-B45F-51E3594E2E86}", true,
                        new GridEnumDisplayType<PayPalAddressStatus>(EnumSortMethod.Value).Decorate(new GridRollupDecorator(EbayOrderFields.RollupEbayItemCount, GridRollupStrategy.SameValueOrNull)), "PayPal Address", PayPalAddressStatus.Confirmed,
                        EbayOrderFields.RollupPayPalAddressStatus)
                        {
                            StoreTypeCode = StoreTypeCode.Ebay
                        },

                    new GridColumnDefinition("{C2766EA1-FF03-481e-BF8D-0313149722CB}", true,
                        new GridEbayFeedbackDisplayType().Decorate(new GridRollupDecorator(EbayOrderFields.RollupEbayItemCount, GridRollupStrategy.SameValueOrNull) { ShowMultiImage = true }), "Feedback Left", new GridEbayFeedbackData(GridEbayFeedbackDirection.Left, EbayFeedbackType.Positive, "Thanks!"),
                        new GridColumnFunctionValueProvider(e => CreateEbayFeedbackData(e, GridEbayFeedbackDirection.Left)),
                        new GridColumnSortProvider(EbayOrderFields.RollupFeedbackLeftType))
                        {
                            StoreTypeCode = StoreTypeCode.Ebay
                        },

                    new GridColumnDefinition("{0330CC41-C652-4732-9C30-930E09A2E332}", true,
                        new GridEbayFeedbackDisplayType().Decorate(new GridRollupDecorator(EbayOrderFields.RollupEbayItemCount, GridRollupStrategy.SameValueOrNull) { ShowMultiImage = true }), "Feedback Received", new GridEbayFeedbackData(GridEbayFeedbackDirection.Received, EbayFeedbackType.Positive, "Thanks!"),
                        new GridColumnFunctionValueProvider(e => CreateEbayFeedbackData(e, GridEbayFeedbackDirection.Received)),
                        new GridColumnSortProvider(EbayOrderFields.RollupFeedbackReceivedComments))
                        {
                            StoreTypeCode = StoreTypeCode.Ebay
                        }, 

                    new GridColumnDefinition("{CD0A9D25-E486-4D19-B517-3824E722CD6A}", 
                        new GridEbayGlobalShippingProgramEligibilityDisplayType(), "Eligible for GSP", true, 
                        EbayOrderFields.GspEligible)
                        {
                            StoreTypeCode = StoreTypeCode.Ebay,
                            DefaultWidth = 80
                        },

                    new GridColumnDefinition("{EE649E7C-D0D7-4D1D-9B69-D7363DF32431}",
                        new GridEnumDisplayType<EbayShippingMethod>(EnumSortMethod.Value), "Shipping Method", EbayShippingMethod.DirectToBuyer,
                        EbayOrderFields.SelectedShippingMethod)
                        {
                            StoreTypeCode = StoreTypeCode.Ebay,
                            DefaultWidth = 135
                        },

                    new GridColumnDefinition("{5A038C0A-A0E6-4059-B4EB-D439809D6CDA}",
                        new GridTextDisplayType(), "Promotion", "FreeShip",
                        OrderMotionOrderFields.OrderMotionPromotion)
                        {
                            StoreTypeCode = StoreTypeCode.OrderMotion
                        },

                    new GridColumnDefinition("{BE34737A-52C9-4DFE-927E-4B259C2495E9}",
                        new GridTextDisplayType(), "Invoice #", "1234567-1",
                        OrderMotionOrderFields.OrderMotionInvoiceNumber)
                        {
                            StoreTypeCode = StoreTypeCode.OrderMotion
                        },
                };

            return definitions;
        }

        /// <summary>
        /// Create a new feedback data object that our grid column can display
        /// </summary>
        private static GridEbayFeedbackData CreateEbayFeedbackData(EntityBase2 entity, GridEbayFeedbackDirection direction)
        {
            EbayOrderEntity order = entity as EbayOrderEntity;
            if (order != null)
            {
                if (direction == GridEbayFeedbackDirection.Left)
                {
                    return new GridEbayFeedbackData(direction, (EbayFeedbackType?)order.RollupFeedbackLeftType, order.RollupFeedbackLeftComments);
                }
                else
                {
                    return new GridEbayFeedbackData(direction, (EbayFeedbackType?)order.RollupFeedbackReceivedType, order.RollupFeedbackReceivedComments);
                }
            }

            return null;
        }

        /// <summary>
        /// Create the sort field to be used for Store.TypeCode
        /// </summary>
        private static EntityField2 CreateStoreTypeSortField()
        {
            EntityField2 storeTypeSortField = StoreFields.TypeCode;

            StringBuilder sb = new StringBuilder("CASE {0} ");

            foreach (StoreType storeType in StoreTypeManager.StoreTypes)
            {
                sb.AppendFormat(" WHEN {0} THEN '{1}' ", (int)storeType.TypeCode, storeType.StoreTypeName);
            }

            sb.AppendFormat(" ELSE 'Unknown' END");

            storeTypeSortField.ExpressionToApply = new DbFunctionCall(sb.ToString(), new object[] { StoreFields.TypeCode });

            return storeTypeSortField;
        }
    }
}
