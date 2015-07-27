using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// A generic control for picking the enums from a checkbox list
    /// </summary>
    /// <typeparam name="T">Since an enumeration cannot be specified for T, struct and IConvertible is the closest we can get.</typeparam>
    [CLSCompliant(false)]
    public partial class EnumCheckBoxControl<T> : UserControl where T : struct, IConvertible
    {
        private readonly List<EnumCheckBoxDataSource<T>> allEnums;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumCheckBoxControl{T}"/> class.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">T must be an enumeration</exception>
        public EnumCheckBoxControl()
        {
            if (!typeof(T).IsEnum)
            {
                // This is to address the fact that a generic cannot be created specifically
                // for an enumeration
                throw new InvalidOperationException("T must be an enumeration");
            }

            InitializeComponent();

            allEnums = new List<EnumCheckBoxDataSource<T>>();
        }

        /// <summary>
        /// Initializes the control with the available enums and the enums that have been excluded.
        /// </summary>
        /// <param name="availableEnums">All of the available enum values.</param>
        /// <param name="excludedEnums">The enum values that have been excluded and will be unchecked.</param>
        public void Initialize(IEnumerable<T> availableEnums, IEnumerable<T> excludedEnums)
        {
            ClearSelections();

            List<T> excludedEnumList = excludedEnums.ToList();

            foreach (T enumValue in availableEnums.Where(s => !EnumHelper.GetDeprecated(s as Enum)).OrderBy(s => EnumHelper.GetDescription(s as Enum)))
            {
                EnumCheckBoxDataSource<T> checkBoxItem = new EnumCheckBoxDataSource<T>(enumValue);

                // Mark the item as selected if it's not in the list of excluded enum values
                selectedEnums.Items.Add(checkBoxItem, !excludedEnumList.Contains(enumValue));
                allEnums.Add(checkBoxItem);
            }
        }

        /// <summary>
        /// Gets the enum values that have been unchecked (i.e. excluded).
        /// </summary>
        public IEnumerable<T> ExcludedEnumValues
        {
            get
            {
                IEnumerable<EnumCheckBoxDataSource<T>> selectedItems = selectedEnums.CheckedItems.Cast<EnumCheckBoxDataSource<T>>();
                return allEnums.Except(selectedItems).Select(service => service.Value);
            }
        }

        /// <summary>
        /// Gets and sets the title of the control
        /// </summary>
        protected string Title
        {
            get
            {
                return title.Text;
            }
            set
            {
                title.Text = value;
            }
        }

        /// <summary>
        /// Gets and sets the description of the control
        /// </summary>
        protected string Description
        {
            get
            {
                return description.Text;
            }
            set
            {
                description.Text = value;
            }
        }

        /// <summary>
        /// Clears the selections and the in memory list of all enum values
        /// </summary>
        private void ClearSelections()
        {
            selectedEnums.Items.Clear();
            allEnums.Clear();
        }
    }
}
