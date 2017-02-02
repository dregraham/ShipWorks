using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Factory for creating label services
    /// </summary>
    public interface ILabelServiceFactory : IFactory<ShipmentTypeCode, ILabelService>
    {
    }
}
