using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    public partial class UpsShipmentCharacteristicsControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsShipmentCharacteristicsControl" /> class.
        /// </summary>
        public UpsShipmentCharacteristicsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Saves to request.
        /// </summary>
        public void SaveToRequest(OpenAccountRequest request)
        {
            var shipmentCharacteristics = new List<ShipmentCharacteristicsType>();

            AddShipmentCharacteristicIfNeeded(shipmentCharacteristics, "01", ground.NumericValue);
            AddShipmentCharacteristicIfNeeded(shipmentCharacteristics, "02", air.NumericValue);
            AddShipmentCharacteristicIfNeeded(shipmentCharacteristics, "03", international.NumericValue);

            // at least one field has a value and it is not 0.
            if (shipmentCharacteristics.Count == 0)
            {   
                throw new UpsOpenAccountException("UPS requires that there is at least one Ground, Air, or International shipment to create an account.");
            }
            
            request.ShipmentCharacteristics = shipmentCharacteristics.ToArray();
        }

        /// <summary>
        /// Adds the shipment characteristic if needed.
        /// </summary>
        private void AddShipmentCharacteristicIfNeeded(ICollection<ShipmentCharacteristicsType> shipmentCharacteristics, string code, int? quantity)
        {
            if (!quantity.HasValue || quantity.Value == 0)
            {
                return;
            }

            // The API only allows values up to 99
            quantity = Math.Min(quantity.Value, 99);

            shipmentCharacteristics.Add(new ShipmentCharacteristicsType
            {
                Code = code,
                Quantity = quantity.Value.ToString()
            });
        }
    }
}
