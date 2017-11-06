using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExTotalInsuredValueManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExSettingsRepository settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExTotalInsuredValueManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber)
        {
            return (FedExServiceType) shipment.FedEx.Service != FedExServiceType.SmartPost &&
                   shipment.FedEx.Packages.Sum(p => p.DeclaredValue) > 0;
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(request, nameof(request));

            if (!ShouldApply(shipment, sequenceNumber))
            {
                return request;
            }

            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // Just need to assign the weight value in pounds
            request.RequestedShipment.TotalInsuredValue = new Money
            {
                Currency = FedExSettings.GetCurrencyTypeApiValue(shipment, () => settings.GetAccountReadOnly(shipment)),
                Amount = shipment.FedEx.Packages.Sum(p => p.DeclaredValue),
            };

            return request;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private void InitializeRequest(ProcessShipmentRequest request)
        {
            request.Ensure(x => x.RequestedShipment);
        }
    }
}
