using System.Windows.Forms;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.ApplicationCore;
using Autofac;
using System;
using System.Linq;
using System.Threading.Tasks;
using ShipWorks.Core.Common.Threading;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// Rates panel container
    /// </summary>
    public partial class RatingPanel : UserControl, IDockingPanelContent
    {
        RatingPanelViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public RatingPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle control load event
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            viewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<RatingPanelViewModel>();

            rateControlWrapper.DataBindings.Clear();
            rateControlWrapper.DataBindings.Add("RateGroup", viewModel, "RateGroup");
            rateControlWrapper.DataBindings.Add("ShowSpinner", viewModel, "ShowSpinner");
            //rateControlWrapper.DataBindings.Add("ClearRates", viewModel, "ClearRates");
            //rateControlWrapper.DataBindings.Add("ErrorMessage", viewModel, "ErrorMessage");
            rateControlWrapper.DataBindings.Add("ShipmentType", viewModel, "ShipmentType");
        }

        public EntityType EntityType => EntityType.ShipmentEntity;

        public FilterTarget[] SupportedTargets => new[] { FilterTarget.Orders, FilterTarget.Shipments };

        public bool SupportsMultiSelect => false;

        public async Task ChangeContent(IGridSelection selection)
        {
            await viewModel.LoadRates(selection.Keys.FirstOrDefault());
        }

        public void LoadState()
        {
            //throw new NotImplementedException();
        }

        public Task ReloadContent()
        {
            return TaskUtility.CompletedTask;
        }

        public void SaveState()
        {
            //throw new NotImplementedException();
        }

        public Task UpdateContent()
        {
            return TaskUtility.CompletedTask;
        }

        public void UpdateStoreDependentUI()
        {
            //throw new NotImplementedException();
        }
    }
}
