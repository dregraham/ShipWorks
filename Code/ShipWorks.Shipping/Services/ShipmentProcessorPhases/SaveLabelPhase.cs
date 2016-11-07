﻿using System;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Actions;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Users;

namespace ShipWorks.Shipping.Services.ShipmentProcessorPhases
{
    /// <summary>
    /// Save the processed label
    /// </summary>
    [Component(RegistrationType.Self)]
    public class SaveLabelPhase
    {
        private readonly ILog log;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IActionDispatcher actionDispatcher;
        private readonly IUserSession userSession;
        private readonly IInsureShipService insureShipService;
        private readonly IShipmentTypeManager shipmentTypeFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public SaveLabelPhase(ISqlAdapterFactory sqlAdapterFactory,
            IDateTimeProvider dateTimeProvider,
            IActionDispatcher actionDispatcher,
            IUserSession userSession,
            IInsureShipService insureShipService,
            IShipmentTypeManager shipmentTypeFactory,
            Func<Type, ILog> createLogger)
        {
            this.shipmentTypeFactory = shipmentTypeFactory;
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
        public ISaveLabelResult SaveLabel(IGetLabelResult result)
        {
            if (!result.Success)
            {
                return new SaveLabelResult(result);
            }

            var shipment = result.OriginalShipment;

            using (new LoggedStopwatch(log, "ShippingManager.ProcessShipmentHelper transaction committed."))
            {
                try
                {
                    // Transacted
                    using (ISqlAdapter adapter = sqlAdapterFactory.CreateTransacted())
                    {
                        // Save the label data.
                        result.LabelData.Save();

                        DateTime shipmentDate = dateTimeProvider.UtcNow;

                        log.InfoFormat("Shipment {0}  - ShipmentType.Process Complete", shipment.ShipmentID);

                        if (insureShipService.IsInsuredByInsureShip(shipment))
                        {
                            log.InfoFormat("Shipment {0}  - Insure Shipment Start", shipment.ShipmentID);

                            insureShipService.Insure(shipment, result.Store);
                            log.InfoFormat("Shipment {0}  - Insure Shipment Complete", shipment.ShipmentID);
                        }

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

                        ShipmentType shipmentType = shipmentTypeFactory.Get(shipment);

                        // For WorldShip actions don't happen until the shipment comes back in after being processed in WorldShip
                        if (!shipmentType.ProcessingCompletesExternally)
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
                    return new SaveLabelResult(result, ex);
                }
            }

            return new SaveLabelResult(result);
        }
    }
}
