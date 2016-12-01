namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface ICustomOption
    {
        IExtensionAttributes ExtensionAttributes { get; set; }
        string OptionId { get; set; }
        string OptionValue { get; set; }
    }
}