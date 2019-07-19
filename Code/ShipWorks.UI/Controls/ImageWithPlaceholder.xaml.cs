using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Interaction logic for ImageWithPlaceholder.xaml
    /// </summary>
    public partial class ImageWithPlaceholder : UserControl
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageWithPlaceholder), new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty ShowPlaceholderProperty =
            DependencyProperty.Register("ShowPlaceholder", typeof(bool), typeof(ImageWithPlaceholder),
                                        new PropertyMetadata(default(bool)));

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageWithPlaceholder()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Source for the image to display
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        public ImageSource Source
        {
            get
            {
                ImageSource imageSource = (ImageSource) GetValue(SourceProperty);

                ShowPlaceholder = imageSource == null;

                return imageSource;
            }
            set => SetValue(SourceProperty, value);
        }

        /// <summary>
        /// Whether or not to show the placeholder image
        /// </summary>
        public bool ShowPlaceholder
        {
            get => (bool) GetValue(ShowPlaceholderProperty);
            set => SetValue(ShowPlaceholderProperty, value);
        }
    }
}
