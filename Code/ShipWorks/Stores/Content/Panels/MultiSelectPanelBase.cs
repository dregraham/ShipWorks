using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.Paging;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Base for panels that allow single selection
    /// </summary>
    public partial class MultiSelectPanelBase : PanelControlBase
    {
        IGridSelection selection;

        List<long> selectedKeys;
        bool keysAreOrdered = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public MultiSelectPanelBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Multiple selection is supported
        /// </summary>
        public override bool SupportsMultiSelect
        {
            get { return true; }
        }

        /// <summary>
        /// The group by box is always visible
        /// </summary>
        protected override bool ShowFooterControls
        {
            get { return true; }
        }

        /// <summary>
        /// Indicates if multiple selected elements should be shown in groups
        /// </summary>
        protected bool ShowGrouped
        {
            get { return groupBy.Checked; }
        }

        /// <summary>
        /// The currently selected keys
        /// </summary>
        protected List<long> SelectedKeys
        {
            get { return selectedKeys; }
        }

        /// <summary>
        /// Update the content of the control 
        /// </summary>
        public override Task ChangeContent(IGridSelection selection)
        {
            IEntityGateway gateway = null;

            // Save the selection for later in case we need to go from unordered to ordered
            this.selection = selection;
            keysAreOrdered = false;

            if (selection.Count > 0 && selection.Count <= PagedEntityGrid.MaxMultiSelectCount)
            {
                if (ShowGrouped)
                {
                    keysAreOrdered = true;
                    selectedKeys = selection.OrderedKeys.ToList();
                }
                else
                {
                    selectedKeys = selection.Keys.ToList();
                }

                gateway = CreateGateway();
            }
            else
            {
                selectedKeys = new List<long>();
            }

            entityGrid.SaveColumns();
            entityGrid.ReloadColumns();

            entityGrid.GroupingContext = groupBy.Checked ? CreateGroupingContext() : null;
            entityGrid.OpenGateway(gateway);

            return TaskEx.FromResult(true);
        }

        /// <summary>
        /// Create the gateway based on the given selection
        /// </summary>
        protected virtual IEntityGateway CreateGateway()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create the grouping context to use for the panel
        /// </summary>
        protected virtual GroupingContext CreateGroupingContext()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update the positioning of controls
        /// </summary>
        protected override void UpdateLayout()
        {
            base.UpdateLayout();

            // Keep the group by box inline with the addlink
            groupBy.Top = addLink.Top;
        }

        /// <summary>
        /// Changing whether we should do grouping or not
        /// </summary>
        private void OnGroupByChanged(object sender, EventArgs e)
        {
            // If we are grouping, we need to make sure to get the ordered keys
            if (groupBy.Checked && !keysAreOrdered)
            {
                keysAreOrdered = true;
                selectedKeys = selection.OrderedKeys.ToList();
            }

            entityGrid.GroupingContext = groupBy.Checked ? CreateGroupingContext() : null;
        }

        /// <summary>
        /// The user has chosen to sort the grid on a specific column
        /// </summary>
        private void OnGridSorted(object sender, Divelements.SandGrid.GridEventArgs e)
        {
            // If there is now a column being sorted, turn of grouping
            if (entityGrid.SortColumn != null)
            {
                groupBy.Checked = false;
            }
        }
    }
}
