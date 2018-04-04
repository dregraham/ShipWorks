using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
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
        private char fontAwesomeIcon;

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
            // Sets the message
            Message = message;

            // Positions the window to the center of the form
            popup.Value.LoadOwner(owner);

            // Trigger the show action to actually show the window
            popup.Value.ShowAction();
        }

        /// <summary>
        /// Shows the popup with the given message and image
        /// </summary>
        public void Show(string message, IWin32Window owner, char fontAwesomeIcon)
        {
            FontAwesomeIcon = fontAwesomeIcon;

            Show(message, owner);
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
        public char FontAwesomeIcon
        {
            get => fontAwesomeIcon;
            set => handler.Set(nameof(FontAwesomeIcon), ref fontAwesomeIcon, value);
        }
    }
}