using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;
using ShipWorks.Users;

namespace ShipWorks.UI.Dialogs.UserConditionalNotification
{
    /// <summary>
    /// View model for the user conditional notification dialog
    /// </summary>
    [Component(Service = typeof(IUserConditionalNotification))]
    public class UserConditionalNotificationViewModel : IUserConditionalNotification, INotifyPropertyChanged
    {
        private readonly IUserConditionalNotificationDialog dialog;
        readonly ICurrentUserSettings currentUserSettings;
        private readonly PropertyChangedHandler handler;

        private string title;
        private string message;
        private bool doNotShowAgain;
        private UserConditionalNotificationType notificationType;

        /// <summary>
        /// Notify of changing properties
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserConditionalNotificationViewModel(IUserConditionalNotificationDialog dialog, ICurrentUserSettings currentUserSettings)
        {
            this.dialog = dialog;
            this.currentUserSettings = currentUserSettings;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            Dismiss = new RelayCommand(() => DismissAction());
        }

        /// <summary>
        /// Dismiss the dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Dismiss { get; }

        /// <summary>
        /// Title of the dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Title
        {
            get { return title; }
            set { handler.Set(nameof(Title), ref title, value); }
        }

        /// <summary>
        /// Message to display
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Message
        {
            get { return message; }
            set { handler.Set(nameof(Message), ref message, value); }
        }

        /// <summary>
        /// Should the dialog be shown again
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool DoNotShowAgain
        {
            get { return doNotShowAgain; }
            set { handler.Set(nameof(DoNotShowAgain), ref doNotShowAgain, value); }
        }

        /// <summary>
        /// Show the dialog, if necessary
        /// </summary>
        public void Show(IMessageHelper messageHelper, string title, string message, UserConditionalNotificationType notificationType)
        {
            this.notificationType = notificationType;

            if (!currentUserSettings.ShouldShowNotification(notificationType))
            {
                return;
            }

            Title = title;
            Message = message;

            dialog.DataContext = this;
            messageHelper.ShowDialog(dialog);
        }

        /// <summary>
        /// Dismiss the dialog
        /// </summary>
        private void DismissAction()
        {
            if (DoNotShowAgain)
            {
                currentUserSettings.StopShowingNotification(notificationType);
            }

            dialog.Close();
        }
    }
}
