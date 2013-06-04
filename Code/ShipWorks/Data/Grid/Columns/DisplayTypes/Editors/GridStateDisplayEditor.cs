using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Editors
{
    /// <summary>
    /// Editor for the display of columns that show State \ Province
    /// </summary>
    public partial class GridStateDisplayEditor : GridColumnDisplayEditor
    {
        GridStateDisplayType displayType;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridStateDisplayEditor(GridStateDisplayType displayType) :
            base(displayType)
        {
            InitializeComponent();

            if (displayType == null)
            {
                throw new ArgumentNullException("displayType");
            }

            this.displayType = displayType;

            formatList.DataSource = new DictionaryEntry[]
                {
                    new DictionaryEntry("Missouri", AbbreviationFormat.Full),
                    new DictionaryEntry("Missouri (MO)", AbbreviationFormat.FullAbbreviated),
                    new DictionaryEntry("MO", AbbreviationFormat.Abbreviated),
                    new DictionaryEntry("MO (Missouri)", AbbreviationFormat.AbbreviatedFull)
                };
            formatList.DisplayMember = "Key";
            formatList.ValueMember = "Value";

            formatList.SelectedValue = displayType.AbbreviationFormat;
            formatList.SelectedIndexChanged += new EventHandler(OnChangeFormat);
        }

        /// <summary>
        /// State format selection has changed
        /// </summary>
        void OnChangeFormat(object sender, EventArgs e)
        {
            displayType.AbbreviationFormat = (AbbreviationFormat) formatList.SelectedValue;

            OnValueChanged();
        }
    }
}
