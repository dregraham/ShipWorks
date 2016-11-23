using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using System.Linq;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExEmailNotificationsManipulatorTest
    {
        private FedExShipRequest shipRequest;

        private ShipmentEntity shipmentEntity;

        private FedExEmailNotificationsManipulator testObject;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        public FedExEmailNotificationsManipulatorTest()
        {
            settingsRepository = new Mock<ICarrierSettingsRepository>();

            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipRequest = new FedExShipRequest(null, shipmentEntity, null, null, settingsRepository.Object, new ProcessShipmentRequest());
            testObject = new FedExEmailNotificationsManipulator();

            shipmentEntity.FedEx.EmailNotifySender = 0;
            shipmentEntity.FedEx.EmailNotifyRecipient = 0;

            shipmentEntity.FedEx.EmailNotifyBroker = 0;
            shipmentEntity.FedEx.BrokerEnabled = true;

            shipmentEntity.FedEx.EmailNotifyOther = 0;
            shipmentEntity.FedEx.EmailNotifyMessage = "emailMessage";

            shipmentEntity.ShipEmail = "ship@blah.com";
            shipmentEntity.OriginEmail = "origin@blah.com";
            shipmentEntity.FedEx.BrokerEmail = "broker@blah.com";
            shipmentEntity.FedEx.EmailNotifyOtherAddress = "other@blah.com";
        }

        [Fact]
        public void Manipulate_NoNotifications_NotifyIsFalseForAllWithBlankEmails()
        {
            shipmentEntity.ShipEmail = "";
            shipmentEntity.OriginEmail = "";
            shipmentEntity.FedEx.BrokerEmail = "";
            shipmentEntity.FedEx.EmailNotifyOtherAddress = "";

            testObject.Manipulate(shipRequest);

            Assert.Null(((ProcessShipmentRequest) shipRequest.NativeRequest).RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_NoNotifications_NotifyIsFalseForAllWithNotBlankEmails()
        {
            testObject.Manipulate(shipRequest);

            Assert.Null(((ProcessShipmentRequest) shipRequest.NativeRequest).RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_NoNotifications_NotifyIsTrueForAllAndEmailsAreBlank()
        {
            shipmentEntity.FedEx.EmailNotifySender = 1;
            shipmentEntity.FedEx.EmailNotifyRecipient = 1;
            shipmentEntity.FedEx.EmailNotifyBroker = 1;
            shipmentEntity.FedEx.EmailNotifyOther = 1;

            shipmentEntity.ShipEmail = "";
            shipmentEntity.OriginEmail = "";
            shipmentEntity.FedEx.BrokerEmail = "";
            shipmentEntity.FedEx.EmailNotifyOtherAddress = "";

            testObject.Manipulate(shipRequest);

            Assert.Null(((ProcessShipmentRequest) shipRequest.NativeRequest).RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_FourNotifications_NotifyIsTrueForAllAndEmailsAreValid()
        {
            shipmentEntity.FedEx.EmailNotifySender = 1;
            shipmentEntity.FedEx.EmailNotifyRecipient = 1;
            shipmentEntity.FedEx.EmailNotifyBroker = 1;
            shipmentEntity.FedEx.EmailNotifyOther = 1;

            testObject.Manipulate(shipRequest);

            Assert.Equal(4, ((ProcessShipmentRequest) shipRequest.NativeRequest).RequestedShipment.SpecialServicesRequested.EventNotificationDetail.EventNotifications.Count());
        }

        //Sender
        [Fact]
        public void Manipulate_GetsExceptionNotifications_NotifyIsTrueForSenderForExceptions()
        {
            shipmentEntity.FedEx.EmailNotifySender = 2;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(1, recipient.Events.Count());
            Assert.Equal(NotificationEventType.ON_EXCEPTION, recipient.Events.First());
        }

        [Fact]
        public void Manipulate_GetsAllNotifications_NotifyIsTrueForSenderForAll()
        {
            shipmentEntity.FedEx.EmailNotifySender = 7;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(3, recipient.Events.Count());
            Assert.True(recipient.Events.Any(x => x == NotificationEventType.ON_SHIPMENT));
            Assert.True(recipient.Events.Any(x => x == NotificationEventType.ON_EXCEPTION));
            Assert.True(recipient.Events.Any(x => x == NotificationEventType.ON_DELIVERY));
        }

        [Fact]
        public void Manipulate_GetsShipNotifications_NotifyIsTrueForSenderForAll()
        {
            shipmentEntity.FedEx.EmailNotifySender = 7;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.True(recipient.Events.Any(x => x == NotificationEventType.ON_SHIPMENT));
        }

        [Fact]
        public void Manipulate_GetsExceptionNotifications_NotifyIsTrueForSenderForAll()
        {
            shipmentEntity.FedEx.EmailNotifySender = 7;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.True(recipient.Events.Any(x => x == NotificationEventType.ON_EXCEPTION));
        }

        [Fact]
        public void Manipulate_GetsDeliveryNotifications_NotifyIsTrueForSenderForAll()
        {
            shipmentEntity.FedEx.EmailNotifySender = 7;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.True(recipient.Events.Any(x => x == NotificationEventType.ON_DELIVERY));
        }

        [Fact]
        public void Manipulate_SenderNotificationFormatCorrect_NotifySenderIsTrueForSenderAndOriginIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifySender = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.True(recipient.FormatSpecification.TypeSpecified);
            Assert.Equal(NotificationFormatType.HTML, recipient.FormatSpecification.Type);
        }

        [Fact]
        public void Manipulate_SenderNotificationLocationCorrect_NotifyIsTrueForSenderAndOriginIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifySender = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal("EN", recipient.NotificationDetail.Localization.LanguageCode);
        }

        [Fact]
        public void Manipulate_SenderNotificationNotificationTypeCorrect_NotifyIsTrueForSenderAndSenderIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifySender = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.True(recipient.RoleSpecified);
            Assert.Equal(ShipmentNotificationRoleType.SHIPPER, recipient.Role);
        }

        [Fact]
        public void Manipulate_SenderNotificationEmailCorrect_NotifyIsTrueForSenderAndSenderIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifySender = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(shipmentEntity.OriginEmail, recipient.NotificationDetail.EmailDetail.EmailAddress);
        }

        //Recipient

        [Fact]
        public void Manipulate_RecipientNotificationNotificationTypeCorrect_NotifyIsTrueForRecipientAndShipToIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifyRecipient = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(ShipmentNotificationRoleType.RECIPIENT, recipient.Role);
        }

        [Fact]
        public void Manipulate_RecipientNotificationEmailCorrect_NotifyIsTrueForRecipientAndShipToEmailIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifyRecipient = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(shipmentEntity.ShipEmail, recipient.NotificationDetail.EmailDetail.EmailAddress);
        }

        //Other

        [Fact]
        public void Manipulate_OtherNotificationNotificationTypeCorrect_NotifyIsTrueForOtherAndOtherEmailIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifyOther = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(ShipmentNotificationRoleType.OTHER, recipient.Role);
        }

        [Fact]
        public void Manipulate_OtherNotificationEmailCorrect_NotifyIsTrueForOtherAndOtherEmailIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifyOther = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(shipmentEntity.FedEx.EmailNotifyOtherAddress, recipient.NotificationDetail.EmailDetail.EmailAddress);
        }

        //Broker

        [Fact]
        public void Manipulate_BrokerNotificationNotificationTypeCorrect_NotifyIsTrueForBrokerAndBrokerEmailIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifyBroker = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(ShipmentNotificationRoleType.BROKER, recipient.Role);
        }

        [Fact]
        public void Manipulate_BrokerNotificationEmailCorrect_NotifyIsTrueForBrokerAndBrokerEmailIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifyBroker = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(shipmentEntity.FedEx.BrokerEmail, recipient.NotificationDetail.EmailDetail.EmailAddress);
        }

        [Fact]
        public void Manipulate_NoSpecialServicesRequested_NotifyIsTrueForBrokerAndBrokerEmailIsNotBlankButEmailNotifyBrokerIsFalse()
        {
            shipmentEntity.FedEx.EmailNotifyBroker = 1;
            shipmentEntity.FedEx.BrokerEnabled = false;

            testObject.Manipulate(shipRequest);

            Assert.Null(((ProcessShipmentRequest) shipRequest.NativeRequest).RequestedShipment.SpecialServicesRequested);
        }

        /// <summary>
        /// Asserts only one recipeint and returns it.
        /// </summary>
        private ShipmentEventNotificationSpecification GetSingleRecipient()
        {
            ShipmentEventNotificationSpecification[] recipients = ((ProcessShipmentRequest) shipRequest.NativeRequest).RequestedShipment.SpecialServicesRequested.EventNotificationDetail.EventNotifications;

            Assert.Equal(1, recipients.Count());

            ShipmentEventNotificationSpecification recipient = recipients[0];
            return recipient;
        }
    }
}
