using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;

namespace ShipWorks.OrderLookup.Controls.Reference
{
    /// <summary>
    /// View model for order lookup reference control
    /// </summary>
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.Reference)]
    public class ReferenceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IOrderLookupMessageBus messageBus;
        private readonly PropertyChangedHandler handler;

        /// <summary>
        /// ctor
        /// </summary>
        public ReferenceViewModel(IOrderLookupMessageBus messageBus)
        {
            this.messageBus = messageBus;
            this.messageBus.PropertyChanged += MessageBusPropertyChanged;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The order lookup message bus
        /// </summary
        [Obfuscation(Exclude = true)]
        public IOrderLookupMessageBus MessageBus => messageBus;

        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void MessageBusPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Order" && messageBus.Order != null)
            {
                handler.RaisePropertyChanged(nameof(MessageBus));
            }
        }
    }
}
