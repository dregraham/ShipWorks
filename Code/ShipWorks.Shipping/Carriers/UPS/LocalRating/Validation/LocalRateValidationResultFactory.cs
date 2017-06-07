using System;
using Autofac.Features.Indexed;
using Interapptive.Shared.UI;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// Factory for a LocalRateValidationResult
    /// </summary>
    [Component]
    public class LocalRateValidationResultFactory : ILocalRateValidationResultFactory
    {
        private readonly IUpsLocalRateDiscrepancyViewModel discrepancyViewModel;
        private readonly IIndex<string, IDialog> windowFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public LocalRateValidationResultFactory(IUpsLocalRateDiscrepancyViewModel discrepancyViewModel,
            IIndex<string, IDialog> windowFactory)
        {
            this.discrepancyViewModel = discrepancyViewModel;
            this.windowFactory = windowFactory;
        }

        /// <summary>
        /// Creates a ILocalRateValidationResult
        /// </summary>
        public ILocalRateValidationResult Create(int totalShipmentsValidated, int shipmentsWithRateDiscrepancies, Action snooze)
        {
            if (shipmentsWithRateDiscrepancies == 0)
            {
                return new SuccessfulLocalRateValidationResult();
            }

            IDialog upsLocalRateDiscrepancyDialog = windowFactory["UpsLocalRateDiscrepancyDialog"];
            discrepancyViewModel.Snooze = snooze;
            discrepancyViewModel.Close = upsLocalRateDiscrepancyDialog.Close;

            return new FailedLocalRateValidationResult(totalShipmentsValidated, shipmentsWithRateDiscrepancies, upsLocalRateDiscrepancyDialog, discrepancyViewModel);
        }
    }
}
