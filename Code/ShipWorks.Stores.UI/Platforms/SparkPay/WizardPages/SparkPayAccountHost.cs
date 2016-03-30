using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.SparkPay.WizardPages
{
    public partial class SparkPayAccountHost : AddStoreWizardPage
    {
        readonly ISparkPayAccountViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public SparkPayAccountHost(ISparkPayAccountViewModel viewModel)
        {
            this.viewModel = viewModel;
            InitializeComponent();
            accountPage.DataContext = viewModel;
        }

        /// <summary>
        /// Next was clicked
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            bool canMoveNext = viewModel.Save(GetStore<SparkPayStoreEntity>());

            if (!canMoveNext)
            {
                e.NextPage = this;
            }
        }
    }
}