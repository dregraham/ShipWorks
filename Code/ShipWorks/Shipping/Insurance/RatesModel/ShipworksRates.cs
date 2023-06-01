using System;

namespace ShipWorks.Shipping.Insurance.RatesModel
{
	public class ShipworksRates
	{
		public decimal FedExUpsCost { get; set; } = 0.55m;
		public decimal UspsDomesticCost { get; set; } = 0.75m;
		public decimal iParcelCost { get; set; } = 0.75m;
		public decimal UspsInternationalCost { get; set; } = 1.55m;
		public decimal OtherCost { get; set; } = 0.55m;
	}
}