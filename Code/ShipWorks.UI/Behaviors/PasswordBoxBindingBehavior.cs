using System.Reflection;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace ShipWorks.UI.Behaviors
{
    public class PasswordBoxBindingBehavior : Behavior<PasswordBox>
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password",
            typeof(SecureString), typeof(PasswordBoxBindingBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            AssociatedObject.PasswordChanged += OnPasswordBoxValueChanged;
        }

        public SecureString Password
        {
            get { return (SecureString) GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value);}
        }

        private void OnPasswordBoxValueChanged(object sender, RoutedEventArgs e)
        {
            BindingExpression binding = BindingOperations.GetBindingExpression(this, PasswordProperty);

            PropertyInfo property = binding?.DataItem.GetType().GetProperty(binding.ParentBinding.Path.Path);

            property?.SetValue(binding.DataItem, AssociatedObject.SecurePassword, null);
        }
    }
}
