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
                throw new UpsOpenAccountException("Ground, Air, or International must have positive value.");
            }
            
            request.ShipmentCharacteristics = shipmentCharacteristics.ToArray();
        }

        /// <summary>
        /// Adds the shipment characteristic if needed.
        /// </summary>
        /// <exception cref="UpsOpenAccountException">Quantity for any method cannot exceed 99.</exception>
        private void AddShipmentCharacteristicIfNeeded(ICollection<ShipmentCharacteristicsType> shipmentCharacteristics, string code, int? quantity)
        {
            if (!quantity.HasValue || quantity.Value == 0)
            {
                return;
            }

            if (quantity.Value >= 100)
            {
                throw new UpsOpenAccountException("Quantity for any method cannot exceed 99.");
            }

            shipmentCharacteristics.Add(new ShipmentCharacteristicsType
            {
                Code = code,
                Quantity = quantity.Value.ToString()
            });
        }
    }
}
