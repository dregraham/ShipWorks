using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Data.Grid
{
    /// <summary>
    /// Base strategy class for controlling how columns are loaded, manipulated, and saved in the entity grid.
    /// </summary>
    public abstract class EntityGridColumnStrategy
    {
        /// <summary>
        /// Load the full set of configured grid columns into the grid.
        /// </summary>
        public abstract void LoadColumns(EntityGrid grid);

        /// <summary>
        /// Persist the set of grid columns and settings from the grid .
        /// </summary>
        public abstract void SaveColumns(EntityGrid grid);

        /// <summary>
        /// Apply the user-configured initial sort to the given grid.
        /// </summary>
        public abstract void ApplyInitialSort(EntityGrid grid);

        /// <summary>
        /// Create a UserControl for editing grid settings in a modeless interactive manner.  Any changes
        /// should be cause immediate invocation of the given callback.
        /// </summary>
        public abstract UserControl CreatePopupEditor(EntityGrid grid, MethodInvoker interactiveEditCallback);

        /// <summary>
        /// Create Form that can be used for modal editing of the column settings.
        /// </summary>
        public abstract Form CreateModalEditor();

        /// <summary>
        /// A function callback that provides context data to the grid column applicabilty test.
        /// </summary>
        public Func<object> GridColumnApplicabilityContextDataProvider { get; set; }
    }
}