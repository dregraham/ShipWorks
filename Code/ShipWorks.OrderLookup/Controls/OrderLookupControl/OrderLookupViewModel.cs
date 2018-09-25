using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using Autofac.Features.Indexed;
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

        ObservableCollection<INotifyPropertyChanged> column1;
        ObservableCollection<INotifyPropertyChanged> column2;
        ObservableCollection<INotifyPropertyChanged> column3;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupViewModel(IOrderLookupMessageBus messageBus,
            OrderLookupSearchViewModel orderLookupSearchViewModel,
            IIndex<OrderLookupPanels, INotifyPropertyChanged> lookupPanels)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            this.messageBus = messageBus;
            messageBus.PropertyChanged += UpdateOutput;
            OrderLookupSearchViewModel = orderLookupSearchViewModel;

            Column1 = new ObservableCollection<INotifyPropertyChanged>(new List<INotifyPropertyChanged>()
            {
                lookupPanels[OrderLookupPanels.From],
                lookupPanels[OrderLookupPanels.To]
            });

            Column2 = new ObservableCollection<INotifyPropertyChanged>()
            {
                lookupPanels[OrderLookupPanels.ShipmentDetails],
                lookupPanels[OrderLookupPanels.LabelOptions],
                lookupPanels[OrderLookupPanels.Reference]
            };

            Column3 = new ObservableCollection<INotifyPropertyChanged>()
            {
                lookupPanels[OrderLookupPanels.Rates],
                lookupPanels[OrderLookupPanels.Customs]
            };
        }

        /// <summary>
        /// View Model for the search section of the OrderLookup UI Mode
        /// </summary>
        [Obfuscation(Exclude = true)]
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
        /// Order Number to search for
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<INotifyPropertyChanged> Column1
        {
            get => column1;
            set => handler.Set(nameof(Column1), ref column1, value);
        }

        /// <summary>
        /// Order Number to search for
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<INotifyPropertyChanged> Column2
        {
            get => column2;
            set => handler.Set(nameof(Column2), ref column2, value);
        }

        /// <summary>
        /// Order Number to search for
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<INotifyPropertyChanged> Column3
        {
            get => column3;
            set => handler.Set(nameof(Column3), ref column3, value);
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
