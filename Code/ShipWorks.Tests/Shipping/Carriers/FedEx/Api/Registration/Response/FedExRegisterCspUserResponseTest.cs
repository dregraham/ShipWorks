using Interapptive.Shared.Utility;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Registration.Response
{
    public class FedExRegisterCspUserResponseTest
    {
        private FedExRegisterCspUserResponse testObject;

        private RegisterWebUserReply nativeResponse;
        private Mock<CarrierRequest> carrierRequest;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        private ShippingSettingsEntity shippingSettings;

        public FedExRegisterCspUserResponseTest()
        {
            shippingSettings = new ShippingSettingsEntity();

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);
            settingsRepository.Setup(r => r.SaveShippingSettings(It.IsAny<ShippingSettingsEntity>()));


            nativeResponse = new RegisterWebUserReply()
            {
                HighestSeverity = NotificationSeverityType.SUCCESS,
                UserCredential = new WebAuthenticationCredential
                {
                    Key = "SomeKey",
                    Password = "password"
                }
            };

            carrierRequest = new Mock<CarrierRequest>(null, null);

            testObject = new FedExRegisterCspUserResponse(nativeResponse, carrierRequest.Object, settingsRepository.Object);
        }

        [Fact]
        public void Process_SetsFedExUserName_ToCredentialKey()
        {
            testObject.Process();

            Assert.Equal(nativeResponse.UserCredential.Key, shippingSettings.FedExUsername);
        }

        [Fact]
        public void Process_SetsFedExPassword()
        {
            testObject.Process();

            // Just checking that the password was set (i.e. not empty)
            Assert.False(string.IsNullOrEmpty(shippingSettings.FedExPassword));
        }

        [Fact]
        public void Process_EncryptsPassword()
        {
            testObject.Process();

            Assert.Equal(SecureText.Encrypt("password", "FedEx"), shippingSettings.FedExPassword);
        }

        [Fact]
        public void Process_DelegatesToRepository_ToGetShippingSettings()
        {
            testObject.Process();

            settingsRepository.Verify(r => r.GetShippingSettings(), Times.Once());
        }

        [Fact]
        public void Process_DelegatesToRepository_ToSaveShippingSettings()
        {
            testObject.Process();

            // Make sure that the settings retrieved from the repository are saved
            settingsRepository.Verify(r => r.SaveShippingSettings(shippingSettings), Times.Once());
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenReceivingErrorSeverity()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.ERROR;
            nativeResponse.Notifications = new Notification[] { new Notification { Message = "message" } };

            Assert.Throws<FedExApiCarrierException>(() => testObject.Process());
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenReceivingFailureSeverity()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.FAILURE;
            nativeResponse.Notifications = new Notification[] { new Notification { Message = "message" } };

            Assert.Throws<FedExApiCarrierException>(() => testObject.Process());
        }
    }
}
