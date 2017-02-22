using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Fims
{
    /// <summary>
    /// Carrier response for FIMS
    /// </summary>
    /// <remarks>This is primarily to work with the downloaded data objects</remarks>
    public class FimsCarrierResponse : ICarrierResponse
    {
        private readonly ShipmentEntity shipmentEntity;
        private readonly IFimsShipResponse fimsShipResponse;
        private readonly IFimsLabelRepository labelRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public FimsCarrierResponse(ShipmentEntity shipmentEntity,
            IFimsShipResponse fimsShipResponse,
            IFimsLabelRepository labelRepository)
        {
            this.shipmentEntity = shipmentEntity;
            this.fimsShipResponse = fimsShipResponse;
            this.labelRepository = labelRepository;
        }

        /// <summary>
        /// Gets the native response received from the carrier API.
        /// </summary>
        public object NativeResponse => fimsShipResponse;

        /// <summary>
        /// We don't have a carrier request for FIMS
        /// </summary>
        public CarrierRequest Request => null;

        /// <summary>
        /// Process the response
        /// </summary>
        public void Process()
        {
            shipmentEntity.TrackingNumber = string.IsNullOrEmpty(fimsShipResponse.ParcelID) ? fimsShipResponse.TrackingNumber : fimsShipResponse.ParcelID;
            shipmentEntity.FedEx.Packages[0].TrackingNumber = string.IsNullOrEmpty(fimsShipResponse.ParcelID) ? fimsShipResponse.TrackingNumber : fimsShipResponse.ParcelID;
            shipmentEntity.FedEx.MasterFormID = string.Empty;
            shipmentEntity.ShipmentCost = 0;

            shipmentEntity.RequestedLabelFormat = (int) ThermalLanguage.None;
            shipmentEntity.ActualLabelFormat = fimsShipResponse.LabelFormat == "Z" ? (int?) ThermalLanguage.ZPL : null;
            shipmentEntity.FedEx.RequestedLabelFormat = (int) ThermalLanguage.None;

            labelRepository.SaveLabel(fimsShipResponse, shipmentEntity.FedEx.Packages[0].FedExPackageID);
        }
    }
}
