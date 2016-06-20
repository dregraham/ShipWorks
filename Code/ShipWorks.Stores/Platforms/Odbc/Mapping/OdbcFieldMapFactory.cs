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
        /// <param name="ioFactory">The io factory.</param>
        public OdbcFieldMapFactory(IOdbcFieldMapIOFactory ioFactory)
	    {
	        this.ioFactory = ioFactory;
	    }

        /// <summary>
        /// Creates the order field map.
        /// </summary>
        public OdbcFieldMap CreateOrderFieldMap()
        {
            OdbcFieldMap orderMap = new OdbcFieldMap(ioFactory);

            foreach (ShipWorksOdbcMappableField orderField in CreateShipWorksOrderFields())
	        {
	            orderMap.AddEntry(new OdbcFieldMapEntry(orderField, new ExternalOdbcMappableField(null, null)));
	        }

	        return orderMap;
		}

        /// <summary>
        /// Creates the ShipWorks order mappable fields.
        /// </summary>
        private IEnumerable<ShipWorksOdbcMappableField> CreateShipWorksOrderFields()
	    {
	        List<ShipWorksOdbcMappableField> fields = new List<ShipWorksOdbcMappableField>
	        {
	            new ShipWorksOdbcMappableField(OrderFields.OrderNumber, OdbcOrderFieldDescription.Number, true),
	            new ShipWorksOdbcMappableField(OrderFields.OrderDate, OdbcOrderFieldDescription.DateAndTime),
                new ShipWorksOdbcMappableField(OrderFields.OnlineLastModified, OdbcOrderFieldDescription.LastModifiedDateAndTime),
                new ShipWorksOdbcMappableField(OrderFields.LocalStatus, OdbcOrderFieldDescription.LocalStatus),
	            new ShipWorksOdbcMappableField(OrderFields.OnlineStatus, OdbcOrderFieldDescription.OnlineStatus),
	            new ShipWorksOdbcMappableField(OrderFields.RequestedShipping, OdbcOrderFieldDescription.RequestedShipping),
	            new ShipWorksOdbcMappableField(OrderFields.CustomerID, OdbcOrderFieldDescription.CustomerID),
	            new ShipWorksOdbcMappableField(NoteFields.Text, OdbcOrderFieldDescription.NoteInternal),
	            new ShipWorksOdbcMappableField(NoteFields.Text, OdbcOrderFieldDescription.NotePublic),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, OdbcOrderFieldDescription.ChargeShipping),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, OdbcOrderFieldDescription.ChargeHandling),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, OdbcOrderFieldDescription.ChargeDiscount),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, OdbcOrderFieldDescription.ChargeInsurance),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, OdbcOrderFieldDescription.ChargeOther),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, OdbcOrderFieldDescription.ChargeTax),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, OdbcOrderFieldDescription.PaymentMethod),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, OdbcOrderFieldDescription.PaymentReference),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, OdbcOrderFieldDescription.PaymentCCType),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, OdbcOrderFieldDescription.PaymentCCNumber),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, OdbcOrderFieldDescription.PaymentCCExpiration),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, OdbcOrderFieldDescription.PaymentCCName)
	        };

	        return fields;
	    }

        /// <summary>
        /// Creates the order item field map.
        /// </summary>
        public OdbcFieldMap CreateOrderItemFieldMap()
        {
            OdbcFieldMap itemMap = new OdbcFieldMap(ioFactory);

            foreach (ShipWorksOdbcMappableField entry in CreateShipWorksOrderItemFields())
            {
                itemMap.AddEntry(new OdbcFieldMapEntry(entry, new ExternalOdbcMappableField(null, null)));
            }

            return itemMap;
        }

        /// <summary>
        /// Creates the ShipWorks order item mappable fields.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ShipWorksOdbcMappableField> CreateShipWorksOrderItemFields()
	    {
	        List<ShipWorksOdbcMappableField> fields = new List<ShipWorksOdbcMappableField>()
	        {
                new ShipWorksOdbcMappableField(OrderItemFields.Name, OdbcOrderFieldDescription.ItemName),
                new ShipWorksOdbcMappableField(OrderItemFields.Code, OdbcOrderFieldDescription.ItemCode),
                new ShipWorksOdbcMappableField(OrderItemFields.SKU, OdbcOrderFieldDescription.ItemSKU),
                new ShipWorksOdbcMappableField(OrderItemFields.Quantity, OdbcOrderFieldDescription.ItemQuantity),
                new ShipWorksOdbcMappableField(OrderItemFields.UnitPrice, OdbcOrderFieldDescription.ItemUnitPrice),
                new ShipWorksOdbcMappableField(OrderItemFields.UnitPrice, OdbcOrderFieldDescription.ItemTotalCost),
                new ShipWorksOdbcMappableField(OrderItemFields.Weight, OdbcOrderFieldDescription.ItemUnitWeight),
                new ShipWorksOdbcMappableField(OrderItemFields.Weight, OdbcOrderFieldDescription.ItemTotalWeight),
                new ShipWorksOdbcMappableField(OrderItemFields.LocalStatus, OdbcOrderFieldDescription.ItemLocalStatus),
                new ShipWorksOdbcMappableField(OrderItemFields.Description, OdbcOrderFieldDescription.ItemDescription),
                new ShipWorksOdbcMappableField(OrderItemFields.Location, OdbcOrderFieldDescription.ItemLocation),
                new ShipWorksOdbcMappableField(OrderItemFields.UnitCost, OdbcOrderFieldDescription.ItemUnitCost),
                new ShipWorksOdbcMappableField(OrderItemFields.UnitCost, OdbcOrderFieldDescription.ItemTotalCost),
                new ShipWorksOdbcMappableField(OrderItemFields.Image, OdbcOrderFieldDescription.ItemImage),
                new ShipWorksOdbcMappableField(OrderItemFields.Thumbnail, OdbcOrderFieldDescription.ItemThumbnail),
                new ShipWorksOdbcMappableField(OrderItemFields.UPC, OdbcOrderFieldDescription.ItemUPC),
                new ShipWorksOdbcMappableField(OrderItemFields.ISBN, OdbcOrderFieldDescription.ItemISBN),
                new ShipWorksOdbcMappableField(OrderItemAttributeFields.Name, OdbcOrderFieldDescription.ItemAttributeName)
            };

	        return fields;
	    }


        /// <summary>
        /// Creates the address field map.
        /// </summary>
        public OdbcFieldMap CreateAddressFieldMap()
        {
            OdbcFieldMap addressMap = new OdbcFieldMap(ioFactory);

            foreach (ShipWorksOdbcMappableField entry in CreateShipWorksAddressFields())
            {
                addressMap.AddEntry(new OdbcFieldMapEntry(entry, new ExternalOdbcMappableField(null, null)));
            }

            return addressMap;
        }

        /// <summary>
        /// Creates the ShipWorks address mappable fields.
        /// </summary>
        private IEnumerable<ShipWorksOdbcMappableField> CreateShipWorksAddressFields()
	    {
            List<ShipWorksOdbcMappableField> fields = new List<ShipWorksOdbcMappableField>()
            {
                new ShipWorksOdbcMappableField(OrderFields.BillFirstName, "Bill First Name"),
                new ShipWorksOdbcMappableField(OrderFields.BillMiddleName, "Bill Middle Name"),
                new ShipWorksOdbcMappableField(OrderFields.BillLastName, "Bill Last Name"),
                new ShipWorksOdbcMappableField(OrderFields.BillUnparsedName, "Bill Full Name"),
                new ShipWorksOdbcMappableField(OrderFields.BillCompany, "Bill Company"),
                new ShipWorksOdbcMappableField(OrderFields.BillStreet1, "Bill Address 1"),
                new ShipWorksOdbcMappableField(OrderFields.BillStreet2, "Bill Address 2"),
                new ShipWorksOdbcMappableField(OrderFields.BillStreet3, "Bill Address 3"),
                new ShipWorksOdbcMappableField(OrderFields.BillCity, "Bill City"),
                new ShipWorksOdbcMappableField(OrderFields.BillStateProvCode, "Bill State/Province"),
                new ShipWorksOdbcMappableField(OrderFields.BillPostalCode, "Bill Postal Code"),
                new ShipWorksOdbcMappableField(OrderFields.BillCountryCode, "Bill Country"),
                new ShipWorksOdbcMappableField(OrderFields.BillEmail, "Bill Email"),
                new ShipWorksOdbcMappableField(OrderFields.BillPhone, "Bill Phone"),
                new ShipWorksOdbcMappableField(OrderFields.BillFax, "Bill Fax"),
                new ShipWorksOdbcMappableField(OrderFields.BillWebsite, "Bill Website"),

                new ShipWorksOdbcMappableField(OrderFields.ShipFirstName, "Ship First Name"),
                new ShipWorksOdbcMappableField(OrderFields.ShipMiddleName, "Ship Middle Name"),
                new ShipWorksOdbcMappableField(OrderFields.ShipLastName, "Ship Last Name"),
                new ShipWorksOdbcMappableField(OrderFields.ShipUnparsedName, "Ship Full Name"),
                new ShipWorksOdbcMappableField(OrderFields.ShipCompany, "Ship Company"),
                new ShipWorksOdbcMappableField(OrderFields.ShipStreet1, "Ship Address 1"),
                new ShipWorksOdbcMappableField(OrderFields.ShipStreet2, "Ship Address 2"),
                new ShipWorksOdbcMappableField(OrderFields.ShipStreet3, "Ship Address 3"),
                new ShipWorksOdbcMappableField(OrderFields.ShipCity, "Ship City"),
                new ShipWorksOdbcMappableField(OrderFields.ShipStateProvCode, "Ship State/Province"),
                new ShipWorksOdbcMappableField(OrderFields.ShipPostalCode, "Ship Postal Code"),
                new ShipWorksOdbcMappableField(OrderFields.ShipCountryCode, "Ship Country"),
                new ShipWorksOdbcMappableField(OrderFields.ShipEmail, "Ship Email"),
                new ShipWorksOdbcMappableField(OrderFields.ShipPhone, "Ship Phone"),
                new ShipWorksOdbcMappableField(OrderFields.ShipFax, "Ship Fax"),
                new ShipWorksOdbcMappableField(OrderFields.ShipWebsite, "Ship Website"),
            };

            return fields;
        }

        /// <summary>
        /// Creates a new field map from a list of field maps
        /// </summary>
        public OdbcFieldMap CreateFieldMapFrom(IEnumerable<OdbcFieldMap> maps)
		{
            OdbcFieldMap masterMap = new OdbcFieldMap(ioFactory);

            foreach (IOdbcFieldMapEntry entry in maps.SelectMany(map => map.Entries)
                .Where(entry => !string.IsNullOrWhiteSpace(entry.ExternalField.Column?.Name) &&
                !entry.ExternalField.Column.Name.Equals("(None)", StringComparison.InvariantCulture)))
            {
                masterMap.AddEntry(entry);
            }

            return masterMap;
		}
	}
}
