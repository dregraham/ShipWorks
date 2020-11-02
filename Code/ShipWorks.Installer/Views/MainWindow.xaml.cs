using System.Windows;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            INavigationService<NavigationPageType> navService =
                App.ServiceProvider.GetService(typeof(INavigationService<NavigationPageType>)) as
                    INavigationService<NavigationPageType>;

            navService.NavigateTo("SystemCheck");
        }

        /// <summary>
        /// Event handler to focus on the main grid when the window is clicked
        /// </summary>
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mainGrid.Focus();
        }
    }
}
