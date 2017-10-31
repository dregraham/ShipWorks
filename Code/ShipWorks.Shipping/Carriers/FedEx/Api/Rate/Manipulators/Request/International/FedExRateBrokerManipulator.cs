using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator that modifies the Broker
    /// properties of the customs detail portion of the FedEx RateRequest object.
    /// </summary>
    public class FedExRateBrokerManipulator : IFedExRateRequestManipulator
    {
        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options) =>
            shipment.FedEx.BrokerEnabled;

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            InitializeRequest(request);

            request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = request.RequestedShipment
                .SpecialServicesRequested
                .SpecialServiceTypes
                .Append(ShipmentSpecialServiceType.BROKER_SELECT_OPTION)
                .ToArray();

            BrokerDetail brokerDetail = new BrokerDetail()
            {
                Broker = new Party()
                {
                    AccountNumber = shipment.FedEx.BrokerAccount,
                    Address = FedExRequestManipulatorUtilities.CreateAddress<Address>(shipment.FedEx.BrokerPerson),
                    Contact = FedExRequestManipulatorUtilities.CreateContact<Contact>(shipment.FedEx.BrokerPerson)
                },
                Type = BrokerType.IMPORT,
                TypeSpecified = true
            };

            brokerDetail.Broker.Contact.PhoneExtension = shipment.FedEx.BrokerPhoneExtension;

            request.RequestedShipment.CustomsClearanceDetail.Brokers = new BrokerDetail[] { brokerDetail };

            return request;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private void InitializeRequest(RateRequest request)
        {
            request.Ensure(x => x.RequestedShipment)
                .Ensure(x => x.SpecialServicesRequested)
                .Ensure(x => x.SpecialServiceTypes);
            request.RequestedShipment.Ensure(x => x.CustomsClearanceDetail);
        }
    }
}
