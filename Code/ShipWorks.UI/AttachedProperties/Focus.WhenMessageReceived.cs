using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using Interapptive.Shared.Messaging;
using ShipWorks.Core.Messaging;

namespace ShipWorks.UI.AttachedProperties
{
    /// <summary>
    /// Set focus on a control
    /// </summary>
    public partial class Focus
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty WhenMessageReceivedProperty =
            DependencyProperty.RegisterAttached("WhenMessageReceived", typeof(Type), typeof(Focus), new PropertyMetadata(WhenMessageReceivedSetCallback));

        private static readonly DependencyProperty SubscriptionProperty =
            DependencyProperty.RegisterAttached("Subscription", typeof(IDisposable), typeof(Focus));

        /// <summary>
        /// Set the startup property
        /// </summary>
        public static void SetWhenMessageReceived(UIElement element, Type value) =>
            element.SetValue(WhenMessageReceivedProperty, value);

        /// <summary>
        /// Get the startup property
        /// </summary>
        public static Type GetWhenMessageReceived(UIElement element) =>
            (Type) element.GetValue(WhenMessageReceivedProperty);

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        private static IDisposable GetSubscription(DependencyObject d) =>
            (IDisposable) d.GetValue(SubscriptionProperty);

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        private static void SetSubscription(DependencyObject d, IDisposable value) =>
            d.SetValue(SubscriptionProperty, value);

        /// <summary>
        /// Dispose the subscription when the control is unloaded
        /// </summary>
        private static void DisposeSubscription(object sender, RoutedEventArgs e) =>
            RemoveExistingSubscription(sender as DependencyObject);

        /// <summary>
        /// Remove the old handler, if there was one
        /// </summary>
        private static void RemoveExistingSubscription(DependencyObject control) =>
            GetSubscription(control)?.Dispose();

        /// <summary>
        /// Handle when the startup property changes
        /// </summary>
        private static void WhenMessageReceivedSetCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement control = d as FrameworkElement;
            if (control == null)
            {
                return;
            }

            RemoveExistingSubscription(control);
            control.Unloaded -= DisposeSubscription;

            Type messageType = e.NewValue as Type;
            if (messageType == null)
            {
                return;
            }

            if (messageType.IsAssignableFrom(typeof(IShipWorksMessage)))
            {
                throw new InvalidOperationException("MessageType must be an implementation of IShipWorksMessage");
            }

            IDisposable subscription = Messenger.Current.AsObservable()
                .Where(x => x.GetType() == messageType)
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(x =>
                {
                    control.Focus();
                    Keyboard.Focus(control);
                });

            SetSubscription(control, subscription);

            control.Unloaded += DisposeSubscription;
        }
    }
}
