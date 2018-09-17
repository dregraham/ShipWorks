using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Newtonsoft.Json;
using ShipWorks.Core.UI;
using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;

namespace ShipWorks.OrderLookup.Controls.OrderLookupControl
{
    /// <summary>
    /// Main view model for the OrderLookup UI Mode
    /// </summary>
    [Component(RegistrationType.Self)]
    public class OrderLookupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private readonly IOrderLookupMessageBus messageBus;
        private string output = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupViewModel(IOrderLookupMessageBus messageBus, OrderLookupSearchViewModel orderLookupSearchViewModel)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            this.messageBus = messageBus;
            messageBus.PropertyChanged += UpdateOutput;
            OrderLookupSearchViewModel = orderLookupSearchViewModel;
        }

        /// <summary>
        /// View Model for the search section of the OrderLookup UI Mode
        /// </summary>
        public OrderLookupSearchViewModel OrderLookupSearchViewModel { get; set; }

        /// <summary>
        /// Order Number to search for
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Output
        {
            get => output;
            set => handler.Set(nameof(Output), ref output, value);
        }

        /// <summary>
        /// Update the order number when the order changes
        /// </summary>
        private void UpdateOutput(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Order")
            {
                if (messageBus.Order != null)
                {
                    Output = JsonConvert.SerializeObject(messageBus.Order);
                }
                else
                {
                    Output = string.Empty;
                }
            }
        }
    }
}
