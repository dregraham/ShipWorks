using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;

namespace ShipWorks.UI.Dialogs
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
        private readonly TimeSpan defaultFadeStartTimeSpan = new TimeSpan(0, 0, 0, 4, 0);
        private Duration duration;

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
        public void Show(string message, IWin32Window owner)
        {
            Show(message, owner, IconType.None, defaultFadeStartTimeSpan);
        }

        /// <summary>
        /// Shows the popup with the given message and image
        /// </summary>
        public void Show(string message, IWin32Window owner, IconType icon, TimeSpan fadeTime)
        {
            Icon = GetIconChar(icon);
            Duration = new Duration(fadeTime);
            
            // Sets the message
            Message = message;

            // Positions the window to the center of the form
            popup.Value.LoadOwner(owner);

            // Trigger the show action to actually show the window
            popup.Value.ShowAction();
        }

        /// <summary>
        /// Given an IconType, return the FontAwesome character representing the icon
        /// </summary>
        private char GetIconChar(IconType iconType)
        {
            switch (iconType)
            {
                case IconType.None: return (char) 0;
                case IconType.Barcode: return (char) 0xf02a;
                case IconType.Keyboard: return (char) 0xf11c;
            }
            
            throw new ArgumentOutOfRangeException(nameof(iconType), iconType, null);
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