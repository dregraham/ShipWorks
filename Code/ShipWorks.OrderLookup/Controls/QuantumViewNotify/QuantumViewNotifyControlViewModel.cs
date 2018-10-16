using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ComponentFactory.Krypton.Toolkit;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.QuantumViewNotify
{
    [KeyedComponent(typeof(IQuantumViewNotifyControlViewModel), ShipmentTypeCode.UpsOnLineTools)]
    [WpfView(typeof(QuantumViewNotifyControl))]
    public class QuantumViewNotifyControlViewModel : IQuantumViewNotifyControlViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private Dictionary<int, string> subjectTypes;

        public QuantumViewNotifyControlViewModel(IOrderLookupShipmentModel shipmentModel)
        {
            ShipmentModel = shipmentModel;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            SubjectTypes = EnumHelper.GetEnumList<UpsEmailNotificationSubject>()
                                     .ToDictionary(x => (int) x.Value, x => x.Description);
        }

        /// <summary>
        /// Is the section expanded
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Expanded { get; set; } = false;

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Title => "Quantum View Notify";

        /// <summary>
        /// Is the section visible
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Visible => true;

        /// <summary>
        /// The ViewModel ShipmentModel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupShipmentModel ShipmentModel { get; }

        /// <summary>
        /// Subject types
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<int, string> SubjectTypes
        {
            get => subjectTypes;
            set => handler.Set(nameof(SubjectTypes), ref subjectTypes, value);
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

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}