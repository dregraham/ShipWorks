using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup;
using ShipWorks.OrderLookup.Controls;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.OrderLookup.Controls
{
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.ShipmentDetails)]
    public class OrderLookupShipmentDetailsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected readonly PropertyChangedHandler handler;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupShipmentDetailsViewModel(IOrderLookupMessageBus messageBus)
		{
            this.MessageBus = messageBus;
            this.MessageBus.PropertyChanged += MessageBusPropertyChanged;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The ViewModel message bus
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupMessageBus MessageBus { get; private set; }

        /// <summary>
        /// Update when order changes
        /// </summary>
        private void MessageBusPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Order" && MessageBus.Order != null)
            {
                handler.RaisePropertyChanged(nameof(MessageBus));
            }
        }






    }
}