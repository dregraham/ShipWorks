using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls.Design;
using ShipWorks.Shipping.UI.MessageHandlers;

namespace ShipWorks.Shipping.UI.RatingPanel
{
    /// <summary>
    /// User control that will fetch and 
    /// show rates for an order that has been selected. If an order doesn't have 
    /// any shipments, a message will be displayed. For orders that have multiple 
    /// shipments, the first unprocessed shipment is used for rating. Rates are 
    /// not retrieved for orders that only have processed shipments.
    /// </summary>
    public partial class RatingPanel : UserControl, IDockingPanelContent
    {
        private readonly RatingPanelViewModel viewModel;
        readonly OrderSelectionChangedHandler orderSelectionChangedHandler;
        private RateGroup rateGroup;
        private bool showAllRates;
        private bool showSpinner;
        private bool actionLinkVisible;
        private string errorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingPanel"/> class.
        /// </summary>
        public RatingPanel()
        {
            InitializeComponent();

            if (DesignModeDetector.IsDesignerHosted())
            {
                return;
            }

            viewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<RatingPanelViewModel>();
            orderSelectionChangedHandler = IoC.UnsafeGlobalLifetimeScope.Resolve<OrderSelectionChangedHandler>();

            DataBindings.Add(nameof(RateGroup), viewModel, nameof(viewModel.RateGroup));
            DataBindings.Add(nameof(ErrorMessage), viewModel, nameof(viewModel.ErrorMessage));
            DataBindings.Add(nameof(ActionLinkVisible), viewModel, nameof(viewModel.ActionLinkVisible));
            DataBindings.Add(nameof(ShowAllRates), viewModel, nameof(viewModel.ShowAllRates));
            DataBindings.Add(nameof(ShowSpinner), viewModel, nameof(viewModel.ShowSpinner));
        }

        /// <summary>
        /// Gets/Sets the rate group 
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RateGroup RateGroup
        {
            get { return rateGroup; }
            set
            {
                if (!Equals(rateGroup, value))
                {
                    rateGroup = value;

                    rateControl.InvokeIfRequired(() => rateControl.LoadRates(rateGroup), true);
                }
            }
        }

        /// <summary>
        /// Gets/Sets any error message
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                if (!value.Equals(errorMessage, StringComparison.InvariantCultureIgnoreCase))
                {
                    errorMessage = value;

                    rateControl.InvokeIfRequired(() => rateControl.ClearRates(errorMessage), true);
                }
            }
        }

        /// <summary>
        /// Gets/Sets whether to show all rates
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowAllRates
        {
            get { return showAllRates; }
            set
            {
                if (!Equals(showAllRates, value))
                {
                    showAllRates = value;
                    rateControl.InvokeIfRequired(() => rateControl.ShowAllRates = showAllRates, true);
                }
            }
        }

        /// <summary>
        /// Gets/Sets whether to show the action link
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ActionLinkVisible
        {
            get { return actionLinkVisible; }
            set
            {
                if (!Equals(actionLinkVisible, value))
                {
                    actionLinkVisible = value;
                    rateControl.InvokeIfRequired(() => rateControl.ActionLinkVisible = actionLinkVisible, true);
                }
            }
        }

        /// <summary>
        /// Gets/Sets whether to show the spinner
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowSpinner
        {
            get { return showSpinner; }
            set
            {
                if (!Equals(showSpinner, value))
                {
                    showSpinner = value;
                    rateControl.InvokeIfRequired(() => rateControl.ShowSpinner = showSpinner, true);
                }
            }
        }

        /// <summary>
        /// Handle control load event
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Force the rates to be refreshed when the rate control tells us
            rateControl.ReloadRatesRequired += (sender, args) =>
            {
                viewModel.RefreshRates(true);
            };

            rateControl.Initialize(new FootnoteParameters(() => viewModel.RefreshRates(false), () => viewModel.Store));

            rateControl.RateSelected += OnRateControlRateSelected;

            orderSelectionChangedHandler.Listen(viewModel.RefreshSelectedShipments);
        }

        /// <summary>
        /// Event handler for rate selection chaning on the rate control
        /// </summary>
        private void OnRateControlRateSelected(object sender, RateSelectedEventArgs e)
        {
            viewModel.SelectedRateResult = e.Rate;
        }

        /// <summary>
        /// Supported entity type
        /// </summary>
        public EntityType EntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// Supported filter targets
        /// </summary>
        public FilterTarget[] SupportedTargets => new[] { FilterTarget.Orders, FilterTarget.Shipments };

        /// <summary>
        /// Supports multiple selections
        /// </summary>
        public bool SupportsMultiSelect => false;


        #region IDockingPanelContent
        /// <summary>
        /// Load state.  Currently nothing to load, but needed for IDockingPanelContent
        /// </summary>
        public void LoadState()
        {
            // Rating panel does not have extra state
        }

        /// <summary>
        /// Save state.  Currently nothing to save, but needed for IDockingPanelContent
        /// </summary>
        public void SaveState()
        {
            // Rating panel does not have extra state
        }

        /// <summary>
        /// Change the order selected
        /// </summary>
        public async Task ChangeContent(IGridSelection selection)
        {
            // Reset the error message and show the spinner
            //viewModel.ErrorMessage = string.Empty;
            rateControl.ShowSpinner = true;
        }

        /// <summary>
        /// Refresh the existing selected content by requerying for the relevant keys to ensure an up-to-date related row 
        /// list with up-to-date displayed entity content.
        /// </summary>
        public Task ReloadContent()
        {
            //viewModel.RefreshRates(true);
            return TaskUtility.CompletedTask;
        }

        /// <summary>
        /// When the content is called to be updated, we need to make sure our rates are up to date as well
        /// </summary>
        public Task UpdateContent()
        {
            //viewModel.RefreshRates(true);
            return TaskUtility.CompletedTask;
        }

        /// <summary>
        /// Currently nothing to save, but needed for IDockingPanelContent
        /// </summary>
        public void UpdateStoreDependentUI()
        {
            // Rating panel does not have extra state
        }
        #endregion
    }
}
