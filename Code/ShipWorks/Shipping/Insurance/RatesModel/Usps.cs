using System;

namespace ShipWorks.Shipping.Insurance.RatesModel
{
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