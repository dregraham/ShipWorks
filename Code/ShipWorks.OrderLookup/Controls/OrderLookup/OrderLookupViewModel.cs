using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;
using Autofac;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Messaging.Messages;
using ShipWorks.OrderLookup.Controls.Customs;
using ShipWorks.OrderLookup.Controls.From;
using ShipWorks.OrderLookup.Controls.LabelOptions;
using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;
using ShipWorks.OrderLookup.Controls.Rating;
using ShipWorks.OrderLookup.Controls.Reference;
using ShipWorks.OrderLookup.Controls.ShipmentDetails;
using ShipWorks.OrderLookup.Controls.To;

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
        private readonly IDisposable subscriptions;

        private ObservableCollection<INotifyPropertyChanged> leftColumn;
        private ObservableCollection<INotifyPropertyChanged> middleColumn;
        private ObservableCollection<INotifyPropertyChanged> rightColumn;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupViewModel(IOrderLookupShipmentModel shipmentModel,
            OrderLookupSearchViewModel orderLookupSearchViewModel,
            IIndex<OrderLookupPanels, INotifyPropertyChanged> lookupPanels,
            ILifetimeScope scope,
            IObservable<IShipWorksMessage> messages)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            ShipmentModel = shipmentModel;
            OrderLookupSearchViewModel = orderLookupSearchViewModel;

            LeftColumn = new ObservableCollection<INotifyPropertyChanged>
            {
                scope.Resolve<IOrderLookupWrapperViewModel<IFromViewModel>>(),
                scope.Resolve<IOrderLookupWrapperViewModel<IToViewModel>>(),
            };

            MiddleColumn = new ObservableCollection<INotifyPropertyChanged>
            {
                scope.Resolve<IOrderLookupWrapperViewModel<IDetailsViewModel>>(),
                scope.Resolve<IOrderLookupWrapperViewModel<ILabelOptionsViewModel>>(),
                scope.Resolve<IOrderLookupWrapperViewModel<IReferenceViewModel>>(),
            };

            RightColumn = new ObservableCollection<INotifyPropertyChanged>
            {
                scope.Resolve<IOrderLookupWrapperViewModel<IRatingViewModel>>(),
                scope.Resolve<IOrderLookupWrapperViewModel<ICustomsViewModel>>(),
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
