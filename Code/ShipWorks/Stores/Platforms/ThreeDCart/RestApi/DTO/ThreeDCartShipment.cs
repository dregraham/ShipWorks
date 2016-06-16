using System.Reflection;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class ThreeDCartShipment
    {
        public long OrderID { get; set; }

        public long ShipmentID { get; set; }

        public string ShipmentLastUpdate { get; set; }

        public int ShipmentOrderStatus { get; set; }

        public string ShipmentAddress { get; set; }

        public string ShipmentAddress2 { get; set; }

        public string ShipmentCity { get; set; }

        public string ShipmentCompany { get; set; }

        public decimal ShipmentCost { get;  set; }

        public string ShipmentCountry { get; set; }

        public string ShipmentEmail { get; set; }

        public string ShipmentFirstName { get; set; }

        public string ShipmentLastName { get; set; }

        public string ShipmentMethodName { get; set; }

        public string ShipmentShippedDate { get; set; }

        public string ShipmentPhone { get; set; }

        public string ShipmentState { get; set; }

        public string ShipmentZipCode { get; set; }

        public double ShipmentWeight { get; set; }

        public string ShipmentTrackingCode { get; set; }

        public bool ShouldSerializeShipmentCost()
        {
            return false;
        }

       public bool ShouldSerializeShipmentWeight()
       {
           return ShipmentWeight > 0;
       }
    }
}