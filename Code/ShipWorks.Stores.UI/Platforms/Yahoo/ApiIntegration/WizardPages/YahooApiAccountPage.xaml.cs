﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration.WizardPages
{
    /// <summary>
    /// Interaction logic for YahooApiAccountSettings.xaml
    /// </summary>
    public partial class YahooApiAccountPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YahooApiAccountPage"/> class.
        /// </summary>
        public YahooApiAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens the Help Link
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RequestNavigateEventArgs"/> instance containing the event data.</param>
        private void HyperlinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        /// <summary>
        /// Copies the Yahoo partner ID to the clipboard.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ButtonBaseOnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText("ypa-005343935941");
        }

    }
}
