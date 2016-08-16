using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

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

            Assert.Equal(4, ((ProcessShipmentRequest) shipRequest.NativeRequest).RequestedShipment.SpecialServicesRequested.EMailNotificationDetail.Recipients.Count());
        }

        //Sender
        [Fact]
        public void Manipulate_GetsExceptionNotifications_NotifyIsTrueForSenderForExceptions()
        {
            shipmentEntity.FedEx.EmailNotifySender = 2;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(1, recipient.NotificationEventsRequested.Count());
            Assert.Equal(EMailNotificationEventType.ON_EXCEPTION, recipient.NotificationEventsRequested.First());
        }

        [Fact]
        public void Manipulate_GetsAllNotifications_NotifyIsTrueForSenderForAll()
        {
            shipmentEntity.FedEx.EmailNotifySender = 7;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(3, recipient.NotificationEventsRequested.Count());
            Assert.True(recipient.NotificationEventsRequested.Any(x => x == EMailNotificationEventType.ON_SHIPMENT));
            Assert.True(recipient.NotificationEventsRequested.Any(x => x == EMailNotificationEventType.ON_EXCEPTION));
            Assert.True(recipient.NotificationEventsRequested.Any(x => x == EMailNotificationEventType.ON_DELIVERY));
        }

        [Fact]
        public void Manipulate_GetsShipNotifications_NotifyIsTrueForSenderForAll()
        {
            shipmentEntity.FedEx.EmailNotifySender = 7;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.True(recipient.NotificationEventsRequested.Any(x => x == EMailNotificationEventType.ON_SHIPMENT));
        }

        [Fact]
        public void Manipulate_GetsExceptionNotifications_NotifyIsTrueForSenderForAll()
        {
            shipmentEntity.FedEx.EmailNotifySender = 7;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.True(recipient.NotificationEventsRequested.Any(x => x == EMailNotificationEventType.ON_EXCEPTION));
        }

        [Fact]
        public void Manipulate_GetsDeliveryNotifications_NotifyIsTrueForSenderForAll()
        {
            shipmentEntity.FedEx.EmailNotifySender = 7;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.True(recipient.NotificationEventsRequested.Any(x => x == EMailNotificationEventType.ON_DELIVERY));
        }

        [Fact]
        public void Manipulate_SenderNotificationFormatCorrect_NotifySenderIsTrueForSenderAndOriginIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifySender = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(EMailNotificationFormatType.HTML, recipient.Format);
        }

        [Fact]
        public void Manipulate_SenderNotificationLocationCorrect_NotifyIsTrueForSenderAndOriginIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifySender = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal("EN", recipient.Localization.LanguageCode);
        }

        [Fact]
        public void Manipulate_SenderNotificationNotificationTypeCorrect_NotifyIsTrueForSenderAndSenderIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifySender = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(EMailNotificationRecipientType.SHIPPER, recipient.EMailNotificationRecipientType);
        }

        [Fact]
        public void Manipulate_SenderNotificationEmailCorrect_NotifyIsTrueForSenderAndSenderIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifySender = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(shipmentEntity.OriginEmail, recipient.EMailAddress);
        }

        //Recipient

        [Fact]
        public void Manipulate_RecipientNotificationNotificationTypeCorrect_NotifyIsTrueForRecipientAndShipToIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifyRecipient = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(EMailNotificationRecipientType.RECIPIENT, recipient.EMailNotificationRecipientType);
        }

        [Fact]
        public void Manipulate_RecipientNotificationEmailCorrect_NotifyIsTrueForRecipientAndShipToEmailIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifyRecipient = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(shipmentEntity.ShipEmail, recipient.EMailAddress);
        }

        //Other

        [Fact]
        public void Manipulate_OtherNotificationNotificationTypeCorrect_NotifyIsTrueForOtherAndOtherEmailIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifyOther = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(EMailNotificationRecipientType.OTHER, recipient.EMailNotificationRecipientType);
        }

        [Fact]
        public void Manipulate_OtherNotificationEmailCorrect_NotifyIsTrueForOtherAndOtherEmailIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifyOther = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(shipmentEntity.FedEx.EmailNotifyOtherAddress, recipient.EMailAddress);
        }

        //Broker

        [Fact]
        public void Manipulate_BrokerNotificationNotificationTypeCorrect_NotifyIsTrueForBrokerAndBrokerEmailIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifyBroker = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(EMailNotificationRecipientType.BROKER, recipient.EMailNotificationRecipientType);
        }

        [Fact]
        public void Manipulate_BrokerNotificationEmailCorrect_NotifyIsTrueForBrokerAndBrokerEmailIsNotBlank()
        {
            shipmentEntity.FedEx.EmailNotifyBroker = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.Equal(shipmentEntity.FedEx.BrokerEmail, recipient.EMailAddress);
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
        private EMailNotificationRecipient GetSingleRecipient()
        {
            EMailNotificationRecipient[] recipients = ((ProcessShipmentRequest) shipRequest.NativeRequest).RequestedShipment.SpecialServicesRequested.EMailNotificationDetail.Recipients;

            Assert.Equal(1, recipients.Count());

            EMailNotificationRecipient recipient = recipients[0];
            return recipient;
        }
    }
}
