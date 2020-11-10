using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FontAwesome5;

namespace ShipWorks.Installer.Controls
{
    /// <summary>
    /// Interaction logic for NavigationOption.xaml
    /// </summary>
    public partial class NavigationOption : UserControl
    {
        public static readonly DependencyProperty IconProperty =
             DependencyProperty.Register(nameof(Icon), typeof(EFontAwesomeIcon),
             typeof(NavigationOption), new FrameworkPropertyMetadata(OnIconChanged));

        private string text;

        /// <summary>
        /// Constructor
        /// </summary>
        public NavigationOption()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Text to display
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                Label.Text = value;
            }
        }

        /// <summary>
        /// Icon to use
        /// </summary>
        public EFontAwesomeIcon Icon
        {
            get => (EFontAwesomeIcon) GetValue(IconProperty);
            set => SetValue(IconProperty, value);

        }

        /// <summary>
        /// Handle icon changed
        /// </summary>
        private static void OnIconChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            NavigationOption control = source as NavigationOption;
            EFontAwesomeIcon newIcon = (EFontAwesomeIcon) e.NewValue;

            control.CurrentIcon.Icon = newIcon;
            if (newIcon == EFontAwesomeIcon.Solid_ExclamationCircle)
            {
                control.CurrentIcon.Foreground = Brushes.Red;
            }
            else if (newIcon == EFontAwesomeIcon.None)
            {
                control.CurrentIcon.Foreground = null;
            }
            else
            {
                control.CurrentIcon.Foreground = Brushes.Green;
            }
        }
    }
}
