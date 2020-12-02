using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.Installer.Controls
{
    /// <summary>
    /// Interaction logic for BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
           "Password", typeof(string), typeof(BindablePasswordBox), new PropertyMetadata(default(string)));

        /// <summary>
        /// The password
        /// </summary>
        public string Password
        {
            get { return (string) GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BindablePasswordBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler to set the password
        /// </summary>
        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = ((PasswordBox) sender).Password;
        }

        /// <summary>
        /// Event handler to give focus to the password box
        /// </summary>
        private void BindablePasswordBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            passwordBox.Focus();
        }
    }
}
