using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;

namespace ShipWorks.Stores.Platforms.ProStores.CoreExtensions.Grid
{
    /// <summary>
    /// Display editor for ProStoresAuthorization column
    /// </summary>
    public partial class ProStoresAuthorizationDisplayEditor : GridDateDisplayEditor
    {
        ProStoresAuthorizationDisplayType displayType;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresAuthorizationDisplayEditor(ProStoresAuthorizationDisplayType displayType)
            : base(displayType)
        {
            InitializeComponent();

            this.displayType = displayType;

            showIcon.Checked = displayType.ShowAuthorizedIcon;
            showIcon.CheckedChanged += new EventHandler(OnShowIconChanged);
        }

        /// <summary>
        /// Controsl if showing the icon is turned on
        /// </summary>
        void OnShowIconChanged(object sender, EventArgs e)
        {
            displayType.ShowAuthorizedIcon = showIcon.Checked;

            OnValueChanged();
        }
    }
}
