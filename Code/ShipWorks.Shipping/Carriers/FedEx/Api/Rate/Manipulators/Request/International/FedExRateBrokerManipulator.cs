using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator that modifies the Broker
    /// properties of the customs detail portion of the FedEx RateRequest object.
    /// </summary>
    public class FedExRateBrokerManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRateBrokerManipulator" /> class.
        /// </summary>
        public FedExRateBrokerManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRateBrokerManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExRateBrokerManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization
            RateRequest nativeRequest = request.NativeRequest as RateRequest;

            if (request.ShipmentEntity.FedEx.BrokerEnabled)
            {
                // Get a list of the existing special service types; FedEx sometimes throws an exception if "empty" elements 
                // are sent over, so this is done here rather than the initialize request method so any objects of the request 
                // that needs instantiated is only instantiated if there is an actual broker that needs to be added.
                List<ShipmentSpecialServiceType> serviceTypes = GetSpecialServiceTypes(nativeRequest);
                
                // Add the broker service to the list and reset the array on the request
                serviceTypes.Add(ShipmentSpecialServiceType.BROKER_SELECT_OPTION);
                nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = serviceTypes.ToArray();

                // Get a handle to the customs detail on the request and prepare a person adapter for creating
                // the contact and address info
                CustomsClearanceDetail customsDetail = GetCustomsDetail(nativeRequest);
                PersonAdapter person = new PersonAdapter(request.ShipmentEntity.FedEx, "Broker");

                BrokerDetail brokerDetail = new BrokerDetail()
                {
                    Broker = new Party()
                    {
                        AccountNumber = request.ShipmentEntity.FedEx.BrokerAccount,
                        Address = FedExRequestManipulatorUtilities.CreateAddress<Address>(person),
                        Contact = FedExRequestManipulatorUtilities.CreateContact<Contact>(person)
                    },
                    Type = BrokerType.IMPORT,
                    TypeSpecified = true
                };
                
                brokerDetail.Broker.Contact.PhoneExtension = request.ShipmentEntity.FedEx.BrokerPhoneExtension;
                
                // Add the broker to brokers array of the customs detail and update the custom detail
                // on the native request (in case a new object was created during the GetCustomsDetail method)
                customsDetail.Brokers = new BrokerDetail[] {brokerDetail};
                nativeRequest.RequestedShipment.CustomsClearanceDetail = customsDetail;
            }
        }

        /// <summary>
        /// Gets a list of any special service types from the RateRequest object.
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        /// <returns>A List of ShipmentSpecialServiceType objects.</returns>
        private List<ShipmentSpecialServiceType> GetSpecialServiceTypes(RateRequest nativeRequest)
        {
            // We want to build a list of the special service types from the existing request
            if (nativeRequest.RequestedShipment.SpecialServicesRequested == null)
            {
                // The "owning" object of the array we're interested in needs to be created
                nativeRequest.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            }

            if (nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes == null)
            {
                // Initialize the array to an empty array, so an empty list is returned
                nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            }

            return nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ToList();
        }

        /// <summary>
        /// Gets the customs detail from the request or creates a new one if the one on the request is null.
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        /// <returns>A CustomsClearanceDetail object.</returns>
        private CustomsClearanceDetail GetCustomsDetail(RateRequest nativeRequest)
        {
            return nativeRequest.RequestedShipment.CustomsClearanceDetail ?? new CustomsClearanceDetail();
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a RateRequest
            RateRequest nativeRequest = request.NativeRequest as RateRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            // Make sure the RequestedShipment is there
            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }            
        }
    }
}
