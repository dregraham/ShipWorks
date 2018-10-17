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
    public class QuantumViewNotifyControlViewModel : IQuantumViewNotifyControlViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private Dictionary<int, string> subjectTypes;
        private bool? notifySenderOnShip = false;
        private bool? notifySenderOnDeliver = false;
        private bool? notifySenderOnException = false;

        public QuantumViewNotifyControlViewModel(IOrderLookupShipmentModel shipmentModel)
        {
            ShipmentModel = shipmentModel;
            ShipmentModel.PropertyChanged += ShipmentModelPropertyChanged;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            SubjectTypes = EnumHelper.GetEnumList<UpsEmailNotificationSubject>()
                .Select(x => x.Value).ToDictionary(s => (int) s, s => EnumHelper.GetDescription(s));
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
        /// Notify sender on ship
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool? NotifySenderOnShip
        {
            get => notifySenderOnShip;
            set
            {
                handler.Set(nameof(NotifySenderOnShip), ref notifySenderOnShip, value);
                
//                if (value)
//                {
//                    ShipmentModel.ShipmentAdapter.Shipment.Ups.EmailNotifySender |= (int) UpsEmailNotificationType.Ship;
//                }
//                else
//                {
//                    ShipmentModel.ShipmentAdapter.Shipment.Ups.EmailNotifySender &= ~ (int) UpsEmailNotificationType.Ship;
//                }
            }
        }
        
        /// <summary>
        /// Notify sender on delivery
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool? NotifySenderOnDeliver
        {
            get => notifySenderOnDeliver;
            set => handler.Set(nameof(NotifySenderOnDeliver), ref notifySenderOnDeliver, value);        
        }
        
        /// <summary>
        /// Notify sender on exception
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool? NotifySenderOnException
        {
            get => notifySenderOnException;
            set => handler.Set(nameof(NotifySenderOnException), ref notifySenderOnException, value);        
        }
        
        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void ShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ShipmentModel.SelectedOrder != null)
            {
                if (e.PropertyName == nameof(ShipmentModel.SelectedOrder))
                {
//                    UpdateVisibility();
//                    LoadNotifications(ShipmentModel.ShipmentAdapter.Shipment.Ups);

                    handler.RaisePropertyChanged(nameof(ShipmentModel));
                }

                if (e.PropertyName == nameof(ShipmentModel.ShipmentAdapter.ShipmentTypeCode))
                {
//                    UpdateVisibility();
//                    LoadNotifications(ShipmentModel.ShipmentAdapter.Shipment.Ups);
                }
            }
        }

//        private void LoadNotifications(UpsShipmentEntity shipment)
//        {
//            UpsEmailNotificationType notifySender = (UpsEmailNotificationType) shipment.EmailNotifySender;
//            UpsEmailNotificationType notifyRecipient = (UpsEmailNotificationType) shipment.EmailNotifyRecipient;
//            UpsEmailNotificationType notifyOther = (UpsEmailNotificationType) shipment.EmailNotifyOther;
//
//            NotifySenderOnShip = notifySender.HasFlag(UpsEmailNotificationType.Ship);
//            NotifySenderOnDeliver = notifySender.HasFlag(UpsEmailNotificationType.Deliver);
//            NotifySenderOnException = notifySender.HasFlag(UpsEmailNotificationType.Exception);
//        }

//        private void UpdateVisibility() =>
//            Visible = ShipmentModel.ShipmentAdapter.ShipmentTypeCode == ShipmentTypeCode.UpsOnLineTools;


        public void Dispose() => ShipmentModel.PropertyChanged -= ShipmentModelPropertyChanged;
    }
}