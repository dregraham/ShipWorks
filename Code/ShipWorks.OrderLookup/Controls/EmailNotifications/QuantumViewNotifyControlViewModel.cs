using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.EmailNotifications
{
    /// <summary>
    /// View model for the QuantumViewNotifyControl
    /// </summary>
    [KeyedComponent(typeof(IEmailNotificationsViewModel), ShipmentTypeCode.UpsOnLineTools)]
    [WpfView(typeof(QuantumViewNotifyControl))]
    public class QuantumViewNotifyControlViewModel : OrderLookupViewModelBase, IEmailNotificationsViewModel
    {
        private Dictionary<int, string> subjectTypes;

        /// <summary>
        /// Ctor
        /// </summary>
        public QuantumViewNotifyControlViewModel(IOrderLookupShipmentModel shipmentModel) : base(shipmentModel)
        {
            SubjectTypes = EnumHelper.GetEnumList<UpsEmailNotificationSubject>()
                                     .ToDictionary(x => (int) x.Value, x => x.Description);
        }

        /// <summary>
        /// Panel ID
        /// </summary>
        public override SectionLayoutIDs PanelID => SectionLayoutIDs.UPSQuantumViewNotify;

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override string Title { get; protected set; } = "Quantum View Notify";

        /// <summary>
        /// Subject types
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<int, string> SubjectTypes
        {
            get => subjectTypes;
            set => Handler.Set(nameof(SubjectTypes), ref subjectTypes, value);
        }

        /// <summary>
        /// Sender notifications flag
        /// </summary>
        [Obfuscation(Exclude = true)]
        public UpsEmailNotificationType Sender
        {
            get => (UpsEmailNotificationType) ShipmentModel.ShipmentAdapter.Shipment.Ups.EmailNotifySender;
            set => ShipmentModel.ShipmentAdapter.Shipment.Ups.EmailNotifySender =
                UpdateNotificationFlag(ShipmentModel.ShipmentAdapter.Shipment.Ups.EmailNotifySender, value);
        }

        /// <summary>
        /// Recipient notifications flag
        /// </summary>
        [Obfuscation(Exclude = true)]
        public UpsEmailNotificationType Recipient
        {
            get => (UpsEmailNotificationType) ShipmentModel.ShipmentAdapter.Shipment.Ups.EmailNotifyRecipient;
            set => ShipmentModel.ShipmentAdapter.Shipment.Ups.EmailNotifyRecipient =
                UpdateNotificationFlag(ShipmentModel.ShipmentAdapter.Shipment.Ups.EmailNotifyRecipient, value);
        }

        /// <summary>
        /// Other notifications flag
        /// </summary>
        [Obfuscation(Exclude = true)]
        public UpsEmailNotificationType Other
        {
            get => (UpsEmailNotificationType) ShipmentModel.ShipmentAdapter.Shipment.Ups.EmailNotifyOther;
            set => ShipmentModel.ShipmentAdapter.Shipment.Ups.EmailNotifyOther =
                UpdateNotificationFlag(ShipmentModel.ShipmentAdapter.Shipment.Ups.EmailNotifyOther, value);
        }

        /// <summary>
        /// Update the current UpsEmailNotificationType flag's value with the flag that changed
        /// </summary>
        private int UpdateNotificationFlag(int currentValue, UpsEmailNotificationType valueChanged)
        {
            if (((UpsEmailNotificationType) currentValue).HasFlag(valueChanged))
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