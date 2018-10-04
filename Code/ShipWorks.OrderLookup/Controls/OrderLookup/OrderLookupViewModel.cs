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

        private ObservableCollection<INotifyPropertyChanged> leftColumn;
        private ObservableCollection<INotifyPropertyChanged> middleColumn;
        private ObservableCollection<INotifyPropertyChanged> rightColumn;

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

            LeftColumn = new ObservableCollection<INotifyPropertyChanged>(new List<INotifyPropertyChanged>
            {
                lookupPanels[OrderLookupPanels.From],
                lookupPanels[OrderLookupPanels.To]
            });

            MiddleColumn = new ObservableCollection<INotifyPropertyChanged>
            {
                lookupPanels[OrderLookupPanels.ShipmentDetails],
                lookupPanels[OrderLookupPanels.LabelOptions],
                lookupPanels[OrderLookupPanels.Reference]
            };

            RightColumn = new ObservableCollection<INotifyPropertyChanged>
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
        public ObservableCollection<INotifyPropertyChanged> LeftColumn
        {
            get => leftColumn;
            set => handler.Set(nameof(LeftColumn), ref leftColumn, value);
        }

        /// <summary>
        /// Order Number to search for
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<INotifyPropertyChanged> MiddleColumn
        {
            get => middleColumn;
            set => handler.Set(nameof(MiddleColumn), ref middleColumn, value);
        }

        /// <summary>
        /// Order Number to search for
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<INotifyPropertyChanged> RightColumn
        {
            get => rightColumn;
            set => handler.Set(nameof(RightColumn), ref rightColumn, value);
        }

        /// <summary>
        /// Viewmodel Orchestrator
        /// </summary>
        public IViewModelOrchestrator Orchestrator { get; }
    }
}
