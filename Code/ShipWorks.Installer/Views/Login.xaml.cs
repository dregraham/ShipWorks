using System.Windows;
using System.Windows.Controls;
using ShipWorks.Installer.ViewModels;

namespace ShipWorks.Installer.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for password changing
        /// </summary>
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            { ((LoginViewModel) DataContext).Password = ((PasswordBox) sender).Password; }
        }
    }
}
