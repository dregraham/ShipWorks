using System.ComponentModel;
using ShipWorks.Core.UI;

namespace ShipWorks.UI.Dialogs
{
    /// <summary>
    /// Viewmodel for the popup window
    /// </summary>
    public class PopupViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly PropertyChangedHandler handler;
        private string message;

        /// <summary>
        /// Constructor
        /// </summary>
        public PopupViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The actual message text we want to display
        /// </summary>
        public string Message
        {
            get { return message; }
            set { handler.Set(nameof(Message), ref message, value); }
        }
    }
}