using Interapptive.Shared.Utility;
using System;
using System.Reflection;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace ShipWorks.UI.Behaviors
{
    /// <summary>
    /// Behavior to allow binding on a Password Box
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class PasswordBoxBindingBehavior : Behavior<PasswordBox>
    {
        /// <summary>
        /// The password property
        /// </summary>
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password",
            typeof(SecureString), typeof(PasswordBoxBindingBehavior), new PropertyMetadata(null, new PropertyChangedCallback(OnBoundPasswordChanged)));

        /// <summary>
        /// Called when [bound password changed].
        /// </summary>
        /// <remarks>
        /// Used to populate the password as the initial value.
        /// </remarks>
        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = ((PasswordBoxBindingBehavior) d).AssociatedObject;
            string newPassword = ((SecureString) e.NewValue).ToInsecureString();
            if (passwordBox.Password != newPassword)
            {
                passwordBox.Password = newPassword;
            }
        }

        /// <summary>
        /// Called when [attached].
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.PasswordChanged += OnPasswordBoxValueChanged;
        }

        /// <summary>
        /// The password
        /// </summary>
        public SecureString Password
        {
            get { return (SecureString) GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value);}
        }

        /// <summary>
        /// Called when [password box value changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnPasswordBoxValueChanged(object sender, RoutedEventArgs e)
        {
            BindingExpression binding = BindingOperations.GetBindingExpression(this, PasswordProperty);

            if (binding != null)
            {
                PropertyInfo property = binding.DataItem.GetType().GetProperty(binding.ParentBinding.Path.Path);

                property?.SetValue(binding.DataItem, AssociatedObject.SecurePassword, null);
            }
        }
    }
}
