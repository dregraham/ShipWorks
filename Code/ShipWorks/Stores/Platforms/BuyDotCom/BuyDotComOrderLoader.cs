using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Import.Spreadsheet.OrderSchema;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Data.Import;

namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    /// <summary>
    /// Custom order loader for buy.com
    /// </summary>
    public class BuyDotComOrderLoader : GenericSpreadsheetOrderLoader
    {
        /// <summary>
        /// Load Buy.com specific item data
        /// </summary>
        protected override void LoadExtraOrderItemData(OrderItemEntity item, GenericSpreadsheetReader csv, IOrderElementFactory factory)
        {
            BuyDotComOrderItemEntity buyItem = (BuyDotComOrderItemEntity) item;

            buyItem.ReceiptItemID = csv.ReadField("BuyDotComItem.ReceiptItemID", "");
            buyItem.ListingID = csv.ReadField("BuyDotComItem.ListingID", 0);
            buyItem.Shipping = csv.ReadField("BuyDotComItem.Shipping", 0m);
            buyItem.Tax = csv.ReadField("BuyDotComItem.Tax", 0m);
            buyItem.Commission = csv.ReadField("BuyDotComItem.Commission", 0m);
            buyItem.ItemFee = csv.ReadField("BuyDotComItem.ItemFee", 0m);
        }

        /// <summary>
        /// Handle additional order data loading for buy.com
        /// </summary>
        protected override void OnLoadComplete(OrderEntity order, GenericSpreadsheetReader csv, IOrderElementFactory factory)
        {
            // Convert the shipping method to user-understandable
            order.RequestedShipping = TranslateRequestedShipmentMethod(order.RequestedShipping);

            // We need to add charges based on the total sum of buy.com shipping and tax charges
            if (order.IsNew)
            {
                decimal tax = order.OrderItems.Sum(oi => ((BuyDotComOrderItemEntity) oi).Tax);
                decimal shipping = order.OrderItems.Sum(oi => ((BuyDotComOrderItemEntity) oi).Shipping);

                // Create the charges on the order
                CreateCharge(order, "Tax", tax);
                CreateCharge(order, "Shipping", shipping);
            }
        }

        /// <summary>
        /// Deciphers Buycom shipping method.
        /// </summary>
        private string TranslateRequestedShipmentMethod(string shipmentMethod)
        {
            int numericShipMethod;
            string translatedShipmentMethod = shipmentMethod;
            if (int.TryParse(shipmentMethod, out numericShipMethod))
            {
                switch (numericShipMethod)
                {
                    case 1:
                        translatedShipmentMethod = "Standard";
                        break;
                    case 2:
                        translatedShipmentMethod = "Expedited";
                        break;
                    case 3:
                        translatedShipmentMethod = "Two Day";
                        break;
                    case 4:
                        translatedShipmentMethod = "One Day";
                        break;
                    case 5:
                        translatedShipmentMethod = "Same Day";
                        break;
                    default:
                        break;
                }
            }

            return translatedShipmentMethod;
        }
    }
}
