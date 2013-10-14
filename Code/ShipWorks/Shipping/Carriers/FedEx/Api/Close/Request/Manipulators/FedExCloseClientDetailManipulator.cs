using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators
{
    /// <summary>
    /// An ICarrierRequestManipulator that will manipulate the ClientDetail information of a GroundCloseRequest object.
    /// </summary>
    public class FedExCloseClientDetailManipulator : ICarrierRequestManipulator
    {
        private bool isSmartPostRequest;
        private readonly FedExSettings fedExSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCloseClientDetailManipulator" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExCloseClientDetailManipulator(ICarrierSettingsRepository settingsRepository)
        {
            this.fedExSettings = new FedExSettings(settingsRepository);
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            FedExAccountEntity account = request.CarrierAccountEntity as FedExAccountEntity;

            if (isSmartPostRequest)
            {
                // We can safely cast this since we've passed validation
                SmartPostCloseRequest nativeRequest = request.NativeRequest as SmartPostCloseRequest;
                nativeRequest.ClientDetail = FedExRequestManipulatorUtilities.CreateCloseClientDetail(account, fedExSettings);
            }
            else
            {
                // We can safely cast this since we've passed validation
                GroundCloseRequest nativeRequest = request.NativeRequest as GroundCloseRequest;
                nativeRequest.ClientDetail = FedExRequestManipulatorUtilities.CreateCloseClientDetail(account, fedExSettings);
            }
        }

        /// <summary>
        /// Validates the request making sure it is not null and of the correct type.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void ValidateRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be eitehr a GroundCloseRequest or SmartPostCloseRequest
            SmartPostCloseRequest smartPostRequest = request.NativeRequest as SmartPostCloseRequest;
            if (smartPostRequest != null)
            {
                isSmartPostRequest = true;
            }
            else
            {
                // See if the native request is for ground close 
                GroundCloseRequest nativeRequest = request.NativeRequest as GroundCloseRequest;
                if (nativeRequest == null)
                {
                    // Abort - we have an unexpected native request
                    throw new CarrierException("An unexpected request type was provided.");
                }

                isSmartPostRequest = false;
            }
        }
    }
}
