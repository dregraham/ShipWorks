using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// ShipmentItem
    /// </summary>
    [DataContract]
    public partial class ShipmentItem : IEquatable<ShipmentItem>, IValidatableObject
    {
        /// <summary>
        /// Defines OrderSourceCode
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
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
        /// Initializes a new instance of the <see cref="ShipmentItem" /> class.
        /// </summary>
        /// <param name="salesOrderId">salesOrderId.</param>
        /// <param name="salesOrderItemId">salesOrderItemId.</param>
        /// <param name="quantity">quantity.</param>
        /// <param name="name">name.</param>
        /// <param name="sku">sku.</param>
        /// <param name="externalOrderId">externalOrderId.</param>
        /// <param name="externalOrderItemId">externalOrderItemId.</param>
        /// <param name="asin">asin.</param>
        /// <param name="orderSourceCode">orderSourceCode.</param>
        public ShipmentItem(string salesOrderId = default(string), string salesOrderItemId = default(string), int? quantity = default(int?), string name = default(string), string sku = default(string), string externalOrderId = default(string), string externalOrderItemId = default(string), string asin = default(string), OrderSourceCodeEnum? orderSourceCode = default(OrderSourceCodeEnum?))
        {
            this.SalesOrderId = salesOrderId;
            this.SalesOrderItemId = salesOrderItemId;
            this.Quantity = quantity;
            this.Name = name;
            this.Sku = sku;
            this.ExternalOrderId = externalOrderId;
            this.ExternalOrderItemId = externalOrderItemId;
            this.Asin = asin;
            this.OrderSourceCode = orderSourceCode;
        }

        /// <summary>
        /// Gets or Sets SalesOrderId
        /// </summary>
        [DataMember(Name = "sales_order_id", EmitDefaultValue = false)]
        public string SalesOrderId { get; set; }

        /// <summary>
        /// Gets or Sets SalesOrderItemId
        /// </summary>
        [DataMember(Name = "sales_order_item_id", EmitDefaultValue = false)]
        public string SalesOrderItemId { get; set; }

        /// <summary>
        /// Gets or Sets Quantity
        /// </summary>
        [DataMember(Name = "quantity", EmitDefaultValue = false)]
        public int? Quantity { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Sku
        /// </summary>
        [DataMember(Name = "sku", EmitDefaultValue = false)]
        public string Sku { get; set; }

        /// <summary>
        /// Gets or Sets ExternalOrderId
        /// </summary>
        [DataMember(Name = "external_order_id", EmitDefaultValue = false)]
        public string ExternalOrderId { get; set; }

        /// <summary>
        /// Gets or Sets ExternalOrderItemId
        /// </summary>
        [DataMember(Name = "external_order_item_id", EmitDefaultValue = false)]
        public string ExternalOrderItemId { get; set; }

        /// <summary>
        /// Gets or Sets Asin
        /// </summary>
        [DataMember(Name = "asin", EmitDefaultValue = false)]
        public string Asin { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ShipmentItem {\n");
            sb.Append("  SalesOrderId: ").Append(SalesOrderId).Append("\n");
            sb.Append("  SalesOrderItemId: ").Append(SalesOrderItemId).Append("\n");
            sb.Append("  Quantity: ").Append(Quantity).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Sku: ").Append(Sku).Append("\n");
            sb.Append("  ExternalOrderId: ").Append(ExternalOrderId).Append("\n");
            sb.Append("  ExternalOrderItemId: ").Append(ExternalOrderItemId).Append("\n");
            sb.Append("  Asin: ").Append(Asin).Append("\n");
            sb.Append("  OrderSourceCode: ").Append(OrderSourceCode).Append("\n");
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
            return this.Equals(input as ShipmentItem);
        }

        /// <summary>
        /// Returns true if ShipmentItem instances are equal
        /// </summary>
        /// <param name="input">Instance of ShipmentItem to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ShipmentItem input)
        {
            if (input == null)
                return false;

            return
                (
                    this.SalesOrderId == input.SalesOrderId ||
                    (this.SalesOrderId != null &&
                    this.SalesOrderId.Equals(input.SalesOrderId))
                ) &&
                (
                    this.SalesOrderItemId == input.SalesOrderItemId ||
                    (this.SalesOrderItemId != null &&
                    this.SalesOrderItemId.Equals(input.SalesOrderItemId))
                ) &&
                (
                    this.Quantity == input.Quantity ||
                    (this.Quantity != null &&
                    this.Quantity.Equals(input.Quantity))
                ) &&
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) &&
                (
                    this.Sku == input.Sku ||
                    (this.Sku != null &&
                    this.Sku.Equals(input.Sku))
                ) &&
                (
                    this.ExternalOrderId == input.ExternalOrderId ||
                    (this.ExternalOrderId != null &&
                    this.ExternalOrderId.Equals(input.ExternalOrderId))
                ) &&
                (
                    this.ExternalOrderItemId == input.ExternalOrderItemId ||
                    (this.ExternalOrderItemId != null &&
                    this.ExternalOrderItemId.Equals(input.ExternalOrderItemId))
                ) &&
                (
                    this.Asin == input.Asin ||
                    (this.Asin != null &&
                    this.Asin.Equals(input.Asin))
                ) &&
                (
                    this.OrderSourceCode == input.OrderSourceCode ||
                    (this.OrderSourceCode != null &&
                    this.OrderSourceCode.Equals(input.OrderSourceCode))
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
                if (this.SalesOrderId != null)
                    hashCode = hashCode * 59 + this.SalesOrderId.GetHashCode();
                if (this.SalesOrderItemId != null)
                    hashCode = hashCode * 59 + this.SalesOrderItemId.GetHashCode();
                if (this.Quantity != null)
                    hashCode = hashCode * 59 + this.Quantity.GetHashCode();
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.Sku != null)
                    hashCode = hashCode * 59 + this.Sku.GetHashCode();
                if (this.ExternalOrderId != null)
                    hashCode = hashCode * 59 + this.ExternalOrderId.GetHashCode();
                if (this.ExternalOrderItemId != null)
                    hashCode = hashCode * 59 + this.ExternalOrderItemId.GetHashCode();
                if (this.Asin != null)
                    hashCode = hashCode * 59 + this.Asin.GetHashCode();
                if (this.OrderSourceCode != null)
                    hashCode = hashCode * 59 + this.OrderSourceCode.GetHashCode();
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
