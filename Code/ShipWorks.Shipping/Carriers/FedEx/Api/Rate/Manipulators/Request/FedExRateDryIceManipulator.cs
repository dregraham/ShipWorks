using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// Add Dry Ice information to shipment
    /// </summary>
    public class FedExRateDryIceManipulator : IFedExRateRequestManipulator
    {
        // This isn't used and is always zero. Ideally, we should remove this and actually handle
        // the situation where the second package has dry ice when the first doesn't.
        const int currentPackage = 0;

        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options) =>
            shipment.FedEx.Packages.ElementAt(currentPackage).DryIceWeight > 0;

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            Validate(shipment.FedEx);

            InitializeShipmentRequest(request);

            ConfigurePackage(shipment.FedEx,
                request.RequestedShipment.RequestedPackageLineItems[currentPackage].SpecialServicesRequested);

            return request;
        }

        /// <summary>
        /// Validates the specified request.
        /// </summary>
        private static void Validate(IFedExShipmentEntity shipment)
        {
            if (shipment.PackagingType != (int) FedExPackagingType.Custom &&
                shipment.Packages.Any(p => p.DryIceWeight > 0))
            {
                throw new FedExException("Standard FedEx packaging cannot be used to ship packages containing dry ice. You must use your own packaging for shipments containing dry ice.");
            }
        }

        /// <summary>
        /// Configures dry ice properties at the package level.
        /// </summary>
        private void ConfigurePackage(IFedExShipmentEntity shipment, PackageSpecialServicesRequested specialServices)
        {
            specialServices.SpecialServiceTypes =
                specialServices.SpecialServiceTypes.Append(PackageSpecialServiceType.DRY_ICE).ToArray();

            specialServices.DryIceWeight = new Weight
            {
                Value = (decimal) (shipment.Packages.ElementAt(currentPackage).DryIceWeight / 2.2046),
                ValueSpecified = true,
                Units = WeightUnits.KG,
                UnitsSpecified = true
            };
        }

        /// <summary>
        /// Initialize the request
        /// </summary>
        private void InitializeShipmentRequest(RateRequest request)
        {
            request.Ensure(x => x.RequestedShipment)
                .EnsureAtLeastOne(x => x.RequestedPackageLineItems)
                .Ensure(x => x.SpecialServicesRequested)
                .Ensure(x => x.SpecialServiceTypes);
        }
    }
}
