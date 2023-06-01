using System;

namespace ShipWorks.Shipping.Insurance.RatesModel
{
	public class InsuranceRates
	{
		public ShipworksRates ShipworksRates { get; set; } = new ShipworksRates();
		public CarrierRates CarrierRates { get; set; } = new CarrierRates();
	}
}