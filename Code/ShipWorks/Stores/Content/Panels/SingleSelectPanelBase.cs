using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Grid;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Base for panels that allow single selection
    /// </summary>
    public partial class SingleSelectPanelBase : PanelControlBase
    {
        long? entityID;

        /// <summary>
        /// Constructor
        /// </summary>
        public SingleSelectPanelBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The entity ID, if any, that is currently selected
        /// </summary>
        public long? EntityID
        {
            get { return entityID; }
        }

        /// <summary>
        /// Single selection
        /// </summary>
        public override bool SupportsMultiSelect
        {
            get { return false; }
        }

        /// <summary>
        /// Load the state
        /// </summary>
        public override void LoadState()
        {
            base.LoadState();

            entityGrid.ColumnStrategy.GridColumnApplicabilityContextDataProvider = () => EntityID ?? null;
        }

        /// <summary>
        /// Reload the content of the control with the given selection
        /// </summary>
        public override Task ChangeContent(IGridSelection selection)
        {
            if (selection.Count != 1)
            {
                entityID = null;
            }
            else
            {
                entityID = selection.Keys.First();
            }

            entityGrid.SaveColumns();
            entityGrid.ReloadColumns();

            IEntityGateway gateway = null;

            if (EntityID != null)
            {
                gateway = CreateGateway(EntityID.Value);
            }

            entityGrid.OpenGateway(gateway);

            return TaskEx.FromResult(true);
        }

        /// <summary>
        /// Create a gateway suitable for pulling data for the given entity
        /// </summary>
        protected virtual IEntityGateway CreateGateway(long entityID)
        {
            // Must be overridden
            throw new NotImplementedException();
        }
    }
}
