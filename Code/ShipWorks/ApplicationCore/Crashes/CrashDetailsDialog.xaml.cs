using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace ShipWorks.ApplicationCore.Crashes
{
    /// <summary>
    /// Interaction logic for CrashDetailsDialog.xaml
    /// </summary>
    public partial class CrashDetailsDialog : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CrashDetailsDialog(string details)
        {
            InitializeComponent();

            if (details == null)
            {
                throw new ArgumentNullException("details");
            }

            reportDetails.Text = details;
        }

        /// <summary>
        /// Save the report to a file.
        /// </summary>
        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = "ShipWorksProblem.txt",
                DefaultExt = ".txt",
                Filter = "Text documents|*.txt"
            };

            if (saveFileDialog.ShowDialog(this) ?? false)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, reportDetails.Text);
                }
                // This is one of the rare cases we catch general Exception.  We
                // do this because we have already crashed.  No need to make it worse
                // by crashing and not being able to report the crash.
                catch (Exception ex)
                {
                    // Use the standard MessageBox for safety
                    MessageBox.Show(this,
                        "An error occurred while saving the file.\n\n" +
                        "Details: " + ex.Message,
                        "ShipWorks",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Handle close button
        /// </summary>
        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
