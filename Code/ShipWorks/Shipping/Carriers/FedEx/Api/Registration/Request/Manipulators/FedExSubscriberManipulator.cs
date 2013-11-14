using System;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator that modifies the properties of
    /// a FedEx SubscriptionRequest object.
    /// </summary>
    public class FedExSubscriberManipulator : ICarrierRequestManipulator
    {
        private readonly FedExSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExSubscriberManipulator" /> class using
        /// the FedExSettingsRepository.
        /// </summary>
        public FedExSubscriberManipulator()
            : this(new FedExSettingsRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExSubscriberManipulator" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExSubscriberManipulator(ICarrierSettingsRepository settingsRepository)
        {
            settings = new FedExSettings(settingsRepository);
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed validation
            SubscriptionRequest nativeRequest = request.NativeRequest as SubscriptionRequest;

            FedExAccountEntity account = request.CarrierAccountEntity as FedExAccountEntity;
            if (account == null)
            {
                throw new CarrierException("An invalid carrier account was provided.");
            }

            PersonAdapter person = new PersonAdapter(account, string.Empty);

            nativeRequest.Subscriber.AccountNumber = account.AccountNumber;
            nativeRequest.Subscriber.Address = FedExRequestManipulatorUtilities.CreateAddress<Address>(person);
            nativeRequest.Subscriber.Contact = FedExRequestManipulatorUtilities.CreateContact<Contact>(person);
            nativeRequest.AccountShippingAddress = FedExRequestManipulatorUtilities.CreateAddress<Address>(person);

            // CSP 
            nativeRequest.CspSolutionId = settings.CspSolutionId;
            nativeRequest.CspType = CspType.CERTIFIED_SOLUTION_PROVIDER;
            nativeRequest.CspTypeSpecified = true;
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

            // The native FedEx request type should be a SubscriptionRequest
            SubscriptionRequest nativeRequest = request.NativeRequest as SubscriptionRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            nativeRequest.Subscriber = new Party();
        }
    }
}
