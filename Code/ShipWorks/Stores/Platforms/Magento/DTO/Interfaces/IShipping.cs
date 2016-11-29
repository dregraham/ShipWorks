namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface IShipping
    {
        IShippingAddress Address { get; set; }
        string Method { get; set; }
        ITotal Total { get; set; }
    }
}