using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Notify the user that something is happening
    /// </summary>
    public class WaitSpinner : ContentControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        static WaitSpinner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WaitSpinner), new FrameworkPropertyMetadata(typeof(WaitSpinner)));
        }
    }
}
