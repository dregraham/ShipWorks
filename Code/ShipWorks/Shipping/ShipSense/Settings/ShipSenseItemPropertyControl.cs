using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Shipping.ShipSense.Settings
{
    /// <summary>
    /// A UserControl for selecting which properties of an OrderItemEntity should be used when
    /// determining unique ShipSense knowledge base entries.
    /// </summary>
    public partial class ShipSenseItemPropertyControl : UserControl
    {
        private Dictionary<string, CheckBox> itemPropertyCheckboxMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseItemPropertyControl"/> class.
        /// </summary>
        public ShipSenseItemPropertyControl()
        {
            InitializeComponent();

            InitializeDictionary();
        }

        /// <summary>
        /// Initializes the entries in the dictionary, so that each check box matches up 
        /// with the name of the corresponding property of the OrderItemEntity.
        /// </summary>
        private void InitializeDictionary()
        {
            itemPropertyCheckboxMap = new Dictionary<string, CheckBox>();

            // The dictionary key needs to match the name of the corresponding 
            // property of the OrderEntityItem
            itemPropertyCheckboxMap.Add("SKU", itemSku);
            itemPropertyCheckboxMap.Add("Code", itemCode);
            itemPropertyCheckboxMap.Add("Name", itemName);
            itemPropertyCheckboxMap.Add("Description", itemDescription);
            itemPropertyCheckboxMap.Add("UPC", itemUPC);
            itemPropertyCheckboxMap.Add("ISBN", itemISBN);
            itemPropertyCheckboxMap.Add("Location", itemLocation);
            itemPropertyCheckboxMap.Add("UnitPrice", itemUnitPrice);
            itemPropertyCheckboxMap.Add("UnitCost", itemUnitPrice);
            itemPropertyCheckboxMap.Add("LocalStatus", itemLocalStatus);
        }

        /// <summary>
        /// Uses the collection of property names to determine which check boxes are 
        /// selected/checked.
        /// </summary>
        /// <param name="propertyNames">The properties names that should be selected/checked.</param>
        public void LoadSelectedProperties(IEnumerable<string> propertyNames)
        {
            foreach (string property in propertyNames)
            {
                if (itemPropertyCheckboxMap.ContainsKey(property))
                {
                    itemPropertyCheckboxMap[property].Checked = true;
                }
            }
        }

        /// <summary>
        /// Gets the corresponding OrderItemEntity property names that have been selected.
        /// </summary>
        public IEnumerable<string> SelectedItemProperties
        {
            get
            {
                return itemPropertyCheckboxMap.Where(pair => pair.Value.Checked).Select(pair => pair.Key);
            }
        }
    }
}
