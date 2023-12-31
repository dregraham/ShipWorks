﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.IO.Hardware.Scales;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.SingleScan
{
    [Component]
    public class AutoWeighService : IAutoWeighService
    {
        private readonly ISingleScanAutomationSettings singleScanAutomationSettings;
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;
        private readonly IMessageHelper messageHelper;
        private readonly IScaleReader scaleReader;
        private readonly ILog log;

        // .1 oz
        private const double WeightDifferenceToIgnore = .00625;

        // .05 oz
        private const double MinimumWeight = 0.003125;

        private const int ScaleTimeoutInSeconds = 2;

        public const string TelemetryPropertyName = "SingleScan.AutoWeigh";

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoWeighService"/> class.
        /// </summary>
        public AutoWeighService(ISingleScanAutomationSettings singleScanAutomationSettings,
            ICarrierShipmentAdapterFactory shipmentAdapterFactory,
            IMessageHelper messageHelper,
            IScaleReader scaleReader,
            Func<Type, ILog> logFactory)
        {
            this.singleScanAutomationSettings = singleScanAutomationSettings;
            this.shipmentAdapterFactory = shipmentAdapterFactory;
            this.messageHelper = messageHelper;
            this.scaleReader = scaleReader;
            this.log = logFactory(typeof(AutoWeighService));
        }

        /// <summary>
        /// Applies the weight on the scale to the specified shipments
        /// </summary>
        [NDependIgnoreLongMethod]
        public bool ApplyWeight(IEnumerable<ShipmentEntity> shipments, ITrackedEvent trackedDurationEvent)
        {
            if (!singleScanAutomationSettings.IsAutoWeighEnabled)
            {
                log.Debug("AutoWeigh is turned off");
                CollectTelemetryData(trackedDurationEvent, "N/A");
                return true;
            }

            Task<ScaleReadResult> scaleReaderTask = scaleReader.ReadScale();
            bool scaleRead = scaleReaderTask.Wait(TimeSpan.FromSeconds(ScaleTimeoutInSeconds));
            if (!scaleRead)
            {
                log.Error($"scaleReader timed out.");
                messageHelper.ShowError("Scale not found. If you just plugged it in or just started ShipWorks, it may have yet to be detected.");
                return false;
            }

            ScaleReadResult scaleResult = scaleReaderTask.Result;

            if (scaleResult.Status != ScaleReadStatus.Success)
            {
                log.Error($"Error from scale: {scaleResult.Message}");
                CollectTelemetryData(trackedDurationEvent, "No");
                messageHelper.ShowError(scaleResult.Message);
                return false;
            }

            if (scaleResult.Weight < MinimumWeight)
            {
                log.Info($"Weight not used because under weightMinimum = {scaleResult.Weight}");
                CollectTelemetryData(trackedDurationEvent, "No");
                return true;
            }

            foreach (ShipmentEntity shipment in shipments)
            {
                ICarrierShipmentAdapter shipmentAdapter = shipmentAdapterFactory.Get(shipment);

                foreach (IPackageAdapter packageAdapter in shipmentAdapter.GetPackageAdaptersAndEnsureShipmentIsLoaded())
                {
                    if (Math.Round(Math.Abs(scaleResult.Weight - packageAdapter.Weight), 6) > WeightDifferenceToIgnore ||
                        packageAdapter.Weight < MinimumWeight)
                    {
                        log.Debug($"{scaleResult.Weight} lbs was applied to package {packageAdapter.PackageId} in shipment {shipment.ShipmentID}");
                        packageAdapter.Weight = scaleResult.Weight;
                        
                        if(scaleResult.HasVolumeDimensions)
                        {
                            packageAdapter.DimsLength = scaleResult.Length;
                            packageAdapter.DimsWidth = scaleResult.Width;
                            packageAdapter.DimsHeight = scaleResult.Height;
                        }
                    }
                }

                shipmentAdapter.UpdateDynamicData();
            }

            CollectTelemetryData(trackedDurationEvent, "Yes");
            return true;
        }

        /// <summary>
        /// Collects the telemetry data.
        /// </summary>
        private static void CollectTelemetryData(ITrackedEvent trackedDurationEvent, string message)
        {
            trackedDurationEvent?.AddProperty(TelemetryPropertyName, message);
        }
    }
}