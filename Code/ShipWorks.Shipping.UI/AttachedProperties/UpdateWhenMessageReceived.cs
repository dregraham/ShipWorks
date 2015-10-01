using Interapptive.Shared.Messaging;
using System;
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

            RemoveExistingHandlers(control, e.OldValue);

            Type messageType = e.NewValue as Type;
            if (messageType.IsAssignableFrom(typeof(IShipWorksMessage)))
            {
                throw new InvalidOperationException("MessageType must be an implementation of IShipWorksMessage");
            }

            Messenger.Current.Handle(control, messageType, x => 
                BindingOperations.GetBindingExpressionBase(control, ItemsControl.ItemsSourceProperty)?.UpdateTarget());
        }

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        public static Type GetMessageType(DependencyObject d)
        {
            return (Type)d.GetValue(MessageTypeProperty);
        }

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        public static void SetMessageType(DependencyObject d, Type value)
        {
            d.SetValue(MessageTypeProperty, value);
        }

        /// <summary>
        /// Remove the old handler, if there was one
        /// </summary>
        private static void RemoveExistingHandlers(ItemsControl control, object oldValue)
        {
            Type oldMessageType = oldValue as Type;
            if (oldMessageType != null)
            {
                Messenger.Current.Remove(control, oldMessageType);
            }
        }
    }
}
