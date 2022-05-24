using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// RateShipmentResponse
    /// </summary>
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public partial class RateShipmentResponse : BaseShipEngineResponse, IEquatable<RateShipmentResponse>, IValidatableObject
    {
        /// <summary>
        /// Defines ShipmentStatus
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
        public enum ShipmentStatusEnum
        {

            /// <summary>
            /// Enum Pending for value: pending
            /// </summary>
            [EnumMember(Value = "pending")]
            Pending = 1,

            /// <summary>
            /// Enum Processing for value: processing
            /// </summary>
            [EnumMember(Value = "processing")]
            Processing = 2,

            /// <summary>
            /// Enum Labelpurchased for value: label_purchased
            /// </summary>
            [EnumMember(Value = "label_purchased")]
            Labelpurchased = 3,

            /// <summary>
            /// Enum Cancelled for value: cancelled
            /// </summary>
            [EnumMember(Value = "cancelled")]
            Cancelled = 4
        }

        /// <summary>
        /// Gets or Sets ShipmentStatus
        /// </summary>
        [DataMember(Name = "shipment_status", EmitDefaultValue = false)]
        public ShipmentStatusEnum? ShipmentStatus { get; set; }
        /// <summary>
        /// Defines Confirmation
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
        public enum ConfirmationEnum
        {

            /// <summary>
            /// Enum None for value: none
            /// </summary>
            [EnumMember(Value = "none")]
            None = 1,

            /// <summary>
            /// Enum Delivery for value: delivery
            /// </summary>
            [EnumMember(Value = "delivery")]
            Delivery = 2,

            /// <summary>
            /// Enum Signature for value: signature
            /// </summary>
            [EnumMember(Value = "signature")]
            Signature = 3,

            /// <summary>
            /// Enum Adultsignature for value: adult_signature
            /// </summary>
            [EnumMember(Value = "adult_signature")]
            Adultsignature = 4,

            /// <summary>
            /// Enum Directsignature for value: direct_signature
            /// </summary>
            [EnumMember(Value = "direct_signature")]
            Directsignature = 5
        }

        /// <summary>
        /// Gets or Sets Confirmation
        /// </summary>
        [DataMember(Name = "confirmation", EmitDefaultValue = false)]
        public ConfirmationEnum? Confirmation { get; set; }
        /// <summary>
        /// Defines OrderSourceCode
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
        public enum OrderSourceCodeEnum
        {

            /// <summary>
            /// Enum ShipStation for value: shipStation
            /// </summary>
            [EnumMember(Value = "shipStation")]
            ShipStation = 1,

            /// <summary>
            /// Enum Ebay for value: ebay
            /// </summary>
            [EnumMember(Value = "ebay")]
            Ebay = 2,

            /// <summary>
            /// Enum Amazon for value: amazon
            /// </summary>
            [EnumMember(Value = "amazon")]
            Amazon = 3,

            /// <summary>
            /// Enum Rakuten for value: rakuten
            /// </summary>
            [EnumMember(Value = "rakuten")]
            Rakuten = 4,

            /// <summary>
            /// Enum Paypal for value: paypal
            /// </summary>
            [EnumMember(Value = "paypal")]
            Paypal = 5,

            /// <summary>
            /// Enum Volusion for value: volusion
            /// </summary>
            [EnumMember(Value = "volusion")]
            Volusion = 6,

            /// <summary>
            /// Enum Etsy for value: etsy
            /// </summary>
            [EnumMember(Value = "etsy")]
            Etsy = 7,

            /// <summary>
            /// Enum GoogleCheckout for value: googleCheckout
            /// </summary>
            [EnumMember(Value = "googleCheckout")]
            GoogleCheckout = 8,

            /// <summary>
            /// Enum ZenCart for value: zenCart
            /// </summary>
            [EnumMember(Value = "zenCart")]
            ZenCart = 9,

            /// <summary>
            /// Enum MagentoGo for value: magentoGo
            /// </summary>
            [EnumMember(Value = "magentoGo")]
            MagentoGo = 10,

            /// <summary>
            /// Enum Bigcommerce for value: bigcommerce
            /// </summary>
            [EnumMember(Value = "bigcommerce")]
            Bigcommerce = 11,

            /// <summary>
            /// Enum Shopify for value: shopify
            /// </summary>
            [EnumMember(Value = "shopify")]
            Shopify = 12,

            /// <summary>
            /// Enum AmazonUK for value: amazonUK
            /// </summary>
            [EnumMember(Value = "amazonUK")]
            AmazonUK = 13,

            /// <summary>
            /// Enum OsCommerce for value: osCommerce
            /// </summary>
            [EnumMember(Value = "osCommerce")]
            OsCommerce = 14,

            /// <summary>
            /// Enum XCart for value: xCart
            /// </summary>
            [EnumMember(Value = "xCart")]
            XCart = 15,

            /// <summary>
            /// Enum Channeladvisor for value: channeladvisor
            /// </summary>
            [EnumMember(Value = "channeladvisor")]
            Channeladvisor = 16,

            /// <summary>
            /// Enum Opencart for value: opencart
            /// </summary>
            [EnumMember(Value = "opencart")]
            Opencart = 17,

            /// <summary>
            /// Enum Memberly for value: memberly
            /// </summary>
            [EnumMember(Value = "memberly")]
            Memberly = 18,

            /// <summary>
            /// Enum Spree for value: spree
            /// </summary>
            [EnumMember(Value = "spree")]
            Spree = 19,

            /// <summary>
            /// Enum CustomStore for value: customStore
            /// </summary>
            [EnumMember(Value = "customStore")]
            CustomStore = 20,

            /// <summary>
            /// Enum ThreeDcart for value: threeDcart
            /// </summary>
            [EnumMember(Value = "threeDcart")]
            ThreeDcart = 21,

            /// <summary>
            /// Enum Yahoo for value: yahoo
            /// </summary>
            [EnumMember(Value = "yahoo")]
            Yahoo = 22,

            /// <summary>
            /// Enum Sears for value: sears
            /// </summary>
            [EnumMember(Value = "sears")]
            Sears = 23,

            /// <summary>
            /// Enum StitchLabs for value: stitchLabs
            /// </summary>
            [EnumMember(Value = "stitchLabs")]
            StitchLabs = 24,

            /// <summary>
            /// Enum TopShelf for value: topShelf
            /// </summary>
            [EnumMember(Value = "topShelf")]
            TopShelf = 25,

            /// <summary>
            /// Enum Magento for value: magento
            /// </summary>
            [EnumMember(Value = "magento")]
            Magento = 26,

            /// <summary>
            /// Enum Bizelo for value: bizelo
            /// </summary>
            [EnumMember(Value = "bizelo")]
            Bizelo = 27,

            /// <summary>
            /// Enum LemonStand for value: lemonStand
            /// </summary>
            [EnumMember(Value = "lemonStand")]
            LemonStand = 28,

            /// <summary>
            /// Enum Highwire for value: highwire
            /// </summary>
            [EnumMember(Value = "highwire")]
            Highwire = 29,

            /// <summary>
            /// Enum AmazonCA for value: amazonCA
            /// </summary>
            [EnumMember(Value = "amazonCA")]
            AmazonCA = 30,

            /// <summary>
            /// Enum Goodsie for value: goodsie
            /// </summary>
            [EnumMember(Value = "goodsie")]
            Goodsie = 31,

            /// <summary>
            /// Enum NewEgg for value: newEgg
            /// </summary>
            [EnumMember(Value = "newEgg")]
            NewEgg = 32,

            /// <summary>
            /// Enum UltraCart for value: ultraCart
            /// </summary>
            [EnumMember(Value = "ultraCart")]
            UltraCart = 33,

            /// <summary>
            /// Enum Woocommerce for value: woocommerce
            /// </summary>
            [EnumMember(Value = "woocommerce")]
            Woocommerce = 34,

            /// <summary>
            /// Enum CommerceInterface for value: commerceInterface
            /// </summary>
            [EnumMember(Value = "commerceInterface")]
            CommerceInterface = 35,

            /// <summary>
            /// Enum AbanteCart for value: abanteCart
            /// </summary>
            [EnumMember(Value = "abanteCart")]
            AbanteCart = 36,

            /// <summary>
            /// Enum Sophio for value: sophio
            /// </summary>
            [EnumMember(Value = "sophio")]
            Sophio = 37,

            /// <summary>
            /// Enum Storenvy for value: storenvy
            /// </summary>
            [EnumMember(Value = "storenvy")]
            Storenvy = 38,

            /// <summary>
            /// Enum Brightpearl for value: brightpearl
            /// </summary>
            [EnumMember(Value = "brightpearl")]
            Brightpearl = 39,

            /// <summary>
            /// Enum AmazonDE for value: amazonDE
            /// </summary>
            [EnumMember(Value = "amazonDE")]
            AmazonDE = 40,

            /// <summary>
            /// Enum AmazonES for value: amazonES
            /// </summary>
            [EnumMember(Value = "amazonES")]
            AmazonES = 41,

            /// <summary>
            /// Enum AmazonFR for value: amazonFR
            /// </summary>
            [EnumMember(Value = "amazonFR")]
            AmazonFR = 42,

            /// <summary>
            /// Enum AmazonIT for value: amazonIT
            /// </summary>
            [EnumMember(Value = "amazonIT")]
            AmazonIT = 43,

            /// <summary>
            /// Enum EcwidV1 for value: ecwidV1
            /// </summary>
            [EnumMember(Value = "ecwidV1")]
            EcwidV1 = 44,

            /// <summary>
            /// Enum Redditgifts for value: redditgifts
            /// </summary>
            [EnumMember(Value = "redditgifts")]
            Redditgifts = 45,

            /// <summary>
            /// Enum Wish for value: wish
            /// </summary>
            [EnumMember(Value = "wish")]
            Wish = 46,

            /// <summary>
            /// Enum Soldsie for value: soldsie
            /// </summary>
            [EnumMember(Value = "soldsie")]
            Soldsie = 47,

            /// <summary>
            /// Enum BestBuy for value: bestBuy
            /// </summary>
            [EnumMember(Value = "bestBuy")]
            BestBuy = 48,

            /// <summary>
            /// Enum PrestaShop for value: prestaShop
            /// </summary>
            [EnumMember(Value = "prestaShop")]
            PrestaShop = 49,

            /// <summary>
            /// Enum EBayUK for value: eBayUK
            /// </summary>
            [EnumMember(Value = "eBayUK")]
            EBayUK = 50,

            /// <summary>
            /// Enum EBayCA for value: eBayCA
            /// </summary>
            [EnumMember(Value = "eBayCA")]
            EBayCA = 51,

            /// <summary>
            /// Enum MivaMerchant for value: mivaMerchant
            /// </summary>
            [EnumMember(Value = "mivaMerchant")]
            MivaMerchant = 52,

            /// <summary>
            /// Enum Jane for value: jane
            /// </summary>
            [EnumMember(Value = "jane")]
            Jane = 53,

            /// <summary>
            /// Enum Vault for value: vault
            /// </summary>
            [EnumMember(Value = "vault")]
            Vault = 54,

            /// <summary>
            /// Enum Squarespace for value: squarespace
            /// </summary>
            [EnumMember(Value = "squarespace")]
            Squarespace = 55,

            /// <summary>
            /// Enum BigcommerceV2 for value: bigcommerceV2
            /// </summary>
            [EnumMember(Value = "bigcommerceV2")]
            BigcommerceV2 = 56,

            /// <summary>
            /// Enum AmeriCommerce for value: ameriCommerce
            /// </summary>
            [EnumMember(Value = "ameriCommerce")]
            AmeriCommerce = 57,

            /// <summary>
            /// Enum FoxyCart for value: foxyCart
            /// </summary>
            [EnumMember(Value = "foxyCart")]
            FoxyCart = 58,

            /// <summary>
            /// Enum CSCart for value: cSCart
            /// </summary>
            [EnumMember(Value = "cSCart")]
            CSCart = 59,

            /// <summary>
            /// Enum Bonanza for value: bonanza
            /// </summary>
            [EnumMember(Value = "bonanza")]
            Bonanza = 60,

            /// <summary>
            /// Enum MijoShop for value: mijoShop
            /// </summary>
            [EnumMember(Value = "mijoShop")]
            MijoShop = 61,

            /// <summary>
            /// Enum SellerActive for value: sellerActive
            /// </summary>
            [EnumMember(Value = "sellerActive")]
            SellerActive = 62,

            /// <summary>
            /// Enum OpenSky for value: openSky
            /// </summary>
            [EnumMember(Value = "openSky")]
            OpenSky = 63,

            /// <summary>
            /// Enum TradeGecko for value: tradeGecko
            /// </summary>
            [EnumMember(Value = "tradeGecko")]
            TradeGecko = 64,

            /// <summary>
            /// Enum GrouponGoods for value: grouponGoods
            /// </summary>
            [EnumMember(Value = "grouponGoods")]
            GrouponGoods = 65,

            /// <summary>
            /// Enum Reverb for value: reverb
            /// </summary>
            [EnumMember(Value = "reverb")]
            Reverb = 66,

            /// <summary>
            /// Enum Square for value: square
            /// </summary>
            [EnumMember(Value = "square")]
            Square = 67,

            /// <summary>
            /// Enum Choxi for value: choxi
            /// </summary>
            [EnumMember(Value = "choxi")]
            Choxi = 68,

            /// <summary>
            /// Enum AmazonJP for value: amazonJP
            /// </summary>
            [EnumMember(Value = "amazonJP")]
            AmazonJP = 69,

            /// <summary>
            /// Enum NeweggBusiness for value: neweggBusiness
            /// </summary>
            [EnumMember(Value = "neweggBusiness")]
            NeweggBusiness = 70,

            /// <summary>
            /// Enum WoocommerceV2 for value: woocommerceV2
            /// </summary>
            [EnumMember(Value = "woocommerceV2")]
            WoocommerceV2 = 71,

            /// <summary>
            /// Enum FullscreenDirect for value: fullscreenDirect
            /// </summary>
            [EnumMember(Value = "fullscreenDirect")]
            FullscreenDirect = 72,

            /// <summary>
            /// Enum Celery for value: celery
            /// </summary>
            [EnumMember(Value = "celery")]
            Celery = 73,

            /// <summary>
            /// Enum Penny for value: penny
            /// </summary>
            [EnumMember(Value = "penny")]
            Penny = 74,

            /// <summary>
            /// Enum Groopdealz for value: groopdealz
            /// </summary>
            [EnumMember(Value = "groopdealz")]
            Groopdealz = 75,

            /// <summary>
            /// Enum Cratejoy for value: cratejoy
            /// </summary>
            [EnumMember(Value = "cratejoy")]
            Cratejoy = 76,

            /// <summary>
            /// Enum DearSystems for value: dearSystems
            /// </summary>
            [EnumMember(Value = "dearSystems")]
            DearSystems = 77,

            /// <summary>
            /// Enum Freestyle for value: freestyle
            /// </summary>
            [EnumMember(Value = "freestyle")]
            Freestyle = 78,

            /// <summary>
            /// Enum SellBrite for value: sellBrite
            /// </summary>
            [EnumMember(Value = "sellBrite")]
            SellBrite = 79,

            /// <summary>
            /// Enum Hatch for value: hatch
            /// </summary>
            [EnumMember(Value = "hatch")]
            Hatch = 80,

            /// <summary>
            /// Enum Zoey for value: zoey
            /// </summary>
            [EnumMember(Value = "zoey")]
            Zoey = 81,

            /// <summary>
            /// Enum Linnworks for value: linnworks
            /// </summary>
            [EnumMember(Value = "linnworks")]
            Linnworks = 82,

            /// <summary>
            /// Enum BigCartel for value: bigCartel
            /// </summary>
            [EnumMember(Value = "bigCartel")]
            BigCartel = 83,

            /// <summary>
            /// Enum AmazonMX for value: amazonMX
            /// </summary>
            [EnumMember(Value = "amazonMX")]
            AmazonMX = 84,

            /// <summary>
            /// Enum SureDone for value: sureDone
            /// </summary>
            [EnumMember(Value = "sureDone")]
            SureDone = 85,

            /// <summary>
            /// Enum SecureStore for value: secureStore
            /// </summary>
            [EnumMember(Value = "secureStore")]
            SecureStore = 86,

            /// <summary>
            /// Enum NeweggCanada for value: neweggCanada
            /// </summary>
            [EnumMember(Value = "neweggCanada")]
            NeweggCanada = 87,

            /// <summary>
            /// Enum Spreesy for value: spreesy
            /// </summary>
            [EnumMember(Value = "spreesy")]
            Spreesy = 88,

            /// <summary>
            /// Enum Ecwid for value: ecwid
            /// </summary>
            [EnumMember(Value = "ecwid")]
            Ecwid = 89,

            /// <summary>
            /// Enum Walmart for value: walmart
            /// </summary>
            [EnumMember(Value = "walmart")]
            Walmart = 90,

            /// <summary>
            /// Enum Mozu for value: mozu
            /// </summary>
            [EnumMember(Value = "mozu")]
            Mozu = 91,

            /// <summary>
            /// Enum Stripe for value: stripe
            /// </summary>
            [EnumMember(Value = "stripe")]
            Stripe = 92,

            /// <summary>
            /// Enum Tanga for value: tanga
            /// </summary>
            [EnumMember(Value = "tanga")]
            Tanga = 93,

            /// <summary>
            /// Enum Weebly for value: weebly
            /// </summary>
            [EnumMember(Value = "weebly")]
            Weebly = 94,

            /// <summary>
            /// Enum LabelApi for value: labelApi
            /// </summary>
            [EnumMember(Value = "labelApi")]
            LabelApi = 95,

            /// <summary>
            /// Enum EBayAU for value: eBayAU
            /// </summary>
            [EnumMember(Value = "eBayAU")]
            EBayAU = 96,

            /// <summary>
            /// Enum Houzz for value: houzz
            /// </summary>
            [EnumMember(Value = "houzz")]
            Houzz = 97,

            /// <summary>
            /// Enum Tophatter for value: tophatter
            /// </summary>
            [EnumMember(Value = "tophatter")]
            Tophatter = 98,

            /// <summary>
            /// Enum ODBC for value: oDBC
            /// </summary>
            [EnumMember(Value = "oDBC")]
            ODBC = 99,

            /// <summary>
            /// Enum SparkShipping for value: sparkShipping
            /// </summary>
            [EnumMember(Value = "sparkShipping")]
            SparkShipping = 100,

            /// <summary>
            /// Enum ChannelSale for value: channelSale
            /// </summary>
            [EnumMember(Value = "channelSale")]
            ChannelSale = 101,

            /// <summary>
            /// Enum CoreCommerce for value: coreCommerce
            /// </summary>
            [EnumMember(Value = "coreCommerce")]
            CoreCommerce = 102,

            /// <summary>
            /// Enum GeekSeller for value: geekSeller
            /// </summary>
            [EnumMember(Value = "geekSeller")]
            GeekSeller = 103,

            /// <summary>
            /// Enum SAPAnywhere for value: sAPAnywhere
            /// </summary>
            [EnumMember(Value = "sAPAnywhere")]
            SAPAnywhere = 104,

            /// <summary>
            /// Enum Cin7 for value: cin7
            /// </summary>
            [EnumMember(Value = "cin7")]
            Cin7 = 105,

            /// <summary>
            /// Enum Bandcamp for value: bandcamp
            /// </summary>
            [EnumMember(Value = "bandcamp")]
            Bandcamp = 106,

            /// <summary>
            /// Enum Jet for value: jet
            /// </summary>
            [EnumMember(Value = "jet")]
            Jet = 107,

            /// <summary>
            /// Enum SalesforceCommerceCloud for value: salesforceCommerceCloud
            /// </summary>
            [EnumMember(Value = "salesforceCommerceCloud")]
            SalesforceCommerceCloud = 108,

            /// <summary>
            /// Enum RevolutionParts for value: revolutionParts
            /// </summary>
            [EnumMember(Value = "revolutionParts")]
            RevolutionParts = 109,

            /// <summary>
            /// Enum AmazonAU for value: amazonAU
            /// </summary>
            [EnumMember(Value = "amazonAU")]
            AmazonAU = 110,

            /// <summary>
            /// Enum Handshake for value: handshake
            /// </summary>
            [EnumMember(Value = "handshake")]
            Handshake = 111,

            /// <summary>
            /// Enum Acumatica for value: acumatica
            /// </summary>
            [EnumMember(Value = "acumatica")]
            Acumatica = 112,

            /// <summary>
            /// Enum Facebook for value: facebook
            /// </summary>
            [EnumMember(Value = "facebook")]
            Facebook = 113,

            /// <summary>
            /// Enum Api for value: api
            /// </summary>
            [EnumMember(Value = "api")]
            Api = 114,

            /// <summary>
            /// Enum RateBrowser for value: rateBrowser
            /// </summary>
            [EnumMember(Value = "rateBrowser")]
            RateBrowser = 115,

            /// <summary>
            /// Enum Wix for value: wix
            /// </summary>
            [EnumMember(Value = "wix")]
            Wix = 116,

            /// <summary>
            /// Enum Overstock for value: overstock
            /// </summary>
            [EnumMember(Value = "overstock")]
            Overstock = 117,

            /// <summary>
            /// Enum ListingMirror for value: listingMirror
            /// </summary>
            [EnumMember(Value = "listingMirror")]
            ListingMirror = 118,

            /// <summary>
            /// Enum Ordertime for value: ordertime
            /// </summary>
            [EnumMember(Value = "ordertime")]
            Ordertime = 119
        }

        /// <summary>
        /// Gets or Sets OrderSourceCode
        /// </summary>
        [DataMember(Name = "order_source_code", EmitDefaultValue = false)]
        public OrderSourceCodeEnum? OrderSourceCode { get; set; }
        /// <summary>
        /// Defines InsuranceProvider
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
        public enum InsuranceProviderEnum
        {

            /// <summary>
            /// Enum None for value: none
            /// </summary>
            [EnumMember(Value = "none")]
            None = 1,

            /// <summary>
            /// Enum Shipsurance for value: shipsurance
            /// </summary>
            [EnumMember(Value = "shipsurance")]
            Shipsurance = 2,

            /// <summary>
            /// Enum Carrier for value: carrier
            /// </summary>
            [EnumMember(Value = "carrier")]
            Carrier = 3,

            /// <summary>
            /// Enum Thirdparty for value: third_party
            /// </summary>
            [EnumMember(Value = "third_party")]
            Thirdparty = 4
        }

        /// <summary>
        /// Gets or Sets InsuranceProvider
        /// </summary>
        [DataMember(Name = "insurance_provider", EmitDefaultValue = false)]
        public InsuranceProviderEnum? InsuranceProvider { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="RateShipmentResponse" /> class.
        /// </summary>
        /// <param name="rateResponse">rateResponse.</param>
        /// <param name="shipmentId">shipmentId.</param>
        /// <param name="carrierId">carrierId.</param>
        /// <param name="serviceCode">serviceCode.</param>
        /// <param name="externalShipmentId">externalShipmentId.</param>
        /// <param name="shipDate">shipDate.</param>
        /// <param name="createdAt">createdAt.</param>
        /// <param name="modifiedAt">modifiedAt.</param>
        /// <param name="shipmentStatus">shipmentStatus.</param>
        /// <param name="shipTo">shipTo.</param>
        /// <param name="shipFrom">shipFrom.</param>
        /// <param name="warehouseId">warehouseId.</param>
        /// <param name="returnTo">returnTo.</param>
        /// <param name="confirmation">confirmation.</param>
        /// <param name="customs">customs.</param>
        /// <param name="externalOrderId">externalOrderId.</param>
        /// <param name="orderSourceCode">orderSourceCode.</param>
        /// <param name="advancedOptions">advancedOptions.</param>
        /// <param name="insuranceProvider">insuranceProvider.</param>
        /// <param name="tags">tags.</param>
        /// <param name="packages">packages.</param>
        /// <param name="items">items.</param>
        public RateShipmentResponse(RateResponse rateResponse = default(RateResponse), string shipmentId = default(string), string carrierId = default(string), string serviceCode = default(string), string externalShipmentId = default(string), DateTime? shipDate = default(DateTime?), DateTime? createdAt = default(DateTime?), DateTime? modifiedAt = default(DateTime?), ShipmentStatusEnum? shipmentStatus = default(ShipmentStatusEnum?), Address shipTo = default(Address), Address shipFrom = default(Address), string warehouseId = default(string), Address returnTo = default(Address), ConfirmationEnum? confirmation = default(ConfirmationEnum?), InternationalOptions customs = default(InternationalOptions), string externalOrderId = default(string), OrderSourceCodeEnum? orderSourceCode = default(OrderSourceCodeEnum?), AdvancedOptions advancedOptions = default(AdvancedOptions), InsuranceProviderEnum? insuranceProvider = default(InsuranceProviderEnum?), List<TagResponse> tags = default(List<TagResponse>), List<ShipmentPackage> packages = default(List<ShipmentPackage>), List<ShipmentItem> items = default(List<ShipmentItem>))
        {
            this.RateResponse = rateResponse;
            this.ShipmentId = shipmentId;
            this.CarrierId = carrierId;
            this.ServiceCode = serviceCode;
            this.ExternalShipmentId = externalShipmentId;
            this.ShipDate = shipDate;
            this.CreatedAt = createdAt;
            this.ModifiedAt = modifiedAt;
            this.ShipmentStatus = shipmentStatus;
            this.ShipTo = shipTo;
            this.ShipFrom = shipFrom;
            this.WarehouseId = warehouseId;
            this.ReturnTo = returnTo;
            this.Confirmation = confirmation;
            this.Customs = customs;
            this.ExternalOrderId = externalOrderId;
            this.OrderSourceCode = orderSourceCode;
            this.AdvancedOptions = advancedOptions;
            this.InsuranceProvider = insuranceProvider;
            this.Tags = tags;
            this.Packages = packages;
            this.Items = items;
        }

        /// <summary>
        /// Gets or Sets RateResponse
        /// </summary>
        [DataMember(Name = "rate_response", EmitDefaultValue = false)]
        public RateResponse RateResponse { get; set; }

        /// <summary>
        /// Gets or Sets ShipmentId
        /// </summary>
        [DataMember(Name = "shipment_id", EmitDefaultValue = false)]
        public string ShipmentId { get; set; }

        /// <summary>
        /// Gets or Sets CarrierId
        /// </summary>
        [DataMember(Name = "carrier_id", EmitDefaultValue = false)]
        public string CarrierId { get; set; }

        /// <summary>
        /// Gets or Sets ServiceCode
        /// </summary>
        [DataMember(Name = "service_code", EmitDefaultValue = false)]
        public string ServiceCode { get; set; }

        /// <summary>
        /// Gets or Sets ExternalShipmentId
        /// </summary>
        [DataMember(Name = "external_shipment_id", EmitDefaultValue = false)]
        public string ExternalShipmentId { get; set; }

        /// <summary>
        /// Gets or Sets ShipDate
        /// </summary>
        [DataMember(Name = "ship_date", EmitDefaultValue = false)]
        public DateTime? ShipDate { get; set; }

        /// <summary>
        /// Gets or Sets CreatedAt
        /// </summary>
        [DataMember(Name = "created_at", EmitDefaultValue = false)]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedAt
        /// </summary>
        [DataMember(Name = "modified_at", EmitDefaultValue = false)]
        public DateTime? ModifiedAt { get; set; }


        /// <summary>
        /// Gets or Sets ShipTo
        /// </summary>
        [DataMember(Name = "ship_to", EmitDefaultValue = false)]
        public Address ShipTo { get; set; }

        /// <summary>
        /// Gets or Sets ShipFrom
        /// </summary>
        [DataMember(Name = "ship_from", EmitDefaultValue = false)]
        public Address ShipFrom { get; set; }

        /// <summary>
        /// Gets or Sets WarehouseId
        /// </summary>
        [DataMember(Name = "warehouse_id", EmitDefaultValue = false)]
        public string WarehouseId { get; set; }

        /// <summary>
        /// Gets or Sets ReturnTo
        /// </summary>
        [DataMember(Name = "return_to", EmitDefaultValue = false)]
        public Address ReturnTo { get; set; }


        /// <summary>
        /// Gets or Sets Customs
        /// </summary>
        [DataMember(Name = "customs", EmitDefaultValue = false)]
        public InternationalOptions Customs { get; set; }

        /// <summary>
        /// Gets or Sets ExternalOrderId
        /// </summary>
        [DataMember(Name = "external_order_id", EmitDefaultValue = false)]
        public string ExternalOrderId { get; set; }


        /// <summary>
        /// Gets or Sets AdvancedOptions
        /// </summary>
        [DataMember(Name = "advanced_options", EmitDefaultValue = false)]
        public AdvancedOptions AdvancedOptions { get; set; }


        /// <summary>
        /// Gets or Sets Tags
        /// </summary>
        [DataMember(Name = "tags", EmitDefaultValue = false)]
        public List<TagResponse> Tags { get; set; }

        /// <summary>
        /// Gets or Sets Packages
        /// </summary>
        [DataMember(Name = "packages", EmitDefaultValue = false)]
        public List<ShipmentPackage> Packages { get; set; }

        /// <summary>
        /// Gets or Sets TotalWeight
        /// </summary>
        [DataMember(Name = "total_weight", EmitDefaultValue = false)]
        public Weight TotalWeight { get; private set; }

        /// <summary>
        /// Gets or Sets Items
        /// </summary>
        [DataMember(Name = "items", EmitDefaultValue = false)]
        public List<ShipmentItem> Items { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RateShipmentResponse {\n");
            sb.Append("  RateResponse: ").Append(RateResponse).Append("\n");
            sb.Append("  ShipmentId: ").Append(ShipmentId).Append("\n");
            sb.Append("  CarrierId: ").Append(CarrierId).Append("\n");
            sb.Append("  ServiceCode: ").Append(ServiceCode).Append("\n");
            sb.Append("  ExternalShipmentId: ").Append(ExternalShipmentId).Append("\n");
            sb.Append("  ShipDate: ").Append(ShipDate).Append("\n");
            sb.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
            sb.Append("  ModifiedAt: ").Append(ModifiedAt).Append("\n");
            sb.Append("  ShipmentStatus: ").Append(ShipmentStatus).Append("\n");
            sb.Append("  ShipTo: ").Append(ShipTo).Append("\n");
            sb.Append("  ShipFrom: ").Append(ShipFrom).Append("\n");
            sb.Append("  WarehouseId: ").Append(WarehouseId).Append("\n");
            sb.Append("  ReturnTo: ").Append(ReturnTo).Append("\n");
            sb.Append("  Confirmation: ").Append(Confirmation).Append("\n");
            sb.Append("  Customs: ").Append(Customs).Append("\n");
            sb.Append("  ExternalOrderId: ").Append(ExternalOrderId).Append("\n");
            sb.Append("  OrderSourceCode: ").Append(OrderSourceCode).Append("\n");
            sb.Append("  AdvancedOptions: ").Append(AdvancedOptions).Append("\n");
            sb.Append("  InsuranceProvider: ").Append(InsuranceProvider).Append("\n");
            sb.Append("  Tags: ").Append(Tags).Append("\n");
            sb.Append("  Packages: ").Append(Packages).Append("\n");
            sb.Append("  TotalWeight: ").Append(TotalWeight).Append("\n");
            sb.Append("  Items: ").Append(Items).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as RateShipmentResponse);
        }

        /// <summary>
        /// Returns true if RateShipmentResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of RateShipmentResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RateShipmentResponse input)
        {
            if (input == null)
                return false;

            return
                (
                    this.RateResponse == input.RateResponse ||
                    (this.RateResponse != null &&
                    this.RateResponse.Equals(input.RateResponse))
                ) &&
                (
                    this.ShipmentId == input.ShipmentId ||
                    (this.ShipmentId != null &&
                    this.ShipmentId.Equals(input.ShipmentId))
                ) &&
                (
                    this.CarrierId == input.CarrierId ||
                    (this.CarrierId != null &&
                    this.CarrierId.Equals(input.CarrierId))
                ) &&
                (
                    this.ServiceCode == input.ServiceCode ||
                    (this.ServiceCode != null &&
                    this.ServiceCode.Equals(input.ServiceCode))
                ) &&
                (
                    this.ExternalShipmentId == input.ExternalShipmentId ||
                    (this.ExternalShipmentId != null &&
                    this.ExternalShipmentId.Equals(input.ExternalShipmentId))
                ) &&
                (
                    this.ShipDate == input.ShipDate ||
                    (this.ShipDate != null &&
                    this.ShipDate.Equals(input.ShipDate))
                ) &&
                (
                    this.CreatedAt == input.CreatedAt ||
                    (this.CreatedAt != null &&
                    this.CreatedAt.Equals(input.CreatedAt))
                ) &&
                (
                    this.ModifiedAt == input.ModifiedAt ||
                    (this.ModifiedAt != null &&
                    this.ModifiedAt.Equals(input.ModifiedAt))
                ) &&
                (
                    this.ShipmentStatus == input.ShipmentStatus ||
                    (this.ShipmentStatus != null &&
                    this.ShipmentStatus.Equals(input.ShipmentStatus))
                ) &&
                (
                    this.ShipTo == input.ShipTo ||
                    (this.ShipTo != null &&
                    this.ShipTo.Equals(input.ShipTo))
                ) &&
                (
                    this.ShipFrom == input.ShipFrom ||
                    (this.ShipFrom != null &&
                    this.ShipFrom.Equals(input.ShipFrom))
                ) &&
                (
                    this.WarehouseId == input.WarehouseId ||
                    (this.WarehouseId != null &&
                    this.WarehouseId.Equals(input.WarehouseId))
                ) &&
                (
                    this.ReturnTo == input.ReturnTo ||
                    (this.ReturnTo != null &&
                    this.ReturnTo.Equals(input.ReturnTo))
                ) &&
                (
                    this.Confirmation == input.Confirmation ||
                    (this.Confirmation != null &&
                    this.Confirmation.Equals(input.Confirmation))
                ) &&
                (
                    this.Customs == input.Customs ||
                    (this.Customs != null &&
                    this.Customs.Equals(input.Customs))
                ) &&
                (
                    this.ExternalOrderId == input.ExternalOrderId ||
                    (this.ExternalOrderId != null &&
                    this.ExternalOrderId.Equals(input.ExternalOrderId))
                ) &&
                (
                    this.OrderSourceCode == input.OrderSourceCode ||
                    (this.OrderSourceCode != null &&
                    this.OrderSourceCode.Equals(input.OrderSourceCode))
                ) &&
                (
                    this.AdvancedOptions == input.AdvancedOptions ||
                    (this.AdvancedOptions != null &&
                    this.AdvancedOptions.Equals(input.AdvancedOptions))
                ) &&
                (
                    this.InsuranceProvider == input.InsuranceProvider ||
                    (this.InsuranceProvider != null &&
                    this.InsuranceProvider.Equals(input.InsuranceProvider))
                ) &&
                (
                    this.Tags == input.Tags ||
                    this.Tags != null &&
                    this.Tags.SequenceEqual(input.Tags)
                ) &&
                (
                    this.Packages == input.Packages ||
                    this.Packages != null &&
                    this.Packages.SequenceEqual(input.Packages)
                ) &&
                (
                    this.TotalWeight == input.TotalWeight ||
                    (this.TotalWeight != null &&
                    this.TotalWeight.Equals(input.TotalWeight))
                ) &&
                (
                    this.Items == input.Items ||
                    this.Items != null &&
                    this.Items.SequenceEqual(input.Items)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.RateResponse != null)
                    hashCode = hashCode * 59 + this.RateResponse.GetHashCode();
                if (this.ShipmentId != null)
                    hashCode = hashCode * 59 + this.ShipmentId.GetHashCode();
                if (this.CarrierId != null)
                    hashCode = hashCode * 59 + this.CarrierId.GetHashCode();
                if (this.ServiceCode != null)
                    hashCode = hashCode * 59 + this.ServiceCode.GetHashCode();
                if (this.ExternalShipmentId != null)
                    hashCode = hashCode * 59 + this.ExternalShipmentId.GetHashCode();
                if (this.ShipDate != null)
                    hashCode = hashCode * 59 + this.ShipDate.GetHashCode();
                if (this.CreatedAt != null)
                    hashCode = hashCode * 59 + this.CreatedAt.GetHashCode();
                if (this.ModifiedAt != null)
                    hashCode = hashCode * 59 + this.ModifiedAt.GetHashCode();
                if (this.ShipmentStatus != null)
                    hashCode = hashCode * 59 + this.ShipmentStatus.GetHashCode();
                if (this.ShipTo != null)
                    hashCode = hashCode * 59 + this.ShipTo.GetHashCode();
                if (this.ShipFrom != null)
                    hashCode = hashCode * 59 + this.ShipFrom.GetHashCode();
                if (this.WarehouseId != null)
                    hashCode = hashCode * 59 + this.WarehouseId.GetHashCode();
                if (this.ReturnTo != null)
                    hashCode = hashCode * 59 + this.ReturnTo.GetHashCode();
                if (this.Confirmation != null)
                    hashCode = hashCode * 59 + this.Confirmation.GetHashCode();
                if (this.Customs != null)
                    hashCode = hashCode * 59 + this.Customs.GetHashCode();
                if (this.ExternalOrderId != null)
                    hashCode = hashCode * 59 + this.ExternalOrderId.GetHashCode();
                if (this.OrderSourceCode != null)
                    hashCode = hashCode * 59 + this.OrderSourceCode.GetHashCode();
                if (this.AdvancedOptions != null)
                    hashCode = hashCode * 59 + this.AdvancedOptions.GetHashCode();
                if (this.InsuranceProvider != null)
                    hashCode = hashCode * 59 + this.InsuranceProvider.GetHashCode();
                if (this.Tags != null)
                    hashCode = hashCode * 59 + this.Tags.GetHashCode();
                if (this.Packages != null)
                    hashCode = hashCode * 59 + this.Packages.GetHashCode();
                if (this.TotalWeight != null)
                    hashCode = hashCode * 59 + this.TotalWeight.GetHashCode();
                if (this.Items != null)
                    hashCode = hashCode * 59 + this.Items.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
