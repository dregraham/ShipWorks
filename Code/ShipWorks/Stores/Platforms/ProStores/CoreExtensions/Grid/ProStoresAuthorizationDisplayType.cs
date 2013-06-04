using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Properties;
using System.Drawing;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;

namespace ShipWorks.Stores.Platforms.ProStores.CoreExtensions.Grid
{
    /// <summary>
    /// Custom display type for ProStores Authorization column
    /// </summary>
    public class ProStoresAuthorizationDisplayType : GridDateDisplayType
    {
        bool showAuthorizedIcon = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresAuthorizationDisplayType()
        {
            UseDescriptiveDates = true;
        }

        /// <summary>
        /// Create the editor for the column display settings
        /// </summary>
        public override GridColumnDisplayEditor CreateEditor()
        {
            return new ProStoresAuthorizationDisplayEditor(this);
        }

        /// <summary>
        /// Indicates if the icon should be shown when its authorized
        /// </summary>
        public bool ShowAuthorizedIcon
        {
            get { return showAuthorizedIcon; }
            set { showAuthorizedIcon = value; }
        }

        /// <summary>
        /// Get the image to use for the given value
        /// </summary>
        protected override Image GetDisplayImage(object value)
        {
            if (value == null || !showAuthorizedIcon)
            {
                return null;
            }

            if (value is DateTime)
            {
                return Resources.check16;
            }

            return null;
        }
    }
}
