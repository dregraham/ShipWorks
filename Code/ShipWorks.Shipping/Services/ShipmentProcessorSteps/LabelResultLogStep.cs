using System;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.ShipSense;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps
{
    /// <summary>
    /// Last step in the label creation process
    /// </summary>
    [Component(RegistrationType.Self)]
    public class LabelResultLogStep
    {
        private readonly ILog log;
        private readonly IShippingErrorManager errorManager;
        private readonly ITangoWebClient tangoWebClient;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IShipmentTypeManager shipmentTypeFactory;
        private readonly IShippingManager shippingManager;
        private readonly IKnowledgebase knowledgebase;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public LabelResultLogStep(IShipmentTypeManager shipmentTypeFactory,
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
        public ILabelResultLogResult Complete(ILabelPersistenceResult result)
        {
            ShipmentEntity shipment = result.OriginalShipment;
            Exception exception = result.Exception;

            errorManager.Remove(shipment.ShipmentID);

            if (result.Success)
            {
                exception = LogSuccessfulShipment(shipment, result.Store);
            }

            result.EntityLock?.Dispose();

            bool worldshipExported = shipment.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip;

            IInsufficientFunds outOfFundsException = null;
            ITermsAndConditionsException termsAndConditionsException = null;

            string errorMessage = TranslateException(exception, shipment.ShipmentID, out outOfFundsException, out termsAndConditionsException);

            errorMessage = RefreshShipment(shipment, errorMessage);

            return new LabelResultLogResult(result, worldshipExported, errorMessage, outOfFundsException, termsAndConditionsException);
        }

        /// <summary>
        /// Refresh the processed shipment
        /// </summary>
        private string RefreshShipment(ShipmentEntity shipment, string errorMessage)
        {
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

            return errorMessage;
        }

        /// <summary>
        /// Translate the specified exception into message, setting special exception types if necessary
        /// </summary>
        private string TranslateException(Exception exception, long shipmentID,
            out IInsufficientFunds outOfFundsException, out ITermsAndConditionsException termsAndConditionsException)
        {
            outOfFundsException = null;
            termsAndConditionsException = null;

            if (exception is ORMConcurrencyException ||
                exception is ObjectDeletedException ||
                exception is SqlForeignKeyException)
            {
                return errorManager.SetShipmentErrorMessage(shipmentID, exception, "processed");
            }

            if (exception is ShippingException)
            {
                outOfFundsException = FindTypeInExceptionChain<IInsufficientFunds>(exception);
                termsAndConditionsException = FindTypeInExceptionChain<ITermsAndConditionsException>(exception);
                return errorManager.SetShipmentErrorMessage(shipmentID, exception);
            }

            return exception?.Message;
        }

        /// <summary>
        /// Log the successful shipment to Tango and ShipSense
        /// </summary>
        private Exception LogSuccessfulShipment(ShipmentEntity shipment, StoreEntity store)
        {
            Exception exception = null;

            try
            {
                LogShipmentToTango(shipment, store);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            try
            {
                // Log to the knowledge base after everything else has been successful, so an error logging
                // to the knowledge base does not prevent the shipment from being actually processed.
                ShipmentType shipmentType = shipmentTypeFactory.Get(shipment);
                knowledgebase.LogShipment(shipmentType, shipment);
            }
            catch (Exception ex)
            {
                // TODO: Just log any errors for now
                log.Error(ex);
            }

            return exception;
        }

        /// <summary>
        /// Log the shipment to Tango
        /// </summary>
        private void LogShipmentToTango(ShipmentEntity shipment, StoreEntity store)
        {
            log.InfoFormat("Shipment {0}  - Committed", shipment.ShipmentID);

            // Now log the result to tango.  For WorldShip we can't do this until the shipment comes back in to ShipWorks
            if (!shipment.ProcessingCompletesExternally())
            {
                tangoWebClient.LogShipment(store, shipment);

                log.InfoFormat("Shipment {0}  - Accounted", shipment.ShipmentID);

                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    adapter.SaveAndRefetch(shipment);
                    adapter.Commit();
                }
            }
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
