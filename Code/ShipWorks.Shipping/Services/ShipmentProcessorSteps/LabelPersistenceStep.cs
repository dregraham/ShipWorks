using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Interapptive.Shared;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Users;
using ShipWorks.Users.Audit;

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
        /// Save both labels generated in the previous phase
        /// </summary>
        public IEnumerable<ILabelPersistenceResult> SaveLabels(IEnumerable<ILabelRetrievalResult> labelRetrievalResults)
        {
            List<ILabelPersistenceResult> labelPersistenceResults = new List<ILabelPersistenceResult>();

            foreach (ILabelRetrievalResult result in labelRetrievalResults)
            {
                if (result.Success)
                {
                    labelPersistenceResults.Add(SaveSingleLabel(result));
                }
                else
                {
                    labelPersistenceResults.Add(new LabelPersistenceResult(result));
                }
            }

            return labelPersistenceResults;
        }

        /// <summary>
        /// Save an individual label
        /// </summary>
        private LabelPersistenceResult SaveSingleLabel(ILabelRetrievalResult result)
        {
            ShipmentEntity shipment = result.OriginalShipment;
            ShipmentEntity shipmentForTango = result.OriginalShipment;

            using (new LoggedStopwatch(log, "ShippingManager.ProcessShipmentHelper transaction committed."))
            {
                try
                {
                    bool insureShipSucceeded = EnsureShipmentIsInsured(shipment);

                    try
                    {
                        log.Info("LabelPersistenceStep.SaveSingleLabel: LabelData.Save");
                        result.LabelData.Save();

                        log.InfoFormat("Shipment {0} - ShipmentType.Process Complete", shipment.ShipmentID);

                        MarkShipmentAsProcessed(shipment);

                        shipmentForTango = ResetTemporaryAddressChanges(result, shipment);

                        SaveSingleLabelTransacted(result, shipment);
                    }
                    catch (Exception ex) when (ex is ORMConcurrencyException || ex is TransactionInDoubtException)
                    {
                        log.Error("Error saving label, retrying.", ex);

                        // Try to get the shipment from the db and make the changes to it, and re-save.
                        ShipmentEntity dbShipment = ShippingManager.GetShipment(shipment.ShipmentID);
                        ShippingManager.EnsureShipmentLoaded(dbShipment);

                        EntityUtility.CopyChangedFields(shipment, dbShipment);

                        SaveSingleLabelTransacted(result, dbShipment);
                        shipment = dbShipment;
                    }

                    using (new AuditBehaviorScope(AuditBehaviorUser.SuperUser, new AuditReason(AuditReasonType.Default), AuditState.Disabled))
                    {
                        using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                        {
                            adapter.KeepConnectionOpen = true;
                            ClearNonActiveShipmentData(shipment, adapter);
                        }
                    }

                    if (!insureShipSucceeded)
                    {
                        string exceptionMessage = "Insuring the shipment failed. If insurance is required, please void the shipment and try again.";

                        return new LabelPersistenceResult(result, shipmentForTango, new InsureShipException(exceptionMessage));
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"Exception during LabelPersistenceStep.SaveSingleLabel", ex);
                    return new LabelPersistenceResult(result, ex);
                }
            }

            return new LabelPersistenceResult(result, shipmentForTango);
        }

        /// <summary>
        /// Save the shipment to the db using a transaction
        /// </summary>
        private void SaveSingleLabelTransacted(ILabelRetrievalResult result, ShipmentEntity shipment)
        {
            using (ISqlAdapter adapter = sqlAdapterFactory.CreateTransacted())
            {
                // Assuming that this save SHOULD be the one that wins since we have a label that could 
                // be bought with out of pocket money.
                // Use the ShipmentIgnoreConcurrencyScope so that any ORMConcurrencyExceptions should not
                // occur.  
                // We should only use this in VERY SPECIFIC SCENARIOS, not just anywhere where 
                // ORMConcurrencyExceptions are found.
                using (new ShipmentIgnoreConcurrencyScope(shipment))
                {
                    SaveShipment(shipment, adapter);
                }

                log.Info("LabelPersistenceStep.SaveSingleLabel: adapter.Commit()");
                adapter.Commit();
            }

            // Dispatch actions in a separate transaction i dont know why
            // and i dont want to know why but if we save the shipment and 
            // dispatch actions in the same transaction it throws a 
            // TransactionInDoubtException very rarely
            using (ISqlAdapter adapter = sqlAdapterFactory.CreateTransacted())
            {
                DispatchShipmentProcessedActions(shipment, adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Clears out any other shipment data that is not application to the actual type of the shipment
        /// </summary>
        /// <param name="shipment">Shipment from which to clear extra data</param>
        /// <param name="adapter">SqlAdapter that will be used to delete other shipment data</param>
        private static void ClearNonActiveShipmentData(ShipmentEntity shipment, ISqlAdapter adapter)
        {
            ClearOtherShipmentData(adapter, shipment, typeof(UpsShipmentEntity), UpsShipmentFields.ShipmentID, ShipmentTypeCode.UpsOnLineTools, ShipmentTypeCode.UpsWorldShip);
            ClearOtherShipmentData(adapter, shipment, typeof(EndiciaShipmentEntity), EndiciaShipmentFields.ShipmentID, ShipmentTypeCode.Endicia, ShipmentTypeCode.Express1Endicia);
            ClearOtherShipmentData(adapter, shipment, typeof(UspsShipmentEntity), UspsShipmentFields.ShipmentID, ShipmentTypeCode.Express1Usps, ShipmentTypeCode.Usps);
            ClearOtherShipmentData(adapter, shipment, typeof(PostalShipmentEntity), PostalShipmentFields.ShipmentID, ShipmentTypeCode.PostalWebTools, ShipmentTypeCode.Endicia, ShipmentTypeCode.Usps, ShipmentTypeCode.Express1Endicia, ShipmentTypeCode.Express1Usps);
            ClearOtherShipmentData(adapter, shipment, typeof(FedExShipmentEntity), FedExShipmentFields.ShipmentID, ShipmentTypeCode.FedEx);
            ClearOtherShipmentData(adapter, shipment, typeof(OnTracShipmentEntity), OnTracShipmentFields.ShipmentID, ShipmentTypeCode.OnTrac);
            ClearOtherShipmentData(adapter, shipment, typeof(IParcelShipmentEntity), IParcelShipmentFields.ShipmentID, ShipmentTypeCode.iParcel);
            ClearOtherShipmentData(adapter, shipment, typeof(OtherShipmentEntity), OtherShipmentFields.ShipmentID, ShipmentTypeCode.Other);
            ClearOtherShipmentData(adapter, shipment, typeof(BestRateShipmentEntity), BestRateShipmentFields.ShipmentID, ShipmentTypeCode.BestRate);
            ClearOtherShipmentData(adapter, shipment, typeof(DhlExpressShipmentEntity), DhlExpressShipmentFields.ShipmentID, ShipmentTypeCode.DhlExpress);
            ClearOtherShipmentData(adapter, shipment, typeof(DhlEcommerceShipmentEntity), DhlEcommerceShipmentFields.ShipmentID, ShipmentTypeCode.DhlEcommerce);
            ClearOtherShipmentData(adapter, shipment, typeof(AmazonSFPShipmentEntity), AmazonSFPShipmentFields.ShipmentID, ShipmentTypeCode.AmazonSFP);
            ClearOtherShipmentData(adapter, shipment, typeof(AsendiaShipmentEntity), AsendiaShipmentFields.ShipmentID, ShipmentTypeCode.Asendia);
        }

        /// <summary>
        /// Clear specified shipment data if not relevant
        /// </summary>
        /// <param name="adapter">SqlAdapter that will be used to delete child shipment entities</param>
        /// <param name="shipment">Shipment from which child shipment data will be deleted</param>
        /// <param name="childShipmentType">Type of child shipment that should be deleted</param>
        /// <param name="shipmentIdField">Field that specifies the ShipmentId for the child</param>
        /// <param name="requiredForTypes">Delete this child shipment unless it is one of the specified types</param>
        private static void ClearOtherShipmentData(ISqlAdapter adapter, ShipmentEntity shipment, Type childShipmentType, EntityField2 shipmentIdField, params ShipmentTypeCode[] requiredForTypes)
        {
            if (requiredForTypes.Contains(shipment.ShipmentTypeCode))
            {
                return;
            }

            adapter.DeleteEntitiesDirectly(childShipmentType, new RelationPredicateBucket(shipmentIdField == shipment.ShipmentID));
        }

        /// <summary>
        /// Save the shipment before dispatching.
        /// </summary>
        private void SaveShipment(ShipmentEntity shipment, ISqlAdapter sqlAdapter)
        {
            sqlAdapter.SaveAndRefetch(shipment);

            // SafeAndRefetch doesn't delete the entities, so force the delete here.
            if (shipment.CustomsItems.RemovedEntitiesTracker?.Count > 0)
            {
                sqlAdapter.DeleteEntityCollection(shipment.CustomsItems.RemovedEntitiesTracker);
                shipment.CustomsItems.RemovedEntitiesTracker.Clear();
            }
        }

        /// <summary>
        /// Mark the shipment as processed
        /// </summary>
        private void MarkShipmentAsProcessed(ShipmentEntity shipment)
        {
            log.Info("LabelPersistenceStep.MarkShipmentAsProcessed");

            shipment.Processed = true;
            shipment.ProcessedDate = dateTimeProvider.UtcNow;
            shipment.ProcessedUserID = userSession.User.UserID;
            shipment.ProcessedComputerID = userSession.Computer.ComputerID;
            shipment.ProcessedWithUiMode = userSession.Settings.UIMode;
        }

        /// <summary>
        /// Dispatch shipment processed actions, if necessary
        /// </summary>
        private void DispatchShipmentProcessedActions(IShipmentEntity shipment, ISqlAdapter adapter)
        {
            log.Info("LabelPersistenceStep.DispatchShipmentProcessedActions");

            // For WorldShip actions don't happen until the shipment comes back in after being processed in WorldShip
            if (!shipment.ProcessingCompletesExternally())
            {
                // Dispatch the shipment processed event
                actionDispatcher.DispatchShipmentProcessed(shipment, adapter);
                log.InfoFormat("Shipment {0}  - Dispatched", shipment.ShipmentID);
            }
        }

        /// <summary>
        /// Reset the address changes that were made temporarily for services like GSP
        /// </summary>
        private ShipmentEntity ResetTemporaryAddressChanges(ILabelRetrievalResult result, ShipmentEntity shipment)
        {
            log.Info("LabelPersistenceStep.ResetTemporaryAddressChanges");

            if (result.FieldsToRestore.None())
            {
                return shipment;
            }

            var modifiedShipment = EntityUtility.CloneEntity(shipment);

            // Now that the label is generated, we can reset the shipping fields the store changed back to their
            // original values before saving to the database
            foreach (ShipmentFieldIndex fieldIndex in result.FieldsToRestore)
            {
                // Make sure the field is not seen as dirty since we're setting the shipment back to its original value
                shipment.SetNewFieldValue((int) fieldIndex, result.Clone.GetCurrentFieldValue((int) fieldIndex));
                shipment.Fields[(int) fieldIndex].IsChanged = false;
            }

            return modifiedShipment;
        }

        /// <summary>
        /// Ensure that the shipment is insured by our service, if necessary
        /// </summary>
        private bool EnsureShipmentIsInsured(ShipmentEntity shipment)
        {
            // Insurance makes multiple web calls so it's done outside of the transaction
            if (insureShipService.IsInsuredByInsureShip(shipment))
            {
                log.InfoFormat("Shipment {0}  - Insure Shipment Start", shipment.ShipmentID);
                var insuranceResult = insureShipService.Insure(shipment);

                if (insuranceResult.Failure)
                {
                    ShipmentTypeManager.GetType(shipment).UnsetInsurance(shipment);
                    log.Error($"Shipment {shipment.ShipmentID}  - Insure Shipment failed with the following error: {insuranceResult.Exception.Message}");
                    return false;
                }
                else
                {
                    log.InfoFormat("Shipment {0}  - Insure Shipment Completed Successfully", shipment.ShipmentID);
                }
            }

            return true;
        }
    }
}
