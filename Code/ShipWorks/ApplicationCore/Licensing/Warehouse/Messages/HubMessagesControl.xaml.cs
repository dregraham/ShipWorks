using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.Messages
{
    /// <summary>
    /// Interaction logic for HubMessagesControl.xaml
    /// </summary>
    public partial class HubMessagesControl : UserControl
    {
        public System.Windows.Forms.Form ParentDialog { get; set; }

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
            }

            var paragraph = new Paragraph();

            var messagesArray = messages.ToArray();

            for (int i = 0; i < messagesArray.Length; i++)
            {
                if (messagesArray.Length > 1)
                {
                    paragraph.Inlines.Add(new Bold(new Run($"Message {i + 1}:")));
                    paragraph.Inlines.Add(new LineBreak());
                }

                paragraph.Inlines.Add(new Run(messagesArray[i].Message));
                paragraph.Inlines.Add(new LineBreak());
                paragraph.Inlines.Add(new LineBreak());
            }

            Messages.Document = new FlowDocument(paragraph);
        }

        /// <summary>
        /// The acknowledgment checkbox changed
        /// </summary>
        private void AcknowledgmentChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            OkButton.IsEnabled = Acknowledgement.IsChecked ?? true;
        }

        /// <summary>
        /// The OK button is clicked
        /// </summary>
        private void OkButtonClicked(object sender, System.Windows.RoutedEventArgs e) => ParentDialog?.Close();
    }
}
