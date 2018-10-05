using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Messaging.Messages.SingleScan;
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
        private readonly IMessenger messenger;
        private IDisposable subscriptions;

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
        public ShipmentHistoryPanel(ShipmentHistoryGrid shipmentGrid, Func<IUserSession> getUserSession, IMessenger messenger) : this()
        {
            this.messenger = messenger;
            this.getUserSession = getUserSession;
            this.shipmentGrid = shipmentGrid;

            shipmentPanel.Controls.Add(shipmentGrid);
        }

        /// <summary>
        /// Control that displays shipment history
        /// </summary>
        public Control Control => this;

        /// <summary>
        /// Refresh the history, load any components
        /// </summary>
        public void Activate()
        {
            kryptonHeader.Values.Heading = "Today's Shipments for " + getUserSession().User.Username;
            shipmentGrid.Reload();

            Deactivate();

            subscriptions = new CompositeDisposable(
                messenger.OfType<SingleScanMessage>()
                    .Where(x => Visible && CanFocus)
                    .Subscribe(x => searchBox.Text = x.ScannedText),

                messenger.OfType<OrderLookupSearchMessage>()
                    .Where(x => Visible && CanFocus)
                    .Subscribe(x => searchBox.Text = x.SearchText)
            );
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
        
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            Deactivate();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Unload any components
        /// </summary>
        public void Deactivate() => subscriptions?.Dispose();
    }
}
