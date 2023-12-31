﻿using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps
{
    /// <summary>
    /// Result of the get label phase
    /// </summary>
    public class LabelRetrievalResult : ILabelRetrievalResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LabelRetrievalResult(IShipmentPreparationResult result)
        {
            Index = result.Index;
            EntityLock = result.EntityLock;
            Exception = result.Exception;
            Canceled = result.Canceled;
            Store = result.Store;
            OriginalShipment = result.OriginalShipment;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelRetrievalResult(IShipmentPreparationResult result, bool canceled) :
            this(result)
        {
            Canceled = canceled;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelRetrievalResult(IShipmentPreparationResult result, ShippingException exception) :
            this(result)
        {
            Exception = exception;
        }


        public LabelRetrievalResult(IShipmentPreparationResult result, ShipmentEntity shipment, ShippingException exception) :
            this(result, exception)
        {
            OriginalShipment = shipment;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelRetrievalResult(IShipmentPreparationResult result, TelemetricResult<IDownloadedLabelData> telemetricLabelData,
            ShipmentEntity shipment, ShipmentEntity clone,
            List<ShipmentFieldIndex> fieldsToRestore) :
            this(result)
        {
            Index = result.Index;
            LabelData = telemetricLabelData.Value;
            Telemetry = telemetricLabelData;
            OriginalShipment = shipment;
            Clone = clone;
            FieldsToRestore = fieldsToRestore;
        }

        /// <summary>
        /// Index of the shipment being processed
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Clone made of the original shipment
        /// </summary>
        public ShipmentEntity Clone { get; }

        /// <summary>
        /// Fields that may need to be restored on the original shipment
        /// </summary>
        public List<ShipmentFieldIndex> FieldsToRestore { get; }

        /// <summary>
        /// Label data that was retrieved for the shipment
        /// </summary>
        public IDownloadedLabelData LabelData { get; }

        /// <summary>
        /// Original shipment used for processing
        /// </summary>
        public ShipmentEntity OriginalShipment { get; }

        /// <summary>
        /// Store associated with the shipment
        /// </summary>
        public StoreEntity Store { get; }

        /// <summary>
        /// Existing entity lock
        /// </summary>
        public IDisposable EntityLock { get; }

        /// <summary>
        /// Was the process canceled
        /// </summary>
        public bool Canceled { get; }

        /// <summary>
        /// Exception generated during the get label phase or earlier
        /// </summary>
        public ShippingException Exception { get; }

        /// <summary>
        /// Was the get label phase successful
        /// </summary>
        public bool Success => Exception == null && !Canceled;

        /// <summary>
        /// Telemetry properties related to this result
        /// </summary>
        public TelemetricResult<IDownloadedLabelData> Telemetry { get; }
    }
}