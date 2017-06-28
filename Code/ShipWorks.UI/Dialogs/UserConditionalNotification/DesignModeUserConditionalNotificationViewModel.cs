namespace ShipWorks.UI.Dialogs.UserConditionalNotification
{
    /// <summary>
    /// Design time version of the UserConditionalNotification view model
    /// </summary>
    public class DesignModeUserConditionalNotificationViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DesignModeUserConditionalNotificationViewModel()
        {
            Title = "Combine Order";
            Message = "Orders #72278 and #72279 were combined into Order #72280.";
        }

        /// <summary>
        /// Title of the dialog
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Message to display in the dialog
        /// </summary>
        public string Message { get; set; }
    }
}
