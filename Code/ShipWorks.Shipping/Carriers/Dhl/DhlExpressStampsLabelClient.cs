﻿using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// LabelClient for getting DhlExpress labels via stamps
    /// </summary>
    public class DhlExpressStampsLabelClient : IDhlExpressLabelClient
    {
        private readonly IDhlExpressStampsWebClient webClient;
        private readonly Func<StampsLabelResponse, StampsDownloadedLabelData> downloadedLabelDataFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressStampsLabelClient(IDhlExpressStampsWebClient webClient, Func<StampsLabelResponse, StampsDownloadedLabelData> downloadedLabelDataFactory)
        {
            this.webClient = webClient;
            this.downloadedLabelDataFactory = downloadedLabelDataFactory;
        }

        /// <summary>
        /// Create a Stamps Dhl express label 
        /// </summary>
        public async Task<TelemetricResult<IDownloadedLabelData>> CreateLabel(ShipmentEntity shipment)
        {
            TelemetricResult<IDownloadedLabelData> telemetricResult = new TelemetricResult<IDownloadedLabelData>(TelemetricResultBaseName.ApiResponseTimeInMilliseconds);

            var telemetricLabelResponse = await webClient.CreateLabel(shipment).ConfigureAwait(false);

            telemetricLabelResponse.CopyTo(telemetricResult);

            telemetricResult.SetValue(downloadedLabelDataFactory(telemetricLabelResponse.Value));

            return telemetricResult;
        }

        /// <summary>
        /// Void the given shipment
        /// </summary>
        public void Void(ShipmentEntity shipment) => webClient.Void(shipment);
    }
}
