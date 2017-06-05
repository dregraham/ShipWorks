using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.ComponentRegistration;

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
        public ILocalRateValidationResult Create(IEnumerable<UpsLocalRateDiscrepancy> rateDiscrepancies, int totalShipmentsValidated, Action snooze)
        {
            if (!rateDiscrepancies.Any())
            {
                return new SuccessfulLocalRateValidationResult();
            }

            IDialog upsLocalRateDiscrepancyDialog = windowFactory["UpsLocalRateDiscrepancyDialog"];
            discrepancyViewModel.Snooze = snooze;
            discrepancyViewModel.Close = upsLocalRateDiscrepancyDialog.Close;

            return new FailedLocalRateValidationResult(rateDiscrepancies, totalShipmentsValidated, upsLocalRateDiscrepancyDialog, discrepancyViewModel);
        }
    }
}
