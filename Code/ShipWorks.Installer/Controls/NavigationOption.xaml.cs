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

        public NavigationOption()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                Label.Text = value;
            }
        }

        public EFontAwesomeIcon Icon
        {
            get => (EFontAwesomeIcon) GetValue(IconProperty);
            set => SetValue(IconProperty, value);

        }

        private static void OnIconChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            NavigationOption control = source as NavigationOption;
            EFontAwesomeIcon newIcon = (EFontAwesomeIcon) e.NewValue;

            control.CurrentIcon.Icon = newIcon;
            if (newIcon == EFontAwesomeIcon.Solid_ExclamationCircle)
            {
                control.CurrentIcon.Foreground = Brushes.Red;
            }
            else
            {
                control.CurrentIcon.Foreground = Brushes.Green;
            }
        }

    }
}
