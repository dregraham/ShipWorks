using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.Linq;
using System.Globalization;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.iParcel.Enums;

namespace ShipWorks.Shipping.Carriers.iParcel.Net.Ship
{
    /// <summary>
    /// This class builds up the "Packages" element that is included in an i-parcel request for creating a
    /// shipment and retrieving rates.
    /// </summary>
    public class iParcelPackageInfoElement : IiParcelRequestElement
    {
        private readonly ShipmentEntity shipment;
        private readonly ITokenProcessor tokenProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelPackageInfoElement" /> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="tokenProcessor">The token processor.</param>
        /// <param name="isDomestic">if set to <c>true</c> [is domestic].</param>
        /// <param name="usedForRates">if set to <c>true</c> [used for rates].</param>
        public iParcelPackageInfoElement(ShipmentEntity shipment, ITokenProcessor tokenProcessor, bool isDomestic, bool usedForRates)
        {
            this.shipment = shipment;
            this.tokenProcessor = tokenProcessor;
            IsDomesticShipment = isDomestic;
            UsedForRates = usedForRates;
        }

        /// <summary>
        /// Gets a value indicating whether this is for a domestic shipment.
        /// </summary>
        /// <value>
        /// <c>true</c> if this is for a domestic shipment; otherwise, <c>false</c>.
        /// </value>
        public bool IsDomesticShipment { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this is used to obtain rates.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [used for rates]; otherwise, <c>false</c>.
        /// </value>
        public bool UsedForRates { get; private set; }

        /// <summary>
        /// Builds the XML element for the validation information required for a shipping request.
        /// </summary>
        /// <returns>An XElement object.</returns>
        public XElement Build()
        {
            XElement element = new XElement("Packages");
            for (int packageIndex = 0; packageIndex < shipment.IParcel.Packages.Count; packageIndex++)
            {
                element.Add(GetPackagingInfo(packageIndex));
            }

            return element;
        }

        /// <summary>
        /// Gets the packaging info in the form of an XElement.
        /// </summary>
        /// <param name="packageIndex">Index of the current package of the shipment the XML is being generated for.</param>
        /// <returns>An XElement object.</returns>
        private XElement GetPackagingInfo(int packageIndex)
        {
            // May need to loop over each package in the shipment
            XElement element = new XElement("PackageInfo",
                                            new XElement("PackageNum", packageIndex + 1),
                                            GetGeneralPackageInfo(packageIndex),
                                            GetShipperInfo(packageIndex),
                                            GetConsigneeInfo(packageIndex),
                                            GetPackageInfoContents(packageIndex)
                );

            return element;
        }

        /// <summary>
        /// Gets the general package info in the form of an XElement.
        /// </summary>
        /// <param name="packageIndex">Index of the current package of the shipment the XML is being generated for.</param>
        /// <returns>An XElement object.</returns>
        private XElement GetGeneralPackageInfo(int packageIndex)
        {
            IParcelPackageEntity package = shipment.IParcel.Packages[packageIndex];

            // A value of zero can be used to obtain rates for all available service types
            string iParcelServiceValue = UsedForRates ? "0" : EnumHelper.GetApiValue((iParcelServiceType) shipment.IParcel.Service);

            // Note: the seller is required to upload his/her product catalog to i-parcel, so i-parcel has most of the 
            // item and customs stats which is why they are not supplied in the request
            XElement element = new XElement("General",
                new XElement("PaymentCurrency", "USD"),
                new XElement("DDP", shipment.IParcel.IsDeliveryDutyPaid ? "1" : "0"),
                new XElement("PackageNum", packageIndex + 1),
                new XElement("Service", iParcelServiceValue),
                new XElement("Reference", tokenProcessor.Process(shipment.IParcel.Reference, shipment)),
                new XElement("SalePrice", shipment.Order.OrderTotal.ToString(CultureInfo.InvariantCulture)),
                new XElement("Items", shipment.Order.OrderItems.Count),
                new XElement("WeightUnits", "LBS"),
                new XElement("Weight", package.Weight.ToString(CultureInfo.InvariantCulture)),
                new XElement("MeasureUnits", "IN"),
                new XElement("Length", package.DimsLength.ToString(CultureInfo.InvariantCulture)),
                new XElement("Width", package.DimsWidth.ToString(CultureInfo.InvariantCulture)),
                new XElement("Height", package.DimsHeight.ToString(CultureInfo.InvariantCulture)),
                new XElement("TrackingBarcode", string.Empty), // Not providing a value results in i-parcel generating the tracking number
                new XElement("GeneralDescription", string.Empty) // Node is required, but don't have to provide a value
                );

            if (!IsDomesticShipment)
            {
                // Include the customs value for international shipments
                element.Add(new XElement("CustomsValue", package.DeclaredValue.ToString(CultureInfo.InvariantCulture)));
            }

            if (package.Insurance && shipment.InsuranceProvider == (int)Insurance.InsuranceProvider.Carrier && package.InsuranceValue > 100)
            {
                // We're using i-parcel insurance, so send the actual insurance value to i-parcel
                element.Add(new XElement("InsuranceValue", package.InsuranceValue.ToString(CultureInfo.InvariantCulture)));
            }
            else
            {
                // We're not using i-parcel, so we don't want to add any additional insurance
                element.Add(new XElement("InsuranceValue", "0"));
            }

            return element;
        }

        /// <summary>
        /// Gets the shipper info in the form of an XElement.
        /// </summary>
        /// <param name="packageIndex">Index of the current package of the shipment the XML is being generated for.</param>
        /// <returns>An XElement object.</returns>
        private XElement GetShipperInfo(int packageIndex)
        {
            XElement element = new XElement("Shipper",
                                            new XElement("PackageNum", packageIndex + 1),
                                            new XElement("Name", string.Format("{0} {1}", shipment.OriginFirstName, shipment.OriginLastName)),
                                            new XElement("Address1", shipment.OriginStreet1),
                                            new XElement("Address2", shipment.OriginStreet2),
                                            new XElement("City", shipment.OriginCity),
                                            new XElement("StateProvince", shipment.OriginStateProvCode),
                                            new XElement("PostCode", shipment.OriginPostalCode),
                                            new XElement("CountryCode", shipment.AdjustedOriginCountryCode())
                                );
            return element;
        }

        /// <summary>
        /// Gets the consignee info in the form of an XElement.
        /// </summary>
        /// <param name="packageIndex">Index of the current package of the shipment the XML is being generated for.</param>
        /// <returns>An XElement object.</returns>
        private XElement GetConsigneeInfo(int packageIndex)
        {
            XElement element = new XElement("Consignee",
                                            new XElement("PackageNum", packageIndex + 1),
                                            new XElement("Name", string.Format("{0} {1}", shipment.ShipFirstName, shipment.ShipLastName)),
                                            new XElement("Address1", shipment.ShipStreet1),
                                            new XElement("Address2", shipment.ShipStreet2),
                                            new XElement("City", shipment.ShipCity),
                                            new XElement("StateProvince", shipment.AdjustedShipCountryCode().Equals("PR", StringComparison.OrdinalIgnoreCase) ? "PR" : shipment.ShipStateProvCode),
                                            new XElement("PostCode", shipment.ShipPostalCode),
                                            new XElement("CountryCode", shipment.AdjustedShipCountryCode()),
                                            new XElement("Phone", shipment.ShipPhone),
                                            new XElement("Email", shipment.ShipEmail),
                                            new XElement("TrackByEmail", shipment.IParcel.TrackByEmail ? "1" : "0"),
                                            new XElement("TrackBySMS", shipment.IParcel.TrackBySMS ? "1" : "0")
                                );
            return element;
        }

        /// <summary>
        /// Gets the package info contents in the form of an XElement.
        /// </summary>
        /// <param name="packageIndex">Index of the current package of the shipment the XML is being generated for.</param>
        /// <returns>An XElement object.</returns>
        private XElement GetPackageInfoContents(int packageIndex)
        {
            IParcelPackageEntity package = shipment.IParcel.Packages[packageIndex];

            XElement element = new XElement
            (
                "Contents",
                new XElement("PackageNum", packageIndex + 1)
            );

            // Parse the delimited sku/quantities string to get a list of items in the package
            iParcelSkuQuantityParser skuQuantityParser = new iParcelSkuQuantityParser(shipment, tokenProcessor);
            Dictionary<string, int> items = skuQuantityParser.Parse(package.SkuAndQuantities);
            
            foreach (string sku in items.Keys)
            {
                // Build the "Item" node for all the SKUs
                int quantity = items[sku];
                element.Add(GetItemContentInfo(packageIndex, sku, quantity));
            }

            return element;
        }


        /// <summary>
        /// Gets the package item content info in the form of an XElement.
        /// </summary>
        /// <param name="packageIndex">Index of the current package of the shipment the XML is being generated for.</param>
        /// <param name="sku">The SKU of an item included in the package.</param>
        /// <param name="quantity">The number of items matching the SKU provided that are included in the package.</param>
        /// <returns>An XElement object.</returns>
        private XElement GetItemContentInfo(int packageIndex, string sku, int quantity)
        {
                // Note: the seller is required to upload his/her product catalog to i-parcel, so i-parcel has most of the 
                // item and customs stats which is why they are not supplied in the request
                XElement element = new XElement("Item",
                                                new XElement("PackageNum", packageIndex + 1),

                                                // The Description node is required to have a value; we don't have this value since we're sending item data via
                                                // the delimited SKU field, so just send the sku (i-parcel confirmed this was acceptable)
                                                new XElement("Description", sku),

                                                // These nodes are required, but we don't have to provide a value
                                                new XElement("HSCode", string.Empty), 
                                                new XElement("CountryOfMan", string.Empty),

                                                //  We just send 0 for weight and value since ShipWorks does not have a built-in mechanism to 
                                                // track items in a package, but i-parcel will have the item weights and values on file due
                                                // to the catalog upload.
                                                new XElement("Weight", "0"),
                                                new XElement("Value", "0"),
                                                
                                                // The SKU and quantity are the only "real" values we need to send to i-parcel
                                                new XElement("SKU", sku),
                                                new XElement("Items", quantity.ToString(CultureInfo.InvariantCulture))
                    );

            return element;
        }
    }
}
