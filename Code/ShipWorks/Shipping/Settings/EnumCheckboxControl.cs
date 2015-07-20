using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// A generic control for picking the carrier services
    /// </summary>
    /// <typeparam name="T">Since an enumeration cannot be specified for T, struct and IConvertible is the closest we can get.</typeparam>
    [CLSCompliant(false)]
    public partial class EnumCheckBoxControl<T> : UserControl where T : struct, IConvertible
    {
        private readonly List<EnumCheckBoxDataSource<T>> allServiceTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumCheckBoxControl{T}"/> class.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">T must be an enumeration</exception>
        public EnumCheckBoxControl()
        {
            if (!typeof (T).IsEnum)
            {
                // This is to address the fact that a generic cannot be created specifically
                // for an enumeration
                throw new InvalidOperationException("T must be an enumeration");
            }

            InitializeComponent();

            allServiceTypes = new List<EnumCheckBoxDataSource<T>>();
        }

        /// <summary>
        /// Initializes the control with the available service types that and the service types that have been excluded.
        /// </summary>
        /// <param name="availableServiceTypes">All of the available service types.</param>
        /// <param name="excludedServiceTypes">The service types that have been excluded and will be unchecked.</param>
        public void Initialize(IEnumerable<T> availableServiceTypes, IEnumerable<T> excludedServiceTypes)
        {
            ClearSelections();

            List<T> excludedServiceTypeList = excludedServiceTypes.ToList();

            foreach (T serviceType in availableServiceTypes.Where(s => !EnumHelper.GetDeprecated(s as Enum)).OrderBy(s => EnumHelper.GetDescription(s as Enum)))
            {
                EnumCheckBoxDataSource<T> checkBoxItem = new EnumCheckBoxDataSource<T>(serviceType);

                // Mark the item as selected if it's not in the list of excluded service types
                selectedServices.Items.Add(checkBoxItem, !excludedServiceTypeList.Contains(serviceType));
                allServiceTypes.Add(checkBoxItem);
            }
        }

        /// <summary>
        /// Gets the service types that have been unchecked (i.e. excluded).
        /// </summary>
        public IEnumerable<T> ExcludedServiceTypes
        {
            get
            {                
                IEnumerable<EnumCheckBoxDataSource<T>> selectedItems = selectedServices.CheckedItems.Cast<EnumCheckBoxDataSource<T>>();
                return allServiceTypes.Except(selectedItems).Select(service => service.Value);
            }
        }

        /// <summary>
        /// Clears the selections and the in memory list of all service types
        /// </summary>
        private void ClearSelections()
        {
            selectedServices.Items.Clear();
            allServiceTypes.Clear();
        }
    }
}
