using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.UI.Controls.Design;

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
        private RatingPanelControl control;

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
        }

        /// <summary>
        /// Handle control load event
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            control = new RatingPanelControl(viewModel);

            elementHost1.Child = control;
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
        public Task ChangeContent(IGridSelection selection)
        {
            return TaskUtility.CompletedTask;
        }

        /// <summary>
        /// Refresh the existing selected content by requerying for the relevant keys to ensure an up-to-date related row
        /// list with up-to-date displayed entity content.
        /// </summary>
        public Task ReloadContent()
        {
            return TaskUtility.CompletedTask;
        }

        /// <summary>
        /// When the content is called to be updated, we need to make sure our rates are up to date as well
        /// </summary>
        public Task UpdateContent()
        {
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
