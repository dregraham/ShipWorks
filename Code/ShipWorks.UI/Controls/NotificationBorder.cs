using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Border used for notification that can display a character icon
    /// </summary>
    public class NotificationBorder : ContentControl
    {
        public static readonly DependencyProperty IconFontFamilyProperty =
            DependencyProperty.Register("IconFontFamily",
                typeof(FontFamily),
                typeof(NotificationBorder));

        public static readonly DependencyProperty IconControlProperty =
            DependencyProperty.Register("IconControl",
                typeof(object),
                typeof(NotificationBorder));

        /// <summary>
        /// Constructor
        /// </summary>
        static NotificationBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationBorder), new FrameworkPropertyMetadata(typeof(NotificationBorder)));
        }

        /// <summary>
        /// FontFamily to use for the icon
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        [Localizability(LocalizationCategory.Font)]
        [TypeConverter(typeof(FontFamilyConverter))]
        public FontFamily IconFontFamily
        {
            get { return (FontFamily) GetValue(IconFontFamilyProperty); }
            set { SetValue(IconFontFamilyProperty, value); }
        }

        /// <summary>
        /// Custom content to use for the
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        public object IconControl
        {
            get { return GetValue(IconControlProperty); }
            set { SetValue(IconControlProperty, value); }
        }
    }
}
