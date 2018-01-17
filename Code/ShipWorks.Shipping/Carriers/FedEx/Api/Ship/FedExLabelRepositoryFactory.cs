using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship
{
    /// <summary>
    /// Factory for creating label repositories for a FedEx shipment
    /// </summary>
    [Component]
    public class FedExLabelRepositoryFactory : IFedExLabelRepositoryFactory
    {
        private readonly Func<FedExLabelRepository> createBasicRepository;
        private readonly Func<FedExLTLFreightLabelRepository> createLTLFreightRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExLabelRepositoryFactory(
            Func<FedExLabelRepository> createBasicRepository,
            Func<FedExLTLFreightLabelRepository> createLTLFreightRepository)
        {
            this.createLTLFreightRepository = createLTLFreightRepository;
            this.createBasicRepository = createBasicRepository;
        }

        /// <summary>
        /// Create a label repository for a FedEx shipment
        /// </summary>
        public IFedExLabelRepository Create(IShipmentEntity shipment) =>
            FedExUtility.IsFreightLtlService(shipment.FedEx.Service) ?
                (IFedExLabelRepository) createLTLFreightRepository() :
                createBasicRepository();
    }
}
