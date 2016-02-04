using System;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.UI.Controls.UpgradePlan
{
    /// <summary>
    /// Creates an UpgradePlanDlg
    /// </summary>
    public class UpgradePlanDlgFactory : IUpgradePlanDlgFactory
    {
        private readonly Func<string, IDialog> dialogFactory;
        private readonly IUpgradePlanDlgViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradePlanDlgFactory"/> class.
        /// </summary>
        public UpgradePlanDlgFactory(Func<string, IDialog> dialogFactory, IUpgradePlanDlgViewModel viewModel)
        {
            this.dialogFactory = dialogFactory;
            this.viewModel = viewModel;
        }

        /// <summary>
        /// Creates an UpgradePlanDlg with the following message.
        /// </summary>
        public IDialog Create(string message)
        {
            viewModel.Load(message);
            IDialog upgradePlanDlg = dialogFactory("UpgradePlanDlg");
            upgradePlanDlg.DataContext = viewModel;

            return upgradePlanDlg;
        }
    }
}