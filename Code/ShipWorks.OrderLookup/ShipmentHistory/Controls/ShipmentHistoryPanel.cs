using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Divelements.SandGrid;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages.Shipping;
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
        private readonly static Guid gridSettingsKey = new Guid("{C5933658-6323-4599-A81A-15DAF6A07D95}");
        private readonly ShipmentHistoryGrid shipmentGrid;
        private readonly Func<ICurrentUserSettings> getCurrentUserSettings;
        private readonly IMessenger messenger;
        private readonly IPreviousShipmentVoidActionHandler shipmentHistoryVoidProcessor;
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
            Func<ICurrentUserSettings> getCurrentUserSettings,
            IMessenger messenger,
            IPreviousShipmentVoidActionHandler shipmentHistoryVoidProcessor,
            IMessageHelper messageHelper) : this()
        {
            this.messageHelper = messageHelper;
            this.shipmentHistoryVoidProcessor = shipmentHistoryVoidProcessor;
            this.messenger = messenger;
            this.getCurrentUserSettings = getCurrentUserSettings;
            this.shipmentGrid = shipmentGrid;

            shipmentPanel.Controls.Add(shipmentGrid);
        }

        /// <summary>
        /// Control that displays shipment history
        /// </summary>
        public Control Control => this;

        /// <summary>
        /// Number of rows in the grid
        /// </summary>
        public long RowCount => shipmentGrid.RowCount;

        /// <summary>
        /// Refresh the history, load any components
        /// </summary>
        public void Activate(Divelements.SandRibbon.Button voidButton)
        {
            kryptonHeader.Values.Heading = "Today's Shipments for " + getCurrentUserSettings().UserSession.User.Username;
            shipmentGrid.Reload();

            Deactivate();

            voidButton.Activate += OnVoid;
            shipmentGrid.SelectionChanged += OnGridSelectionChanged;
            voidButton.Enabled = false;

            subscriptions = new CompositeDisposable(
                messenger.OfType<SingleScanMessage>()
                    .Where(_ => IsFocusable)
                    .Subscribe(x => searchBox.Text = x.ScannedText),

                messenger.OfType<OrderLookupSearchMessage>()
                    .Where(_ => IsFocusable)
                    .Subscribe(x => searchBox.Text = x.SearchText),

                messenger.OfType<FocusQuickSearchMessage>()
                    .Where(_ => IsFocusable)
                    .Subscribe(_ => searchBox.Focus()),

                messenger.OfType<ShortcutMessage>()
                    .Where(m => m.AppliesTo(KeyboardShortcutCommand.FocusQuickSearch))
                    .ObserveOn(messenger.Schedulers.WindowsFormsEventLoop)
                    .Where(_ => IsFocusable)
                    .Do(_ => searchBox.Focus())
                    .Do(ShowShortcutIndicator)
                    .Subscribe(),

                messenger.OfType<ShortcutMessage>()
                    .Where(m => m.AppliesTo(KeyboardShortcutCommand.ClearQuickSearch))
                    .ObserveOn(messenger.Schedulers.WindowsFormsEventLoop)
                    .Where(_ => IsFocusable)
                    .Do(_ => searchBox.Clear())
                    .Do(ShowShortcutIndicator)
                    .Subscribe(),

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
        /// Is this control focusable
        /// </summary>
        private bool IsFocusable => Visible && base.CanFocus;

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
            shipmentGrid.Initialize(gridSettingsKey, GridColumnDefinitionSet.ShipmentsHistory, layout =>
            {
                layout.DefaultSortColumnGuid = layout.AllColumns[ProcessedShipmentFields.ProcessedDate].Definition.ColumnGuid;
                layout.DefaultSortOrder = ListSortDirection.Descending;
            });

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
        /// Show shortcut indicator
        /// </summary>
        private void ShowShortcutIndicator(ShortcutMessage shortcutMessage)
        {
            if (getCurrentUserSettings().ShouldShowNotification(UserConditionalNotificationType.ShortcutIndicator))
            {
                string actionName = EnumHelper.GetDescription(shortcutMessage.Shortcut.Action);

                if (shortcutMessage.Trigger == ShortcutTriggerType.Hotkey)
                {
                    messageHelper.ShowKeyboardPopup($"{shortcutMessage.Value}: {actionName}");
                }
                else
                {
                    messageHelper.ShowBarcodePopup($"Barcode: {actionName}");
                }
            }
        }

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
