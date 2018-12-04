using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace ShipWorks.UI.Controls
{
    [TemplatePart(Name = "PART_Entry", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_BrowseButton", Type = typeof(Button))]
    public class FilePicker : Control
    {
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(FilePicker),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnFilePathChanged)));

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(string), typeof(FilePicker));

        private TextBox entry;
        private Button browseButton;

        /// <summary>
        /// Static constructor
        /// </summary>
        static FilePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FilePicker), new FrameworkPropertyMetadata(typeof(FilePicker)));
        }

        /// <summary>
        /// File path
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        public string FilePath
        {
            get { return (string) GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        /// <summary>
        /// Filter for the dialog
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        public string Filter
        {
            get { return (string) GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        /// <summary>
        /// Apply the template
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            entry = GetTemplateChild("PART_Entry") as TextBox;
            browseButton = GetTemplateChild("PART_BrowseButton") as Button;

            if (entry == null)
            {
                throw new InvalidOperationException("PART_Entry is not available in the template");
            }

            if (browseButton == null)
            {
                throw new InvalidOperationException("PART_BrowseButton is not available in the template");
            }

            // Remove any existing handlers before adding another
            browseButton.Click -= OnBrowseButtonClicked;
            browseButton.Click += OnBrowseButtonClicked;
        }

        private void OnBrowseButtonClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = this.Filter
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SetCurrentValue(FilePathProperty, openFileDialog.FileName);
            }
        }

        private static void OnFilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
