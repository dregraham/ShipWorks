using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Grid
{
    public partial class GridChannelAdvisorFlagDisplayEditor : GridColumnDisplayEditor
    {
        GridChannelAdvisorFlagDisplayType displayType; 

        /// <summary>
        /// Initializes a new instance of the <see cref="GridChannelAdvisorFlagDisplayEditor"/> class.
        /// </summary>
        /// <param name="displayType">The display type.</param>
        public GridChannelAdvisorFlagDisplayEditor(GridChannelAdvisorFlagDisplayType displayType)
            : base(displayType)
        {
            InitializeComponent();

            if (displayType == null)
            {
                throw new ArgumentNullException("displayType");
            }

            this.displayType = displayType;

            showFlag.Checked = displayType.ShowFlag;
            showFlag.CheckedChanged += new EventHandler(OnChangeFormat);
        }

        /// <summary>
        /// Called when [change format].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void OnChangeFormat(object sender, EventArgs e)
        {
            displayType.ShowFlag = showFlag.Checked;
            OnValueChanged();
        }
    }
}
