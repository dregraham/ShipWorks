using System;

namespace ShipWorks.Shipping.Insurance.RatesModel
{
	public class InsuranceRates
	{
		public ShipworksRates ShipworksRates { get; set; } = new ShipworksRates();
		public CarrierRates CarrierRates { get; set; } = new CarrierRates();
	}

	public class ShipworksRates
	{
		public decimal FedExUpsDomesticCost { get; set; } = 0.55m;
		public decimal FedExUpsInternationalCost { get; set; } = 0.55m;
		public decimal UspsDomesticCost { get; set; } = 0.75m;
		public decimal iParcelCost { get; set; } = 0.75m;
		public decimal UspsInternationalCost { get; set; } = 1.55m;
		public decimal OtherDomesticCost { get; set; } = 0.55m;
		public decimal OtherInternationalCost { get; set; } = 0.55m;
	}

	public class CarrierRates
	{
		public decimal FedExUpsMinimumCost { get; set; } = 2.25m;
		public decimal FedExUpsRatePer100 { get; set; } = 0.75m;
		public decimal OnTracMinimumCost { get; set; } = 0m;
		public decimal OnTracRatePer100 { get; set; } = 0.8m;
		public Usps Usps { get; set; } = new Usps();
	}

	public class Usps
	{
		public decimal CostFor50 { get; set; } = 1.8m;
		public decimal CostFor100 { get; set; } = 2.3m;
		public decimal CostFor200 { get; set; } = 2.85m;
		public decimal CostFor300 { get; set; } = 4.75m;
		public decimal RatePer100 { get; set; } = 1.05m;
		public decimal InternationalRatePer50 { get; set; } = 2.3m;
	}
}