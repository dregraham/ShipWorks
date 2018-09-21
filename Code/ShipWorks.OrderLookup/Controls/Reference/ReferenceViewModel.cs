using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;

namespace ShipWorks.OrderLookup.Controls.Reference
{
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.Reference)]
    public class ReferenceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IOrderLookupMessageBus messageBus;
        protected readonly PropertyChangedHandler handler;

        public ReferenceViewModel(IOrderLookupMessageBus messageBus)
        {
            this.messageBus = messageBus;
            this.messageBus.PropertyChanged += MessageBusPropertyChanged;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Is address validation enabled or not
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupMessageBus MessageBus
        {
            get { return messageBus; }
        }

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
