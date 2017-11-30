using System.Linq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared.Carriers.FedEx;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExEmailNotificationsManipulatorTest
    {
        private ShipmentEntity shipment;
        private FedExEmailNotificationsManipulator testObject;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        public FedExEmailNotificationsManipulatorTest()
        {
            settingsRepository = new Mock<ICarrierSettingsRepository>();

            shipment = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            testObject = new FedExEmailNotificationsManipulator();

            shipment.FedEx.EmailNotifySender = 0;
            shipment.FedEx.EmailNotifyRecipient = 0;

            shipment.FedEx.EmailNotifyBroker = 0;
            shipment.FedEx.BrokerEnabled = true;

            shipment.FedEx.EmailNotifyOther = 0;
            shipment.FedEx.EmailNotifyMessage = "emailMessage";

            shipment.ShipEmail = "ship@blah.com";
            shipment.OriginEmail = "origin@blah.com";
            shipment.FedEx.BrokerEmail = "broker@blah.com";
            shipment.FedEx.EmailNotifyOtherAddress = "other@blah.com";
        }

        [Fact]
        public void Manipulate_NoNotifications_NotifyIsFalseForAllWithBlankEmails()
        {
            shipment.ShipEmail = "";
            shipment.OriginEmail = "";
            shipment.FedEx.BrokerEmail = "";
            shipment.FedEx.EmailNotifyOtherAddress = "";

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Null(result.Value.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_NoNotifications_NotifyIsFalseForAllWithNotBlankEmails()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Null(result.Value.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_NoNotifications_NotifyIsTrueForAllAndEmailsAreBlank()
        {
            shipment.FedEx.EmailNotifySender = 1;
            shipment.FedEx.EmailNotifyRecipient = 1;
            shipment.FedEx.EmailNotifyBroker = 1;
            shipment.FedEx.EmailNotifyOther = 1;

            shipment.ShipEmail = "";
            shipment.OriginEmail = "";
            shipment.FedEx.BrokerEmail = "";
            shipment.FedEx.EmailNotifyOtherAddress = "";

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Null(result.Value.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_FourNotifications_NotifyIsTrueForAllAndEmailsAreValid()
        {
            shipment.FedEx.EmailNotifySender = 1;
            shipment.FedEx.EmailNotifyRecipient = 1;
            shipment.FedEx.EmailNotifyBroker = 1;
            shipment.FedEx.EmailNotifyOther = 1;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(4, result.Value.RequestedShipment.SpecialServicesRequested.EventNotificationDetail.EventNotifications.Count());
        }

        //Sender
        [Fact]
        public void Manipulate_GetsExceptionNotifications_NotifyIsTrueForSenderForExceptions()
        {
            shipment.FedEx.EmailNotifySender = 2;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            var recipient = GetSingleRecipient(result.Value);

            Assert.Equal(1, recipient.Events.Count());
            Assert.Equal(NotificationEventType.ON_EXCEPTION, recipient.Events.First());
        }

        [Fact]
        public void Manipulate_GetsAllNotifications_NotifyIsTrueForSenderForAll()
        {
            shipment.FedEx.EmailNotifySender = 7;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            var recipient = GetSingleRecipient(result.Value);

            Assert.Equal(3, recipient.Events.Count());
            Assert.True(recipient.Events.Any(x => x == NotificationEventType.ON_SHIPMENT));
            Assert.True(recipient.Events.Any(x => x == NotificationEventType.ON_EXCEPTION));
            Assert.True(recipient.Events.Any(x => x == NotificationEventType.ON_DELIVERY));
        }

        [Fact]
        public void Manipulate_GetsShipNotifications_NotifyIsTrueForSenderForAll()
        {
            shipment.FedEx.EmailNotifySender = 7;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            var recipient = GetSingleRecipient(result.Value);

            Assert.True(recipient.Events.Any(x => x == NotificationEventType.ON_SHIPMENT));
        }

        [Fact]
        public void Manipulate_GetsExceptionNotifications_NotifyIsTrueForSenderForAll()
        {
            shipment.FedEx.EmailNotifySender = 7;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            var recipient = GetSingleRecipient(result.Value);

            Assert.True(recipient.Events.Any(x => x == NotificationEventType.ON_EXCEPTION));
        }

        [Fact]
        public void Manipulate_GetsDeliveryNotifications_NotifyIsTrueForSenderForAll()
        {
            shipment.FedEx.EmailNotifySender = 7;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            var recipient = GetSingleRecipient(result.Value);

            Assert.True(recipient.Events.Any(x => x == NotificationEventType.ON_DELIVERY));
        }

        [Fact]
        public void Manipulate_SenderNotificationFormatCorrect_NotifySenderIsTrueForSenderAndOriginIsNotBlank()
        {
            shipment.FedEx.EmailNotifySender = 1;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            var recipient = GetSingleRecipient(result.Value);

            Assert.True(recipient.FormatSpecification.TypeSpecified);
            Assert.Equal(NotificationFormatType.HTML, recipient.FormatSpecification.Type);
        }

        [Fact]
        public void Manipulate_SenderNotificationLocationCorrect_NotifyIsTrueForSenderAndOriginIsNotBlank()
        {
            shipment.FedEx.EmailNotifySender = 1;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            var recipient = GetSingleRecipient(result.Value);

            Assert.Equal("EN", recipient.NotificationDetail.Localization.LanguageCode);
        }

        [Fact]
        public void Manipulate_SenderNotificationNotificationTypeCorrect_NotifyIsTrueForSenderAndSenderIsNotBlank()
        {
            shipment.FedEx.EmailNotifySender = 1;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            var recipient = GetSingleRecipient(result.Value);

            Assert.True(recipient.RoleSpecified);
            Assert.Equal(ShipmentNotificationRoleType.SHIPPER, recipient.Role);
        }

        [Fact]
        public void Manipulate_SenderNotificationEmailCorrect_NotifyIsTrueForSenderAndSenderIsNotBlank()
        {
            shipment.FedEx.EmailNotifySender = 1;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            var recipient = GetSingleRecipient(result.Value);

            Assert.Equal(shipment.OriginEmail, recipient.NotificationDetail.EmailDetail.EmailAddress);
        }

        //Recipient

        [Fact]
        public void Manipulate_RecipientNotificationNotificationTypeCorrect_NotifyIsTrueForRecipientAndShipToIsNotBlank()
        {
            shipment.FedEx.EmailNotifyRecipient = 1;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            var recipient = GetSingleRecipient(result.Value);

            Assert.Equal(ShipmentNotificationRoleType.RECIPIENT, recipient.Role);
        }

        [Fact]
        public void Manipulate_RecipientNotificationEmailCorrect_NotifyIsTrueForRecipientAndShipToEmailIsNotBlank()
        {
            shipment.FedEx.EmailNotifyRecipient = 1;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            var recipient = GetSingleRecipient(result.Value);

            Assert.Equal(shipment.ShipEmail, recipient.NotificationDetail.EmailDetail.EmailAddress);
        }

        //Other

        [Fact]
        public void Manipulate_OtherNotificationNotificationTypeCorrect_NotifyIsTrueForOtherAndOtherEmailIsNotBlank()
        {
            shipment.FedEx.EmailNotifyOther = 1;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            var recipient = GetSingleRecipient(result.Value);

            Assert.Equal(ShipmentNotificationRoleType.OTHER, recipient.Role);
        }

        [Fact]
        public void Manipulate_OtherNotificationEmailCorrect_NotifyIsTrueForOtherAndOtherEmailIsNotBlank()
        {
            shipment.FedEx.EmailNotifyOther = 1;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            var recipient = GetSingleRecipient(result.Value);

            Assert.Equal(shipment.FedEx.EmailNotifyOtherAddress, recipient.NotificationDetail.EmailDetail.EmailAddress);
        }

        //Broker

        [Fact]
        public void Manipulate_BrokerNotificationNotificationTypeCorrect_NotifyIsTrueForBrokerAndBrokerEmailIsNotBlank()
        {
            shipment.FedEx.EmailNotifyBroker = 1;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            var recipient = GetSingleRecipient(result.Value);

            Assert.Equal(ShipmentNotificationRoleType.BROKER, recipient.Role);
        }

        [Fact]
        public void Manipulate_BrokerNotificationEmailCorrect_NotifyIsTrueForBrokerAndBrokerEmailIsNotBlank()
        {
            shipment.FedEx.EmailNotifyBroker = 1;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            var recipient = GetSingleRecipient(result.Value);

            Assert.Equal(shipment.FedEx.BrokerEmail, recipient.NotificationDetail.EmailDetail.EmailAddress);
        }

        [Fact]
        public void Manipulate_NoSpecialServicesRequested_NotifyIsTrueForBrokerAndBrokerEmailIsNotBlankButEmailNotifyBrokerIsFalse()
        {
            shipment.FedEx.EmailNotifyBroker = 1;
            shipment.FedEx.BrokerEnabled = false;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Null(result.Value.RequestedShipment.SpecialServicesRequested);
        }

        /// <summary>
        /// Asserts only one recipient and returns it.
        /// </summary>
        private ShipmentEventNotificationSpecification GetSingleRecipient(ProcessShipmentRequest request)
        {
            ShipmentEventNotificationSpecification[] recipients = request.RequestedShipment.SpecialServicesRequested.EventNotificationDetail.EventNotifications;

            Assert.Equal(1, recipients.Count());

            ShipmentEventNotificationSpecification recipient = recipients[0];
            return recipient;
        }
    }
}
