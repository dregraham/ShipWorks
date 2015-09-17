using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Rating;
using TD.SandDock;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// User control that will fetch and 
    /// show rates for an order that has been selected. If an order doesn't have 
    /// any shipments, a message will be displayed. For orders that have multiple 
    /// shipments, the first unprocessed shipment is used for rating. Rates are 
    /// not retrieved for orders that only have processed shipments.
    /// </summary>
    public partial class RatingPanel : UserControl, IDockingPanelContent, IRegisterDockableWindow
    {
        private readonly RatingPanelViewModel viewModel;
        private RateGroup rateGroup;
        private bool showAllRates;
        private bool actionLinkVisible;
        private string errorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingPanel"/> class.
        /// </summary>
        public RatingPanel()
        {
            InitializeComponent();

            viewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<RatingPanelViewModel>();

            DataBindings.Add(nameof(RateGroup), viewModel, nameof(viewModel.RateGroup));
            DataBindings.Add(nameof(ErrorMessage), viewModel, nameof(viewModel.ErrorMessage));
            DataBindings.Add(nameof(ActionLinkVisible), viewModel, nameof(viewModel.ActionLinkVisible));
            DataBindings.Add(nameof(ShowAllRates), viewModel, nameof(viewModel.ShowAllRates));
            
            // Force the rates to be refreshed when the rate control tells us
            rateControl.ReloadRatesRequired += (sender, args) => viewModel.RefreshRates(true);

            rateControl.Initialize(new FootnoteParameters(() => viewModel.RefreshRates(false), () => viewModel.Store));
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
                    rateControl.LoadRates(rateGroup);
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
                    rateControl.ClearRates(errorMessage);
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
                    rateControl.ShowAllRates = showAllRates;
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
                    rateControl.ActionLinkVisible = actionLinkVisible;
                }
            }
        }

        #region IDockingPanelContent
        /// <summary>
        /// Load state.  Currently nothing to load, but needed for IDockingPanelContent
        /// </summary>
        public void LoadState()
        {
        }

        /// <summary>
        /// Save state.  Currently nothing to save, but needed for IDockingPanelContent
        /// </summary>
        public void SaveState()
        {
        }

        /// <summary>
        /// EntityType displayed by this panel
        /// </summary>
        public EntityType EntityType
        {
            get { return EntityType.OrderEntity; }
        }

        /// <summary>
        /// The targets this supports
        /// </summary>
        public FilterTarget[] SupportedTargets
        {
            get { return new FilterTarget[] { FilterTarget.Orders, FilterTarget.Shipments }; }
        }

        /// <summary>
        /// Supports multiple selections
        /// </summary>
        public bool SupportsMultiSelect { get; }

        /// <summary>
        /// Change the order selected
        /// </summary>
        public void ChangeContent(IGridSelection selection)
        {
            // Reset the error message and show the spinner
            viewModel.ErrorMessage = string.Empty;
            rateControl.ShowSpinner = true;

            List<ShipmentEntity> shipments = ShippingManager.GetShipments(selection.Keys.FirstOrDefault(), false);

            viewModel.RefreshSelectedShipments(shipments);
        }

        /// <summary>
        /// Refresh the existing selected content by requerying for the relevant keys to ensure an up-to-date related row 
        /// list with up-to-date displayed entity content.
        /// </summary>
        public void ReloadContent()
        {
            //RefreshSelectedShipments();
        }

        /// <summary>
        /// When the content is called to be updated, we need to make sure our rates are up to date as well
        /// </summary>
        public void UpdateContent()
        {
            //RefreshSelectedShipments();
        }

        /// <summary>
        /// Currently nothing to save, but needed for IDockingPanelContent
        /// </summary>
        public void UpdateStoreDependentUI()
        {
        }
        #endregion

        #region IRegisterDockableWindow
        /// <summary>
        /// Register with the dock manager
        /// </summary>
        public void Register(SandDockManager dockManager)
        {
            RatingPanel panelRating = new RatingPanel
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(1, 1),
                Name = "panelRating",
                Size = new Size(376, 168),
                TabIndex = 1
            };

            DockableWindow dockableWindowRating = new DockableWindow(dockManager, panelRating, "Rating")
            {
                BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat,
                Guid = new Guid("B82A3A5F-931A-40E7-AB35-9189D564C187"),
                Location = new Point(0, 25),
                Name = "dockableWindowRating",
                ShowOptions = false,
                Size = new Size(378, 170),
                TabImage = Properties.Resources.add16,
                TabIndex = 0
            };
        }
        #endregion
    }
}
