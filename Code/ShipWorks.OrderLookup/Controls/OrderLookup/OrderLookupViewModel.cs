using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;
using Autofac;
using Autofac.Features.Indexed;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.OrderLookup.Controls.Customs;
using ShipWorks.OrderLookup.Controls.EmailNotifications;
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

        private ObservableCollection<IOrderLookupWrapperViewModel<IOrderLookupViewModel>> leftColumn;
        private ObservableCollection<IOrderLookupWrapperViewModel<IOrderLookupViewModel>> middleColumn;
        private ObservableCollection<IOrderLookupWrapperViewModel<IOrderLookupViewModel>> rightColumn;
        private ILifetimeScope innerScope;
        private readonly ILifetimeScope scope;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupViewModel(IOrderLookupShipmentModel shipmentModel,
            OrderLookupSearchViewModel orderLookupSearchViewModel,
            IIndex<OrderLookupPanels, IOrderLookupWrapperViewModel<IOrderLookupViewModel>> lookupPanels,
            ILifetimeScope scope,
            IObservable<IShipWorksMessage> messages)
        {
            this.scope = scope;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            ShipmentModel = shipmentModel;
            ShipmentModel.PropertyChanged += OnShipmentModelPropertyChanged;
            ShipmentModel.OrderUnloading += OnShipmentModelOrderUnloading;
            ShipmentModel.OrderLoading += OnShipmentModelOrderLoading;
            ShipmentModel.OrderLoaded += OnShipmentModelOrderLoaded;
            OrderLookupSearchViewModel = orderLookupSearchViewModel;

            LeftColumn = new ObservableCollection<IOrderLookupWrapperViewModel<IOrderLookupViewModel>>
            {
                scope.Resolve<IOrderLookupWrapperViewModel<IFromViewModel>>(),
                scope.Resolve<IOrderLookupWrapperViewModel<IToViewModel>>(),
            };

            MiddleColumn = new ObservableCollection<IOrderLookupWrapperViewModel<IOrderLookupViewModel>>
            {
                scope.Resolve<IOrderLookupWrapperViewModel<IDetailsViewModel>>(),
                scope.Resolve<IOrderLookupWrapperViewModel<ILabelOptionsViewModel>>(),
                scope.Resolve<IOrderLookupWrapperViewModel<IReferenceViewModel>>(),
                scope.Resolve<IOrderLookupWrapperViewModel<IEmailNotificationsViewModel>>()
            };

            RightColumn = new ObservableCollection<IOrderLookupWrapperViewModel<IOrderLookupViewModel>>
            {
                scope.Resolve<IOrderLookupWrapperViewModel<IRatingViewModel>>(),
                scope.Resolve<IOrderLookupWrapperViewModel<ICustomsViewModel>>(),
            };

            subscriptions = new CompositeDisposable(
                messages.OfType<ShipmentSelectionChangedMessage>()
                    .Subscribe(_ => handler.RaisePropertyChanged(nameof(ShowColumns))),
                messages.OfType<OrderLookupClearOrderMessage>()
                    .Subscribe(_ => handler.RaisePropertyChanged(nameof(ShowColumns))),
                messages.OfType<ShipmentsProcessedMessage>()
                    .Subscribe(x => OrderLookupSearchViewModel.ShipmentModel.LoadOrder(x.Shipments.FirstOrDefault().Shipment?.Order))
                );
        }

        /// <summary>
        /// An order is unloading
        /// </summary>
        private void OnShipmentModelOrderUnloading(object sender, EventArgs e) => UnloadViewModels();

        /// <summary>
        /// An order has fully loaded
        /// </summary>
        private void OnShipmentModelOrderLoaded(object sender, EventArgs e) => LoadViewModels();

        /// <summary>
        /// An order is loading
        /// </summary>
        private void OnShipmentModelOrderLoading(object sender, EventArgs e) => UnloadViewModels();

        /// <summary>
        /// Handle changing of shipments or shipment types
        /// </summary>
        private void OnShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderLookupShipmentModel))  // The provider has changed on the shipment
            {
                UnloadViewModels();
                LoadViewModels();
            }
        }

        /// <summary>
        /// Load the view models
        /// </summary>
        private void LoadViewModels()
        {
            // This shouldn't be necessary, but it's safe to do so I'd rather be safe than sorry
            UnloadViewModels();

            innerScope = scope.BeginLifetimeScope();

            LeftColumn
                .Concat(MiddleColumn)
                .Concat(RightColumn)
                .ForEach(x => x.UpdateViewModel(ShipmentModel, innerScope));
        }

        /// <summary>
        /// Unload the view models
        /// </summary>
        private void UnloadViewModels() => innerScope?.Dispose();

        /// <summary>
        /// View Model for the search section of the OrderLookup UI Mode
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OrderLookupSearchViewModel OrderLookupSearchViewModel { get; set; }

        /// <summary>
        /// Order Number to search for
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<IOrderLookupWrapperViewModel<IOrderLookupViewModel>> LeftColumn
        {
            get => leftColumn;
            set => handler.Set(nameof(LeftColumn), ref leftColumn, value);
        }

        /// <summary>
        /// Order Number to search for
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<IOrderLookupWrapperViewModel<IOrderLookupViewModel>> MiddleColumn
        {
            get => middleColumn;
            set => handler.Set(nameof(MiddleColumn), ref middleColumn, value);
        }

        /// <summary>
        /// Order Number to search for
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<IOrderLookupWrapperViewModel<IOrderLookupViewModel>> RightColumn
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
            ShipmentModel.PropertyChanged -= OnShipmentModelPropertyChanged;
            ShipmentModel.OrderLoading -= OnShipmentModelOrderLoading;
            ShipmentModel.OrderLoaded -= OnShipmentModelOrderLoaded;
            subscriptions?.Dispose();
            innerScope?.Dispose();
        }
    }
}
