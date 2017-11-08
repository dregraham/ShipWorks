using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request.International
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator that modifies the Broker
    /// properties of the customs detail portion of the FedEx IFedExNativeShipmentRequest object.
    /// </summary>
    public class FedExBrokerManipulator : IFedExShipRequestManipulator
    {
        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber)
        {
            return shipment.FedEx.BrokerEnabled;
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(shipment, request);

            if (ShouldApply(shipment, sequenceNumber))
            {
                // Get a list of the existing special service types; FedEx sometimes throws an exception if "empty" elements 
                // are sent over, so this is done here rather than the initialize request method so any objects of the request 
                // that needs instantiated is only instantiated if there is an actual broker that needs to be added.
                List<ShipmentSpecialServiceType> serviceTypes = GetSpecialServiceTypes(request);

                // Add the broker service to the list and reset the array on the request
                serviceTypes.Add(ShipmentSpecialServiceType.BROKER_SELECT_OPTION);
                request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = serviceTypes.ToArray();

                // Get a handle to the customs detail on the request and prepare a person adapter for creating
                // the contact and address info
                CustomsClearanceDetail customsDetail = GetCustomsDetail(request);
                PersonAdapter person = shipment.FedEx.BrokerPerson;

                BrokerDetail brokerDetail = new BrokerDetail()
                {
                    Broker = new Party()
                    {
                        AccountNumber = shipment.FedEx.BrokerAccount,
                        Address = FedExRequestManipulatorUtilities.CreateAddress<Address>(person),
                        Contact = FedExRequestManipulatorUtilities.CreateContact<Contact>(person)
                    },
                    Type = BrokerType.IMPORT,
                    TypeSpecified = true
                };

                brokerDetail.Broker.Contact.PhoneExtension = shipment.FedEx.BrokerPhoneExtension;

                // Add the broker to brokers array of the customs detail and update the custom detail
                // on the native request (in case a new object was created during the GetCustomsDetail method)
                customsDetail.Brokers = new BrokerDetail[] { brokerDetail };
                request.RequestedShipment.CustomsClearanceDetail = customsDetail;
            }

            return request;
        }

        /// <summary>
        /// Gets a list of any special service types from the IFedExNativeShipmentRequest object.
        /// </summary>
        private List<ShipmentSpecialServiceType> GetSpecialServiceTypes(ProcessShipmentRequest request)
        {
            request.RequestedShipment.Ensure(rs => rs.SpecialServicesRequested)
                .Ensure(sst => sst.SpecialServiceTypes);

            return request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ToList();
        }

        /// <summary>
        /// Gets the customs detail from the request or creates a new one if the one on the request is null.
        /// </summary>
        private CustomsClearanceDetail GetCustomsDetail(ProcessShipmentRequest request)
        {
            return request.RequestedShipment.CustomsClearanceDetail ?? new CustomsClearanceDetail();
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private void InitializeRequest(IShipmentEntity shipment, ProcessShipmentRequest request) =>
            request.Ensure(r => r.RequestedShipment);
    }
}
