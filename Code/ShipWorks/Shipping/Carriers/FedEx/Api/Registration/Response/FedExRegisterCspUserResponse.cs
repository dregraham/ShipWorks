using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Response
{
    public class FedExRegisterCspUserResponse : ICarrierResponse
    {
        private readonly RegisterWebUserReply nativeResponse;
        private readonly CarrierRequest request;
        private readonly ICarrierSettingsRepository settingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRegisterCspUserResponse" /> class using
        /// the FedExSettingsRepository as the carrier settings repository.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request.</param>
        public FedExRegisterCspUserResponse(RegisterWebUserReply nativeResponse, CarrierRequest request)
            : this (nativeResponse, request, new FedExSettingsRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRegisterCspUserResponse" /> class.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request.</param>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExRegisterCspUserResponse(RegisterWebUserReply nativeResponse, CarrierRequest request, ICarrierSettingsRepository settingsRepository)
        {
            this.nativeResponse = nativeResponse;
            this.request = request;
            this.settingsRepository = settingsRepository;
        }


        /// <summary>
        /// Gets the request the was used to generate the response.
        /// </summary>
        /// <value>The CarrierRequest object.</value>
        public CarrierRequest Request
        {
            get { return request; }
        }

        /// <summary>
        /// Gets the native response received from the carrier API.
        /// </summary>
        /// <value>The native response.</value>
        public object NativeResponse
        {
            get { return nativeResponse; }
        }

        /// <summary>
        /// Performs any processing required based on the response from the carrier.
        /// </summary>
        /// <exception cref="FedExApiCarrierException"></exception>
        public void Process()
        {
            if (nativeResponse.HighestSeverity == NotificationSeverityType.ERROR || nativeResponse.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                throw new FedExApiCarrierException(nativeResponse.Notifications);
            }

            // Save the user name and password from the response back to the shipping settings
            ShippingSettingsEntity settings = settingsRepository.GetShippingSettings();
            
            settings.FedExUsername = nativeResponse.UserCredential.Key;
            settings.FedExPassword = SecureText.Encrypt(nativeResponse.UserCredential.Password, "FedEx");

            settingsRepository.SaveShippingSettings(settings);
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public IEntity2 CarrierAccountEntity { get; set; }
    }
}
