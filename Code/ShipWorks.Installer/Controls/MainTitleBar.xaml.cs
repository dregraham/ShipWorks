using System;
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
        private readonly Window mainWindow;
        public MainTitleBar()
        {
            InitializeComponent();
            mainWindow = Application.Current.MainWindow;
            SetWindowMaxSize();
            RefreshMaximizeRestoreButton();
        }

        /// <summary>
        /// Event handler for the minimize button click
        /// </summary>
        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            mainWindow.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Event handler for the maximize button click
        /// </summary>
        private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            if (mainWindow.WindowState == WindowState.Maximized)
            {
                mainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                SetWindowMaxSize();
                mainWindow.WindowState = WindowState.Maximized;
            }
        }

        /// <summary>
        /// Event handler for the close button click
        /// </summary>
        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
        }

        /// <summary>
        /// Refreshes the state of the min max button
        /// </summary>
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

        /// <summary>
        /// Sets the MaxSize of the window to the working area of the current screen
        /// </summary>
        private void SetWindowMaxSize()
        {
            var screen = GetScreenDims(mainWindow);
            mainWindow.MaxHeight = TransformPixelsToDeviceIndependant(screen.WorkingArea.Height);
        }

        /// <summary>
        /// Gets the dimension of the screen the attached window is 
        /// currently displayed on
        /// </summary>
        private System.Windows.Forms.Screen GetScreenDims(Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            return System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
        }

        /// <summary>
        /// Transforms pixels into device independant units
        /// </summary>
        private double TransformPixelsToDeviceIndependant(double pixels)
        {
            double deviceIndependant = 0;
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
            {
                deviceIndependant = (pixels / (g.DpiX / 96.75));
            }

            return deviceIndependant;
        }
    }
}
