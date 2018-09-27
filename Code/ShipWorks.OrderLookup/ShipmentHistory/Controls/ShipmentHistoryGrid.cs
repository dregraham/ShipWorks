using System;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.Stores.Content.Panels;

namespace ShipWorks.OrderLookup.ShipmentHistory.Controls
{
    /// <summary>
    /// Grid for displaying shipment history
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ShipmentHistoryGrid : SingleSelectPanelBase
    {
        private const long ReloadValue = 0;
        private const long FilterValue = 1;
        private readonly Func<ShipmentHistoryEntityGateway> createGateway;
        private string searchText;
        private ShipmentHistoryEntityGateway gateway;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentHistoryGrid() : base()
        {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentHistoryGrid(Func<ShipmentHistoryEntityGateway> createGateway) : this()
        {
            this.createGateway = createGateway;
        }

        /// <summary>
        /// Handle the load event
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            entityGrid.Dock = DockStyle.Fill;
            addLink.Visible = false;
        }

        /// <summary>
        /// Perform a search using the given text
        /// </summary>
        public void Search(string text)
        {
            searchText = text;

            ChangeContent(FilterValue);
        }

        /// <summary>
        /// Reload the contents of the grid
        /// </summary>
        public void Reload()
        {
            searchText = string.Empty;

            ChangeContent(ReloadValue);
        }

        /// <summary>
        /// EntityType displayed by this panel
        /// </summary>
        public override EntityType EntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// The targets this supports
        /// </summary>
        public override FilterTarget[] SupportedTargets => new[] { FilterTarget.Orders, FilterTarget.Customers };

        /// <summary>
        /// Create the gateway used by the underlying entity grid
        /// </summary>
        protected override IEntityGateway CreateGateway(long entityID) =>
            entityID == FilterValue ?
                gateway.Filter(searchText) :
                (gateway = createGateway());
    }
}
