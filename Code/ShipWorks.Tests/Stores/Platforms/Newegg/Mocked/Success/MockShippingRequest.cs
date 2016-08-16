using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping.Response;

namespace ShipWorks.Tests.Stores.Newegg.Mocked.Success
{
    public class MockShippingRequest : IShippingRequest
    {
        
        public ShippingResult Ship(Shipment shipment)
        {
            ShippingResult result = new ShippingResult();

            result.IsSuccessful = true;

            result.PackageSummary.TotalPackages = 1;
            result.PackageSummary.SuccessCount = 1;

            result.Detail.SellerId = shipment.Header.SellerId;
            result.Detail.OrderStatus = "Shipped";
            result.Detail.OrderNumber = shipment.Header.OrderNumber;

            foreach (ShipmentPackage packageInShipment in shipment.Packages)
            {
                ShipmentPackage package = new ShipmentPackage();
                package.IsSuccessfullyProcessed = true;
                package.ShipCarrier = packageInShipment.ShipCarrier;
                package.ShipService = packageInShipment.ShipService;
                package.TrackingNumber = packageInShipment.TrackingNumber;

                foreach (ShippedItem item in packageInShipment.Items)
                {
                    package.Items.Add(new ShippedItem { SellerPartNumber = item.SellerPartNumber });
                }
            }

            return result;
        }
    }
}
