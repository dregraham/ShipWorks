using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ShipWorks.UI.Controls.BindableRichTextBox
{
    /// <summary>
    /// Bindable rich text box
    /// </summary>
    public class BindableRichTextBox : RichTextBox
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source",
                typeof(Uri), typeof(BindableRichTextBox),
                new PropertyMetadata(OnSourceChanged));

        /// <summary>
        /// Source
        /// </summary>
        public Uri Source
        {
            get => GetValue(SourceProperty) as Uri;
            set => SetValue(SourceProperty, value);
        }

        /// <summary>
        /// Handle on source changed
        /// </summary>
        private static void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is BindableRichTextBox rtf && 
                rtf.Source != null)
            {
                var uri = rtf.Source as Uri;
                if (uri == null || string.IsNullOrWhiteSpace(uri.OriginalString))
                {
                    return;
                }

                MemoryStream stream;
                using (WebClient webClient = new WebClient())
                {
                    var dataBytes = webClient.DownloadData(uri);
                    stream = new MemoryStream(dataBytes);
                }

                if (stream != null)
                {
                    var range = new TextRange(rtf.Document.ContentStart, rtf.Document.ContentEnd);
                    range.Load(stream, DataFormats.Rtf);
                }
            }
        }
    }
}