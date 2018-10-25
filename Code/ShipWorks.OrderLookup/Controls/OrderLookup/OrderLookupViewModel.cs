using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;
using Autofac;
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
using ShipWorks.OrderLookup.Controls.OrderItems;
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

        private ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>> leftColumn;
        private ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>> middleColumn;
        private ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>> rightColumn;
        private ILifetimeScope innerScope;
        private readonly ILifetimeScope scope;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupViewModel(IOrderLookupShipmentModel shipmentModel,
            OrderLookupSearchViewModel orderLookupSearchViewModel,
            ILifetimeScope scope,
            IObservable<IShipWorksMessage> messages)
        {
            this.scope = scope;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            ShipmentModel = shipmentModel;
            ShipmentModel.ShipmentUnloading += OnShipmentModelShipmentUnloading;
            ShipmentModel.ShipmentLoading += OnShipmentModelShipmentLoading;
            ShipmentModel.ShipmentLoaded += OnShipmentModelShipmentLoaded;
            OrderLookupSearchViewModel = orderLookupSearchViewModel;

            LeftColumn = new ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>>
            {
                scope.Resolve<IOrderLookupPanelViewModel<IFromViewModel>>(),
                scope.Resolve<IOrderLookupPanelViewModel<IToViewModel>>(),
                scope.Resolve<IOrderLookupPanelViewModel<IOrderItemsViewModel>>()
            };

            MiddleColumn = new ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>>
            {
                scope.Resolve<IOrderLookupPanelViewModel<IDetailsViewModel>>(),
                scope.Resolve<IOrderLookupPanelViewModel<ILabelOptionsViewModel>>(),
                scope.Resolve<IOrderLookupPanelViewModel<IReferenceViewModel>>(),
                scope.Resolve<IOrderLookupPanelViewModel<IEmailNotificationsViewModel>>()
            };

            RightColumn = new ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>>
            {
                scope.Resolve<IOrderLookupPanelViewModel<IRatingViewModel>>(),
                scope.Resolve<IOrderLookupPanelViewModel<ICustomsViewModel>>(),
            };

            subscriptions = new CompositeDisposable(
                messages.OfType<ShipmentSelectionChangedMessage>()
                    .Subscribe(_ => handler.RaisePropertyChanged(nameof(ShowColumns))),
                messages.OfType<OrderLookupClearOrderMessage>()
                    .Subscribe(_ => handler.RaisePropertyChanged(nameof(ShowColumns))),
                messages.OfType<ShipmentsProcessedMessage>()
                    .Where(x => x.Shipments.All(y => y.IsSuccessful))
                    .Subscribe(x => OrderLookupSearchViewModel.ShipmentModel.Unload()),
                messages.OfType<ShipmentsProcessedMessage>()
                    .Where(x => x.Shipments.Any(y => !y.IsSuccessful))
                    .Subscribe(x => OrderLookupSearchViewModel.ShipmentModel.LoadOrder(x.Shipments.FirstOrDefault().Shipment?.Order))
                );
        }

        /// <summary>
        /// A shipment is unloading
        /// </summary>
        private void OnShipmentModelShipmentUnloading(object sender, EventArgs e) => UnloadViewModels();

        /// <summary>
        /// A shipment has fully loaded
        /// </summary>
        private void OnShipmentModelShipmentLoaded(object sender, EventArgs e) => LoadViewModels();

        /// <summary>
        /// A shipment is loading
        /// </summary>
        private void OnShipmentModelShipmentLoading(object sender, EventArgs e) => UnloadViewModels();

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
        public ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>> LeftColumn
        {
            get => leftColumn;
            set => handler.Set(nameof(LeftColumn), ref leftColumn, value);
        }

        /// <summary>
        /// Order Number to search for
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>> MiddleColumn
        {
            get => middleColumn;
            set => handler.Set(nameof(MiddleColumn), ref middleColumn, value);
        }

        /// <summary>
        /// Order Number to search for
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>> RightColumn
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
            ShipmentModel.ShipmentUnloading -= OnShipmentModelShipmentUnloading;
            ShipmentModel.ShipmentLoading -= OnShipmentModelShipmentLoading;
            ShipmentModel.ShipmentLoaded -= OnShipmentModelShipmentLoaded;
            subscriptions?.Dispose();
            innerScope?.Dispose();
        }
    }
}
