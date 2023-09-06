using System;
using System.Reflection;

namespace ShipWorks.Shipping.Insurance.RatesModel
{
	[Obfuscation(Exclude = true, ApplyToMembers = true)]
	public class InsuranceRates
	{
		public ShipworksRates ShipworksRates { get; set; } = new ShipworksRates();
		public CarrierRates CarrierRates { get; set; } = new CarrierRates();
	}

	[Obfuscation(Exclude = true, ApplyToMembers = true)]
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

	[Obfuscation(Exclude = true, ApplyToMembers = true)]
	public class CarrierRates
	{
		public decimal FedExUpsMinimumCost { get; set; } = 2.25m;
		public decimal FedExUpsRatePer100 { get; set; } = 0.75m;
		public decimal OnTracMinimumCost { get; set; } = 0m;
		public decimal OnTracRatePer100 { get; set; } = 0.8m;
		public Usps Usps { get; set; } = new Usps();
	}

	[Obfuscation(Exclude = true, ApplyToMembers = true)]
	public class Usps
	{
		public decimal CostFor50 { get; set; } = 2.7m;
		public decimal CostFor100 { get; set; } = 3.45m;
		public decimal CostFor200 { get; set; } = 4.55m;
		public decimal CostFor300 { get; set; } = 5.95m;
		public decimal CostFor400 { get; set; } = 7.5m;
		public decimal CostFor500 { get; set; } = 9.05m;
		public decimal CostFor600 { get; set; } = 12.15m;
		public decimal RatePer100 { get; set; } = 1.85m;
		public decimal InternationalCostFor300 { get; set; } = 12.75m;
		public decimal InternationalRatePer100 { get; set; } = 3.4m;
	}
}