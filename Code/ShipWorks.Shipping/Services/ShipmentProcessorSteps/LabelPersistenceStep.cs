using System;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Actions;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Users;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps
{
    /// <summary>
    /// Save the processed label
    /// </summary>
    [Component(RegistrationType.Self)]
    public class LabelPersistenceStep
    {
        private readonly ILog log;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IActionDispatcher actionDispatcher;
        private readonly IUserSession userSession;
        private readonly IInsureShipService insureShipService;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public LabelPersistenceStep(ISqlAdapterFactory sqlAdapterFactory,
            IDateTimeProvider dateTimeProvider,
            IActionDispatcher actionDispatcher,
            IUserSession userSession,
            IInsureShipService insureShipService,
            Func<Type, ILog> createLogger)
        {
            this.insureShipService = insureShipService;
            this.userSession = userSession;
            this.actionDispatcher = actionDispatcher;
            this.dateTimeProvider = dateTimeProvider;
            this.sqlAdapterFactory = sqlAdapterFactory;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Save the label generated in the previous phase
        /// </summary>
        public ILabelPersistenceResult SaveLabel(ILabelRetrievalResult result)
        {
            if (!result.Success)
            {
                return new LabelPersistenceResult(result);
            }

            ShipmentEntity shipment = result.OriginalShipment;

            using (new LoggedStopwatch(log, "ShippingManager.ProcessShipmentHelper transaction committed."))
            {
                try
                {
                    // Insurance makes multiple web calls so it's done outside of the transaction
                    if (insureShipService.IsInsuredByInsureShip(shipment))
                    {
                        log.InfoFormat("Shipment {0}  - Insure Shipment Start", shipment.ShipmentID);
                        insureShipService.Insure(shipment, result.Store);
                        log.InfoFormat("Shipment {0}  - Insure Shipment Complete", shipment.ShipmentID);
                    }

                    // Transacted
                    using (ISqlAdapter adapter = sqlAdapterFactory.CreateTransacted())
                    {
                        // Save the label data.
                        result.LabelData.Save();

                        DateTime shipmentDate = dateTimeProvider.UtcNow;

                        log.InfoFormat("Shipment {0}  - ShipmentType.Process Complete", shipment.ShipmentID);

                        // Now that the label is generated, we can reset the shipping fields the store changed back to their
                        // original values before saving to the database
                        foreach (ShipmentFieldIndex fieldIndex in result.FieldsToRestore)
                        {
                            // Make sure the field is not seen as dirty since we're setting the shipment back to its original value
                            shipment.SetNewFieldValue((int) fieldIndex, result.Clone.GetCurrentFieldValue((int) fieldIndex));
                            shipment.Fields[(int) fieldIndex].IsChanged = false;
                        }

                        shipment.Processed = true;
                        shipment.ProcessedDate = shipmentDate;
                        shipment.ProcessedUserID = userSession.User.UserID;
                        shipment.ProcessedComputerID = userSession.Computer.ComputerID;

                        adapter.SaveAndRefetch(shipment);

                        // For WorldShip actions don't happen until the shipment comes back in after being processed in WorldShip
                        if (!shipment.ProcessingCompletesExternally())
                        {
                            // Dispatch the shipment processed event
                            actionDispatcher.DispatchShipmentProcessed(shipment, adapter);
                            log.InfoFormat("Shipment {0}  - Dispatched", shipment.ShipmentID);
                        }

                        adapter.Commit();
                    }
                }
                catch (Exception ex)
                {
                    return new LabelPersistenceResult(result, ex);
                }
            }

            return new LabelPersistenceResult(result);
        }
    }
}
