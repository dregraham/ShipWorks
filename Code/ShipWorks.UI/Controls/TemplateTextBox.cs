using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Text box that allows template editing
    /// </summary>
    public class TemplateTextBox : TextBox
    {
        /// <summary>
        /// Constructor
        /// </summary>
        static TemplateTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TemplateTextBox), new FrameworkPropertyMetadata(typeof(TemplateTextBox)));
        }
    }
}
