﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using Moq;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExEmailNotificationsManipulatorTest
    {
        private FedExShipRequest shipRequest;

        private ShipmentEntity shipmentEntity;

        private FedExEmailNotificationsManipulator testObject;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        [TestInitialize]
        public void Initialize()
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

        [TestMethod]
        public void Manipulate_NoNotifications_NotifyIsFalseForAllWithBlankEmails_Test()
        {
            shipmentEntity.ShipEmail = "";
            shipmentEntity.OriginEmail = "";
            shipmentEntity.FedEx.BrokerEmail = "";
            shipmentEntity.FedEx.EmailNotifyOtherAddress = "";

            testObject.Manipulate(shipRequest);

            Assert.IsNull(((ProcessShipmentRequest) shipRequest.NativeRequest).RequestedShipment.SpecialServicesRequested);
        }

        [TestMethod]
        public void Manipulate_NoNotifications_NotifyIsFalseForAllWithNotBlankEmails_Test()
        {
            testObject.Manipulate(shipRequest);

            Assert.IsNull(((ProcessShipmentRequest) shipRequest.NativeRequest).RequestedShipment.SpecialServicesRequested);
        }

        [TestMethod]
        public void Manipulate_NoNotifications_NotifyIsTrueForAllAndEmailsAreBlank_Test()
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

            Assert.IsNull(((ProcessShipmentRequest) shipRequest.NativeRequest).RequestedShipment.SpecialServicesRequested);
        }

        [TestMethod]
        public void Manipulate_FourNotifications_NotifyIsTrueForAllAndEmailsAreValid_Test()
        {
            shipmentEntity.FedEx.EmailNotifySender = 1;
            shipmentEntity.FedEx.EmailNotifyRecipient = 1;
            shipmentEntity.FedEx.EmailNotifyBroker = 1;
            shipmentEntity.FedEx.EmailNotifyOther = 1;

            testObject.Manipulate(shipRequest);

            Assert.AreEqual(4, ((ProcessShipmentRequest) shipRequest.NativeRequest).RequestedShipment.SpecialServicesRequested.EMailNotificationDetail.Recipients.Count());
        }

        //Sender
        [TestMethod]
        public void Manipulate_GetsExceptionNotifications_NotifyIsTrueForSenderForExceptions_Test()
        {
            shipmentEntity.FedEx.EmailNotifySender = 2;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.AreEqual(1, recipient.NotificationEventsRequested.Count());
            Assert.AreEqual(EMailNotificationEventType.ON_EXCEPTION, recipient.NotificationEventsRequested.First());
        }

        [TestMethod]
        public void Manipulate_GetsAllNotifications_NotifyIsTrueForSenderForAll_Test()
        {
            shipmentEntity.FedEx.EmailNotifySender = 7;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.AreEqual(3, recipient.NotificationEventsRequested.Count());
            Assert.IsTrue(recipient.NotificationEventsRequested.Any(x => x == EMailNotificationEventType.ON_SHIPMENT));
            Assert.IsTrue(recipient.NotificationEventsRequested.Any(x => x == EMailNotificationEventType.ON_EXCEPTION));
            Assert.IsTrue(recipient.NotificationEventsRequested.Any(x => x == EMailNotificationEventType.ON_DELIVERY));
        }

        [TestMethod]
        public void Manipulate_GetsShipNotifications_NotifyIsTrueForSenderForAll_Test()
        {
            shipmentEntity.FedEx.EmailNotifySender = 7;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.IsTrue(recipient.NotificationEventsRequested.Any(x => x == EMailNotificationEventType.ON_SHIPMENT));
        }

        [TestMethod]
        public void Manipulate_GetsExceptionNotifications_NotifyIsTrueForSenderForAll_Test()
        {
            shipmentEntity.FedEx.EmailNotifySender = 7;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.IsTrue(recipient.NotificationEventsRequested.Any(x => x == EMailNotificationEventType.ON_EXCEPTION));
        }

        [TestMethod]
        public void Manipulate_GetsDeliveryNotifications_NotifyIsTrueForSenderForAll_Test()
        {
            shipmentEntity.FedEx.EmailNotifySender = 7;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.IsTrue(recipient.NotificationEventsRequested.Any(x => x == EMailNotificationEventType.ON_DELIVERY));
        }

        [TestMethod]
        public void Manipulate_SenderNotificationFormatCorrect_NotifySenderIsTrueForSenderAndOriginIsNotBlank_Test()
        {
            shipmentEntity.FedEx.EmailNotifySender = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.AreEqual(EMailNotificationFormatType.HTML, recipient.Format);
        }

        [TestMethod]
        public void Manipulate_SenderNotificationLocationCorrect_NotifyIsTrueForSenderAndOriginIsNotBlank_Test()
        {
            shipmentEntity.FedEx.EmailNotifySender = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.AreEqual("EN", recipient.Localization.LanguageCode);
        }

        [TestMethod]
        public void Manipulate_SenderNotificationNotificationTypeCorrect_NotifyIsTrueForSenderAndSenderIsNotBlank_Test()
        {
            shipmentEntity.FedEx.EmailNotifySender = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.AreEqual(EMailNotificationRecipientType.SHIPPER, recipient.EMailNotificationRecipientType);
        }

        [TestMethod]
        public void Manipulate_SenderNotificationEmailCorrect_NotifyIsTrueForSenderAndSenderIsNotBlank_Test()
        {
            shipmentEntity.FedEx.EmailNotifySender = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.AreEqual(shipmentEntity.OriginEmail, recipient.EMailAddress);
        }

        //Recipient

        [TestMethod]
        public void Manipulate_RecipientNotificationNotificationTypeCorrect_NotifyIsTrueForRecipientAndShipToIsNotBlank_Test()
        {
            shipmentEntity.FedEx.EmailNotifyRecipient = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.AreEqual(EMailNotificationRecipientType.RECIPIENT, recipient.EMailNotificationRecipientType);
        }

        [TestMethod]
        public void Manipulate_RecipientNotificationEmailCorrect_NotifyIsTrueForRecipientAndShipToEmailIsNotBlank_Test()
        {
            shipmentEntity.FedEx.EmailNotifyRecipient = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.AreEqual(shipmentEntity.ShipEmail, recipient.EMailAddress);
        }

        //Other

        [TestMethod]
        public void Manipulate_OtherNotificationNotificationTypeCorrect_NotifyIsTrueForOtherAndOtherEmailIsNotBlank_Test()
        {
            shipmentEntity.FedEx.EmailNotifyOther = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.AreEqual(EMailNotificationRecipientType.OTHER, recipient.EMailNotificationRecipientType);
        }

        [TestMethod]
        public void Manipulate_OtherNotificationEmailCorrect_NotifyIsTrueForOtherAndOtherEmailIsNotBlank_Test()
        {
            shipmentEntity.FedEx.EmailNotifyOther = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.AreEqual(shipmentEntity.FedEx.EmailNotifyOtherAddress, recipient.EMailAddress);
        }

        //Broker

        [TestMethod]
        public void Manipulate_BrokerNotificationNotificationTypeCorrect_NotifyIsTrueForBrokerAndBrokerEmailIsNotBlank_Test()
        {
            shipmentEntity.FedEx.EmailNotifyBroker = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.AreEqual(EMailNotificationRecipientType.BROKER, recipient.EMailNotificationRecipientType);
        }

        [TestMethod]
        public void Manipulate_BrokerNotificationEmailCorrect_NotifyIsTrueForBrokerAndBrokerEmailIsNotBlank_Test()
        {
            shipmentEntity.FedEx.EmailNotifyBroker = 1;
            testObject.Manipulate(shipRequest);

            var recipient = GetSingleRecipient();

            Assert.AreEqual(shipmentEntity.FedEx.BrokerEmail, recipient.EMailAddress);
        }

        [TestMethod]
        public void Manipulate_NoSpecialServicesRequested_NotifyIsTrueForBrokerAndBrokerEmailIsNotBlankButEmailNotifyBrokerIsFalse_Test()
        {
            shipmentEntity.FedEx.EmailNotifyBroker = 1;
            shipmentEntity.FedEx.BrokerEnabled = false;

            testObject.Manipulate(shipRequest);

            Assert.IsNull(((ProcessShipmentRequest) shipRequest.NativeRequest).RequestedShipment.SpecialServicesRequested);
        }

        /// <summary>
        /// Asserts only one recipeint and returns it.
        /// </summary>
        private EMailNotificationRecipient GetSingleRecipient()
        {
            EMailNotificationRecipient[] recipients = ((ProcessShipmentRequest) shipRequest.NativeRequest).RequestedShipment.SpecialServicesRequested.EMailNotificationDetail.Recipients;

            Assert.AreEqual(1, recipients.Count());

            EMailNotificationRecipient recipient = recipients[0];
            return recipient;
        }
    }
}
