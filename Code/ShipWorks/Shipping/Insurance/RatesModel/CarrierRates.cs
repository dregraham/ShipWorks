using System;

namespace ShipWorks.Shipping.Insurance.RatesModel
{
	public class CarrierRates
	{
		public decimal FedExUpsMinimumCost { get; set; } = 2.25m;
		public decimal FedExUpsRatePer100 { get; set; } = 0.75m;
		public decimal OnTracMinimumCost { get; set; } = 0m;
		public decimal OnTracRatePer100 { get; set; } = 0.8m;
		public Usps Usps { get; set; } = new Usps();
	}
}