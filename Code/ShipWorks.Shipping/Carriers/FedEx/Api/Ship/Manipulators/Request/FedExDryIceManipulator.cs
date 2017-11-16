using System.Linq;
using System.Reactive;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// Add Dry Ice information to shipment
    /// </summary>
    public class FedExDryIceManipulator : IFedExShipRequestManipulator
    {
        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) =>
            shipment.FedEx.Packages.Any(x => x.DryIceWeight > 0);

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            return Validate(shipment)
                .Map(() => ApplyDryIce(shipment, request, sequenceNumber));
        }

        /// <summary>
        /// Apply the dry ice details
        /// </summary>
        private static ProcessShipmentRequest ApplyDryIce(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            var package = shipment.FedEx.Packages.ElementAt(sequenceNumber);

            if (package.DryIceWeight > 0)
            {
                if (FedExUtility.IsFreightLtlService(shipment.FedEx.Service))
                {
                    ConfigureShipment(package, request, sequenceNumber);
                }
                else
                {
                    ConfigurePackage(package, request, sequenceNumber);
                }
            }

            return request;
        }

        /// <summary>
        /// Validates the specified request.
        /// </summary>
        private static Result Validate(IShipmentEntity shipment)
        {
            if (shipment.FedEx.PackagingType != (int) FedExPackagingType.Custom)
            {
                return new FedExException("Standard FedEx packaging cannot be used to ship packages containing dry ice. You must use your own packaging for shipments containing dry ice.");
            }

            return Unit.Default;
        }

        /// <summary>
        /// Configures dry ice properties at the shipment level
        /// </summary>
        private static void ConfigureShipment(IFedExPackageEntity package, ProcessShipmentRequest request, int sequenceNumber)
        {
            var specialServices = request.Ensure(x => x.RequestedShipment)
                .Ensure(x => x.SpecialServicesRequested);
            specialServices.SpecialServiceTypes = specialServices.Ensure(x => x.SpecialServiceTypes)
                .Append(ShipmentSpecialServiceType.DRY_ICE)
                .ToArray();

            request.RequestedShipment.SpecialServicesRequested.ShipmentDryIceDetail = new ShipmentDryIceDetail
            {
                PackageCount = "1",
                TotalWeight = new Weight
                {
                    Value = (decimal) (package.DryIceWeight / 2.2046),
                    Units = WeightUnits.KG
                }
            };
        }

        /// <summary>
        /// Configures dry ice properties at the package level.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="nativeRequest">The native request.</param>
        /// <param name="currentPackageIndex">Index of the current package.</param>
        private static void ConfigurePackage(IFedExPackageEntity package, ProcessShipmentRequest request, int currentPackageIndex)
        {
            var specialServices = request.Ensure(x => x.RequestedShipment)
                .EnsureAtLeastOne(x => x.RequestedPackageLineItems)
                .Ensure(x => x.SpecialServicesRequested);
            specialServices.SpecialServiceTypes = specialServices.Ensure(x => x.SpecialServiceTypes)
                .Append(PackageSpecialServiceType.DRY_ICE)
                .ToArray();

            request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DryIceWeight = new Weight
            {
                Value = (decimal) (package.DryIceWeight / 2.2046),
                Units = WeightUnits.KG
            };
        }
    }
}
