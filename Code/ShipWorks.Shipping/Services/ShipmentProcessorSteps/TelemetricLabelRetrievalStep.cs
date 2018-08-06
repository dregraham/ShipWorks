using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps
{
    /// <summary>
    /// Decorator for the LabelRetrievalStep that collects telemetry
    /// </summary>
    public class TelemetricLabelRetrievalStep : ILabelRetrievalStep
    {
        private readonly ILabelRetrievalStep labelRetrievalStep;
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public TelemetricLabelRetrievalStep(ILabelRetrievalStep labelRetrievalStep, ICarrierShipmentAdapterFactory shipmentAdapterFactory)
        {
            this.labelRetrievalStep = labelRetrievalStep;
            this.shipmentAdapterFactory = shipmentAdapterFactory;
        }

        /// <summary>
        /// Get a label for a shipment and collect telemetry
        /// </summary>
        public async Task<ILabelRetrievalResult> GetLabel(IShipmentPreparationResult result)
        {
            ILabelRetrievalResult labelRetrievalResult = await labelRetrievalStep.GetLabel(result);
            using (TrackedDurationEvent telemetryEvent = new TrackedDurationEvent("Shipping.Label.Creation"))
            {
                SetTelemetryProperties(telemetryEvent, labelRetrievalResult);
            }

            return labelRetrievalResult;
        }

        /// <summary>
        /// Set the label generation telemetry properties
        /// </summary>
        private void SetTelemetryProperties(TrackedDurationEvent telemetryEvent, ILabelRetrievalResult labelResult)
        {
            ShipmentEntity shipment = labelResult.OriginalShipment;
            
            ICarrierShipmentAdapter shipmentAdapter = shipmentAdapterFactory.Get(shipment);

            // Add carrier specific properties
            labelResult.Telemetry?.WriteTo(telemetryEvent);
            
            // Add label/shipment properties
            telemetryEvent.AddProperty("Label.Creation.IsSuccessful", labelResult.Success.ToString());
            telemetryEvent.AddProperty("Carrier.Name", GetCarrierName(shipment));
            telemetryEvent.AddProperty("Carrier.ServiceName", shipmentAdapter.ServiceTypeName);
            telemetryEvent.AddProperty("Label.IsDomestic", shipmentAdapter.IsDomestic.ToString());
            telemetryEvent.AddProperty("Label.ShipDate", shipmentAdapter.ShipDate.ToShortDateString());
            telemetryEvent.AddMetric("Shipment.WeightInPounds", shipmentAdapter.TotalWeight);
            SetPackageTelemetryProperties(telemetryEvent, shipmentAdapter.GetPackageAdapters());
            telemetryEvent.AddProperty("StoreType", EnumHelper.GetDescription(shipmentAdapter.Store.StoreTypeCode));
            telemetryEvent.AddProperty("Label.IsReturn", shipment.ReturnShipment.ToString());
        }

        /// <summary>
        /// Gets the carrier name
        /// </summary>
        private static string GetCarrierName(ShipmentEntity shipment)
        {
            string carrierName = EnumHelper.GetDescription(shipment.ShipmentTypeCode);
            if (shipment.ShipmentTypeCode == ShipmentTypeCode.Other)
            {
                carrierName = $"{carrierName} - {shipment.Other.Carrier}";
            }

            return carrierName;
        }

        /// <summary>
        /// Add package properties to telemetry event
        /// </summary>
        private void SetPackageTelemetryProperties(TrackedDurationEvent telemetryEvent, IEnumerable<IPackageAdapter> packageAdapters)
        {
            IPackageAdapter[] packages = packageAdapters as IPackageAdapter[] ?? packageAdapters.ToArray();
            telemetryEvent.AddMetric("Label.Package.Count", packages.Length);
            
            for (int i=0;i<packages.Length;i++)
            {
                string dimensions = $"{packages[i].DimsLength}x{packages[i].DimsWidth}x{packages[i].DimsHeight}";
                telemetryEvent.AddProperty($"Label.Package.{i}.Dimensions", dimensions);
            }
        }
    }
}