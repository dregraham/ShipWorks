using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace ShipWorks.Installer.Controls
{
    /// <summary>
    /// Interaction logic for MainTitleBar.xaml
    /// </summary>
    public partial class MainTitleBar : UserControl
    {
        Window mainWindow;
        public MainTitleBar()
        {
            InitializeComponent();
            mainWindow = Application.Current.MainWindow;
            RefreshMaximizeRestoreButton();
        }

        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            mainWindow.WindowState = WindowState.Minimized;
        }

        private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            if (mainWindow.WindowState == WindowState.Maximized)
            {
                mainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                var screen = GetScreenDims(mainWindow);
                mainWindow.MaxHeight = screen.WorkingArea.Height * .67;
                mainWindow.MaxWidth = screen.WorkingArea.Width;
                mainWindow.WindowState = WindowState.Maximized;
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
        }

        public void RefreshMaximizeRestoreButton()
        {
            if (mainWindow != null && mainWindow.WindowState == WindowState.Maximized)
            {
                this.maximizeButton.Visibility = Visibility.Collapsed;
                this.restoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.maximizeButton.Visibility = Visibility.Visible;
                this.restoreButton.Visibility = Visibility.Collapsed;
            }
        }

        private System.Windows.Forms.Screen GetScreenDims(Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            return System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
        }
    }
}
