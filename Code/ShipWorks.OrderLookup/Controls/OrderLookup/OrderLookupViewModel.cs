using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Messaging.Messages;
using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;

namespace ShipWorks.OrderLookup.Controls.OrderLookup
{
    /// <summary>
    /// Main view model for the OrderLookup UI Mode
    /// </summary>
    [Component(RegisterAs = RegistrationType.Self)]
    public class OrderLookupViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private bool showColumns = false;
        private readonly IDisposable subscriptions;
        private readonly IObservable<IShipWorksMessage> messages;

        private ObservableCollection<INotifyPropertyChanged> leftColumn;
        private ObservableCollection<INotifyPropertyChanged> middleColumn;
        private ObservableCollection<INotifyPropertyChanged> rightColumn;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupViewModel(IOrderLookupShipmentModel shipmentModel,
            OrderLookupSearchViewModel orderLookupSearchViewModel,
            IIndex<OrderLookupPanels, INotifyPropertyChanged> lookupPanels,
            IObservable<IShipWorksMessage> messages)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            ShipmentModel = shipmentModel;
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
            
            subscriptions = new CompositeDisposable(
                messages.OfType<ShipmentSelectionChangedMessage>()
                    .Subscribe(_ => handler.RaisePropertyChanged(nameof(ShowColumns))),
                messages.OfType<OrderLookupClearOrderMessage>()
                    .Subscribe(_ => handler.RaisePropertyChanged(nameof(ShowColumns)))
                );
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
        /// View model ShipmentModel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupShipmentModel ShipmentModel { get; }

        /// <summary>
        /// Should the columns be displayed?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Visibility ShowColumns => 
            ShipmentModel?.ShipmentAdapter?.Shipment?.Processed == false ? 
                Visibility.Visible : 
                Visibility.Hidden;

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            subscriptions?.Dispose();
        }
    }
}
