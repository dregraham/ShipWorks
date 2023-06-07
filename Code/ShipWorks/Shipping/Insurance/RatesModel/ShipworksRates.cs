using System;

namespace ShipWorks.Shipping.Insurance.RatesModel
{
	public class ShipworksRates
	{
		public decimal FedExUpsDomesticCost { get; set; } = 0.55m;
		public decimal FedExUpsInternationalCost { get; set; } = 0.5m;
		public decimal UspsDomesticCost { get; set; } = 0.75m;
		public decimal iParcelCost { get; set; } = 0.75m;
		public decimal UspsInternationalCost { get; set; } = 1.55m;
		public decimal OtherDomesticCost { get; set; } = 0.55m;
		public decimal OtherInternationalCost { get; set; } = 0.55m;
	}
}