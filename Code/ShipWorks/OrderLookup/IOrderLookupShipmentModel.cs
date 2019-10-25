﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Represents the Order Lookup Data Service
    /// </summary>
    [Obfuscation(ApplyToMembers = true, Exclude = true, StripAfterObfuscation = false)]
    public interface IOrderLookupShipmentModel : IDisposable
    {
        /// <summary>
        /// Field layout repo
        /// </summary>
        IOrderLookupFieldLayoutProvider FieldLayoutProvider { get; }

        /// <summary>
        /// The order that's in context
        /// </summary>
        OrderEntity SelectedOrder { get; }

        /// <summary>
        /// Does the Shipment allow editing
        /// </summary>
        bool ShipmentAllowEditing { get; }

        /// <summary>
        /// The order's shipment adapter
        /// </summary>
        ICarrierShipmentAdapter ShipmentAdapter { get; }

        /// <summary>
        /// The package adapters for the order in context
        /// </summary>
        IEnumerable<IPackageAdapter> PackageAdapters { get; }

        /// <summary>
        /// Event raised when an order property changes
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise property changed event when an order property changes
        /// </summary>
        void RaisePropertyChanged(string propertyName);

        /// <summary>
        /// Saves changes to the database
        /// </summary>
        void SaveToDatabase();

        /// <summary>
        /// Refresh the shipment from the database
        /// </summary>
        void RefreshShipmentFromDatabase();

        /// <summary>
        /// Unload the order
        /// </summary>
        void Unload();

        /// <summary>
        /// Unload the order with reason
        /// </summary>
        void Unload(OrderClearReason reason);

        /// <summary>
        /// Fires when an order is cleared
        /// </summary>
        event EventHandler OnSearchOrder;

        /// <summary>
        /// Total cost of the shipment
        /// </summary>
        decimal TotalCost { get; set; }

        /// <summary>
        /// Keep track of the original ShipmentTypeCode so we can ensure its in the list of providers
        /// </summary>
        ShipmentTypeCode OriginalShipmentTypeCode { get; }

        /// <summary>
        /// Keep track of the SelectedRate
        /// </summary>
        RateResult SelectedRate { get; set; }

        /// <summary>
        /// Can the view accept focus
        /// </summary>
        Func<bool> CanAcceptFocus { get; set; }

        /// <summary>
        /// Wrapper for creating a label
        /// </summary>
        Func<Func<bool>, Task> CreateLabelWrapper { get; set; }

        /// <summary>
        /// Changes the shipment type
        /// </summary>
        void ChangeShipmentType(ShipmentTypeCode value);

        /// <summary>
        /// Load the order
        /// </summary>
        void LoadOrder(OrderEntity order);

        /// <summary>
        /// Create the label for an order
        /// </summary>
        Task CreateLabel();

        /// <summary>
        /// Register the profile handler
        /// </summary>
        void RegisterProfileHandler(Func<Func<ShipmentEntity>, Action<IShippingProfile>, IDisposable> profileRegistration);

        /// <summary>
        /// Wire a property changed event on an INotifyPropertyChanged object
        /// </summary>
        void WirePropertyChangedEvent(INotifyPropertyChanged eventObject);

        /// <summary>
        /// Unwire property changed event on an INotifyPropertyChanged object
        /// </summary>
        void UnwirePropertyChangedEvent(INotifyPropertyChanged eventObject);

        /// <summary>
        /// A shipment is starting to unload
        /// </summary>
        event EventHandler ShipmentUnloading;

        /// <summary>
        /// A shipment is starting to load
        /// </summary>
        event EventHandler ShipmentLoading;

        /// <summary>
        /// A shipment needs binding
        /// </summary>
        event EventHandler ShipmentNeedsBinding;

        /// <summary>
        /// A shipment was fully loaded
        /// </summary>
        event EventHandler ShipmentLoaded;

        /// <summary>
        /// Raise after the shipment is loaded
        /// </summary>
        event EventHandler ShipmentLoadedComplete;

        /// <summary>
        /// Apply the profile to the current shipment
        /// </summary>
        bool ApplyProfile(IShippingProfile profile);
    }
}