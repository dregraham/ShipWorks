using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;

namespace ShipWorks.UI.Dialogs.Popup
{
    /// <summary>
    /// ViewModel for the popup window
    /// </summary>
    [Component(SingleInstance = true)]
    public class PopupViewModel : INotifyPropertyChanged, IPopupViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly PropertyChangedHandler handler;
        private readonly Lazy<IPopup> popup;

        private string message;
        private char icon;
        private readonly TimeSpan defaultFadeStartTimeSpan = TimeSpan.FromSeconds(4);
        private readonly TimeSpan iconFadeStartTimeSpan = TimeSpan.FromSeconds(2);
        private Duration duration;

        private const char NoIcon = '\0';
        private const char KeyboardIcon = (char) 0xf11c;
        private const char BarcodeIcon = (char) 0xf02a;

        /// <summary>
        /// Constructor
        /// </summary>
        public PopupViewModel(Func<IPopup> popupFactory)
        {
            popup = new Lazy<IPopup>(() =>
            {
                IPopup newPopup = popupFactory();
                newPopup.SetViewModel(this);
                return newPopup;
            });

            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Shows the popup
        /// </summary>
        public void Show(string message, IWin32Window owner) =>
            Show(message, owner, NoIcon, defaultFadeStartTimeSpan);

        /// <summary>
        /// Shows the popup
        /// </summary>
        public void Show(string message, IWin32Window owner, TimeSpan fadeStartTimeSpan) =>
            Show(message, owner, NoIcon, fadeStartTimeSpan);

        /// <summary>
        /// Shows popup with keyboard icon
        /// </summary>
        public void ShowWithKeyboard(string message, Control owner) =>
            Show(message, owner, KeyboardIcon, iconFadeStartTimeSpan);
            

        /// <summary>
        /// Shows the popup with barcode icon
        /// </summary>
        public void ShowWithBarcode(string message, Control owner) =>
            Show(message, owner, BarcodeIcon, iconFadeStartTimeSpan);

        /// <summary>
        /// Shows the popup with the given message and image
        /// </summary>
        private void Show(string message, IWin32Window owner, char icon, TimeSpan fadeTime)
        {
            Icon = icon;
            Duration = new Duration(fadeTime);
            
            // Sets the message
            Message = message;

            // Positions the window to the center of the form
            popup.Value.LoadOwner(owner);

            // Trigger the show action to actually show the window
            popup.Value.ShowAction();
        }

        /// <summary>
        /// The actual message text we want to display
        /// </summary>
        [Obfuscation(Exclude=true)]
        public string Message
        {
            get => message;
            set => handler.Set(nameof(Message), ref message, value);
        }

        /// <summary>
        /// The actual message text we want to display
        /// </summary>
        [Obfuscation(Exclude = true)]
        public char Icon
        {
            get => icon;
            private set => handler.Set(nameof(Icon), ref icon, value);
        }

        /// <summary>
        /// How long the popup will be displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Duration Duration
        {
            get => duration;
            set => handler.Set(nameof(Duration), ref duration, value);
        }
    }
}