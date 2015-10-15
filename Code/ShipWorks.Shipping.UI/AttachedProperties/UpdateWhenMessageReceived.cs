using ShipWorks.Core.Messaging;
using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

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
                .Subscribe(x =>
                {
                    BindingOperations.GetBindingExpressionBase(control, ItemsControl.ItemsSourceProperty)?.UpdateTarget();
                    ResetSelection(control as Selector);
                });

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

        /// <summary>
        /// Reset the selection of the items control so it matches the item in the collection
        /// </summary>
        /// <remarks>
        /// If we don't clear the selected value first, the control 
        /// doesn't think anything has changed when we update the binding
        /// </remarks>
        private static void ResetSelection(Selector selector)
        {
            if (selector == null)
            {
                return;
            }
            
            selector.SetCurrentValue(Selector.SelectedValueProperty, null);
            BindingOperations.GetBindingExpressionBase(selector, Selector.SelectedValueProperty)?.UpdateTarget();

            UpdateComboBoxText(selector as ComboBox);
        }

        /// <summary>
        /// Update the displayed text of the combo box
        /// </summary>
        /// <remarks>
        /// The Text property updates correctly when we reset the selection, 
        /// but the displayed text does not
        /// </remarks>
        private static void UpdateComboBoxText(ComboBox comboBox)
        {
            if (comboBox == null)
            {
                return;
            }

            SetSelectedText(comboBox, comboBox.Text);
        }

        /// <summary>
        /// Set the selected text of the combobox
        /// </summary>
        /// <remarks>
        /// This is necessary because updating the list of items doesn't update the text of the selected item.
        /// I believe this is because the selected value of </remarks>
        private static bool SetSelectedText(DependencyObject element, string textValue)
        {
            ContentPresenter contentPresenter = element as ContentPresenter;
            int childCount = VisualTreeHelper.GetChildrenCount(element);

            if (contentPresenter != null && childCount > 0)
            {
                TextBlock textBlock = VisualTreeHelper.GetChild(contentPresenter, 0) as TextBlock ;
                textBlock.SetCurrentValue(TextBlock.TextProperty, textValue);
                return true;
            }

            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(element, i) as DependencyObject;
                if (SetSelectedText(child, textValue))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
