using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.Express.International;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.Canada.Ground
{
    public class FedExCAGroundDomesticInternationalFixture : FedExUSExpressInternationalFixture
    {
        public string CodDetailRateTypeBasis { get; set; }

        /// <summary>
        /// Creates the shipment.
        /// </summary>
        /// <returns></returns>
        public override ShipmentEntity CreateShipment()
        {
            if (string.IsNullOrWhiteSpace(CustomsClearanceDocumentContent))
            {
                CustomsClearanceDocumentContent = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(CommoditiesQuantity))
            {
                CommoditiesQuantity = "0";
            }

            if (string.IsNullOrWhiteSpace(CustomsClearanceInsuranceAmount))
            {
                CustomsClearanceInsuranceAmount = "0";
            }

            if (string.IsNullOrWhiteSpace(CustomerReferenceValue))
            {
                CustomerReferenceValue = string.Empty;
            }

            ShipmentEntity shipment = base.CreateShipment();

            if (string.IsNullOrWhiteSpace(CodChargeBasis))
            {
                shipment.FedEx.CodAddFreight = false;
            }

            return shipment;
        }

        /// <summary>
        /// Setups the commercial invoice.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        protected override void SetupCommercialInvoice(ShipmentEntity shipment)
        { }

        /// <summary>
        /// Setups the recipient identification.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        protected override void SetupRecipientIdentification(ShipmentEntity shipment)
        { }
    }
}
