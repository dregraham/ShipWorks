using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.Messages
{
    /// <summary>
    /// Interaction logic for HubMessagesControl.xaml
    /// </summary>
    public partial class HubMessagesControl : UserControl
    {
        private bool hasScrolledToBottom = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubMessagesControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set the messages to display
        /// </summary>
        public void SetMessages(IEnumerable<MessageDTO> messages)
        {
            if (messages.Any(x => x.RequiresConfirmation))
            {
                Acknowledgement.Visibility = System.Windows.Visibility.Visible;
                OkButton.IsEnabled = false;
                RequiresAcknowledgement = true;
                IsAcknowledged = false;
            }

            var messagesArray = messages.ToArray();

            var markdown = string.Empty;

            for (int i = 0; i < messagesArray.Length; i++)
            {
                if (messagesArray.Length > 1)
                {
                    markdown += $"**Message {i + 1}:**\n\n";
                }

                markdown += $"{messagesArray[i].Message}\n\n\n";
            }

            Messages.Markdown = markdown;
        }

        /// <summary>
        /// The control has finished loading
        /// </summary>
        private void MessagesControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var scrollViewer = Messages.Template.FindName("PART_ContentHost", Messages) as ScrollViewer;

            if (RequiresAcknowledgement && scrollViewer.VerticalOffset != scrollViewer.ScrollableHeight)
            {
                Acknowledgement.IsEnabled = false;
                scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
                hasScrolledToBottom = false;
            }
        }

        /// <summary>
        /// The scroll bar has moved
        /// </summary>
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer) sender;

            if (!hasScrolledToBottom && scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
            {
                hasScrolledToBottom = true;
                Acknowledgement.IsEnabled = true;
            }
        }

        /// <summary>
        /// The acknowledgment checkbox changed
        /// </summary>
        private void AcknowledgmentChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            IsAcknowledged = Acknowledgement.IsChecked ?? true;
            OkButton.IsEnabled = IsAcknowledged;
        }

        /// <summary>
        /// The OK button is clicked
        /// </summary>
        private void OkButtonClicked(object sender, System.Windows.RoutedEventArgs e) => ParentDialog?.Close();

        /// <summary>
        /// Whether or not confirmation is required
        /// </summary>
        public bool RequiresAcknowledgement { get; set; } = false;

        /// <summary>
        /// Whether or not the user has confirmed they've read the messages
        /// </summary>
        public bool IsAcknowledged { get; set; } = true;

        /// <summary>
        /// The parent dialog for this control
        /// </summary>
        public System.Windows.Forms.Form ParentDialog { get; set; }
    }
}
