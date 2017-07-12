using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.UI;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

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
        public ILocalRateValidationResult Create(IEnumerable<UpsLocalRateDiscrepancy> rateDiscrepancies, IEnumerable<ShipmentEntity> shipments, Action snooze)
        {
            if (!rateDiscrepancies.Any())
            {
                return new SuccessfulLocalRateValidationResult(shipments);
            }

            IDialog upsLocalRateDiscrepancyDialog = windowFactory["UpsLocalRateDiscrepancyDialog"];
            discrepancyViewModel.Snooze = snooze;
            discrepancyViewModel.Close = upsLocalRateDiscrepancyDialog.Close;

            return new FailedLocalRateValidationResult(rateDiscrepancies, shipments, upsLocalRateDiscrepancyDialog, discrepancyViewModel);
        }

        /// <summary>
        /// Creates a LocalRateValidationResult with the specified rate discrepancies.
        /// </summary>
        public ILocalRateValidationResult Create(IEnumerable<UpsLocalRateDiscrepancy> rateDiscrepancies, IEnumerable<ShipmentEntity> shipments)
        {
            if (!rateDiscrepancies.Any())
            {
                return new SuccessfulLocalRateValidationResult(shipments);
            }

            return new FailedLocalRateValidationResult(rateDiscrepancies, shipments);
        }
    }
}
