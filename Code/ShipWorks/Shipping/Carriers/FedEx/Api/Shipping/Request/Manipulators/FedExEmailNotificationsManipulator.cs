using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// Manipulator for adding email notification information to the FedEx request
    /// </summary>
    public class FedExEmailNotificationsManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExEmailNotificationsManipulator" /> class.
        /// </summary>
        public FedExEmailNotificationsManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExEmailNotificationsManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExEmailNotificationsManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Add the email notifications to the FedEx carrier request
        /// </summary>
        /// <param name="request">The FedEx carrier request</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Get the RequestedShipment object for the request
            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            // Apply email notification options
            ApplyEmailOptions(requestedShipment, request.ShipmentEntity);
        }

        /// <summary>
        /// Apply email notification options
        /// </summary>
        private static void ApplyEmailOptions(RequestedShipment requestedShipment, ShipmentEntity shipment)
        {
            // Get the FedEx shipment
            FedExShipmentEntity fedExShipment = shipment.FedEx;

            bool notifySender = fedExShipment.EmailNotifySender != 0 && shipment.OriginEmail.Length > 0;
            bool notifyRecipient = fedExShipment.EmailNotifyRecipient != 0 && shipment.ShipEmail.Length > 0;
            bool notifyBroker = fedExShipment.EmailNotifyBroker != 0 && fedExShipment.BrokerEnabled && !string.IsNullOrWhiteSpace(fedExShipment.BrokerEmail);
            bool notifyOther = fedExShipment.EmailNotifyOther != 0 && fedExShipment.EmailNotifyOtherAddress.Length > 0;

            // Check if there are any at all up front
            if (!(notifySender || notifyRecipient || notifyOther || notifyBroker))
            {
                return;
            } 

            // We have notifications request, so add the shipment special service type
            SetShipmentSpecialServiceTypes(requestedShipment);

            EMailNotificationDetail emailDetail = new EMailNotificationDetail();
            requestedShipment.SpecialServicesRequested.EMailNotificationDetail = emailDetail;

            // Set the special message
            if (!string.IsNullOrEmpty(fedExShipment.EmailNotifyMessage))
            {
                emailDetail.PersonalMessage = fedExShipment.EmailNotifyMessage;
            }

            List<EMailNotificationRecipient> recipients = new List<EMailNotificationRecipient>();

            // See if any are being sent to the sender
            if (notifySender)
            {
                EMailNotificationRecipient recipient = AddEmailNotificationRecipient(EMailNotificationRecipientType.SHIPPER, shipment.OriginEmail, fedExShipment.EmailNotifySender);
                recipients.Add(recipient);
            }

            // See if any are being sent to the recipient
            if (notifyRecipient)
            {
                EMailNotificationRecipient recipient = AddEmailNotificationRecipient(EMailNotificationRecipientType.RECIPIENT, shipment.ShipEmail, fedExShipment.EmailNotifyRecipient);
                recipients.Add(recipient);
            }

            // See if any are being sent to other
            if (notifyOther)
            {
                EMailNotificationRecipient recipient = AddEmailNotificationRecipient(EMailNotificationRecipientType.OTHER, fedExShipment.EmailNotifyOtherAddress, fedExShipment.EmailNotifyOther);
                recipients.Add(recipient);
            }

            // See if any are being sent to broker
            if (notifyBroker)
            {
                EMailNotificationRecipient recipient = AddEmailNotificationRecipient(EMailNotificationRecipientType.BROKER, fedExShipment.BrokerEmail, fedExShipment.EmailNotifyBroker);
                recipients.Add(recipient);
            }

            emailDetail.Recipients = recipients.ToArray();
        }

        /// <summary>
        /// Populates and returns an EMailNotificationRecipient
        /// </summary>
        /// <param name="recipientType">The EMailNotificationRecipientType for this EMailNotificationRecipient</param>
        /// <param name="emailAddress">The email address for the notification</param>
        /// <param name="notificationTypes">The ShipWorks FedExEmailNotificationType enum, as an int, containing all notification types to return.</param>
        /// <returns></returns>
        private static EMailNotificationRecipient AddEmailNotificationRecipient(EMailNotificationRecipientType recipientType, string emailAddress, int notificationTypes)
        {
            EMailNotificationRecipient recipient = new EMailNotificationRecipient
                {
                    Format = EMailNotificationFormatType.HTML,
                    Localization = new Localization {LanguageCode = "EN"},
                    EMailNotificationRecipientType = recipientType,
                    EMailAddress = emailAddress
                };

            ApplyEmailRecipientOptions(recipient, notificationTypes);

            return recipient;
        }

        /// <summary>
        /// Apply the given notification types to the recipient
        /// </summary>
        private static void ApplyEmailRecipientOptions(EMailNotificationRecipient recipient, int notifcationTypes)
        {
            List<EMailNotificationEventType> emailNotificationEventTypes = new List<EMailNotificationEventType>();

            if ((notifcationTypes & (int)FedExEmailNotificationType.Ship) != 0)
            {
                emailNotificationEventTypes.Add(EMailNotificationEventType.ON_SHIPMENT);
            }

            if ((notifcationTypes & (int)FedExEmailNotificationType.Exception) != 0)
            {
                emailNotificationEventTypes.Add(EMailNotificationEventType.ON_EXCEPTION);
            }

            if ((notifcationTypes & (int)FedExEmailNotificationType.Deliver) != 0)
            {
                emailNotificationEventTypes.Add(EMailNotificationEventType.ON_DELIVERY);
            }

            recipient.NotificationEventsRequested = emailNotificationEventTypes.ToArray();
        }

        /// <summary>
        /// Add Email Notification shipment special service type to the requested shipment
        /// </summary>
        private static void SetShipmentSpecialServiceTypes(RequestedShipment requestedShipment)
        {
            if (requestedShipment.SpecialServicesRequested == null)
            {
                requestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            }

            List<ShipmentSpecialServiceType> shipmentSpecialServicesTypes;
            if (requestedShipment.SpecialServicesRequested.SpecialServiceTypes != null)
            {
                shipmentSpecialServicesTypes = requestedShipment.SpecialServicesRequested.SpecialServiceTypes.OfType<ShipmentSpecialServiceType>().ToList();
            }
            else
            {
                shipmentSpecialServicesTypes = new List<ShipmentSpecialServiceType>();
            }

            if (!shipmentSpecialServicesTypes.Contains(ShipmentSpecialServiceType.EMAIL_NOTIFICATION))
            {
                shipmentSpecialServicesTypes.Add(ShipmentSpecialServiceType.EMAIL_NOTIFICATION);
            }

            requestedShipment.SpecialServicesRequested.SpecialServiceTypes = shipmentSpecialServicesTypes.ToArray();
        }
    }
}
