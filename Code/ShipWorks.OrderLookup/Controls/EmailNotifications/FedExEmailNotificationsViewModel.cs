using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.EmailNotifications
{
    /// <summary>
    /// View model for the FedExEmailNotificationsControl
    /// </summary>
    [KeyedComponent(typeof(IEmailNotificationsViewModel), ShipmentTypeCode.FedEx)]
    [WpfView(typeof(FedExEmailNotificationsControl))]
    public class FedExEmailNotificationsViewModel : OrderLookupViewModelBase, IEmailNotificationsViewModel
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public FedExEmailNotificationsViewModel(IOrderLookupShipmentModel shipmentModel) : base(shipmentModel)
        {

        }

        /// <summary>
        /// Panel ID
        /// </summary>
        public override SectionLayoutIDs PanelID => SectionLayoutIDs.FedExEmailNotifications;

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override string Title { get; protected set; } = "Email Notifications";

        /// <summary>
        /// Sender notifications flag
        /// </summary>
        [Obfuscation(Exclude = true)]
        public FedExEmailNotificationType Sender
        {
            get => (FedExEmailNotificationType) ShipmentModel.ShipmentAdapter.Shipment.FedEx.EmailNotifySender;
            set => ShipmentModel.ShipmentAdapter.Shipment.FedEx.EmailNotifySender =
                UpdateNotificationFlag(ShipmentModel.ShipmentAdapter.Shipment.FedEx.EmailNotifySender, value);
        }

        /// <summary>
        /// Recipient notifications flag
        /// </summary>
        [Obfuscation(Exclude = true)]
        public FedExEmailNotificationType Recipient
        {
            get => (FedExEmailNotificationType) ShipmentModel.ShipmentAdapter.Shipment.FedEx.EmailNotifyRecipient;
            set => ShipmentModel.ShipmentAdapter.Shipment.FedEx.EmailNotifyRecipient =
                UpdateNotificationFlag(ShipmentModel.ShipmentAdapter.Shipment.FedEx.EmailNotifyRecipient, value);
        }

        /// <summary>
        /// Other notifications flag
        /// </summary>
        [Obfuscation(Exclude = true)]
        public FedExEmailNotificationType Broker
        {
            get => (FedExEmailNotificationType) ShipmentModel.ShipmentAdapter.Shipment.FedEx.EmailNotifyBroker;
            set => ShipmentModel.ShipmentAdapter.Shipment.FedEx.EmailNotifyBroker =
                UpdateNotificationFlag(ShipmentModel.ShipmentAdapter.Shipment.FedEx.EmailNotifyBroker, value);
        }

        /// <summary>
        /// Other notifications flag
        /// </summary>
        [Obfuscation(Exclude = true)]
        public FedExEmailNotificationType Other
        {
            get => (FedExEmailNotificationType) ShipmentModel.ShipmentAdapter.Shipment.FedEx.EmailNotifyOther;
            set => ShipmentModel.ShipmentAdapter.Shipment.FedEx.EmailNotifyOther =
                UpdateNotificationFlag(ShipmentModel.ShipmentAdapter.Shipment.FedEx.EmailNotifyOther, value);
        }

        /// <summary>
        /// Update the current FedExEmailNotificationType flag's value with the flag that changed
        /// </summary>
        private int UpdateNotificationFlag(int currentValue, FedExEmailNotificationType valueChanged)
        {
            if (((FedExEmailNotificationType) currentValue).HasFlag(valueChanged))
            {
                currentValue &= ~(int) valueChanged;
            }
            else
            {
                currentValue |= (int) valueChanged;
            }

            return currentValue;
        }
    }
}