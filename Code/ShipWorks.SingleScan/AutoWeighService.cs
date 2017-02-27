using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.IO.Hardware.Scales;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.SingleScan
{
    [Component]
    public class AutoWeighService : IAutoWeighService
    {
        private readonly IAutoPrintSettings autoPrintSettings;
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;
        private readonly IMessageHelper messageHelper;
        private readonly IScaleReader scaleReader;
        private readonly ILog log;

        // .1 oz
        private const double WeightDifferenceToIgnore = .00625;

        // .05 oz
        private const double MinimumWeight = 0.003125;

        private const int ScaleTimeoutInSeconds = 2;

        public const string TelemetryPropertyName = "SingleScan.AutoPrint.ShipmentsProcessed.AutoWeigh";

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoWeighService"/> class.
        /// </summary>
        public AutoWeighService(IAutoPrintSettings autoPrintSettings,
            ICarrierShipmentAdapterFactory shipmentAdapterFactory,
            IMessageHelper messageHelper,
            IScaleReader scaleReader,
            Func<Type, ILog> logFactory)
        {
            this.autoPrintSettings = autoPrintSettings;
            this.shipmentAdapterFactory = shipmentAdapterFactory;
            this.messageHelper = messageHelper;
            this.scaleReader = scaleReader;
            this.log = logFactory(typeof(AutoWeighService));
        }

        /// <summary>
        /// Applies the weight on the scale to the specified shipments
        /// </summary>
        public bool ApplyWeight(IEnumerable<ShipmentEntity> shipments, ITrackedDurationEvent trackedDurationEvent)
        {
            if (!autoPrintSettings.IsAutoWeighEnabled())
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

            ScaleReadResult weighResult = scaleReaderTask.Result;

            if (weighResult.Status != ScaleReadStatus.Success)
            {
                log.Error($"Error from scale: {weighResult.Message}");
                CollectTelemetryData(trackedDurationEvent, "No");
                messageHelper.ShowError(weighResult.Message);
                return false;
            }

            if (weighResult.Weight < MinimumWeight)
            {
                log.Info($"Weight not used because under weightMinimum = {weighResult.Weight}");
                CollectTelemetryData(trackedDurationEvent, "No");
                return true;
            }

            foreach (ShipmentEntity shipment in shipments)
            {
                ICarrierShipmentAdapter shipmentAdapter = shipmentAdapterFactory.Get(shipment);

                foreach (IPackageAdapter packageAdapter in shipmentAdapter.GetPackageAdapters())
                {
                    if (Math.Round(Math.Abs(weighResult.Weight - packageAdapter.Weight), 6) > WeightDifferenceToIgnore ||
                        packageAdapter.Weight < MinimumWeight)
                    {
                        log.Debug(
                            $"Weight applied to package {packageAdapter.PackageId} in shipment {shipment.ShipmentID}");
                        packageAdapter.Weight = weighResult.Weight;
                    }
                }

                shipmentAdapter.SaveShipment(shipment);
            }

            CollectTelemetryData(trackedDurationEvent, "Yes");
            return true;
        }

        /// <summary>
        /// Collects the telemetry data.
        /// </summary>
        private static void CollectTelemetryData(ITrackedDurationEvent trackedDurationEvent, string message)
        {
            trackedDurationEvent.AddProperty(TelemetryPropertyName, message);
        }
    }
}