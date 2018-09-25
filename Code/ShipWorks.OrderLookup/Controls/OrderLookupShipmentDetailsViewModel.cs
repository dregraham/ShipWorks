using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup.Controls
{
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.ShipmentDetails)]
    public class OrderLookupShipmentDetailsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected readonly PropertyChangedHandler handler;
        private readonly IDimensionsManager dimensionsManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupShipmentDetailsViewModel(
            IOrderLookupMessageBus messageBus, 
            IDimensionsManager dimensionsManager)
        {
            MessageBus = messageBus;
            this.dimensionsManager = dimensionsManager;
            MessageBus.PropertyChanged += MessageBusPropertyChanged;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The ViewModel message bus
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupMessageBus MessageBus { get; private set; }

        [Obfuscation(Exclude = true)]
        public bool ProfileSelected
        {
            get => MessageBus.ShipmentAdapter.Shipment.Postal.DimsProfileID > 0;
        }

        /// <summary>
        /// Update when order changes
        /// </summary>
        private void MessageBusPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (MessageBus.Order != null)
            {
                if (e.PropertyName == "Order")
                {
                    handler.RaisePropertyChanged(nameof(MessageBus));
                }

                if (e.PropertyName == "DimsProfileID")
                {
                    Data.Model.EntityClasses.PostalShipmentEntity postal = MessageBus.ShipmentAdapter.Shipment.Postal;
                    if (postal.DimsProfileID > 0)
                    {
                        DimensionsProfileEntity profile = dimensionsManager.GetProfile(postal.DimsProfileID);
                        if (profile != null)
                        {
                            postal.DimsLength = profile.Length;
                            postal.DimsWidth = profile.Width;
                            postal.DimsHeight = profile.Height;
                            postal.DimsWeight = profile.Weight;
                        }
                    }
                    handler.RaisePropertyChanged(nameof(ProfileSelected));
                }
            }


        }
    }
}