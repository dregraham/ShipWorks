using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using System;
using System.Collections.Generic;

namespace ShipWorks.Shipping
{
    public class ShippingErrorManager : IShippingErrorManager
    {
        /// <summary>
        /// Messages that should be used when trying to save a shipment
        /// </summary>
        readonly static Dictionary<Type, string> exceptionMessages = new Dictionary<Type, string>
        {
            { typeof(ORMConcurrencyException), "Another user had recently made changes, so the shipment was not {0}." },
            { typeof(ObjectDeletedException), "The shipment has been deleted." },
            { typeof(SqlForeignKeyException), "The shipment has been deleted." },
        };

        // The singleton list of the current set of shipping errors.
        private Dictionary<long, Exception> processingErrors = new Dictionary<long, Exception>();

        /// <summary>
        /// Is there currently an error for the specified shipment id?
        /// </summary>
        public bool ShipmentHasError(long shipmentId) => ProcessingErrors.ContainsKey(shipmentId);

        /// <summary>
        /// The singleton instance of the processing errors from the last time shipments were processed using the shipping window.  This works
        /// because the shipping window can only be open at most once on the screen.
        /// </summary>
        public Dictionary<long, Exception> ProcessingErrors => processingErrors;

        /// <summary>
        /// Set an error on the processing errors collection
        /// </summary>
        public string SetShipmentErrorMessage(long shipmentID, Exception ex)
        {
            processingErrors[shipmentID] = ex;
            return ex.Message;
        }

        /// <summary>
        /// Set an error on the processing errors collection
        /// </summary>
        public string SetShipmentErrorMessage(long shipmentID, Exception exception, string actionName)
        {
            if (exception == null)
            {
                return null;
            }

            string errorMessage;
            if (exceptionMessages.TryGetValue(exception.GetType(), out errorMessage))
            {
                errorMessage = string.Format(errorMessage, actionName);
                processingErrors[shipmentID] = new ShippingException(errorMessage, exception);
            }

            return errorMessage;
        }

        /// <summary>
        /// Clear all shipping errors
        /// </summary>
        public void Clear()
        {
            processingErrors.Clear();
        }

        /// <summary>
        /// Remove the specific shipping error
        /// </summary>
        public void Remove(long shipmentID)
        {
            processingErrors.Remove(shipmentID);
        }

        /// <summary>
        /// Get an error for the given shipment, if there is one
        /// </summary>
        public Exception GetErrorForShipment(long shipmentID)
        {
            Exception error;
            ProcessingErrors.TryGetValue(shipmentID, out error);
            return error;
        }
    }
}
