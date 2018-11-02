using System;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
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
        private readonly IMainForm mainForm;
        private string searchText;
        private ShipmentHistoryEntityGateway gateway;
        private readonly Func<string, ITrackedDurationEvent> telemetryFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentHistoryGrid() : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentHistoryGrid(Func<ShipmentHistoryEntityGateway> createGateway, IMainForm mainForm, Func<string, ITrackedDurationEvent> telemetryFactory) : this()
        {
            this.createGateway = createGateway;
            this.mainForm = mainForm;
            this.telemetryFactory = telemetryFactory;

            entityGrid.RowLoadingComplete += OnEntityGridRowLoadingComplete;
        }

        /// <summary>
        /// Number of rows in the grid
        /// </summary>
        public long RowCount { get; private set; }

        /// <summary>
        /// Event handler for when the grid finishes loading rows.
        /// </summary>
        private void OnEntityGridRowLoadingComplete(object sender, EventArgs e)
        {
            using (ITrackedDurationEvent telemetryEvent = telemetryFactory("OrderLookup.Shipment.History"))
            {
                RowCount = entityGrid.Rows.Count;
                mainForm.UpdateStatusBar();

                telemetryEvent.AddMetric("Shipment.History.Shipment.Count", RowCount);
            }

            RowCount = entityGrid.Rows.Count;
            mainForm.UpdateStatusBar();
        }

        /// <summary>
        /// A selection in the grid has changed
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;

        /// <summary>
        /// Handle the load event
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            entityGrid.SelectionChanged += OnEntityGridSelectionChanged;
            entityGrid.AllowMultipleSelection = false;
            entityGrid.Dock = DockStyle.Fill;
            addLink.Visible = false;
        }

        /// <summary>
        /// Handle when the selection has changed in the grid
        /// </summary>
        private void OnEntityGridSelectionChanged(object sender, SelectionChangedEventArgs e) =>
            SelectionChanged?.Invoke(this, e);

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
        /// Refresh the 
        /// </summary>
        public GenericResult<ProcessedShipmentEntity> RefreshEntity(ProcessedShipmentEntity shipment) =>
            gateway.RefreshEntity(shipment);

        /// <summary>
        /// Create the gateway used by the underlying entity grid
        /// </summary>
        protected override IEntityGateway CreateGateway(long entityID) =>
            entityID == FilterValue ?
                gateway.Filter(searchText) :
                (gateway = createGateway());
    }
}
