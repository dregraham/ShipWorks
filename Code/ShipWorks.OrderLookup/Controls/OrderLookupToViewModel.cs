using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;

namespace ShipWorks.OrderLookup.Controls
{
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.To)]
    public class OrderLookupToViewModel : INotifyPropertyChanged
    {
        private readonly IOrderLookupMessageBus messageBus;
        private readonly PropertyChangedHandler handler;

        public event PropertyChangedEventHandler PropertyChanged;

        public OrderLookupToViewModel(IOrderLookupMessageBus messageBus)
        {
            this.messageBus = messageBus;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }
    }
}
