using ShipWorks.Core.Messaging;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ShipWorks.Shipping.UI.AttachedProperties
{
    /// <summary>
    /// Update a binding when a specific message is received
    /// </summary>
    public class UpdateWhenMessageReceived : DependencyObject
    {
        /// <summary>
        /// Message type dependency property
        /// </summary>
        public static readonly DependencyProperty MessageTypeProperty = DependencyProperty.RegisterAttached("MessageType", typeof(Type),
                typeof(UpdateWhenMessageReceived), new PropertyMetadata(null, OnMessageTypeChanged));

        private static readonly DependencyProperty SubscriptionProperty = 
            DependencyProperty.RegisterAttached("Subscription", typeof(IDisposable), typeof(UpdateWhenMessageReceived));

        /// <summary>
        /// Handle when the specific message type changes
        /// </summary>
        private static void OnMessageTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ItemsControl control = d as ItemsControl;
            if (control == null)
            {
                return;
            }

            RemoveExistingSubscription(control);

            Type messageType = e.NewValue as Type;
            if (messageType.IsAssignableFrom(typeof(IShipWorksMessage)))
            {
                throw new InvalidOperationException("MessageType must be an implementation of IShipWorksMessage");
            }
            
            IDisposable subscription = Messenger.Current.AsObservable<IShipWorksMessage>()
                .Where(x => x.GetType() == messageType)
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(x => BindingOperations.GetBindingExpressionBase(control, ItemsControl.ItemsSourceProperty)?.UpdateTarget());

            SetSubscription(control, subscription);

            control.Unloaded -= DisposeSubscription;
            control.Unloaded += DisposeSubscription;
        }

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        public static Type GetMessageType(DependencyObject d) => (Type)d.GetValue(MessageTypeProperty);

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        public static void SetMessageType(DependencyObject d, Type value) => d.SetValue(MessageTypeProperty, value);

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        private static IDisposable GetSubscription(DependencyObject d) => (IDisposable)d.GetValue(SubscriptionProperty);

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        private static void SetSubscription(DependencyObject d, IDisposable value) => d.SetValue(SubscriptionProperty, value);

        /// <summary>
        /// Dispose the subscription when the control is unloaded
        /// </summary>
        private static void DisposeSubscription(object sender, RoutedEventArgs e) => RemoveExistingSubscription(sender as DependencyObject);

        /// <summary>
        /// Remove the old handler, if there was one
        /// </summary>
        private static void RemoveExistingSubscription(DependencyObject control) => GetSubscription(control)?.Dispose();
    }
}
