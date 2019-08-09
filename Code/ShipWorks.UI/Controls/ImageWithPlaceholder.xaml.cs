using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Interaction logic for ImageWithPlaceholder.xaml
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
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
        public ImageSource Source
        {
            get => (ImageSource) GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
    }
}
