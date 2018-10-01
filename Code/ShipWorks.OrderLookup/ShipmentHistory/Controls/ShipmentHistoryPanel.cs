using System;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Users;

namespace ShipWorks.OrderLookup.ShipmentHistory.Controls
{
    /// <summary>
    /// Shipment history panel for the OrderLookup mode
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(IShipmentHistory))]
    public partial class ShipmentHistoryPanel : UserControl, IShipmentHistory, IDisposable
    {
        private readonly ShipmentHistoryGrid shipmentGrid;
        private readonly Func<IUserSession> getUserSession;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentHistoryPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentHistoryPanel(ShipmentHistoryGrid shipmentGrid, Func<IUserSession> getUserSession) : this()
        {
            this.getUserSession = getUserSession;
            this.shipmentGrid = shipmentGrid;

            this.shipmentPanel.Controls.Add(shipmentGrid);
        }

        /// <summary>
        /// Control that displays shipment history
        /// </summary>
        public Control Control => this;

        /// <summary>
        /// Update the history
        /// </summary>
        public void ReloadShipmentData()
        {
            kryptonHeader.Values.Heading = "Today's Shipments for " + getUserSession().User.Username;
            shipmentGrid.Reload();
        }

        /// <summary>
        /// Save the grid column state
        /// </summary>
        public void SaveGridColumnState() => shipmentGrid.SaveState();

        /// <summary>
        /// Control is loading
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Dock = DockStyle.Fill;
            shipmentGrid.Dock = DockStyle.Fill;

            // Initialize the shipmentHistory panel
            shipmentGrid.Initialize(new Guid("{C5933658-6323-4599-A81A-15DAF6A07D95}"), GridColumnDefinitionSet.ShipmentsHistory, null);

            shipmentGrid.LoadState();
        }

        /// <summary>
        /// Handle when the text box content changes
        /// </summary>
        private void OnSearchTextBoxTextChanged(object sender, EventArgs e) =>
            shipmentGrid.Search((sender as KryptonTextBox)?.Text);

        /// <summary>
        /// End the search
        /// </summary>
        private void OnEndSearch(object sender, EventArgs e) =>
            shipmentGrid.Search(string.Empty);
    }
}
