using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;
using Autofac.Features.Indexed;

namespace ShipWorks.OrderLookup.Controls
{
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.From)]
    public class OrderLookupFromViewModel : INotifyPropertyChanged
    {
        private readonly IOrderLookupMessageBus messageBus;
        private readonly PropertyChangedHandler handler;

        public event PropertyChangedEventHandler PropertyChanged;

        public OrderLookupFromViewModel(IOrderLookupMessageBus messageBus)
        {
            this.messageBus = messageBus;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }
    }
}
