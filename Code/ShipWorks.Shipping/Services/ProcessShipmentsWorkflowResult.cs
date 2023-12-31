﻿using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services.ProcessShipmentsWorkflow;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// State used when processing shipments
    /// </summary>
    internal class ProcessShipmentsWorkflowResult : IProcessShipmentsWorkflowResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProcessShipmentsWorkflowResult(RateResult chosenRate)
        {
            SelectedRate = chosenRate;

            NewErrors = new List<string>();
            ConcurrencyErrors = new Dictionary<ShipmentEntity, Exception>();

            // Special cases - I didn't think we needed to abstract these.  If it gets out of hand we should refactor.
            WorldshipExported = false;
            OutOfFundsException = null;
            TermsAndConditionsException = null;

            // Maps storeID's to license exceptions, so we only have to check a store once per processing batch
            LicenseCheckResults = new Dictionary<long, Exception>();

            OrderHashes = new List<string>();

            Shipments = new List<ShipmentEntity>();
        }

        /// <summary>
        /// List of new errors
        /// </summary>
        public IList<string> NewErrors { get; }

        /// <summary>
        /// List of concurrency errors
        /// </summary>
        public IDictionary<ShipmentEntity, Exception> ConcurrencyErrors { get; internal set; }

        /// <summary>
        /// Results of store license checks
        /// </summary>
        public IDictionary<long, Exception> LicenseCheckResults { get; internal set; }

        /// <summary>
        /// Order hashes
        /// </summary>
        public IList<string> OrderHashes { get; internal set; }

        /// <summary>
        /// Out of funds exception that may have been generated while processing
        /// </summary>
        public IInsufficientFunds OutOfFundsException { get; internal set; }

        /// <summary>
        /// Terms and conditions exception that may have been generated while processing
        /// </summary>
        public ITermsAndConditionsException TermsAndConditionsException { get; internal set; }

        /// <summary>
        /// Is this for Worldship
        /// </summary>
        public bool WorldshipExported { get; internal set; }

        /// <summary>
        /// Selected rate for processing
        /// </summary>
        public RateResult SelectedRate { get; }

        /// <summary>
        /// Local Rate ValidationResult
        /// </summary>
        public ILocalRateValidationResult LocalRateValidationResult { get; set; }

        /// <summary>
        /// List of shipments
        /// </summary>
        public IList<ShipmentEntity> Shipments { get; }
    }
}