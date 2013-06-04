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
    /// Editor for the formatting of countries
    /// </summary>
    public partial class GridCountryDisplayEditor : GridColumnDisplayEditor
    {
        GridCountryDisplayType displayType;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridCountryDisplayEditor(GridCountryDisplayType displayType) : base(displayType)
        {
            InitializeComponent();

            if (displayType == null)
            {
                throw new ArgumentNullException("displayType");
            }

            this.displayType = displayType;

            formatList.DataSource = new DictionaryEntry[]
                {
                    new DictionaryEntry("United States", AbbreviationFormat.Full),
                    new DictionaryEntry("United States (US)", AbbreviationFormat.FullAbbreviated),
                    new DictionaryEntry("US", AbbreviationFormat.Abbreviated),
                    new DictionaryEntry("US (United States)", AbbreviationFormat.AbbreviatedFull)
                };
            formatList.DisplayMember = "Key";
            formatList.ValueMember = "Value";

            formatList.SelectedValue = displayType.AbbreviationFormat;
            formatList.SelectedIndexChanged += new EventHandler(OnChangeFormat);

            showFlag.Checked = displayType.ShowFlag;
            showFlag.CheckedChanged += new EventHandler(OnChangeFormat);
        }

        /// <summary>
        /// Country format selection has changed
        /// </summary>
        void OnChangeFormat(object sender, EventArgs e)
        {
            displayType.AbbreviationFormat = (AbbreviationFormat) formatList.SelectedValue;
            displayType.ShowFlag = showFlag.Checked;

            OnValueChanged();
        }
    }
}
