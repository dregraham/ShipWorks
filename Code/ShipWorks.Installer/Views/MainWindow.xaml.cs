using System.Windows;
using ShipWorks.Installer.Behaviors;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Services;
using ShipWorks.Installer.ViewModels;

namespace ShipWorks.Installer.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            new WindowChromeLoadedBehavior().Attach(this);
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var viewModel = this.DataContext as MainViewModel;
            if (!viewModel.IsClosing)
            {
                e.Cancel = !viewModel.Close(true);
            }
        }

        private void Window_StateChanged(object sender, System.EventArgs e)
        {
            TitleBar.RefreshMaximizeRestoreButton();
        }
    }
}
