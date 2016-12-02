namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface ICustomAttribute
    {
        string AttributeCode { get; set; }
        object Value { get; set; }
    }
}