using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Factory for creating Odbc field maps
    /// </summary>
    public class OdbcFieldMapFactory : IOdbcFieldMapFactory
	{
        private readonly IOdbcFieldMapIOFactory ioFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcFieldMapFactory"/> class.
        /// </summary>
        public OdbcFieldMapFactory(IOdbcFieldMapIOFactory ioFactory)
        {
            this.ioFactory = ioFactory;
        }

        /// <summary>
        /// Creates the order field map.
        /// </summary>
        public IOdbcFieldMap CreateOrderFieldMap(IOdbcFieldMap storeFieldMap)
        {
            return CreateMapWithMappedFields(CreateShipWorksOrderFields(), storeFieldMap);
		}

        /// <summary>
        /// Creates the ShipWorks order mappable fields.
        /// </summary>
        private IEnumerable<ShipWorksOdbcMappableField> CreateShipWorksOrderFields()
	    {
	        List<ShipWorksOdbcMappableField> fields = new List<ShipWorksOdbcMappableField>
	        {
	            new ShipWorksOdbcMappableField(OrderFields.OrderNumber, OdbcOrderFieldDescription.Number, true, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(OrderFields.OrderDate, OdbcOrderFieldDescription.DateAndTime, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.OnlineLastModified, OdbcOrderFieldDescription.LastModifiedDateAndTime, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.LocalStatus, OdbcOrderFieldDescription.LocalStatus, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(OrderFields.OnlineStatus, OdbcOrderFieldDescription.OnlineStatus, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(OrderFields.RequestedShipping, OdbcOrderFieldDescription.RequestedShipping, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(OrderFields.OnlineCustomerID, OdbcOrderFieldDescription.CustomerID, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(NoteFields.Text, OdbcOrderFieldDescription.NoteInternal, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(NoteFields.Text, OdbcOrderFieldDescription.NotePublic, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, OdbcOrderFieldDescription.ChargeShipping, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, OdbcOrderFieldDescription.ChargeHandling, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, OdbcOrderFieldDescription.ChargeDiscount, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, OdbcOrderFieldDescription.ChargeInsurance, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, OdbcOrderFieldDescription.ChargeOther, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, OdbcOrderFieldDescription.ChargeTax, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, OdbcOrderFieldDescription.PaymentMethod, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, OdbcOrderFieldDescription.PaymentReference, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, OdbcOrderFieldDescription.PaymentCCType, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, OdbcOrderFieldDescription.PaymentCCNumber, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, OdbcOrderFieldDescription.PaymentCCExpiration, OdbcFieldValueResolutionStrategy.Default),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, OdbcOrderFieldDescription.PaymentCCName, OdbcFieldValueResolutionStrategy.Default)
	        };

	        return fields;
	    }

        /// <summary>
        /// Creates the order item field map.
        /// </summary>
        public IOdbcFieldMap CreateOrderItemFieldMap(IOdbcFieldMap storeFieldMap, int index)
        {
            return CreateMapWithMappedFields(CreateShipWorksOrderItemFields(), storeFieldMap, index);
        }

        /// <summary>
        /// Creates the ShipWorks order item mappable fields.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ShipWorksOdbcMappableField> CreateShipWorksOrderItemFields()
	    {
	        List<ShipWorksOdbcMappableField> fields = new List<ShipWorksOdbcMappableField>()
	        {
                new ShipWorksOdbcMappableField(OrderItemFields.Name, OdbcOrderFieldDescription.ItemName, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderItemFields.Code, OdbcOrderFieldDescription.ItemCode, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderItemFields.SKU, OdbcOrderFieldDescription.ItemSKU, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderItemFields.Quantity, OdbcOrderFieldDescription.ItemQuantity, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderItemFields.UnitPrice, OdbcOrderFieldDescription.ItemUnitPrice, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderItemFields.UnitPrice, OdbcOrderFieldDescription.ItemTotalPrice, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderItemFields.Weight, OdbcOrderFieldDescription.ItemUnitWeight, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderItemFields.Weight, OdbcOrderFieldDescription.ItemTotalWeight, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderItemFields.LocalStatus, OdbcOrderFieldDescription.ItemLocalStatus, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderItemFields.Description, OdbcOrderFieldDescription.ItemDescription, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderItemFields.Location, OdbcOrderFieldDescription.ItemLocation, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderItemFields.UnitCost, OdbcOrderFieldDescription.ItemUnitCost, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderItemFields.UnitCost, OdbcOrderFieldDescription.ItemTotalCost, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderItemFields.Image, OdbcOrderFieldDescription.ItemImage, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderItemFields.Thumbnail, OdbcOrderFieldDescription.ItemThumbnail, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderItemFields.UPC, OdbcOrderFieldDescription.ItemUPC, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderItemFields.ISBN, OdbcOrderFieldDescription.ItemISBN, OdbcFieldValueResolutionStrategy.Default),
            };

	        return fields;
	    }
        
        /// <summary>
        /// Creates the address field map.
        /// </summary>
        /// <param name="storeFieldMap"></param>
        public IOdbcFieldMap CreateAddressFieldMap(IOdbcFieldMap storeFieldMap)
        {
            return CreateMapWithMappedFields(CreateShipWorksAddressFields(), storeFieldMap);
        }

        /// <summary>
        /// Creates the ShipWorks address mappable fields.
        /// </summary>
        private IEnumerable<ShipWorksOdbcMappableField> CreateShipWorksAddressFields()
	    {
            List<ShipWorksOdbcMappableField> addressFields = new List<ShipWorksOdbcMappableField>()
            {
                new ShipWorksOdbcMappableField(OrderFields.BillFirstName, "Bill First Name", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.BillMiddleName, "Bill Middle Name", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.BillLastName, "Bill Last Name", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.BillUnparsedName, "Bill Full Name", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.BillCompany, "Bill Company", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.BillStreet1, "Bill Address 1", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.BillStreet2, "Bill Address 2", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.BillStreet3, "Bill Address 3", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.BillCity, "Bill City", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.BillStateProvCode, "Bill State/Province", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.BillPostalCode, "Bill Postal Code", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.BillCountryCode, "Bill Country", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.BillEmail, "Bill Email", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.BillPhone, "Bill Phone", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.BillFax, "Bill Fax", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.BillWebsite, "Bill Website", OdbcFieldValueResolutionStrategy.Default),

                new ShipWorksOdbcMappableField(OrderFields.ShipFirstName, "Ship First Name", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.ShipMiddleName, "Ship Middle Name", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.ShipLastName, "Ship Last Name", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.ShipUnparsedName, "Ship Full Name", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.ShipCompany, "Ship Company", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.ShipStreet1, "Ship Address 1", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.ShipStreet2, "Ship Address 2", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.ShipStreet3, "Ship Address 3", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.ShipCity, "Ship City", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.ShipStateProvCode, "Ship State/Province", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.ShipPostalCode, "Ship Postal Code", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.ShipCountryCode, "Ship Country", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.ShipEmail, "Ship Email", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.ShipPhone, "Ship Phone", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.ShipFax, "Ship Fax", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.ShipWebsite, "Ship Website", OdbcFieldValueResolutionStrategy.Default),
            };

            return addressFields;
        }

        /// <summary>
        /// Creates the empty map.
        /// </summary>
        private IOdbcFieldMap CreateMapWithMappedFields(IEnumerable<ShipWorksOdbcMappableField> shipWorksOdbcMappableFields, IOdbcFieldMap storeFieldMap, int index = 0)
        {
            IOdbcFieldMap map = new OdbcFieldMap(ioFactory);

            foreach (ShipWorksOdbcMappableField entry in shipWorksOdbcMappableFields)
            {
                // If StoreFieldMap isn't null and has a matching entry, use the entries external field, else create a new one.
                IExternalOdbcMappableField externalEntry =
                    storeFieldMap?.Entries.FirstOrDefault(entryFromStoreMap => IsMatchingField(entry, entryFromStoreMap, index))?.ExternalField ??
                    new ExternalOdbcMappableField(null);

                map.AddEntry(new OdbcFieldMapEntry(entry, externalEntry, index));
            }

            return map;
        }

        /// <summary>
        /// Is matching field
        /// </summary>
        private static bool IsMatchingField(ShipWorksOdbcMappableField fieldFromNewMap, IOdbcFieldMapEntry entryFromStoreMap, int index)
        {
            return entryFromStoreMap.ShipWorksField.Name == fieldFromNewMap.Name &&
                   entryFromStoreMap.ShipWorksField.ContainingObjectName == fieldFromNewMap.ContainingObjectName &&
                   entryFromStoreMap.Index == index;
        }

        /// <summary>
        /// Creates the shipment field map.
        /// </summary>
        public IOdbcFieldMap CreateShipmentFieldMap()
        {
            return CreateMapWithMappedFields(CreateShipmentFields(), null);
        }

        /// <summary>
        /// Creates the shipment fields.
        /// </summary>
        private IEnumerable<ShipWorksOdbcMappableField> CreateShipmentFields()
        {
            return new List<ShipWorksOdbcMappableField>
            {
                new ShipWorksOdbcMappableField(OrderFields.OrderNumber, OdbcOrderFieldDescription.Number, true, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.TrackingNumber, OdbcShipmentFieldDescription.TrackingNumber, true, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.ShipDate, OdbcShipmentFieldDescription.ShipDate, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(OrderFields.LocalStatus, OdbcOrderFieldDescription.LocalStatus, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.TotalWeight, OdbcShipmentFieldDescription.TotalWeight, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.ShipmentCost, OdbcShipmentFieldDescription.ShipmentCost, OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField("", "", EnumHelper.GetDescription(OdbcFieldValueResolutionStrategy.ShippingService), OdbcFieldValueResolutionStrategy.ShippingService),
                new ShipWorksOdbcMappableField("", "", EnumHelper.GetDescription(OdbcFieldValueResolutionStrategy.ShippingCarrier), OdbcFieldValueResolutionStrategy.ShippingCarrier)
                // Todo: Still need to add Packaging, # of Packages, Length, Width, Height,
                // Todo: Insured Value & Insurance Fee
            };
        }

        /// <summary>
        /// Creates the shipto address field map.
        /// </summary>
        public IOdbcFieldMap CreateShiptoAddressFieldMap()
        {
            return CreateMapWithMappedFields(CreateShipToAddressFields(), null);
        }

        /// <summary>
        /// Creates the ship to address fields.
        /// </summary>
        private IEnumerable<ShipWorksOdbcMappableField> CreateShipToAddressFields()
        {
            return new List<ShipWorksOdbcMappableField>
            {
                new ShipWorksOdbcMappableField(ShipmentFields.ShipFirstName, "Ship First Name", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.ShipMiddleName, "Ship Middle Name", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.ShipLastName, "Ship Last Name", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.ShipUnparsedName, "Ship Full Name", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.ShipCompany, "Ship Company", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.ShipStreet1, "Ship Address 1", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.ShipStreet2, "Ship Address 2", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.ShipStreet3, "Ship Address 3", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.ShipCity, "Ship City", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.ShipStateProvCode, "Ship State/Province", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.ShipPostalCode, "Ship Postal Code", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.ShipCountryCode, "Ship Country", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.ShipEmail, "Ship Email", OdbcFieldValueResolutionStrategy.Default),
                new ShipWorksOdbcMappableField(ShipmentFields.ShipPhone, "Ship Phone", OdbcFieldValueResolutionStrategy.Default)
            };
        }

        /// <summary>
        /// Creates a new field map from the entries
        /// </summary>
        public OdbcFieldMap CreateFieldMapFrom(IEnumerable<IOdbcFieldMapEntry> entries)
		{
            OdbcFieldMap masterMap = new OdbcFieldMap(ioFactory);

            foreach (IOdbcFieldMapEntry entry in entries
                .Where(entry => !string.IsNullOrWhiteSpace(entry.ExternalField.Column?.Name) &&
                !entry.ExternalField.Column.Name.Equals("(None)", StringComparison.InvariantCulture)))
            {
                masterMap.AddEntry(entry);
            }

            return masterMap;
		}

        /// <summary>
        /// Gets a map with the specified number of attributes with item numbers started at the specified start number.
        /// </summary>
        public IOdbcFieldMap GetAttributeRangeFieldMap(int startAttributeNumber, int numberOfAttributes, int itemIndex)
        {
            // Generate attribute numbers for new attributes to add and add them.
            IEnumerable<ShipWorksOdbcMappableField> attributeFieldMapEntries = Enumerable.Range(startAttributeNumber, numberOfAttributes)
                .Select(
                    attributeNumber => new ShipWorksOdbcMappableField(OrderItemAttributeFields.Name, $"Attribute {attributeNumber}", OdbcFieldValueResolutionStrategy.Default));

            return CreateMapWithMappedFields(attributeFieldMapEntries, null, itemIndex);
        }
	}
}
