using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Interapptive.Shared.Collections;

namespace ShipWorks.UI.AttachedProperties
{
    /// <summary>
    /// Restrictions on text box
    /// </summary>
    public static class TextRestriction
    {
        public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.RegisterAttached("MaxLines", typeof(int),
                typeof(TextRestriction), new PropertyMetadata(0, OnMaxLinesChanged));

        /// <summary>
        /// Set the max lines allowed in the text box
        /// </summary>
        private static void OnMaxLinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            int length = (int) e.NewValue;
            TextBox textBox = d as TextBox;

            if (textBox == null)
            {
                return;
            }

            DataObject.RemovePastingHandler(textBox, OnTextBoxPaste);
            textBox.PreviewKeyDown -= OnTextBoxPreviewKeyDown;
            textBox.PreviewDrop -= OnTextBoxPreviewDrop;

            if (length > 0)
            {
                DataObject.AddPastingHandler(textBox, OnTextBoxPaste);
                textBox.PreviewKeyDown += OnTextBoxPreviewKeyDown;
                textBox.PreviewDrop += OnTextBoxPreviewDrop;
            }
        }

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        public static int GetMaxLines(DependencyObject d) => (int) d.GetValue(MaxLinesProperty);

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        public static void SetMaxLines(DependencyObject d, int value) => d.SetValue(MaxLinesProperty, value);

        /// <summary>
        /// Handle previewing key down
        /// </summary>
        private static void OnTextBoxPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (e.Key == System.Windows.Input.Key.Enter && textBox.LineCount >= GetMaxLines(textBox))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handle a paste on the text box
        /// </summary>
        private static void OnTextBoxPaste(object sender, DataObjectPastingEventArgs e)
        {
            HandleDataChange(sender as TextBox, e.SourceDataObject, e.CancelCommand);
        }

        /// <summary>
        /// Handle previewing drop
        /// </summary>
        private static void OnTextBoxPreviewDrop(object sender, DragEventArgs e)
        {
            HandleDataChange(sender as TextBox, e.Data, () => e.Handled = true);

            e.Effects = DragDropEffects.Copy;
        }

        /// <summary>
        /// Handle a data change from a data object
        /// </summary>
        private static void HandleDataChange(TextBox textBox, IDataObject dataObject, Action cancelAction)
        {
            bool isText = dataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!isText)
            {
                return;
            }

            int maxLines = GetMaxLines(textBox);
            string text = dataObject.GetData(DataFormats.UnicodeText) as string;
            string[] lines = (text ?? string.Empty).Replace("\r", "").Split('\n');

            if (lines.Length > maxLines)
            {
                cancelAction();

                textBox.Text = lines.Take(maxLines).Combine(Environment.NewLine);
                textBox.SelectionStart = textBox.Text.Length;
            }
        }
    }
}
