using ShipWorks.UI.Controls;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// Expose the state of service control expanders
    /// </summary>
    public class ExpanderState : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private const string OriginKey = "{1EEA4267-BE5E-4550-A9A2-4CE855FF77E6}";
        private const string DestinationKey = "{38D40A3F-7886-4295-9804-8BF69E45F702}";

        static readonly ExpanderState current = new ExpanderState();

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Get the current instance of the expander state
        /// </summary>
        public static ExpanderState Current => current;

        /// <summary>
        /// Ensure the class is a singleton
        /// </summary>
        private ExpanderState()
        {

        }

        /// <summary>
        /// IsOriginExpanded
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsOriginExpanded
        {
            get
            {
                return !CollapsibleGroupControl.GetCollapsedState(OriginKey, false);
            }
            set
            {
                CollapsibleGroupControl.SaveCollapsedState(OriginKey, !value);
                RaisePropertyChanged(nameof(IsOriginExpanded));
            }
        }

        /// <summary>
        /// IsDestinationExpanded
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsDestinationExpanded
        {
            get
            {
                return !CollapsibleGroupControl.GetCollapsedState(DestinationKey, false);
            }
            set
            {
                CollapsibleGroupControl.SaveCollapsedState(DestinationKey, !value);
                RaisePropertyChanged(nameof(IsDestinationExpanded));
            }
        }

        /// <summary>
        /// Notify consumers that the property has changed
        /// </summary>
        /// <param name="property"></param>
        private void RaisePropertyChanged(string property) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}
