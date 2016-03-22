using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Border used for notification that can display a character icon
    /// </summary>
    public class NotificationBorder : ContentControl
    {
        public static readonly DependencyProperty IconCharacterProperty =
            DependencyProperty.Register("IconCharacter",
                typeof(string),
                typeof(NotificationBorder));

        /// <summary>
        /// Constructor
        /// </summary>
        static NotificationBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationBorder), new FrameworkPropertyMetadata(typeof(NotificationBorder)));
        }

        /// <summary>
        /// Character to use for the icon
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        public string IconCharacter
        {
            get { return (string) GetValue(IconCharacterProperty); }
            set { SetValue(IconCharacterProperty, value); }
        }
    }
}
