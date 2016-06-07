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
	            new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number", true),
	            new ShipWorksOdbcMappableField(OrderFields.OrderDate, ShipWorksOdbcMappableField.OrderDateAndTimeDisplayName),
	            new ShipWorksOdbcMappableField(OrderFields.OrderDate, ShipWorksOdbcMappableField.OrderDateDisplayName),
	            new ShipWorksOdbcMappableField(OrderFields.OrderDate, ShipWorksOdbcMappableField.OrderTimeDisplayName),
	            new ShipWorksOdbcMappableField(OrderFields.LocalStatus, "Local Status"),
	            new ShipWorksOdbcMappableField(OrderFields.OnlineStatus, "Store Status"),
	            new ShipWorksOdbcMappableField(OrderFields.RequestedShipping, "Requested Shipping"),
	            new ShipWorksOdbcMappableField(OrderFields.CustomerID, "Customer Number"),
	            new ShipWorksOdbcMappableField(NoteFields.Text, "Note (Internal)"),
	            new ShipWorksOdbcMappableField(NoteFields.Text, "Note (Public)"),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, "Shipping Amount"),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, "Handling Amount"),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, "Discount Amount"),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, "Insurance Amount"),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, "Other Amount"),
	            new ShipWorksOdbcMappableField(OrderChargeFields.Amount, "Tax Amount"),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, "Payment Method"),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, "Payment Reference"),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, "Credit Card Type"),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, "Credit Card Number"),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, "Credit Card Expiration"),
	            new ShipWorksOdbcMappableField(OrderPaymentDetailFields.Value, "Credit Card Name")
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
                new ShipWorksOdbcMappableField(OrderItemFields.Name, "Name"),
                new ShipWorksOdbcMappableField(OrderItemFields.Code, "Code"),
                new ShipWorksOdbcMappableField(OrderItemFields.SKU, "SKU"),
                new ShipWorksOdbcMappableField(OrderItemFields.Quantity, ShipWorksOdbcMappableField.QuantityDisplayName),
                new ShipWorksOdbcMappableField(OrderItemFields.UnitPrice, ShipWorksOdbcMappableField.UnitPriceDisplayName),
                new ShipWorksOdbcMappableField(OrderItemFields.UnitPrice, ShipWorksOdbcMappableField.TotalPriceDisplayName),
                new ShipWorksOdbcMappableField(OrderItemFields.Weight, ShipWorksOdbcMappableField.UnitWeightDisplayName),
                new ShipWorksOdbcMappableField(OrderItemFields.Weight, ShipWorksOdbcMappableField.TotalWeightDisplayName),
                new ShipWorksOdbcMappableField(OrderItemFields.LocalStatus, "Status"),
                new ShipWorksOdbcMappableField(OrderItemFields.Description, "Description"),
                new ShipWorksOdbcMappableField(OrderItemFields.Location, "Location"),
                new ShipWorksOdbcMappableField(OrderItemFields.UnitCost, ShipWorksOdbcMappableField.UnitCostDisplayName),
                new ShipWorksOdbcMappableField(OrderItemFields.UnitCost, ShipWorksOdbcMappableField.TotalCostDisplayName),
                new ShipWorksOdbcMappableField(OrderItemFields.Image, "Image URL"),
                new ShipWorksOdbcMappableField(OrderItemFields.Thumbnail, "Thumbnail URL"),
                new ShipWorksOdbcMappableField(OrderItemFields.UPC, "UPC"),
                new ShipWorksOdbcMappableField(OrderItemFields.ISBN, "ISBN"),
                new ShipWorksOdbcMappableField(OrderItemAttributeFields.Name, "Attribute Name")
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
