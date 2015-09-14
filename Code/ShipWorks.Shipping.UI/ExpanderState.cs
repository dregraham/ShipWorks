using ShipWorks.UI.Controls;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// Expose the state of service control expanders
    /// </summary>
    public class ExpanderState : INotifyPropertyChanged
    {
        private const string OriginKey = "{1EEA4267-BE5E-4550-A9A2-4CE855FF77E6}";
        private const string DestinationKey = "{38D40A3F-7886-4295-9804-8BF69E45F702}";
        private const string DetailsKey = "{A02BA03E-D768-4152-94F8-65BD8951C750}";
        private const string CustomsKey = "{E254D445-5F07-455C-8E34-DCA0214ACAD3}";

        static readonly ExpanderState current = new ExpanderState();

        public event PropertyChangedEventHandler PropertyChanged;

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
        /// IsDetailsExpanded
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsDetailsExpanded
        {
            get
            {
                return !CollapsibleGroupControl.GetCollapsedState(DetailsKey, false);
            }
            set
            {
                CollapsibleGroupControl.SaveCollapsedState(DetailsKey, !value);
                RaisePropertyChanged(nameof(IsDetailsExpanded));
            }
        }

        /// <summary>
        /// IsCustomsExpanded
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsCustomsExpanded
        {
            get
            {
                return !CollapsibleGroupControl.GetCollapsedState(CustomsKey, false);
            }
            set
            {
                CollapsibleGroupControl.SaveCollapsedState(CustomsKey, !value);
                RaisePropertyChanged(nameof(IsCustomsExpanded));
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
