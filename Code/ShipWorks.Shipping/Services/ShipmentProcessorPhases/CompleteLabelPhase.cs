using System;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.ShipSense;

namespace ShipWorks.Shipping.Services.ShipmentProcessorPhases
{
    /// <summary>
    /// Last step in the label creation process
    /// </summary>
    [Component(RegistrationType.Self)]
    public class CompleteLabelPhase
    {
        private readonly ILog log;
        private readonly IShippingErrorManager errorManager;
        private readonly ITangoWebClient tangoWebClient;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IShipmentTypeManager shipmentTypeFactory;
        private readonly IShippingManager shippingManager;
        readonly IKnowledgebase knowledgebase;

        /// <summary>
        /// Constructor
        /// </summary>
        public CompleteLabelPhase(IShipmentTypeManager shipmentTypeFactory,
            IShippingManager shippingManager,
            IShippingErrorManager errorManager,
            ISqlAdapterFactory sqlAdapterFactory,
            ITangoWebClient tangoWebClient,
            IKnowledgebase knowledgebase,
            Func<Type, ILog> createLogger)
        {
            this.knowledgebase = knowledgebase;
            this.shippingManager = shippingManager;
            this.shipmentTypeFactory = shipmentTypeFactory;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.tangoWebClient = tangoWebClient;
            this.errorManager = errorManager;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Complete the label creation process
        /// </summary>
        public ICompleteLabelCreationResult Finish(ISaveLabelResult result)
        {
            ShipmentEntity shipment = result.OriginalShipment;
            Exception exception = result.Exception;
            string orderHash = null;

            errorManager.Remove(shipment.ShipmentID);

            if (result.Success)
            {
                ShipmentType shipmentType = shipmentTypeFactory.Get(shipment);

                try
                {
                    log.InfoFormat("Shipment {0}  - Committed", shipment.ShipmentID);

                    // Now log the result to tango.  For WorldShip we can't do this until the shipment comes back in to ShipWorks
                    if (!shipmentType.ProcessingCompletesExternally)
                    {
                        tangoWebClient.LogShipment(result.Store, shipment);

                        log.InfoFormat("Shipment {0}  - Accounted", shipment.ShipmentID);

                        using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                        {
                            adapter.SaveAndRefetch(shipment);
                            adapter.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                try
                {
                    // Log to the knowledge base after everything else has been successful, so an error logging
                    // to the knowledge base does not prevent the shipment from being actually processed.
                    knowledgebase.LogShipment(shipmentType, shipment);
                }
                catch (Exception ex)
                {
                    // TODO: Just log any errors for now
                    log.Error(ex);
                }
            }

            string errorMessage = result.Exception?.Message;
            IInsufficientFunds outOfFundsException = null;
            ITermsAndConditionsException termsAndConditionsException = null;

            if (exception is ORMConcurrencyException ||
                exception is ObjectDeletedException ||
                exception is SqlForeignKeyException)
            {
                errorMessage = errorManager.SetShipmentErrorMessage(shipment.ShipmentID, exception, "processed");
            }
            else if (exception is ShippingException)
            {
                errorMessage = errorManager.SetShipmentErrorMessage(shipment.ShipmentID, exception);
                outOfFundsException = FindTypeInExceptionChain<IInsufficientFunds>(exception);
                termsAndConditionsException = FindTypeInExceptionChain<ITermsAndConditionsException>(exception);
            }

            result.EntityLock?.Dispose();

            bool worldshipExported = shipment.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip;

            try
            {
                // Reload it so we can show the most recent data when the grid redisplays
                shippingManager.RefreshShipment(shipment);
            }
            catch (ObjectDeletedException ex)
            {
                // If there wasn't already a different error, set this as the error
                if (errorMessage == null)
                {
                    errorMessage = "The shipment has been deleted.";
                    errorManager.SetShipmentErrorMessage(shipment.ShipmentID, new ShippingException(errorMessage, ex));
                }
            }

            return new CompleteLabelCreationResult(result, worldshipExported, errorMessage, outOfFundsException, termsAndConditionsException);
        }

        /// <summary>
        /// Find an exception of the specified type in the exception chain
        /// </summary>
        private T FindTypeInExceptionChain<T>(Exception exception) where T : class
        {
            if (exception == null)
            {
                return null;
            }

            return exception as T ?? FindTypeInExceptionChain<T>(exception.InnerException);
        }
    }
}
