using System.Windows;
using ShipWorks.Installer.Services;
using ShipWorks.Installer.ViewModels;

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
    }
}
