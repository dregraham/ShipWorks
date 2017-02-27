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
using ShipWorks.Users;

namespace ShipWorks.SingleScan
{
    [Component]
    public class AutoWeighService : IAutoWeighService
    {
        private readonly IAutoPrintPermissions autoPrintPermissions;
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;
        private readonly IMessageHelper messageHelper;
        private readonly IScaleReader scaleReader;
        private readonly ILog log;

        // .1 oz
        private const double weightDifferenceToIgnore = .00625; 

        // .05 oz
        private const double weightMinimum = 0.003125;
        public const string TelemetryPropertyName = "SingleScan.AutoPrint.ShipmentsProcessed.AutoWeigh";

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoWeighService"/> class.
        /// </summary>
        public AutoWeighService(IAutoPrintPermissions autoPrintPermissions,
            ICarrierShipmentAdapterFactory shipmentAdapterFactory,
            IMessageHelper messageHelper,
            IScaleReader scaleReader,
            Func<Type, ILog> logFactory)
        {
            this.autoPrintPermissions = autoPrintPermissions;
            this.shipmentAdapterFactory = shipmentAdapterFactory;
            this.messageHelper = messageHelper;
            this.scaleReader = scaleReader;
            this.log = logFactory(typeof(AutoWeighService));
        }

        /// <summary>
        /// Applies the weight on the scale to the specified shipments
        /// </summary>
        public async Task<bool> ApplyWeights(IEnumerable<ShipmentEntity> shipments, ITrackedDurationEvent trackedDurationEvent)
        {
            if (!autoPrintPermissions.AutoWeighOn())
            {
                log.Debug("AutoWeigh is turned off");
                CollectTelemetryData(trackedDurationEvent, "N/A");
                return true;
            }

            ScaleReadResult weighResult = await scaleReader.ReadScale();
            if (weighResult.Status != ScaleReadStatus.Success)
            {
                log.Info($"Error from scale: {weighResult.Message}");
                CollectTelemetryData(trackedDurationEvent, "No");
                messageHelper.ShowError(weighResult.Message);
                return false;
            }

            if (weighResult.Weight < weightMinimum)
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
                    if (Math.Round(Math.Abs(weighResult.Weight - packageAdapter.Weight), 6) > weightDifferenceToIgnore || 
                        packageAdapter.Weight < weightMinimum)
                    {
                        log.Debug(
                            $"Weight applied to package {packageAdapter.PackageId} in shipment {shipment.ShipmentID}");
                        packageAdapter.Weight = weighResult.Weight;
                    }
                }
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