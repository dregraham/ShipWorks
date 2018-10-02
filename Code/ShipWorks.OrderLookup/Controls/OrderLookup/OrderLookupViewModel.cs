using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;

namespace ShipWorks.OrderLookup.Controls.OrderLookup
{
    /// <summary>
    /// Main view model for the OrderLookup UI Mode
    /// </summary>
    [Component(RegisterAs = RegistrationType.Self)]
    public class OrderLookupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;

        ObservableCollection<INotifyPropertyChanged> column1;
        ObservableCollection<INotifyPropertyChanged> column2;
        ObservableCollection<INotifyPropertyChanged> column3;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupViewModel(IViewModelOrchestrator orchestrator,
            OrderLookupSearchViewModel orderLookupSearchViewModel,
            IIndex<OrderLookupPanels, INotifyPropertyChanged> lookupPanels)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            Orchestrator = orchestrator;
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
        public IViewModelOrchestrator Orchestrator { get; private set; }
    }
}
