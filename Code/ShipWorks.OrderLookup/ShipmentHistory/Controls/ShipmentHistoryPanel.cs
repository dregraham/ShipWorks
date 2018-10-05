using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Divelements.SandGrid;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model.EntityClasses;
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
        private readonly IShipmentHistoryVoidProcessor shipmentHistoryVoidProcessor;
        private readonly IMessageHelper messageHelper;
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
        public ShipmentHistoryPanel(
            ShipmentHistoryGrid shipmentGrid,
            Func<IUserSession> getUserSession,
            IMessenger messenger,
            IShipmentHistoryVoidProcessor shipmentHistoryVoidProcessor,
            IMessageHelper messageHelper) : this()
        {
            this.messageHelper = messageHelper;
            this.shipmentHistoryVoidProcessor = shipmentHistoryVoidProcessor;
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
        public void Activate(Divelements.SandRibbon.Button voidButton)
        {
            kryptonHeader.Values.Heading = "Today's Shipments for " + getUserSession().User.Username;
            shipmentGrid.Reload();

            Deactivate();

            voidButton.Activate += OnVoid;
            shipmentGrid.SelectionChanged += OnGridSelectionChanged;
            voidButton.Enabled = false;

            subscriptions = new CompositeDisposable(
                messenger.OfType<SingleScanMessage>()
                    .Where(x => Visible && CanFocus)
                    .Subscribe(x => searchBox.Text = x.ScannedText),

                messenger.OfType<OrderLookupSearchMessage>()
                    .Where(x => Visible && CanFocus)
                    .Subscribe(x => searchBox.Text = x.SearchText),

                Disposable.Create(() => voidButton.Activate -= OnVoid),
                Disposable.Create(() => shipmentGrid.SelectionChanged -= OnGridSelectionChanged)
            );

            // Handle a grid summary change
            void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                var row = e?.Grid.SelectedElements.OfType<ShipWorks.Data.Grid.Paging.PagedEntityGrid.PagedEntityGridRow>()?.FirstOrDefault();
                voidButton.Enabled = e?.Grid.SelectedElementCount == 1 &&
                    row.Entity is ProcessedShipmentEntity shipment &&
                    !shipment.Voided;
                voidButton.Tag = row;
            }
        }

        /// <summary>
        /// Void the shipment
        /// </summary>
        private async void OnVoid(object sender, EventArgs e)
        {
            if (sender is Divelements.SandRibbon.Button voidButton &&
                voidButton.Tag is PagedEntityGrid.PagedEntityGridRow row &&
                row.Entity is ProcessedShipmentEntity processedShipment)
            {
                using (messageHelper.ShowProgressDialog("Voiding", "Voiding shipment"))
                {
                    await Task.Run(() => shipmentHistoryVoidProcessor.Void(processedShipment)
                            .Bind(x => shipmentGrid.RefreshEntity(x))
                            .Do(x => row.LoadRowEntity(x))
                            .Do(x => voidButton.Enabled = !x.Voided))
                        .ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// Save the grid column state
        /// </summary>
        public void SaveGridColumnState() => shipmentGrid.SaveState();

        /// <summary>
        /// Unload any components
        /// </summary>
        public void Deactivate() => subscriptions?.Dispose();

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
    }
}
