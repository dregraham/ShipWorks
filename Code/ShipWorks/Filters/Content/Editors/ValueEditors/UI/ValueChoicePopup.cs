using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.UI;
using ShipWorks.UI.Controls;

namespace ShipWorks.Filters.Content.Editors.ValueEditors.UI
{
    /// <summary>
    /// Popup providing a list of values to be selected
    /// </summary>
    public partial class ValueChoicePopup<T> : PopupComboBox where T : struct
    {
        private List<(CheckBox checkbox, T value)> valueList;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressValidationStatusPopup"/> class.
        /// </summary>
        public ValueChoicePopup()
        {
            InitializeComponent();

            PopupController = new PopupController(statusPanel);
        }

        /// <summary>
        /// List of values to exclude
        /// </summary>
        public IEnumerable<T> Exclusions { get; set; } = Enumerable.Empty<T>();

        /// <summary>
        /// Binds the statuses.
        /// </summary>
        private IEnumerable<(CheckBox checkbox, T value)> BuildValueList(IEnumerable<ValueChoice<T>> values)
        {
            return values
                .Select((x, i) =>
                {
                    // * 2 makes room for the label buttons.
                    int verticlePosition = 23 * (i + 2);

                    var checkbox = new CheckBox
                    {
                        Text = x.Name,
                        Location = new Point(3, verticlePosition),
                        AutoSize = true
                    };

                    checkbox.CheckedChanged += OnStatusCheckChanged;
                    return (checkbox, x.Value);


                    //// Build valueList with new checkboxes and related enums.
                    //var checkboxAndStatusType = (checkbox: new CheckBox
                    //{
                    //    Text = x.Key,
                    //    Location = new Point(3, verticlePosition),
                    //    AutoSize = true
                    //}, value: x.Value);

                    //checkboxAndStatusType.checkbox.CheckedChanged += OnStatusCheckChanged;

                    //return checkboxAndStatusType;
                    ////valueList.Add(checkboxAndStatusType);
                });

            //for (int statusIndex = 0; statusIndex < statuses.Length; statusIndex++)
            //{
            //    EnumEntry<T> status = statuses[statusIndex];

            //    // * 2 makes room for the label buttons.
            //    int verticlePosition = 23 * (statusIndex + 2);

            //    // Build valueList with new checkboxes and related enums.
            //    var checkboxAndStatusType = (checkbox: new CheckBox
            //    {
            //        Text = status.Key,
            //        Location = new Point(3, verticlePosition),
            //        AutoSize = true
            //    }, value: status.Value);

            //    checkboxAndStatusType.checkbox.CheckedChanged += OnStatusCheckChanged;

            //    valueList.Add(checkboxAndStatusType);

            //    // Add checkbox to panel
            //    statusPanel.Controls.Add(checkboxAndStatusType.checkbox);
            //    if (statusPanel.Width < checkboxAndStatusType.checkbox.Width)
            //    {
            //        statusPanel.Width = checkboxAndStatusType.checkbox.Width;
            //    }
            //}
        }

        /// <summary>
        /// Called when [status check changed].
        /// </summary>
        private void OnStatusCheckChanged(object sender, EventArgs e)
        {
            if (!ignoreChanged)
            {
                StatusChanged?.Invoke();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the status changed.
        /// </summary>
        public Action StatusChanged { private get; set; }

        /// <summary>
        /// Initialize the value list
        /// </summary>
        public void InitializeValuesList(IEnumerable<ValueChoice<T>> values)
        {
            valueList = BuildValueList(values).ToList();
            statusPanel.Controls.AddRange(valueList.Select(x => x.checkbox).ToArray());
            statusPanel.Width = valueList.Max(x => x.checkbox.Width);

            int panelHeight = valueList.Last().checkbox.Bottom + 10;
            statusPanel.Height = panelHeight;
            DropDownMinimumHeight = panelHeight - 12;
            DropDownHeight = panelHeight - 12;
            Height = panelHeight + 20;
        }

        /// <summary>
        /// Called when [select none clicked].
        /// </summary>
        private void OnNotValidatedClicked(object sender, EventArgs e) =>
            SelectStatuses(Enumerable.Empty<T>());

        /// <summary>
        /// Called when [select all clicked].
        /// </summary>
        private void OnReadyToGoLabelClicked(object sender, EventArgs e) =>
            SelectStatuses(valueList.Select(x => x.value));

        /// <summary>
        /// Selects the statuses specified and deselects the other statuses.
        /// </summary>
        /// <param name="addressValidationStatusTypes">The address validation status types to select.</param>
        public void SelectStatuses(IEnumerable<T> addressValidationStatusTypes)
        {
            ignoreChanged = true;

            foreach (var (checkbox, value) in valueList)
            {
                checkbox.Checked = addressValidationStatusTypes.Contains(value);
            }

            StatusChanged?.Invoke();

            ignoreChanged = false;
        }

        /// <summary>
        /// Gets the selected statuses.
        /// </summary>
        public List<T> GetSelectedStatuses()
        {
            return valueList
                .Where(s => s.checkbox.Checked)
                .Select(s => s.value)
                .ToList();
        }

        /// <summary>
        /// Draw the item that the user has currently selected
        /// </summary>
        protected override void OnDrawSelectedItem(Graphics graphics, Color foreColor, Rectangle bounds)
        {
            string text = valueList.All(s => s.checkbox.Checked) ?
                "All Values" :
                string.Join(", ", GetSelectedStatuses().OfType<Enum>().Select(EnumHelper.GetDescription));

            using (StringFormat stringFormat = new StringFormat())
            {
                stringFormat.Trimming = StringTrimming.EllipsisCharacter;

                using (var brush = new SolidBrush(ForeColor))
                {
                    graphics.DrawString(text, Font, brush, bounds, stringFormat);
                }
            }
        }
    }
}