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

            ShipmentEventNotificationDetail eventNotificationDetail = new ShipmentEventNotificationDetail();
            requestedShipment.SpecialServicesRequested.EventNotificationDetail = eventNotificationDetail;

            // Set the special message
            if (!string.IsNullOrEmpty(fedExShipment.EmailNotifyMessage))
            {
                eventNotificationDetail.PersonalMessage = fedExShipment.EmailNotifyMessage;
            }

            List<ShipmentEventNotificationSpecification> eventNotificationSpecifications = new List<ShipmentEventNotificationSpecification>();

            // See if any are being sent to the sender
            if (notifySender)
            {
                ShipmentEventNotificationSpecification notification = CreateShipmentEventNotification(ShipmentNotificationRoleType.SHIPPER, shipment.OriginEmail, fedExShipment.EmailNotifySender);
                eventNotificationSpecifications.Add(notification);
            }

            // See if any are being sent to the recipient
            if (notifyRecipient)
            {
                ShipmentEventNotificationSpecification notification = CreateShipmentEventNotification(ShipmentNotificationRoleType.RECIPIENT, shipment.ShipEmail, fedExShipment.EmailNotifyRecipient);
                eventNotificationSpecifications.Add(notification);
            }

            // See if any are being sent to other
            if (notifyOther)
            {
                ShipmentEventNotificationSpecification notification = CreateShipmentEventNotification(ShipmentNotificationRoleType.OTHER, fedExShipment.EmailNotifyOtherAddress, fedExShipment.EmailNotifyOther);
                eventNotificationSpecifications.Add(notification);
            }

            // See if any are being sent to broker
            if (notifyBroker)
            {
                ShipmentEventNotificationSpecification notification = CreateShipmentEventNotification(ShipmentNotificationRoleType.BROKER, fedExShipment.BrokerEmail, fedExShipment.EmailNotifyBroker);
                eventNotificationSpecifications.Add(notification);
            }

            eventNotificationDetail.EventNotifications = eventNotificationSpecifications.ToArray();
        }

        /// <summary>
        /// Populates and returns an EMailNotificationRecipient
        /// </summary>
        /// <param name="roleType">The EMailNotificationRecipientType for this EMailNotificationRecipient</param>
        /// <param name="emailAddress">The email address for the notification</param>
        /// <param name="notificationTypes">The ShipWorks FedExEmailNotificationType enum, as an int, containing all notification types to return.</param>
        /// <returns></returns>
        private static ShipmentEventNotificationSpecification CreateShipmentEventNotification(ShipmentNotificationRoleType roleType, string emailAddress, int notificationTypes)
        {
            ShipmentEventNotificationSpecification notification = new ShipmentEventNotificationSpecification
            {
                    FormatSpecification = new ShipmentNotificationFormatSpecification()
                    {
                        Type = NotificationFormatType.HTML,
                        TypeSpecified = true
                    },
                    NotificationDetail = new NotificationDetail()
                    {
                        Localization = new Localization { LanguageCode = "EN" },
                        NotificationType = NotificationType.EMAIL,
                        NotificationTypeSpecified = true,
                        EmailDetail = new EMailDetail()
                        {
                            EmailAddress = emailAddress
                        }
                    },
                    Role = roleType,
                    RoleSpecified = true
                };

            ApplyEmailRecipientOptions(notification, notificationTypes);

            return notification;
        }

        /// <summary>
        /// Apply the given notification types to the recipient
        /// </summary>
        private static void ApplyEmailRecipientOptions(ShipmentEventNotificationSpecification recipient, int notifcationTypes)
        {
            List<NotificationEventType> notificationEventTypes = new List<NotificationEventType>();

            if ((notifcationTypes & (int)FedExEmailNotificationType.Ship) != 0)
            {
                notificationEventTypes.Add(NotificationEventType.ON_SHIPMENT);
            }

            if ((notifcationTypes & (int)FedExEmailNotificationType.Exception) != 0)
            {
                notificationEventTypes.Add(NotificationEventType.ON_EXCEPTION);
            }

            if ((notifcationTypes & (int)FedExEmailNotificationType.Deliver) != 0)
            {
                notificationEventTypes.Add(NotificationEventType.ON_DELIVERY);
            }

            if ((notifcationTypes & (int)FedExEmailNotificationType.EstimatedDelivery) != 0)
            {
                notificationEventTypes.Add(NotificationEventType.ON_ESTIMATED_DELIVERY);
            }

            recipient.Events = notificationEventTypes.ToArray();
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

            List<ShipmentSpecialServiceType> shipmentSpecialServicesTypes =
                requestedShipment.SpecialServicesRequested.SpecialServiceTypes?.ToList() ?? new List<ShipmentSpecialServiceType>();

            if (!shipmentSpecialServicesTypes.Contains(ShipmentSpecialServiceType.EVENT_NOTIFICATION))
            {
                shipmentSpecialServicesTypes.Add(ShipmentSpecialServiceType.EVENT_NOTIFICATION);
            }

            requestedShipment.SpecialServicesRequested.SpecialServiceTypes = shipmentSpecialServicesTypes.ToArray();
        }
    }
}
