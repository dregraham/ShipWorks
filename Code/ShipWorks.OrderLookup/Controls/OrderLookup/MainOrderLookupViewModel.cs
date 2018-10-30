﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;
using Autofac;
using GongSolutions.Wpf.DragDrop;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.OrderLookup.Layout;

namespace ShipWorks.OrderLookup.Controls.OrderLookup
{
    /// <summary>
    /// Main view model for the OrderLookup UI Mode
    /// </summary>
    [Component(RegisterAs = RegistrationType.Self)]
    public class MainOrderLookupViewModel : INotifyPropertyChanged, IDisposable, IMainOrderLookupViewModel, IDropTarget
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private readonly IDisposable subscriptions;

        private ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>> leftColumn;
        private ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>> middleColumn;
        private ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>> rightColumn;
        private ILifetimeScope innerScope;
        private GridLength rightColumnWidth;
        private GridLength leftColumnWidth;
        private readonly IOrderLookupLayout layout;
        private readonly ILifetimeScope scope;
        private readonly IDropTarget dropTarget;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainOrderLookupViewModel(IOrderLookupShipmentModel shipmentModel,
            OrderLookupSearchViewModel orderLookupSearchViewModel,
            IOrderLookupLayout layout,
            ILifetimeScope scope,
            IObservable<IShipWorksMessage> messages,
            IDropTarget dropTarget)
        {
            this.scope = scope;
            this.dropTarget = dropTarget;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            ShipmentModel = shipmentModel;
            ShipmentModel.ShipmentUnloading += OnShipmentModelShipmentUnloading;
            ShipmentModel.ShipmentLoading += OnShipmentModelShipmentLoading;
            ShipmentModel.ShipmentLoaded += OnShipmentModelShipmentLoaded;
            OrderLookupSearchViewModel = orderLookupSearchViewModel;
            this.layout = layout;
            layout.Apply(this, scope);
            LeftColumn.Concat(MiddleColumn).Concat(RightColumn).ForEach(p => p.PropertyChanged += PanelPropertyChanged);
            

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
        /// Save state when expansion mode changes
        /// </summary>
        private void PanelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderLookupViewModelPanel<IOrderLookupViewModel>.Expanded))
            {
                layout.Save(this);
            }
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

            List<SectionLayout> sectionLayouts = ShipmentModel.FieldLayoutRepository.Fetch().ToList();

            LeftColumn
                .Concat(MiddleColumn)
                .Concat(RightColumn)
                .ForEach(x =>
                {
                    x.UpdateViewModel(ShipmentModel, innerScope);
                    x.Visible = sectionLayouts.FirstOrDefault(sl => sl.Id == x.Context?.PanelID)?.Selected == true;
                });
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
        /// Width of the left column
        /// </summary>
        [Obfuscation(Exclude = true)]
        public GridLength LeftColumnWidth
        {
            get => leftColumnWidth;
            set
            {
                // This gets rid of the star that is part of the default layout.
                if (value.Value > 1 && value.IsStar)
                {
                    value = new GridLength(value.Value);
                }

                handler.Set(nameof(LeftColumnWidth), ref leftColumnWidth, value);
                layout.Save(this);
            }
        }

        /// <summary>
        /// Width of the middle column
        /// </summary>
        [Obfuscation(Exclude = true)]
        public GridLength RightColumnWidth
        {
            get => rightColumnWidth;
            set
            {
                // This gets rid of the star that is part of the default layout.
                if (value.Value > 1 && value.IsStar)
                {
                    value = new GridLength(value.Value);
                }

                handler.Set(nameof(RightColumnWidth), ref rightColumnWidth, value);
                layout.Save(this);
            }
        }

        /// <summary>
        /// Updates the current drag state
        /// </summary>
        public void DragOver(IDropInfo dropInfo)
        {
            dropTarget.DragOver(dropInfo);
        }

        /// <summary>
        /// Performs a drop
        /// </summary>
        /// <param name="dropInfo"></param>
        public void Drop(IDropInfo dropInfo)
        {
            dropTarget.Drop(dropInfo);

            layout.Save(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            LeftColumn.Concat(MiddleColumn).Concat(RightColumn).ForEach(p => p.PropertyChanged -= PanelPropertyChanged);
            ShipmentModel.ShipmentUnloading -= OnShipmentModelShipmentUnloading;
            ShipmentModel.ShipmentLoading -= OnShipmentModelShipmentLoading;
            ShipmentModel.ShipmentLoaded -= OnShipmentModelShipmentLoaded;
            subscriptions?.Dispose();
            innerScope?.Dispose();
        }
    }
}
